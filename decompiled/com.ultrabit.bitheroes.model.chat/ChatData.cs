using System.Collections.Generic;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.chat;

public class ChatData
{
	private int _charID;

	private string _name;

	private int _level;

	private string _guildInitials;

	private string _message;

	private bool _admin;

	private bool _moderator;

	private bool _hasVipgor;

	public int charID => _charID;

	public string name => _name;

	public int level => _level;

	public string guildInitials => _guildInitials;

	public string message => _message;

	public bool admin => _admin;

	public bool moderator => _moderator;

	public bool hasVipgor => _hasVipgor;

	public ChatData(int charID, int level, string name, string guildInitials, string message, bool admin, bool moderator, bool hasVipgor)
	{
		_charID = charID;
		_name = name;
		_level = level;
		_guildInitials = guildInitials;
		_message = message;
		_admin = admin;
		_moderator = moderator;
		_hasVipgor = hasVipgor;
	}

	public static ChatData fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("cha1"))
		{
			return null;
		}
		int @int = sfsob.GetInt("cha1");
		string utfString = sfsob.GetUtfString("cha2");
		int int2 = sfsob.GetInt("cha4");
		string text = (sfsob.ContainsKey("gui3") ? sfsob.GetUtfString("gui3") : null);
		string utfString2 = sfsob.GetUtfString("chat0");
		bool @bool = sfsob.GetBool("cha15");
		bool bool2 = sfsob.GetBool("cha34");
		bool bool3 = sfsob.GetBool("hasVipgor");
		return new ChatData(@int, int2, utfString, text, utfString2, @bool, bool2, bool3);
	}

	public static List<ChatData> listFromSFSObject(ISFSObject sfsob)
	{
		List<ChatData> list = new List<ChatData>();
		if (!sfsob.ContainsKey("chat3"))
		{
			return list;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("chat3");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(fromSFSObject(sFSObject));
		}
		return list;
	}
}
