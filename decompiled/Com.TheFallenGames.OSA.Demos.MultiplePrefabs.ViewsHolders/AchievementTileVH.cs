using System;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.multiplePrefabsAchievementsList;
using com.ultrabit.bitheroes.ui.utility;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;

public class AchievementTileVH : BaseVH
{
	public Image UserTrophyImg;

	public TextMeshProUGUI UIName;

	public TextMeshProUGUI UIDesc;

	public Transform UIProgressBar;

	public Image UIProgressBarFill;

	public TextMeshProUGUI UIProgress;

	public Button UIChest;

	public Button UIClaim;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<Image>("TrophyImg", out UserTrophyImg);
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		root.GetComponentAtPath<TextMeshProUGUI>("DescTxt", out UIDesc);
		root.GetComponentAtPath<Transform>("ProgressBar", out UIProgressBar);
		root.GetComponentAtPath<Image>("ProgressBar/ProgressBarFill", out UIProgressBarFill);
		root.GetComponentAtPath<TextMeshProUGUI>("ProgressBar/ProgressTxt", out UIProgress);
		root.GetComponentAtPath<Button>("Chest", out UIChest);
		root.GetComponentAtPath<Button>("ClaimBtn", out UIClaim);
	}

	public override bool CanPresentModelType(Type modelType)
	{
		return modelType == typeof(FriendTileModel);
	}

	public void UpdateACTViews(BaseModel pmodel, TrophyHandler trophyHandler)
	{
		base.UpdateViews(pmodel);
		AchievementTileModel model = pmodel as AchievementTileModel;
		trophyHandler.ReplaceTrophyByRarity(UserTrophyImg.gameObject, model.achievementData.achievementRef.rarityRef);
		UIName.text = model.achievementData.achievementRef.rarityRef.ConvertString(model.achievementData.achievementRef.name + ":");
		UIDesc.text = model.achievementData.achievementRef.desc;
		int num = 100;
		bool completed = model.achievementData.completed;
		UIClaim.gameObject.SetActive(completed);
		UIProgressBar.gameObject.SetActive(!completed);
		UIChest.gameObject.SetActive(!completed);
		if (completed)
		{
			UIClaim.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_claim");
			UIClaim.onClick.RemoveAllListeners();
			UIClaim.onClick.AddListener(delegate
			{
				OnLootBtn(model);
			});
			return;
		}
		Color color = Color.white;
		ColorUtility.TryParseHtmlString("#" + model.achievementData.achievementRef.rarityRef.textColor, out color);
		UIProgressBarFill.color = color;
		if (model.achievementData.achievementRef.amount < 0)
		{
			UIProgress.text = com.ultrabit.bitheroes.model.utility.Util.NumberFormat(model.achievementData.progress) + "%";
		}
		else
		{
			num = model.achievementData.achievementRef.amount;
			UIProgress.text = com.ultrabit.bitheroes.model.utility.Util.NumberFormat(model.achievementData.progress) + "/" + com.ultrabit.bitheroes.model.utility.Util.NumberFormat(model.achievementData.achievementRef.amount);
		}
		UIProgressBarFill.GetComponent<RegularBarFill>().UpdateBar(model.achievementData.progress, num);
		UIChest.onClick.RemoveAllListeners();
		UIChest.onClick.AddListener(delegate
		{
			OnCheckLootBtn(model);
		});
	}

	public void OnLootBtn(AchievementTileModel model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoCharacterAchievementLoot(model.achievementData);
	}

	public void OnCheckLootBtn(AchievementTileModel model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoCharacterAchievementCheckLoot(model.achievementData);
	}
}
