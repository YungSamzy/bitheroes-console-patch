using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.gamemodifiertimelist;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.game;

public class GameModifierTimeWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI sourceNameTxt;

	public TextMeshProUGUI timeNameTxt;

	public TextMeshProUGUI bonusesNameTxt;

	public TimeBarColor timeBar;

	public GameModifierTimeList gameModifierTimeList;

	private long _millisecondsRemaining;

	private long _millisecondsTotal;

	private List<GameModifier> _modifiers;

	private IEnumerator _timer;

	private float _timerTime;

	private UnityEvent COMPLETE = new UnityEvent();

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(string name, long millisecondsRemaining, long millisecondsTotal, List<GameModifier> modifiers, UnityAction OnComplete)
	{
		_millisecondsRemaining = millisecondsRemaining;
		_millisecondsTotal = millisecondsTotal;
		_modifiers = modifiers;
		COMPLETE.AddListener(OnComplete);
		_timerTime = Time.realtimeSinceStartup;
		topperTxt.text = Language.GetString("ui_bonus");
		sourceNameTxt.text = name;
		timeNameTxt.text = Language.GetString("ui_time_remaining") + ":";
		bonusesNameTxt.text = Language.GetString("ui_bonuses") + ":";
		timeBar.SetMaxValueMilliseconds(_millisecondsTotal);
		timeBar.SetCurrentValueMilliseconds(_millisecondsRemaining);
		timeBar.COMPLETE.AddListener(OnComplete);
		timeBar.COMPLETE.AddListener(OnClose);
		GameData.instance.PROJECT.PauseDungeon();
		gameModifierTimeList.InitList();
		CreateList();
		ListenForBack(OnClose);
		CreateWindow();
		if (GetMillisecondsRemaining() <= 0)
		{
			OnClose();
		}
	}

	public void CreateList()
	{
		gameModifierTimeList.ClearList();
		for (int i = 0; i < _modifiers.Count; i++)
		{
			string modifierDesc = ((_modifiers[i] != null) ? ((_modifiers[i].desc != null) ? Util.ParseModifierString(_modifiers[i]) : GameModifier.getTypeDescriptionShort(_modifiers[i].type, _modifiers[i].value)) : "-");
			gameModifierTimeList.Data.InsertOneAtEnd(new GameModifierTimeItem
			{
				modifierDesc = modifierDesc
			});
		}
	}

	public long GetMillisecondsRemaining()
	{
		return (long)((float)_millisecondsRemaining - (Time.realtimeSinceStartup - _timerTime) * 1000f);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}

	public override void DoDestroy()
	{
		timeBar.COMPLETE.RemoveAllListeners();
		GameData.instance.PROJECT.ResumeDungeon();
		base.DoDestroy();
	}
}
