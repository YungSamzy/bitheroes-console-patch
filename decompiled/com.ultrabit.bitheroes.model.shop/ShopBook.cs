using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.shop;

public class ShopBook
{
	private static List<ShopPromoRef> _promos;

	private static List<ShopSaleRef> _sales;

	private static List<ShopSaleRef> _rotations;

	private static List<ShopSaleRef> _progressionOffer;

	private static List<ShopSaleRef> _vipgor;

	private static List<ShopTabRef> _tabs;

	public static int promosSize => _promos.Count;

	public static int salesSize => _sales.Count;

	public static int rotationsSize => _rotations.Count;

	public static int progressionOfferSize => _progressionOffer.Count;

	public static int vipgorSize => _vipgor.Count;

	public static int tabsSize => _tabs.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_promos = new List<ShopPromoRef>();
		_sales = new List<ShopSaleRef>();
		_rotations = new List<ShopSaleRef>();
		_progressionOffer = new List<ShopSaleRef>();
		_vipgor = new List<ShopSaleRef>();
		_tabs = new List<ShopTabRef>();
		int num = 0;
		foreach (ShopBookData.Promo item in XMLBook.instance.shopBook.promos.lstPromo)
		{
			_promos.Add(new ShopPromoRef(num, item));
			num++;
		}
		num = 0;
		foreach (ShopBookData.Item item2 in XMLBook.instance.shopBook.sales.lstItem)
		{
			_sales.Add(new ShopSaleRef(num, item2));
			num++;
		}
		num = 0;
		foreach (ShopBookData.Item item3 in XMLBook.instance.shopBook.rotations.lstItem)
		{
			_rotations.Add(new ShopSaleRef(num, item3));
			num++;
		}
		num = 0;
		foreach (ShopBookData.Item item4 in XMLBook.instance.shopBook.progressions.lstItem)
		{
			_progressionOffer.Add(new ShopSaleRef(num, item4));
			num++;
		}
		num = 0;
		foreach (ShopBookData.Item item5 in XMLBook.instance.shopBook.vipgor.lstItem)
		{
			_vipgor.Add(new ShopSaleRef(num, item5));
			num++;
		}
		num = 0;
		foreach (ShopBookData.Tab item6 in XMLBook.instance.shopBook.tabs.lstTab)
		{
			_tabs.Add(new ShopTabRef(num, item6));
			num++;
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static ShopSaleRef GetItemSaleRef(ItemRef itemRef, int rotation)
	{
		ShopSaleRef shopSaleRef = LookupRotation(rotation);
		if (shopSaleRef != null && shopSaleRef.itemRef == itemRef)
		{
			return shopSaleRef;
		}
		foreach (ShopSaleRef sale in _sales)
		{
			if (sale != null && sale.getActive() && sale.itemRef.Equals(itemRef))
			{
				return sale;
			}
		}
		return null;
	}

	public static long GetSaleChangeMilliseconds()
	{
		long num = 0L;
		foreach (ShopSaleRef sale in _sales)
		{
			if (sale != null && sale.dateRef != null)
			{
				long millisecondsUntilStart = sale.dateRef.getMillisecondsUntilStart();
				long millisecondsUntilEnd = sale.dateRef.getMillisecondsUntilEnd();
				if (millisecondsUntilStart > 0 && (num == 0L || millisecondsUntilStart < num))
				{
					num = millisecondsUntilStart;
				}
				if (millisecondsUntilEnd > 0 && (num == 0L || millisecondsUntilEnd < num))
				{
					num = millisecondsUntilEnd;
				}
			}
		}
		return num;
	}

	public static ShopPromoRef LookupPromo(int id)
	{
		if (id < 0 || id >= _promos.Count)
		{
			return null;
		}
		return _promos[id];
	}

	public static ShopSaleRef LookupSale(int index)
	{
		if (index < 0 || index >= _sales.Count)
		{
			return null;
		}
		return _sales[index];
	}

	public static ShopSaleRef LookupRotation(int index)
	{
		if (index < 0 || index >= _rotations.Count)
		{
			return null;
		}
		return _rotations[index];
	}

	public static ShopTabRef LookupTab(int index)
	{
		if (index < 0 || index >= _tabs.Count)
		{
			return null;
		}
		return _tabs[index];
	}

	public static ShopSaleRef LookupProgressionOffer(int id)
	{
		if (id >= 0 && id < _progressionOffer.Count)
		{
			return _progressionOffer[id];
		}
		return null;
	}

	public static ShopSaleRef LookupVipgor(int id)
	{
		if (id >= 0 && id < _vipgor.Count)
		{
			return _vipgor[id];
		}
		return null;
	}
}
