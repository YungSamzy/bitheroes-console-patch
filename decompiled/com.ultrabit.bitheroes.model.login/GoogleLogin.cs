using System.Collections.Generic;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.login;

public class GoogleLogin : PlatformLogin
{
	public override void Login(UnityAction<float> onLoginCompleted, UnityAction<float> onLoginFailed)
	{
		base.Login(onLoginCompleted, onLoginFailed);
		networks = new List<int>();
		networks.Add(4);
		networks.Add(GetPlatformSpecificNetwork());
		networks.Add(-1);
	}

	public override string GetUserID()
	{
		return null;
	}

	public override string GetUserToken()
	{
		return null;
	}

	protected override string GetPlatformSpecificUsername()
	{
		return null;
	}

	protected override string GetPlatformSpecificUserToken()
	{
		string platformSpecificUserID = GetPlatformSpecificUserID();
		if (platformSpecificUserID != null)
		{
			return platformSpecificUserID;
		}
		return "000";
	}

	public override bool PlatformAllowGuestLogin()
	{
		return true;
	}

	public override bool ForceCharacterCreation()
	{
		return GetNetwork() == -1;
	}

	public override string GetPlatformSpecificUserID()
	{
		return null;
	}

	public override int GetPlatformSpecificNetwork()
	{
		return 1;
	}

	public bool IsPlayServicesAvailable()
	{
		return false;
	}
}
