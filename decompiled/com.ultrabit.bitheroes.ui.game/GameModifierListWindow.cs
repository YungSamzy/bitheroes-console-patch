using System.Collections.Generic;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.lists.gamemodifierlist;
using TMPro;

namespace com.ultrabit.bitheroes.ui.game;

public class GameModifierListWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	private GameModifierList gameModifierList;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_bonuses");
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void LoadDetails(List<GameModifier> modifiers)
	{
		gameModifierList = GetComponentInChildren<GameModifierList>();
		gameModifierList.InitList();
		for (int i = 0; i < modifiers.Count; i++)
		{
			gameModifierList.Data.InsertOneAtEnd(new GameModifierItem
			{
				description = modifiers[i].GetTileDesc()
			});
		}
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}
}
