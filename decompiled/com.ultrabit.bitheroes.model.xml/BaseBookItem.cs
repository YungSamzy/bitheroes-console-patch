using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

public class BaseBookItem
{
	[XmlAttribute("id")]
	public int id;

	[XmlAttribute("name")]
	public string name;

	[XmlAttribute("link")]
	public string link;

	[XmlAttribute("desc")]
	public string desc;

	[XmlAttribute("box")]
	public string box;

	[XmlAttribute("statname")]
	public string statname;

	[XmlAttribute("thumbnail")]
	public string thumbnail;

	[XmlAttribute("icon")]
	public string icon;

	[XmlAttribute("type")]
	public string type;

	[XmlAttribute("rarity")]
	public string rarity;

	[XmlAttribute("loadLocal")]
	public string loadLocalString;

	[XmlAttribute("probability")]
	public string probability;

	[XmlAttribute("costGold")]
	public int costGold;

	[XmlAttribute("costCredits")]
	public int costCredits;

	[XmlAttribute("sellGold")]
	public int sellGold;

	[XmlAttribute("sellCredits")]
	public int sellCredits;

	[XmlAttribute("tier")]
	public int tier;

	[XmlAttribute("exchangeable")]
	public string exchangeableString;

	[XmlAttribute("unique")]
	public string unique;

	[XmlAttribute("allowQty")]
	public string allowQty;

	[XmlAttribute("lootDisplay")]
	public string lootDisplay;

	[XmlAttribute("cosmetic")]
	public string cosmetic;

	[XmlAttribute("assetsOverride")]
	public string assetsOverride;

	[XmlAttribute("assetsSourceID")]
	public int assetsSourceID;

	[XmlAttribute("attachSource")]
	public string attachSource;

	[XmlAttribute("setSourceID")]
	public int setSourceID;

	[XmlAttribute("tutorial")]
	public string tutorial;

	[XmlAttribute("gacha")]
	public string gacha;

	[XmlAttribute("itemOwner")]
	public string itemOwner;

	public bool loadLocal
	{
		get
		{
			if (loadLocalString == null)
			{
				return true;
			}
			return loadLocalString.ToLower().Equals("true");
		}
	}

	public bool exchangeable
	{
		get
		{
			if (exchangeableString == null)
			{
				return true;
			}
			return exchangeableString.ToLower().Trim().Equals("true");
		}
	}
}
