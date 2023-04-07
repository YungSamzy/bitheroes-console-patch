using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceQuestTile : MainUIButton
{
	public GameObject notifyIcon;

	public override void Create()
	{
		LoadDetails(Language.GetString("ui_quest"), VariableBook.GetGameRequirement(0));
		notifyIcon.gameObject.SetActive(value: false);
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.PROJECT.ShowZoneWindow();
	}

	public void SetNotify(bool active)
	{
		notifyIcon.gameObject.SetActive(active);
	}
}
