using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class AugmentBookData : BaseBook
{
	public class Augments : BaseBookItem
	{
		[XmlElement("augment")]
		public List<Augment> lstAugment { get; set; }
	}

	public class Types : BaseBookItem
	{
		[XmlElement("type")]
		public List<Type> lstType { get; set; }
	}

	public class Type : BaseBookItem
	{
		[XmlElement("modifier")]
		public List<Modifier> lstModifiers { get; set; }
	}

	public class Modifier : BaseBookItem
	{
		[XmlElement("rank")]
		public List<Rank> lstRank { get; set; }
	}

	public class Rank : BaseBookItem
	{
		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }

		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }
	}

	public class Slots : BaseBookItem
	{
		[XmlElement("slot")]
		public List<Slot> lstSlot { get; set; }
	}

	public class Slot : BaseBookItem
	{
		[XmlAttribute("levelReq")]
		public int levelReq { get; set; }
	}

	public class Augment : BaseBookItem
	{
		[XmlAttribute("exchangeLoot")]
		public string exchangeLoot { get; set; }
	}

	[XmlElement("types")]
	public Types types { get; set; }

	[XmlElement("slots")]
	public Slots slots { get; set; }

	[XmlElement("augments")]
	public Augments augments { get; set; }

	public void Clear()
	{
		augments.lstAugment.Clear();
		augments = null;
		slots.lstSlot.Clear();
		slots = null;
		types.lstType.Clear();
		types = null;
	}

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return augments.lstAugment.Find((Augment item) => item.id.Equals(identifier));
	}
}
