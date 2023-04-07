using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.ability;

public class AbilityBook
{
	private static Dictionary<string, AbilityPositionRef> _positions;

	private static Dictionary<string, AbilityRef> _abilities;

	private static List<string> _abilitiesDictionaryLink;

	private static Dictionary<string, List<AbilityRef>> _abilityList;

	private static List<AbilityActionRef> _actions;

	private static List<string> _actionsDictionaryLink;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_positions = new Dictionary<string, AbilityPositionRef>();
		_abilities = new Dictionary<string, AbilityRef>();
		_abilityList = new Dictionary<string, List<AbilityRef>>();
		_abilitiesDictionaryLink = new List<string>();
		_actions = new List<AbilityActionRef>();
		AbilityBookData.Position[] array = XMLBook.instance.abilityBook.lstPosition.ToArray();
		foreach (AbilityBookData.Position position in array)
		{
			if (!_positions.ContainsKey(position.link))
			{
				_positions.Add(position.link, new AbilityPositionRef(position));
			}
			if (XMLBook.instance.UpdateProcessingCount())
			{
				yield return null;
			}
		}
		int abilityID = 0;
		AbilityBookData.Ability[] array2 = XMLBook.instance.abilityBook.lstAbility.ToArray();
		foreach (AbilityBookData.Ability ability in array2)
		{
			if (!_abilities.ContainsKey(ability.link))
			{
				AbilityRef value = new AbilityRef(abilityID, ability);
				_abilities.Add(ability.link, value);
			}
			abilityID++;
			_abilitiesDictionaryLink.Add(ability.link);
			if (XMLBook.instance.UpdateProcessingCount())
			{
				yield return null;
			}
		}
		AbilityBookData.Abilities[] array3 = XMLBook.instance.abilityBook.lstAbilities.ToArray();
		foreach (AbilityBookData.Abilities abilities in array3)
		{
			if (!_abilityList.ContainsKey(abilities.link))
			{
				_abilityList.Add(abilities.link, LookupLinks(abilities.links));
			}
			if (XMLBook.instance.UpdateProcessingCount())
			{
				yield return null;
			}
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static void AddActionRef(AbilityActionRef actionRef)
	{
		_actions.Add(actionRef);
	}

	public static List<AbilityRef> LookupAbilities(string link)
	{
		if (link != null && _abilityList.ContainsKey(link))
		{
			return _abilityList[link];
		}
		if (link != null)
		{
			D.LogError("AbilityBook null abilities for link " + link);
		}
		return null;
	}

	public static AbilityRef Lookup(int id)
	{
		if (id >= 0 && id < _abilitiesDictionaryLink.Count)
		{
			return _abilities[_abilitiesDictionaryLink[id]];
		}
		return null;
	}

	public static List<AbilityRef> LookupIDs(int[] ids)
	{
		List<AbilityRef> list = new List<AbilityRef>();
		for (int i = 0; i < ids.Length; i++)
		{
			AbilityRef abilityRef = Lookup(ids[i]);
			if (abilityRef != null)
			{
				list.Add(abilityRef);
			}
		}
		return list;
	}

	public static AbilityRef LookupLink(string link)
	{
		if (link != null && _abilities.ContainsKey(link))
		{
			return _abilities[link];
		}
		return null;
	}

	public static List<AbilityRef> LookupLinks(string link)
	{
		List<AbilityRef> list = new List<AbilityRef>();
		string[] array = link.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			AbilityRef abilityRef = LookupLink(array[i]);
			if (abilityRef != null)
			{
				list.Add(abilityRef);
			}
		}
		return list;
	}

	public static AbilityPositionRef LookupPositionLink(string link)
	{
		if (link != null && _positions.ContainsKey(link))
		{
			return _positions[link];
		}
		return null;
	}

	public static AbilityActionRef LookupAction(int id)
	{
		if (id >= 0 && id < _actions.Count)
		{
			return _actions[id];
		}
		return null;
	}
}
