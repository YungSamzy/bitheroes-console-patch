using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.daily;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.dailyrewardlist;
using com.ultrabit.bitheroes.ui.tutorial;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.daily;

public class DailyRewardWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button claimBtn;

	public DailyRewardList dailyRewardList;

	public void LoadDetails()
	{
		Disable();
		topperTxt.text = Language.GetString("daily_reward_plural_name");
		claimBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_claim");
		dailyRewardList.InitList();
		CreateList();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		DailyBonusBook.GetCurrentBonusRef();
		int groupIndex = Mathf.FloorToInt(GameData.instance.PROJECT.character.dailyID / 5);
		dailyRewardList.ScrollToGroup(groupIndex);
		ListenForBack(OnClaimBtn);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && !GameData.instance.PROJECT.character.tutorial.GetState(37))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(37);
			GameData.instance.tutorialManager.ShowTutorialForButton(claimBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(37), 4, claimBtn.gameObject), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	private void CreateList()
	{
		dailyRewardList.ClearList();
		string dailyRewardStep = GameData.instance.PROJECT.character.extraInfo.getDailyRewardStep();
		for (int i = 1; i <= DailyRewardBook.getSizeByRewardKey(dailyRewardStep); i++)
		{
			DailyRewardRef dailyRewardRef = DailyRewardBook.Lookup(i, dailyRewardStep);
			if (dailyRewardRef != null)
			{
				dailyRewardList.Data.InsertOneAtEnd(new DailyRewardItem
				{
					dailyRef = dailyRewardRef
				});
			}
		}
	}

	public void OnClaimBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (!base.disabled)
		{
			Disable();
			GameData.instance.main.ShowLoading();
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnDailyReward);
			CharacterDALC.instance.doDailyReward();
		}
	}

	private void OnDailyReward(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnDailyReward);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		int @int = sfsob.GetInt("cha25");
		DateTime dateFromString = Util.GetDateFromString(sfsob.GetUtfString("cha26"));
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		GameData.instance.PROJECT.character.dailyID = @int;
		GameData.instance.PROJECT.character.dailyDate = dateFromString;
		GameData.instance.PROJECT.character.addItems(list);
		KongregateAnalytics.checkEconomyTransaction("Daily Login Reward", null, list, sfsob, "Daily Login", 2);
		GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true).SCROLL_OUT_START.AddListener(OnItemsClose);
	}

	private void OnItemsClose(object e)
	{
		base.OnClose();
	}

	public override void DoDestroy()
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (claimBtn != null)
		{
			claimBtn.interactable = true;
		}
		CheckTutorial();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		claimBtn.interactable = false;
	}
}
