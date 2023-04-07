using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.sound;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.ability;

[DebuggerDisplay("{name} (AbilityRef)")]
public class AbilityRef : BaseRef, IEquatable<AbilityRef>, IComparable<AbilityRef>
{
	public const string NAME_COLOR = "#FFFF00";

	private AbilityPositionRef _position;

	private List<AbilityActionRef> _actions;

	private float _meter;

	private float _perc;

	private int _select;

	private int _uses;

	private bool _selectDead;

	private bool _selectAlive;

	private bool _subanimate;

	private string _animation;

	private SoundRef _sound;

	public int meterCost
	{
		get
		{
			if (_meter <= 0f)
			{
				return 0;
			}
			if (_meter >= 2f)
			{
				return VariableBook.battleMeterMax * 2;
			}
			return Mathf.RoundToInt((float)VariableBook.battleMeterMax * _meter);
		}
	}

	public float perc => _perc;

	public int select => _select;

	public int uses => _uses;

	public bool selectDead => _selectDead;

	public bool selectAlive => _selectAlive;

	public bool subanimate => _subanimate;

	public string animation => _animation;

	public SoundRef sound => _sound;

	public AbilityPositionRef position => _position;

	public List<AbilityActionRef> actions => _actions;

	public AbilityRef(int id, AbilityBookData.Ability abilityData)
		: base(id)
	{
		_position = AbilityBook.LookupPositionLink(abilityData.position);
		_actions = new List<AbilityActionRef>();
		_meter = abilityData.meter;
		_perc = abilityData.perc;
		_select = AbilityTarget.getType(Util.GetStringFromStringProperty(abilityData.select));
		_uses = abilityData.uses;
		_selectDead = Util.GetBoolFromStringProperty(abilityData.selectDead);
		_selectAlive = Util.GetBoolFromStringProperty(abilityData.selectAlive, defaultValue: true);
		_subanimate = Util.GetBoolFromStringProperty(abilityData.subanimate);
		_animation = abilityData.animation;
		_sound = SoundBook.Lookup(abilityData.sound);
		for (int i = 0; i < abilityData.lstAction.Count; i++)
		{
			AbilityActionRef abilityActionRef = new AbilityActionRef(i, abilityData.lstAction[i]);
			_actions.Add(abilityActionRef);
			AbilityBook.AddActionRef(abilityActionRef);
		}
		LoadDetails(abilityData);
	}

	public string getTooltipText(int power, float bonus, bool showSP = false)
	{
		string text = "";
		string text2 = (showSP ? (" (^" + _meter * 4f + "^ " + Language.GetString("ability_meter_short_name") + ")") : "");
		if (name != null && name.Length > 0)
		{
			text = text + Util.colorString(name + ((!showSP) ? ":" : ""), "#FFFF00") + text2 + (showSP ? ":" : "") + "<br>";
		}
		return text + Util.ParseAbilityString(desc, this, power, bonus);
	}

	public static List<string> GetAbilityLinkFromString(string ability)
	{
		if (ability == null || ability.Trim().Equals(""))
		{
			return null;
		}
		List<string> list = new List<string>();
		list.AddRange(ability.Split(','));
		return list;
	}

	public override Sprite GetSpriteIcon()
	{
		return GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.ABILITY_ICON, icon);
	}

	public bool Equals(AbilityRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(AbilityRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
