using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class ConsumableBookData : BaseBook
{
	public class Consumable : BaseBookItem
	{
		[XmlAttribute("value")]
		public string value { get; set; }

		[XmlAttribute("currencyID")]
		public int currencyID { get; set; }

		[XmlAttribute("autoConsume")]
		public string autoConsume { get; set; }

		[XmlAttribute("viewable")]
		public string viewable { get; set; }

		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }

		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }

		[XmlAttribute("summary")]
		public string summary { get; set; }

		[XmlAttribute("displayQty")]
		public string displayQty { get; set; }

		[XmlAttribute("infinite")]
		public string infinite { get; set; }

		[XmlAttribute("displayUnlocked")]
		public string displayUnlocked { get; set; }

		[XmlAttribute("displayCompare")]
		public string displayCompare { get; set; }

		[XmlAttribute("notify")]
		public string notify { get; set; }

		[XmlAttribute("forceConsume")]
		public string forceConsume { get; set; }

		[XmlAttribute("purchaseLimit")]
		public int purchaseLimit { get; set; }

		[XmlAttribute("hidden")]
		public string hidden { get; set; }

		[XmlAttribute("consumableItemType")]
		public string consumableItemType { get; set; }

		[XmlAttribute("requiredZone")]
		public string requiredZone { get; set; }

		[XmlAttribute("eventRequired")]
		public string eventRequired { get; set; }
	}

	[XmlElement("consumable")]
	public List<Consumable> lstConsumable { get; set; }

	public void Clear()
	{
		lstConsumable.Clear();
		lstConsumable = null;
	}

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return lstConsumable.Find((Consumable item) => item.id.Equals(identifier));
	}
}
