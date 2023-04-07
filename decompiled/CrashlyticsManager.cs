using System;

public class CrashlyticsManager : BaseManager<CrashlyticsManager>
{
	private bool firebaseInitialized;

	public void Init()
	{
		firebaseInitialized = false;
	}

	private bool CheckInitialized()
	{
		return firebaseInitialized;
	}

	public void Log(string message)
	{
	}

	public void LogException(Exception ex)
	{
	}

	public void SetKeyValue(string key, string value)
	{
	}

	public void SetUserIdentifier(string identifier)
	{
	}

	public void SetUserName(string name)
	{
	}
}
