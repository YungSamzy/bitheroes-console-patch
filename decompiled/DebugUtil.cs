using UnityEngine;

public static class DebugUtil
{
	public static void LogComponents(GameObject go)
	{
		if (go != null)
		{
			int num = 1;
			string text = go.GetType().Name + ": " + go.name;
			Component[] components = go.GetComponents<Component>();
			foreach (Component component in components)
			{
				text = text + "\n" + num + ": " + component.GetType().Name;
				num++;
			}
			Debug.Log(text);
		}
		else
		{
			Debug.LogWarning("GameObject is null");
		}
	}

	public static void LogObject(object obj)
	{
		Debug.Log(JsonUtil.SerializeObjectIndented(obj));
	}
}
