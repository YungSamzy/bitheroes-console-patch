public class ISAdQualityConfig
{
	private string userId;

	private bool userIdSet;

	private bool testMode;

	private ISAdQualityInitCallback adQualityInitCallback;

	private ISAdQualityLogLevel logLevel;

	public string UserId
	{
		get
		{
			return userId;
		}
		set
		{
			userIdSet = true;
			userId = value;
		}
	}

	internal bool UserIdSet => userIdSet;

	public bool TestMode
	{
		get
		{
			return testMode;
		}
		set
		{
			testMode = value;
		}
	}

	public ISAdQualityLogLevel LogLevel
	{
		get
		{
			return logLevel;
		}
		set
		{
			logLevel = value;
		}
	}

	public ISAdQualityInitCallback AdQualityInitCallback
	{
		get
		{
			return adQualityInitCallback;
		}
		set
		{
			adQualityInitCallback = value;
		}
	}

	public ISAdQualityConfig()
	{
		userId = null;
		testMode = false;
		userIdSet = false;
		logLevel = ISAdQualityLogLevel.INFO;
	}
}
