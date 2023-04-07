using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.zone;

namespace com.ultrabit.bitheroes.model.character;

[DebuggerDisplay("{type} (TutorialRef)")]
public class TutorialRef : IEquatable<TutorialRef>, IComparable<TutorialRef>
{
	public const string AUGMENT_CRAFT = "augment_craft";

	public const string AUGMENT_SHOP = "augment_shop";

	public const string SCHEMATIC_FUSION = "schematic_fusion";

	public const string PET_EQUIP = "pet_equip";

	public const string ACCESSORY_EQUIP = "accessory_equip";

	public const string MOUNT_SUMMON = "mount_summon";

	public const string RUNES_CRAFT = "runes_craft";

	public const string RUNES_SHOP = "runes_shop";

	public const string ENCHANTS_IDENTIFY = "enchants_identify";

	public const string CRAFT_UPGRADE = "craft_upgrade";

	public const string ADGOR_CHECK = "adgor_check";

	public const string PERSUADE_TUBBO = "persuade_tubbo";

	public const string FUSION_ADD = "fusion_add";

	public const string TEAMMATE_ADD = "teammate_add";

	public const string CRAFT_REFORGE = "craft_reforge";

	public const string FRIENDS_SUGGEST = "friends_suggest";

	private string _type;

	private int _minZone;

	private int _minNode;

	private int _maxZone;

	private int _maxNode;

	private int _requiredItemType;

	private int _requiredItemSubtype;

	private int _requiredItemQty;

	private bool _minFlagConditionMet
	{
		get
		{
			if (_minZone > -1)
			{
				if (_minNode > -1)
				{
					return GameData.instance.PROJECT.character.zones.nodeIsCompleted(ZoneBook.Lookup(_minZone).getNodeRef(_minNode));
				}
				return GameData.instance.PROJECT.character.zones.nodeIsCompleted(ZoneBook.Lookup(_minZone).getNodeRef(12));
			}
			if (_minNode > -1)
			{
				return GameData.instance.PROJECT.character.zones.nodeIsCompleted(ZoneBook.Lookup(1).getNodeRef(_minNode));
			}
			return true;
		}
	}

	private bool _maxFlagConditionMet
	{
		get
		{
			if (_maxZone > -1)
			{
				if (_maxNode > -1)
				{
					return !GameData.instance.PROJECT.character.zones.nodeIsCompleted(ZoneBook.Lookup(_maxZone).getNodeRef(_maxNode));
				}
				return GameData.instance.PROJECT.character.zones.getHighestCompletedZoneID() < _maxZone;
			}
			if (_maxNode > -1)
			{
				return !GameData.instance.PROJECT.character.zones.nodeIsCompleted(ZoneBook.Lookup(1).getNodeRef(_maxNode));
			}
			return true;
		}
	}

	private bool _requiredItemConditionsMet
	{
		get
		{
			if (_requiredItemType > -1)
			{
				return GameData.instance.PROJECT.character.inventory.GetItemsByType(_requiredItemType, _requiredItemSubtype).Count >= _requiredItemQty;
			}
			return true;
		}
	}

	public string type => _type;

	public bool isMinFlagConditionMet => _minFlagConditionMet;

	public bool isMaxFlagConditionMet => _maxFlagConditionMet;

	public bool areFlagConditionsMet
	{
		get
		{
			if (_minFlagConditionMet)
			{
				return _maxFlagConditionMet;
			}
			return false;
		}
	}

	public bool areItemConditionsMet => _requiredItemConditionsMet;

	public bool areConditionsMet
	{
		get
		{
			if (_minFlagConditionMet && _maxFlagConditionMet)
			{
				return _requiredItemConditionsMet;
			}
			return false;
		}
	}

	public TutorialRef(VariableBookData.Tutorial data)
	{
		_type = data.type;
		_minZone = ((data.minZone != null) ? int.Parse(data.minZone) : (-1));
		_minNode = ((data.minNode != null) ? int.Parse(data.minNode) : (-1));
		_maxZone = ((data.maxZone != null) ? int.Parse(data.maxZone) : (-1));
		_maxNode = ((data.maxNode != null) ? int.Parse(data.maxNode) : (-1));
		_requiredItemType = ((data.requiredItemType != null) ? int.Parse(data.requiredItemType) : (-1));
		_requiredItemSubtype = ((data.requiredItemSubtype != null) ? int.Parse(data.requiredItemSubtype) : (-1));
		_requiredItemQty = ((data.requiredItemQty != null) ? int.Parse(data.requiredItemQty) : 0);
	}

	public bool Equals(TutorialRef other)
	{
		if (other == null)
		{
			return false;
		}
		return type.Equals(other.type);
	}

	public int CompareTo(TutorialRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return type.CompareTo(other.type);
	}
}
