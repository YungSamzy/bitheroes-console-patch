using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.fusionlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public Image backgroundImage;

	public Button craftBtn;

	public RectTransform ingredientsCont;

	public RectTransform ResultImg;

	public override void CollectViews()
	{
		base.CollectViews();
		backgroundImage = root.GetComponent<Image>();
		root.GetComponentAtPath<RectTransform>("ResultImg", out ResultImg);
		root.GetComponentAtPath<Button>("CraftBtn", out craftBtn);
		root.GetComponentAtPath<RectTransform>("IngredientsCont", out ingredientsCont);
	}
}
