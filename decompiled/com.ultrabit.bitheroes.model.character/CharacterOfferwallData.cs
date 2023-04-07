using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.character;

public class CharacterOfferwallData
{
	private int _id;

	private DateTime _createDate;

	private int _credits;

	private string _network;

	private string _eventID;

	private bool _looted;

	public int id => _id;

	public DateTime createDate => _createDate;

	public int credits => _credits;

	public string network => _network;

	public string eventID => _eventID;

	public bool looted => _looted;

	public CharacterOfferwallData(int id, DateTime createDate, int credits, string network, string eventID, bool looted)
	{
		_id = id;
		_createDate = createDate;
		_credits = credits;
		_network = network;
		_eventID = eventID;
		_looted = looted;
	}

	public static CharacterOfferwallData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("off0");
		DateTime dateFromString = Util.GetDateFromString(sfsob.GetUtfString("off1"));
		int int2 = sfsob.GetInt("off2");
		string utfString = sfsob.GetUtfString("off6");
		string utfString2 = sfsob.GetUtfString("off3");
		bool @bool = sfsob.GetBool("off5");
		return new CharacterOfferwallData(@int, dateFromString, int2, utfString, utfString2, @bool);
	}

	public static List<CharacterOfferwallData> listFromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("off4");
		List<CharacterOfferwallData> list = new List<CharacterOfferwallData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(fromSFSObject(sFSObject));
		}
		return list;
	}
}
