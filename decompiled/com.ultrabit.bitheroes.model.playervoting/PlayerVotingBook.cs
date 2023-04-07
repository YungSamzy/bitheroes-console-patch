using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.date;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.playervoting;

public class PlayerVotingBook
{
	private static List<PlayerVotingRef> _positions;

	private static string _name;

	private static DateRef _dateRef;

	private static bool _activeVoting;

	private static int _elegibleAccountsMinLevel;

	public static string name => _name;

	public static bool activeVoting => _activeVoting;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}
}
