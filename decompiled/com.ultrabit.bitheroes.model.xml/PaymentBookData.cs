using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class PaymentBookData : BaseBook
{
	public class Payment : BaseBookItem
	{
		[XmlAttribute("paymentID")]
		public string paymentID { get; set; }

		[XmlAttribute("cost")]
		public string cost { get; set; }

		[XmlAttribute("credits")]
		public int credits { get; set; }

		[XmlElement("item")]
		public Item item { get; set; }

		[XmlAttribute("amount")]
		public int amount { get; set; }
	}

	public class Item : BaseBookItem
	{
		[XmlAttribute("qty")]
		public string qty { get; set; }
	}

	public class Platform : BaseBookItem
	{
		[XmlElement("payment")]
		public List<Payment> lstPayment { get; set; }

		[XmlAttribute("types")]
		public string types { get; set; }
	}

	[XmlElement("platform")]
	public List<Platform> lstPlatform { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
