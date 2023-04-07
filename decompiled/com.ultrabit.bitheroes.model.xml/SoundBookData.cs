using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class SoundBookData : BaseBook
{
	public class SoundPool : BaseBookItem
	{
		[XmlAttribute("sounds")]
		public string sounds;
	}

	public class Sound : BaseBookItem
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

		[XmlAttribute("musicVolume")]
		public float musicVolume;

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

	[XmlElement("pool")]
	public List<SoundPool> pool;

	[XmlElement("sound")]
	public List<Sound> lstSound;

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return lstSound.Find((Sound item) => item.link.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}

	public void Clear()
	{
		pool.Clear();
		pool = null;
		lstSound.Clear();
		lstSound = null;
	}
}
