using com.ultrabit.bitheroes.model.fishing;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class FishingDALC : BaseDALC
{
	public const int EVENT_STATS = 0;

	public const int EVENT_LOOT_CHECK = 1;

	public const int EVENT_LOOT_ITEMS = 2;

	public const int ITEM_PURCHASE = 3;

	private static FishingDALC _instance;

	public static FishingDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new FishingDALC();
			}
			return _instance;
		}
	}

	public void doEventStats(int eventID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 0);
		sFSObject.PutInt("eve0", eventID);
		send(sFSObject);
	}

	public void doEventLootCheck()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		send(sFSObject);
	}

	public void doEventLootItems(int eventID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		sFSObject.PutInt("eve0", eventID);
		send(sFSObject);
	}

	public void doItemPurchase(FishingShopItemRef itemRef, int qty = 1)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 3);
		sFSObject.PutInt("ite0", itemRef.id);
		sFSObject.PutInt("ite2", qty);
		send(sFSObject);
	}
}
