using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.probability;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.wallet;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.ui.currency;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.item;

[DebuggerDisplay("{name} (ItemRef)")]
public class ItemRef : BaseRef, IEquatable<ItemRef>, IComparable<ItemRef>
{
	public enum BindType
	{
		hero,
		player
	}

	public const int ITEM_TYPE_NONE = 0;

	public const int ITEM_TYPE_EQUIPMENT = 1;

	public const int ITEM_TYPE_MATERIAL = 2;

	public const int ITEM_TYPE_CURRENCY = 3;

	public const int ITEM_TYPE_CONSUMABLE = 4;

	public const int ITEM_TYPE_SERVICE = 5;

	public const int ITEM_TYPE_FAMILIAR = 6;

	public const int ITEM_TYPE_FUSION = 7;

	public const int ITEM_TYPE_MOUNT = 8;

	public const int ITEM_TYPE_RUNE = 9;

	public const int ITEM_TYPE_GUILD = 10;

	public const int ITEM_TYPE_ENCHANT = 11;

	public const int ITEM_TYPE_FISH = 12;

	public const int ITEM_TYPE_BAIT = 13;

	public const int ITEM_TYPE_BOBBER = 14;

	public const int ITEM_TYPE_AUGMENT = 15;

	public const int ITEM_TYPE_ARMORY = 16;

	public const int ITEM_TYPE_ARMORY_MOUNT = 17;

	public const int ITEM_TYPE_ARMORY_ENCHANT = 18;

	public const int ITEM_TYPE_ARMORY_RUNE = 19;

	public const int ITEM_TYPE_COUNT = 22;

	private static Dictionary<string, int> ITEM_TYPES = new Dictionary<string, int>
	{
		["none"] = 0,
		["equipment"] = 1,
		["armory"] = 16,
		["material"] = 2,
		["currency"] = 3,
		["consumable"] = 4,
		["service"] = 5,
		["familiar"] = 6,
		["fusion"] = 7,
		["mount"] = 8,
		["rune"] = 9,
		["guild"] = 10,
		["enchant"] = 11,
		["fish"] = 12,
		["bait"] = 13,
		["bobber"] = 14,
		["augment"] = 15
	};

	private int _type;

	protected RarityRef _rarityRef;

	protected ProbabilityRef _probabilityRef;

	protected int _costGold;

	protected int _costCredits;

	protected int _sellGold;

	protected int _sellCredits;

	protected int _tier;

	protected bool _exchangeable;

	protected bool _unique;

	protected bool _allowQty;

	protected bool _lootDisplay;

	protected bool _cosmetic;

	protected bool _gacha;

	protected List<ItemUpgradeRef> _upgrades;

	protected List<ItemReforgeRef> _reforges;

	protected bool _assetsOverride;

	protected int _assetsSourceID;

	protected bool _attachSource;

	protected int _setSourceID;

	protected int _tutorialID;

	protected int _rank;

	protected int _ranks;

	protected ItemRef _assetsSource;

	public List<CraftTradeRef> trades = new List<CraftTradeRef>();

	protected BindType _bindType;

	private int overrideType = -1;

	private bool _isNFT;

	public bool isExchangeable
	{
		get
		{
			if (!rarityRef.exchangeable)
			{
				return false;
			}
			return exchangeable;
		}
	}

	public override string name
	{
		get
		{
			if (base.name == null || base.name.Length <= 0)
			{
				if (!(assetsSource != null))
				{
					return "";
				}
				return assetsSource.name;
			}
			return base.name;
		}
	}

	public override string desc
	{
		get
		{
			if (base.desc == null || base.desc.Length <= 0)
			{
				if (!(assetsSource != null))
				{
					return "";
				}
				return assetsSource.desc;
			}
			return base.desc;
		}
	}

	public string coloredName
	{
		get
		{
			if (rarityRef != null)
			{
				return rarityRef.ConvertString(Util.FirstCharToUpper(Language.GetString(name)));
			}
			return Util.FirstCharToUpper(Language.GetString(name));
		}
	}

	public int subtype
	{
		get
		{
			switch (itemType)
			{
			case 1:
			case 16:
				return (this as EquipmentRef).equipmentType;
			case 9:
				return (this as RuneRef).runeType;
			case 4:
				return (this as ConsumableRef).consumableType;
			case 15:
				return (this as AugmentRef).typeRef.id;
			default:
				return 0;
			}
		}
	}

	public int itemType
	{
		get
		{
			if (overrideType == -1)
			{
				return _type;
			}
			return overrideType;
		}
	}

	public int costGold => Mathf.RoundToInt((float)_costGold * getCostMult(GameData.instance.PROJECT.character.shopRotationID));

	public int costCredits => Mathf.RoundToInt((float)_costCredits * getCostMult(GameData.instance.PROJECT.character.shopRotationID));

	public int sellGold => _sellGold;

	public int sellCredits => _sellCredits;

	public int tier
	{
		get
		{
			if (!(assetsSource != null))
			{
				return _tier;
			}
			return assetsSource.tier;
		}
	}

	public bool exchangeable
	{
		get
		{
			if (!rarityRef.exchangeable)
			{
				return false;
			}
			return _exchangeable;
		}
	}

	public bool unique => _unique;

	public bool gacha => _gacha;

	public bool allowQty => _allowQty;

	public bool lootDisplay => _lootDisplay;

	public bool cosmetic => _cosmetic;

	public List<ItemUpgradeRef> upgrades => _upgrades;

	public List<ItemReforgeRef> reforges => _reforges;

	public bool assetsOverride => _assetsOverride;

	public int assetsSourceID => _assetsSourceID;

	public bool attachSource => _attachSource;

	public int setSourceID => _setSourceID;

	public int tutorialID => _tutorialID;

	public RarityRef rarityRef
	{
		get
		{
			if (_rarityRef == null)
			{
				if (!(assetsSource != null))
				{
					return null;
				}
				return assetsSource.rarityRef;
			}
			return _rarityRef;
		}
	}

	public ProbabilityRef probabilityRef => _probabilityRef;

	public int costGoldRaw => _costGold;

	public int costCreditsRaw => _costCredits;

	public int ranks => _ranks;

	public int rank => _rank;

	public ItemRef assetsSource => _assetsSource;

	public override string icon
	{
		get
		{
			if (base.icon != null)
			{
				return base.icon;
			}
			if (assetsSource != null)
			{
				return assetsSource.icon;
			}
			return null;
		}
	}

	public int rarity => rarityRef.id;

	public ItemRef originSource
	{
		get
		{
			ItemRef itemRef = null;
			while (true)
			{
				ItemRef itemRef2 = ((itemRef != null) ? itemRef.assetsSource : assetsSource);
				if (!(itemRef2 != null))
				{
					break;
				}
				itemRef = itemRef2;
			}
			return itemRef;
		}
	}

	public override string statName
	{
		get
		{
			if (base.statName != null)
			{
				return base.statName;
			}
			if (assetsSource != null && assetsSource.statName != null)
			{
				return assetsSource.statName;
			}
			return _rawName;
		}
	}

	public bool isNFT
	{
		get
		{
			return WalletBook.isNFT(this);
		}
		set
		{
			_isNFT = value;
			if (_isNFT)
			{
				_exchangeable = false;
			}
		}
	}

	public BindType bindType => _bindType;

	public string bindTypeString
	{
		get
		{
			if (_bindType == BindType.player)
			{
				return Language.GetString("ui_player_bound");
			}
			return Language.GetString("ui_hero_bound");
		}
	}

	public ItemRef(int id, int type)
		: base(id)
	{
		_type = type;
	}

	public void SetType(int type)
	{
		_type = type;
	}

	public override void LoadDetails(BaseBookItem xml)
	{
		base.LoadDetails(xml);
		if (xml.rarity != null)
		{
			_rarityRef = RarityBook.Lookup(xml.rarity);
		}
		if (xml.probability != null)
		{
			_probabilityRef = ProbabilityBook.Lookup(xml.probability);
		}
		_costGold = xml.costGold;
		_costCredits = xml.costCredits;
		_sellGold = xml.sellGold;
		_sellCredits = xml.sellCredits;
		_tier = xml.tier;
		_exchangeable = ((xml.exchangeableString != null) ? xml.exchangeableString.ToLower().Trim().Equals("true") : (_type == 1 || _type == 9 || _type == 11 || _type == 8 || _type == 15));
		_unique = Util.GetBoolFromStringProperty(xml.unique);
		_allowQty = Util.GetBoolFromStringProperty(xml.allowQty, defaultValue: true);
		_lootDisplay = Util.GetBoolFromStringProperty(xml.lootDisplay, defaultValue: true);
		_cosmetic = Util.GetBoolFromStringProperty(xml.cosmetic, defaultValue: true);
		_gacha = Util.GetBoolFromStringProperty(xml.gacha);
		Enum.TryParse<BindType>(Util.GetStringFromStringProperty(xml.itemOwner, "hero"), out _bindType);
		_assetsOverride = Util.GetBoolFromStringProperty(xml.assetsOverride);
		_assetsSourceID = xml.assetsSourceID;
		_attachSource = Util.GetBoolFromStringProperty(xml.attachSource);
		_setSourceID = xml.setSourceID;
		_tutorialID = Util.GetIntFromStringProperty(xml.tutorial, -1);
	}

	public void setRarityRef(RarityRef rarityRef)
	{
		_rarityRef = rarityRef;
	}

	public bool MatchesSubtype(int sub)
	{
		if (sub < 0)
		{
			return true;
		}
		if (sub == subtype)
		{
			return true;
		}
		return false;
	}

	public List<ItemRef> getReforgeableItems()
	{
		List<ItemRef> list = new List<ItemRef>();
		ItemRef itemRef = originSource;
		if (itemRef != null && rank > 0)
		{
			foreach (ItemRef rankUpgrade in itemRef.getRankUpgrades(rank))
			{
				if (!rankUpgrade.Equals(this))
				{
					list.Add(rankUpgrade);
				}
			}
		}
		if (_reforges != null)
		{
			foreach (ItemReforgeRef reforge in _reforges)
			{
				if (!reforge.itemRef.Equals(this))
				{
					list.Add(reforge.itemRef);
				}
			}
			return list;
		}
		return list;
	}

	public bool canUpgrade()
	{
		if (_upgrades == null || _upgrades.Count <= 0)
		{
			return false;
		}
		foreach (ItemUpgradeRef upgrade in _upgrades)
		{
			if (upgrade != null && upgrade.getUpgradeRef() != null && upgrade.getUpgradeRef().RequirementsMet())
			{
				return true;
			}
		}
		return false;
	}

	public ItemUpgradeRef getUpgradeRef(int id = 0)
	{
		if (id < 0 || id >= _upgrades.Count)
		{
			return null;
		}
		return _upgrades[id];
	}

	public List<ItemRef> getRankUpgrades(int rank)
	{
		List<ItemRef> list = new List<ItemRef>();
		if (_upgrades != null)
		{
			foreach (ItemUpgradeRef upgrade in _upgrades)
			{
				if (upgrade.getUpgradeItemRef()._rank == rank)
				{
					list.Add(upgrade.getUpgradeItemRef());
				}
				new List<ItemRef>();
				foreach (ItemRef rankUpgrade in upgrade.getUpgradeItemRef().getRankUpgrades(rank))
				{
					list.Add(rankUpgrade);
				}
			}
			return list;
		}
		return list;
	}

	public bool isHidden()
	{
		if (_type != 6)
		{
			return false;
		}
		return !GameData.instance.PROJECT.character.inventory.hasOwnedItem(this);
	}

	public void updateRanks(int rank, int ranks)
	{
		_rank = rank;
		_ranks = ranks;
		if (_assetsSource != null)
		{
			_assetsSource.updateRanks(_assetsSource.rank, ranks);
		}
	}

	public void UpdateAssetsSource(ItemRef assetsSource = null)
	{
		if (assetsSource == null)
		{
			if (assetsSourceID > 0)
			{
				_assetsSource = ItemBook.Lookup(assetsSourceID, _type);
			}
		}
		else
		{
			_assetsSource = assetsSource;
		}
	}

	public bool isViewable()
	{
		int type = _type;
		if (type == 3 || (uint)(type - 5) <= 2u)
		{
			return false;
		}
		return true;
	}

	public bool hasContents()
	{
		if (_type == 4)
		{
			ConsumableRef consumableRef = this as ConsumableRef;
			if (consumableRef.consumableType == 4)
			{
				return consumableRef.viewable;
			}
			return false;
		}
		return false;
	}

	public int getItemTooltipType(Character character = null)
	{
		switch (itemType)
		{
		case 1:
		{
			EquipmentRef equipmentRef = this as EquipmentRef;
			if (character != null && character.equipment.getEquipmentSlot(Equipment.getAvailableSlot(equipmentRef.equipmentType)) == equipmentRef)
			{
				return 2;
			}
			return 1;
		}
		case 16:
		{
			EquipmentRef equipmentRef2 = this as EquipmentRef;
			if (equipmentRef2 == null)
			{
				return 2;
			}
			ArmoryRef armoryRef = ArmoryRef.EquipmentRefToArmoryRef(equipmentRef2);
			if (character != null && character.armory != null && character.armory.currentArmoryEquipmentSlot != null && character.armory.currentArmoryEquipmentSlot.GetArmoryEquipmentSlot(Equipment.getAvailableSlot(armoryRef.equipmentType)) == armoryRef)
			{
				return 2;
			}
			return 1;
		}
		case 4:
			if (ConsumableBook.Lookup(base.id).inventoryUsable)
			{
				return 6;
			}
			break;
		case 11:
		case 15:
		case 18:
			return 14;
		case 8:
			return 16;
		case 17:
			return 16;
		case 6:
			return 7;
		case 9:
		case 19:
			return (this as RuneRef).runeAction;
		}
		if (trades.Count > 0 && GameData.instance.PROJECT.battle == null && GameData.instance.PROJECT.dungeon == null)
		{
			return 15;
		}
		return 0;
	}

	public void OverrideItemType(int newType)
	{
		overrideType = newType;
	}

	public int getCost(int currencyID, int qty = 1)
	{
		switch (currencyID)
		{
		case 1:
			return costGold * qty;
		case 2:
			return costCredits * qty;
		case 7:
		{
			GuildShopRef guildShopRef = GuildShopBook.LookupItem(this);
			if (guildShopRef != null)
			{
				return guildShopRef.costHonor * qty;
			}
			return 0;
		}
		default:
			return 0;
		}
	}

	public List<object> getCostClasses()
	{
		int num = ((costCredits <= 0) ? 1 : 2);
		List<object> list = new List<object>();
		switch (num)
		{
		case 2:
		{
			CurrencyCreditsTile item2 = new CurrencyCreditsTile();
			list.Add(item2);
			break;
		}
		case 1:
		{
			CurrencyGoldTile item = new CurrencyGoldTile();
			list.Add(item);
			break;
		}
		}
		return list;
	}

	public int[] getCostClassesArray()
	{
		int num = ((costCredits <= 0) ? 1 : 2);
		List<int> list = new List<int>();
		switch (num)
		{
		case 2:
			list.Add(0);
			break;
		case 1:
			list.Add(1);
			break;
		}
		return list.ToArray();
	}

	public float getCostMult(int rotation)
	{
		return ShopBook.GetItemSaleRef(this, rotation)?.mult ?? 1f;
	}

	public bool allowPurchase()
	{
		if (!getPurchasable())
		{
			return false;
		}
		if (unique && GameData.instance.PROJECT.character.inventory.hasOwnedItem(this))
		{
			return false;
		}
		return true;
	}

	public bool getPurchasable()
	{
		if (costGold > 0 || costCredits > 0 || GuildShopBook.LookupItem(this) != null)
		{
			return true;
		}
		if (getPaymentData() != null)
		{
			return true;
		}
		return false;
	}

	public PaymentRef getPaymentRef()
	{
		PaymentRef firstPaymentByItem = PaymentBook.GetFirstPaymentByItem(this);
		if (firstPaymentByItem != null)
		{
			return firstPaymentByItem;
		}
		return null;
	}

	public PaymentRef getPaymentRefForBooster()
	{
		PaymentRef firstPaymentByItemForBooster = PaymentBook.GetFirstPaymentByItemForBooster(this);
		if (firstPaymentByItemForBooster != null)
		{
			return firstPaymentByItemForBooster;
		}
		return null;
	}

	public PaymentData getPaymentData()
	{
		return PaymentBook.GetPaymentRefData(getPaymentRef());
	}

	public void addTrade(CraftTradeRef trade)
	{
		trades.Add(trade);
	}

	public static int getItemType(string type)
	{
		if (type != null && ITEM_TYPES.ContainsKey(type))
		{
			return ITEM_TYPES[type.ToLowerInvariant()];
		}
		D.LogError("ItemRef::getItemType " + type);
		return -1;
	}

	public static string getItemLink(int type)
	{
		foreach (KeyValuePair<string, int> iTEM_TYPE in ITEM_TYPES)
		{
			string key = iTEM_TYPE.Key;
			if (ITEM_TYPES[iTEM_TYPE.Key] == type)
			{
				return key;
			}
		}
		return "";
	}

	public static string GetItemName(int type, int subtype = -1)
	{
		if (subtype < 0 || type <= 0)
		{
			return Language.GetString("item_type_" + type + "_name");
		}
		switch (type)
		{
		case 1:
		case 16:
			return EquipmentRef.getEquipmentTypeName(subtype);
		case 9:
			return RuneRef.getRuneTypeName(subtype);
		case 18:
			return Language.GetString("item_type_" + 11 + "_name");
		default:
			return Language.GetString("item_type_" + type + "_name");
		}
	}

	public static string GetItemNamePlural(int type, int subtype = -1)
	{
		if (subtype < 0 || type <= 0)
		{
			return Language.GetString("item_type_" + type + "_plural_name");
		}
		if (type == 1 || type == 16)
		{
			return EquipmentRef.GetEquipmentTypeNamePlural(subtype);
		}
		return Language.GetString("item_type_" + type + "_plural_name");
	}

	public static string getItemColorHex(ItemRef itemRef)
	{
		if (itemRef.itemType == 3)
		{
			switch (itemRef.id)
			{
			case 1:
				return "FFE96A";
			case 2:
				return "AEE5FF";
			}
		}
		return itemRef.rarityRef.textColor;
	}

	public static string getItemSoundLink(ItemRef itemRef)
	{
		if (itemRef.itemType == 3)
		{
			switch (itemRef.id)
			{
			case 1:
				return "gold";
			case 2:
				return "credits";
			}
		}
		return null;
	}

	public static bool listHasItem(List<ItemRef> items, ItemRef itemRef)
	{
		if (items == null || itemRef == null)
		{
			return false;
		}
		foreach (ItemRef item in items)
		{
			if (item == itemRef)
			{
				return true;
			}
		}
		return false;
	}

	public static List<ItemRef> listFromSFSObject(ISFSObject sfsob, string constant)
	{
		if (!sfsob.ContainsKey(constant))
		{
			return null;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray(constant);
		List<ItemRef> list = new List<ItemRef>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ItemRef item = fromSFSObject(sFSArray.GetSFSObject(i));
			list.Add(item);
		}
		return list;
	}

	public static ItemRef fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("ite0"))
		{
			return null;
		}
		int @int = sfsob.GetInt("ite0");
		int int2 = sfsob.GetInt("ite1");
		return ItemBook.Lookup(@int, int2);
	}

	public static List<ItemData> listToItemDataList(List<ItemRef> items)
	{
		List<ItemData> list = new List<ItemData>();
		foreach (ItemRef item in items)
		{
			list.Add(new ItemData(item, 1));
		}
		return list;
	}

	public static SFSObject listToSFSObject(SFSObject sfsob, List<ItemRef> items, string constant)
	{
		ISFSArray iSFSArray = new SFSArray();
		for (int i = 0; i < items.Count; i++)
		{
			ItemRef itemRef = items[i];
			iSFSArray.AddSFSObject(itemRef.toSFSObject(new SFSObject()));
		}
		sfsob.PutSFSArray(constant, iSFSArray);
		return sfsob;
	}

	public static List<ItemRef> sortList(List<ItemRef> items)
	{
		List<object> list = new List<object>();
		foreach (ItemRef item2 in items)
		{
			list.Add(item2);
		}
		List<object> list2 = Util.SortVector(list, new string[3] { "rarity", "itemType", "id" });
		items.Clear();
		foreach (ItemRef item3 in list2)
		{
			items.Add(item3);
		}
		return items;
	}

	public static List<ItemRef> addItems(List<ItemRef> source, List<ItemRef> add)
	{
		if (add == null || source == null)
		{
			return source;
		}
		foreach (ItemRef item in add)
		{
			source.Add(item);
		}
		return source;
	}

	public SFSObject toSFSObject(SFSObject sfsob)
	{
		sfsob.PutInt("ite0", base.id);
		sfsob.PutInt("ite1", _type);
		return sfsob;
	}

	public override bool Equals(object obj)
	{
		ItemRef itemRef = (ItemRef)obj;
		if (itemRef != null)
		{
			if (itemRef.id == base.id)
			{
				return itemRef.itemType == itemType;
			}
			return false;
		}
		return false;
	}

	public override Sprite GetSpriteIcon()
	{
		if (icon == null)
		{
			D.LogError("nacho", "ItemRef::GetSpriteIcon:: null icon for " + base.id + " and type: " + itemType);
			return null;
		}
		return GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.GetAssetType(this, icon: true), icon);
	}

	public Sprite GetSpriteIcon(out bool isPrefab)
	{
		isPrefab = false;
		if (icon == null)
		{
			D.LogError("ItemRef::GetSpriteIcon:: null icon for " + base.id + " and type: " + itemType);
			return null;
		}
		return GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.GetAssetType(this, icon: true), icon, out isPrefab);
	}

	public GameObject GetPrefab()
	{
		return GameData.instance.main.assetLoader.GetGameObjectAsset(AssetURL.GetAssetType(this, icon: true), icon);
	}

	public static bool operator ==(ItemRef itemA, ItemRef itemB)
	{
		return itemA?.Equals(itemB) ?? ((object)itemB == null);
	}

	public static bool operator !=(ItemRef itemA, ItemRef itemB)
	{
		return !(itemA == itemB);
	}

	public bool hasUpgrade()
	{
		if (_upgrades == null || _upgrades.Count <= 0)
		{
			return false;
		}
		return true;
	}

	public override int GetHashCode()
	{
		return -331038658 + _type.GetHashCode();
	}

	public bool Equals(ItemRef other)
	{
		if (other == null)
		{
			return false;
		}
		if (base.id.Equals(other.id))
		{
			return itemType.Equals(other.itemType);
		}
		return false;
	}

	public int CompareTo(ItemRef other)
	{
		if (other == null)
		{
			return -1;
		}
		int num = itemType.CompareTo(other.itemType);
		if (num == 0)
		{
			return base.id.CompareTo(other.itemType);
		}
		return num;
	}
}
