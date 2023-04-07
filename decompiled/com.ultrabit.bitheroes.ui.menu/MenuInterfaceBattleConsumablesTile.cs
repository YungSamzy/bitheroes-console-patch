using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.battle;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceBattleConsumablesTile : MainUIButton
{
	private Battle _battle;

	public void Create(Battle battle)
	{
		_battle = battle;
		LoadDetails(Language.GetString("ui_battle_consumables"), VariableBook.GetGameRequirement(16));
	}

	public override void DoClick()
	{
		base.DoClick();
		_battle.DoConsumablesSelect();
	}
}
