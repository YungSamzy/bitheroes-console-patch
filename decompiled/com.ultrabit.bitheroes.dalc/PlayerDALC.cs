using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class PlayerDALC : BaseDALC
{
	public const int LOGIN_PLATFORM = 1;

	public const int LOGIN_EMAIL = 2;

	public const int CREATE_EMAIL = 3;

	public const int GET_CHARACTER_LIST = 4;

	public const int CREATE_CHARACTER = 5;

	public const int SELECT_CHARACTER = 6;

	public const int NAME_CHARACTER = 7;

	public const int CONFIRM_LINKAGE = 8;

	private static PlayerDALC _instance;

	public static PlayerDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PlayerDALC();
			}
			return _instance;
		}
	}

	private static void AddKongIDToSFSObject(SFSObject sfsob)
	{
		string kongID = AppInfo.kongID;
		string kongToken = AppInfo.kongToken;
		if (!string.IsNullOrEmpty(kongID))
		{
			sfsob.PutUtfString("pla8", kongID);
		}
		if (!string.IsNullOrEmpty(kongToken))
		{
			sfsob.PutUtfString("pla9", kongToken);
		}
	}

	private static void AddDeviceIDsToSFSObject(SFSObject sfsob)
	{
		string deviceID = AppInfo.deviceID;
		string anonID = AppInfo.anonID;
		string deviceToken = AppInfo.deviceToken;
		if (!string.IsNullOrEmpty(deviceID))
		{
			sfsob.PutUtfString("pla10", deviceID);
		}
		if (!string.IsNullOrEmpty(deviceToken))
		{
			sfsob.PutUtfString("pla11", deviceToken);
		}
		if (!string.IsNullOrEmpty(anonID))
		{
			sfsob.PutUtfString("pla12", anonID);
		}
	}

	private static void AddClientInfoToSFSObject(SFSObject sfsob)
	{
		sfsob.PutInt("pla2", AppInfo.platform);
		sfsob.PutInt("cli0", AppInfo.GetClientPlatform());
		sfsob.PutUtfString("pla4", AppInfo.GetSystem());
		sfsob.PutUtfString("pla5", AppInfo.GetLanguage());
		sfsob.PutUtfString("pla6", Util.getIPAddress());
	}

	public void doLoginPlatform()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		if (AppInfo.playerID != -1)
		{
			sFSObject.PutInt("pla3", AppInfo.playerID);
		}
		AddKongIDToSFSObject(sFSObject);
		AddDeviceIDsToSFSObject(sFSObject);
		AddClientInfoToSFSObject(sFSObject);
		send(sFSObject);
	}

	public void doLoginEmail(string email = "", string password = "")
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		sFSObject.PutUtfString("cha3", email);
		sFSObject.PutUtfString("pla1", password);
		AddKongIDToSFSObject(sFSObject);
		AddDeviceIDsToSFSObject(sFSObject);
		AddClientInfoToSFSObject(sFSObject);
		send(sFSObject);
	}

	public static void doCreateEmail(string email, string password)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 3);
		sFSObject.PutUtfString("cha3", email);
		sFSObject.PutUtfString("pla1", ServerExtension.instance.GenerateHash(password));
		AddDeviceIDsToSFSObject(sFSObject);
		AddKongIDToSFSObject(sFSObject);
		AddClientInfoToSFSObject(sFSObject);
		instance.send(sFSObject);
	}

	public void doCreateCharacter(string name, bool genderMale, int hairID, int hairColorID, int skinColorID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 5);
		sFSObject.PutUtfString("cha2", name);
		sFSObject.PutBool("cha12", genderMale);
		sFSObject.PutInt("cha20", hairID);
		sFSObject.PutInt("cha21", hairColorID);
		sFSObject.PutInt("cha22", skinColorID);
		if (AppInfo.playerID != -1)
		{
			sFSObject.PutInt("pla3", AppInfo.playerID);
		}
		if (!string.IsNullOrEmpty(GameData.instance.SAVE_STATE.email) && !string.IsNullOrEmpty(GameData.instance.SAVE_STATE.password))
		{
			sFSObject.PutUtfString("cha3", GameData.instance.SAVE_STATE.email);
			sFSObject.PutUtfString("pla1", ServerExtension.instance.GenerateHash(GameData.instance.SAVE_STATE.password));
		}
		AddDeviceIDsToSFSObject(sFSObject);
		AddKongIDToSFSObject(sFSObject);
		AddClientInfoToSFSObject(sFSObject);
		instance.send(sFSObject);
	}

	public void doGetCharacterList()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 4);
		if (AppInfo.playerID != -1)
		{
			sFSObject.PutInt("pla3", AppInfo.playerID);
		}
		AddKongIDToSFSObject(sFSObject);
		AddDeviceIDsToSFSObject(sFSObject);
		AddClientInfoToSFSObject(sFSObject);
		instance.send(sFSObject);
	}

	public void doConfirmCharacter(int characterID, int playerID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 6);
		sFSObject.PutInt("cha1", characterID);
		if (playerID != -1)
		{
			AppInfo.playerID = playerID;
			sFSObject.PutInt("pla3", playerID);
		}
		AddDeviceIDsToSFSObject(sFSObject);
		AddKongIDToSFSObject(sFSObject);
		AddClientInfoToSFSObject(sFSObject);
		instance.send(sFSObject);
	}

	public void doNFTNameChange(string nftToken, string name)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 7);
		sFSObject.PutUtfString("nft2", nftToken);
		sFSObject.PutUtfString("cha2", name);
		if (AppInfo.playerID != -1)
		{
			sFSObject.PutInt("pla3", AppInfo.playerID);
		}
		if (!string.IsNullOrEmpty(GameData.instance.SAVE_STATE.email) && !string.IsNullOrEmpty(GameData.instance.SAVE_STATE.password))
		{
			sFSObject.PutUtfString("cha3", GameData.instance.SAVE_STATE.email);
			sFSObject.PutUtfString("pla1", ServerExtension.instance.GenerateHash(GameData.instance.SAVE_STATE.password));
		}
		AddDeviceIDsToSFSObject(sFSObject);
		AddKongIDToSFSObject(sFSObject);
		AddClientInfoToSFSObject(sFSObject);
		instance.send(sFSObject);
	}

	public void doConfirmLinkage(int playerIDSelected, int charIDSelected, int playerIDIgnored, int charIDIgnored)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 8);
		sFSObject.PutInt("lnk1", playerIDSelected);
		sFSObject.PutInt("lnk2", charIDSelected);
		sFSObject.PutInt("lnk3", playerIDIgnored);
		sFSObject.PutInt("lnk4", charIDIgnored);
		AddDeviceIDsToSFSObject(sFSObject);
		AddKongIDToSFSObject(sFSObject);
		AddClientInfoToSFSObject(sFSObject);
		instance.send(sFSObject);
	}
}
