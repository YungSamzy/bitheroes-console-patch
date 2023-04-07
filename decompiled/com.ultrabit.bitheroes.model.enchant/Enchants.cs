using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.enchant;

public class Enchants
{
	public const int SLOT_A = 0;

	public const int SLOT_B = 1;

	public const int SLOT_C = 2;

	public const int SLOT_D = 3;

	public const int SLOT_E = 4;

	public const int SLOT_F = 5;

	public const int SLOTS = 6;

	private List<long> _slots;

	private List<EnchantData> _enchants;

	public UnityEvent OnChange = new UnityEvent();

	public List<long> slots => _slots;

	public List<EnchantData> enchants
	{
		get
		{
			return _enchants;
		}
		protected set
		{
			_enchants = value;
		}
	}

	public bool hasAnyEnchant => _enchants.Count > 0;

	public bool hasAnyEnchantEquipped
	{
		get
		{
			for (int i = 0; i < 6; i++)
			{
				if (getSlot(i) != null)
				{
					return true;
				}
			}
			return false;
		}
	}

	public Enchants(List<long> slots, List<EnchantData> enchants)
	{
		_slots = ((slots != null) ? slots : new List<long>());
		_enchants = ((enchants != null) ? enchants : new List<EnchantData>());
	}

	public void Broadcast()
	{
		OnChange?.Invoke();
		SetNewStats();
	}

	private void SetNewStats()
	{
		if (GameData.instance.windowGenerator.CharacterWindow != null)
		{
			GameData.instance.windowGenerator.CharacterWindow._equipmentPanel.SetNewStats(0, GetStat(0));
			GameData.instance.windowGenerator.CharacterWindow._equipmentPanel.SetNewStats(1, GetStat(1));
			GameData.instance.windowGenerator.CharacterWindow._equipmentPanel.SetNewStats(2, GetStat(2));
		}
	}

	public virtual void setEnchantSlots(List<long> slots)
	{
		_slots = slots;
		Broadcast();
	}

	public void addEnchant(EnchantData enchantData)
	{
		if (getEnchant(enchantData.uid) == null)
		{
			_enchants.Add(enchantData);
			Broadcast();
		}
	}

	public void removeEnchant(long uid)
	{
		for (int i = 0; i < _enchants.Count; i++)
		{
			if (_enchants[i].uid == uid)
			{
				_enchants.RemoveAt(i);
				Broadcast();
				break;
			}
		}
	}

	public void updateEnchant(EnchantData enchantData)
	{
		EnchantData enchant = getEnchant(enchantData.uid);
		if (enchant != null)
		{
			enchant.copyData(enchantData);
			Broadcast();
		}
	}

	public int getStatTotal(int stat)
	{
		int num = 0;
		foreach (EnchantData item in getEnchantsEquipped())
		{
			if (item != null)
			{
				switch (stat)
				{
				case 0:
					num += item.power;
					break;
				case 1:
					num += item.stamina;
					break;
				case 2:
					num += item.agility;
					break;
				}
			}
		}
		return num;
	}

	public int GetStat(int stat)
	{
		int result = 0;
		foreach (EnchantData item in getEnchantsEquipped())
		{
			if (item != null)
			{
				switch (stat)
				{
				case 0:
					result = item.power;
					break;
				case 1:
					result = item.stamina;
					break;
				case 2:
					result = item.agility;
					break;
				}
			}
		}
		return result;
	}

	public virtual EnchantData getSlot(int slot)
	{
		if (slot < 0 || slot >= _slots.Count)
		{
			return null;
		}
		return getEnchant(_slots[slot]);
	}

	public virtual EnchantData getEnchant(long uid)
	{
		return _enchants.Find((EnchantData item) => item.uid == uid);
	}

	public int getEnchantSlot(EnchantData enchantData)
	{
		for (int i = 0; i < _slots.Count; i++)
		{
			float num = _slots[i];
			if ((float)enchantData.uid == num)
			{
				return i;
			}
		}
		return -1;
	}

	public void clearEnchantSlot(long uid)
	{
		for (int i = 0; i < _slots.Count; i++)
		{
			long num = _slots[i];
			if (uid == num)
			{
				_slots[i] = 0L;
			}
		}
	}

	public virtual void clearEnchantSlots()
	{
		_enchants = new List<EnchantData>();
		for (int i = 0; i < _slots.Count; i++)
		{
			_ = _slots[i];
			_slots[i] = 0L;
		}
	}

	public List<GameModifier> getGameModifiers()
	{
		List<GameModifier> list = new List<GameModifier>();
		foreach (EnchantData item in getEnchantsEquipped())
		{
			foreach (GameModifier gameModifier in item.getGameModifiers())
			{
				list.Add(gameModifier);
			}
		}
		return list;
	}

	public List<EnchantData> getEnchantsEquipped()
	{
		List<EnchantData> list = new List<EnchantData>();
		for (int i = 0; i < _slots.Count; i++)
		{
			EnchantData slot = getSlot(i);
			if (slot != null)
			{
				list.Add(slot);
			}
		}
		return list;
	}

	public bool getEnchantEquipped(EnchantData enchantData)
	{
		return slotListHasEnchant(_slots, enchantData);
	}

	public bool getSlotUnlocked(Character character, int slot)
	{
		EnchantSlotRef enchantSlotRef = EnchantBook.LookupSlot(slot);
		if (enchantSlotRef == null)
		{
			return false;
		}
		return character.level >= enchantSlotRef.levelReq;
	}

	public static Enchants fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("ench0"))
		{
			return null;
		}
		ISFSObject sFSObject = sfsob.GetSFSObject("ench0");
		if (sFSObject == null)
		{
			return null;
		}
		List<long> list = Util.arrayToNumberVector(sFSObject.GetLongArray("ench8"));
		List<EnchantData> list2 = EnchantData.listFromSFSObject(sFSObject);
		return new Enchants(list, list2);
	}

	public static bool slotListHasEnchant(List<long> slots, EnchantData enchantData)
	{
		if (slots == null || enchantData == null || slots.Count <= 0)
		{
			return false;
		}
		foreach (long slot in slots)
		{
			if (enchantData.uid == slot)
			{
				return true;
			}
		}
		return false;
	}
}
