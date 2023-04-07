using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.victory;

public class DungeonVictoryWindow : VictoryWindow
{
	private UnityAction _onCloseButton;

	public override void Start()
	{
		base.Start();
	}

	public void LoadDetails(int type = 0, List<ItemData> items = null, List<BattleStat> battleStats = null, bool isRerunnable = false, UnityAction OnCloseBtn = null)
	{
		base.LoadDetails(type, 0L, 0, items, null, shouldPlayMusic: true, Language.GetString("ui_cleared_dungeon"), Language.GetString("ui_loot_recap"), Language.GetString("ui_town"), isVictorious: true, isCloseRed: true, isRerunnable);
		closeBtn.onClick.AddListener(OnCloseBtn);
		_onCloseButton = OnCloseBtn;
		_shouldCheckTutorials = false;
		if (GameData.instance.PROJECT.character.autoPilot && AppInfo.TESTING)
		{
			Invoke("AutoClose", 1f);
		}
	}

	private void AutoClose()
	{
		_onCloseButton?.Invoke();
	}

	public override void OnClose()
	{
		if (GameData.instance.PROJECT.character.autoPilot && AppInfo.TESTING)
		{
			CancelInvoke("AutoClose");
		}
		_onCloseButton?.Invoke();
		base.OnClose();
	}

	public override void Enable()
	{
		base.Enable();
	}

	public override void Disable()
	{
		base.Disable();
	}

	public override void DoDestroy()
	{
		closeBtn.onClick.RemoveAllListeners();
		base.DoDestroy();
	}
}
