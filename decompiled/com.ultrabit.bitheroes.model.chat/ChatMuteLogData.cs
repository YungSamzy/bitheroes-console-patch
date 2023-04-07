using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.chat;

public class ChatMuteLogData
{
	private int _id;

	private DateTime _date;

	private int _characterID;

	private string _characterName;

	private int _moderatorID;

	private string _moderatorName;

	private int _duration;

	private int _reason;

	public int id => _id;

	public DateTime date => _date;

	public int characterID => _characterID;

	public string characterName => _characterName;

	public int moderatorID => _moderatorID;

	public string moderatorName => _moderatorName;

	public int duration => _duration;

	public int reason => _reason;

	public ChatMuteLogData(int id, DateTime date, int characterID, string characterName, int moderatorID, string moderatorName, int duration, int reason)
	{
		_id = id;
		_date = date;
		_characterID = characterID;
		_characterName = characterName;
		_moderatorID = moderatorID;
		_moderatorName = moderatorName;
		_duration = duration;
		_reason = reason;
	}

	public static ChatMuteLogData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("chat8");
		DateTime dateFromString = Util.GetDateFromString(sfsob.GetUtfString("chat9"));
		int int2 = sfsob.GetInt("chat10");
		string utfString = sfsob.GetUtfString("chat11");
		int int3 = sfsob.GetInt("chat12");
		string utfString2 = sfsob.GetUtfString("chat13");
		int int4 = sfsob.GetInt("chat14");
		int int5 = sfsob.GetInt("chat15");
		return new ChatMuteLogData(@int, dateFromString, int2, utfString, int3, utfString2, int4, int5);
	}

	public static List<ChatMuteLogData> listFromSFSObject(ISFSObject sfsob)
	{
		List<ChatMuteLogData> list = new List<ChatMuteLogData>();
		if (!sfsob.ContainsKey("chat7"))
		{
			return list;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("chat7");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(fromSFSObject(sFSObject));
		}
		return list;
	}
}
