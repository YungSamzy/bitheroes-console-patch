using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.lists.familiarboardlist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;

namespace com.ultrabit.bitheroes.ui.familiar;

public class FamiliarStableBoardWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public TextMeshProUGUI blankTxt;

	private List<ItemData> currentItems;

	public FamiliarBoardList itemList;

	private FamiliarBoardItemModel _familiarBoard;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_board");
		descTxt.text = Language.GetString("ui_board_familiar_stable_select", new string[1] { ItemRef.GetItemName(6) });
		itemList.StartList(OnSelectedFamiliar);
		CreateTiles();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnSelectedFamiliar(BaseModelData item)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		FamiliarRef familiarRef = item.itemRef as FamiliarRef;
		if (GameData.instance.PROJECT.character.familiarStable.getFamiliarQty(familiarRef) >= VariableBook.familiarStableMaxQty)
		{
			GameData.instance.windowGenerator.ShowErrorCode(111);
		}
		else
		{
			DoFamiliarBoardConfirm(familiarRef);
		}
	}

	private void CreateTiles()
	{
		GetItems();
		double virtualAbstractNormalizedScrollPosition = itemList.GetVirtualAbstractNormalizedScrollPosition();
		itemList.ClearList();
		List<FamiliarBoardItemModel> list = new List<FamiliarBoardItemModel>();
		foreach (ItemData currentItem in currentItems)
		{
			list.Add(new FamiliarBoardItemModel
			{
				itemData = currentItem
			});
		}
		itemList.Data.InsertItems(0, list);
		itemList.SetVirtualAbstractNormalizedScrollPosition(virtualAbstractNormalizedScrollPosition, computeVisibilityNow: true, out var _);
	}

	private void GetItems()
	{
		currentItems = new List<ItemData>();
		foreach (ItemData item in (from item in GameData.instance.PROJECT.character.inventory.GetItemsByType(6)
			orderby item.rarity descending, item.id
			select item).ToList())
		{
			if (item != null && item.qty > 0)
			{
				currentItems.Add(item);
			}
		}
	}

	private void DoFamiliarBoardConfirm(FamiliarRef familiarRef)
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_board_familiar_stable_confirm", new string[2]
		{
			familiarRef.coloredName,
			Language.GetString("ui_stable")
		}), Language.GetString("ui_accept"), Language.GetString("ui_cancel"), delegate
		{
			DoFamiliarBoard(familiarRef);
		});
	}

	private void DoFamiliarBoard(FamiliarRef familiarRef)
	{
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(28), OnFamiliarBoard);
		CharacterDALC.instance.doFamiliarStableBoard(familiarRef);
	}

	private void OnFamiliarBoard(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(28), OnFamiliarBoard);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		FamiliarStable familiarStable = FamiliarStable.fromSFSObject(sfsob);
		List<ItemData> items = ItemData.listFromSFSObject(sfsob);
		GameData.instance.PROJECT.character.familiarStable = familiarStable;
		GameData.instance.PROJECT.character.removeItems(items);
		CreateTiles();
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
