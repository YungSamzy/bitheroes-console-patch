using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.rune;

public class RuneBook
{
	private static Dictionary<int, RuneRef> _runes;

	public static int size => _runes.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_runes = new Dictionary<int, RuneRef>();
		foreach (RuneBookData.Rune item in XMLBook.instance.runeBookData.lstRune)
		{
			_runes.Add(item.id, new RuneRef(item.id, item));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static RuneRef Lookup(int runeID)
	{
		if (_runes.ContainsKey(runeID))
		{
			return _runes[runeID];
		}
		return null;
	}

	public static List<RuneRef> LookupList(int[] runeIDs)
	{
		List<RuneRef> list = new List<RuneRef>();
		for (int i = 0; i < runeIDs.Length; i++)
		{
			RuneRef runeRef = Lookup(runeIDs[i]);
			if (runeRef != null)
			{
				list.Add(runeRef);
			}
		}
		return list;
	}
}
