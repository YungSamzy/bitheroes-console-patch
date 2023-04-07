using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.service;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.transaction;
using com.ultrabit.bitheroes.ui.item;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.dungeon;

public class DungeonMerchantWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Image goldIcon;

	public Image creditsIcon;

	public TextMeshProUGUI costTxt;

	public TextMeshProUGUI costNameTxt;

	public Image costBG;

	public TextMeshProUGUI dialogTxt;

	public ItemIcon itemIcon;

	public Image placeholderAsset;

	public Button purchaseBtn;

	public Button declineBtn;

	private Dungeon _dungeon;

	private DungeonPlayer _player;

	private DungeonObject _object;

	private ItemData _itemData;

	private ServiceRef _serviceRef;

	public void LoadDetails(Dungeon dungeon, DungeonPlayer player, DungeonObject theObject)
	{
		_dungeon = dungeon;
		_player = player;
		_object = theObject;
		_itemData = _object.GetFirstItem();
		_serviceRef = _itemData.itemRef.rarityRef.merchantServiceRef;
		topperTxt.text = Language.GetString("dungeon_merchant_name");
		costNameTxt.text = Language.GetString("ui_cost") + ":";
		dialogTxt.text = Language.GetString("dungeon_merchant_desc", new string[1] { _itemData.itemRef.coloredName });
		itemIcon.SetItemData(_itemData);
		itemIcon.SetupItemComparision(showCosmetic: false, showComparision: true);
		itemIcon.SetItemActionType(0);
		purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_buy");
		declineBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_decline");
		GameData.instance.PROJECT.character.AddListener("SHOP_ROTATION_ID_CHANGE", OnShopRotationChange);
		placeholderAsset.overrideSprite = _object.objectRef.GetSpriteIcon();
		UpdateCost();
		GameData.instance.windowGenerator.ShowCurrencies(show: true);
		ListenForBack(DoDecline);
		ListenForForward(DoPurchase);
		CreateWindow();
	}

	public void OnAssetLoaded()
	{
	}

	public void OnPurchaseBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoPurchase();
	}

	public void OnDeclineBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoDecline();
	}

	private void DoPurchase()
	{
		string coloredName = _itemData.itemRef.coloredName;
		TransactionManager.PurchaseObject purchaseObject2 = new TransactionManager.PurchaseObject();
		purchaseObject2.name = coloredName;
		purchaseObject2.serviceRef = _serviceRef;
		int currencyID = ((_serviceRef.costCreditsRaw <= 0) ? 1 : 2);
		CurrencyRef currencyRef = CurrencyBook.Lookup(currencyID);
		int currencyCost = _serviceRef.getCost(currencyID);
		string @string = Language.GetString("purchase_confirm", new string[3]
		{
			coloredName,
			Util.NumberFormat(currencyCost),
			currencyRef.name
		});
		TransactionManager.instance.ConfirmItemPurchase(_serviceRef, "Dungeon", 1, @string, delegate
		{
			OnPurchaseConfirm(currencyID, currencyCost);
		}, purchaseObject2);
	}

	private void OnPurchaseConfirm(int currencyID, int currencyCost)
	{
		GameData.instance.main.ShowLoading();
		GameData.instance.audioManager.PlaySoundLink("purchase");
		_dungeon.ActivateObject(_player, _object, wait: false, currencyID, currencyCost);
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

	private void OnShopRotationChange()
	{
		UpdateCost();
	}

	private void UpdateCost()
	{
		ShopSaleRef itemSaleRef = ShopBook.GetItemSaleRef(_serviceRef, GameData.instance.PROJECT.character.shopRotationID);
		int num = ((_serviceRef.costCreditsRaw <= 0) ? 1 : 2);
		int num2 = ((num == 2) ? _serviceRef.costCredits : _serviceRef.costGold);
		goldIcon.gameObject.SetActive(num == 1);
		creditsIcon.gameObject.SetActive(num == 2);
		string text = Util.NumberFormat(num2);
		if (itemSaleRef != null)
		{
			text = Util.ParseString("^" + text + "^");
		}
		costTxt.text = text;
	}

	public override void DoDestroy()
	{
		if (!GameData.instance.windowGenerator.HasDialogByClass(typeof(ItemListWindow)))
		{
			_dungeon.CheckAutoPilot();
		}
		GameData.instance.PROJECT.character.RemoveListener("SHOP_ROTATION_ID_CHANGE", OnShopRotationChange);
		GameData.instance.windowGenerator.ShowCurrencies(show: false);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		purchaseBtn.interactable = true;
		declineBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		purchaseBtn.interactable = false;
		declineBtn.interactable = false;
	}
}
