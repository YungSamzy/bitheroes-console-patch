using System;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceEventsDispatcher : MonoBehaviour
{
	private static IronSourceEventsDispatcher instance = null;

	private static readonly Queue<Action> ironSourceExecuteOnMainThreadQueue = new Queue<Action>();

	public static void executeAction(Action action)
	{
		lock (ironSourceExecuteOnMainThreadQueue)
		{
			ironSourceExecuteOnMainThreadQueue.Enqueue(action);
		}
	}

	private void Update()
	{
		while (ironSourceExecuteOnMainThreadQueue.Count > 0)
		{
			Action action = null;
			lock (ironSourceExecuteOnMainThreadQueue)
			{
				try
				{
					action = ironSourceExecuteOnMainThreadQueue.Dequeue();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			action?.Invoke();
		}
	}

	public void removeFromParent()
	{
		if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public static void initialize()
	{
		if (!isCreated())
		{
			GameObject obj = new GameObject("IronSourceEventsDispatcher")
			{
				hideFlags = HideFlags.HideAndDontSave
			};
			UnityEngine.Object.DontDestroyOnLoad(obj);
			instance = obj.AddComponent<IronSourceEventsDispatcher>();
		}
	}

	public static bool isCreated()
	{
		return instance != null;
	}

	public void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void OnDisable()
	{
		instance = null;
	}
}
