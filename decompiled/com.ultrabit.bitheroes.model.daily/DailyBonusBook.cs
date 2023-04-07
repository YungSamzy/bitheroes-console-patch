using System;
using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.daily;

public class DailyBonusBook
{
	private static List<DailyBonusRef> _bonuses;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_bonuses = new List<DailyBonusRef>();
		int num = 0;
		foreach (DailyBonusBookData.Daily item in XMLBook.instance.dailyBonusBook.lstDaily)
		{
			DailyBonusRef dailyBonusRef = new DailyBonusRef(num, item);
			dailyBonusRef.LoadDetails(item);
			_bonuses.Add(dailyBonusRef);
			num++;
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static DailyBonusRef GetCurrentBonusRef()
	{
		DateTime date = ServerExtension.instance.GetDate();
		foreach (DailyBonusRef bonuse in _bonuses)
		{
			if (bonuse.hasDate && bonuse.date.Day == date.Day && bonuse.date.Month == date.Month && bonuse.date.Year == date.Year)
			{
				return bonuse;
			}
			if (bonuse.day == (int)date.DayOfWeek)
			{
				return bonuse;
			}
		}
		return null;
	}

	public static DailyBonusRef GetNextBonusRef()
	{
		DateTime dateTime = ServerExtension.instance.GetDate().AddDays(1.0);
		foreach (DailyBonusRef bonuse in _bonuses)
		{
			if (bonuse.hasDate && bonuse.date.Day == dateTime.Day && bonuse.date.Month == dateTime.Month && bonuse.date.Year == dateTime.Year)
			{
				return bonuse;
			}
			if (bonuse.day == (int)dateTime.DayOfWeek)
			{
				return bonuse;
			}
		}
		return null;
	}
}
