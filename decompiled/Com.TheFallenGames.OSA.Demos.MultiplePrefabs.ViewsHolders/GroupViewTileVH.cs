using System;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using com.ultrabit.bitheroes.ui.lists.MultiplePrefabsTeamList;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;

namespace Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;

public class GroupViewTileVH : BaseVH
{
	private TextMeshProUGUI groupTxt;

	private RectTransform rect;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("GroupTxt", out groupTxt);
		rect = root.GetComponent<RectTransform>();
	}

	public override bool CanPresentModelType(Type modelType)
	{
		return modelType == typeof(GroupTileModel);
	}

	public override void UpdateViews(BaseModel model)
	{
		base.UpdateViews(model);
		GroupTileModel groupTileModel = model as GroupTileModel;
		groupTxt.text = groupTileModel.titleGroup;
	}
}
