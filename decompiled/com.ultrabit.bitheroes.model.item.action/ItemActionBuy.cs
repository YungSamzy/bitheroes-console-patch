using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.transaction;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionBuy : ItemActionBase
{
	private TransactionManager transactionManager;

	public ItemActionBuy(BaseModelData itemData)
		: base(itemData, 4)
	{
	}

	public override void Execute()
	{
		base.Execute();
		int qty = ((itemData == null || itemData.qty <= 0) ? 1 : itemData.qty);
		int currencyID = -1;
		bool flag = true;
		if (itemData is FishingShopItemRefModelData)
		{
			ExecuteFishingPurchase(itemData as FishingShopItemRefModelData);
			return;
		}
		if (itemData is EventSalesShopItemRefModelData)
		{
			ExecuteEventSalesShopPurchase(itemData as EventSalesShopItemRefModelData);
			return;
		}
		if (itemData is GuildShopRefModelData)
		{
			currencyID = 7;
			flag = false;
		}
		else if (itemData.itemRef.costCreditsRaw > 0)
		{
			currencyID = 2;
		}
		else if (itemData.itemRef.costGoldRaw > 0)
		{
			currencyID = 1;
		}
		if (flag && !itemData.itemRef.allowPurchase())
		{
			GameData.instance.windowGenerator.ShowErrorCode(116);
			return;
		}
		PaymentData paymentData = itemData.itemRef.getPaymentData();
		if (paymentData != null)
		{
			TransactionManager.instance.DoPaymentPurchase(paymentData);
		}
		else if (GameData.instance.PROJECT.character.tutorial.GetState(29) && !GameData.instance.PROJECT.character.tutorial.GetState(31))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(31);
			TransactionManager.instance.DoItemPurchase(itemData.itemRef, currencyID, itemData.itemRef.getCost(currencyID, qty), qty);
		}
		else
		{
			TransactionManager.instance.ConfirmItemPurchase(itemData.itemRef, null, qty, null, null, null, itemData.data);
		}
	}

	private void ExecuteEventSalesShopPurchase(EventSalesShopItemRefModelData eventSalesShopItemRefModelData)
	{
		string text = null;
		text = ((itemData.qty <= 1) ? Language.GetString("purchase_confirm", new string[3]
		{
			base.itemRef.coloredName,
			Util.NumberFormat(eventSalesShopItemRefModelData.eventSalesShopItemRef.cost),
			eventSalesShopItemRefModelData.eventRef.GetMaterialRef().name
		}) : Language.GetString("purchase_quantity_confirm", new string[4]
		{
			Language.GetString("ui_number_multiplier", new string[1] { Util.NumberFormat(itemData.qty) }),
			eventSalesShopItemRefModelData.eventSalesShopItemRef.itemData.itemRef.coloredName,
			Util.NumberFormat(eventSalesShopItemRefModelData.eventSalesShopItemRef.cost * itemData.qty),
			eventSalesShopItemRefModelData.eventRef.GetMaterialRef().name
		}));
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), text, null, null, delegate
		{
			TransactionManager.instance.DoEventSalesItemPurchase(eventSalesShopItemRefModelData.eventRef, eventSalesShopItemRefModelData.eventSalesShopItemRef, itemData.qty, eventSalesShopItemRefModelData.eventSalesShopItemRef.purchaseRemainingQty);
		});
	}

	private void ExecuteFishingPurchase(FishingShopItemRefModelData itemData)
	{
		string text = null;
		text = ((itemData.qty <= 1) ? Language.GetString("purchase_confirm", new string[3]
		{
			base.itemRef.coloredName,
			Util.NumberFormat(itemData.fishingShopItemRef.cost),
			VariableBook.fishingShopItem.name
		}) : Language.GetString("purchase_quantity_confirm", new string[4]
		{
			Language.GetString("ui_number_multiplier", new string[1] { Util.NumberFormat(itemData.qty) }),
			itemData.fishingShopItemRef.itemData.itemRef.coloredName,
			Util.NumberFormat(itemData.fishingShopItemRef.cost * itemData.qty),
			VariableBook.fishingShopItem.name
		}));
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), text, null, null, delegate
		{
			TransactionManager.instance.DoFishingItemPurchase(itemData.fishingShopItemRef, itemData.qty);
		});
	}
}
