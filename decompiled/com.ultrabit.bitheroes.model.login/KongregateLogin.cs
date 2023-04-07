using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.login;

public class KongregateLogin : PlatformLogin
{
	public override void Login(UnityAction<float> onLoginCompleted, UnityAction<float> onLoginFailed)
	{
		base.Login(onLoginCompleted, onLoginFailed);
		if (!AppInfo.kongApiAvailable)
		{
			OnLoginFailed();
			D.LogError($"KongregateLogin - kongApiAvailable: {AppInfo.kongApiAvailable}");
		}
		else if (AppInfo.kongApi.Services.GetUserId() > 0)
		{
			OnLoginCompleted();
		}
		else
		{
			D.LogError($"KongregateLogin - Invalid User ID {AppInfo.kongApi.Services.GetUserId()}");
		}
	}

	protected override string GetPlatformSpecificUsername()
	{
		if (AppInfo.kongApi != null && AppInfo.kongApi.Services != null)
		{
			string username = AppInfo.kongApi.Services.GetUsername();
			if (username != null && username.Length > 0 && !username.ToLowerInvariant().Equals("guest"))
			{
				return username;
			}
		}
		return null;
	}

	protected override string GetPlatformSpecificUserToken()
	{
		return AppInfo.kongToken;
	}

	public override bool PlatformAllowGuestLogin()
	{
		return false;
	}

	public override string GetPlatformSpecificUserID()
	{
		return AppInfo.kongID;
	}

	public override int GetPlatformSpecificNetwork()
	{
		return 4;
	}

	public static void Link()
	{
		if (AppInfo.kongApiAvailable && !AppInfo.DoKongregateLoginCheck())
		{
			KongregateAPI.GetAPI().Mobile.OpenKongregateWindow();
		}
	}
}
