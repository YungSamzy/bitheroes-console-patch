using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.ui.rune;

namespace com.ultrabit.bitheroes.model.rune;

[DebuggerDisplay("{name} (RuneRef)")]
public class RuneRef : ItemRef, IEquatable<RuneRef>, IComparable<RuneRef>
{
	public const int RUNE_TYPE_MAJOR = 1;

	public const int RUNE_TYPE_MINOR = 2;

	public const int RUNE_TYPE_META = 3;

	public const int RUNE_TYPE_RELIC = 4;

	public const int RUNE_TYPE_ARTIFACT = 5;

	public const int RUNE_TYPE_COUNT = 5;

	public static Dictionary<string, int> RUNE_TYPES = new Dictionary<string, int>
	{
		["major"] = 1,
		["minor"] = 2,
		["meta"] = 3,
		["relic"] = 4,
		["artifact"] = 5
	};

	private List<AbilityRef> _abilities;

	private List<GameModifier> _modifiers;

	private int _runeType;

	private List<string> _values;

	public int runeType => _runeType;

	public List<string> values => _values;

	public List<AbilityRef> abilities
	{
		get
		{
			if (!(base.assetsSource != null))
			{
				return _abilities;
			}
			return (base.assetsSource as RuneRef).abilities;
		}
	}

	public List<GameModifier> modifiers
	{
		get
		{
			if (!(base.assetsSource != null))
			{
				return _modifiers;
			}
			return (base.assetsSource as RuneRef).modifiers;
		}
	}

	public int runeAction { get; set; }

	public RuneTile runeTile { get; set; }

	public ArmoryRuneTile amoryRuneTile { get; set; }

	public RuneRef(int id, RuneBookData.Rune runeData)
		: base(id, 9)
	{
		runeAction = 0;
		_modifiers = new List<GameModifier>();
		_abilities = new List<AbilityRef>();
		if (runeData.abilities != null && runeData.abilities != "")
		{
			_abilities = AbilityBook.LookupAbilities(runeData.abilities);
		}
		if (runeData.modifier != null)
		{
			_modifiers.Add(new GameModifier(runeData.modifier));
		}
		_runeType = getRuneType(runeData.type);
		_values = ((runeData.values != null && !runeData.values.Trim().Equals("")) ? new List<string>(Util.GetStringArrayFromStringProperty(runeData.values)) : new List<string>());
		LoadDetails(runeData);
	}

	public static int getRuneType(string type)
	{
		return RUNE_TYPES[type.ToLowerInvariant()];
	}

	public static string getRuneTypeName(int type)
	{
		return Language.GetString("rune_type_" + type + "_name");
	}

	public bool Equals(RuneRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(RuneRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
