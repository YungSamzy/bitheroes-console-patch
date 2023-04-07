using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceGuildTile : MainUIButton
{
	public override void Create()
	{
		LoadDetails(Language.GetString("ui_guild"), VariableBook.GetGameRequirement(8));
		EnableButton();
		DoUpdate();
	}

	public override void DoUpdate()
	{
		hasRequirementException = GameData.instance.PROJECT.character.guildData != null;
		base.DoUpdate();
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.PROJECT.ShowGuildWindow();
	}
}
