using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.login;

public class IOSLogin : PlatformLogin
{
	public override void Login(UnityAction<float> onLoginCompleted, UnityAction<float> onLoginFailed)
	{
		base.Login(onLoginCompleted, onLoginFailed);
		networks = new List<int>();
		networks.Add(4);
		networks.Add(GetPlatformSpecificNetwork());
		networks.Add(-1);
		try
		{
			D.Log("all", "Starting Auth with GameCenter");
			Social.localUser.Authenticate(OnUserLoggedIn);
		}
		catch (Exception ex)
		{
			OnLoginCompleted();
			D.LogException(ex.Message, ex);
		}
	}

	private void OnUserLoggedIn(bool success, string message)
	{
		if (success)
		{
			D.Log("all", "Auth with GameCenter Succeed: " + Social.localUser.id + " -> " + Social.localUser.userName);
		}
		else
		{
			D.Log("all", "Auth with GameCenter Failed: " + message);
		}
		OnLoginCompleted();
	}

	protected override string GetPlatformSpecificUsername()
	{
		if (Social.localUser.authenticated)
		{
			return Social.localUser.userName;
		}
		return null;
	}

	public override string GetUserID()
	{
		return null;
	}

	protected override string GetPlatformSpecificUserToken()
	{
		return GetUserID();
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
		if (Social.localUser.authenticated)
		{
			string id = Social.localUser.id;
			if (!id.Equals("Unavailable Player Identification") && !id.Equals("UnknownID"))
			{
				return id;
			}
		}
		return null;
	}

	public override int GetPlatformSpecificNetwork()
	{
		return 2;
	}
}
