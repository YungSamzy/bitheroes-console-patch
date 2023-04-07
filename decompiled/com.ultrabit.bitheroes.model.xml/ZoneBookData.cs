using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.zone;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class ZoneBookData : BaseBook
{
	public class DifficultyModes : BaseBookItem
	{
		[XmlElement("difficulty")]
		public List<DifficultyMode> lstDifficulties { get; set; }
	}

	public class DifficultyMode : BaseBookItem
	{
	}

	public class ZoneFiles : BaseBookItem
	{
		[XmlElement("zone")]
		public List<ZoneFile> lstFiles { get; set; }
	}

	public class ZoneFile : BaseBookItem
	{
		[XmlAttribute("xml")]
		public string xml { get; set; }
	}

	[XmlElement("difficulties")]
	public DifficultyModes difficultyModes;

	[XmlIgnore]
	public List<ZoneXMLData> dictZones;

	[XmlElement("zones")]
	public ZoneFiles zoneFiles { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
