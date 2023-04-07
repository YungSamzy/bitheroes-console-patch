using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.consumable;

public class ConsumableModifierData
{
	private ConsumableRef _consumableRef;

	private long _milliseconds;

	private float _startTime;

	public ConsumableRef consumableRef => _consumableRef;

	public ConsumableModifierData(ConsumableRef consumableRef, long milliseconds)
	{
		_consumableRef = consumableRef;
		setMilliseconds(milliseconds);
	}

	public void setMilliseconds(long milliseconds)
	{
		_milliseconds = milliseconds;
		_startTime = Time.realtimeSinceStartup;
	}

	public long getMillisecondsRemaining()
	{
		return (long)((float)_milliseconds - (Time.realtimeSinceStartup - _startTime) * 1000f);
	}

	public bool isActive()
	{
		return getMillisecondsRemaining() > 0;
	}

	public static ConsumableModifierData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("ite0");
		return new ConsumableModifierData(milliseconds: sfsob.GetLong("ite7"), consumableRef: ConsumableBook.Lookup(@int));
	}

	public static List<ConsumableModifierData> listFromSFSObject(ISFSObject sfsob)
	{
		List<ConsumableModifierData> list = new List<ConsumableModifierData>();
		ISFSArray sFSArray = sfsob.GetSFSArray("cha33");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ConsumableModifierData item = fromSFSObject(sFSArray.GetSFSObject(i));
			list.Add(item);
		}
		return list;
	}

	public static bool listContainsData(List<ConsumableModifierData> list, ConsumableModifierData data)
	{
		foreach (ConsumableModifierData item in list)
		{
			if (item == data)
			{
				return true;
			}
		}
		return false;
	}
}
