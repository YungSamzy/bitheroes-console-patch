using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.equipment;

[DebuggerDisplay("{name} (EquipmentRef)")]
public class EquipmentRef : ItemRef, IEquatable<EquipmentRef>, IComparable<EquipmentRef>
{
	public const int EQUIPMENT_TYPE_NONE = 0;

	public const int EQUIPMENT_TYPE_MAINHAND = 1;

	public const int EQUIPMENT_TYPE_OFFHAND = 2;

	public const int EQUIPMENT_TYPE_HEAD = 3;

	public const int EQUIPMENT_TYPE_BODY = 4;

	public const int EQUIPMENT_TYPE_NECK = 5;

	public const int EQUIPMENT_TYPE_RING = 6;

	public const int EQUIPMENT_TYPE_ACCESSORY = 7;

	public const int EQUIPMENT_TYPE_PET = 8;

	public const int EQUIPMENT_TYPE_COUNT = 8;

	private static Dictionary<string, int> EQUIPMENT_TYPES = new Dictionary<string, int>
	{
		["mainhand"] = 1,
		["offhand"] = 2,
		["head"] = 3,
		["body"] = 4,
		["neck"] = 5,
		["ring"] = 6,
		["accessory"] = 7,
		["pet"] = 8
	};

	public const int EQUIPMENT_ELEMENTAL_TYPE_PHYSIC = 0;

	public const int EQUIPMENT_ELEMENTAL_TYPE_FIRE = 1;

	public const int EQUIPMENT_ELEMENTAL_TYPE_AIR = 2;

	public const int EQUIPMENT_ELEMENTAL_TYPE_ELECTRIC = 3;

	public const int EQUIPMENT_ELEMENTAL_TYPE_WATER = 4;

	public const int EQUIPMENT_ELEMENTAL_TYPE_EARTH = 5;

	private static Dictionary<string, int> ELEMENTAL_TYPES = new Dictionary<string, int>
	{
		["physic"] = 0,
		["fire"] = 1,
		["air"] = 2,
		["electric"] = 3,
		["water"] = 4,
		["earth"] = 5
	};

	protected List<EquipmentSubtypeRef> _subtypes;

	protected List<AbilityRef> _abilities;

	protected List<EquipmentAssetRef> _assets;

	protected List<GameModifier> _modifiers;

	protected int _order;

	protected bool _projectileCenter;

	protected Vector2 _projectileOffset;

	protected EquipmentSetRef _equipmentSet;

	protected EquipmentRef _setSource;

	protected int _equipmentType;

	protected int _power;

	protected int _stamina;

	protected int _agility;

	protected int _elemental;

	public int equipmentType
	{
		get
		{
			if (!(base.assetsSource != null))
			{
				return _equipmentType;
			}
			return (base.assetsSource as EquipmentRef).equipmentType;
		}
	}

	public int power => _power;

	public int stamina => _stamina;

	public int agility => _agility;

	public int total => power + stamina + agility;

	public int elemental
	{
		get
		{
			if (!(base.assetsSource != null))
			{
				return _elemental;
			}
			return (base.assetsSource as EquipmentRef).elemental;
		}
	}

	public List<EquipmentSubtypeRef> subtypes
	{
		get
		{
			if (!(base.assetsSource != null))
			{
				return _subtypes;
			}
			return (base.assetsSource as EquipmentRef).subtypes;
		}
	}

	public List<AbilityRef> abilities
	{
		get
		{
			if (!(base.assetsSource != null))
			{
				return _abilities;
			}
			return (base.assetsSource as EquipmentRef).abilities;
		}
	}

	public List<GameModifier> modifiers
	{
		get
		{
			if ((_modifiers != null && _modifiers.Count > 0) || !(base.assetsSource != null))
			{
				return _modifiers;
			}
			return (base.assetsSource as EquipmentRef).modifiers;
		}
	}

	public List<EquipmentAssetRef> assets
	{
		get
		{
			if (!(base.assetsSource != null))
			{
				return _assets;
			}
			return (base.assetsSource as EquipmentRef).assets;
		}
	}

	public int order
	{
		get
		{
			if (!(base.assetsSource != null))
			{
				return _order;
			}
			return (base.assetsSource as EquipmentRef).order;
		}
	}

	public bool projectileCenter
	{
		get
		{
			if (!(base.assetsSource != null))
			{
				return _projectileCenter;
			}
			return (base.assetsSource as EquipmentRef).projectileCenter;
		}
	}

	public Vector2 projectileOffset
	{
		get
		{
			if (!(base.assetsSource != null))
			{
				return _projectileOffset;
			}
			return (base.assetsSource as EquipmentRef).projectileOffset;
		}
	}

	public EquipmentSetRef equipmentSet
	{
		get
		{
			if (_equipmentSet != null)
			{
				return _equipmentSet;
			}
			if (setSource != null)
			{
				return setSource.equipmentSet;
			}
			if (base.assetsSource != null)
			{
				return (base.assetsSource as EquipmentRef).equipmentSet;
			}
			return _equipmentSet;
		}
	}

	public EquipmentRef setSource
	{
		get
		{
			if (_equipmentSet != null)
			{
				return this;
			}
			if (base.setSourceID > 0 && _setSource == null)
			{
				updateSetSource();
			}
			if (_setSource != null)
			{
				return _setSource;
			}
			if (base.assetsSource != null)
			{
				return (base.assetsSource as EquipmentRef).setSource;
			}
			return _setSource;
		}
	}

	public EquipmentRef(int id, int type)
		: base(id, type)
	{
	}

	public EquipmentRef(int id, EquipmentBookData.Equipment equipmentData)
		: base(id, 1)
	{
		_equipmentType = getEquipmentType(equipmentData.type);
		_power = equipmentData.power;
		_stamina = equipmentData.stamina;
		_agility = equipmentData.agility;
		_elemental = getEquipmentElementalType(equipmentData.elemental);
		_projectileCenter = Util.parseBoolean(equipmentData.projectileCenter);
		_projectileOffset = ((equipmentData.projectileOffset != null) ? Util.pointFromString(equipmentData.projectileOffset) : new Vector2(0f, 0f));
		_modifiers = GameModifier.GetGameModifierFromData(equipmentData.modifiers, equipmentData.lstModifier);
		if (equipmentData.abilities != null)
		{
			_abilities = AbilityBook.LookupAbilities(equipmentData.abilities);
		}
		_subtypes = EquipmentBook.LookupSubtypeLinks(equipmentData.subtypes);
		_assets = new List<EquipmentAssetRef>();
		foreach (EquipmentBookData.Asset item in equipmentData.lstAsset)
		{
			_assets.Add(new EquipmentAssetRef(item));
		}
		_upgrades = new List<ItemUpgradeRef>();
		if (equipmentData.lstUpgrade.Count > 0)
		{
			for (int i = 0; i < equipmentData.lstUpgrade.Count; i++)
			{
				_upgrades.Add(new ItemUpgradeRef(i, equipmentData.lstUpgrade[i].id, 1, equipmentData.lstUpgrade[i].link));
			}
		}
		_reforges = new List<ItemReforgeRef>();
		if (equipmentData.lstReforge.Count > 0)
		{
			foreach (EquipmentBookData.Reforge item2 in equipmentData.lstReforge)
			{
				_reforges.Add(new ItemReforgeRef(item2.id, 1, item2.link));
			}
		}
		LoadDetails(equipmentData);
	}

	public Asset getFirstAsset(bool center = false, float scale = 1f, bool offset = true)
	{
		if (assets == null || assets.Count <= 0)
		{
			return null;
		}
		using (List<EquipmentAssetRef>.Enumerator enumerator = assets.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current.getAsset(center, scale, offset);
			}
		}
		return null;
	}

	public bool hasSubtype(int subtype)
	{
		List<EquipmentSubtypeRef> list = subtypes;
		if (list == null)
		{
			return false;
		}
		foreach (EquipmentSubtypeRef item in list)
		{
			if (item.id == subtype)
			{
				return true;
			}
		}
		return false;
	}

	public bool subtypesMatch(EquipmentRef targetRef)
	{
		if (subtypes == null && targetRef.subtypes == null)
		{
			return true;
		}
		if (subtypes.Count <= 0 && targetRef.subtypes.Count <= 0)
		{
			return true;
		}
		foreach (EquipmentSubtypeRef subtype in subtypes)
		{
			foreach (EquipmentSubtypeRef subtype2 in targetRef.subtypes)
			{
				if (subtype.id == subtype2.id)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool isRelated(EquipmentRef equipmentRef)
	{
		if (equipmentRef == null)
		{
			return false;
		}
		if (equipmentRef == this)
		{
			return true;
		}
		if (equipmentRef.assetsSource != null && (equipmentRef.assetsSource as EquipmentRef).isRelated(this))
		{
			return true;
		}
		if (base.assetsSource != null && (base.assetsSource as EquipmentRef).isRelated(equipmentRef))
		{
			return true;
		}
		return false;
	}

	public EquipmentSubtypeRef getSubtypeTooltip()
	{
		if (subtypes == null)
		{
			return null;
		}
		foreach (EquipmentSubtypeRef subtype in subtypes)
		{
			if (subtype.tooltip)
			{
				return subtype;
			}
		}
		return null;
	}

	public static int getEquipmentType(string type)
	{
		if (type == null || type.Length <= 0)
		{
			return 0;
		}
		return EQUIPMENT_TYPES[type.ToLowerInvariant()];
	}

	public static string getEquipmentTypeName(int type)
	{
		return Language.GetString("equipment_type_" + type + "_name");
	}

	public static string GetEquipmentTypeNamePlural(int type)
	{
		return Language.GetString("equipment_type_" + type + "_plural_name");
	}

	public static int getEquipmentElementalType(string type)
	{
		if (type == null || type.Length <= 0)
		{
			return 0;
		}
		return ELEMENTAL_TYPES[type.ToLower()];
	}

	public void SetEquipmentSet(EquipmentSetRef pEquipmentSet)
	{
		_equipmentSet = pEquipmentSet;
	}

	private void updateSetSource()
	{
		_setSource = EquipmentBook.Lookup(base.setSourceID);
	}

	public ArmoryRef GetArmoryRefFromEquipmentRef()
	{
		ArmoryRef armoryRef = new ArmoryRef(this);
		armoryRef.armoryType = equipmentType;
		armoryRef.UpdateAssetsSource(base.assetsSource);
		armoryRef.updateRanks(base.rank, base.ranks);
		return armoryRef;
	}

	public bool Equals(EquipmentRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(EquipmentRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
