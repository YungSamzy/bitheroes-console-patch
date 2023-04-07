using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class FamiliarBookData : BaseBook
{
	public class Familiar : AssetDisplayData
	{
		[XmlAttribute("powerMult")]
		public float powerMult { get; set; }

		[XmlAttribute("staminaMult")]
		public float staminaMult { get; set; }

		[XmlAttribute("agilityMult")]
		public float agilityMult { get; set; }

		[XmlAttribute("npc")]
		public string npc { get; set; }

		[XmlAttribute("abilities")]
		public string abilities { get; set; }

		[XmlAttribute("selectOffset")]
		public string selectOffset { get; set; }

		[XmlAttribute("selectScale")]
		public float selectScale { get; set; }

		[XmlAttribute("disabledAugmentSlots")]
		public string disabledAugmentSlots { get; set; }

		[XmlAttribute("intro")]
		public string intro { get; set; }

		[XmlAttribute("obtainable")]
		public string obtainable { get; set; }
	}

	[XmlElement("familiar")]
	public List<Familiar> lstFamiliar { get; set; }

	public void Clear()
	{
		lstFamiliar.Clear();
		lstFamiliar = null;
	}

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return lstFamiliar.Find((Familiar item) => item.id.Equals(identifier));
	}
}
