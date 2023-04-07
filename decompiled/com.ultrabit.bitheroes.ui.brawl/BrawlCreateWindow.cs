using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.lists.brawllist;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.brawl;

public class BrawlCreateWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public BrawlCreateList brawlCreateList;

	public CurrencyBarFill currencyBarFill;

	public SpriteMask[] masks;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_brawl");
		CreatePanel();
		currencyBarFill.Init();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		if (brawlCreateList.Data != null)
		{
			brawlCreateList.Refresh();
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
		brawlCreateList.InitList(this);
		if (!VariableBook.GameRequirementMet(34))
		{
			return;
		}
		if (brawlCreateList.Data.Count > 0)
		{
			brawlCreateList.Data.RemoveItems(0, brawlCreateList.Data.Count);
		}
		List<BrawlListModel> list = new List<BrawlListModel>();
		int index = 0;
		for (int i = 0; i <= BrawlBook.size; i++)
		{
			BrawlRef brawlRef = BrawlBook.Lookup(i);
			if (brawlRef != null && brawlRef.requirementsMet())
			{
				if (brawlRef.id == GameData.instance.SAVE_STATE.GetBrawlSelected())
				{
					index = list.Count;
				}
				list.Add(new BrawlListModel
				{
					brawlRef = brawlRef
				});
			}
		}
		brawlCreateList.AddItemsAt(0, list);
		brawlCreateList.Refresh();
		brawlCreateList.ScrollToPage(index);
	}

	public void OnXealsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowServiceType(15);
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
