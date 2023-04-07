using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class RarityBookData : BaseBook
{
	public class Rarity : BaseBookItem
	{
		[XmlAttribute("textColor")]
		public string textColor;

		[XmlAttribute("objectColor")]
		public string objectColor;

		[XmlAttribute("objectColorUnity")]
		public string objectColorUnity;

		[XmlAttribute("magidFindMult")]
		public float magicFindMult;

		[XmlAttribute("familiarChanceMult")]
		public float familiarChanceMult;

		[XmlAttribute("augmentMax")]
		public int augmentMax;

		[XmlAttribute("notifyGlobal")]
		public string notifyGlobal;

		[XmlAttribute("exchangeLoot")]
		public string exchangeLoot;

		[XmlAttribute("captureChancePerc")]
		public float captureChancePerc;

		[XmlAttribute("captureSuccessPerc")]
		public float captureSuccessPerc;

		[XmlAttribute("captureChanceServiceID")]
		public int captureChanceServiceID;

		[XmlAttribute("captureGuaranteeServiceID")]
		public int captureGuaranteeServiceID;

		[XmlAttribute("merchantServiceID")]
		public int merchantServiceID;

		[XmlAttribute("overlayAlpha")]
		public float overlayAlpha;

		[XmlAttribute("filtered")]
		public string filteredString;

		[XmlAttribute("filterDefault")]
		public string filterDefaultString;

		[XmlAttribute("notifyDuration")]
		public string notifyDuration;

		public bool filtered
		{
			get
			{
				if (filteredString == null)
				{
					return true;
				}
				return filteredString.ToLower().Contains("true");
			}
		}

		public bool filterDefault => filterDefaultString.ToLower().Trim().Equals("true");
	}

	[XmlElement("rarity")]
	public List<Rarity> lstRarity;

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return lstRarity.Find((Rarity item) => item.link.Equals(identifier));
	}

	public void Clear()
	{
		lstRarity.Clear();
		lstRarity = null;
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
