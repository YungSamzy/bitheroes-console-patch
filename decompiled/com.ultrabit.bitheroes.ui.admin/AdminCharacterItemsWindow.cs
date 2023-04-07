using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.lists.adminitemslist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.admin;

public class AdminCharacterItemsWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button addBtn;

	public Button sendBtn;

	public AdminItemsList adminItemsList;

	private int _charID;

	private List<AdminItemsItem> _tiles = new List<AdminItemsItem>();

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(int charID)
	{
		_charID = charID;
		topperTxt.text = Language.GetString("ui_items");
		addBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_add");
		sendBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_send");
		adminItemsList.InitList(OnTileSelect);
		DoUpdate();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void DoUpdate()
	{
		adminItemsList.ClearList();
		adminItemsList.Data.InsertItemsAtEnd(_tiles);
		if (_tiles.Count > 0)
		{
			Util.SetButton(sendBtn);
		}
		else
		{
			Util.SetButton(sendBtn, enabled: false);
		}
	}

	public void OnAddBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoItemSelect();
	}

	public void OnSendBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoAdjustItems();
	}

	private void DoItemSelect()
	{
		List<ItemRef> allItems = ItemBook.GetAllItems();
		List<ItemData> list = new List<ItemData>();
		foreach (ItemRef item in allItems)
		{
			int itemType = item.itemType;
			if (itemType != 5 && itemType != 16)
			{
				list.Add(new ItemData(item, 1));
			}
		}
		GameData.instance.windowGenerator.NewItemSearchWindow(list, adminWindow: false, null, showQty: false, closeOnSelect: false, GetSelectedItems(), showLock: false, tooltipSuggested: false, base.gameObject).DESTROYED.AddListener(OnItemSelect);
	}

	private void OnItemSelect(object e)
	{
		List<ItemRef> selectedItems = (e as ItemSearchWindow).GetSelectedItems();
		List<ItemRef> list = new List<ItemRef>();
		foreach (AdminItemsItem tile in _tiles)
		{
			if (!ItemRef.listHasItem(selectedItems, tile.itemRef))
			{
				list.Add(tile.itemRef);
			}
		}
		foreach (ItemRef item in list)
		{
			RemoveItem(item);
		}
		foreach (ItemRef item2 in selectedItems)
		{
			AddItem(item2);
		}
	}

	private void DoAdjustItems()
	{
		List<ItemData> items = GetItems(0, int.MaxValue);
		List<ItemData> items2 = GetItems(int.MinValue, -1, flipQty: true);
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(19), OnAdjustItems);
		AdminDALC.instance.doCharacterAdjustItems(_charID, items, items2);
	}

	private void OnAdjustItems(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(19), OnAdjustItems);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		AdminCharacterWindow adminCharacterWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AdminCharacterWindow)) as AdminCharacterWindow;
		if (adminCharacterWindow != null)
		{
			adminCharacterWindow.DoRefresh();
		}
		OnClose();
	}

	private void AddItem(ItemRef itemRef)
	{
		if (GetItemTile(itemRef) == null)
		{
			AdminItemsItem item = new AdminItemsItem
			{
				itemRef = itemRef,
				qty = 1
			};
			_tiles.Add(item);
			DoUpdate();
		}
	}

	private void OnTileSelect(AdminItemsItem tile)
	{
		RemoveItem(tile.itemRef);
	}

	private void RemoveItem(ItemRef itemRef)
	{
		for (int i = 0; i < _tiles.Count; i++)
		{
			if (_tiles[i].itemRef == itemRef)
			{
				_tiles.RemoveAt(i);
				DoUpdate();
				break;
			}
		}
	}

	private AdminItemsItem GetItemTile(ItemRef itemRef)
	{
		foreach (AdminItemsItem tile in _tiles)
		{
			if (tile.itemRef == itemRef)
			{
				return tile;
			}
		}
		return null;
	}

	private List<ItemRef> GetSelectedItems()
	{
		List<ItemRef> list = new List<ItemRef>();
		foreach (AdminItemsItem tile in _tiles)
		{
			list.Add(tile.itemRef);
		}
		return list;
	}

	private List<ItemData> GetItems(int minQty, int maxQty, bool flipQty = false)
	{
		List<ItemData> list = new List<ItemData>();
		foreach (AdminItemsItem tile in _tiles)
		{
			if (tile.qty >= minQty && tile.qty <= maxQty)
			{
				int num = tile.qty;
				if (flipQty)
				{
					num *= -1;
				}
				list.Add(new ItemData(tile.itemRef, num));
			}
		}
		return list;
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		addBtn.interactable = true;
		sendBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		addBtn.interactable = false;
		sendBtn.interactable = false;
	}
}
