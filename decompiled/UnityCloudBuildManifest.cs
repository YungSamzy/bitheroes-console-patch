using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.CloudBuild;

public class UnityCloudBuildManifest
{
	private static BuildManifestObject buildManifestObject;

	public string scmCommitId = "BuildCommitId";

	public string scmBranch = "LocalBranch";

	public string buildNumber = "0";

	public string buildStartTime;

	public string projectId = "local-build";

	public string bundleId;

	public string unityVersion;

	public string xcodeVersion;

	public string cloudBuildTargetName = "local-target";

	private static UnityCloudBuildManifest m_manifest;

	public static bool TryGetValue<T>(string key, out T result)
	{
		bool result2 = false;
		if (buildManifestObject != null)
		{
			buildManifestObject.TryGetValue<T>(key, out result);
			result2 = true;
		}
		else
		{
			result = default(T);
		}
		return result2;
	}

	public static void InitBuildManifestObject(BuildManifestObject obj)
	{
		buildManifestObject = obj;
	}

	public static UnityCloudBuildManifest GetLocalManifest()
	{
		if (m_manifest == null)
		{
			TextAsset textAsset = (TextAsset)Resources.Load("UnityCloudBuildManifest.json");
			if (textAsset != null)
			{
				Debug.Log("MANIFEST CREATED: " + textAsset.text);
			}
			if ((bool)textAsset)
			{
				m_manifest = JsonConvert.DeserializeObject<UnityCloudBuildManifest>(textAsset.text);
			}
			else
			{
				m_manifest = new UnityCloudBuildManifest();
			}
		}
		return m_manifest;
	}

	public static string GetInfoText()
	{
		string text = "";
		UnityCloudBuildManifest localManifest = GetLocalManifest();
		if (localManifest != null)
		{
			text += SystemInfoHelper.bundleVersion;
			if (localManifest.buildNumber != "")
			{
				text = text + "b" + localManifest.buildNumber;
			}
			return text + " " + SingletonMonoBehaviour<EnvironmentManager>.instance.environmentName;
		}
		return text + "Unable to get build manifest";
	}
}
