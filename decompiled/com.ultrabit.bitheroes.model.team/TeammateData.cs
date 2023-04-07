using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.model.user;
using com.ultrabit.bitheroes.ui.assets;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.team;

[DebuggerDisplay("{nameColorlessOnly} {typeName} (TeammateData)")]
public class TeammateData
{
	public const int TYPE_PLAYER = 1;

	public const int TYPE_FAMILIAR = 2;

	private const string STRING_DIVIDER = ":";

	private const string STRING_SEPERATOR = ",";

	private int _id;

	private int _type;

	private int _teamType;

	private object _data;

	private int _power = -1;

	private int _powerToCalculate = -1;

	private int _stamina = -1;

	private int _staminaToCalculate = -1;

	private int _agility = -1;

	private int _agilityToCalculate = -1;

	private int _battleType = -1;

	private long _armoryID = -1L;

	public int powerCalculate => _powerToCalculate;

	public int staminaCalculate => _staminaToCalculate;

	public int agilityCalculate => _agilityToCalculate;

	public Vector2 selectOffset
	{
		get
		{
			if (data is CharacterData)
			{
				_ = data;
			}
			FamiliarRef familiarRef = ((data is FamiliarRef) ? (data as FamiliarRef) : null);
			if (!(familiarRef != null))
			{
				return default(Vector2);
			}
			return familiarRef.selectOffset;
		}
	}

	public float selectScale
	{
		get
		{
			if (data is CharacterData)
			{
				_ = data;
			}
			FamiliarRef familiarRef = ((data is FamiliarRef) ? (data as FamiliarRef) : null);
			if (!(familiarRef != null))
			{
				return 1f;
			}
			return familiarRef.selectScale;
		}
	}

	public int level
	{
		get
		{
			CharacterData characterData = ((data is CharacterData) ? (data as CharacterData) : null);
			if (data is FamiliarRef)
			{
				_ = data;
			}
			return characterData?.level ?? GameData.instance.PROJECT.character.level;
		}
	}

	public int total => power + stamina + agility;

	public int totalCalculated => _power + _stamina + _agility;

	public int staminaCalculated => _stamina;

	public int power
	{
		get
		{
			if (_power >= 0)
			{
				return _power;
			}
			CharacterData characterData = ((data is CharacterData) ? (data as CharacterData) : null);
			FamiliarRef familiarRef = ((data is FamiliarRef) ? (data as FamiliarRef) : null);
			_power = characterData?.getTotalPower() ?? familiarRef.getPower(GameData.instance.PROJECT.character.getTotalStats());
			return _power;
		}
	}

	public int stamina
	{
		get
		{
			if (_stamina >= 0)
			{
				return _stamina;
			}
			CharacterData characterData = ((data is CharacterData) ? (data as CharacterData) : null);
			FamiliarRef familiarRef = ((data is FamiliarRef) ? (data as FamiliarRef) : null);
			_stamina = characterData?.getTotalStamina() ?? familiarRef.getStamina(GameData.instance.PROJECT.character.getTotalStats());
			return _stamina;
		}
	}

	public int agility
	{
		get
		{
			if (_agility >= 0)
			{
				return _agility;
			}
			CharacterData characterData = ((data is CharacterData) ? (data as CharacterData) : null);
			FamiliarRef familiarRef = ((data is FamiliarRef) ? (data as FamiliarRef) : null);
			_agility = characterData?.getTotalAgility() ?? familiarRef.getAgility(GameData.instance.PROJECT.character.getTotalStats());
			return _agility;
		}
	}

	public string nameColorless
	{
		get
		{
			CharacterData characterData = ((data is CharacterData) ? (data as CharacterData) : null);
			FamiliarRef familiarRef = ((data is FamiliarRef) ? (data as FamiliarRef) : null);
			if (characterData == null)
			{
				return familiarRef.name;
			}
			return characterData.parsedName;
		}
	}

	public string name
	{
		get
		{
			CharacterData characterData = ((data is CharacterData) ? (data as CharacterData) : null);
			FamiliarRef familiarRef = ((data is FamiliarRef) ? (data as FamiliarRef) : null);
			if (characterData == null)
			{
				return familiarRef.coloredName;
			}
			return characterData.parsedName;
		}
	}

	public string nameColorlessOnly
	{
		get
		{
			CharacterData characterData = ((data is CharacterData) ? (data as CharacterData) : null);
			FamiliarRef familiarRef = ((data is FamiliarRef) ? (data as FamiliarRef) : null);
			if (characterData == null)
			{
				return familiarRef.name;
			}
			return characterData.name;
		}
	}

	public string typeName => type switch
	{
		1 => "Player", 
		_ => "Familiar", 
	};

	public int id => _id;

	public long armoryID
	{
		get
		{
			return _armoryID;
		}
		set
		{
			_armoryID = value;
			SetCharacterDataArmory();
		}
	}

	public int type => _type;

	public object data
	{
		get
		{
			if (_data != null)
			{
				return _data;
			}
			switch (_type)
			{
			case 1:
			{
				if (id == GameData.instance.PROJECT.character.id)
				{
					_data = GameData.instance.PROJECT.character.toCharacterData();
					break;
				}
				UserData contact = GameData.instance.PROJECT.character.getContact(id, _teamType);
				if (contact != null)
				{
					_data = contact.characterData;
				}
				break;
			}
			case 2:
				_data = FamiliarBook.Lookup(id);
				break;
			}
			return _data;
		}
	}

	public TeammateData(int id, int type, long armoryID = -1L, bool forceCalculate = false, int teamType = -1)
	{
		_id = id;
		_type = type;
		_armoryID = armoryID;
		_teamType = teamType;
		if (forceCalculate)
		{
			_ = total;
		}
	}

	public void SetBattleType(int pType)
	{
		_battleType = pType;
		if (!(data is CharacterData))
		{
			return;
		}
		CharacterData characterData = ((data is CharacterData) ? (data as CharacterData) : null);
		characterData = characterData.Duplicate();
		if (characterData.armory.armoryEquipmentSlots.Count <= 0 || _armoryID > 0)
		{
			return;
		}
		for (int i = 0; i < characterData.armory.armoryEquipmentSlots.Count; i++)
		{
			ArmoryEquipment armoryEquipment = characterData.armory.armoryEquipmentSlots[i];
			if (armoryEquipment.battleType == _battleType)
			{
				_armoryID = armoryEquipment.id;
				if (characterData.charID == GameData.instance.PROJECT.character.id)
				{
					GameData.instance.PROJECT.character.armory.battleArmorySelected = true;
				}
				break;
			}
		}
	}

	public Transform getAsset(float scale)
	{
		CharacterData characterData = ((data is CharacterData) ? (data as CharacterData) : null);
		FamiliarRef familiarRef = ((data is FamiliarRef) ? (data as FamiliarRef) : null);
		RecarculateStats();
		if (characterData != null)
		{
			characterData = characterData.Duplicate();
			if (_armoryID > 0)
			{
				characterData.armory.SetCurrentArmoryEquipmentSlotByID(_armoryID);
				Equipment equipment = ArmoryEquipment.ArmoryEquipmentToEquipment(characterData.armory.currentArmoryEquipmentSlot);
				characterData.setEquipment(equipment);
				Runes runes = new Runes(characterData.armory.currentArmoryEquipmentSlot.runes.runeSlots, characterData.armory.currentArmoryEquipmentSlot.runes.runeSlotsMemory);
				characterData.runes = runes;
				RecarculateStats(forceCalc: true);
			}
			else if (characterData.charID == GameData.instance.PROJECT.character.id)
			{
				_armoryID = -1L;
				GameData.instance.PROJECT.character.armory.battleArmorySelected = false;
				GameData.instance.PROJECT.character.armory.SetCurrentArmoryEquipmentSlot(null);
				characterData = GameData.instance.PROJECT.character.toCharacterData(duplicateMounts: true);
			}
			return characterData.toCharacterDisplay(scale, displayMount: false, null, enableLoading: false).transform;
		}
		if (familiarRef != null)
		{
			return familiarRef.displayRef.getAsset(center: true, scale);
		}
		return null;
	}

	private void RecarculateStats(bool forceCalc = false)
	{
		CharacterData characterData = ((data is CharacterData) ? (data as CharacterData) : null);
		FamiliarRef familiarRef = ((data is FamiliarRef) ? (data as FamiliarRef) : null);
		_power = characterData?.getTotalPower(forceCalc) ?? familiarRef.getPower(GameData.instance.PROJECT.character.getTotalStats());
		_stamina = characterData?.getTotalStamina(forceCalc) ?? familiarRef.getStamina(GameData.instance.PROJECT.character.getTotalStats());
		_agility = characterData?.getTotalAgility(forceCalc) ?? familiarRef.getAgility(GameData.instance.PROJECT.character.getTotalStats());
	}

	public void SetPowerToCalulate(int val)
	{
		_powerToCalculate = val;
	}

	public void SetStaminaToCalulate(int val)
	{
		_staminaToCalculate = val;
	}

	public void SetAgilityToCalulate(int val)
	{
		_agilityToCalculate = val;
	}

	public Asset getIconAsset(bool center, float scale)
	{
		CharacterData characterData = ((data is CharacterData) ? (data as CharacterData) : null);
		if (data is FamiliarRef)
		{
			_ = data;
		}
		return characterData?.toCharacterDisplay(scale).convertToIcon(center, scale);
	}

	public void SetCharacterDataArmory()
	{
		if (data is CharacterData characterData && armoryID > 0)
		{
			characterData.armory.SetCurrentArmoryEquipmentSlotByID(armoryID);
			Equipment equipment = ArmoryEquipment.ArmoryEquipmentToEquipment(characterData.armory.currentArmoryEquipmentSlot);
			characterData.setEquipment(equipment);
			Runes runes = new Runes(characterData.armory.currentArmoryEquipmentSlot.runes.runeSlots, characterData.armory.currentArmoryEquipmentSlot.runes.runeSlotsMemory);
			characterData.runes = runes;
		}
	}

	public SFSObject toSFSObject(SFSObject sfsob)
	{
		sfsob.PutInt("tmts1", _id);
		sfsob.PutInt("tmts2", _type);
		sfsob.PutFloat("tmts3", _armoryID);
		return sfsob;
	}

	public string toString()
	{
		return _id + ":" + _type;
	}

	public static TeammateData fromString(string theString)
	{
		string[] array = theString.Split(":".ToCharArray());
		return new TeammateData(int.Parse(array[0]), int.Parse(array[1]), -1L);
	}

	public static string listToString(List<TeammateData> list)
	{
		if (list == null || list.Count <= 0)
		{
			return "";
		}
		string text = "";
		for (int i = 0; i < list.Count; i++)
		{
			TeammateData teammateData = list[i];
			if (i > 0)
			{
				text += ",";
			}
			text += teammateData.toString();
		}
		return text;
	}

	public static List<TeammateData> listFromString(string theString)
	{
		if (theString == null)
		{
			return new List<TeammateData>();
		}
		List<TeammateData> list = new List<TeammateData>();
		string[] array = theString.Split(",".ToCharArray());
		foreach (string theString2 in array)
		{
			list.Add(fromString(theString2));
		}
		return list;
	}

	public static bool listHasTeammate(List<TeammateData> list, int id, int type)
	{
		foreach (TeammateData item in list)
		{
			if (item.id == id && item.type == type)
			{
				return true;
			}
		}
		return false;
	}

	public static SFSObject listToSFSObject(SFSObject sfsob, List<TeammateData> teammates)
	{
		ISFSArray iSFSArray = new SFSArray();
		for (int i = 0; i < teammates.Count; i++)
		{
			if (teammates[i] != null)
			{
				TeammateData teammateData = teammates[i];
				iSFSArray.AddSFSObject(teammateData.toSFSObject(new SFSObject()));
			}
		}
		sfsob.PutSFSArray("tmts0", iSFSArray);
		return sfsob;
	}

	public static TeammateData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("tmts1");
		int int2 = sfsob.GetInt("tmts2");
		return new TeammateData(@int, int2, -1L);
	}

	public static List<TeammateData> listFromSFSObject(ISFSObject sfsob)
	{
		List<TeammateData> list = new List<TeammateData>();
		ISFSArray sFSArray = sfsob.GetSFSArray("tmts0");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			TeammateData item = fromSFSObject(sFSArray.GetSFSObject(i));
			list.Add(item);
		}
		return list;
	}
}
