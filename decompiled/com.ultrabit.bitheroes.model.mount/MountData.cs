using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.mount;

public class MountData : BaseModelData
{
	public UnityEvent CHANGE = new UnityEvent();

	private long _uid;

	private MountRef _mountRef;

	private float _powerMult;

	private float _staminaMult;

	private float _agilityMult;

	private List<MountModifierRef> _modifiers;

	private int _rank;

	private int _rerolls;

	private int _qty;

	public long uid => _uid;

	public MountRef mountRef => _mountRef;

	public int total => power + stamina + agility;

	public float powerMult => _powerMult;

	public float staminaMult => _staminaMult;

	public float agilityMult => _agilityMult;

	public List<MountModifierRef> modifiers => _modifiers;

	public int rank => _rank;

	public int rankMax => _mountRef.mountRarityRef.rankMax;

	public int rerolls => _rerolls;

	public override ItemRef itemRef => mountRef;

	public override int power => getPower(GameData.instance.PROJECT.character.tier);

	public override int stamina => getStamina(GameData.instance.PROJECT.character.tier);

	public override int agility => getAgility(GameData.instance.PROJECT.character.tier);

	public override object data => null;

	public override int qty
	{
		get
		{
			return 1;
		}
		set
		{
			_qty = value;
		}
	}

	public override int type => 8;

	public MountData(long uid, MountRef mountRef, float powerMult, float staminaMult, float agilityMult, List<MountModifierRef> modifiers, int rank, int rerolls)
	{
		_uid = uid;
		_mountRef = mountRef;
		_powerMult = powerMult;
		_staminaMult = staminaMult;
		_agilityMult = agilityMult;
		_modifiers = modifiers;
		_rank = rank;
		_rerolls = rerolls;
	}

	public void copyData(MountData mountData)
	{
		_powerMult = mountData.powerMult;
		_staminaMult = mountData.staminaMult;
		_agilityMult = mountData.agilityMult;
		_modifiers = mountData.modifiers;
		_rank = mountData.rank;
		_rerolls = mountData.rerolls;
		CHANGE.Invoke();
	}

	public int getTotalStat(int stat, int tier)
	{
		return stat switch
		{
			0 => getPower(tier), 
			1 => getStamina(tier), 
			2 => getAgility(tier), 
			_ => 0, 
		};
	}

	public CharacterStats getStats(int tier)
	{
		int stats = _mountRef.mountRarityRef.getStats(_rank, tier);
		int num = Mathf.RoundToInt((float)stats * _powerMult);
		int num2 = Mathf.RoundToInt((float)stats * _staminaMult);
		int num3 = Mathf.RoundToInt((float)stats * _agilityMult);
		CharacterStats characterStats = new CharacterStats(num, num2, num3);
		characterStats.balance(stats);
		return characterStats;
	}

	public int getTotal(int tier)
	{
		return getStats(tier).total;
	}

	public int getPower(int tier)
	{
		return getStats(tier).power;
	}

	public int getStamina(int tier)
	{
		return getStats(tier).stamina;
	}

	public int getAgility(int tier)
	{
		return getStats(tier).agility;
	}

	public List<GameModifier> getGameModifiers()
	{
		List<GameModifier> list = new List<GameModifier>();
		if (_mountRef.mountRarityRef != null && _mountRef.mountRarityRef.modifiers != null)
		{
			foreach (GameModifier modifier in _mountRef.mountRarityRef.modifiers)
			{
				list.Add(modifier);
			}
		}
		if (_mountRef != null && _mountRef.baseModifiers != null)
		{
			foreach (GameModifier baseModifier in _mountRef.baseModifiers)
			{
				list.Add(baseModifier);
			}
		}
		if (_modifiers != null)
		{
			foreach (MountModifierRef modifier2 in _modifiers)
			{
				foreach (GameModifier modifier3 in modifier2.modifiers)
				{
					list.Add(modifier3);
				}
			}
			return list;
		}
		return list;
	}

	public CraftUpgradeRef getUpgradeRef()
	{
		return _mountRef.mountRarityRef.getRankRef(_rank)?.upgradeRef;
	}

	public static MountData fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("moun1"))
		{
			return null;
		}
		long @long = sfsob.GetLong("moun1");
		MountRef mountRef = MountBook.Lookup(sfsob.GetInt("moun2"));
		if (mountRef == null)
		{
			return null;
		}
		float @float = sfsob.GetFloat("moun7");
		float float2 = sfsob.GetFloat("moun8");
		float float3 = sfsob.GetFloat("moun9");
		List<MountModifierRef> list = MountBook.LookupModifiers(Util.arrayToIntegerVector(sfsob.GetIntArray("moun10")));
		int @int = sfsob.GetInt("moun4");
		int int2 = sfsob.GetInt("moun5");
		return new MountData(@long, mountRef, @float, float2, float3, list, @int, int2);
	}

	public static List<MountData> listFromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("moun3"))
		{
			return null;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("moun3");
		List<MountData> list = new List<MountData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			MountData mountData = fromSFSObject(sFSArray.GetSFSObject(i));
			if (mountData != null)
			{
				list.Add(mountData);
			}
		}
		return list;
	}

	public static MountData fromXML(int id)
	{
		long num = -1L;
		MountRef mountRef = MountBook.Lookup(id);
		if (mountRef == null)
		{
			return null;
		}
		return new MountData(num, mountRef, 0f, 0f, 0f, new List<MountModifierRef>(), 0, 0);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		return (obj as MountData).uid == uid;
	}
}
