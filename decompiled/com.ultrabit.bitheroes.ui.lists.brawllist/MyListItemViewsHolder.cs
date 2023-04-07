using Com.TheFallenGames.OSA.Core;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.language;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.brawllist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public SummonBtn summonBtn;

	public TextMeshProUGUI txtTitle;

	public TextMeshProUGUI txtDescription;

	public RectTransform placeholderPromo;

	private AsianLanguageFontManager asianLangManager;

	public override void CollectViews()
	{
		base.CollectViews();
		summonBtn = root.GetComponentInChildren<SummonBtn>();
		summonBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_summon");
		root.GetComponentAtPath<TextMeshProUGUI>("RaidTitleTxt", out txtTitle);
		root.GetComponentAtPath<TextMeshProUGUI>("RaidDescTxt", out txtDescription);
		root.GetComponentAtPath<RectTransform>("PlaceholderPromo", out placeholderPromo);
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
