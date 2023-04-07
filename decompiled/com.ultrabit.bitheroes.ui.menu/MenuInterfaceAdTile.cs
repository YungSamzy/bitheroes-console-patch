using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceAdTile : MainUIButton
{
	public override void Create()
	{
		LoadDetails(Language.GetString("ad_instance_name"), VariableBook.GetGameRequirement(18));
		DoHide();
	}

	public void DoHide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void DoShow()
	{
		base.gameObject.SetActive(value: true);
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.windowGenerator.NewAdWindow();
	}
}
