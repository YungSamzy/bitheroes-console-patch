using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class MusicBookData : BaseBook
{
	public class Music : BaseBookItem
	{
		[XmlAttribute("url")]
		public string url;

		[XmlAttribute("volume")]
		public float volume;

		[XmlAttribute("duration")]
		public int duration;

		[XmlAttribute("value")]
		public string value;

		[XmlAttribute("loadImmediately")]
		public string loadImmediatelyString;

		public bool loadImmediately
		{
			get
			{
				if (loadImmediatelyString == null)
				{
					return false;
				}
				return loadImmediatelyString.ToLower().Equals("true");
			}
		}
	}

	[XmlElement("music")]
	public List<Music> lstMusic;

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return lstMusic.Find((Music item) => item.link.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}

	public void Clear()
	{
		lstMusic.Clear();
		lstMusic = null;
	}
}
