using System;
using Steamworks;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.login;

public class SteamLogin : PlatformLogin
{
	public override void Login(UnityAction<float> onLoginCompleted, UnityAction<float> onLoginFailed)
	{
		base.Login(onLoginCompleted, onLoginFailed);
		if (SteamManager.Initialized)
		{
			OnLoginCompleted(0.5f);
		}
		else
		{
			OnLoginFailed();
		}
	}

	protected override string GetPlatformSpecificUsername()
	{
		if (SteamManager.Initialized)
		{
			return SteamFriends.GetPersonaName();
		}
		return null;
	}

	protected override string GetPlatformSpecificUserToken()
	{
		if (SteamManager.Initialized)
		{
			byte[] array = new byte[1024];
			SteamUser.GetAuthSessionTicket(array, 1024, out var pcbTicket);
			byte[] array2 = new byte[pcbTicket];
			Array.Copy(array, array2, array2.Length);
			return BitConverter.ToString(array2, 0, array2.Length).Replace("-", string.Empty);
		}
		return base.GetUserID();
	}

	public override bool PlatformAllowGuestLogin()
	{
		return false;
	}

	public override string GetPlatformSpecificUserID()
	{
		if (SteamManager.Initialized)
		{
			return ((long)SteamUser.GetSteamID().m_SteamID).ToString();
		}
		return null;
	}

	public static string GetSteamUserID()
	{
		if (SteamManager.Initialized)
		{
			return ((long)SteamUser.GetSteamID().m_SteamID).ToString();
		}
		return null;
	}

	public override int GetPlatformSpecificNetwork()
	{
		return 7;
	}
}
