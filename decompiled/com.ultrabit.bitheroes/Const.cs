namespace com.ultrabit.bitheroes;

public class Const
{
	public class Crashlytics
	{
		public const string FIREBASE_USERNAME = "FirebaseUsername";

		public const string FIREBASE_ID = "FirebaseID";

		public const string USER_ID = "UserID";

		public const string CHAR_ID = "CharID";

		public const string CHAR_NAME = "CharName";

		public const string INIT_FLOW_STEP = "InitFlowStep";
	}

	public class Values
	{
		public const int LOGIN_ATTEMPS = 15;

		public const int IAP_ATTEMPS = 3;
	}

	public class SmartFoxEventParams
	{
		public const string SUCCESS = "success";

		public const string PARAMS = "params";

		public const string CMD = "cmd";
	}

	public class Tags
	{
		public const string DYNAMIC_SPRITE = "DynamicSprite";
	}

	public class Scenes
	{
		public const string SCENE_LOGIN = "Login";

		public const string SCENE_MAIN = "Main";

		public const string SCENE_FISHING = "Fishing";

		public const string SCENE_DUNGEON = "Dungeon";

		public const string SCENE_BATTLE = "Battle";

		public const string SCENE_PAUSED = "Paused";

		public const string SCENE_TOWN = "town";

		public const string SCENE_GUILD = "guild";

		public const string SCENE_ARMORY = "armory";

		public const string SCENE_PLAYER_ARMORY = "player_armory";
	}

	public class Tools
	{
		public const string LAYER_ADJUSTMENT = "LayerAdjuster";

		public const string MAP_LAYER_SUFIX = "Map";

		public const string MAP_FISHING_LAYER_SUFIX = "Fishing";

		public const float OPTIMAL_HEIGHT_RESOLUTION = 480f;

		public const float OPTIMAL_WIDTH_RESOLUTION = 800f;
	}

	public class LayerMasks
	{
		public const string MOUSE = "Mouse";

		public const string UI = "UI";

		public const string OVERUI = "OVERUI";

		public const string DEFAULT = "Default";
	}

	public class CustomSortingLayers
	{
		public const string OVERALL = "Overall";

		public const string BATTLE = "Battle";

		public const string BACKGROUND = "Background";

		public const string UI = "UI";

		public const string DEFAULT = "Default";

		public const string OVERUI = "OverUI";
	}

	public class Steam
	{
		public const int RESPONSE_OnUserStatsReceived = 0;

		public const int RESPONSE_OnUserStatsStored = 1;

		public const int RESPONSE_OnAchievementStored = 2;

		public const int RESPONSE_OnGlobalStatsReceived = 3;

		public const int RESPONSE_OnFindLeaderboard = 4;

		public const int RESPONSE_OnUploadLeaderboardScore = 5;

		public const int RESPONSE_OnDownloadLeaderboardEntries = 6;

		public const int RESPONSE_OnGameOverlayActivated = 7;

		public const int RESPONSE_OnFileShared = 8;

		public const int RESPONSE_OnUGCDownload = 9;

		public const int RESPONSE_OnPublishWorkshopFile = 10;

		public const int RESPONSE_OnDeletePublishedFile = 11;

		public const int RESPONSE_OnGetPublishedFileDetails = 12;

		public const int RESPONSE_OnEnumerateUserPublishedFiles = 13;

		public const int RESPONSE_OnEnumeratePublishedWorkshopFiles = 14;

		public const int RESPONSE_OnEnumerateUserSubscribedFiles = 15;

		public const int RESPONSE_OnEnumerateUserSharedWorkshopFiles = 16;

		public const int RESPONSE_OnEnumeratePublishedFilesByUserAction = 17;

		public const int RESPONSE_OnCommitPublishedFileUpdate = 18;

		public const int RESPONSE_OnSubscribePublishedFile = 19;

		public const int RESPONSE_OnUnsubscribePublishedFile = 20;

		public const int RESPONSE_OnGetPublishedItemVoteDetails = 21;

		public const int RESPONSE_OnGetUserPublishedItemVoteDetails = 22;

		public const int RESPONSE_OnUpdateUserPublishedItemVote = 23;

		public const int RESPONSE_OnSetUserPublishedFileAction = 24;

		public const int RESPONSE_OnGetAuthSessionTicketResponse = 25;

		public const int RESPONSE_OnValidateAuthTicketResponse = 26;

		public const int RESPONSE_OnDLCInstalled = 27;

		public const int RESPONSE_OnMicroTxnAuthorizationResponse = 28;

		public const int RESPONSE_OnEncryptedAppTicketResponse = 29;
	}

	public class UserConstant
	{
		public const int BEGINAUTH_OK = 0;

		public const int BEGINAUTH_InvalidTicket = 1;

		public const int BEGINAUTH_DuplicateRequest = 2;

		public const int BEGINAUTH_InvalidVersion = 3;

		public const int BEGINAUTH_GameMismatch = 4;

		public const int BEGINAUTH_ExpiredTicket = 5;

		public const int LICENSE_HasLicense = 0;

		public const int LICENSE_DoesNotHaveLicense = 1;

		public const int LICENSE_NoAuth = 2;

		public const int SESSION_OK = 0;

		public const int SESSION_UserNotConnectedToSteam = 1;

		public const int SESSION_NoLicenseOrExpired = 2;

		public const int SESSION_VACBanned = 3;

		public const int SESSION_LoggedInElseWhere = 4;

		public const int SESSION_VACCheckTimedOut = 5;

		public const int SESSION_AuthTicketCanceled = 6;

		public const int SESSION_AuthTicketInvalidAlreadyUsed = 7;

		public const int SESSION_AuthTicketInvalid = 8;

		public const uint AUTHTICKET_Invalid = 0u;
	}

	public class SteamResults
	{
		public const int OK = 1;

		public const int Fail = 2;

		public const int NoConnection = 3;

		public const int InvalidPassword = 5;

		public const int LoggedInElsewhere = 6;

		public const int InvalidProtocolVer = 7;

		public const int InvalidParam = 8;

		public const int FileNotFound = 9;

		public const int Busy = 10;

		public const int InvalidState = 11;

		public const int InvalidName = 12;

		public const int InvalidEmail = 13;

		public const int DuplicateName = 14;

		public const int AccessDenied = 15;

		public const int Timeout = 16;

		public const int Banned = 17;

		public const int AccountNotFound = 18;

		public const int InvalidSteamID = 19;

		public const int ServiceUnavailable = 20;

		public const int NotLoggedOn = 21;

		public const int Pending = 22;

		public const int EncryptionFailure = 23;

		public const int InsufficientPrivilege = 24;

		public const int LimitExceeded = 25;

		public const int Revoked = 26;

		public const int Expired = 27;

		public const int AlreadyRedeemed = 28;

		public const int DuplicateRequest = 29;

		public const int AlreadyOwned = 30;

		public const int IPNotFound = 31;

		public const int PersistFailed = 32;

		public const int LockingFailed = 33;

		public const int LogonSessionReplaced = 34;

		public const int ConnectFailed = 35;

		public const int HandshakeFailed = 36;

		public const int IOFailure = 37;

		public const int RemoteDisconnect = 38;

		public const int ShoppingCartNotFound = 39;

		public const int Blocked = 40;

		public const int Ignored = 41;

		public const int NoMatch = 42;

		public const int AccountDisabled = 43;

		public const int ServiceReadOnly = 44;

		public const int AccountNotFeatured = 45;

		public const int AdministratorOK = 46;

		public const int ContentVersion = 47;

		public const int TryAnotherCM = 48;

		public const int PasswordRequiredToKickSession = 49;

		public const int AlreadyLoggedInElsewhere = 50;

		public const int Suspended = 51;

		public const int Cancelled = 52;

		public const int DataCorruption = 53;

		public const int DiskFull = 54;

		public const int RemoteCallFailed = 55;

		public const int PasswordUnset = 56;

		public const int ExternalAccountUnlinked = 57;

		public const int PSNTicketInvalid = 58;

		public const int ExternalAccountAlreadyLinked = 59;

		public const int RemoteFileConflict = 60;

		public const int IllegalPassword = 61;

		public const int SameAsPreviousValue = 62;

		public const int AccountLogonDenied = 63;

		public const int CannotUseOldPassword = 64;

		public const int InvalidLoginAuthCode = 65;

		public const int AccountLogonDeniedNoMail = 66;

		public const int HardwareNotCapableOfIPT = 67;

		public const int IPTInitError = 68;

		public const int ParentalControlRestricted = 69;

		public const int FacebookQueryError = 70;

		public const int ExpiredLoginAuthCode = 71;

		public const int IPLoginRestrictionFailed = 72;

		public const int AccountLockedDown = 73;

		public const int AccountLogonDeniedVerifiedEmailRequired = 74;

		public const int NoMatchingURL = 75;

		public const int BadResponse = 76;

		public const int RequirePasswordReEntry = 77;

		public const int ValueOutOfRange = 78;
	}
}
