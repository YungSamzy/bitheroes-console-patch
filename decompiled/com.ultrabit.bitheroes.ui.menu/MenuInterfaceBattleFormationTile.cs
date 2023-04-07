using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.battle;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceBattleFormationTile : MainUIButton
{
	private Battle _battle;

	public void Create(Battle battle)
	{
		_battle = battle;
		LoadDetails(Language.GetString("ui_battle_formation"), VariableBook.GetGameRequirement(15));
	}

	public override void DoClick()
	{
		base.DoClick();
		if (!_battle.battleRules.allowSwitch)
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_cannot_switch"));
		}
		else
		{
			GameData.instance.windowGenerator.NewBattleEntitySelectWindow().GetComponent<BattleEntitySelectWindow>().LoadDetails(_battle.GetControlledEntities(), Language.GetString("ui_battle_formation_desc"), drag: true);
		}
	}
}
