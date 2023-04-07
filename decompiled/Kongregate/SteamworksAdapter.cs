using System;
using Steamworks;
using UnityEngine;

namespace Kongregate;

public class SteamworksAdapter : SteamStandaloneAdapter
{
	private const int MAX_TICKET_SIZE = 1024;

	private byte[] pendingAuthTicket;

	private HAuthTicket authTicketHandle;

	private Callback<GetAuthSessionTicketResponse_t> sessionTicketCallback;

	private AuthSessionTicketDelegate sessionTicketDelegate;

	public override bool Initialized => SteamManager.Initialized;

	public override string SteamID
	{
		get
		{
			if (!Initialized)
			{
				return null;
			}
			return SteamUser.GetSteamID().ToString();
		}
	}

	public override bool OverlayEnabled
	{
		get
		{
			if (!Initialized)
			{
				return false;
			}
			return SteamUtils.IsOverlayEnabled();
		}
	}

	public override string PersonaName
	{
		get
		{
			if (!Initialized)
			{
				return null;
			}
			return SteamFriends.GetPersonaName()?.ToString();
		}
	}

	public SteamworksAdapter()
		: base(null)
	{
		steamAdapter = this;
		sessionTicketCallback = Callback<GetAuthSessionTicketResponse_t>.Create(OnAuthSessionTicketResponse);
		Debug.Log("Kongregate: SteamworksAdapter startup, initialized=" + Initialized);
	}

	public override void ActivateGameOverlayToWebPage(string url)
	{
		if (Initialized)
		{
			SteamFriends.ActivateGameOverlayToWebPage(url);
		}
	}

	public override void GetAuthSessionTicket(AuthSessionTicketDelegate callback)
	{
		if (!Initialized)
		{
			Debug.Log("Kongregate: Ignoring GetAuthSessionTicket call, SteamManager is not initialized");
			return;
		}
		sessionTicketDelegate = callback;
		byte[] array = new byte[1024];
		uint pcbTicket = 0u;
		authTicketHandle = SteamUser.GetAuthSessionTicket(array, 1024, out pcbTicket);
		pendingAuthTicket = new byte[pcbTicket];
		Array.Copy(array, 0L, pendingAuthTicket, 0L, pcbTicket);
		HAuthTicket hAuthTicket = authTicketHandle;
		Debug.Log("Pending AuthTicket handle: " + hAuthTicket.ToString() + ", length: " + pcbTicket);
	}

	private void OnAuthSessionTicketResponse(GetAuthSessionTicketResponse_t response)
	{
		if (response.m_hAuthTicket == authTicketHandle)
		{
			if (sessionTicketDelegate != null)
			{
				Debug.Log("AuthSessionTicket result: " + response.m_eResult.ToString() + ", firing callback");
				sessionTicketDelegate((response.m_eResult == EResult.k_EResultOK) ? pendingAuthTicket : null);
				pendingAuthTicket = null;
				authTicketHandle = default(HAuthTicket);
			}
		}
		else
		{
			HAuthTicket hAuthTicket = response.m_hAuthTicket;
			Debug.Log("Ignoring AuthSessionTicketResponse we did not request: " + hAuthTicket.ToString());
		}
	}
}
