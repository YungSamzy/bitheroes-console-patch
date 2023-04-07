using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class CurrencyBookData : BaseBook
{
	public class Currency : BaseBookItem
	{
	}

	[XmlElement("currency")]
	public List<Currency> lstCurrencies { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return lstCurrencies.Find((Currency item) => item.id.Equals(identifier));
	}

	public void Clear()
	{
		lstCurrencies.Clear();
		lstCurrencies = null;
	}
}
