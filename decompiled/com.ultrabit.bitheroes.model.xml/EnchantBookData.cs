using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class EnchantBookData : BaseBook
{
	public class Slot : BaseBookItem
	{
		[XmlAttribute("levelReq")]
		public int levelReq { get; set; }
	}

	public class Slots : BaseBookItem
	{
		[XmlElement("slot")]
		public List<Slot> lstSlot { get; set; }
	}

	public class Modifier : BaseBookItem
	{
		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }
	}

	public class Modifiers : BaseBookItem
	{
		[XmlElement("modifier")]
		public List<Modifier> lstModifier { get; set; }
	}

	public class Enchant : BaseBookItem
	{
		[XmlAttribute("allowReroll")]
		public string allowReroll;

		[XmlAttribute("stats")]
		public int stats { get; set; }

		[XmlAttribute("modsMin")]
		public int modsMin { get; set; }

		[XmlAttribute("modsMax")]
		public int modsMax { get; set; }
	}

	public class Enchants
	{
		[XmlElement("enchant")]
		public List<Enchant> lstEnchant { get; set; }
	}

	[XmlElement("slots")]
	public Slots slots { get; set; }

	[XmlElement("modifiers")]
	public Modifiers modifiers { get; set; }

	[XmlElement("enchants")]
	public Enchants enchants { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return enchants.lstEnchant.Find((Enchant item) => item.id.Equals(identifier));
	}
}
