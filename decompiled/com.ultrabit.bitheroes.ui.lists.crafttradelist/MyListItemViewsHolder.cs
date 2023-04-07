using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.crafttradelist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public Button craftBtn;

	public RectTransform result;

	public Transform requirementsContainer;

	public Transform quantityContainer;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<Button>("CraftBtn", out craftBtn);
		root.GetComponentAtPath<RectTransform>("ResultImg", out result);
		root.GetComponentAtPath<Transform>("IngredientsCont", out requirementsContainer);
		root.GetComponentAtPath<Transform>("QuantityCont", out quantityContainer);
	}
}
