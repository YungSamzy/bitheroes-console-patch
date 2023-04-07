using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.mount;

public class MountBook
{
	private static Dictionary<string, MountRarityRef> _rarities;

	private static Dictionary<int, MountModifierRef> _modifiers;

	private static Dictionary<int, MountRef> _mounts;

	public static int size => _mounts.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_rarities = new Dictionary<string, MountRarityRef>();
		_modifiers = new Dictionary<int, MountModifierRef>();
		_mounts = new Dictionary<int, MountRef>();
		foreach (MountBookData.Rarity item in XMLBook.instance.mountBook.rarities.lstRarity)
		{
			_rarities.Add(item.link, new MountRarityRef(item));
		}
		foreach (MountBookData.Modifier item2 in XMLBook.instance.mountBook.modifiers.lstModifier)
		{
			List<GameModifier> list = new List<GameModifier>();
			list.Add(new GameModifier(item2.modifier));
			MountModifierRef mountModifierRef = new MountModifierRef(item2.id, RarityBook.Lookup(item2.rarity), list);
			mountModifierRef.LoadDetails(item2);
			_modifiers.Add(item2.id, mountModifierRef);
		}
		foreach (MountBookData.Mount item3 in XMLBook.instance.mountBook.mounts.lstMount)
		{
			_mounts.Add(item3.id, new MountRef(item3.id, item3));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static List<string> CheckAsset()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (KeyValuePair<int, MountRef> mount in _mounts)
		{
			if (!(mount.Value != null) || mount.Value.asset == null || mount.Value.asset.Trim().Equals(""))
			{
				continue;
			}
			if (GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.MOUNT, mount.Value.asset, instantiate: false) == null)
			{
				string item = "Missing Mount Asset " + mount.Value.asset + " (" + mount.Value.name + ")";
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			if (GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.MOUNT_ICON, mount.Value.icon, instantiate: false) == null)
			{
				string item2 = "Missing Mount Icon " + mount.Value.icon + " (" + mount.Value.name + ")";
				if (!list2.Contains(item2))
				{
					list2.Add(item2);
				}
			}
		}
		List<string> list3 = new List<string>();
		list3.AddRange(list);
		list3.AddRange(list2);
		return list3;
	}

	public static List<MountRef> GetFullMountList()
	{
		return new List<MountRef>(_mounts.Values);
	}

	public static MountRef Lookup(int id)
	{
		if (_mounts.ContainsKey(id))
		{
			return _mounts[id];
		}
		return null;
	}

	public static MountRarityRef LookupRarity(string link)
	{
		if (_rarities.ContainsKey(link))
		{
			return _rarities[link];
		}
		return null;
	}

	public static MountModifierRef LookupModifier(int id)
	{
		if (_modifiers.ContainsKey(id))
		{
			return _modifiers[id];
		}
		return null;
	}

	public static List<MountModifierRef> LookupModifiers(List<int> ids)
	{
		List<MountModifierRef> list = new List<MountModifierRef>();
		if (ids != null && ids.Count > 0)
		{
			foreach (int id in ids)
			{
				MountModifierRef mountModifierRef = LookupModifier(id);
				if (mountModifierRef != null)
				{
					list.Add(mountModifierRef);
				}
			}
			return list;
		}
		return list;
	}

	public static List<MountModifierRef> GetRarityModifiers(int rarity)
	{
		List<MountModifierRef> list = new List<MountModifierRef>();
		foreach (KeyValuePair<int, MountModifierRef> modifier in _modifiers)
		{
			if (modifier.Value.rarityRef.id == rarity)
			{
				list.Add(modifier.Value);
			}
		}
		return list;
	}
}
