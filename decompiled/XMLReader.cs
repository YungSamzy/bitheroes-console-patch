using System.Xml;
using com.ultrabit.bitheroes.model.utility;

public class XMLReader
{
	public static string getXmlString(XmlElement xml, string attribute, string def = "")
	{
		return getXmlAttribute(xml, attribute, def);
	}

	public static int getXmlInteger(XmlElement xml, string attribute, int def = 0)
	{
		return int.Parse(getXmlAttribute(xml, attribute, def.ToString()));
	}

	public static bool getXmlBoolean(XmlElement xml, string attribute, bool def = false)
	{
		return Util.parseBoolean(getXmlAttribute(xml, attribute, def.ToString()), def);
	}

	public static string getXmlAttribute(XmlElement xml, string attribute, string def)
	{
		if (!xml.HasAttribute(attribute))
		{
			return def;
		}
		return xml.GetAttribute(attribute);
	}

	public static string getXmlContentsString(XmlNode xml, string child, string def = "")
	{
		return getXmlContents(xml, child, def);
	}

	public static int getXmlContentsInteger(XmlNode xml, string child, int def = 0)
	{
		return int.Parse(getXmlContents(xml, child, def.ToString()));
	}

	public static bool getXmlContentsBoolean(XmlNode xml, string child, bool def = false)
	{
		return Util.parseBoolean(getXmlContents(xml, child, def.ToString()), def);
	}

	public static string getXmlContents(XmlNode xml, string child, string def)
	{
		XmlNode xmlNode = xml.SelectSingleNode(child);
		if (xmlNode == null)
		{
			return def;
		}
		return xmlNode.InnerText;
	}
}
