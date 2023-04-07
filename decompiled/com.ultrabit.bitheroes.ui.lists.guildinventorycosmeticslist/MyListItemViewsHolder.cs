using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.guildinventorycosmeticslist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public Image UIFrame;

	public Image UIAsset;

	public TextMeshProUGUI UIName;

	public TextMeshProUGUI UIType;

	public TextMeshProUGUI UILevel;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<Image>("Frame", out UIFrame);
		root.GetComponentAtPath<Image>("Asset", out UIAsset);
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		root.GetComponentAtPath<TextMeshProUGUI>("TypeTxt", out UIType);
		root.GetComponentAtPath<TextMeshProUGUI>("LevelTxt", out UILevel);
	}
}
