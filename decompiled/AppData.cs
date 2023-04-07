using System.Xml;
using com.ultrabit.bitheroes.model.application;

public class AppData
{
	public string versionUrl { get; private set; } = "";


	public string serversUrl { get; private set; } = "";


	public string contentUrl { get; private set; } = "";


	public AppData(string _versionUrl, string _serversUrl, string _contentUrl)
	{
		versionUrl = _versionUrl;
		serversUrl = _serversUrl;
		contentUrl = _contentUrl;
	}

	public static AppData fromXml(XmlDocument xml)
	{
		XmlElement xmlElement = null;
		XmlElement xmlElement2 = null;
		foreach (XmlElement item in xml.FirstChild.SelectNodes("platform"))
		{
			if (xmlElement != null && xmlElement2 != null)
			{
				break;
			}
			if (!item.HasAttribute("types"))
			{
				xmlElement = item;
				continue;
			}
			foreach (int item2 in AppInfo.getPlatformIDsFromString(item.GetAttribute("types")))
			{
				if (item2 == AppInfo.platform)
				{
					xmlElement2 = item;
					break;
				}
			}
		}
		string dataValue = getDataValue(xmlElement, xmlElement2, "versionUrl");
		string dataValue2 = getDataValue(xmlElement, xmlElement2, "serversUrl");
		string dataValue3 = getDataValue(xmlElement, xmlElement2, "contentUrl");
		return new AppData(dataValue, dataValue2, dataValue3);
	}

	public static string getDataValue(XmlElement dataDefault, XmlElement dataPlatform, string value)
	{
		if (dataPlatform != null)
		{
			XmlNode xmlNode = dataPlatform.SelectSingleNode(value);
			if (xmlNode != null)
			{
				return xmlNode.InnerText;
			}
		}
		if (dataDefault != null)
		{
			XmlNode xmlNode2 = dataDefault.SelectSingleNode(value);
			if (xmlNode2 != null)
			{
				return xmlNode2.InnerText;
			}
		}
		return "";
	}
}
