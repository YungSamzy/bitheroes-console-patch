using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.equipment;

public class EquipmentBook
{
	private static List<EquipmentSubtypeRef> _SUBTYPES;

	private static Dictionary<int, EquipmentRef> _EQUIPMENT;

	private static List<EquipmentSetRef> _SETS;

	public static int size => _EQUIPMENT.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_SUBTYPES = new List<EquipmentSubtypeRef>();
		_EQUIPMENT = new Dictionary<int, EquipmentRef>();
		_SETS = new List<EquipmentSetRef>();
		foreach (EquipmentBookData.Subtype item in XMLBook.instance.equipmentBookData.lstSubtype)
		{
			if (item != null)
			{
				_SUBTYPES.Add(new EquipmentSubtypeRef(_SUBTYPES.Count, item));
			}
		}
		foreach (EquipmentBookData.Equipment item2 in XMLBook.instance.equipmentBookData.lstEquipment)
		{
			EquipmentRef value = new EquipmentRef(item2.id, item2);
			_EQUIPMENT.Add(item2.id, value);
		}
		foreach (KeyValuePair<int, EquipmentRef> item3 in _EQUIPMENT)
		{
			EquipmentRef value2 = item3.Value;
			value2.UpdateAssetsSource();
			if (value2.upgrades == null || value2.upgrades.Count <= 0)
			{
				continue;
			}
			foreach (ItemUpgradeRef upgrade in item3.Value.upgrades)
			{
				_EQUIPMENT[upgrade.itemID].UpdateAssetsSource(item3.Value);
				_EQUIPMENT[upgrade.itemID].updateRanks(item3.Value.rank + 1, item3.Value.rank + 1);
			}
		}
		foreach (EquipmentBookData.Equipmentset item4 in XMLBook.instance.equipmentBookData.lstEquipmentSet)
		{
			if (item4 != null)
			{
				_SETS.Add(new EquipmentSetRef(item4.id, item4));
			}
		}
		yield return null;
		if (onUpdatedProgress != null && onUpdatedProgress.Target != null && !onUpdatedProgress.Equals(null))
		{
			onUpdatedProgress(XMLBook.instance.UpdateProgress());
		}
	}

	public static List<string> CheckAsset()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (KeyValuePair<int, EquipmentRef> item3 in _EQUIPMENT)
		{
			if (!(item3.Value != null) || item3.Value.icon == null || item3.Value.icon.Trim().Equals(""))
			{
				continue;
			}
			if (GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.EQUIPMENT_ICON, item3.Value.icon, instantiate: false) == null)
			{
				string item = "Missing Equipment Icon " + item3.Value.icon + " (" + item3.Value.name + ")";
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			foreach (EquipmentAssetRef asset in item3.Value.assets)
			{
				if (asset.url != null && GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.EQUIPMENT, asset.url, instantiate: false) == null)
				{
					string item2 = "Missing Equipment " + asset.url + " (" + item3.Value.name + ")";
					if (!list2.Contains(item2))
					{
						list2.Add(item2);
					}
				}
			}
		}
		List<string> list3 = new List<string>();
		list3.AddRange(list);
		list3.AddRange(list2);
		return list3;
	}

	public static List<EquipmentRef> GetFullEquipmentList()
	{
		return new List<EquipmentRef>(_EQUIPMENT.Values);
	}

	public static EquipmentRef Lookup(int equipmentID)
	{
		if (_EQUIPMENT.ContainsKey(equipmentID))
		{
			return _EQUIPMENT[equipmentID];
		}
		if (equipmentID > 0)
		{
			D.LogError("david", "EquipmentBook::EquipmentRef::Lookup - equipment null for: " + equipmentID);
		}
		return null;
	}

	public static List<EquipmentSubtypeRef> LookupSubtypeLinks(string link)
	{
		if (link == null)
		{
			return null;
		}
		string[] array = link.Split(',');
		if (array.Length == 0)
		{
			return null;
		}
		List<EquipmentSubtypeRef> list = new List<EquipmentSubtypeRef>();
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EquipmentSubtypeRef equipmentSubtypeRef = LookupSubtypeLink(array2[i]);
			if (equipmentSubtypeRef != null)
			{
				list.Add(equipmentSubtypeRef);
			}
		}
		return list;
	}

	public static EquipmentSubtypeRef LookupSubtypeLink(string link)
	{
		if (link == null)
		{
			return null;
		}
		EquipmentSubtypeRef equipmentSubtypeRef = _SUBTYPES.Find((EquipmentSubtypeRef item) => item.link.ToLower().Equals(link.ToLower()));
		if (equipmentSubtypeRef != null)
		{
			return equipmentSubtypeRef;
		}
		return null;
	}

	public static EquipmentRef GetUpgradeSource(EquipmentRef itemRef)
	{
		foreach (KeyValuePair<int, EquipmentRef> item in _EQUIPMENT)
		{
			EquipmentRef value = item.Value;
			if (value == null)
			{
				continue;
			}
			foreach (ItemUpgradeRef upgrade in value.upgrades)
			{
				if (upgrade.getUpgradeItemRef() == itemRef)
				{
					return value;
				}
			}
			foreach (ItemReforgeRef reforge in value.reforges)
			{
				if (reforge.itemRef == itemRef)
				{
					return value;
				}
			}
		}
		return null;
	}
}
