using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class ServerBookData : BaseBook
{
	public class Servers : BaseBookItem
	{
		[XmlElement("server")]
		public List<Server> server;
	}

	public class Server : BaseBookItem
	{
		[XmlElement("instance")]
		public List<ServerInstance> lstInstance;

		[XmlAttribute("url")]
		public string url;

		[XmlAttribute("recommended")]
		public string recommended;
	}

	public class ServerInstance
	{
		[XmlAttribute("id")]
		public string id;

		[XmlAttribute("name")]
		public string name;

		[XmlAttribute("url")]
		public string url;
	}

	[XmlElement("servers")]
	public Servers servers;

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
