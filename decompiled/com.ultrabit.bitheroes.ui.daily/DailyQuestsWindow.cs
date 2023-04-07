using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.daily;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.lists.dailyquestslist;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.daily;

public class DailyQuestsWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI questsDescTxt;

	public TextMeshProUGUI timeDescTxt;

	public TextMeshProUGUI questsTxt;

	public Button helpBtn;

	public Button achievementsBtn;

	public TimeBarColor timeBar;

	public DailyQuestsList dailyQuestsList;

	private bool _updatePending;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("daily_quest_plural_name");
		timeDescTxt.text = Language.GetString("daily_quest_new_desc");
		questsDescTxt.text = Language.GetString("daily_quest_plural_name") + ":";
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		achievementsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_achievements");
		UpdateText();
		UpdateTime();
		dailyQuestsList.InitList();
		GameData.instance.PROJECT.character.dailyQuests.setUpdated(v: false);
		GameData.instance.PROJECT.character.AddListener("DAILY_QUEST_CHANGE", OnDailyQuestChange);
		GameData.instance.PROJECT.character.Broadcast("DAILY_QUEST_CHANGE");
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		timeBar.COMPLETE.AddListener(ResetTime);
		GameData.instance.PROJECT.PauseDungeon();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null)
		{
			return;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(49))
		{
			MyListItemViewsHolder lootableTile = GetLootableTile();
			if (lootableTile != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(49);
				GameData.instance.tutorialManager.ShowTutorialForButton(lootableTile.UILoot.gameObject, new TutorialPopUpSettings(Tutorial.GetText(49), 4, lootableTile.UILoot.gameObject), stageTrigger: true, EventTriggerType.PointerClick, funcSameAsTargetFunc: false, OnTutorialEnded, shadow: false, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(131) && achievementsBtn != null)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(131);
			GameData.instance.tutorialManager.ShowTutorialForButton(achievementsBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(131), 4, achievementsBtn.gameObject), stageTrigger: false, EventTriggerType.PointerClick, funcSameAsTargetFunc: false, OnTutorialEnded, shadow: true, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	private MyListItemViewsHolder GetLootableTile()
	{
		for (int i = 0; i < dailyQuestsList.Data.Count; i++)
		{
			if (dailyQuestsList.GetBaseItemViewsHolderIfVisible(i) is MyListItemViewsHolder result && dailyQuestsList.Data[i].questData.completed && !dailyQuestsList.Data[i].questData.looted)
			{
				return result;
			}
		}
		return null;
	}

	private void OnTutorialEnded(object e)
	{
		if (_updatePending)
		{
			_updatePending = false;
			OnDailyQuestChange();
		}
		CheckTutorial();
	}

	public void RefreshList()
	{
		dailyQuestsList.ClearList();
		List<DailyQuestData> list = new List<DailyQuestData>();
		list.AddRange(GameData.instance.PROJECT.character.dailyQuests.quests);
		list = (from x in list
			orderby x.percentage descending, x.rarity descending, x.id descending
			select x).ToList();
		for (int i = 0; i < list.Count; i++)
		{
			dailyQuestsList.Data.InsertOneAtEnd(new DailyQuestItem
			{
				questData = list[i]
			});
		}
	}

	private void UpdateText()
	{
		questsTxt.text = Util.NumberFormat(GameData.instance.PROJECT.character.dailyQuests.quests.Count) + "/" + Util.NumberFormat(VariableBook.dailyQuestLimit);
	}

	private void OnDailyQuestChange()
	{
		if (GameData.instance.tutorialManager.hasPopup)
		{
			_updatePending = true;
			return;
		}
		RefreshList();
		UpdateText();
	}

	public void UpdateTime()
	{
		timeBar.SetCurrentValueMilliseconds(ServerExtension.instance.GetMillisecondsTillDayEnds());
		timeBar.SetMaxValueMilliseconds(86400000L);
	}

	private void ResetTime()
	{
		timeBar.SetCurrentValueMilliseconds(86400000L);
		timeBar.SetMaxValueMilliseconds(86400000L);
	}

	public void OnAchievemntsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewCharacterAchievementsWindow();
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("daily_quest_plural_name"), Language.GetString("daily_quest_help_desc", new string[2]
		{
			Util.NumberFormat(VariableBook.dailyQuestGain),
			Util.NumberFormat(VariableBook.dailyQuestLimit)
		}, color: true));
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("DAILY_QUEST_CHANGE", OnDailyQuestChange);
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		timeBar.COMPLETE.RemoveListener(ResetTime);
		GameData.instance.PROJECT.ResumeDungeon();
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		helpBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		helpBtn.interactable = false;
	}
}
