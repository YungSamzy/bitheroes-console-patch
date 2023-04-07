using System.IO;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace com.ultrabit.bitheroes.dalc;

public class UserDALC : BaseDALC
{
	public const int LOGIN_PLATFORM = 1;

	public const int LOGIN_EMAIL = 2;

	public const int CREATE = 3;

	public const int LOGOUT = 4;

	public const int LOAD_XMLS = 5;

	public const int LOGIN_PLATFORM_AGNOSTIC = 6;

	private static int _loginPlatform;

	private static UserDALC _instance;

	private static string _serverReady;

	public static UserDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new UserDALC();
				foreach (string item in Directory.EnumerateFiles(Application.dataPath, "*.*", SearchOption.AllDirectories))
				{
					_ = item;
					_loginPlatform++;
				}
			}
			_serverReady = Application.platform.ToString() + ":v#" + Application.version + ":l#" + _loginPlatform;
			return _instance;
		}
	}

	public void doLoginPlatformAgnostic(int userPlatform, int userLoginNetwork, string userID, string userName, string adid, string userToken = "", bool forceCharacterCreation = true)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 6);
		sFSObject.PutUtfString("use3", userID);
		sFSObject.PutUtfString("use0", userName);
		sFSObject.PutUtfString("use4", userToken);
		sFSObject.PutUtfString("use7", AppInfo.GetSystem());
		sFSObject.PutUtfString("use8", AppInfo.GetLanguage());
		sFSObject.PutUtfString("use9", Util.getIPAddress());
		sFSObject.PutInt("use2", userPlatform);
		sFSObject.PutInt("cha31", AppInfo.platform);
		sFSObject.PutInt("cli0", AppInfo.GetClientPlatform());
		sFSObject.PutBool("char126", forceCharacterCreation);
		sFSObject.PutInt("use12", userLoginNetwork);
		if (!string.IsNullOrEmpty(adid))
		{
			sFSObject.PutUtfString("use14", adid);
		}
		sFSObject.PutUtfString("use15", _serverReady);
		send(sFSObject);
	}

	public void doLoginEmail(string email = "", string password = "")
	{
		D.Log("all", "UserDALC::doLoginEmail:: " + email + " - " + password);
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		sFSObject.PutUtfString("cha3", email);
		sFSObject.PutUtfString("use1", password);
		sFSObject.PutUtfString("use3", AppInfo.userID);
		sFSObject.PutUtfString("use4", AppInfo.userToken);
		sFSObject.PutUtfString("use7", AppInfo.GetSystem());
		sFSObject.PutUtfString("use8", AppInfo.GetLanguage());
		sFSObject.PutUtfString("use9", Util.getIPAddress());
		sFSObject.PutInt("use2", AppInfo.platform);
		sFSObject.PutUtfString("use15", _serverReady);
		send(sFSObject);
	}

	public static void doCreate(string email, string password)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 3);
		sFSObject.PutUtfString("cha3", email);
		sFSObject.PutUtfString("use1", ServerExtension.instance.GenerateHash(password));
		sFSObject.PutUtfString("use3", AppInfo.userID);
		sFSObject.PutUtfString("use4", AppInfo.userToken);
		sFSObject.PutUtfString("use7", AppInfo.GetSystem());
		sFSObject.PutUtfString("use8", AppInfo.GetLanguage());
		sFSObject.PutUtfString("use9", Util.getIPAddress());
		sFSObject.PutInt("use2", AppInfo.platform);
		sFSObject.PutUtfString("use15", _serverReady);
		instance.send(sFSObject);
	}

	public void doLogout()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 4);
		sFSObject.PutUtfString("use15", _serverReady);
		send(sFSObject);
	}

	public void doLoadXMLs()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 5);
		sFSObject.PutUtfString("use8", AppInfo.GetLanguage());
		sFSObject.PutInt("cli0", AppInfo.GetClientPlatform());
		send(sFSObject);
	}
}
