using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.login;
using com.ultrabit.bitheroes.model.variable;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceKongregateTile : MainUIButton
{
	public override void Create()
	{
		LoadDetails(Language.GetString("ui_kongregate"), VariableBook.GetGameRequirement(24));
	}

	public override void DoClick()
	{
		base.DoClick();
		KongregateLogin.Link();
	}
}
