using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceSettingsTile : MainUIButton
{
	public override void Create()
	{
		LoadDetails(Language.GetString("ui_settings"), VariableBook.GetGameRequirement(9));
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.windowGenerator.NewGameSettingsWindow();
	}
}
