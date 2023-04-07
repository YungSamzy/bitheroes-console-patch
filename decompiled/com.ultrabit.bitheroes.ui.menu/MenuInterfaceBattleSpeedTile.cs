using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.battle;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceBattleSpeedTile : MainUIButton
{
	public TextMeshProUGUI speedTxt;

	public Image icon;

	public Sprite[] speedIcons;

	private Battle _battle;

	private int _speedIndex;

	public int speedIndex => _speedIndex;

	public void Create(Battle battle)
	{
		LoadDetails(Language.GetString("ui_battle_speed"), VariableBook.GetGameRequirement(14));
		_battle = battle;
		SetSpeed(GameData.instance.SAVE_STATE.GetBattleSpeed(GameData.instance.PROJECT.character.id));
	}

	public void SetSpeed(int index)
	{
		if (index <= 0)
		{
			index = 0;
		}
		else if (index >= Battle.BATTLE_SPEEDS.Length - 1)
		{
			index = Battle.BATTLE_SPEEDS.Length - 1;
		}
		_speedIndex = index;
		icon.sprite = speedIcons[index];
		DoUpdate();
	}

	public override void DoUpdate()
	{
		base.DoUpdate();
		if (!(_battle == null))
		{
			float speed = _battle.GetSpeed(speedIndex);
			float num = 100f;
			string text = Util.NumberFormat(Mathf.Round(speed * num) / num);
			speedTxt.text = Language.GetString("ui_number_multiplier", new string[1] { text });
			UpdateGrayscale();
		}
	}

	public override void DoClick()
	{
		base.DoClick();
		if (_speedIndex >= Battle.BATTLE_SPEEDS.Length - 1)
		{
			_speedIndex = 0;
		}
		else
		{
			_speedIndex++;
		}
		SetSpeed(_speedIndex);
		GameData.instance.SAVE_STATE.SetBattleSpeed(GameData.instance.PROJECT.character.id, _speedIndex);
	}
}
