using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceBrawlTile : MainUIButton
{
	public override void Create()
	{
		LoadDetails(Language.GetString("ui_brawl_short"), VariableBook.GetGameRequirement(34));
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.PROJECT.ShowBrawlWindow();
	}
}
