using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.boober;

public class BobberBook
{
	private static Dictionary<int, BobberRef> _bobbers;

	public static int size => _bobbers.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_bobbers = new Dictionary<int, BobberRef>();
		foreach (BobberBookData.Bobber item in XMLBook.instance.bobberBook.lstBobber)
		{
			BobberRef bobberRef = new BobberRef(item.id, item);
			bobberRef.LoadDetails(item);
			_bobbers.Add(item.id, bobberRef);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static BobberRef Lookup(int id)
	{
		if (_bobbers.ContainsKey(id))
		{
			return _bobbers[id];
		}
		return null;
	}
}
