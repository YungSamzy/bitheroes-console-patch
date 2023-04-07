using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.instance;

public class InstanceBook
{
	private static Dictionary<int, InstanceRef> _instance;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_instance = new Dictionary<int, InstanceRef>();
		foreach (InstanceBookData.Instance item in XMLBook.instance.instanceBook.lstInstance)
		{
			if (_instance.ContainsKey(item.id))
			{
				D.LogError($"InstanceBook - Duplicated instance entry for id {item.id}");
			}
			else
			{
				_instance.Add(item.id, new InstanceRef(item.id, item));
			}
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static InstanceRef Lookup(int id)
	{
		if (_instance.ContainsKey(id))
		{
			return _instance[id];
		}
		return null;
	}

	public static InstanceRef GetFirstInstanceByType(int type)
	{
		foreach (KeyValuePair<int, InstanceRef> item in _instance)
		{
			if (item.Value.type == type)
			{
				return item.Value;
			}
		}
		return null;
	}
}
