using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.lists.itemcraftreforgelist;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemReforgeWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	private ItemRef _itemRef;

	private List<ItemData> _tiles;

	public ItemCraftReforgeList itemList;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(ItemRef itemRef)
	{
		_itemRef = itemRef;
		topperTxt.text = Language.GetString("ui_reforge");
		descTxt.text = Language.GetString("ui_select_item_reforge", new string[1] { itemRef.coloredName });
		itemList.StartList();
		itemList.REFORGE.AddListener(OnItemReforge);
		CreateTiles();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	public void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null))
		{
			MyListItemViewsHolder itemViewsHolderIfVisible = itemList.GetItemViewsHolderIfVisible(0);
			if (GameData.instance.PROJECT.character.tutorial.GetState(124) && !GameData.instance.PROJECT.character.tutorial.GetState(126))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(126);
				GameData.instance.tutorialManager.ShowTutorialForButton(itemViewsHolderIfVisible.craftBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(126), 3, itemViewsHolderIfVisible.craftBtn.gameObject), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
		}
	}

	private void CreateTiles()
	{
		itemList.ClearList();
		List<ReforgeItemModel> list = new List<ReforgeItemModel>();
		List<ItemRef> reforgeableItems = _itemRef.getReforgeableItems();
		for (int i = 0; i < reforgeableItems.Count; i++)
		{
			ItemRef itemRef = reforgeableItems[i];
			if (!(itemRef == null))
			{
				CraftReforgeRef itemReforgeRef = CraftBook.getItemReforgeRef(_itemRef, itemRef);
				if (!itemRef.statName.ToLowerInvariant().Equals("evolvium") || itemReforgeRef.link != null)
				{
					list.Add(new ReforgeItemModel
					{
						itemRef = _itemRef,
						resultRef = itemRef,
						reforgeRef = itemReforgeRef
					});
				}
			}
		}
		itemList.Data.InsertItems(0, list);
	}

	private void OnItemReforge()
	{
		OnClose();
	}

	public override void DoDestroy()
	{
		itemList.REFORGE.RemoveListener(OnItemReforge);
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
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
