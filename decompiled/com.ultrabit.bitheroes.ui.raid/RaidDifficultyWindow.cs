using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.raid;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.raid;

public class RaidDifficultyWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Image shardsBtn;

	public Button consumablesBtn;

	public CurrencyBarFill currencyBarFill;

	public RaidDifficultyTile[] tiles;

	public int raidID;

	public override void Start()
	{
		base.Start();
		Disable();
		currencyBarFill.Init();
		InitRaidInformation();
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		UpdateButtons();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnInventoryChange()
	{
		UpdateButtons();
	}

	private void InitRaidInformation()
	{
		RaidRef raidRef = RaidBook.LookUp(raidID);
		topperTxt.text = Language.GetString(raidRef.name);
		List<RaidDifficultyRef> difficulties = raidRef.difficulties;
		for (int i = 0; i < difficulties.Count; i++)
		{
			if (tiles.Length > i)
			{
				tiles[i].SetDifficultyRef(raidRef, difficulties[i], this);
			}
		}
	}

	private void UpdateButtons()
	{
		List<ItemData> consumablesByCurrencyID = GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(8);
		consumablesBtn.gameObject.SetActive(consumablesByCurrencyID.Count > 0);
	}

	public void OnConsumablesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowItems(GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(8), compare: false, added: true);
	}

	public void OnShardsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowServiceType(6);
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
		if (shardsBtn != null && shardsBtn.GetComponent<EventTrigger>() != null)
		{
			shardsBtn.GetComponent<EventTrigger>().enabled = true;
		}
		if (consumablesBtn != null)
		{
			consumablesBtn.interactable = true;
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		shardsBtn.GetComponent<EventTrigger>().enabled = false;
		consumablesBtn.interactable = false;
	}
}
