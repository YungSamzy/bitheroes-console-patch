using System.Collections.Generic;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.character;

public class CharacterPlatformData
{
	private int _id;

	private string _userID;

	private int _platform;

	private bool _active;

	public int id => _id;

	public string userID => _userID;

	public int platform => _platform;

	public bool active => _active;

	public CharacterPlatformData(int id, string userID, int platform, bool active)
	{
		_id = id;
		_userID = userID;
		_platform = platform;
		_active = active;
	}

	public static CharacterPlatformData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("plat0");
		string utfString = sfsob.GetUtfString("plat2");
		int int2 = sfsob.GetInt("plat1");
		bool @bool = sfsob.GetBool("plat3");
		return new CharacterPlatformData(@int, utfString, int2, @bool);
	}

	public static List<CharacterPlatformData> listFromSFSObject(ISFSObject sfsob)
	{
		List<CharacterPlatformData> list = new List<CharacterPlatformData>();
		if (!sfsob.ContainsKey("plat4"))
		{
			return list;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("plat4");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(fromSFSObject(sFSObject));
		}
		return list;
	}
}
