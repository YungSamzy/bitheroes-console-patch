using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class FishBookData : BaseBook
{
	public class Fish : BaseBookItem
	{
	}

	[XmlElement("fish")]
	public List<Fish> lstFish { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return lstFish.Find((Fish item) => item.id.Equals(identifier));
	}
}
