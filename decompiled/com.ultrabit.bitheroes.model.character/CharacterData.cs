using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.adgor;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.daily;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.server;
using com.ultrabit.bitheroes.ui.character;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.character;

public class CharacterData : IEquatable<CharacterData>
{
	private int _charID;

	private int _playerID;

	private string _name;

	private CharacterPuppetInfoDefault _puppetInfoDefault;

	private CharacterPuppetInfoIMXG0 _puppetInfoIMXG0;

	private string _nftToken;

	private int _nftState;

	private string _customconsum;

	private bool _hasVipgor;

	private int _level;

	private int _power;

	private int _stamina;

	private int _agility;

	private int _zoneCompleted;

	private Runes _runes;

	private Enchants _enchants;

	private CharacterGuildInfo _guildInfo;

	private ArmoryEquipment _armoryEquipment;

	private Armory _armory;

	private Augments _augments;

	private string _herotag;

	private bool _nameHasChanged;

	private long _loginMilliseconds;

	private long _loginStartMilliseconds;

	public Armory armory => _armory;

	public string parsedName => Util.ParseName(_name, (_guildInfo != null) ? _guildInfo.initials : null);

	public int charID => _charID;

	public int playerID => _playerID;

	public string herotag => _herotag;

	public bool nameHasChanged => _nameHasChanged;

	public string name => _name;

	public string nameAndHeroTag => name + "#" + herotag;

	public string heroType
	{
		get
		{
			if (_puppetInfoIMXG0 == null)
			{
				return "Basic Hero";
			}
			return "hero #" + _puppetInfoIMXG0.name;
		}
	}

	public CharacterPuppet.Gender gender
	{
		get
		{
			if (_puppetInfoIMXG0 != null)
			{
				return _puppetInfoIMXG0.gender;
			}
			return _puppetInfoDefault.gender;
		}
	}

	public int hairID
	{
		get
		{
			if (_puppetInfoDefault != null)
			{
				return _puppetInfoDefault.hairID;
			}
			return 17;
		}
	}

	public int hairColorID
	{
		get
		{
			if (_puppetInfoDefault != null)
			{
				return _puppetInfoDefault.hairColorID;
			}
			return 11;
		}
	}

	public int skinColorID
	{
		get
		{
			if (_puppetInfoDefault != null)
			{
				return _puppetInfoDefault.skinColorID;
			}
			return 6;
		}
	}

	public int level => _level;

	public int power => _power;

	public int stamina => _stamina;

	public int agility => _agility;

	public int zoneCompleted => _zoneCompleted;

	public int tier => Character.getTier(_zoneCompleted);

	public bool showHelm
	{
		get
		{
			if (_puppetInfoIMXG0 != null)
			{
				return _puppetInfoIMXG0.showHelm;
			}
			return _puppetInfoDefault.showHelm;
		}
	}

	public bool showMount
	{
		get
		{
			if (_puppetInfoIMXG0 != null)
			{
				return _puppetInfoIMXG0.showMount;
			}
			return _puppetInfoDefault.showMount;
		}
		set
		{
			if (_puppetInfoIMXG0 != null)
			{
				_puppetInfoIMXG0.showMount = value;
			}
			else
			{
				_puppetInfoDefault.showMount = value;
			}
		}
	}

	public bool showBody
	{
		get
		{
			if (_puppetInfoIMXG0 != null)
			{
				return _puppetInfoIMXG0.showBody;
			}
			return _puppetInfoDefault.showBody;
		}
		set
		{
			if (_puppetInfoIMXG0 != null)
			{
				_puppetInfoIMXG0.showBody = value;
			}
			else
			{
				_puppetInfoDefault.showBody = value;
			}
		}
	}

	public bool showAccessory
	{
		get
		{
			if (_puppetInfoIMXG0 != null)
			{
				return _puppetInfoIMXG0.showAccessory;
			}
			return _puppetInfoDefault.showAccessory;
		}
		set
		{
			if (_puppetInfoIMXG0 != null)
			{
				_puppetInfoIMXG0.showAccessory = value;
			}
			else
			{
				_puppetInfoDefault.showAccessory = value;
			}
		}
	}

	public int totalPower => getTotalPower();

	public int totalStamina => getTotalStamina();

	public int totalAgility => getTotalAgility();

	public Equipment equipment
	{
		get
		{
			if (_puppetInfoIMXG0 != null)
			{
				return _puppetInfoIMXG0.equipment;
			}
			return _puppetInfoDefault.equipment;
		}
	}

	public Runes runes
	{
		get
		{
			return _runes;
		}
		set
		{
			_runes = value;
		}
	}

	public Enchants enchants => _enchants;

	public Mounts mounts
	{
		get
		{
			if (_puppetInfoIMXG0 != null)
			{
				return _puppetInfoIMXG0.mounts;
			}
			return _puppetInfoDefault.mounts;
		}
	}

	public CharacterGuildInfo guildInfo => _guildInfo;

	public long loginMilliseconds
	{
		get
		{
			return _loginMilliseconds + Mathf.RoundToInt((Time.realtimeSinceStartup - (float)_loginStartMilliseconds) * 1000f);
		}
		set
		{
			_loginMilliseconds = value;
			_loginStartMilliseconds = (long)Time.realtimeSinceStartup;
		}
	}

	public Augments augments => _augments;

	public bool hasVipgor => _hasVipgor;

	public string nameplateWithGuild
	{
		get
		{
			string nameplateColorString = _name;
			if (hasVipgor)
			{
				nameplateColorString = AdGor.GetNameplateColorString(_name);
			}
			return Util.ParseGuildInitials((_guildInfo != null) ? _guildInfo.initials : null) + " " + nameplateColorString;
		}
	}

	public bool isIMXG0 => _puppetInfoIMXG0 != null;

	public RarityRef nftRarity => RarityBook.Lookup(_puppetInfoIMXG0?.rarity);

	public Color nftRarityColor
	{
		get
		{
			ColorUtility.TryParseHtmlString("#" + RarityBook.Lookup(_puppetInfoIMXG0?.rarity).objectColor, out var color);
			return color;
		}
	}

	public bool nftIsAdFree
	{
		get
		{
			if (nftRarity != null)
			{
				return nftRarity != RarityBook.LookupID(1);
			}
			return false;
		}
	}

	public int nftGeneration
	{
		get
		{
			if (_puppetInfoIMXG0 == null)
			{
				return 0;
			}
			return _puppetInfoIMXG0.cardInfo.gen;
		}
	}

	public GameObject nftBackground
	{
		get
		{
			GameObject background = null;
			_puppetInfoIMXG0?.GetBackgroundAsset(out background);
			return background;
		}
	}

	public GameObject nftFrame
	{
		get
		{
			GameObject frame = null;
			_puppetInfoIMXG0?.GetFrameAsset(out frame);
			return frame;
		}
	}

	public GameObject nftFrameSimple
	{
		get
		{
			GameObject frame = null;
			_puppetInfoIMXG0?.GetFrameSimpleAsset(out frame);
			return frame;
		}
	}

	public GameObject nftFrameSeparator
	{
		get
		{
			GameObject frameSeparator = null;
			_puppetInfoIMXG0?.GetFrameSeparatorAsset(out frameSeparator);
			return frameSeparator;
		}
	}

	public GameObject nftFrameMenuInterface
	{
		get
		{
			GameObject frameMenuInterface = null;
			_puppetInfoIMXG0?.GetFrameMenuInterfaceAsset(out frameMenuInterface);
			return frameMenuInterface;
		}
	}

	public Character.NFTState nftState => (Character.NFTState)_nftState;

	public string nftToken => _nftToken;

	public string nftName => _puppetInfoIMXG0?.name;

	public CharacterData(CharacterPuppetInfoDefault puppetInfo, int charID, int playerID, string name, string herotag, bool nameHasChanged, string customconsum, int level, int power, int stamina, int agility, int zoneCompleted, Runes runes, Enchants enchants, CharacterGuildInfo guildInfo, ArmoryEquipment armoryEquipment, bool hasVipgor)
	{
		_puppetInfoDefault = puppetInfo;
		SetMainInfo(charID, playerID, name, herotag, nameHasChanged, customconsum, level, power, stamina, agility, zoneCompleted, runes, enchants, guildInfo, armoryEquipment, hasVipgor);
	}

	public CharacterData(CharacterPuppetInfoIMXG0 puppetInfo, int charID, int playerID, string name, string herotag, bool nameHasChanged, string customconsum, int level, int power, int stamina, int agility, int zoneCompleted, Runes runes, Enchants enchants, CharacterGuildInfo guildInfo, ArmoryEquipment armoryEquipment, bool hasVipgor, string nftToken, int nftState)
	{
		_puppetInfoIMXG0 = puppetInfo;
		_nftToken = nftToken;
		_nftState = nftState;
		SetMainInfo(charID, playerID, name, herotag, nameHasChanged, customconsum, level, power, stamina, agility, zoneCompleted, runes, enchants, guildInfo, armoryEquipment, hasVipgor);
	}

	private void SetMainInfo(int charID, int playerID, string name, string herotag, bool nameHasChanged, string customconsum, int level, int power, int stamina, int agility, int zoneCompleted, Runes runes, Enchants enchants, CharacterGuildInfo guildInfo, ArmoryEquipment armoryEquipment, bool hasVipgor)
	{
		_charID = charID;
		_playerID = playerID;
		_name = name;
		_herotag = herotag;
		_nameHasChanged = nameHasChanged;
		_customconsum = customconsum;
		_level = level;
		_power = power;
		_stamina = stamina;
		_agility = agility;
		_zoneCompleted = zoneCompleted;
		_runes = runes;
		_enchants = enchants;
		_guildInfo = guildInfo;
		_hasVipgor = hasVipgor;
		_armoryEquipment = armoryEquipment;
	}

	public CharacterData Duplicate()
	{
		Equipment equipment = new Equipment(this.equipment.equipmentSlots, this.equipment.cosmeticSlots);
		Mounts mounts = new Mounts(this.mounts.getMountEquippedUID(), this.mounts.cosmetic, this.mounts.mounts);
		CharacterData characterData;
		if (_puppetInfoIMXG0 != null)
		{
			CharacterPuppetInfoIMXG0 obj = (CharacterPuppetInfoIMXG0)_puppetInfoIMXG0.Clone();
			obj.equipment = equipment;
			obj.mounts = mounts;
			characterData = new CharacterData(obj, _charID, _playerID, _name, _herotag, _nameHasChanged, _customconsum, _level, _power, _stamina, _agility, _zoneCompleted, _runes, _enchants, _guildInfo, _armoryEquipment, _hasVipgor, _nftToken, _nftState);
		}
		else
		{
			CharacterPuppetInfoDefault obj2 = (CharacterPuppetInfoDefault)_puppetInfoDefault.Clone();
			obj2.equipment = equipment;
			obj2.mounts = mounts;
			characterData = new CharacterData(obj2, _charID, _playerID, _name, _herotag, _nameHasChanged, _customconsum, _level, _power, _stamina, _agility, _zoneCompleted, _runes, _enchants, _guildInfo, _armoryEquipment, _hasVipgor);
		}
		characterData.setArmory(_armory);
		return characterData;
	}

	public void SetMounts(Mounts mounts)
	{
		if (_puppetInfoDefault != null)
		{
			_puppetInfoDefault.mounts = mounts;
		}
		if (_puppetInfoIMXG0 != null)
		{
			_puppetInfoIMXG0.mounts = mounts;
		}
	}

	public void SetPower(int val)
	{
		_power = val;
	}

	public void SetStamina(int val)
	{
		_stamina = val;
	}

	public void SetAgility(int val)
	{
		_agility = val;
	}

	public int getHighestStat()
	{
		int num = getTotalPower();
		int num2 = getTotalStamina();
		int num3 = getTotalAgility();
		if (num >= num2 && num >= num3)
		{
			return 0;
		}
		if (num2 >= num && num2 >= num3)
		{
			return 1;
		}
		if (num3 >= num && num3 >= num2)
		{
			return 2;
		}
		return 0;
	}

	public void setArmory(Armory parmory)
	{
		_armory = parmory;
	}

	public void setAugments(Augments augments)
	{
		_augments = augments;
	}

	public int getTotalStats(bool forceCalc = false)
	{
		return getTotalPower(forceCalc) + getTotalStamina(forceCalc) + getTotalAgility(forceCalc);
	}

	public int getTotalPower(bool forceCalc = false)
	{
		return getTotalStat(0, forceCalc);
	}

	public int getTotalStamina(bool forceCalc = false)
	{
		return getTotalStat(1, forceCalc);
	}

	public int getTotalAgility(bool forceCalc = false)
	{
		return getTotalStat(2, forceCalc);
	}

	public int getTotalStat(int stat, bool forceCalc = false)
	{
		int num = 0;
		if (GameData.instance.PROJECT.character != null && charID == GameData.instance.PROJECT.character.id && !forceCalc)
		{
			return GameData.instance.PROJECT.character.getTotalStat(stat);
		}
		switch (stat)
		{
		case 0:
			num += _power + VariableBook.characterBase.power;
			break;
		case 1:
			num += _stamina + VariableBook.characterBase.stamina;
			break;
		case 2:
			num += _agility + VariableBook.characterBase.agility;
			break;
		}
		num += ((equipment != null) ? equipment.getStatTotal(stat) : 0);
		num += ((_enchants != null) ? _enchants.getStatTotal(stat) : 0);
		MountData mountData = mounts?.getMountEquipped();
		if (mountData != null)
		{
			num += mountData.getTotalStat(stat, tier);
		}
		return num;
	}

	public List<GameModifier> getModifiers()
	{
		List<GameModifier> list = new List<GameModifier>();
		list.AddRange(GameModifierHelper.GetGameModifierBase());
		list.AddRange(GameModifierHelper.GetGameModifierCharacterBase());
		list.AddRange(equipment.getModifiers());
		list = addRuneModifiers(list);
		list = addMountModifiers(list);
		list = addEnchantModifiers(list);
		if (GameModifier.listHasType(list, 68))
		{
			list = addRuneModifiers(list, all: false);
		}
		if (GameModifier.listHasType(list, 70))
		{
			list = addMountModifiers(list);
		}
		if (GameModifier.listHasType(list, 69))
		{
			list = addEnchantModifiers(list);
		}
		DailyBonusRef currentBonusRef = DailyBonusBook.GetCurrentBonusRef();
		if (currentBonusRef != null)
		{
			foreach (GameModifier modifier in currentBonusRef.modifiers)
			{
				list.Add(modifier);
			}
			return list;
		}
		return list;
	}

	private List<GameModifier> addMountModifiers(List<GameModifier> modifiers)
	{
		foreach (GameModifier gameModifier in mounts.getGameModifiers())
		{
			modifiers.Add(gameModifier);
		}
		return modifiers;
	}

	private List<GameModifier> addEnchantModifiers(List<GameModifier> modifiers)
	{
		foreach (GameModifier gameModifier in _enchants.getGameModifiers())
		{
			modifiers.Add(gameModifier);
		}
		return modifiers;
	}

	private List<GameModifier> addRuneModifiers(List<GameModifier> modifiers, bool all = true)
	{
		foreach (KeyValuePair<int, RuneRef> runeSlot in _runes.runeSlots)
		{
			if ((!all && runeSlot.Value.runeType != 1) || runeSlot.Value.modifiers == null)
			{
				continue;
			}
			foreach (GameModifier modifier in runeSlot.Value.modifiers)
			{
				modifiers.Add(modifier);
			}
		}
		return modifiers;
	}

	public List<AbilityRef> getAbilities()
	{
		Dictionary<int, EquipmentRef> equipmentSlots = equipment.equipmentSlots;
		Dictionary<int, RuneRef> runeSlots = _runes.runeSlots;
		List<AbilityRef> list = new List<AbilityRef>();
		foreach (KeyValuePair<int, EquipmentRef> item in equipmentSlots)
		{
			if (item.Value == null || item.Value.abilities == null)
			{
				continue;
			}
			foreach (AbilityRef ability in item.Value.abilities)
			{
				list.Add(ability);
			}
		}
		if (list.Count <= 0)
		{
			foreach (AbilityRef item2 in VariableBook.abilitiesDefault)
			{
				list.Add(item2);
			}
		}
		foreach (KeyValuePair<int, RuneRef> item3 in runeSlots)
		{
			if (item3.Value.abilities == null)
			{
				continue;
			}
			foreach (AbilityRef ability2 in item3.Value.abilities)
			{
				list.Add(ability2);
			}
		}
		foreach (EquipmentSetBonusRef equippedSetBonuse in equipment.getEquippedSetBonuses())
		{
			if (equippedSetBonuse == null || equippedSetBonuse.abilities == null)
			{
				continue;
			}
			foreach (AbilityRef ability3 in equippedSetBonuse.abilities)
			{
				list.Add(ability3);
			}
		}
		MountData mountEquipped = mounts.getMountEquipped();
		if (mountEquipped != null && mountEquipped.mountRef.abilities != null)
		{
			foreach (AbilityRef ability4 in mountEquipped.mountRef.abilities)
			{
				list.Add(ability4);
			}
		}
		List<AbilityRef> equippedModifierConditionWithAbilities = equipment.getEquippedModifierConditionWithAbilities();
		if (equippedModifierConditionWithAbilities != null && equippedModifierConditionWithAbilities.Count > 0)
		{
			foreach (AbilityRef item4 in equippedModifierConditionWithAbilities)
			{
				list.Add(item4);
			}
			return list;
		}
		return list;
	}

	public void setEquipment(Equipment equipment)
	{
		if (_puppetInfoDefault != null)
		{
			_puppetInfoDefault.equipment = equipment;
		}
		if (_puppetInfoIMXG0 != null)
		{
			_puppetInfoIMXG0.equipment = equipment;
		}
	}

	public CharacterDisplay toCharacterDisplay(float scale = 3f, bool displayMount = false, List<object> overrideEquipment = null, bool enableLoading = true)
	{
		CharacterPuppetInfo characterPuppetInfo = (CharacterPuppetInfo)(((object)_puppetInfoIMXG0) ?? ((object)_puppetInfoDefault));
		characterPuppetInfo.scale = scale;
		characterPuppetInfo.showMount &= displayMount;
		characterPuppetInfo.equipmentOverride = overrideEquipment;
		characterPuppetInfo.enableLoading = enableLoading;
		CharacterDisplay characterDisplay = GameData.instance.windowGenerator.GetCharacterDisplay(characterPuppetInfo);
		characterDisplay.SetCharacterDisplay(characterPuppetInfo);
		return characterDisplay;
	}

	public string getLoginText(bool online = false)
	{
		if (!online)
		{
			return Language.GetString("ui_offline_for_time", new string[1] { Util.TimeFormatShort((int)(loginMilliseconds / 1000)) });
		}
		return Language.GetString("ui_online_now");
	}

	public static CharacterData fromSFSObject(ISFSObject sfsob)
	{
		if (sfsob == null)
		{
			return null;
		}
		bool num = sfsob.ContainsKey("cha1") && sfsob.ContainsKey("cha4");
		bool flag = sfsob.ContainsKey("nft1") && sfsob.ContainsKey("nft2");
		if (!num && !flag)
		{
			return null;
		}
		int num2 = (sfsob.ContainsKey("cha1") ? sfsob.GetInt("cha1") : 0);
		int num3 = (sfsob.ContainsKey("pla3") ? sfsob.GetInt("pla3") : 0);
		string text = (sfsob.ContainsKey("cha2") ? sfsob.GetUtfString("cha2") : "");
		string text2 = (sfsob.ContainsKey("cha109") ? sfsob.GetUtfString("cha109") : "");
		bool flag2 = sfsob.ContainsKey(ServerConstants.CHARACTER_NAMEHASCHANGED) && sfsob.GetBool(ServerConstants.CHARACTER_NAMEHASCHANGED);
		string customconsum = (sfsob.ContainsKey("char107") ? sfsob.GetUtfString("char107") : "");
		bool flag3 = sfsob.ContainsKey("hasVipgor") && sfsob.GetBool("hasVipgor");
		int num4 = (sfsob.ContainsKey("cha4") ? sfsob.GetInt("cha4") : 0);
		int num5 = (sfsob.ContainsKey("cha6") ? sfsob.GetInt("cha6") : 0);
		int num6 = (sfsob.ContainsKey("cha7") ? sfsob.GetInt("cha7") : 0);
		int num7 = (sfsob.ContainsKey("cha8") ? sfsob.GetInt("cha8") : 0);
		int num8 = (sfsob.ContainsKey("cha94") ? sfsob.GetInt("cha94") : 0);
		Runes runes = Runes.fromSFSObject(sfsob);
		Enchants enchants = Enchants.fromSFSObject(sfsob);
		CharacterGuildInfo characterGuildInfo = CharacterGuildInfo.fromSFSObject(sfsob);
		Armory armory = Armory.FromSFSObject(sfsob);
		Augments augments = Augments.fromSFSObject(sfsob);
		CharacterPuppet.Gender genre = CharacterPuppet.ParseGenderFromString(sfsob.ContainsKey("cha12") ? sfsob.GetUtfString("cha12") : "M");
		bool flag4 = sfsob.ContainsKey("cha48") && sfsob.GetBool("cha48");
		bool flag5 = sfsob.ContainsKey("cha133") && sfsob.GetBool("cha133");
		bool flag6 = sfsob.ContainsKey("cha134") && sfsob.GetBool("cha134");
		bool flag7 = sfsob.ContainsKey("cha93") && sfsob.GetBool("cha93");
		Equipment equipment = Equipment.fromSFSObject(sfsob);
		Mounts mounts = Mounts.fromSFSObject(sfsob);
		int num9 = (sfsob.ContainsKey("cha20") ? sfsob.GetInt("cha20") : 0);
		int num10 = (sfsob.ContainsKey("cha21") ? sfsob.GetInt("cha21") : 0);
		int num11 = (sfsob.ContainsKey("cha22") ? sfsob.GetInt("cha22") : 0);
		string text3 = (sfsob.ContainsKey("nft1") ? sfsob.GetUtfString("nft1") : null);
		string text4 = (sfsob.ContainsKey("nft2") ? sfsob.GetUtfString("nft2") : null);
		int num12 = (sfsob.ContainsKey("nft3") ? sfsob.GetInt("nft3") : 0);
		Character.IMXG0Data iMXG0Data = null;
		if (!string.IsNullOrEmpty(text3))
		{
			iMXG0Data = JsonUtility.FromJson<Character.IMXG0Data>(text3);
		}
		CharacterData characterData = ((iMXG0Data == null) ? new CharacterData(new CharacterPuppetInfoDefault(genre, num9, num10, num11, 1f, 1f, equipment, mounts, flag4, flag7, flag5, flag6), num2, num3, text, text2, flag2, customconsum, num4, num5, num6, num7, num8, runes, enchants, characterGuildInfo, null, flag3) : new CharacterData(new CharacterPuppetInfoIMXG0(iMXG0Data.puppet, iMXG0Data.card, iMXG0Data.rarity, iMXG0Data.name, genre, 1f, 1f, equipment, mounts, flag4, flag7, flag5, flag6), num2, num3, text, text2, flag2, customconsum, num4, num5, num6, num7, num8, runes, enchants, characterGuildInfo, null, flag3, text4, num12));
		characterData.loginMilliseconds = (sfsob.ContainsKey("cha44") ? sfsob.GetLong("cha44") : 0);
		characterData.setArmory(armory);
		characterData.setAugments(augments);
		return characterData;
	}

	public bool Equals(CharacterData other)
	{
		if (other != null)
		{
			return charID.Equals(other.charID);
		}
		return false;
	}
}
