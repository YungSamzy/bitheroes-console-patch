using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.currency;

public class CurrencyBook
{
	private static Dictionary<int, CurrencyRef> _currencies = new Dictionary<int, CurrencyRef>();

	public static int size => _currencies.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_currencies = new Dictionary<int, CurrencyRef>();
		foreach (CurrencyBookData.Currency lstCurrency in XMLBook.instance.currencyBook.lstCurrencies)
		{
			CurrencyRef currencyRef = new CurrencyRef(lstCurrency.id, lstCurrency);
			currencyRef.LoadDetails(lstCurrency);
			_currencies.Add(lstCurrency.id, currencyRef);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static List<CurrencyRef> GetAllPossibleCurrencies()
	{
		return new List<CurrencyRef>(_currencies.Values);
	}

	public static CurrencyRef Lookup(int id)
	{
		if (_currencies.ContainsKey(id))
		{
			return _currencies[id];
		}
		return null;
	}
}
