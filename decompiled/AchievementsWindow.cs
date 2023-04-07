using System.Collections.Generic;
using System.Linq;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.character.achievements;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui;
using com.ultrabit.bitheroes.ui.lists.multiplePrefabsAchievementsCompletedList;
using com.ultrabit.bitheroes.ui.lists.multiplePrefabsAchievementsList;
using com.ultrabit.bitheroes.ui.tutorial;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AchievementsWindow : WindowsMain
{
	private enum ListType
	{
		ACHIEVEMENTS,
		ACCOMLPLISHED
	}

	public TextMeshProUGUI topperTxt;

	public MultiplePrefabsAchievementsCompletedList achievementsCompletedList;

	public MultiplePrefabsAchievementsList achievementsList;

	public Button remainingTab;

	public Button completedTab;

	public Button swapBtn;

	private TrophyHandler trophyHandler;

	private ListType listType;

	public override void Start()
	{
		base.Start();
		Disable();
		trophyHandler = base.gameObject.AddComponent<TrophyHandler>();
		achievementsList.trophyHandler = trophyHandler;
		achievementsCompletedList.trophyHandler = trophyHandler;
		topperTxt.text = Language.GetString("ui_character_achievements_achievements");
		remainingTab.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_character_achievements_remaining");
		completedTab.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_character_achievements_completed");
		achievementsCompletedList.StartList();
		achievementsList.StartList();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		CreateWindow();
		InitAchievementsList();
		InitAchievementsCompletedList();
		SetListByType();
	}

	public void OnUpdateAchievements()
	{
		InitAchievementsList();
		InitAchievementsCompletedList();
		SetListByType();
	}

	private void SetListByType()
	{
		achievementsList.gameObject.SetActive(listType == ListType.ACHIEVEMENTS);
		Util.SetTab(remainingTab, listType == ListType.ACHIEVEMENTS, useInteractable: true);
		achievementsCompletedList.gameObject.SetActive(listType == ListType.ACCOMLPLISHED);
		Util.SetTab(completedTab, listType == ListType.ACCOMLPLISHED, useInteractable: true);
	}

	private void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && !GameData.instance.PROJECT.character.tutorial.GetState(132))
		{
			AchievementTileVH claimableAchievementVH = GetClaimableAchievementVH();
			if (claimableAchievementVH != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(132);
				GameData.instance.tutorialManager.ShowTutorialForButton(claimableAchievementVH.UIClaim.gameObject, new TutorialPopUpSettings(Tutorial.GetText(132), 2, claimableAchievementVH.UIClaim.gameObject), stageTrigger: false, EventTriggerType.PointerClick, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
		}
	}

	public void OnSwapBtn()
	{
		if (listType == ListType.ACHIEVEMENTS)
		{
			listType = ListType.ACCOMLPLISHED;
		}
		else
		{
			listType = ListType.ACHIEVEMENTS;
		}
		SetListByType();
	}

	private void InitAchievementsCompletedList()
	{
		achievementsCompletedList.ClearList();
		List<CharacterAchievementData> list = new List<CharacterAchievementData>();
		list.AddRange(GameData.instance.PROJECT.character.characterAchievements.lootedAchievements);
		list = list.OrderByDescending((CharacterAchievementData x) => x.achievementRef.category).ToList();
		string text = "";
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].achievementRef.category != text)
			{
				achievementsCompletedList.Data.InsertOneAtEnd(new AchievementDificultyViewTileModel
				{
					TrophyRarity = Language.GetString(list[i].achievementRef.category)
				});
				text = list[i].achievementRef.category;
			}
			achievementsCompletedList.Data.InsertOneAtEnd(new AchievementCompletedTileModel
			{
				achivementData = list[i]
			});
		}
	}

	private void InitAchievementsList()
	{
		achievementsList.ClearList();
		List<CharacterAchievementData> list = new List<CharacterAchievementData>();
		list.AddRange(GameData.instance.PROJECT.character.characterAchievements.activeAchievements);
		list = list.OrderByDescending((CharacterAchievementData x) => x.achievementRef.category).ToList();
		List<CharacterAchievementData> list2 = new List<CharacterAchievementData>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].completed)
			{
				list2.Add(list[i]);
			}
		}
		if (list2.Count > 0)
		{
			achievementsList.Data.InsertOneAtEnd(new AchievementDificultyViewTileModel
			{
				TrophyRarity = Language.GetString("title_completed")
			});
			for (int j = 0; j < list2.Count; j++)
			{
				achievementsList.Data.InsertOneAtEnd(new AchievementTileModel
				{
					achievementData = list2[j]
				});
			}
		}
		string text = "";
		for (int k = 0; k < list.Count; k++)
		{
			if (!list[k].completed)
			{
				if (list[k].achievementRef.category != text)
				{
					achievementsList.Data.InsertOneAtEnd(new AchievementDificultyViewTileModel
					{
						TrophyRarity = Language.GetString(list[k].achievementRef.category)
					});
					text = list[k].achievementRef.category;
				}
				achievementsList.Data.InsertOneAtEnd(new AchievementTileModel
				{
					achievementData = list[k]
				});
			}
		}
	}

	private AchievementTileVH GetClaimableAchievementVH()
	{
		for (int i = 0; i < achievementsList.Data.Count; i++)
		{
			if (achievementsList.GetBaseItemViewsHolderIfVisible(i) is AchievementTileVH result && achievementsList.Data[i] is AchievementTileModel)
			{
				AchievementTileModel achievementTileModel = achievementsList.Data[i] as AchievementTileModel;
				if (achievementTileModel.achievementData.completed && !achievementTileModel.achievementData.looted)
				{
					return result;
				}
			}
		}
		return null;
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
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
}
