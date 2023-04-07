using System.Collections.Generic;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.guild;

public class GuildPerks
{
	private List<int> _perks;

	public GuildPerks(List<int> perks)
	{
		_perks = perks;
	}

	public int getPerkRank(int perk)
	{
		if (perk < 0 || perk >= _perks.Count)
		{
			return 0;
		}
		return _perks[perk];
	}

	public List<GameModifier> getModifiers()
	{
		List<GameModifier> list = new List<GameModifier>();
		for (int i = 0; i < _perks.Count; i++)
		{
			GuildPerkRef guildPerkRef = GuildBook.LookupPerk(i);
			if (guildPerkRef == null)
			{
				continue;
			}
			int rank = _perks[i];
			GuildPerkRankRef perkRank = guildPerkRef.getPerkRank(rank);
			if (perkRank == null)
			{
				continue;
			}
			foreach (GameModifier modifier in perkRank.modifiers)
			{
				list.Add(modifier);
			}
		}
		return list;
	}

	public static GuildPerks fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("gui15"))
		{
			return null;
		}
		return new GuildPerks(Util.arrayToIntegerVector(sfsob.GetIntArray("gui15")));
	}
}
