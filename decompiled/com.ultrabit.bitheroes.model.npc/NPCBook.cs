using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.npc;

public class NPCBook
{
	private static Dictionary<string, NPCRef> _npcs;

	private static List<string> _npcIndex;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_npcs = new Dictionary<string, NPCRef>();
		_npcIndex = new List<string>();
		foreach (NPCBookData.NPC item in XMLBook.instance.NPCBook.lstNPC)
		{
			_npcs.Add(item.link, new NPCRef(_npcIndex.Count, item));
			_npcIndex.Add(item.link);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static List<string> CheckAsset()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, NPCRef> npc in _npcs)
		{
			if (npc.Value != null && npc.Value.asset != null && !npc.Value.asset.Trim().Equals("") && GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.NPC, npc.Value.asset, instantiate: false) == null)
			{
				string item = "Missing NPC " + npc.Value.asset + " (" + npc.Value.name + ")";
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	public static NPCRef Lookup(int index)
	{
		if (index >= 0 && index < _npcIndex.Count)
		{
			return LookupLink(_npcIndex[index]);
		}
		return null;
	}

	public static NPCRef LookupLink(string link)
	{
		if (_npcs.ContainsKey(link))
		{
			return _npcs[link];
		}
		return null;
	}
}
