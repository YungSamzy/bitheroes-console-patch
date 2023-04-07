using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.language;

namespace com.ultrabit.bitheroes.model.armory;

[DebuggerDisplay("{name} (ArmoryRef)")]
public class ArmoryRef : EquipmentRef, IEquatable<ArmoryRef>, IComparable<ArmoryRef>
{
	public const int ARMORY_TYPE_NONE = 0;

	public const int ARMORY_TYPE_MAINHAND = 1;

	public const int ARMORY_TYPE_OFFHAND = 2;

	public const int ARMORY_TYPE_HEAD = 3;

	public const int ARMORY_TYPE_BODY = 4;

	public const int ARMORY_TYPE_NECK = 5;

	public const int ARMORY_TYPE_RING = 6;

	public const int ARMORY_TYPE_ACCESSORY = 7;

	public const int ARMORY_TYPE_PET = 8;

	public const int ARMORY_TYPE_MOUNT = 9;

	public const int ARMORY_TYPE_COUNT = 10;

	private int _armoryType;

	private static Dictionary<string, int> ARMORY_TYPES = new Dictionary<string, int>
	{
		["mainhand"] = 1,
		["offhand"] = 2,
		["head"] = 3,
		["body"] = 4,
		["neck"] = 5,
		["ring"] = 6,
		["accessory"] = 7,
		["pet"] = 8,
		["mount"] = 9
	};

	public int armoryType
	{
		get
		{
			return _armoryType;
		}
		set
		{
			_armoryType = value;
		}
	}

	public ArmoryRef(EquipmentRef equipmentRef)
		: base(equipmentRef.id, 16)
	{
		_equipmentType = equipmentRef.equipmentType;
		_power = equipmentRef.power;
		_stamina = equipmentRef.stamina;
		_agility = equipmentRef.agility;
		_elemental = equipmentRef.elemental;
		_projectileCenter = equipmentRef.projectileCenter;
		_projectileOffset = equipmentRef.projectileOffset;
		_modifiers = equipmentRef.modifiers;
		_abilities = equipmentRef.abilities;
		_subtypes = equipmentRef.subtypes;
		_assets = equipmentRef.assets;
		_upgrades = equipmentRef.upgrades;
		_reforges = equipmentRef.reforges;
		_statLabel = equipmentRef.statName;
		_name = equipmentRef.name;
		_desc = equipmentRef.desc;
		_box = equipmentRef.box;
		_thumbnail = equipmentRef.thumbnail;
		_icon = equipmentRef.icon;
		_loadLocal = equipmentRef.loadLocal;
		_rarityRef = equipmentRef.rarityRef;
		_probabilityRef = equipmentRef.probabilityRef;
		_costGold = equipmentRef.costGoldRaw;
		_costCredits = equipmentRef.costCreditsRaw;
		_sellGold = equipmentRef.sellGold;
		_sellCredits = equipmentRef.sellCredits;
		_tier = equipmentRef.tier;
		_exchangeable = equipmentRef.exchangeable;
		_unique = equipmentRef.unique;
		_allowQty = equipmentRef.allowQty;
		_lootDisplay = equipmentRef.lootDisplay;
		_cosmetic = equipmentRef.cosmetic;
		_gacha = equipmentRef.gacha;
		_assetsOverride = equipmentRef.assetsOverride;
		_assetsSourceID = equipmentRef.assetsSourceID;
		_attachSource = equipmentRef.attachSource;
		_setSourceID = equipmentRef.setSourceID;
		_tutorialID = equipmentRef.tutorialID;
		_equipmentSet = equipmentRef.equipmentSet;
	}

	public static string GetArmoryTypeName(int type)
	{
		return Language.GetString("equipment_type_" + type + "_name");
	}

	public static ArmoryRef EquipmentRefToArmoryRef(EquipmentRef equipRef)
	{
		if (equipRef == null)
		{
			return null;
		}
		return equipRef.GetArmoryRefFromEquipmentRef();
	}

	public bool Equals(ArmoryRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((EquipmentRef)other);
	}

	public int CompareTo(ArmoryRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((EquipmentRef)other);
	}
}
