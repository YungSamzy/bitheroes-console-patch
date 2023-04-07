using System;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.character;

public class CharacterExtraInfoData
{
	private int _charID;

	private int _key;

	private string _data;

	private DateTime? _date;

	public string data
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
		}
	}

	public DateTime? date
	{
		get
		{
			return _date;
		}
		set
		{
			_date = value;
		}
	}

	public int key => _key;

	public int charID => _charID;

	public CharacterExtraInfoData(int charID = -1, int key = -1, string data = "", DateTime? date = null)
	{
		_charID = charID;
		_key = key;
		_data = data;
		_date = date;
	}

	public static CharacterExtraInfoData FromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("cha1"))
		{
			return null;
		}
		int @int = sfsob.GetInt("cha1");
		int int2 = sfsob.GetInt("ceid1");
		string utfString = sfsob.GetUtfString("ceid2");
		DateTime dateFromString = Util.GetDateFromString(sfsob.GetText("ceid3"));
		return new CharacterExtraInfoData(@int, int2, utfString, dateFromString);
	}
}
