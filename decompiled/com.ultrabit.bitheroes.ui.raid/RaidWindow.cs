using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.raid;
using com.ultrabit.bitheroes.ui.lists.raidlist;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.raid;

public class RaidWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Image shardsBtn;

	public RaidList raidList;

	public Transform ContentNodes;

	public Transform NodePrefab;

	public CurrencyBarFill currencyBarFill;

	public SpriteMask[] masks;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_raid");
		CreatePanel();
		currencyBarFill.Init();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		if (raidList.Data != null)
		{
			raidList.Refresh();
		}
		SpriteMask[] array = masks;
		foreach (SpriteMask obj in array)
		{
			obj.frontSortingLayerID = SortingLayer.NameToID("UI");
			obj.frontSortingOrder = base.sortingLayer + 99;
			obj.backSortingLayerID = SortingLayer.NameToID("UI");
			obj.backSortingOrder = base.sortingLayer - 1;
		}
	}

	private void CreatePanel()
	{
		raidList.InitList(this);
		if (raidList.Data.Count > 0)
		{
			raidList.Data.RemoveItems(0, raidList.Data.Count);
		}
		List<RaidListModel> list = new List<RaidListModel>();
		int index = 0;
		for (int i = 0; i < RaidBook.size; i++)
		{
			RaidRef raidRef = RaidBook.LookUp(i + 1);
			if (raidRef != null && raidRef.getUnlocked())
			{
				if (raidRef.id == GameData.instance.SAVE_STATE.GetRaidSelected())
				{
					index = i;
				}
				list.Add(new RaidListModel
				{
					id = raidRef.id,
					title = raidRef.coloredName,
					description = Language.GetString(raidRef.desc),
					raidRef = raidRef
				});
			}
		}
		raidList.AddItemsAt(0, list);
		raidList.ScrollToPage(index, directo: false, noAnim: true);
	}

	public void OnShardsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowServiceType(6);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (shardsBtn != null && shardsBtn.gameObject != null && shardsBtn.GetComponent<EventTrigger>() != null)
		{
			shardsBtn.GetComponent<EventTrigger>().enabled = true;
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		if (shardsBtn != null && shardsBtn.gameObject != null && shardsBtn.GetComponent<EventTrigger>() != null)
		{
			shardsBtn.GetComponent<EventTrigger>().enabled = false;
		}
	}
}
