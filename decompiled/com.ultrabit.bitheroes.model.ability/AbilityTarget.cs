using System.Collections.Generic;

namespace com.ultrabit.bitheroes.model.ability;

public class AbilityTarget
{
	public const int TYPE_NONE = 0;

	public const int TYPE_ALL = 1;

	public const int TYPE_SELF = 2;

	public const int TYPE_SELF_FRONT = 3;

	public const int TYPE_SELF_BACK = 4;

	public const int TYPE_SELF_TEAM = 5;

	public const int TYPE_SELF_TEAM_OTHERS = 6;

	public const int TYPE_ENEMY_FRONT = 7;

	public const int TYPE_ENEMY_BACK = 8;

	public const int TYPE_ENEMY_TEAM = 9;

	public const int TYPE_ENEMY_TEAM_OTHERS = 10;

	public const int TYPE_SELECT = 11;

	public const int TYPE_SELECT_TEAM = 12;

	public const int TYPE_SELECT_TEAM_OTHERS = 13;

	private static Dictionary<string, int> TYPES = new Dictionary<string, int>
	{
		[""] = 0,
		["none"] = 0,
		["all"] = 1,
		["self"] = 2,
		["selffront"] = 3,
		["selfback"] = 4,
		["selfteam"] = 5,
		["selfteamothers"] = 6,
		["enemyfront"] = 7,
		["enemyback"] = 8,
		["enemyteam"] = 9,
		["enemyteamothers"] = 10,
		["select"] = 11,
		["selectteam"] = 12,
		["selectteamothers"] = 13
	};

	public static int getType(string type)
	{
		return TYPES[type.ToLowerInvariant()];
	}
}
