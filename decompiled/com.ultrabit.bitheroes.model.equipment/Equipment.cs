using System.Collections.Generic;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.variable;
using Sfs2X.Entities.Data;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.equipment;

public class Equipment
{
	public const int SLOT_TYPE_MAINHAND = 0;

	public const int SLOT_TYPE_OFFHAND = 1;

	public const int SLOT_TYPE_HEAD = 2;

	public const int SLOT_TYPE_BODY = 3;

	public const int SLOT_TYPE_NECK = 4;

	public const int SLOT_TYPE_RING = 5;

	public const int SLOT_TYPE_ACCESSORY = 6;

	public const int SLOT_TYPE_PET = 7;

	public const int SLOTS = 8;

	private Dictionary<int, EquipmentRef> _equipmentSlots;

	private Dictionary<int, EquipmentRef> _cosmeticSlots;

	public UnityEvent OnChange = new UnityEvent();

	public UnityEvent BeforeChange = new UnityEvent();

	private EquipmentRef _updatedEquipmentRef;

	private EquipmentRef _prevUpdatedEquipmentRef;

	public Dictionary<int, EquipmentRef> equipmentSlots => _equipmentSlots;

	public Dictionary<int, EquipmentRef> cosmeticSlots => _cosmeticSlots;

	public bool hasAnySlotEmpty
	{
		get
		{
			foreach (KeyValuePair<int, EquipmentRef> equipmentSlot in equipmentSlots)
			{
				if (equipmentSlot.Key == 6)
				{
					TutorialRef tutorialRef = VariableBook.LookUpTutorial("accessory_equip");
					if (!tutorialRef.areItemConditionsMet && !tutorialRef.isMinFlagConditionMet)
					{
						continue;
					}
				}
				if (equipmentSlot.Key == 7)
				{
					TutorialRef tutorialRef2 = VariableBook.LookUpTutorial("pet_equip");
					if (!tutorialRef2.areItemConditionsMet && !tutorialRef2.isMinFlagConditionMet)
					{
						continue;
					}
				}
				if (equipmentSlot.Value == null)
				{
					return true;
				}
			}
			return false;
		}
	}

	public Equipment(Dictionary<int, EquipmentRef> equipmentSlots = null, Dictionary<int, EquipmentRef> cosmeticSlots = null)
	{
		_equipmentSlots = ((equipmentSlots != null) ? equipmentSlots : new Dictionary<int, EquipmentRef>());
		_cosmeticSlots = ((cosmeticSlots != null) ? cosmeticSlots : new Dictionary<int, EquipmentRef>());
	}

	public int getEquipmentCount(ItemRef itemRef)
	{
		int num = 0;
		foreach (KeyValuePair<int, EquipmentRef> equipmentSlot in _equipmentSlots)
		{
			if (!(equipmentSlot.Value == null) && equipmentSlot.Value == itemRef)
			{
				num++;
			}
		}
		return num;
	}

	public int getStatTotal(int stat)
	{
		int num = 0;
		foreach (KeyValuePair<int, EquipmentRef> equipmentSlot in equipmentSlots)
		{
			if (!(equipmentSlot.Value == null))
			{
				switch (stat)
				{
				case 0:
					num += equipmentSlot.Value.power;
					break;
				case 1:
					num += equipmentSlot.Value.stamina;
					break;
				case 2:
					num += equipmentSlot.Value.agility;
					break;
				}
			}
		}
		return num;
	}

	public List<GameModifier> getModifiers(bool checkSets = true)
	{
		List<GameModifier> list = new List<GameModifier>();
		if (equipmentSlots != null)
		{
			foreach (KeyValuePair<int, EquipmentRef> equipmentSlot in equipmentSlots)
			{
				if (equipmentSlot.Value == null || equipmentSlot.Value.modifiers == null)
				{
					continue;
				}
				foreach (GameModifier modifier in equipmentSlot.Value.modifiers)
				{
					list.Add(modifier);
				}
			}
		}
		if (checkSets)
		{
			foreach (EquipmentSetBonusRef equippedSetBonuse in getEquippedSetBonuses())
			{
				foreach (GameModifier modifier2 in equippedSetBonuse.modifiers)
				{
					list.Add(modifier2);
				}
			}
			return list;
		}
		return list;
	}

	public List<EquipmentSetRef> getEquippedSets()
	{
		List<EquipmentSetRef> list = new List<EquipmentSetRef>();
		foreach (KeyValuePair<int, EquipmentRef> equipmentSlot in equipmentSlots)
		{
			if (!(equipmentSlot.Value == null) && equipmentSlot.Value.equipmentSet != null && !EquipmentSetRef.listHasSet(list, equipmentSlot.Value.equipmentSet))
			{
				list.Add(equipmentSlot.Value.equipmentSet);
			}
		}
		return list;
	}

	public List<EquipmentSetBonusRef> getEquippedSetBonuses()
	{
		List<EquipmentSetBonusRef> list = new List<EquipmentSetBonusRef>();
		foreach (EquipmentSetRef equippedSet in getEquippedSets())
		{
			int equipmentSetCount = getEquipmentSetCount(equippedSet);
			foreach (EquipmentSetBonusRef bonuse in equippedSet.bonuses)
			{
				if (bonuse.getBonusEnabled(equipmentSetCount))
				{
					list.Add(bonuse);
				}
			}
		}
		return list;
	}

	public List<AbilityRef> getEquippedModifierConditionWithAbilities()
	{
		List<AbilityRef> list = new List<AbilityRef>();
		foreach (KeyValuePair<int, EquipmentRef> equipmentSlot in _equipmentSlots)
		{
			if (!(equipmentSlot.Value != null))
			{
				continue;
			}
			foreach (GameModifier modifier in equipmentSlot.Value.modifiers)
			{
				if (modifier.conditions == null)
				{
					continue;
				}
				foreach (BattleConditionRef condition in modifier.conditions)
				{
					if (!condition.getNonBattleConditionMet(new List<EquipmentRef>(_equipmentSlots.Values)) || condition.abilities == null)
					{
						continue;
					}
					foreach (AbilityRef ability in condition.abilities)
					{
						list.Add(ability);
					}
				}
			}
		}
		return list;
	}

	public bool getItemOriginEquipped(ItemRef itemRef)
	{
		if (itemRef == null)
		{
			return false;
		}
		foreach (KeyValuePair<int, EquipmentRef> equipmentSlot in _equipmentSlots)
		{
			if (!(equipmentSlot.Value == null) && (equipmentSlot.Value == itemRef || equipmentSlot.Value.originSource == itemRef))
			{
				return true;
			}
		}
		return false;
	}

	public int getEquipmentSetCount(EquipmentSetRef equipmentSet)
	{
		int num = 0;
		if (equipmentSet == null)
		{
			return num;
		}
		foreach (EquipmentRef item in equipmentSet.equipment)
		{
			if (getItemOriginEquipped(item) || getItemSetEquipped(item))
			{
				num++;
			}
		}
		foreach (GameModifier typeModifier in GameModifier.getTypeModifiers(getModifiers(checkSets: false), 67))
		{
			int num2 = (int)typeModifier.value;
			if (num >= num2)
			{
				return num + 1;
			}
		}
		return num;
	}

	public EquipmentRef getEquipmentSlot(int slot)
	{
		if (!_equipmentSlots.ContainsKey(slot))
		{
			return null;
		}
		return _equipmentSlots[slot];
	}

	public EquipmentRef getCosmeticSlot(int slot)
	{
		if (!_cosmeticSlots.ContainsKey(slot))
		{
			return null;
		}
		return _cosmeticSlots[slot];
	}

	public EquipmentRef getDisplaySlot(int slot)
	{
		EquipmentRef equipmentSlot = getEquipmentSlot(slot);
		EquipmentRef cosmeticSlot = getCosmeticSlot(slot);
		EquipmentRef result = equipmentSlot;
		if (cosmeticSlot != null && equipmentSlot != null && equipmentSlot.subtypesMatch(cosmeticSlot))
		{
			result = cosmeticSlot;
		}
		return result;
	}

	public bool setEquipmentSlot(EquipmentRef equipRef, int slot, bool doBroadcast = true)
	{
		if (doBroadcast)
		{
			BroadcastBefore();
		}
		if (equipRef != null && !canEquipItem(equipRef))
		{
			return false;
		}
		if (_equipmentSlots.ContainsKey(slot))
		{
			_equipmentSlots[slot] = equipRef;
		}
		else
		{
			_equipmentSlots.Add(slot, equipRef);
		}
		if (equipRef == null)
		{
			_prevUpdatedEquipmentRef = _updatedEquipmentRef;
		}
		_updatedEquipmentRef = equipRef;
		if (doBroadcast)
		{
			Broadcast();
		}
		return true;
	}

	public EquipmentRef GetLastUpdatedEquipment()
	{
		if (_updatedEquipmentRef != null)
		{
			return _updatedEquipmentRef;
		}
		return _prevUpdatedEquipmentRef;
	}

	public bool setCosmeticSlot(EquipmentRef equipRef, int slot, bool doBroadcast = true)
	{
		if (doBroadcast)
		{
			BroadcastBefore();
		}
		if (equipRef != null && !canEquipItem(equipRef))
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
		if (doBroadcast)
		{
			Broadcast();
		}
		return true;
	}

	public void setEquipmentSlots(Dictionary<int, EquipmentRef> slots)
	{
		BroadcastBefore();
		_equipmentSlots = slots;
		Broadcast();
	}

	public void clearSlots()
	{
		BroadcastBefore();
		_equipmentSlots.Clear();
		_cosmeticSlots.Clear();
		Broadcast();
	}

	public void Broadcast()
	{
		OnChange?.Invoke();
	}

	public void BroadcastBefore()
	{
		BeforeChange?.Invoke();
	}

	public void equipItem(EquipmentRef equipRef)
	{
		if (!(equipRef == null))
		{
			setEquipmentSlot(equipRef, getAvailableSlot(equipRef.equipmentType));
		}
	}

	public bool canEquipItem(EquipmentRef equipRef)
	{
		return getAvailableSlot(equipRef.equipmentType) >= 0;
	}

	public List<EquipmentRef> getDisplayItems()
	{
		List<EquipmentRef> list = new List<EquipmentRef>();
		for (int i = 0; i < 8; i++)
		{
			EquipmentRef displaySlot = getDisplaySlot(i);
			list.Add(displaySlot);
		}
		return list;
	}

	public static int getAvailableSlot(int equipType)
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
			_ => -1, 
		};
	}

	public static Equipment fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("cha17"))
		{
			return null;
		}
		Dictionary<int, EquipmentRef> dictionary = new Dictionary<int, EquipmentRef>();
		Dictionary<int, EquipmentRef> dictionary2 = new Dictionary<int, EquipmentRef>();
		int[] intArray = sfsob.GetIntArray("cha17");
		for (int i = 0; i < intArray.Length; i++)
		{
			EquipmentRef value = EquipmentBook.Lookup(intArray[i]);
			dictionary.Add(i, value);
		}
		int[] intArray2 = sfsob.GetIntArray("cha64");
		for (int j = 0; j < intArray2.Length; j++)
		{
			int equipmentID = intArray2[j];
			dictionary2.Add(j, EquipmentBook.Lookup(equipmentID));
		}
		return new Equipment(dictionary, dictionary2);
	}

	public static int getSlotEquipmentType(int slot)
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
			_ => -1, 
		};
	}

	public static string getSlotName(int slot)
	{
		return EquipmentRef.getEquipmentTypeName(getSlotEquipmentType(slot));
	}

	public bool getItemSetEquipped(ItemRef itemRef)
	{
		if (itemRef == null)
		{
			return false;
		}
		foreach (KeyValuePair<int, EquipmentRef> equipmentSlot in _equipmentSlots)
		{
			if (!(equipmentSlot.Value == null) && (equipmentSlot.Value == itemRef || equipmentSlot.Value.setSource == itemRef))
			{
				return true;
			}
		}
		return false;
	}

	public List<KongregateAnalyticsSchema.ItemStat> statAllEquipement()
	{
		List<KongregateAnalyticsSchema.ItemStat> list = new List<KongregateAnalyticsSchema.ItemStat>();
		foreach (KeyValuePair<int, EquipmentRef> equipmentSlot in _equipmentSlots)
		{
			if (!(equipmentSlot.Value == null))
			{
				list.Add(equipToStat(equipmentSlot.Value));
			}
		}
		return list;
	}

	public KongregateAnalyticsSchema.ItemStat equipToStat(EquipmentRef equip)
	{
		return new KongregateAnalyticsSchema.ItemStat
		{
			item_name = equip.statName,
			rarity = equip.rarity,
			type = equip.itemType
		};
	}
}
