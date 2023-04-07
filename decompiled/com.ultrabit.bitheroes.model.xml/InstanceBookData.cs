using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class InstanceBookData : BaseBook
{
	[XmlRoot("objects")]
	public class Objects : BaseBookItem
	{
		[XmlElement("object")]
		public List<AssetDisplayData> lstObject { get; set; }
	}

	public class Offset : BaseBookItem
	{
		[XmlAttribute("x")]
		public int x { get; set; }

		[XmlAttribute("y")]
		public int y { get; set; }

		[XmlAttribute("tiles")]
		public string tiles { get; set; }
	}

	public class Offsets : BaseBookItem
	{
		[XmlElement("offset")]
		public List<Offset> lstOffset { get; set; }
	}

	public class Transition : BaseBookItem
	{
		[XmlAttribute("instance")]
		public string instance { get; set; }

		[XmlAttribute("tiles")]
		public string tiles { get; set; }
	}

	public class Transitions : BaseBookItem
	{
		[XmlElement("transition")]
		public List<Transition> lstTransition { get; set; }
	}

	public class Instance : BaseBookItem
	{
		[XmlElement("objects")]
		public Objects objects { get; set; }

		[XmlElement("offsets")]
		public Offsets offsets { get; set; }

		[XmlElement("transitions")]
		public Transitions transitions { get; set; }

		[XmlAttribute("players")]
		public int players { get; set; }

		[XmlAttribute("size")]
		public int size { get; set; }

		[XmlAttribute("width")]
		public int width { get; set; }

		[XmlAttribute("height")]
		public int height { get; set; }

		[XmlAttribute("asset")]
		public string asset { get; set; }

		[XmlAttribute("music")]
		public string music { get; set; }

		[XmlAttribute("spawns")]
		public string spawns { get; set; }

		[XmlAttribute("walkable")]
		public string walkable { get; set; }

		[XmlAttribute("footsteps")]
		public string footsteps { get; set; }

		[XmlAttribute("footstepsDefault")]
		public string footstepsDefault { get; set; }
	}

	[XmlElement("instance")]
	public List<Instance> lstInstance { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return lstInstance.Find((Instance item) => item.id.Equals(identifier));
	}
}
