using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.date;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.model.xml.common;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.instance;

public class InstanceObjectRef : BaseRef
{
	public const int OBJECT_TYPE_NONE = 0;

	public const int OBJECT_TYPE_QUEST = 1;

	public const int OBJECT_TYPE_PVP = 2;

	public const int OBJECT_TYPE_LEADERBOARD = 3;

	public const int OBJECT_TYPE_PVP_STATUES = 4;

	public const int OBJECT_TYPE_CRAFT = 5;

	public const int OBJECT_TYPE_SHOP = 6;

	public const int OBJECT_TYPE_RAID = 7;

	public const int OBJECT_TYPE_DAILY_QUESTS = 8;

	public const int OBJECT_TYPE_FUSION = 9;

	public const int OBJECT_TYPE_MOUNTS = 10;

	public const int OBJECT_TYPE_RIFTS = 11;

	public const int OBJECT_TYPE_RUNE = 12;

	public const int OBJECT_TYPE_GAUNTLET = 13;

	public const int OBJECT_TYPE_FISHING = 14;

	public const int OBJECT_TYPE_INSTANCE = 15;

	public const int OBJECT_TYPE_GUILD_HALL_COSMETIC = 16;

	public const int OBJECT_TYPE_DIALOG = 17;

	public const int OBJECT_TYPE_FAMILIAR_STABLE = 18;

	public const int OBJECT_TYPE_ENCHANTS = 19;

	public const int OBJECT_TYPE_INVASION = 20;

	public const int OBJECT_TYPE_BRAWL = 21;

	public const int OBJECT_TYPE_REWARD = 22;

	public const int OBJECT_TYPE_FISHING_STATUES = 23;

	public const int OBJECT_TYPE_FISHING_EVENTS = 24;

	public const int OBJECT_TYPE_FISHING_SHOP = 25;

	public const int OBJECT_TYPE_FISHING_MODE = 26;

	public const int OBJECT_TYPE_AUGMENTS = 27;

	public const int OBJECT_TYPE_PLAYER_VOTING = 28;

	public const int OBJECT_TYPE_ARMORY_SLOT = 29;

	public const int OBJECT_TYPE_PLAYER_ARMORY_SLOT = 30;

	public const int OBJECT_TYPE_MYTHIC_AUGMENT_CRAFT = 31;

	public const int OBJECT_TYPE_MYTHIC_ENCHANT_CRAFT = 32;

	public const int OBJECT_TYPE_MYTHIC_RUNE_CRAFT = 33;

	public const int OBJECT_TYPE_NPC_SETS = 34;

	public const int OBJECT_TYPE_ARMORY_FORGE = 35;

	public const int OBJECT_TYPE_NPC_SETS_DEAD = 36;

	public const int OBJECT_TYPE_VIPGOR = 37;

	public const int OBJECT_TYPE_UNITY_WEBGL = 38;

	public const int OBJECT_TYPE_EVENT_SHOP = 39;

	public const int OBJECT_TYPE_EVENT_SHOP_MATERIAL_EXCHANGE = 40;

	private static Dictionary<string, int> OBJECT_TYPES = new Dictionary<string, int>
	{
		["none"] = 0,
		["quest"] = 1,
		["pvp"] = 2,
		["leaderboard"] = 3,
		["pvpstatues"] = 4,
		["craft"] = 5,
		["shop"] = 6,
		["raid"] = 7,
		["dailyquests"] = 8,
		["fusion"] = 9,
		["mounts"] = 10,
		["rifts"] = 11,
		["rune"] = 12,
		["gauntlet"] = 13,
		["fishing"] = 14,
		["instance"] = 15,
		["guildhallcosmetic"] = 16,
		["dialog"] = 17,
		["familiarstable"] = 18,
		["enchants"] = 19,
		["invasion"] = 20,
		["brawl"] = 21,
		["reward"] = 22,
		["fishingstatues"] = 23,
		["fishingevents"] = 24,
		["fishingshop"] = 25,
		["fishingmode"] = 26,
		["augments"] = 27,
		["playervoting"] = 28,
		["armoryslot"] = 29,
		["player_armoryslot"] = 30,
		["mythic_augment"] = 31,
		["mythic_enchant"] = 32,
		["mythic_rune"] = 33,
		["npc_sets"] = 34,
		["armory_forge"] = 35,
		["npc_sets_dead"] = 36,
		["vipgorinstance"] = 37,
		["unity_webgl"] = 38,
		["eventshop"] = 39,
		["eventshopmaterialexchange"] = 40
	};

	private int _type;

	private string _definition;

	private string _value;

	private int _tileID;

	private bool _flipped;

	private bool _hidden;

	private int _speed;

	private int _order;

	private AssetDisplayRef _displayRef;

	private Vector2 _offset;

	private string _dialog;

	private string _blockedDialog;

	private DateRef _dateRef;

	private List<ItemRewardRef> _rewards;

	private List<int> _collision;

	private List<InstanceActionRef> _actions;

	private bool _display;

	public int type => _type;

	public string value => _value;

	public string definition => _definition;

	public int tileID => _tileID;

	public bool flipped => _flipped;

	public bool hidden => _hidden;

	public int speed => _speed;

	public int order => _order;

	public Vector2 offset => _offset;

	public List<int> collision => _collision;

	public AssetDisplayRef displayRef => _displayRef;

	public bool display => _display;

	public InstanceObjectRef(int id, AssetDisplayData objectData)
		: base(id)
	{
		_type = getObjectType(objectData.type);
		_value = objectData.value;
		_tileID = objectData.tile;
		_flipped = Util.parseBoolean(objectData.flipped, defaultVal: false);
		_hidden = Util.parseBoolean(objectData.hidden, defaultVal: false);
		_speed = Util.GetIntFromStringProperty(objectData.speed, 250);
		_order = objectData.order;
		_definition = objectData.definition;
		_displayRef = new AssetDisplayRef(objectData, AssetURL.INSTANCE_OBJECT);
		if (_displayRef.asset == null && _displayRef.equipment == null && (_displayRef.itemID < 0 || _displayRef.itemType < 0))
		{
			_displayRef = null;
		}
		_offset = Util.pointFromString(objectData.offset);
		_dialog = objectData.dialog;
		_blockedDialog = objectData.blockedDialog;
		if (objectData.startDate != null && objectData.endDate != null)
		{
			_dateRef = new DateRef(objectData.startDate, objectData.endDate);
		}
		_collision = Util.GetIntListFromStringProperty(objectData.collision);
		_actions = new List<InstanceActionRef>();
		for (int i = 0; i < objectData.lstAction.Count; i++)
		{
			_actions.Add(new InstanceActionRef(i, objectData.lstAction[i]));
		}
		_rewards = VariableBook.GetItemRewards(objectData.rewards);
		_display = Util.parseBoolean(objectData.display, defaultVal: false);
		LoadDetails(objectData);
	}

	public ItemRewardRef getAvailableReward()
	{
		foreach (ItemRewardRef reward in _rewards)
		{
			if (reward != null && reward.isAvailable)
			{
				return reward;
			}
		}
		return null;
	}

	public InstanceActionRef getAction(int index = 0)
	{
		if (index < 0 || index >= _actions.Count)
		{
			return null;
		}
		return _actions[index];
	}

	public bool getClickable()
	{
		int num = _type;
		if (num == 0 || num == 16)
		{
			return false;
		}
		return true;
	}

	public DialogRef getDialogRef()
	{
		if (_dialog == null || _dialog.Length <= 0)
		{
			return null;
		}
		return DialogBook.Lookup(_dialog);
	}

	public DialogRef getBlockedDialogRef()
	{
		if (_blockedDialog == null || _blockedDialog.Length <= 0)
		{
			return null;
		}
		return DialogBook.Lookup(_blockedDialog);
	}

	public bool getActive()
	{
		if (_dateRef != null)
		{
			return _dateRef.getActive();
		}
		return true;
	}

	public static int getObjectType(string type)
	{
		if (!OBJECT_TYPES.ContainsKey(type))
		{
			D.LogError("InstanceObjectRef::getObjectType " + type + " not found");
			return -1;
		}
		return OBJECT_TYPES[type.ToLowerInvariant()];
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
