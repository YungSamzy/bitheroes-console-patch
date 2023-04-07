using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceRaidTile : MainUIButton
{
	public override void Create()
	{
		LoadDetails(Language.GetString("ui_raid"), VariableBook.GetGameRequirement(3));
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.PROJECT.ShowRaidWindow();
	}
}
