using System.Collections.Generic;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rarity;

namespace com.ultrabit.bitheroes.model.guild;

public class Guild
{
	private const float LEVEL_MULTIPLIER = 50f;

	public const int RANK_LEADER = 0;

	public const int RANK_OFFICER = 1;

	public const int RANK_MEMBER = 2;

	public const int RANK_RECRUIT = 3;

	public const int RANKS = 4;

	public const int PERMISSION_KICK = 0;

	public const int PERMISSION_INVITE = 1;

	public const int PERMISSION_APPLICATIONS = 2;

	public const int PERMISSION_PROMOTE = 3;

	public const int PERMISSION_DEMOTE = 4;

	public const int PERMISSION_PERMISSIONS = 5;

	public const int PERMISSION_HALL_COSMETICS = 6;

	public const int PERMISSION_PERK_UPGRADE = 7;

	public const int PERMISSION_MESSAGE = 8;

	public static long getLevelExp(int level)
	{
		return (long)((float)Character.getLevelExp(level) * 50f);
	}

	public static int getExpLevel(long exp)
	{
		return (int)((float)Character.getExpLevel(exp) * 50f);
	}

	public static string getRankName(int rank)
	{
		return Language.GetString("guild_rank_" + rank + "_name");
	}

	public static string getRankColoredName(int rank)
	{
		string rankName = getRankName(rank);
		RarityRef rarityRef = RarityBook.LookupID(4 - rank - 1);
		if (rarityRef != null)
		{
			return rarityRef.ConvertString(rankName);
		}
		return rankName;
	}

	public static bool hasPermission(int action, List<bool> permissions)
	{
		if (action < 0 || action >= permissions.Count)
		{
			return false;
		}
		return permissions[action];
	}
}
