using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class NPCBookData : BaseBook
{
	public class NPC : AssetDisplayData
	{
		[XmlAttribute("power")]
		public float power { get; set; }

		[XmlAttribute("stamina")]
		public float stamina { get; set; }

		[XmlAttribute("agility")]
		public float agility { get; set; }

		[XmlAttribute("abilities")]
		public string abilities { get; set; }

		[XmlAttribute("priority")]
		public int priority { get; set; }

		[XmlAttribute("loot")]
		public string loot { get; set; }

		[XmlAttribute("boss")]
		public string boss { get; set; }

		[XmlAttribute("familiar")]
		public string familiar { get; set; }

		[XmlAttribute("captureChancePerc")]
		public float captureChancePerc { get; set; }

		[XmlAttribute("anchorTop")]
		public string anchorTop { get; set; }

		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }

		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }
	}

	public class Equipment : BaseBookItem
	{
	}

	public class Mount : BaseBookItem
	{
	}

	[XmlElement(ElementName = "npc")]
	public List<NPC> lstNPC { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return lstNPC.Find((NPC item) => item.link.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
