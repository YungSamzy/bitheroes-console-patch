using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.ui.lists.fishingshoplist;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.fishing;

public class FishingShopPanel : MonoBehaviour
{
	public GameObject fishingShopListView;

	public GameObject fishingShopListScroll;

	public FishingShopList fishingShopList;

	private FishingShopTabRef _tabRef;

	private FishingShopWindow _fishingShopWindow;

	public FishingShopTabRef tabRef => _tabRef;

	public FishingShopWindow fishingShopWindow => _fishingShopWindow;

	public void LoadDetails(FishingShopWindow fishingShopWindow, FishingShopTabRef tabRef = null)
	{
		_fishingShopWindow = fishingShopWindow;
		_tabRef = tabRef;
		fishingShopList.InitList();
	}

	public void CheckTutorial()
	{
	}

	public void DoUpdate()
	{
		double virtualAbstractNormalizedScrollPosition = fishingShopList.GetVirtualAbstractNormalizedScrollPosition();
		fishingShopList.ClearList();
		foreach (FishingShopItemRef item in _tabRef.items)
		{
			fishingShopList.Data.InsertOneAtEnd(new FishingShopItem
			{
				fishingShopPanel = this,
				fishingItemRef = item
			});
		}
		fishingShopList.SetVirtualAbstractNormalizedScrollPosition(virtualAbstractNormalizedScrollPosition, computeVisibilityNow: true, out var _);
	}

	public void DoRefreshQty()
	{
		fishingShopList.Refresh();
	}

	public void DoShow()
	{
		fishingShopListView.SetActive(value: true);
		fishingShopListScroll.SetActive(value: true);
	}

	public void DoHide()
	{
		fishingShopListView.SetActive(value: false);
		fishingShopListScroll.SetActive(value: false);
	}
}
