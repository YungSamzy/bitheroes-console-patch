using System.Collections.Generic;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.chat;

public class ChatMuteData
{
	private int _charID;

	private string _name;

	private long _muteSeconds;

	private int _muteReason;

	public int charID => _charID;

	public string name => _name;

	public long muteSeconds => _muteSeconds;

	public int muteReason => _muteReason;

	public ChatMuteData(int charID, string name, long muteSeconds, int muteReason)
	{
		_charID = charID;
		_name = name;
		_muteSeconds = muteSeconds;
		_muteReason = muteReason;
	}

	public static ChatMuteData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("cha1");
		string utfString = sfsob.GetUtfString("cha2");
		long @long = sfsob.GetLong("cha38");
		int int2 = sfsob.GetInt("cha39");
		return new ChatMuteData(@int, utfString, @long, int2);
	}

	public static List<ChatMuteData> listFromSFSObject(ISFSObject sfsob)
	{
		List<ChatMuteData> list = new List<ChatMuteData>();
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
