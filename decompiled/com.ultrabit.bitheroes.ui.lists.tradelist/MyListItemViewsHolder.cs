using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.tradelist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public Image backgroundImage;

	public Button craftBtn;

	public RectTransform requiredPlaceholder;

	public RectTransform resultIcon;

	public Image resultIconBg;

	public TextMeshProUGUI unlocksText1;

	public TextMeshProUGUI unlocksText2;

	public override void CollectViews()
	{
		base.CollectViews();
		backgroundImage = root.GetComponent<Image>();
		root.GetComponentAtPath<RectTransform>("ResultImg", out resultIcon);
		root.GetComponentAtPath<Image>("ResultImg/Views/ItemIconColor", out resultIconBg);
		root.GetComponentAtPath<Button>("CraftBtn", out craftBtn);
		root.GetComponentAtPath<RectTransform>("RequiredPlaceholder", out requiredPlaceholder);
		root.GetComponentAtPath<TextMeshProUGUI>("UnlocksText1", out unlocksText1);
		root.GetComponentAtPath<TextMeshProUGUI>("UnlocksText2", out unlocksText2);
	}
}
