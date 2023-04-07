using System;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using com.ultrabit.bitheroes.ui.lists.multiplePrefabsAchievementsCompletedList;
using com.ultrabit.bitheroes.ui.utility;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;

public class AchievementCompletedTileVH : BaseVH
{
	public Image UserTrophyImg;

	public TextMeshProUGUI UIName;

	public TextMeshProUGUI UIDesc;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<Image>("TrophyImg", out UserTrophyImg);
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		root.GetComponentAtPath<TextMeshProUGUI>("DescTxt", out UIDesc);
	}

	public override bool CanPresentModelType(Type modelType)
	{
		return modelType == typeof(FriendTileModel);
	}

	public void UpdateACTViews(BaseModel model, TrophyHandler trophyHandler)
	{
		base.UpdateViews(model);
		AchievementCompletedTileModel achievementCompletedTileModel = model as AchievementCompletedTileModel;
		trophyHandler.ReplaceTrophyByRarity(UserTrophyImg.gameObject, achievementCompletedTileModel.achivementData.achievementRef.rarityRef);
		UIName.text = achievementCompletedTileModel.achivementData.achievementRef.rarityRef.ConvertString(achievementCompletedTileModel.achivementData.achievementRef.name + ":");
		UIDesc.text = achievementCompletedTileModel.achivementData.achievementRef.desc;
	}
}
