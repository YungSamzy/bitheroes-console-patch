using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.equipmentupgradelist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public Button craftBtn;

	public RectTransform result;

	public Transform requirementsContainer;

	public Transform quantityContainer;

	public Transform requiredPlaceholder;

	public Image backgroundImage;

	public TextMeshProUGUI powerTxt;

	public TextMeshProUGUI staminaTxt;

	public TextMeshProUGUI agilityTxt;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<Button>("CraftBtn", out craftBtn);
		root.GetComponentAtPath<RectTransform>("ResultImg", out result);
		root.GetComponentAtPath<Transform>("IngredientsCont", out requirementsContainer);
		root.GetComponentAtPath<Transform>("QuantityCont", out quantityContainer);
		root.GetComponentAtPath<Transform>("RequiredPlaceholder", out requiredPlaceholder);
		root.GetComponentAtPath<TextMeshProUGUI>("PowerTxt", out powerTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("StaminaTxt", out staminaTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("AgilityTxt", out agilityTxt);
		backgroundImage = root.GetComponent<Image>();
	}
}
