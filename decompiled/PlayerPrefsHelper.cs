using UnityEngine;

public static class PlayerPrefsHelper
{
	public static void Save()
	{
		PlayerPrefs.Save();
	}

	private static void TrySave(bool saveImmediately)
	{
		if (saveImmediately)
		{
			Save();
		}
	}

	public static void DeleteKey(string key, bool saveImmediately = true)
	{
		PlayerPrefs.DeleteKey(key);
		TrySave(saveImmediately);
		Debug.Log($"Deleted PlayerPrefs key \"{key}\"!");
	}

	public static void DeleteAll(bool saveImmediately = true)
	{
		PlayerPrefs.DeleteAll();
		TrySave(saveImmediately);
		Debug.Log("Deleted all PlayerPrefs!");
	}

	public static bool GetBool(string key, bool defaultValue = false)
	{
		return GetInt(key, defaultValue ? 1 : 0) == 1;
	}

	public static void SetBool(string key, bool value, bool saveImmediately = true)
	{
		SetInt(key, value ? 1 : 0, saveImmediately);
	}

	public static int GetInt(string key, int defaultValue = 0)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	public static void SetInt(string key, int value, bool saveImmediately = true)
	{
		PlayerPrefs.SetInt(key, value);
		TrySave(saveImmediately);
	}

	public static float GetFloat(string key, float defaultValue = 0f)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}

	public static void SetFloat(string key, float value, bool saveImmediately = true)
	{
		PlayerPrefs.SetFloat(key, value);
		TrySave(saveImmediately);
	}

	public static string GetString(string key, string defaultValue = null)
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	public static void SetString(string key, string value, bool saveImmediately = true)
	{
		PlayerPrefs.SetString(key, value);
		TrySave(saveImmediately);
	}
}
