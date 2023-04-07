using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.consumable;

public class ConsumableBook
{
	private static Dictionary<int, ConsumableRef> _consumables;

	public static int size => _consumables.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_consumables = new Dictionary<int, ConsumableRef>();
		foreach (ConsumableBookData.Consumable item in XMLBook.instance.consumableBook.lstConsumable)
		{
			ConsumableRef consumableRef = new ConsumableRef(item.id, item);
			consumableRef.LoadDetails(item);
			_consumables.Add(item.id, consumableRef);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static List<string> CheckAsset()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<int, ConsumableRef> consumable in _consumables)
		{
			if (consumable.Value != null && consumable.Value.icon != null && !consumable.Value.icon.Trim().Equals("") && GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.CONSUMABLE_ICON, consumable.Value.icon, instantiate: false) == null)
			{
				string item = "Missing Consumable Icon " + consumable.Value.icon + "  (" + consumable.Value.name + ")";
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	public static List<ConsumableRef> GetAllPossiblesConsumables()
	{
		return new List<ConsumableRef>(_consumables.Values);
	}

	public static ConsumableRef Lookup(int id)
	{
		if (_consumables.ContainsKey(id))
		{
			return _consumables[id];
		}
		return null;
	}

	public static List<ItemRef> GetConsumablesByTypes(int[] types)
	{
		List<ItemRef> list = new List<ItemRef>();
		List<int> list2 = new List<int>(types);
		foreach (KeyValuePair<int, ConsumableRef> consumable in _consumables)
		{
			if (list2.Contains(consumable.Value.consumableType))
			{
				list.Add(consumable.Value);
			}
		}
		return list;
	}

	public static List<ItemRef> GetConsumablesByType(int type)
	{
		return GetConsumablesByTypes(new int[1] { type });
	}

	public static ConsumableRef GetFirstConsumableByType(int type)
	{
		foreach (KeyValuePair<int, ConsumableRef> consumable in _consumables)
		{
			if (consumable.Value.consumableType == type)
			{
				return consumable.Value;
			}
		}
		return null;
	}
}
