using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.variable;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfacePvPTile : MainUIButton
{
	public override void Create()
	{
		LoadDetails(EventRef.getEventTypeNameShort(1), VariableBook.GetGameRequirement(1));
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.PROJECT.ShowPvPWindow();
	}
}
