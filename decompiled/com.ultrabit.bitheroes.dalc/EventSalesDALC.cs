using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.eventsales;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class EventSalesDALC : BaseDALC
{
	public const int ITEM_PURCHASE = 0;

	public const int EVENT_REMAINING_PURCHASES = 1;

	private static EventSalesDALC _instance;

	public static EventSalesDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new EventSalesDALC();
			}
			return _instance;
		}
	}

	public void doItemPurchase(EventSalesShopEventRef eventRef, EventSalesShopItemRef itemRef, int qty = 1, int purchaseRemainingQty = -1)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 0);
		sFSObject.PutInt("eve0", eventRef.id);
		sFSObject.PutInt("ite0", itemRef.id);
		sFSObject.PutInt("ite2", qty);
		sFSObject.PutInt("pur10", purchaseRemainingQty);
		send(sFSObject);
	}

	public void doEventRemainingPurchases(EventSalesShopEventRef eventRef)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		sFSObject.PutInt("eve0", eventRef.id);
		send(sFSObject);
	}
}
