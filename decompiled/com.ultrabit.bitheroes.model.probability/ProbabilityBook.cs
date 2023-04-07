using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.probability;

public class ProbabilityBook
{
	private static Dictionary<string, ProbabilityRef> _probabilities;

	public static int size => _probabilities.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_probabilities = new Dictionary<string, ProbabilityRef>();
		int num = 0;
		foreach (ProbabilityBookData.Probability item in XMLBook.instance.probabilityBook.lstProbability)
		{
			_probabilities.Add(item.link, new ProbabilityRef(num, item));
			num++;
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static ProbabilityRef Lookup(string link)
	{
		if (link != null && _probabilities.ContainsKey(link))
		{
			return _probabilities[link];
		}
		return null;
	}
}
