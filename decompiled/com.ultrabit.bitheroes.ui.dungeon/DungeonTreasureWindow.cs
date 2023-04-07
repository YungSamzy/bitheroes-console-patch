using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.transaction;
using com.ultrabit.bitheroes.ui.item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.dungeon;

public class DungeonTreasureWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI qtyTxt;

	public TextMeshProUGUI dialogTxt;

	public TextMeshProUGUI keysNameTxt;

	public ItemIcon itemIcon;

	public Image placeholderAsset;

	public Button useBtn;

	public Button declineBtn;

	private Dungeon _dungeon;

	private DungeonPlayer _player;

	private DungeonObject _object;

	private ConsumableRef _consumableRef;

	public void LoadDetails(Dungeon dungeon, DungeonPlayer player, DungeonObject theObject)
	{
		_dungeon = dungeon;
		_player = player;
		_object = theObject;
		_consumableRef = ConsumableBook.GetFirstConsumableByType(1);
		topperTxt.text = Language.GetString("dungeon_treasure_name");
		keysNameTxt.text = Language.GetString("ui_keys") + ":";
		dialogTxt.text = Language.GetString("dungeon_treasure_locked_desc", new string[1] { _consumableRef.coloredName });
		itemIcon.SetItemData(new ItemData(_consumableRef, 0));
		itemIcon.SetItemActionType(0);
		useBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_open");
		declineBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_decline");
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		UpdateQty();
		placeholderAsset.overrideSprite = _object.objectRef.GetSpriteIcon();
		ListenForBack(DoDecline);
		ListenForForward(DoOpen);
		CreateWindow();
	}

	public void OnAssetLoaded()
	{
		Debug.Log("Asset Loaded");
	}

	private void OnInventoryChange()
	{
		UpdateQty();
	}

	private void UpdateQty()
	{
		int itemQty = GameData.instance.PROJECT.character.getItemQty(_consumableRef);
		qtyTxt.text = Util.colorString(Util.NumberFormat(itemQty), Util.getCurrentColor(1, itemQty, 0));
	}

	public void OnUseBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoOpen();
	}

	public void OnDeclineBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoDecline();
	}

	private void DoOpen()
	{
		if (GameData.instance.PROJECT.character.getItemQty(_consumableRef) < 1)
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_locked"), Language.GetString("dungeon_treasure_locked_purchase", new string[1] { _consumableRef.coloredName }), null, null, delegate
			{
				OnDialogKeyBuyYes();
			});
		}
		else
		{
			DoDialogUseConfirm(Language.GetString("ui_locked"), Language.GetString("dungeon_treasure_locked_confirm", new string[1] { _consumableRef.coloredName }));
		}
	}

	private void OnDialogKeyBuyYes()
	{
		TransactionManager.instance.ConfirmItemPurchase(_consumableRef, "Dungeon");
	}

	private void DoDialogUseConfirm(string name, string text)
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(name, text, null, null, delegate
		{
			OnDialogUseYes();
		});
	}

	private void OnDialogUseYes()
	{
		GameData.instance.main.ShowLoading();
		_dungeon.ActivateObject(_player, _object);
	}

	private void DoDecline()
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_decline_confirm"), null, null, delegate
		{
			OnDeclineConfirm();
		});
	}

	private void OnDeclineConfirm()
	{
		_dungeon.ActivateObject(_player, _object, wait: true, 0, 0);
		base.OnClose();
	}

	public override void DoDestroy()
	{
		if (!GameData.instance.windowGenerator.HasDialogByClass(typeof(ItemListWindow)))
		{
			_dungeon.CheckAutoPilot();
		}
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		useBtn.interactable = true;
		declineBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		useBtn.interactable = false;
		declineBtn.interactable = false;
	}
}
