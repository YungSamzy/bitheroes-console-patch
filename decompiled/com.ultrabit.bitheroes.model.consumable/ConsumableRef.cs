using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.consumable;

[DebuggerDisplay("{name} (ConsumableRef)")]
public class ConsumableRef : ItemRef, IEquatable<ConsumableRef>, IComparable<ConsumableRef>
{
	public const int CONSUMABLE_TYPE_KEY = 1;

	public const int CONSUMABLE_TYPE_HEALING_POTION = 2;

	public const int CONSUMABLE_TYPE_REVIVE_POTION = 3;

	public const int CONSUMABLE_TYPE_LOOT = 4;

	public const int CONSUMABLE_TYPE_MODIFIERS = 5;

	public const int CONSUMABLE_TYPE_SKIN_COLOR = 6;

	public const int CONSUMABLE_TYPE_STAT_RESET = 7;

	public const int CONSUMABLE_TYPE_SUPER_POTION = 8;

	public const int CONSUMABLE_TYPE_CUSTOMIZE = 9;

	public const int CONSUMABLE_TYPE_CHANGENAME = 10;

	public const int CONSUMABLE_TYPE_CUSTOMCONSUM = 12;

	public const int CONSUMABLE_TYPE_VIPGOR = 15;

	public const string TUTORIAL_SCHEMATIC = "tutorial_schematic";

	private static Dictionary<string, int> CONSUMABLE_TYPES = new Dictionary<string, int>
	{
		["key"] = 1,
		["healingpotion"] = 2,
		["revivepotion"] = 3,
		["loot"] = 4,
		["modifiers"] = 5,
		["skincolor"] = 6,
		["statreset"] = 7,
		["superpotion"] = 8,
		["customize"] = 9,
		["changename"] = 10,
		["customconsum"] = 12,
		["vipgor"] = 15
	};

	private List<GameModifier> _modifiers;

	private int _consumableType;

	private string _summary;

	private string _value;

	private int _currencyID;

	private bool _viewable;

	private bool _displayQty;

	private bool _displayCompare;

	private bool _displayUnlocked;

	private bool _forceConsume;

	private int _requiredZone;

	private int _eventRequired;

	private bool _hidden;

	private int _consumableItemType;

	public bool inventoryUsable
	{
		get
		{
			switch (consumableType)
			{
			case 4:
			case 5:
			case 6:
			case 7:
			case 9:
			case 10:
			case 12:
				return true;
			default:
				return false;
			}
		}
	}

	public int consumableType => _consumableType;

	public string summary => _summary;

	public string localizedSummary => Language.GetString(summary);

	public string value => _value;

	public int currencyID => _currencyID;

	public bool viewable => _viewable;

	public bool displayQty => _displayQty;

	public bool displayCompare => _displayCompare;

	public bool displayUnlocked => _displayUnlocked;

	public bool forceConsume => _forceConsume;

	public List<GameModifier> modifiers => _modifiers;

	public int requiredZone => _requiredZone;

	public int eventRequired => _eventRequired;

	public bool hidden => _hidden;

	public int consumableItemType => _consumableItemType;

	public ConsumableRef(int id, ConsumableBookData.Consumable data)
		: base(id, 4)
	{
		_consumableType = GetConsumableType(data.type);
		_summary = data.summary;
		_value = data.value;
		_currencyID = data.currencyID;
		_viewable = Util.GetBoolFromStringProperty(data.viewable, defaultValue: true);
		_displayQty = Util.GetBoolFromStringProperty(data.displayQty, defaultValue: true);
		_displayCompare = Util.GetBoolFromStringProperty(data.displayCompare, defaultValue: true);
		_displayUnlocked = Util.GetBoolFromStringProperty(data.displayUnlocked, defaultValue: true);
		_forceConsume = Util.GetBoolFromStringProperty(data.forceConsume);
		_requiredZone = Util.GetIntFromStringProperty(data.requiredZone);
		_eventRequired = Util.GetIntFromStringProperty(data.eventRequired);
		_hidden = Util.parseBoolean(data.hidden, defaultVal: false);
		_consumableItemType = ((data.consumableItemType != null) ? ItemRef.getItemType(data.consumableItemType) : 0);
		_modifiers = GameModifier.GetGameModifierFromData(data.modifiers, data.lstModifier);
	}

	public static int GetConsumableType(string type)
	{
		if (!CONSUMABLE_TYPES.ContainsKey(type.ToLowerInvariant()))
		{
			return -1;
		}
		return CONSUMABLE_TYPES[type.ToLowerInvariant()];
	}

	public static string getConsumableTypeName(int type)
	{
		return Language.GetString("consumable_type_" + type + "_name");
	}

	public bool Equals(ConsumableRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(ConsumableRef other)
	{
		if (other == null)
		{
			return -1;
		}
		int num = consumableType.CompareTo(other.consumableType);
		if (num == 0)
		{
			return base.id.CompareTo(other.id);
		}
		return num;
	}
}
