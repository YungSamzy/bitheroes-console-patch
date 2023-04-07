using System;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using com.ultrabit.bitheroes.ui.lists.multiplePrefabsAchievementsCompletedList;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;

public class AchievementDifficultyViewTileVH : BaseVH
{
	private TextMeshProUGUI TitleTxt;

	public Button claimBtn;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("TitleTxt", out TitleTxt);
		root.GetComponentAtPath<Button>("ClaimBtn", out claimBtn);
	}

	public override bool CanPresentModelType(Type modelType)
	{
		return modelType == typeof(FriendRequestViewTileModel);
	}

	public override void UpdateViews(BaseModel model)
	{
		base.UpdateViews(model);
		AchievementDificultyViewTileModel achievementDificultyViewTileModel = model as AchievementDificultyViewTileModel;
		TitleTxt.text = achievementDificultyViewTileModel.TrophyRarity;
		claimBtn.gameObject.SetActive(achievementDificultyViewTileModel.showClaimButton);
		claimBtn.onClick.RemoveAllListeners();
		claimBtn.onClick.AddListener(achievementDificultyViewTileModel.claimBtnAction);
	}
}
