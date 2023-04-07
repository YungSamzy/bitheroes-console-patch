using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class EquipmentBookData : BaseBook
{
	public class Subtype : BaseBookItem
	{
		[XmlAttribute("tooltip")]
		public string tooltip { get; set; }
	}

	public class Equipment : BaseBookItem
	{
		[XmlElement("asset")]
		public List<Asset> lstAsset { get; set; }

		[XmlAttribute("power")]
		public int power { get; set; }

		[XmlAttribute("stamina")]
		public int stamina { get; set; }

		[XmlAttribute("agility")]
		public int agility { get; set; }

		[XmlAttribute("elemental")]
		public string elemental { get; set; }

		[XmlAttribute("subtypes")]
		public string subtypes { get; set; }

		[XmlAttribute("abilities")]
		public string abilities { get; set; }

		[XmlAttribute("projectileOffset")]
		public string projectileOffset { get; set; }

		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }

		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }

		[XmlAttribute("order")]
		public int order { get; set; }

		[XmlAttribute("exchangeLoot")]
		public string exchangeLoot { get; set; }

		[XmlElement("upgrade")]
		public List<Upgrade> lstUpgrade { get; set; }

		[XmlElement("reforge")]
		public List<Reforge> lstReforge { get; set; }

		[XmlAttribute("projectileCenter")]
		public string projectileCenter { get; set; }
	}

	public class Bonus : BaseBookItem
	{
		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }

		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }

		[XmlAttribute("count")]
		public int count { get; set; }

		[XmlAttribute("abilities")]
		public string abilities { get; set; }
	}

	[XmlRoot("equipmentset")]
	public class Equipmentset : BaseBookItem
	{
		[XmlElement("equipment")]
		public List<Equipment> lstEquipment { get; set; }

		[XmlElement("bonus")]
		public List<Bonus> lstBonus { get; set; }

		[XmlAttribute("textSize")]
		public int textSize { get; set; }
	}

	public class Asset : BaseBookItem
	{
		[XmlAttribute("url")]
		public string url { get; set; }

		[XmlAttribute("bodyPart")]
		public string bodyPart { get; set; }

		[XmlAttribute("offset")]
		public string offset { get; set; }

		[XmlAttribute("showSkin")]
		public string showSkin { get; set; }

		[XmlAttribute("underSkin")]
		public string underSkin { get; set; }

		[XmlAttribute("showHair")]
		public string showHair { get; set; }

		[XmlAttribute("playRandom")]
		public string playRandom { get; set; }

		[XmlAttribute("genders")]
		public string genders { get; set; }

		[XmlAttribute("definition")]
		public string definition { get; set; }

		[XmlAttribute("order")]
		public string order { get; set; }
	}

	public class Upgrade : BaseBookItem
	{
	}

	public class Reforge : BaseBookItem
	{
	}

	[XmlElement("subtype")]
	public List<Subtype> lstSubtype { get; set; }

	[XmlElement("equipmentset")]
	public List<Equipmentset> lstEquipmentSet { get; set; }

	[XmlElement("equipment")]
	public List<Equipment> lstEquipment { get; set; }

	public void Clear()
	{
		lstEquipment.Clear();
		lstEquipment = null;
		lstEquipmentSet.Clear();
		lstEquipmentSet = null;
		lstSubtype.Clear();
		lstSubtype = null;
	}

	public BaseBookItem GetSubtypeByIdentifier(string identifier, out int id)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
