using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.augment;

public class AugmentBook
{
	private static Dictionary<int, AugmentTypeRef> _types;

	private static Dictionary<int, AugmentSlotRef> _slots;

	private static Dictionary<int, AugmentRef> _augments;

	public static int size => _augments.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_types = new Dictionary<int, AugmentTypeRef>();
		_slots = new Dictionary<int, AugmentSlotRef>();
		_augments = new Dictionary<int, AugmentRef>();
		foreach (AugmentBookData.Type item in XMLBook.instance.augmentBookData.types.lstType)
		{
			_types.Add(item.id, new AugmentTypeRef(item.id, item));
		}
		foreach (AugmentBookData.Slot item2 in XMLBook.instance.augmentBookData.slots.lstSlot)
		{
			_slots.Add(item2.id, new AugmentSlotRef(item2.id, item2));
		}
		foreach (AugmentBookData.Augment item3 in XMLBook.instance.augmentBookData.augments.lstAugment)
		{
			if (_augments.ContainsKey(item3.id))
			{
				D.LogWarning("Augment with ID: " + item3.id + " already added to the dictionary. Skipping");
				continue;
			}
			AugmentRef augmentRef = new AugmentRef(item3.id, item3);
			augmentRef.LoadDetails(item3);
			_augments.Add(item3.id, augmentRef);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static AugmentRef Lookup(int id)
	{
		if (_augments.ContainsKey(id))
		{
			return _augments[id];
		}
		return null;
	}

	public static List<AugmentModifierRef> LookupModifiers(AugmentTypeRef typeRef, List<int> list)
	{
		List<AugmentModifierRef> list2 = new List<AugmentModifierRef>();
		if (list == null || list.Count <= 0)
		{
			return list2;
		}
		foreach (int item in list)
		{
			AugmentModifierRef modifier = typeRef.getModifier(item);
			if (modifier != null)
			{
				list2.Add(modifier);
			}
		}
		return list2;
	}

	public static AugmentSlotRef LookupSlot(int slot)
	{
		if (_slots.ContainsKey(slot))
		{
			return _slots[slot];
		}
		return null;
	}

	public static AugmentTypeRef LookupTypeLink(string link)
	{
		if (link == null)
		{
			return null;
		}
		foreach (KeyValuePair<int, AugmentTypeRef> type in _types)
		{
			if (type.Value.link.Equals(link))
			{
				return type.Value;
			}
		}
		return null;
	}

	public static List<AugmentSlotRef> GetTypeSlots(int type)
	{
		List<AugmentSlotRef> list = new List<AugmentSlotRef>();
		foreach (KeyValuePair<int, AugmentSlotRef> slot in _slots)
		{
			if (slot.Value.typeRef.id == type)
			{
				list.Add(slot.Value);
			}
		}
		return list;
	}

	public static Dictionary<int, AugmentTypeRef> GetTypes()
	{
		return _types;
	}
}
