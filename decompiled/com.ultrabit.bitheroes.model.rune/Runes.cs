using System.Collections.Generic;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.rune;

public class Runes
{
	public const int SLOT_TYPE_MAJOR_A = 0;

	public const int SLOT_TYPE_MAJOR_B = 1;

	public const int SLOT_TYPE_MAJOR_C = 2;

	public const int SLOT_TYPE_MAJOR_D = 3;

	public const int SLOT_TYPE_MINOR_A = 4;

	public const int SLOT_TYPE_MINOR_B = 5;

	public const int SLOT_TYPE_META = 6;

	public const int SLOT_TYPE_RELIC = 7;

	public const int SLOT_TYPE_ARTIFACT = 8;

	public const int SLOTS = 9;

	public UnityEvent OnChange = new UnityEvent();

	private Dictionary<int, RuneRef> _runeSlots;

	private Dictionary<int, List<RuneRef>> _runeSlotsMemory;

	public Dictionary<int, RuneRef> runeSlots => _runeSlots;

	public Dictionary<int, List<RuneRef>> runeSlotsMemory => _runeSlotsMemory;

	public bool hasAnySlotMemory
	{
		get
		{
			for (int i = 0; i < 9; i++)
			{
				if (getRuneSlotMemory(i).Count > 0)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool hasAnySlotEquipped
	{
		get
		{
			for (int i = 0; i < 9; i++)
			{
				if (runeSlots.ContainsKey(i))
				{
					return true;
				}
			}
			return false;
		}
	}

	public Runes(Dictionary<int, RuneRef> runeSlots = null, Dictionary<int, List<RuneRef>> runeSlotsMemory = null)
	{
		_runeSlots = ((runeSlots != null) ? new Dictionary<int, RuneRef>(runeSlots) : new Dictionary<int, RuneRef>());
		_runeSlotsMemory = ((runeSlotsMemory != null) ? new Dictionary<int, List<RuneRef>>(runeSlotsMemory) : new Dictionary<int, List<RuneRef>>());
	}

	public void Broadcast()
	{
		OnChange?.Invoke();
	}

	public RuneRef getRuneSlot(int slot)
	{
		if (runeSlots.ContainsKey(slot))
		{
			return _runeSlots[slot];
		}
		return null;
	}

	public virtual List<RuneRef> getRuneSlotMemory(int slot)
	{
		if (_runeSlotsMemory.ContainsKey(slot))
		{
			return _runeSlotsMemory[slot];
		}
		D.LogWarning("Slot Memory for slot " + slot + " is empty");
		return null;
	}

	public int getRuneSlotID(int slot)
	{
		RuneRef runeSlot = getRuneSlot(slot);
		if (runeSlot == null)
		{
			return 0;
		}
		return runeSlot.id;
	}

	public bool setRuneSlot(RuneRef runeRef, int slot)
	{
		if (runeRef != null)
		{
			if (!getSlotAllowed(runeRef.runeType, slot))
			{
				return false;
			}
			if (!_runeSlots.ContainsKey(slot))
			{
				_runeSlots.Add(slot, runeRef);
			}
			else
			{
				_runeSlots[slot] = runeRef;
			}
			Broadcast();
			return true;
		}
		return false;
	}

	public void setRuneSlotMemory(List<RuneRef> memory, int slot)
	{
		_runeSlotsMemory.Add(slot, memory);
		Broadcast();
	}

	public void setRuneSlots(Dictionary<int, RuneRef> slots)
	{
		_runeSlots = slots;
		Broadcast();
	}

	public bool hasSlotMemory(RuneRef runeRef, int slot)
	{
		List<RuneRef> runeSlotMemory = getRuneSlotMemory(slot);
		if (runeSlotMemory == null)
		{
			return false;
		}
		foreach (RuneRef item in runeSlotMemory)
		{
			if (item == runeRef)
			{
				return true;
			}
		}
		return false;
	}

	public bool addSlotMemory(RuneRef runeRef, int slot)
	{
		if (hasSlotMemory(runeRef, slot))
		{
			return false;
		}
		List<RuneRef> list = getRuneSlotMemory(slot);
		if (list == null)
		{
			list = new List<RuneRef>();
		}
		list.Add(runeRef);
		setRuneSlotMemory(list, slot);
		return true;
	}

	public bool canChangeSlot(int slot)
	{
		if (getChangeableRunes(slot).Count <= 0)
		{
			return false;
		}
		return true;
	}

	public List<ItemData> getChangeableArmoryRunes(int slot)
	{
		List<ItemData> list = new List<ItemData>();
		List<RuneRef> runeSlotMemory = getRuneSlotMemory(slot);
		if (runeSlotMemory == null || runeSlotMemory.Count <= 0)
		{
			return list;
		}
		getRuneSlot(slot);
		foreach (RuneRef item in runeSlotMemory)
		{
			list.Add(new ItemData(item, 1));
		}
		return list;
	}

	public List<ItemData> getChangeableRunes(int slot)
	{
		List<ItemData> list = new List<ItemData>();
		List<RuneRef> runeSlotMemory = getRuneSlotMemory(slot);
		if (runeSlotMemory == null || runeSlotMemory.Count <= 0)
		{
			return list;
		}
		RuneRef runeSlot = getRuneSlot(slot);
		foreach (RuneRef item in runeSlotMemory)
		{
			if (item != runeSlot)
			{
				list.Add(new ItemData(item, 1));
			}
		}
		return list;
	}

	public void clearSlots()
	{
		_runeSlots.Clear();
		_runeSlotsMemory.Clear();
		Broadcast();
	}

	public static int getSlotType(int slot)
	{
		switch (slot)
		{
		case 0:
		case 1:
		case 2:
		case 3:
			return 1;
		case 4:
		case 5:
			return 2;
		case 6:
			return 3;
		case 7:
			return 4;
		case 8:
			return 5;
		default:
			return -1;
		}
	}

	public static bool getSlotAllowed(int runeType, int slot)
	{
		switch (runeType)
		{
		case 1:
			if ((uint)slot <= 3u)
			{
				return true;
			}
			return false;
		case 2:
			if ((uint)(slot - 4) <= 1u)
			{
				return true;
			}
			return false;
		case 3:
			if (slot == 6)
			{
				return true;
			}
			return false;
		case 4:
			if (slot == 7)
			{
				return true;
			}
			return false;
		case 5:
			if (slot == 8)
			{
				return true;
			}
			return false;
		default:
			return false;
		}
	}

	public static Runes fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("run0"))
		{
			return null;
		}
		Runes runes = new Runes();
		int[] intArray = sfsob.GetIntArray("run0");
		for (int i = 0; i < intArray.Length; i++)
		{
			RuneRef runeRef = RuneBook.Lookup(intArray[i]);
			if (runeRef != null)
			{
				runes.setRuneSlot(runeRef, i);
			}
		}
		if (sfsob.ContainsKey("run1"))
		{
			ISFSArray sFSArray = sfsob.GetSFSArray("run1");
			for (int j = 0; j < sFSArray.Size(); j++)
			{
				ISFSObject sFSObject = sFSArray.GetSFSObject(j);
				int[] intArray2 = sFSObject.GetIntArray("run3");
				int @int = sFSObject.GetInt("run2");
				runes.setRuneSlotMemory(RuneBook.LookupList(intArray2), @int);
			}
		}
		return runes;
	}

	public List<KongregateAnalyticsSchema.ItemStat> statAllRunes()
	{
		List<KongregateAnalyticsSchema.ItemStat> list = new List<KongregateAnalyticsSchema.ItemStat>();
		foreach (KeyValuePair<int, RuneRef> runeSlot in runeSlots)
		{
			if (!(runeSlot.Value == null))
			{
				list.Add(runeToStat(runeSlot.Value));
			}
		}
		return list;
	}

	public KongregateAnalyticsSchema.ItemStat runeToStat(RuneRef rune)
	{
		return new KongregateAnalyticsSchema.ItemStat
		{
			item_name = rune.statName,
			rarity = rune.rarity,
			type = rune.itemType
		};
	}
}
