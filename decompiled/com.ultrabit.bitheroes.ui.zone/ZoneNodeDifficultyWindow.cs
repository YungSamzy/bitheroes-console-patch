using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.zone;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.zone;

public class ZoneNodeDifficultyWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button consumablesBtn;

	public ZoneNodeDifficultyTile[] tiles;

	private ZoneNodeRef _nodeRef;

	private List<ItemData> consumables;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(ZoneNodeRef nodeRef)
	{
		_nodeRef = nodeRef;
		topperTxt.text = Util.ParseString(nodeRef.name);
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		UpdateButtons();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnInventoryChange()
	{
		UpdateButtons();
	}

	public void OnConsumablesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowItems(GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(4), compare: false, added: true);
	}

	private void UpdateButtons()
	{
		consumables = GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(4);
		consumablesBtn.gameObject.SetActive(consumables.Count > 0);
		int num = 0;
		foreach (ZoneNodeDifficultyRef difficulty in _nodeRef.difficulties)
		{
			if (num < tiles.Length)
			{
				tiles[num].LoadDetails(difficulty, this);
			}
			num++;
		}
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (consumablesBtn != null)
		{
			consumablesBtn.interactable = true;
		}
		for (int i = 0; i < tiles.Length; i++)
		{
			if (tiles[i] != null)
			{
				tiles[i].DoEnable();
			}
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		consumablesBtn.interactable = false;
		for (int i = 0; i < tiles.Length; i++)
		{
			tiles[i].DoDisable();
		}
	}
}
