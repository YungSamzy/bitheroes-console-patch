using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.bait;
using com.ultrabit.bitheroes.model.boober;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.fish;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.itemsearchlist;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemSearchWindow : WindowsMain
{
	private const int DEFAULT_SUBTYPE = -1;

	public TextMeshProUGUI topperTxt;

	public TMP_InputField searchTxt;

	public Image itemsDropdown;

	public ItemSearchList itemSearchList;

	private List<ItemData> _items;

	private bool _showQty;

	private bool _adminWindow;

	private bool _closeOnSelect;

	private List<ItemRef> _selectedItems;

	private bool _showLock;

	private bool _tooltipSuggested;

	private bool _waitToDestroy;

	private bool _changed;

	private ItemRef _selectedItem;

	private Vector2 selectedType;

	private int currentType;

	private Transform dropdownWindow;

	private List<ItemSearchItem> _currentItems = new List<ItemSearchItem>();

	[HideInInspector]
	public UnityCustomEvent SELECT = new UnityCustomEvent();

	public bool changed => _changed;

	public ItemRef selectedItem => _selectedItem;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(List<ItemData> items, bool adminWindow, string name = null, bool showQty = true, bool closeOnSelect = true, List<ItemRef> selectedItems = null, bool showLock = false, bool tooltipSuggested = false)
	{
		if (name == null)
		{
			name = Language.GetString("ui_items");
		}
		_items = items;
		_showQty = showQty;
		_adminWindow = adminWindow;
		_closeOnSelect = closeOnSelect;
		_selectedItems = selectedItems;
		_showLock = showLock;
		_tooltipSuggested = tooltipSuggested;
		topperTxt.text = name;
		searchTxt.text = "";
		itemSearchList.StartList(OnItemSelect);
		selectedType = new Vector2(0f, -1f);
		itemsDropdown.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_all_items");
		ListenForBack(OnClose);
		CreateTiles(items);
		forceAnimation = true;
		CreateWindow();
	}

	private IEnumerator SyncNames()
	{
		for (int i = _items.Count - 1; i >= 0; i--)
		{
			_ = _items[i].itemRef.name;
			if (i % 100 == 0)
			{
				yield return null;
			}
		}
	}

	private void CreateTiles(List<ItemData> items)
	{
		List<ItemRef> list = new List<ItemRef>();
		List<ItemRef> list2 = new List<ItemRef>();
		if (_currentItems != null && _currentItems.Count > 0)
		{
			foreach (ItemSearchItem currentItem in _currentItems)
			{
				if (currentItem.selected)
				{
					list.Add(currentItem.itemData.itemRef);
				}
				if (currentItem.locked)
				{
					list2.Add(currentItem.itemData.itemRef);
				}
			}
		}
		itemSearchList.ClearList();
		_currentItems.Clear();
		List<ItemSearchItem> list3 = new List<ItemSearchItem>();
		int num = Mathf.RoundToInt(selectedType.x);
		int sub = Mathf.RoundToInt(selectedType.y);
		for (int num2 = items.Count - 1; num2 >= 0; num2--)
		{
			ItemData itemData = items[num2];
			ItemRef itemRef = itemData.itemRef;
			bool locked = false;
			bool selected = false;
			if (_selectedItems != null && ItemRef.listHasItem(_selectedItems, itemData.itemRef))
			{
				if (_showLock)
				{
					locked = true;
				}
				else
				{
					selected = true;
				}
			}
			if (list.Count > 0 && ItemRef.listHasItem(list, itemData.itemRef))
			{
				selected = true;
			}
			if (list2.Count > 0 && ItemRef.listHasItem(list2, itemData.itemRef))
			{
				locked = true;
			}
			_currentItems.Add(new ItemSearchItem
			{
				itemData = itemData,
				locked = locked,
				selected = selected,
				adminWindow = _adminWindow
			});
			bool flag = true;
			if (flag && num != 0 && (num != itemRef.itemType || !itemRef.MatchesSubtype(sub)))
			{
				flag = false;
			}
			if (flag)
			{
				list3.Add(new ItemSearchItem
				{
					itemData = itemData,
					locked = locked,
					selected = selected,
					adminWindow = _adminWindow
				});
			}
		}
		List<ItemSearchItem> list4 = new List<ItemSearchItem>();
		string text = searchTxt.text;
		if (text.Length > 0)
		{
			foreach (ItemSearchItem item in list3)
			{
				ItemRef itemRef2 = item.itemData.itemRef;
				string @string = itemRef2.name;
				if (itemRef2.itemType == 11)
				{
					@string = Language.GetString(itemRef2.name);
				}
				if (@string.ToLowerInvariant().IndexOf(text.ToLowerInvariant()) >= 0)
				{
					list4.Add(item);
				}
			}
		}
		else
		{
			list4 = list3;
		}
		itemSearchList.Data.InsertItemsAtStart(list4);
	}

	private void OnItemSelect(BaseModelData item, ItemSearchItem model)
	{
		_changed = true;
		if (_closeOnSelect)
		{
			_selectedItem = (item as ItemData).itemRef;
			SELECT.Invoke(this);
			return;
		}
		if (_showLock)
		{
			if (!model.locked)
			{
				model.locked = true;
			}
			else
			{
				model.locked = false;
			}
		}
		else if (!model.selected)
		{
			model.selected = true;
		}
		else
		{
			model.selected = false;
		}
		foreach (ItemSearchItem currentItem in _currentItems)
		{
			if (!(currentItem.itemData.itemRef == model.itemData.itemRef))
			{
				continue;
			}
			if (_showLock)
			{
				if (!currentItem.locked)
				{
					currentItem.locked = true;
				}
				else
				{
					currentItem.locked = false;
				}
			}
			else if (!currentItem.selected)
			{
				currentItem.selected = true;
			}
			else
			{
				currentItem.selected = false;
			}
			break;
		}
		itemSearchList.Refresh();
	}

	private bool HasItemType(int itemType)
	{
		foreach (ItemData item in _items)
		{
			if (item.itemRef.itemType == itemType)
			{
				return true;
			}
		}
		return false;
	}

	public void OnItemsDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		dropdownWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_items"));
		DropdownList componentInChildren = dropdownWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, currentType, OnItemsDropdownChange);
		componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
		{
			id = 0,
			title = Language.GetString("ui_all_items"),
			btnHelp = false,
			data = new Vector2(0f, -1f)
		});
		for (int i = 1; i < 22; i++)
		{
			if (HasItemType(i))
			{
				bool flag = true;
				switch (i)
				{
				case 1:
					componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
					{
						id = 10001,
						title = ItemRef.GetItemNamePlural(1),
						btnHelp = false,
						data = new Vector2(1f, -1f)
					});
					componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
					{
						id = 10002,
						title = ItemRef.GetItemNamePlural(1, 1),
						btnHelp = false,
						data = new Vector2(1f, 1f)
					});
					componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
					{
						id = 10003,
						title = ItemRef.GetItemNamePlural(1, 2),
						btnHelp = false,
						data = new Vector2(1f, 2f)
					});
					componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
					{
						id = 10004,
						title = ItemRef.GetItemNamePlural(1, 3),
						btnHelp = false,
						data = new Vector2(1f, 3f)
					});
					componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
					{
						id = 10005,
						title = ItemRef.GetItemNamePlural(1, 4),
						btnHelp = false,
						data = new Vector2(1f, 4f)
					});
					componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
					{
						id = 10006,
						title = ItemRef.GetItemNamePlural(1, 5),
						btnHelp = false,
						data = new Vector2(1f, 5f)
					});
					componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
					{
						id = 10007,
						title = ItemRef.GetItemNamePlural(1, 6),
						btnHelp = false,
						data = new Vector2(1f, 6f)
					});
					componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
					{
						id = 10008,
						title = ItemRef.GetItemNamePlural(1, 7),
						btnHelp = false,
						data = new Vector2(1f, 7f)
					});
					componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
					{
						id = 10009,
						title = ItemRef.GetItemNamePlural(1, 8),
						btnHelp = false,
						data = new Vector2(1f, 8f)
					});
					flag = false;
					break;
				case 5:
					flag = false;
					break;
				case 12:
					flag = FishBook.size > 0;
					break;
				case 13:
					flag = BaitBook.size > 0;
					break;
				case 14:
					flag = BobberBook.size > 0;
					break;
				default:
					flag = true;
					break;
				}
				if (flag)
				{
					componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
					{
						id = i,
						title = ItemRef.GetItemNamePlural(i),
						btnHelp = false,
						data = new Vector2(i, -1f)
					});
				}
			}
		}
	}

	private void OnItemsDropdownChange(MyDropdownItemModel model)
	{
		if (changed)
		{
			_selectedItems = GetSelectedItems();
		}
		currentType = model.id;
		selectedType = (model.data as Vector2?).Value;
		itemsDropdown.GetComponentInChildren<TextMeshProUGUI>().text = model.title;
		if (dropdownWindow != null)
		{
			dropdownWindow.GetComponent<DropdownWindow>().OnClose();
		}
		CreateTiles(_items);
	}

	public void OnSearchTextValueChanged()
	{
		CancelInvoke("DoSearch");
		Invoke("DoSearch", Util.SEARCHBOX_ACTION_DELAY);
	}

	private void DoSearch()
	{
		CreateTiles(_items);
	}

	public List<ItemRef> GetSelectedItems()
	{
		List<ItemRef> list = new List<ItemRef>();
		foreach (ItemSearchItem currentItem in _currentItems)
		{
			if (currentItem.locked && _showLock)
			{
				list.Add(currentItem.itemData.itemRef);
			}
			else if (currentItem.selected && !_showLock)
			{
				list.Add(currentItem.itemData.itemRef);
			}
		}
		return list;
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		searchTxt.interactable = true;
		itemsDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		searchTxt.interactable = false;
		itemsDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
