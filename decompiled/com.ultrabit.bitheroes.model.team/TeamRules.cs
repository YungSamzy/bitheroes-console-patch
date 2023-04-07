using System.Collections.Generic;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.rarity;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.team;

public class TeamRules
{
	private int _slots;

	private int _size;

	private bool _allowFamiliars;

	private bool _allowFriends;

	private bool _allowGuildmates;

	private bool _statBalance;

	private bool _allowPlayerMultiHero;

	private float _familiarMult = 1f;

	private List<RarityRef> _familiarRarities;

	private List<FamiliarRef> _familiarsAdded;

	public int slots => _slots;

	public int size => _size;

	public bool allowFamiliars => _allowFamiliars;

	public bool allowFriends => _allowFriends;

	public bool allowGuildmates => _allowGuildmates;

	public bool statBalance => _statBalance;

	public bool allowPlayerMultiHero => _allowPlayerMultiHero;

	public List<FamiliarRef> familiarsAdded => _familiarsAdded;

	public TeamRules(int slots, int size, bool allowFamiliars, bool allowFriends, bool allowGuildmates, bool statBalance, bool allowPlayerMultiHero = false)
	{
		_slots = slots;
		_size = size;
		_allowFamiliars = allowFamiliars;
		_allowFriends = allowFriends;
		_allowGuildmates = allowGuildmates;
		_statBalance = statBalance;
		_allowPlayerMultiHero = allowPlayerMultiHero;
	}

	public void setModifiers(float familiarMult, List<RarityRef> familiarRarities, List<FamiliarRef> familiarsAdded)
	{
		_familiarMult = familiarMult;
		_familiarRarities = familiarRarities;
		_familiarsAdded = familiarsAdded;
	}

	public bool matches(TeamRules teamRules)
	{
		if (slots == teamRules.slots && size == teamRules.size && allowFamiliars == teamRules.allowFamiliars && allowFriends == teamRules.allowFriends && allowGuildmates == teamRules.allowGuildmates && statBalance == teamRules.statBalance && allowPlayerMultiHero == teamRules.allowPlayerMultiHero)
		{
			return true;
		}
		return false;
	}

	public bool hasFamiliarAdded(FamiliarRef familiarRef)
	{
		if (_familiarsAdded == null)
		{
			return false;
		}
		foreach (FamiliarRef item in _familiarsAdded)
		{
			if (familiarRef.id == item.id)
			{
				return true;
			}
		}
		return false;
	}

	public TeamRules copy()
	{
		TeamRules teamRules = new TeamRules(_slots, _size, _allowFamiliars, _allowFriends, _allowGuildmates, _statBalance, _allowPlayerMultiHero);
		teamRules.setModifiers(_familiarMult, _familiarRarities, _familiarsAdded);
		return teamRules;
	}

	public SFSObject toSFSObject(SFSObject sfsob)
	{
		sfsob.PutInt("team1", _slots);
		sfsob.PutInt("team6", _size);
		sfsob.PutBool("team2", _allowFamiliars);
		sfsob.PutBool("team3", _allowFriends);
		sfsob.PutBool("team4", _allowGuildmates);
		sfsob.PutBool("team5", _statBalance);
		sfsob.PutBool("team8", _allowPlayerMultiHero);
		return sfsob;
	}

	public static TeamRules fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("team1");
		int int2 = sfsob.GetInt("team6");
		bool @bool = sfsob.GetBool("team2");
		bool bool2 = sfsob.GetBool("team3");
		bool bool3 = sfsob.GetBool("team4");
		bool bool4 = sfsob.GetBool("team5");
		bool bool5 = sfsob.GetBool("team8");
		return new TeamRules(@int, int2, @bool, bool2, bool3, bool4, bool5);
	}

	public float getFamiliarMult(FamiliarRef familiarRef)
	{
		if (_familiarRarities == null || _familiarRarities.Count <= 0)
		{
			return _familiarMult;
		}
		foreach (RarityRef familiarRarity in _familiarRarities)
		{
			if (familiarRarity == familiarRef.rarityRef)
			{
				return _familiarMult;
			}
		}
		return 1f;
	}
}
