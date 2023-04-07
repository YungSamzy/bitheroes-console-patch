using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.encounter;

public class EncounterBook
{
	private static Dictionary<string, EncounterRef> _encounters;

	public static int size => _encounters.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_encounters = new Dictionary<string, EncounterRef>();
		int num = 0;
		foreach (EncounterBookData.Encounter item in XMLBook.instance.encounterBook.lstEncounter)
		{
			_encounters.Add(item.link, new EncounterRef(num, item));
			num++;
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static EncounterRef LookupLink(string link)
	{
		if (_encounters.ContainsKey(link))
		{
			return _encounters[link];
		}
		return null;
	}
}
