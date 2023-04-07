using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.eventsalesshoplist;

public class EventSalesItemViewsHolder : CellViewsHolder
{
	public TextMeshProUGUI UIName;

	public Image UIBg;

	public Image UIItemIconColor;

	public Image UIBlueButtonBack;

	public TextMeshProUGUI UICost;

	public Image UIMaterialImage;

	public Image UIRibbon;

	public TextMeshProUGUI UIRibbonTxt;

	public override void CollectViews()
	{
		base.CollectViews();
		views.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		views.GetComponentAtPath<Image>("BackgroundImg", out UIBg);
		views.GetComponentAtPath<Image>("ItemIcon/Views/ItemIconColor", out UIItemIconColor);
		views.GetComponentAtPath<Image>("BlueButtonImg", out UIBlueButtonBack);
		views.GetComponentAtPath<TextMeshProUGUI>("CostTxt", out UICost);
		views.GetComponentAtPath<Image>("MaterialImage", out UIMaterialImage);
		views.GetComponentAtPath<Image>("ItemIcon/Ribbon", out UIRibbon);
		views.GetComponentAtPath<TextMeshProUGUI>("ItemIcon/Ribbon/RibbonTxt", out UIRibbonTxt);
	}
}
