using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.fusion;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.npc;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.familiar;

[DebuggerDisplay("{name} (FamiliarRef)")]
public class FamiliarRef : ItemRef, IEquatable<FamiliarRef>, IComparable<FamiliarRef>
{
	private AssetDisplayRef _displayRef;

	private List<int> _disabledAugmentSlots;

	private NPCRef _npcRef;

	private List<FusionRef> _fusionResults;

	private string _npc;

	private float _powerMult;

	private float _staminaMult;

	private float _agilityMult;

	private Vector2 _selectOffset;

	private float _selectScale;

	private bool _intro;

	private bool _obtainable;

	public Rect bounds => Rect.zero;

	public AssetDisplayRef displayRef
	{
		get
		{
			if (_displayRef != null)
			{
				return _displayRef;
			}
			NPCRef nPCRef = getNPCRef();
			if (nPCRef != null)
			{
				return nPCRef.displayRef;
			}
			return _displayRef;
		}
	}

	public List<AbilityRef> abilities
	{
		get
		{
			NPCRef nPCRef = getNPCRef();
			if (nPCRef != null && nPCRef.abilities != null && nPCRef.abilities.Count > 0)
			{
				return nPCRef.abilities;
			}
			return null;
		}
	}

	public List<GameModifier> modifiers
	{
		get
		{
			NPCRef nPCRef = getNPCRef();
			if (nPCRef != null && nPCRef.modifiers != null && nPCRef.modifiers.Count > 0)
			{
				return nPCRef.modifiers;
			}
			return null;
		}
	}

	private float getStatMultTotal => _powerMult + _staminaMult + _agilityMult;

	public Vector2 selectOffset => _selectOffset;

	public float selectScale => _selectScale;

	public bool intro => _intro;

	public bool obtainable => _obtainable;

	public string npc => _npc;

	public FamiliarRef(int id, FamiliarBookData.Familiar familiarData)
		: base(id, 6)
	{
		_disabledAugmentSlots = Util.GetIntListFromStringProperty(familiarData.disabledAugmentSlots);
		_displayRef = new AssetDisplayRef(familiarData, AssetURL.NPC);
		if (_displayRef.asset == null && _displayRef.equipment == null && (_displayRef.itemID < 0 || _displayRef.itemType < 0))
		{
			_displayRef = null;
		}
		_npc = familiarData.npc;
		_powerMult = familiarData.powerMult;
		_staminaMult = familiarData.staminaMult;
		_agilityMult = familiarData.agilityMult;
		_selectOffset = Util.GetVector2FromStringProperty(familiarData.selectOffset);
		_selectScale = familiarData.selectScale;
		_intro = Util.GetBoolFromStringProperty(familiarData.intro, defaultValue: true);
		_obtainable = Util.GetBoolFromStringProperty(familiarData.obtainable, defaultValue: true);
		LoadDetails(familiarData);
	}

	private NPCRef getNPCRef()
	{
		NPCRef nPCRef = _npcRef;
		if (nPCRef == null)
		{
			if (_npc == null)
			{
				return null;
			}
			nPCRef = NPCBook.LookupLink(_npc);
		}
		return nPCRef;
	}

	public bool isFusion()
	{
		if (_fusionResults != null)
		{
			return _fusionResults.Count > 0;
		}
		return false;
	}

	public void addFusionResult(FusionRef fusionRef)
	{
		if (_fusionResults == null)
		{
			_fusionResults = new List<FusionRef>();
		}
		_fusionResults.Add(fusionRef);
	}

	public bool getFusionVisible()
	{
		FusionRef resultFusion = FusionBook.GetResultFusion(this);
		if (resultFusion == null)
		{
			return true;
		}
		return GameData.instance.PROJECT.character.inventory.hasOwnedItem(resultFusion);
	}

	public bool isFusionFamiliar()
	{
		for (int i = 0; i < FusionBook.fusions.Count; i++)
		{
			FusionRef fusionRef = FusionBook.Lookup(i);
			if (!(fusionRef == null) && fusionRef.tradeRef.resultItem.itemRef == this)
			{
				return true;
			}
		}
		return false;
	}

	public bool allowAugmentSlot(int slot)
	{
		foreach (int disabledAugmentSlot in _disabledAugmentSlots)
		{
			if (disabledAugmentSlot == slot)
			{
				return false;
			}
		}
		return true;
	}

	public bool Equals(FamiliarRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(FamiliarRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}

	private float getPowerStableGain(FamiliarStable stable, int adjust = 0)
	{
		return _powerMult / getStatMultTotal * stable.getFamiliarMult(this, adjust);
	}

	private float getStaminaStableGain(FamiliarStable stable, int adjust = 0)
	{
		return _staminaMult / getStatMultTotal * stable.getFamiliarMult(this, adjust);
	}

	private float getAgilityStableGain(FamiliarStable stable, int adjust = 0)
	{
		return _agilityMult / getStatMultTotal * stable.getFamiliarMult(this, adjust);
	}

	public int getPower(int total, int adjust = 0)
	{
		return (int)((float)total * (_powerMult + getPowerStableGain(GameData.instance.PROJECT.character.familiarStable, adjust)));
	}

	public int getStamina(int total, int adjust = 0)
	{
		return (int)((float)total * (_staminaMult + getStaminaStableGain(GameData.instance.PROJECT.character.familiarStable, adjust)));
	}

	public int getAgility(int total, int adjust = 0)
	{
		return (int)((float)total * (_agilityMult + getAgilityStableGain(GameData.instance.PROJECT.character.familiarStable, adjust)));
	}
}
