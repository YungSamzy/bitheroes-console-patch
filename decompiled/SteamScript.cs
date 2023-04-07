using Sfs2X.Util;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

public class SteamScript : MonoBehaviour
{
	private uint mAuthHandle;

	private ByteArray mAuthTicket;

	private UnityAction<ByteArray> mAuthTicketCallback;

	private PublishedFileId_t m_PublishedFileId;

	protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

	private CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers;

	private CallResult<GetAppDependenciesResult_t> OnGetAppDependenciesResultCallResult;

	private CallResult<CreateItemResult_t> OnCreateItemResultCallResult;

	private string _steamID;

	public string steamID
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

	public bool Initialized => SteamManager.Initialized;

	public void Init()
	{
		MonoBehaviour.print("SteamScript INIT");
		if (SteamManager.Initialized)
		{
			_steamID = SteamUser.GetSteamID().m_SteamID.ToString();
		}
	}

	private void OnEnable()
	{
		if (SteamManager.Initialized)
		{
			m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
			m_NumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
			OnGetAppDependenciesResultCallResult = CallResult<GetAppDependenciesResult_t>.Create(OnGetAppDependenciesResult);
			OnCreateItemResultCallResult = CallResult<CreateItemResult_t>.Create(OnCreateItemResult);
		}
	}

	private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
	{
		if (pCallback.m_bActive != 0)
		{
			Debug.Log("Steam Overlay has been activated");
		}
		else
		{
			Debug.Log("Steam Overlay has been closed");
		}
	}

	private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure)
	{
		if (pCallback.m_bSuccess != 1 || bIOFailure)
		{
			Debug.Log("There was an error retrieving the NumberOfCurrentPlayers.");
		}
		else
		{
			Debug.Log("The number of players playing your game: " + pCallback.m_cPlayers);
		}
	}

	private void OnGetAppDependenciesResult(GetAppDependenciesResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[12]
		{
			"[",
			3416.ToString(),
			" - GetAppDependenciesResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_rgAppIDs?.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_nNumAppDependencies.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_nTotalNumAppDependencies.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnCreateItemResult(CreateItemResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			3403.ToString(),
			" - CreateItemResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement.ToString();
		Debug.Log(string.Concat(obj));
		m_PublishedFileId = pCallback.m_nPublishedFileId;
		SteamAPICall_t appDependencies = SteamUGC.GetAppDependencies(m_PublishedFileId);
		OnGetAppDependenciesResultCallResult.Set(appDependencies);
	}

	public void getAuthSessionTicket(UnityAction<ByteArray> callback)
	{
		SteamAPICall_t numberOfCurrentPlayers = SteamUserStats.GetNumberOfCurrentPlayers();
		m_NumberOfCurrentPlayers.Set(numberOfCurrentPlayers);
		Debug.Log("getAuthSessionTicket");
		mAuthTicket = new ByteArray();
		mAuthTicketCallback = callback;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SteamAPICall_t numberOfCurrentPlayers = SteamUserStats.GetNumberOfCurrentPlayers();
			m_NumberOfCurrentPlayers.Set(numberOfCurrentPlayers);
			Debug.Log("Called GetNumberOfCurrentPlayers()");
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			SteamAPICall_t appDependencies = SteamUGC.GetAppDependencies(m_PublishedFileId);
			OnGetAppDependenciesResultCallResult.Set(appDependencies);
			Debug.Log("Called GetNumberOfCurrentPlayers()");
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			SteamAPICall_t steamAPICall_t = SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeFirst);
			OnCreateItemResultCallResult.Set(steamAPICall_t);
			string[] obj = new string[6]
			{
				"SteamUGC.CreateItem(",
				SteamUtils.GetAppID().ToString(),
				", ",
				EWorkshopFileType.k_EWorkshopFileTypeFirst.ToString(),
				") : ",
				null
			};
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
			obj[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj));
		}
	}

	private void OnDestroy()
	{
	}
}
