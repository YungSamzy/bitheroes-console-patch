using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class WalletBookData : BaseBook
{
	public class NFTItems : BaseBookItem
	{
		[XmlElement("nft")]
		public List<NFT> nft;
	}

	public class Item : BaseBookItem
	{
		[XmlElement("qty")]
		public int qty;
	}

	public class NFT : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> items;

		[XmlAttribute("identifier")]
		public string identifier { get; set; }

		[XmlAttribute("source")]
		public string source { get; set; }
	}

	[XmlElement("nftitems")]
	public NFTItems nftItems;

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
