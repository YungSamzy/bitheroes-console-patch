using System.Collections.Generic;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.login;

public abstract class PlatformLogin
{
	private string _status = "";

	private bool _success;

	public const string GUEST = "guest";

	protected UnityAction<float> onLoginCompleted;

	protected UnityAction<float> onLoginFailed;

	public string errorMessage;

	public int serverErrorLoginAttempt;

	protected List<int> networks;

	protected int network;

	protected bool loginCancelled;

	public const int NETWORK_ANON = -1;

	public virtual void Login(UnityAction<float> onLoginCompleted, UnityAction<float> onLoginFailed)
	{
		network = 0;
		networks = new List<int>();
		networks.Add(GetPlatformSpecificNetwork());
		loginCancelled = false;
		serverErrorLoginAttempt = 15;
		this.onLoginCompleted = onLoginCompleted;
		this.onLoginFailed = onLoginFailed;
	}

	protected void SetStatus(bool success, string status)
	{
		_success = success;
		_status = status;
	}

	protected void OnLoginCompleted(float delay = 0f)
	{
		if (onLoginCompleted != null && !loginCancelled)
		{
			onLoginCompleted(delay);
		}
	}

	protected void OnLoginFailed(float delay = 0f)
	{
		if (onLoginFailed != null && !loginCancelled)
		{
			onLoginFailed(delay);
		}
	}

	public string GetStatus()
	{
		return _status;
	}

	public bool GetSuccess()
	{
		return _success;
	}

	public virtual string GetUserID()
	{
		return GetPlatformSpecificUserID();
	}

	public virtual string GetUserToken()
	{
		return GetPlatformSpecificUserToken();
	}

	public string GetUserName()
	{
		string platformSpecificUsername = GetPlatformSpecificUsername();
		if (platformSpecificUsername == null)
		{
			return "";
		}
		return platformSpecificUsername;
	}

	public virtual bool ForceCharacterCreation()
	{
		return true;
	}

	public int GetNetwork()
	{
		return networks[network];
	}

	public void SetNetwork(int network)
	{
		this.network = network;
	}

	public int MoveToNextNetwork()
	{
		network++;
		if (network >= networks.Count)
		{
			network = networks.Count - 1;
		}
		return GetNetwork();
	}

	public abstract string GetPlatformSpecificUserID();

	protected abstract string GetPlatformSpecificUserToken();

	protected abstract string GetPlatformSpecificUsername();

	public abstract bool PlatformAllowGuestLogin();

	public abstract int GetPlatformSpecificNetwork();

	public void CancelLogin()
	{
		loginCancelled = true;
	}
}
