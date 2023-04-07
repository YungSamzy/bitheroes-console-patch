using Com.TheFallenGames.OSA.Core;
using com.ultrabit.bitheroes.ui.ability;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.utility;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.abilitylist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public Image bg;

	public Image abilityBack;

	public Image abilityIcon;

	public Image loadingIcon;

	public TextMeshProUGUI description;

	public AbilityListTileOverlay abilityOverlay;

	public HoverImages hoverImages;

	private AsianLanguageFontManager asianLangManager;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<Image>("AbilityBack/AbilityImg", out abilityIcon);
		root.GetComponentAtPath<Image>("AbilityBack/AbilityImg/LoadingIcon", out loadingIcon);
		root.GetComponentAtPath<Image>("AbilityBack", out abilityBack);
		root.GetComponentAtPath<AbilityListTileOverlay>("AbilityBack/Notches/AbilityOverlay", out abilityOverlay);
		root.GetComponentAtPath<Image>("bg", out bg);
		root.GetComponentAtPath<TextMeshProUGUI>("DescTxt", out description);
		hoverImages = root.GetComponent<HoverImages>();
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
