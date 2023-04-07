using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.armory.enchant;
using com.ultrabit.bitheroes.model.armory.rune;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.model.variable;
using Sfs2X.Entities.Data;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.armory;

public class ArmoryEquipment
{
	public const int SLOT_TYPE_MAINHAND = 0;

	public const int SLOT_TYPE_OFFHAND = 1;

	public const int SLOT_TYPE_HEAD = 2;

	public const int SLOT_TYPE_BODY = 3;

	public const int SLOT_TYPE_NECK = 4;

	public const int SLOT_TYPE_RING = 5;

	public const int SLOT_TYPE_ACCESSORY = 6;

	public const int SLOT_TYPE_PET = 7;

	public const int SLOT_TYPE_MOUNT = 8;

	public const int SLOTS = 8;

	public const string AE_ID = "id";

	public const string AE_POSITION = "position";

	public const string AE_NAME = "name";

	public const string AE_UNLOCKED = "unlocked";

	public const string AE_MOUNT = "mount";

	public const string AE_MOUNT_COSMETIC = "mountCosmetic";

	public const string AE_RUNES = "armoryRunes";

	public const string AE_ENCHANTS = "armoryEnchants";

	public const string AE_PRIVATE = "armoryPrivate";

	public const string AE_BATTLE_TYPE = "armoryBattleType";

	public const string AE_VALUE = "armoryEquipmentValue";

	public const string AE_REQUIRED_LEVEL = "armoryEquipmentRequiredLevel";

	public const string AE_CURRENCY = "ae_currency";

	public const int AE_CURRENCY_GOLD = 0;

	public const int AE_CURRENCY_DIAMONDS = 1;

	private Dictionary<int, ArmoryRef> _armorySlots;

	private Dictionary<int, ArmoryRef> _cosmeticSlots;

	private long _id;

	private uint _position;

	private long _mount;

	private long _mountCosmetic;

	private string _name;

	private bool _unlocked;

	private bool _private;

	private uint _battleType;

	private ArmoryRunes _runes;

	private ArmoryEnchants _enchants;

	private uint _value;

	private uint _currency;

	private uint _requiredLevel;

	public UnityEvent OnChange = new UnityEvent();

	public Dictionary<int, ArmoryRef> armorySlots => _armorySlots;

	public Dictionary<int, ArmoryRef> cosmeticSlots => _cosmeticSlots;

	public long id
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

	public uint position
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

	public long mount
	{
		get
		{
			return _mount;
		}
		set
		{
			_mount = value;
		}
	}

	public long mountCosmetic
	{
		get
		{
			return _mountCosmetic;
		}
		set
		{
			_mountCosmetic = value;
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

	public bool unlocked
	{
		get
		{
			return _unlocked;
		}
		set
		{
			_unlocked = value;
		}
	}

	public bool pprivate
	{
		get
		{
			return _private;
		}
		set
		{
			_private = value;
		}
	}

	public uint battleType
	{
		get
		{
			return _battleType;
		}
		set
		{
			_battleType = value;
		}
	}

	public ArmoryRunes runes => _runes;

	public ArmoryEnchants enchants => _enchants;

	public uint value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}

	public uint requiredLevel
	{
		get
		{
			return _requiredLevel;
		}
		set
		{
			_requiredLevel = value;
		}
	}

	public int currency
	{
		get
		{
			return (int)_currency;
		}
		set
		{
			_currency = (uint)value;
		}
	}

	public ArmoryEquipment(Dictionary<int, ArmoryRef> armorySlots = null, Dictionary<int, ArmoryRef> cosmeticSlots = null)
	{
		_armorySlots = ((armorySlots != null) ? armorySlots : new Dictionary<int, ArmoryRef>());
		_cosmeticSlots = ((cosmeticSlots != null) ? cosmeticSlots : new Dictionary<int, ArmoryRef>());
	}

	public void Broadcast()
	{
		OnChange?.Invoke();
	}

	public int GetArmoryEquipmentCount(ItemRef itemRef)
	{
		int num = 0;
		foreach (KeyValuePair<int, ArmoryRef> armorySlot in _armorySlots)
		{
			if (!(armorySlot.Value == null) && armorySlot.Value == itemRef)
			{
				num++;
			}
		}
		return num;
	}

	public bool CheckArmoryEquipmentState(Inventory inventory)
	{
		bool result = false;
		lock (_armorySlots)
		{
			foreach (KeyValuePair<int, ArmoryRef> armorySlot in _armorySlots)
			{
				ArmoryRef armoryRef = armorySlot.Value;
				if (armoryRef != null && inventory.getItem(armoryRef.id, 1) == null)
				{
					armoryRef = null;
					_armorySlots[armorySlot.Key] = null;
					result = true;
				}
			}
		}
		if (_runes != null && _runes.runeSlots != null)
		{
			foreach (int item in new List<int>(_runes.runeSlots.Keys))
			{
				bool flag = true;
				RuneRef runeRef = _runes.runeSlots[item];
				if (runeRef != null)
				{
					ItemData[] array = GameData.instance.PROJECT.character.runes.getChangeableArmoryRunes(item).ToArray();
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].id == runeRef.id)
						{
							flag = false;
							break;
						}
					}
				}
				if (runeRef != null && flag)
				{
					_runes.runeSlots[item] = null;
					CharacterDALC.instance.doArmoryRuneChange(null, item, (int)_id);
					result = true;
				}
			}
			return result;
		}
		return result;
	}

	public bool RemoveRuneIfEquipped(RuneRef runeRef)
	{
		bool result = false;
		if (_runes != null && _runes.runeSlots != null)
		{
			foreach (KeyValuePair<int, RuneRef> runeSlot in _runes.runeSlots)
			{
				RuneRef runeRef2 = runeSlot.Value;
				if (runeRef2 != null && runeRef2.id == runeRef.id)
				{
					_runes.runeSlots[runeSlot.Key] = null;
					return true;
				}
			}
			return result;
		}
		return result;
	}

	public void RemoveEnchantIfEquipped(EnchantData enchanData)
	{
		if (enchants == null || enchants.enchants == null)
		{
			return;
		}
		for (int i = 0; i < enchants.enchants.Count; i++)
		{
			EnchantData enchantData = enchants.enchants[i];
			if (enchantData != null && enchantData.uid == enchanData.uid)
			{
				enchants.enchants[i] = null;
				break;
			}
		}
	}

	public MountData GetMountDataEquipped()
	{
		MountData result = null;
		foreach (MountData mount in GameData.instance.PROJECT.character.mounts.mounts)
		{
			if (mount.uid == _mount)
			{
				return mount;
			}
		}
		return result;
	}

	public bool GetItemOriginEquipped(ItemRef itemRef)
	{
		if (itemRef == null)
		{
			return false;
		}
		foreach (KeyValuePair<int, ArmoryRef> armorySlot in _armorySlots)
		{
			if (!(armorySlot.Value == null))
			{
				ArmoryRef armoryRef = armorySlot.Value;
				if (armoryRef == itemRef || (armoryRef.itemType == itemRef.itemType && armoryRef.id == itemRef.id))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool GetItemSetEquipped(ItemRef itemRef)
	{
		if (itemRef == null)
		{
			return false;
		}
		foreach (KeyValuePair<int, ArmoryRef> armorySlot in _armorySlots)
		{
			if (!(armorySlot.Value == null))
			{
				ArmoryRef armoryRef = armorySlot.Value;
				if (armoryRef == itemRef || (armoryRef.itemType == itemRef.itemType && armoryRef.id == itemRef.id))
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetStatTotal(int stat)
	{
		int num = 0;
		foreach (KeyValuePair<int, ArmoryRef> armorySlot in _armorySlots)
		{
			if (!(armorySlot.Value == null))
			{
				switch (stat)
				{
				case 0:
					num += armorySlot.Value.power;
					break;
				case 1:
					num += armorySlot.Value.stamina;
					break;
				case 2:
					num += armorySlot.Value.agility;
					break;
				}
			}
		}
		return num;
	}

	public List<GameModifier> GetModifiers(bool checkSets = true)
	{
		List<GameModifier> list = new List<GameModifier>();
		foreach (KeyValuePair<int, ArmoryRef> armorySlot in _armorySlots)
		{
			if (armorySlot.Value == null)
			{
				continue;
			}
			foreach (GameModifier modifier in armorySlot.Value.modifiers)
			{
				list.Add(modifier);
			}
		}
		if (checkSets)
		{
			foreach (EquipmentSetBonusRef armoryEquippedSetBonuse in GetArmoryEquippedSetBonuses())
			{
				foreach (GameModifier modifier2 in armoryEquippedSetBonuse.modifiers)
				{
					list.Add(modifier2);
				}
			}
			return list;
		}
		return list;
	}

	public List<EquipmentSetRef> GetArmoryEquippedSets()
	{
		List<EquipmentSetRef> list = new List<EquipmentSetRef>();
		foreach (KeyValuePair<int, ArmoryRef> armorySlot in _armorySlots)
		{
			if (!(armorySlot.Value == null) && armorySlot.Value.equipmentSet != null && !EquipmentSetRef.listHasSet(list, armorySlot.Value.equipmentSet))
			{
				list.Add(armorySlot.Value.equipmentSet);
			}
		}
		return list;
	}

	public List<EquipmentSetBonusRef> GetArmoryEquippedSetBonuses()
	{
		List<EquipmentSetBonusRef> list = new List<EquipmentSetBonusRef>();
		foreach (EquipmentSetRef armoryEquippedSet in GetArmoryEquippedSets())
		{
			int armoryEquipmentSetCount = GetArmoryEquipmentSetCount(armoryEquippedSet);
			foreach (EquipmentSetBonusRef bonuse in armoryEquippedSet.bonuses)
			{
				if (bonuse.getBonusEnabled(armoryEquipmentSetCount))
				{
					list.Add(bonuse);
				}
			}
		}
		return list;
	}

	public int GetArmoryEquipmentSetCount(EquipmentSetRef equipmentSet)
	{
		int num = 0;
		if (equipmentSet == null)
		{
			return num;
		}
		foreach (GameModifier typeModifier in GameModifier.getTypeModifiers(GetModifiers(checkSets: false), 67))
		{
			int num2 = (int)typeModifier.value;
			if (num >= num2)
			{
				return num + 1;
			}
		}
		return num;
	}

	public ArmoryRef GetArmoryEquipmentSlot(int slot)
	{
		if (!_armorySlots.ContainsKey(slot))
		{
			return null;
		}
		if (_armorySlots[slot] != null)
		{
			_armorySlots[slot].OverrideItemType(16);
		}
		return _armorySlots[slot];
	}

	public ArmoryRef GetArmoryEquipmentSlotByArmoryType(int type)
	{
		foreach (KeyValuePair<int, ArmoryRef> armorySlot in _armorySlots)
		{
			if (armorySlot.Value != null && armorySlot.Value.equipmentType == type)
			{
				return armorySlot.Value;
			}
		}
		return null;
	}

	public ArmoryRef GetCosmeticSlot(int slot)
	{
		if (!_cosmeticSlots.ContainsKey(slot))
		{
			return null;
		}
		return _cosmeticSlots[slot];
	}

	public ArmoryRef GetCosmeticSlotByArmoryType(int type)
	{
		foreach (KeyValuePair<int, ArmoryRef> cosmeticSlot in _cosmeticSlots)
		{
			if (cosmeticSlot.Value != null && cosmeticSlot.Value.equipmentType == type)
			{
				return cosmeticSlot.Value;
			}
		}
		return null;
	}

	public MountRef GetCosmeticMountEquipped()
	{
		if (_mountCosmetic > 0)
		{
			foreach (MountData mount in GameData.instance.PROJECT.character.mounts.mounts)
			{
				if (mount.mountRef.id == _mountCosmetic)
				{
					return mount.mountRef;
				}
			}
		}
		return null;
	}

	public ArmoryRef GetDisplaySlot(int slot)
	{
		ArmoryRef armoryEquipmentSlot = GetArmoryEquipmentSlot(slot);
		ArmoryRef cosmeticSlot = GetCosmeticSlot(slot);
		ArmoryRef result = armoryEquipmentSlot;
		if (cosmeticSlot != null && armoryEquipmentSlot != null && armoryEquipmentSlot.subtypesMatch(cosmeticSlot))
		{
			result = cosmeticSlot;
		}
		return result;
	}

	public bool SetArmoryEquipmentSlot(ArmoryRef equipRef, int slot)
	{
		if (equipRef != null && !CanEquipItem(equipRef))
		{
			return false;
		}
		if (_armorySlots.ContainsKey(slot))
		{
			_armorySlots[slot] = equipRef;
		}
		else
		{
			_armorySlots.Add(slot, equipRef);
		}
		Broadcast();
		return true;
	}

	public bool SetCosmeticSlot(ArmoryRef equipRef, int slot)
	{
		if (equipRef != null && !CanEquipItem(equipRef))
		{
			return false;
		}
		if (_cosmeticSlots.ContainsKey(slot))
		{
			_cosmeticSlots[slot] = equipRef;
		}
		else
		{
			_cosmeticSlots.Add(slot, equipRef);
		}
		Broadcast();
		return true;
	}

	public void SetArmoryEquipmentSlots(Dictionary<int, ArmoryRef> slots)
	{
		_armorySlots = slots;
		Broadcast();
	}

	public void ClearSlots()
	{
		_armorySlots.Clear();
		_cosmeticSlots.Clear();
		Broadcast();
	}

	public void EquipItem(ArmoryRef equipRef)
	{
		if (!(equipRef == null))
		{
			SetArmoryEquipmentSlot(equipRef, GetAvailableSlot(equipRef.equipmentType));
		}
	}

	public bool CanEquipItem(ArmoryRef equipRef)
	{
		return GetAvailableSlot(equipRef.armoryType) >= 0;
	}

	public List<ArmoryRef> GetDisplayItems()
	{
		List<ArmoryRef> list = new List<ArmoryRef>();
		for (int i = 0; i < 8; i++)
		{
			ArmoryRef displaySlot = GetDisplaySlot(i);
			list.Add(displaySlot);
		}
		return list;
	}

	public static Equipment ArmoryEquipmentToEquipment(ArmoryEquipment armoryEquip)
	{
		Equipment equipment = new Equipment();
		foreach (KeyValuePair<int, ArmoryRef> armorySlot in armoryEquip.armorySlots)
		{
			int equipmentID = 0;
			if (armorySlot.Value != null)
			{
				equipmentID = armorySlot.Value.id;
			}
			equipment.setEquipmentSlot(EquipmentBook.Lookup(equipmentID), armorySlot.Key);
		}
		foreach (KeyValuePair<int, ArmoryRef> cosmeticSlot in armoryEquip.cosmeticSlots)
		{
			int equipmentID2 = 0;
			if (cosmeticSlot.Value != null)
			{
				equipmentID2 = cosmeticSlot.Value.id;
			}
			equipment.setCosmeticSlot(EquipmentBook.Lookup(equipmentID2), cosmeticSlot.Key);
		}
		return equipment;
	}

	public static ArmoryEquipment equipmentToArmoryEquipment(Equipment equip, Runes runes, Mounts mounts, Enchants enchants)
	{
		ArmoryEquipment armoryEquipment = new ArmoryEquipment();
		armoryEquipment.id = -1L;
		foreach (KeyValuePair<int, EquipmentRef> equipmentSlot in equip.equipmentSlots)
		{
			int num = 0;
			ArmoryRef equipRef = null;
			num = equipmentSlot.Key;
			if (equipmentSlot.Value != null)
			{
				equipRef = ArmoryRef.EquipmentRefToArmoryRef(equipmentSlot.Value);
			}
			armoryEquipment.SetArmoryEquipmentSlot(equipRef, num);
		}
		foreach (KeyValuePair<int, EquipmentRef> cosmeticSlot in equip.cosmeticSlots)
		{
			int slot = 0;
			ArmoryRef equipRef2 = null;
			if (cosmeticSlot.Value != null)
			{
				equipRef2 = ArmoryRef.EquipmentRefToArmoryRef(cosmeticSlot.Value);
				slot = cosmeticSlot.Key;
			}
			armoryEquipment.SetCosmeticSlot(equipRef2, slot);
		}
		ArmoryRunes armoryRunes = new ArmoryRunes(runes.runeSlots, runes.runeSlotsMemory);
		armoryEquipment.SetRunes(armoryRunes);
		ArmoryEnchants armoryEnchants = new ArmoryEnchants(enchants.slots, enchants.enchants);
		armoryEquipment.SetEnchants(armoryEnchants);
		if (mounts.getMountEquipped() != null)
		{
			armoryEquipment.mount = mounts.getMountEquipped().uid;
		}
		else
		{
			armoryEquipment.mount = 0L;
		}
		if (mounts.cosmetic != null)
		{
			armoryEquipment.mountCosmetic = mounts.cosmetic.id;
		}
		else
		{
			armoryEquipment.mountCosmetic = 0L;
		}
		return armoryEquipment;
	}

	public static MountData GetCurrentArmoryMountData()
	{
		MountData result = null;
		for (int i = 0; i < GameData.instance.PROJECT.character.mounts.mounts.Count; i++)
		{
			MountData mountData = GameData.instance.PROJECT.character.mounts.mounts[i];
			if (GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.mount == mountData.uid)
			{
				result = mountData;
				break;
			}
		}
		return result;
	}

	public static List<ArmoryEquipment> ListFromSFSObject(ISFSObject sfsob)
	{
		if (sfsob == null)
		{
			return new List<ArmoryEquipment>();
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("cha102");
		List<ArmoryEquipment> list = new List<ArmoryEquipment>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ArmoryEquipment armoryEquipment = FromSFSObject(sFSArray.GetSFSObject(i));
			if (armoryEquipment != null)
			{
				list.Add(armoryEquipment);
			}
		}
		return list;
	}

	public static int GetAvailableSlot(int equipType)
	{
		return equipType switch
		{
			1 => 0, 
			2 => 1, 
			3 => 2, 
			4 => 3, 
			5 => 4, 
			6 => 5, 
			7 => 6, 
			8 => 7, 
			9 => 8, 
			_ => -1, 
		};
	}

	public static string GetArmorySlotCurrency(int currency)
	{
		string result = "";
		switch (currency)
		{
		case 1:
			result = Language.GetString("ui_armory_diamonds");
			break;
		case 0:
			result = Language.GetString("ui_armory_gold");
			break;
		}
		return result;
	}

	public static ArmoryEquipment FromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("cha103"))
		{
			return null;
		}
		ArmoryEquipment armoryEquipment = new ArmoryEquipment();
		armoryEquipment.id = sfsob.GetInt("id");
		armoryEquipment.position = (uint)sfsob.GetInt("position");
		armoryEquipment.name = sfsob.GetUtfString("name");
		armoryEquipment.unlocked = sfsob.GetBool("unlocked");
		armoryEquipment.pprivate = sfsob.GetBool("armoryPrivate");
		armoryEquipment.battleType = (uint)sfsob.GetInt("armoryBattleType");
		armoryEquipment.value = (uint)sfsob.GetInt("armoryEquipmentValue");
		armoryEquipment.requiredLevel = (uint)sfsob.GetInt("armoryEquipmentRequiredLevel");
		armoryEquipment.currency = sfsob.GetInt("ae_currency");
		armoryEquipment.mount = sfsob.GetLong("mount");
		armoryEquipment.mountCosmetic = sfsob.GetLong("mountCosmetic");
		if (armoryEquipment.unlocked)
		{
			armoryEquipment.SetRunes(ArmoryRunes.fromSFSObject(sfsob.GetSFSObject("armoryRunes")));
			armoryEquipment.SetEnchants(ArmoryEnchants.fromSFSObject(sfsob.GetSFSObject("armoryEnchants")));
		}
		int[] intArray = sfsob.GetIntArray("cha103");
		for (int i = 0; i < intArray.Length; i++)
		{
			EquipmentRef equipmentRef = EquipmentBook.Lookup(intArray[i]);
			ArmoryRef equipRef = null;
			if (equipmentRef != null)
			{
				equipRef = equipmentRef.GetArmoryRefFromEquipmentRef();
			}
			armoryEquipment.SetArmoryEquipmentSlot(equipRef, i);
		}
		int[] intArray2 = sfsob.GetIntArray("cha104");
		for (int j = 0; j < intArray2.Length; j++)
		{
			EquipmentRef equipmentRef2 = EquipmentBook.Lookup(intArray2[j]);
			ArmoryRef equipRef2 = null;
			if (equipmentRef2 != null)
			{
				equipRef2 = equipmentRef2.GetArmoryRefFromEquipmentRef();
			}
			armoryEquipment.SetCosmeticSlot(equipRef2, j);
		}
		return armoryEquipment;
	}

	public static int GetSlotArmoryEquipmentType(int slot)
	{
		return slot switch
		{
			0 => 1, 
			1 => 2, 
			2 => 3, 
			3 => 4, 
			4 => 5, 
			5 => 6, 
			6 => 7, 
			7 => 8, 
			8 => 9, 
			_ => -1, 
		};
	}

	public static string GetSlotName(int slot)
	{
		return ArmoryRef.GetArmoryTypeName(GetSlotArmoryEquipmentType(slot));
	}

	public List<AbilityRef> GetAbilities()
	{
		List<AbilityRef> list = new List<AbilityRef>();
		foreach (KeyValuePair<int, ArmoryRef> armorySlot in _armorySlots)
		{
			if (armorySlot.Value == null || armorySlot.Value.abilities == null)
			{
				continue;
			}
			foreach (AbilityRef ability in armorySlot.Value.abilities)
			{
				list.Add(ability);
			}
		}
		if (list.Count <= 0)
		{
			foreach (AbilityRef item in VariableBook.abilitiesDefault)
			{
				list.Add(item);
			}
		}
		foreach (KeyValuePair<int, RuneRef> runeSlot in _runes.runeSlots)
		{
			if (runeSlot.Value == null || runeSlot.Value.abilities == null)
			{
				continue;
			}
			foreach (AbilityRef ability2 in runeSlot.Value.abilities)
			{
				list.Add(ability2);
			}
		}
		foreach (EquipmentSetBonusRef armoryEquippedSetBonuse in GetArmoryEquippedSetBonuses())
		{
			if (armoryEquippedSetBonuse == null || armoryEquippedSetBonuse.abilities == null)
			{
				continue;
			}
			foreach (AbilityRef ability3 in armoryEquippedSetBonuse.abilities)
			{
				list.Add(ability3);
			}
		}
		MountData mountData = GameData.instance.PROJECT.character.mounts.getMount(_mount);
		if (mountData != null && mountData.mountRef.abilities != null)
		{
			foreach (AbilityRef ability4 in mountData.mountRef.abilities)
			{
				list.Add(ability4);
			}
			return list;
		}
		return list;
	}

	public void SetRunes(ArmoryRunes runes)
	{
		_runes = runes;
		if (GameData.instance.PROJECT.character != null)
		{
			GameData.instance.PROJECT.character.Broadcast("armoryRuneChange");
		}
	}

	public void SetEnchants(ArmoryEnchants enchants)
	{
		_enchants = enchants;
	}
}
