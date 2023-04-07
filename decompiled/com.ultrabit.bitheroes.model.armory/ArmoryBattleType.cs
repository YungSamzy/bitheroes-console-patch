using System.Collections.Generic;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.armory;

public class ArmoryBattleType
{
	private int _id;

	private string _name;

	private int _position;

	public int id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}

	public string name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public int position
	{
		get
		{
			return _position;
		}
		set
		{
			_position = value;
		}
	}

	public ArmoryBattleType(int? id, string name, int? position)
	{
		_id = (id.HasValue ? id.Value : 0);
		_name = ((name != null) ? name : "");
		_position = (position.HasValue ? position.Value : 0);
	}

	public static List<ArmoryBattleType> ListFromSFSObject(ISFSObject sfsob)
	{
		if (sfsob == null)
		{
			return new List<ArmoryBattleType>();
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("cha106");
		List<ArmoryBattleType> list = new List<ArmoryBattleType>();
		if (sFSArray == null)
		{
			return list;
		}
		for (uint num = 0u; num < sFSArray.Size(); num++)
		{
			ArmoryBattleType armoryBattleType = FromSFSObject(sFSArray.GetSFSObject((int)num));
			if (armoryBattleType != null)
			{
				list.Add(armoryBattleType);
			}
		}
		return list;
	}

	public static ArmoryBattleType FromSFSObject(ISFSObject sfsob)
	{
		sfsob.GetSFSObject("cha106");
		return new ArmoryBattleType(0, "", 0)
		{
			id = sfsob.GetInt("id"),
			position = sfsob.GetInt("position"),
			name = sfsob.GetUtfString("name")
		};
	}
}
