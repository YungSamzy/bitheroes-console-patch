using System.Collections.Generic;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.login;
using com.ultrabit.bitheroes.model.payment;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class MerchantDALC : BaseDALC
{
	public static int ITEM_PURCHASE = 1;

	public static int ITEM_SELL = 2;

	public static int ITEM_CONTENTS = 3;

	public static int ITEM_UPGRADE = 4;

	public static int ITEM_EXCHANGE = 5;

	public static int ITEM_TRADE = 6;

	public static int ITEM_FUSION = 7;

	public static int ITEM_REFORGE = 8;

	public static int PAYMENT_STEAM_START = 9;

	public static int PAYMENT_STEAM_COMPLETE = 10;

	private static MerchantDALC _instance;

	public static MerchantDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new MerchantDALC();
			}
			return _instance;
		}
	}

	public void doItemPurchase(int id, int type, int currencyID, int cost, int qty = 1, string data = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", ITEM_PURCHASE);
		sFSObject.PutInt("ite0", id);
		sFSObject.PutInt("ite1", type);
		sFSObject.PutInt("ite2", qty);
		sFSObject.PutInt("curr0", currencyID);
		sFSObject.PutInt("curr2", cost);
		if (data != null && data.Length > 0)
		{
			sFSObject.PutUtfString("ite6", data);
		}
		send(sFSObject);
	}

	public void doItemSell(int id, int type, int qty = 1)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", ITEM_SELL);
		sFSObject.PutInt("ite0", id);
		sFSObject.PutInt("ite1", type);
		sFSObject.PutInt("ite2", qty);
		send(sFSObject);
	}

	public void doItemTrade(int craftID, int qty = 1)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", ITEM_TRADE);
		sFSObject.PutInt("cra0", craftID);
		sFSObject.PutInt("ite2", qty);
		send(sFSObject);
	}

	public void doItemFusion(int fusionID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", ITEM_FUSION);
		sFSObject.PutInt("ite0", fusionID);
		send(sFSObject);
	}

	public void doItemContents(int id, int type)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", ITEM_CONTENTS);
		sFSObject.PutInt("ite0", id);
		sFSObject.PutInt("ite1", type);
		send(sFSObject);
	}

	public void doItemUpgrade(int id, int type, int craftID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", ITEM_UPGRADE);
		sFSObject.PutInt("ite0", id);
		sFSObject.PutInt("ite1", type);
		sFSObject.PutInt("cra0", craftID);
		send(sFSObject);
	}

	public void doItemExchange(List<ItemData> items)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", ITEM_EXCHANGE);
		sFSObject = ItemData.listToSFSObject(sFSObject, items);
		send(sFSObject);
	}

	public void doItemReforge(ItemRef itemRef, ItemRef resultRef)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", ITEM_REFORGE);
		sFSObject.PutInt("ite0", itemRef.id);
		sFSObject.PutInt("ite1", itemRef.itemType);
		sFSObject.PutInt("ite8", resultRef.id);
		sFSObject.PutInt("ite9", resultRef.itemType);
		send(sFSObject);
	}

	public void doPaymentSteamStart(PaymentRef paymentRef)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", PAYMENT_STEAM_START);
		sFSObject.PutInt("pay0", paymentRef.id);
		sFSObject.PutUtfString("pay10", paymentRef.name);
		sFSObject.PutUtfString("use3", SteamLogin.GetSteamUserID());
		send(sFSObject);
	}

	public void doPaymentSteamComplete()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", PAYMENT_STEAM_COMPLETE);
		send(sFSObject);
	}
}
