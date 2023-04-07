using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.enchant;

public class EnchantBook
{
	private static Dictionary<int, EnchantSlotRef> _slots;

	private static Dictionary<int, EnchantModifierRef> _modifiers;

	private static Dictionary<int, EnchantRef> _enchants;

	public static int size => _enchants.Count;

	public static List<EnchantRef> enchants => new List<EnchantRef>(_enchants.Values);

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_slots = new Dictionary<int, EnchantSlotRef>();
		_modifiers = new Dictionary<int, EnchantModifierRef>();
		_enchants = new Dictionary<int, EnchantRef>();
		foreach (EnchantBookData.Slot item in XMLBook.instance.enchantBookData.slots.lstSlot)
		{
			EnchantSlotRef enchantSlotRef = new EnchantSlotRef(item.id, item.levelReq);
			enchantSlotRef.LoadDetails(item);
			_slots.Add(item.id, enchantSlotRef);
		}
		foreach (EnchantBookData.Modifier item2 in XMLBook.instance.enchantBookData.modifiers.lstModifier)
		{
			if (_modifiers.ContainsKey(item2.id))
			{
				D.LogWarning("EnchantBook, modifier duplicated for ID: " + item2.id);
			}
			else
			{
				_modifiers.Add(item2.id, new EnchantModifierRef(item2.id, item2));
			}
		}
		foreach (EnchantBookData.Enchant item3 in XMLBook.instance.enchantBookData.enchants.lstEnchant)
		{
			_enchants.Add(item3.id, new EnchantRef(item3.id, item3));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static List<string> CheckAsset()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<int, EnchantRef> enchant in _enchants)
		{
			if (enchant.Value != null && enchant.Value.icon != null && !enchant.Value.icon.Trim().Equals("") && GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.ENCHANT_ICON, enchant.Value.icon, instantiate: false) == null)
			{
				string item = "Missing Enchant Icon " + enchant.Value.icon + " (" + enchant.Value.name + ")";
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	public static EnchantRef Lookup(int id)
	{
		if (!_enchants.ContainsKey(id))
		{
			return null;
		}
		return _enchants[id];
	}

	public static List<EnchantModifierRef> lookupModifiers(List<int> list)
	{
		List<EnchantModifierRef> list2 = new List<EnchantModifierRef>();
		for (int i = 0; i < list.Count; i++)
		{
			if (_modifiers.ContainsKey(list[i]))
			{
				list2.Add(_modifiers[list[i]]);
			}
		}
		return list2;
	}

	public static EnchantSlotRef LookupSlot(int slot)
	{
		if (_slots.ContainsKey(slot))
		{
			return _slots[slot];
		}
		return null;
	}
}
