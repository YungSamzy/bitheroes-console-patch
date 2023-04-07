using System;
using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.application;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class LogManager : BaseManager<LogManager>
{
	private const int MAX_BREADCRUMBS = 10;

	private const int MAX_BREADCRUMB_SIZE = 60;

	private const int MAX_CONNECTIONS = 5;

	private const int MAX_ERROR_LOGS_WITHIN_INTERVAL = 5;

	private const int MAX_ERROR_THROTTLES = 5;

	private const float MAX_ERROR_LOGS_INTERVAL = 30f;

	public const string NOT_AVAILABLE = "N/A";

	public const string BREADCRUMB_PREFIX = "breadcrumb: ";

	private Queue<string> m_breadcrumbs = new Queue<string>(10);

	private string m_browserAgent;

	private float m_maxIntervalCheck;

	private bool m_throttled;

	private int m_errorIntervalCount;

	private int m_errorThrottleCount;

	public bool initialized { get; private set; }

	public JSONNode config { get; private set; }

	public string url { get; private set; }

	public int numConnections { get; private set; }

	private void Awake()
	{
		SetDontDestroyOnLoad();
		Application.logMessageReceived += OnLogMessageReceived;
	}

	private void Update()
	{
		if (m_maxIntervalCheck + 30f < Time.realtimeSinceStartup)
		{
			m_errorIntervalCount = 0;
			m_maxIntervalCheck = Time.realtimeSinceStartup;
			if (m_throttled)
			{
				m_errorThrottleCount++;
				m_throttled = false;
			}
		}
	}

	private void OnDestroy()
	{
		Application.logMessageReceived -= OnLogMessageReceived;
	}

	public void Initialize()
	{
		if (initialized)
		{
			return;
		}
		config = SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("loggly");
		if (config != null)
		{
			string text = config["token"];
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = config["tag"];
				if (!string.IsNullOrEmpty(text2))
				{
					url = "https://logs-01.loggly.com/inputs/" + text + "/tag/" + text2 + "/";
				}
				else
				{
					Debug.LogWarning("Loggly tag is null. Remote logging will be disabled.");
				}
			}
			else
			{
				Debug.LogWarning("Loggly token is null. Remote logging will be disabled.");
			}
		}
		else
		{
			Debug.LogWarning("Loggly config is missing. Remote logging will be disabled.");
		}
		initialized = true;
		SetDirty();
	}

	public IEnumerator Reset()
	{
		initialized = false;
		m_maxIntervalCheck = Time.realtimeSinceStartup;
		m_errorIntervalCount = 0;
		m_errorThrottleCount = 0;
		m_maxIntervalCheck = 0f;
		Initialize();
		yield break;
	}

	public void AddBreadcrumb(string breadcrumb, bool capBreadCrumbLength = true)
	{
		if (capBreadCrumbLength && breadcrumb.Length > 60)
		{
			breadcrumb = breadcrumb.Substring(0, 60);
		}
		if (m_breadcrumbs.Count >= 10)
		{
			m_breadcrumbs.Dequeue();
		}
		m_breadcrumbs.Enqueue(breadcrumb);
		SingletonMonoBehaviour<CrashlyticsManager>.instance.Log("breadcrumb: " + breadcrumb);
	}

	public string GetBreadcrumbs()
	{
		string[] breadcrumbsArray = GetBreadcrumbsArray();
		return string.Join(",", breadcrumbsArray);
	}

	public string[] GetBreadcrumbsArray()
	{
		string[] array = m_breadcrumbs.ToArray();
		Array.Reverse((Array)array);
		return array;
	}

	public void SetBrowserAgent(string agent)
	{
		Debug.Log("Browser Agent: " + agent);
		m_browserAgent = agent;
	}

	public void SendLog(string message, string stackTrace = "", LogType type = LogType.Log, Dictionary<string, string> extraFields = null)
	{
		StartCoroutine(SendLogInternal(message, stackTrace, type, extraFields));
	}

	private IEnumerator SendLogInternal(string message, string stackTrace = "", LogType type = LogType.Log, Dictionary<string, string> extraFields = null)
	{
		if (!initialized || string.IsNullOrEmpty(url) || m_errorThrottleCount >= 5)
		{
			yield break;
		}
		if (numConnections < 5 && m_errorIntervalCount < 5)
		{
			m_errorIntervalCount++;
			numConnections++;
			SetDirty();
			Dictionary<string, string> fields = new Dictionary<string, string>
			{
				{ "Message", message },
				{ "StackTrace", stackTrace },
				{
					"Type",
					type.ToString()
				},
				{
					"Breadcrumbs",
					GetBreadcrumbs()
				},
				{
					"Conns",
					numConnections.ToString()
				},
				{
					"DeviceModel",
					SystemInfo.deviceModel
				},
				{
					"OS",
					SystemInfo.operatingSystem
				},
				{
					"Platform",
					SystemInfoHelper.platform.ToString()
				},
				{
					"Version",
					SystemInfoHelper.bundleVersion
				},
				{
					"BuildNumber",
					UnityCloudBuildManifest.GetLocalManifest().buildNumber
				},
				{
					"BuildTarget",
					UnityCloudBuildManifest.GetLocalManifest().cloudBuildTargetName
				},
				{
					"BuildProject",
					UnityCloudBuildManifest.GetLocalManifest().projectId
				},
				{
					"DLCVersion",
					SingletonMonoBehaviour<AssetBundleManager>.instance.cachedDLCVersion
				},
				{
					"timeSinceLoad",
					Time.time.ToString()
				}
			};
			if (PlatformUtil.isWeb)
			{
				fields.Add("Agent", m_browserAgent);
			}
			AppInfo.AddLoggingKeys(ref fields);
			if (extraFields != null)
			{
				foreach (KeyValuePair<string, string> extraField in extraFields)
				{
					if (!fields.ContainsKey(extraField.Key))
					{
						fields.Add(extraField.Key, extraField.Value);
					}
					else
					{
						Debug.LogWarning("Log field already exists " + extraField.Key);
					}
				}
			}
			WWWForm wWWForm = new WWWForm();
			foreach (KeyValuePair<string, string> item in fields)
			{
				if (item.Key != null && item.Value != null)
				{
					wWWForm.AddField(item.Key, item.Value);
				}
			}
			UnityWebRequest request = UnityWebRequest.Post(url, wWWForm);
			yield return request.SendWebRequest();
			while (!request.isDone)
			{
				yield return null;
			}
			numConnections--;
			SetDirty();
		}
		else
		{
			m_throttled = true;
		}
	}

	public void DumpObjectRemote(object obj, Dictionary<string, string> extraFields = null)
	{
		string message = JsonUtil.SerializeObject(obj);
		SendLog(message, "", LogType.Log, extraFields);
	}

	private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Error || type == LogType.Exception)
		{
			SendLog(condition, stackTrace, type);
		}
	}
}
