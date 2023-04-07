using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.ui.assets;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class EncounterBookData : BaseBook
{
	public class Encounter : BaseBookItem
	{
		public string asset;

		public string definition;

		public string dialogVictory;

		public string dialogDefeat;

		[XmlElement(ElementName = "npc")]
		public List<NPCBookData.NPC> npc { get; set; }

		[XmlAttribute("loot")]
		public string loot { get; set; }

		[XmlAttribute("dialogStart")]
		public string dialogStart { get; set; }

		[XmlAttribute("guaranteeCapture")]
		public string guaranteeCapture { get; set; }

		public void loadAssets()
		{
		}

		public Asset getAsset(bool center = false, float scale = 1f)
		{
			if (asset != null)
			{
				_ = asset.Length;
				_ = 0;
			}
			return null;
		}

		public DialogRef getDialogStart()
		{
			if (dialogStart == null)
			{
				return null;
			}
			return DialogBook.Lookup(dialogStart);
		}

		public DialogRef getDialogVictory()
		{
			if (dialogVictory == null)
			{
				return null;
			}
			return DialogBook.Lookup(dialogVictory);
		}

		public DialogRef getDialogDefeat()
		{
			if (dialogDefeat == null)
			{
				return null;
			}
			return DialogBook.Lookup(dialogDefeat);
		}
	}

	[XmlElement("encounter")]
	public List<Encounter> lstEncounter { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier, out int id)
	{
		for (int i = 0; i < lstEncounter.Count; i++)
		{
			if (lstEncounter[i].link.Equals(identifier))
			{
				id = i;
				return lstEncounter[i];
			}
		}
		id = -1;
		return null;
	}

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return lstEncounter.Find((Encounter item) => item.link.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
