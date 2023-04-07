using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.brawlsearchlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI UIName;

	public TextMeshProUGUI UITier;

	public TextMeshProUGUI UIDifficulty;

	public TextMeshProUGUI UIEnergy;

	public Button UIJoin;

	public RectTransform UIPlayers;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		root.GetComponentAtPath<TextMeshProUGUI>("TierTxt", out UITier);
		root.GetComponentAtPath<TextMeshProUGUI>("DifficultyTxt", out UIDifficulty);
		root.GetComponentAtPath<TextMeshProUGUI>("EnergyTxt", out UIEnergy);
		root.GetComponentAtPath<Button>("JoinBtn", out UIJoin);
		root.GetComponentAtPath<RectTransform>("PlaceholderPlayers", out UIPlayers);
	}
}
