using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using com.ultrabit.bitheroes.ui.language;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.guildshoplist;

public class MyGridItemViewsHolder : CellViewsHolder
{
	public TextMeshProUGUI UIName;

	public TextMeshProUGUI UIReq;

	public Image UIBg;

	public Image UIItemIconColor;

	public Image UIBlueButtonBack;

	public TextMeshProUGUI UICost;

	private AsianLanguageFontManager asianLangManager;

	public override void CollectViews()
	{
		base.CollectViews();
		views.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		views.GetComponentAtPath<TextMeshProUGUI>("ReqTxt", out UIReq);
		views.GetComponentAtPath<Image>("Bg", out UIBg);
		views.GetComponentAtPath<Image>("ItemIcon/Views/ItemIconColor", out UIItemIconColor);
		views.GetComponentAtPath<Image>("BlueButtonBack", out UIBlueButtonBack);
		views.GetComponentAtPath<TextMeshProUGUI>("CostTxt", out UICost);
		asianLangManager = root.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = root.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
	}
}
