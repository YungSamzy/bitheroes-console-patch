using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceFishingTile : MainUIButton
{
	public override void Create()
	{
		LoadDetails(Language.GetString("ui_fishing"), VariableBook.GetGameRequirement(25));
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.PROJECT.ShowFishingWindow();
	}
}
