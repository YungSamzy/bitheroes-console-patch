using com.ultrabit.bitheroes.model.application;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.login;

public class PlatformLoginHandler
{
	private static PlatformLoginHandler _instance;

	private string _advertisingID;

	private bool _trackingEnabled;

	private PlatformLogin _currentLogin;

	public static PlatformLoginHandler instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PlatformLoginHandler();
			}
			return _instance;
		}
	}

	public string adIdentifier => _advertisingID;

	public void Init()
	{
		Application.RequestAdvertisingIdentifierAsync(OnIdentifierObtained);
	}

	private void OnIdentifierObtained(string advertisingID, bool trackingEnabled, string error)
	{
		if (advertisingID != null && !advertisingID.Trim().Equals(""))
		{
			_advertisingID = advertisingID;
			_trackingEnabled = trackingEnabled;
		}
	}

	public PlatformLogin GetPlatformLogin(int platform = -1)
	{
		if (platform == -1)
		{
			if (_currentLogin != null)
			{
				return _currentLogin;
			}
			platform = AppInfo.platform;
		}
		switch (platform)
		{
		case 4:
			_currentLogin = new KongregateLogin();
			break;
		case 1:
			_currentLogin = new GoogleLogin();
			break;
		case 2:
			_currentLogin = new IOSLogin();
			break;
		case 7:
			_currentLogin = new SteamLogin();
			break;
		default:
			_currentLogin = null;
			break;
		}
		return _currentLogin;
	}
}
