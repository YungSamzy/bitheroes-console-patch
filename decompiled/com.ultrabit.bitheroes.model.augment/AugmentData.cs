using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using Sfs2X.Entities.Data;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.augment;

public class AugmentData : BaseModelData
{
	private long _uid;

	private AugmentRef _augmentRef;

	private List<AugmentModifierRef> _modifiers;

	private FamiliarRef _familiarRef;

	private int _slot;

	private int _rerolls;

	public UnityEvent OnChange = new UnityEvent();

	public long uid => _uid;

	public AugmentRef augmentRef => _augmentRef;

	public List<AugmentModifierRef> modifiers => _modifiers;

	public int familiarID
	{
		get
		{
			if (!(_familiarRef != null))
			{
				return -1;
			}
			return _familiarRef.id;
		}
	}

	public FamiliarRef familiarRef => _familiarRef;

	public int slot => _slot;

	public int rerolls => _rerolls;

	public bool equipped
	{
		get
		{
			if (_familiarRef != null)
			{
				return _slot >= 0;
			}
			return false;
		}
	}

	public override ItemRef itemRef => augmentRef;

	public override int power => 0;

	public override int stamina => 0;

	public override int agility => 0;

	public override object data => null;

	public override int qty
	{
		get
		{
			return 1;
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public override int type => 15;

	public AugmentData(long uid, AugmentRef augmentRef, List<AugmentModifierRef> modifiers, FamiliarRef familiarRef, int slot, int rerolls)
	{
		_uid = uid;
		_augmentRef = augmentRef;
		_modifiers = modifiers;
		_familiarRef = familiarRef;
		_slot = slot;
		_rerolls = rerolls;
	}

	public void copyData(AugmentData augmentData)
	{
		_modifiers = augmentData.modifiers;
		_familiarRef = augmentData.familiarRef;
		_slot = augmentData.slot;
		_rerolls = augmentData.rerolls;
		OnChange?.Invoke();
	}

	public void clearData()
	{
		setData(null, -1);
	}

	public void setData(FamiliarRef familiarRef, int slot)
	{
		_familiarRef = familiarRef;
		_slot = slot;
		OnChange?.Invoke();
	}

	public int getRank(FamiliarStable stable)
	{
		if (_familiarRef == null || stable == null)
		{
			return 0;
		}
		int rankMax = getRankMax();
		int num = stable.getFamiliarQty(_familiarRef);
		if (num > rankMax)
		{
			num = rankMax;
		}
		return num;
	}

	public int getRankMax()
	{
		if (_familiarRef == null)
		{
			return 0;
		}
		int num = _familiarRef.rarityRef.augmentMax;
		if (_familiarRef.isFusion())
		{
			num += VariableBook.fusionAugmentIncrease;
		}
		if (num > VariableBook.familiarStableMaxQty)
		{
			num = VariableBook.familiarStableMaxQty;
		}
		return num;
	}

	public List<GameModifier> getGameModifiers(int rank)
	{
		List<GameModifier> list = new List<GameModifier>();
		foreach (AugmentModifierRef modifier in _modifiers)
		{
			foreach (GameModifier rankModifier in modifier.getRankModifiers(rank))
			{
				list.Add(rankModifier);
			}
		}
		return list;
	}

	public static AugmentData fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("aug1"))
		{
			return null;
		}
		long @long = sfsob.GetLong("aug1");
		AugmentRef augmentRef = AugmentBook.Lookup(sfsob.GetInt("aug2"));
		List<AugmentModifierRef> list = AugmentBook.LookupModifiers(augmentRef.typeRef, Util.arrayToIntegerVector(sfsob.GetIntArray("aug3")));
		FamiliarRef familiarRef = FamiliarBook.Lookup(sfsob.GetInt("aug8"));
		int @int = sfsob.GetInt("aug6");
		int int2 = sfsob.GetInt("aug7");
		return new AugmentData(@long, augmentRef, list, familiarRef, @int, int2);
	}

	public static List<AugmentData> listFromSFSObject(ISFSObject sfsob)
	{
		if (sfsob == null || !sfsob.ContainsKey("aug4"))
		{
			return null;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("aug4");
		List<AugmentData> list = new List<AugmentData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			AugmentData item = fromSFSObject(sFSArray.GetSFSObject(i));
			list.Add(item);
		}
		return list;
	}
}
