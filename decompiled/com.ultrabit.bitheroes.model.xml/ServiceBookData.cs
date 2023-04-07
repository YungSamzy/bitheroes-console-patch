using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class ServiceBookData : BaseBook
{
	public class Service : BaseBookItem
	{
		[XmlAttribute("value")]
		public int value { get; set; }

		[XmlAttribute("visible")]
		public string visible { get; set; }
	}

	public class Services : BaseBookItem
	{
		[XmlElement("service")]
		public List<Service> lstService { get; set; }
	}

	[XmlElement("services")]
	public Services services;

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return services.lstService.Find((Service item) => item.id.Equals(identifier));
	}

	public void Clear()
	{
		services.lstService.Clear();
		services = null;
	}
}
