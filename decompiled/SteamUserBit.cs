using System;
using com.ultrabit.bitheroes.messenger;
using Sfs2X.Util;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

public class SteamUserBit : Messenger
{
	private uint mAppId;

	private uint mAuthHandle;

	private byte[] mAuthTicket;

	private Vector2 m_ScrollPos;

	private byte[] m_Ticket;

	private uint m_pcbTicket;

	private HAuthTicket m_HAuthTicket;

	private GameObject m_VoiceLoopback;

	private string mUserId;

	private UnityAction<ByteArray> mAuthTicketCallback;

	protected Callback<SteamServersConnected_t> m_SteamServersConnected;

	protected Callback<SteamServerConnectFailure_t> m_SteamServerConnectFailure;

	protected Callback<SteamServersDisconnected_t> m_SteamServersDisconnected;

	protected Callback<ClientGameServerDeny_t> m_ClientGameServerDeny;

	protected Callback<IPCFailure_t> m_IPCFailure;

	protected Callback<LicensesUpdated_t> m_LicensesUpdated;

	protected Callback<ValidateAuthTicketResponse_t> m_ValidateAuthTicketResponse;

	protected Callback<MicroTxnAuthorizationResponse_t> m_MicroTxnAuthorizationResponse;

	protected Callback<GetAuthSessionTicketResponse_t> m_GetAuthSessionTicketResponse;

	protected Callback<GameWebCallback_t> m_GameWebCallback;

	private CallResult<EncryptedAppTicketResponse_t> OnEncryptedAppTicketResponseCallResult;

	private CallResult<StoreAuthURLResponse_t> OnStoreAuthURLResponseCallResult;

	private CallResult<MarketEligibilityResponse_t> OnMarketEligibilityResponseCallResult;

	private CallResult<DurationControl_t> OnDurationControlCallResult;

	private string _steamID;

	public string SteamID
	{
		get
		{
			return _steamID;
		}
		set
		{
			_steamID = value;
		}
	}

	public void OnEnable()
	{
		m_SteamServersConnected = Callback<SteamServersConnected_t>.Create(OnSteamServersConnected);
		m_SteamServerConnectFailure = Callback<SteamServerConnectFailure_t>.Create(OnSteamServerConnectFailure);
		m_SteamServersDisconnected = Callback<SteamServersDisconnected_t>.Create(OnSteamServersDisconnected);
		m_ClientGameServerDeny = Callback<ClientGameServerDeny_t>.Create(OnClientGameServerDeny);
		m_IPCFailure = Callback<IPCFailure_t>.Create(OnIPCFailure);
		m_LicensesUpdated = Callback<LicensesUpdated_t>.Create(OnLicensesUpdated);
		m_ValidateAuthTicketResponse = Callback<ValidateAuthTicketResponse_t>.Create(OnValidateAuthTicketResponse);
		m_MicroTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(OnMicroTxnAuthorizationResponse);
		m_GetAuthSessionTicketResponse = Callback<GetAuthSessionTicketResponse_t>.Create(OnGetAuthSessionTicketResponse);
		m_GameWebCallback = Callback<GameWebCallback_t>.Create(OnGameWebCallback);
		OnEncryptedAppTicketResponseCallResult = CallResult<EncryptedAppTicketResponse_t>.Create(OnEncryptedAppTicketResponse);
		OnStoreAuthURLResponseCallResult = CallResult<StoreAuthURLResponse_t>.Create(OnStoreAuthURLResponse);
		OnMarketEligibilityResponseCallResult = CallResult<MarketEligibilityResponse_t>.Create(OnMarketEligibilityResponse);
		OnDurationControlCallResult = CallResult<DurationControl_t>.Create(OnDurationControl);
	}

	private void OnSteamServersConnected(SteamServersConnected_t pCallback)
	{
		Debug.Log("[" + 101 + " - SteamServersConnected]");
	}

	private void OnSteamServerConnectFailure(SteamServerConnectFailure_t pCallback)
	{
		Debug.Log("[" + 102 + " - SteamServerConnectFailure] - " + pCallback.m_eResult.ToString() + " -- " + pCallback.m_bStillRetrying);
	}

	private void OnSteamServersDisconnected(SteamServersDisconnected_t pCallback)
	{
		Debug.Log("[" + 103 + " - SteamServersDisconnected] - " + pCallback.m_eResult);
	}

	private void OnClientGameServerDeny(ClientGameServerDeny_t pCallback)
	{
		Debug.Log("[" + 113 + " - ClientGameServerDeny] - " + pCallback.m_uAppID + " -- " + pCallback.m_unGameServerIP + " -- " + pCallback.m_usGameServerPort + " -- " + pCallback.m_bSecure + " -- " + pCallback.m_uReason);
	}

	private void OnIPCFailure(IPCFailure_t pCallback)
	{
		Debug.Log("[" + 117 + " - IPCFailure] - " + pCallback.m_eFailureType);
	}

	private void OnLicensesUpdated(LicensesUpdated_t pCallback)
	{
		Debug.Log("[" + 125 + " - LicensesUpdated]");
	}

	private void OnValidateAuthTicketResponse(ValidateAuthTicketResponse_t pCallback)
	{
		string[] obj = new string[8]
		{
			"[",
			143.ToString(),
			" - ValidateAuthTicketResponse] - ",
			null,
			null,
			null,
			null,
			null
		};
		CSteamID steamID = pCallback.m_SteamID;
		obj[3] = steamID.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_eAuthSessionResponse.ToString();
		obj[6] = " -- ";
		steamID = pCallback.m_OwnerSteamID;
		obj[7] = steamID.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t pCallback)
	{
		Debug.Log("[" + 152 + " - MicroTxnAuthorizationResponse] - " + pCallback.m_unAppID + " -- " + pCallback.m_ulOrderID + " -- " + pCallback.m_bAuthorized);
	}

	private void OnEncryptedAppTicketResponse(EncryptedAppTicketResponse_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 154 + " - EncryptedAppTicketResponse] - " + pCallback.m_eResult);
		if (pCallback.m_eResult != EResult.k_EResultOK)
		{
			return;
		}
		byte[] array = new byte[1024];
		SteamUser.GetEncryptedAppTicket(array, 1024, out var pcbTicket);
		byte[] array2 = new byte[32]
		{
			237, 147, 134, 7, 54, 71, 206, 165, 139, 119,
			33, 73, 13, 89, 237, 68, 87, 35, 240, 246,
			110, 116, 20, 225, 83, 59, 163, 60, 216, 3,
			189, 189
		};
		byte[] rgubTicketDecrypted = new byte[1024];
		uint pcubTicketDecrypted = 1024u;
		if (!SteamEncryptedAppTicket.BDecryptTicket(array, pcbTicket, rgubTicketDecrypted, ref pcubTicketDecrypted, array2, array2.Length))
		{
			Debug.Log("Ticket failed to decrypt");
			return;
		}
		if (!SteamEncryptedAppTicket.BIsTicketForApp(rgubTicketDecrypted, pcubTicketDecrypted, SteamUtils.GetAppID()))
		{
			Debug.Log("Ticket for wrong app id");
		}
		SteamEncryptedAppTicket.GetTicketSteamID(rgubTicketDecrypted, pcubTicketDecrypted, out var psteamID);
		if (psteamID != SteamUser.GetSteamID())
		{
			Debug.Log("Ticket for wrong user");
		}
		uint pcubUserData;
		byte[] userVariableData = SteamEncryptedAppTicket.GetUserVariableData(rgubTicketDecrypted, pcubTicketDecrypted, out pcubUserData);
		if (pcubUserData != 4)
		{
			Debug.Log("Secret data size is wrong.");
		}
		Debug.Log(userVariableData.Length);
		Debug.Log(BitConverter.ToUInt32(userVariableData, 0));
		if (BitConverter.ToUInt32(userVariableData, 0) != 21572)
		{
			Debug.Log("Failed to retrieve secret data");
		}
		else
		{
			Debug.Log("Successfully retrieved Encrypted App Ticket");
		}
	}

	public void getAuthSessionTicket(UnityAction<ByteArray> callback)
	{
		m_Ticket = new byte[1024];
		m_HAuthTicket = SteamUser.GetAuthSessionTicket(m_Ticket, 1024, out m_pcbTicket);
		mAuthTicketCallback = callback;
	}

	private void OnGetAuthSessionTicketResponse(GetAuthSessionTicketResponse_t pCallback)
	{
		string[] obj = new string[6]
		{
			"OnGetAuthSessionTicketResponse [",
			163.ToString(),
			" - GetAuthSessionTicketResponse] - ",
			null,
			null,
			null
		};
		HAuthTicket hAuthTicket = pCallback.m_hAuthTicket;
		obj[3] = hAuthTicket.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_eResult.ToString();
		Debug.Log(string.Concat(obj));
		if (mAuthHandle != 0)
		{
			ByteArray byteArray = new ByteArray();
			byteArray.WriteBytes(mAuthTicket);
			mAuthTicket = null;
			mAuthHandle = 0u;
			if (mAuthTicketCallback != null)
			{
				mAuthTicketCallback(byteArray);
			}
			Broadcast("USER_TOKEN_UPDATE");
		}
	}

	private void OnGameWebCallback(GameWebCallback_t pCallback)
	{
		Debug.Log("[" + 164 + " - GameWebCallback] - " + pCallback.m_szURL);
	}

	private void OnStoreAuthURLResponse(StoreAuthURLResponse_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 165 + " - StoreAuthURLResponse] - " + pCallback.m_szURL);
	}

	private void OnMarketEligibilityResponse(MarketEligibilityResponse_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[12]
		{
			"[",
			166.ToString(),
			" - MarketEligibilityResponse] - ",
			pCallback.m_bAllowed.ToString(),
			" -- ",
			pCallback.m_eNotAllowedReason.ToString(),
			" -- ",
			null,
			null,
			null,
			null,
			null
		};
		RTime32 rtAllowedAtTime = pCallback.m_rtAllowedAtTime;
		obj[7] = rtAllowedAtTime.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_cdaySteamGuardRequiredDays.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_cdayNewDeviceCooldown.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnDurationControl(DurationControl_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[18]
		{
			"[",
			167.ToString(),
			" - DurationControl] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		AppId_t appid = pCallback.m_appid;
		obj[5] = appid.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_bApplicable.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_csecsLast5h.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_progress.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.m_notification.ToString();
		obj[14] = " -- ";
		obj[15] = pCallback.m_csecsToday.ToString();
		obj[16] = " -- ";
		obj[17] = pCallback.m_csecsRemaining.ToString();
		Debug.Log(string.Concat(obj));
	}
}
