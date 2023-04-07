using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.dailyquestslist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public Transform UIItemIcon;

	public TextMeshProUGUI UIName;

	public TextMeshProUGUI UIDesc;

	public Transform UIProgressBar;

	public Image UIProgressBarFill;

	public TextMeshProUGUI UIProgress;

	public Button UILoot;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<Transform>("ItemIcon", out UIItemIcon);
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		root.GetComponentAtPath<TextMeshProUGUI>("DescTxt", out UIDesc);
		root.GetComponentAtPath<Transform>("ProgressBar", out UIProgressBar);
		root.GetComponentAtPath<Image>("ProgressBar/ProgressBarFill", out UIProgressBarFill);
		root.GetComponentAtPath<TextMeshProUGUI>("ProgressBar/ProgressTxt", out UIProgress);
		root.GetComponentAtPath<Button>("LootBtn", out UILoot);
	}
}
