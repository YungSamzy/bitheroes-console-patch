using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.booster;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceBoostersTile : MainUIButton
{
	public TextMeshProUGUI badge;

	public Image badge_bg;

	public override void Create()
	{
		LoadDetails(Language.GetString("ui_btn_nbpz"), VariableBook.GetGameRequirement(6));
		DoUpdate();
		GameData.instance.PROJECT.character.BOOSTER_CHANGED.AddListener(OnBoosterChanged);
	}

	private void OnBoosterChanged(object e)
	{
		DoUpdate();
	}

	public void UpdateBadges()
	{
		int num = 0;
		foreach (BoosterRef activeBooster in GameData.instance.PROJECT.character.activeBoosters)
		{
			num += activeBooster.items.Count;
		}
		badge.text = num.ToString();
	}

	public void DoShow(bool show)
	{
		if (show)
		{
			EnableButton();
		}
		else
		{
			DisableButton();
		}
		GameData.instance.PROJECT.instance.instanceInterface.RepositionButtons();
	}

	public override void DoUpdate()
	{
		base.DoUpdate();
		DoShow(base.available && GameData.instance.PROJECT.character.activeBoosters.Count > 0);
		UpdateBadges();
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.windowGenerator.ShowBoosters();
	}

	private new void OnDestroy()
	{
		GameData.instance.PROJECT.character.BOOSTER_CHANGED.RemoveListener(OnBoosterChanged);
	}
}
