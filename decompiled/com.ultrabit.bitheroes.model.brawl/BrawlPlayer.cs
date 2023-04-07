using System.Collections.Generic;
using com.ultrabit.bitheroes.model.character;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.brawl;

public class BrawlPlayer
{
	private CharacterData _characterData;

	private bool _ready;

	public CharacterData characterData => _characterData;

	public bool ready => _ready;

	public BrawlPlayer(CharacterData characterData)
	{
		_characterData = characterData;
	}

	public void setReady(bool ready)
	{
		_ready = ready;
	}

	public static BrawlPlayer fromSFSObject(ISFSObject sfsob)
	{
		CharacterData characterData = CharacterData.fromSFSObject(sfsob);
		if (characterData == null)
		{
			return null;
		}
		bool @bool = sfsob.GetBool("bra7");
		BrawlPlayer brawlPlayer = new BrawlPlayer(characterData);
		brawlPlayer.setReady(@bool);
		return brawlPlayer;
	}

	public static List<BrawlPlayer> listFromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("bra6"))
		{
			return null;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("bra6");
		List<BrawlPlayer> list = new List<BrawlPlayer>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			BrawlPlayer item = fromSFSObject(sFSArray.GetSFSObject(i));
			list.Add(item);
		}
		return list;
	}
}
