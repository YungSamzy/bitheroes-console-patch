using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.lists.tradelist;
using TMPro;

namespace com.ultrabit.bitheroes.ui.craft;

public class CraftTradeWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI emptyTxt;

	public TradeList tradeList;

	private List<CraftTradeRef> _tradeRefs;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(List<CraftTradeRef> tradeRefs)
	{
		_tradeRefs = tradeRefs;
		topperTxt.text = Language.GetString("ui_craft");
		tradeList.InitList();
		CreateTiles();
		GameData.instance.PROJECT.character.equipment.OnChange.AddListener(OnChange);
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnChange);
		GameData.instance.PROJECT.character.AddListener("LOCKED_ITEMS_CHANGE", OnChange);
		emptyTxt.SetText(Language.GetString("ui_no_trade_recipes"));
		DoUpdate();
		ListenForBack(OnClose);
		forceAnimation = true;
		CreateWindow();
	}

	private void OnChange()
	{
		DoUpdate();
	}

	private void CreateTiles()
	{
		double virtualAbstractNormalizedScrollPosition = tradeList.GetVirtualAbstractNormalizedScrollPosition();
		tradeList.ClearList();
		for (int i = 0; i < _tradeRefs.Count; i++)
		{
			CraftTradeRef craftTradeRef = _tradeRefs[i];
			if (craftTradeRef == null || craftTradeRef.isOld || (craftTradeRef.gameRequirement != null && !craftTradeRef.gameRequirement.RequirementsMet()))
			{
				continue;
			}
			bool flag = true;
			foreach (ItemRef craftingRequiredItem in craftTradeRef.craftingRequiredItems)
			{
				if (!GameData.instance.PROJECT.character.inventory.hasOwnedItem(craftingRequiredItem))
				{
					flag = false;
				}
			}
			if (flag)
			{
				tradeList.Data.InsertOneAtEnd(new TradeItem
				{
					tradeRef = craftTradeRef.tradeRef,
					sourceRef = craftTradeRef
				});
			}
		}
		emptyTxt.gameObject.SetActive(tradeList.Data.Count <= 0);
		tradeList.SetVirtualAbstractNormalizedScrollPosition(virtualAbstractNormalizedScrollPosition, computeVisibilityNow: true, out var _);
	}

	private void DoUpdate()
	{
		CreateTiles();
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.equipment.OnChange.RemoveListener(OnChange);
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnChange);
		GameData.instance.PROJECT.character.RemoveListener("LOCKED_ITEMS_CHANGE", OnChange);
		base.DoDestroy();
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
