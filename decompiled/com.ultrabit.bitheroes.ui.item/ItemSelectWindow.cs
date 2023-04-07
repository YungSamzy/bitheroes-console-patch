using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.transaction;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.lists.itemselectlist;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemSelectWindow : WindowsMain
{
	[HideInInspector]
	public UnityCustomEvent SELECT = new UnityCustomEvent();

	[HideInInspector]
	public UnityCustomEvent CLOSE = new UnityCustomEvent();

	public TextMeshProUGUI topperTxt;

	public ItemSelectList itemSelectList;

	private List<ItemRef> _items;

	private string _context;

	private object _data;

	private ItemRef _selectedItem;

	public object data => _data;

	public ItemRef selectedItem => _selectedItem;

	public override void Start()
	{
		base.Start();
		GameData.instance.PROJECT.PauseDungeon();
		Disable();
	}

	public void LoadDetails(List<ItemRef> items, string context = null, object data = null)
	{
		_items = items;
		_context = context;
		_data = data;
		topperTxt.text = Language.GetString("ui_select_item");
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		itemSelectList.InitList(OnTileClick);
		CreateTiles();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void CreateTiles()
	{
		itemSelectList.ClearList();
		for (int i = 0; i < _items.Count; i++)
		{
			itemSelectList.Data.InsertOneAtEnd(new ItemSelectItem
			{
				itemRef = _items[i]
			});
		}
	}

	private void OnInventoryChange()
	{
		CreateTiles();
	}

	private void OnTileClick(ItemRef itemRef)
	{
		if (GameData.instance.PROJECT.character.getItemQty(itemRef) <= 0)
		{
			TransactionManager.instance.ConfirmItemPurchase(itemRef, _context);
		}
		else if (itemRef.itemType == 4)
		{
			string text = null;
			if (_data != null)
			{
				if (_data is DungeonEntity)
				{
					text = (_data as DungeonEntity).name;
				}
				if (_data is BattleEntity)
				{
					text = (_data as BattleEntity).name;
				}
			}
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString((text != null) ? "ui_use_item_target_confirm" : "ui_use_item_confirm", new string[2] { itemRef.coloredName, text }), null, null, delegate
			{
				SelectItem(itemRef);
			});
		}
		else
		{
			SelectItem(itemRef);
		}
	}

	private void SelectItem(ItemRef itemRef)
	{
		_selectedItem = itemRef;
		OnClose();
		SELECT.Invoke(this);
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public override void DoDestroy()
	{
		CLOSE.Invoke(this);
		GameData.instance.PROJECT.ResumeDungeon();
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
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
