using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.events;

public class EventHistoryData
{
	private int _id;

	private DateTime _createDate;

	private int _charID;

	private string _name;

	private string _guildInitials;

	private int _points;

	public string parsedName => Util.ParseName(_name, _guildInitials);

	public string parsedCreatedDate => Util.TimeFormatClean((float)ServerExtension.instance.GetDate().Subtract(_createDate).TotalMilliseconds / 1000f);

	public string parsedPoints
	{
		get
		{
			if (_points >= 0)
			{
				return "<color=green>+" + _points + "</color>";
			}
			return "<color=red>+" + _points + "</color>";
		}
	}

	public int id => _id;

	public DateTime createDate => _createDate;

	public int charID => _charID;

	public string name => _name;

	public int points => _points;

	public EventHistoryData(int id, DateTime createDate, int charID, string name, string guildInitials, int points)
	{
		_id = id;
		_createDate = createDate;
		_charID = charID;
		_name = name;
		_guildInitials = guildInitials;
		_points = points;
	}

	public static EventHistoryData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("eve4");
		DateTime dateFromString = Util.GetDateFromString(sfsob.GetUtfString("eve8"));
		int int2 = sfsob.GetInt("cha1");
		string utfString = sfsob.GetUtfString("cha2");
		string guildInitials = (sfsob.ContainsKey("gui3") ? sfsob.GetUtfString("gui3") : null);
		int int3 = sfsob.GetInt("eve2");
		return new EventHistoryData(@int, dateFromString, int2, utfString, guildInitials, int3);
	}

	public static List<EventHistoryData> listFromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("eve3");
		List<EventHistoryData> list = new List<EventHistoryData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(fromSFSObject(sFSObject));
		}
		return list;
	}
}
