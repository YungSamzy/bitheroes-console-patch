using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.victory;

namespace com.ultrabit.bitheroes.ui.battle;

public class DefeatableVictoryWindow : VictoryWindow
{
	public override void Start()
	{
		base.Start();
	}

	public void LoadDetails(int type, bool isVictorious, long exp = 0L, int gold = 0, List<ItemData> items = null, bool isRerunnable = false)
	{
		string text = (isVictorious ? Language.GetString("battle_victory") : Language.GetString("battle_defeat"));
		_shouldCheckTutorials = false;
		base.LoadDetails(type, exp, gold, items, null, isVictorious, text, isVictorious: isVictorious, customCloseText: Language.GetString("ui_town"), customLootText: (GameData.instance.PROJECT.dungeon != null) ? Language.GetString("ui_loot_recap") : null, isCloseRed: true, isRerunnable: isRerunnable);
	}
}
