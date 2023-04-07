using System.Collections.Generic;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.user;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.guild;

public class GuildMemberData : UserData
{
	private int _rank;

	public bool eventAdded;

	public int rank => _rank;

	public GuildMemberData(CharacterData characterData, bool online, int rank)
		: base(characterData, online)
	{
		_rank = rank;
	}

	public new static GuildMemberData fromSFSObject(ISFSObject sfsob)
	{
		CharacterData obj = CharacterData.fromSFSObject(sfsob);
		bool @bool = sfsob.GetBool("use5");
		int @int = sfsob.GetInt("gui1");
		return new GuildMemberData(obj, @bool, @int);
	}

	public static List<GuildMemberData> listFromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("gui5"))
		{
			return null;
		}
		List<GuildMemberData> list = new List<GuildMemberData>();
		ISFSArray sFSArray = sfsob.GetSFSArray("gui5");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			GuildMemberData item = fromSFSObject(sFSArray.GetSFSObject(i));
			list.Add(item);
		}
		return list;
	}

	public void setRank(int rank)
	{
		_rank = rank;
	}
}
