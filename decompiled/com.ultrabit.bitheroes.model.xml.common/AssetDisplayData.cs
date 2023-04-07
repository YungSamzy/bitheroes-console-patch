using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml.common;

public class AssetDisplayData : BaseBookItem
{
	public class Equipment : BaseBookItem
	{
	}

	public class Mount : BaseBookItem
	{
	}

	public class Action : BaseBookItem
	{
		[XmlAttribute("values")]
		public string values { get; set; }
	}

	[XmlAttribute("clickable")]
	public string clickable;

	[XmlAttribute("asset")]
	public string asset { get; set; }

	[XmlAttribute("animation")]
	public string animation { get; set; }

	[XmlAttribute("collision")]
	public string collision { get; set; }

	[XmlAttribute("spread")]
	public int spread { get; set; }

	[XmlAttribute("distance")]
	public int distance { get; set; }

	[XmlAttribute("instant")]
	public string instant { get; set; }

	[XmlElement("equipment")]
	public List<Equipment> lstEquipment { get; set; }

	[XmlElement("action")]
	public List<Action> lstAction { get; set; }

	[XmlAttribute("rewards")]
	public string rewards { get; set; }

	[XmlAttribute("tile")]
	public string tileString { get; set; }

	public int tile
	{
		get
		{
			if (tileString == null)
			{
				return -1;
			}
			return int.Parse(tileString);
		}
	}

	[XmlAttribute("display")]
	public string display { get; set; }

	[XmlAttribute("hair")]
	public int hair { get; set; }

	[XmlAttribute("hairColor")]
	public int hairColor { get; set; }

	[XmlAttribute("skinColor")]
	public int skinColor { get; set; }

	[XmlAttribute("gender")]
	public string gender { get; set; }

	[XmlAttribute("speed")]
	public string speed { get; set; }

	[XmlAttribute("flipped")]
	public string flipped { get; set; }

	[XmlAttribute("dialog")]
	public string dialog { get; set; }

	[XmlAttribute("blockedDialog")]
	public string blockedDialog { get; set; }

	[XmlElement("mount")]
	public List<Mount> lstMount { get; set; }

	[XmlAttribute("offset")]
	public string offset { get; set; }

	[XmlAttribute("value")]
	public string value { get; set; }

	[XmlAttribute("definition")]
	public string definition { get; set; }

	[XmlAttribute("order")]
	public int order { get; set; }

	[XmlAttribute("hidden")]
	public string hidden { get; set; }

	[XmlAttribute("startDate")]
	public string startDate { get; set; }

	[XmlAttribute("endDate")]
	public string endDate { get; set; }

	[XmlAttribute("scale")]
	public string scale { get; set; }

	[XmlAttribute("itemType")]
	public string itemType { get; set; }

	[XmlAttribute("itemID")]
	public string itemIDString { get; set; }

	public int itemID
	{
		get
		{
			if (itemIDString == null)
			{
				return -1;
			}
			return int.Parse(itemIDString);
		}
	}

	[XmlAttribute("point")]
	public string point { get; set; }

	[XmlAttribute("position")]
	public string position { get; set; }
}
