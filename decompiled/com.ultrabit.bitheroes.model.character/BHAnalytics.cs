using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.kongregate;

namespace com.ultrabit.bitheroes.model.character;

public class BHAnalytics
{
	public static int GOLD_BOUGHT = 0;

	public static int GOLD_SPENT = 1;

	public static int GOLD_EARNED = 2;

	public static int CREDITS_BOUGHT = 3;

	public static int CREDITS_SPENT = 4;

	public static int CREDITS_EARNED = 5;

	public static int DUNGEONS_PLAYED = 6;

	public static int DUNGEONS_WON = 7;

	public static int PVP_BATTLES_PLAYED = 8;

	public static int PVP_BATTLES_WON = 9;

	private static List<string> NAMES = new List<string> { "Gold Bought", "Gold Spent", "Gold Earned", "Credits Bought", "Credits Spent", "Credits Earned", "Dungeons Played", "Dungeons Won", "PvP Battles Played", "PvP Battles Won" };

	private int _charID;

	private Dictionary<int, int> _data;

	public BHAnalytics(int charID)
	{
		_charID = charID;
		_data = GameData.instance.SAVE_STATE.GetAnalytics(_charID);
		if (_data == null)
		{
			_data = new Dictionary<int, int>();
		}
	}

	public void setValue(int id, int value, bool update = true)
	{
		getValue(id);
		if (_data.ContainsKey(id))
		{
			_data[id] = value;
		}
		else
		{
			_data.Add(id, value);
		}
		GameData.instance.SAVE_STATE.SetAnalytics(_charID, _data);
		if (update)
		{
			KongregateAnalytics.updateCommonFields();
		}
	}

	public int getValue(int id)
	{
		if (_data.ContainsKey(id))
		{
			return _data[id];
		}
		return 0;
	}

	public void adjustValue(int id, int value, bool update = true)
	{
		int value2 = getValue(id);
		int num = value2 + value;
		if (value2 != num)
		{
			setValue(id, num, update);
		}
	}

	public void incrementValue(int id, bool update = true)
	{
		int value = getValue(id) + 1;
		setValue(id, value, update);
	}

	public static string getName(int id)
	{
		return NAMES[id];
	}
}
