using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.enchant;

public class EnchantData : BaseModelData
{
	private long _uid;

	private EnchantRef _enchantRef;

	private int _power;

	private int _stamina;

	private int _agility;

	private List<EnchantModifierRef> _modifiers;

	private int _rerolls;

	private bool _allowReroll;

	private int _slot;

	public UnityEvent OnChange = new UnityEvent();

	public long uid => _uid;

	public EnchantRef enchantRef => _enchantRef;

	public int total => power + stamina + agility;

	public List<EnchantModifierRef> modifiers => _modifiers;

	public int rerolls => _rerolls;

	public int slot
	{
		get
		{
			return _slot;
		}
		set
		{
			_slot = value;
		}
	}

	public bool allowReroll => _allowReroll;

	public override ItemRef itemRef => enchantRef;

	public override int power => _power;

	public override int stamina => _stamina;

	public override int agility => _agility;

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

	public override int type => 11;

	public int tier => _enchantRef.tier;

	public int rarity => _enchantRef.rarity;

	public EnchantData(long uid, EnchantRef enchantRef, int power, int stamina, int agility, bool allowReroll, List<EnchantModifierRef> modifiers, int rerolls)
	{
		_uid = uid;
		_enchantRef = enchantRef;
		_power = power;
		_stamina = stamina;
		_agility = agility;
		_modifiers = modifiers;
		_rerolls = rerolls;
		_allowReroll = allowReroll;
		_slot = -1;
	}

	public void copyData(EnchantData enchantData)
	{
		_power = enchantData.power;
		_stamina = enchantData.stamina;
		_agility = enchantData.agility;
		_modifiers = enchantData.modifiers;
		_allowReroll = enchantData.allowReroll;
		_rerolls = enchantData.rerolls;
		OnChange?.Invoke();
	}

	public List<GameModifier> getGameModifiers()
	{
		List<GameModifier> list = new List<GameModifier>();
		foreach (EnchantModifierRef modifier in _modifiers)
		{
			foreach (GameModifier modifier2 in modifier.modifiers)
			{
				list.Add(modifier2);
			}
		}
		return list;
	}

	public static EnchantData fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("ench1"))
		{
			return null;
		}
		long @long = sfsob.GetLong("ench1");
		EnchantRef enchantRef = EnchantBook.Lookup(sfsob.GetInt("ench2"));
		return new EnchantData(power: sfsob.GetInt("ench3"), stamina: sfsob.GetInt("ench4"), agility: sfsob.GetInt("ench5"), modifiers: EnchantBook.lookupModifiers(Util.arrayToIntegerVector(sfsob.GetIntArray("ench6"))), rerolls: sfsob.GetInt("ench10"), uid: @long, enchantRef: enchantRef, allowReroll: enchantRef.allowReroll);
	}

	public static List<EnchantData> listFromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("ench7"))
		{
			return null;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("ench7");
		List<EnchantData> list = new List<EnchantData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			EnchantData item = fromSFSObject(sFSArray.GetSFSObject(i));
			list.Add(item);
		}
		return list;
	}
}
