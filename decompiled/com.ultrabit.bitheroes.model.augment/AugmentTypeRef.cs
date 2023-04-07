using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.augment;

[DebuggerDisplay("{name} (AugmentTypeRef)")]
public class AugmentTypeRef : BaseRef, IEquatable<AugmentTypeRef>, IComparable<AugmentTypeRef>
{
	public const int AUGMENT_TYPE_PUMP = 0;

	public const int AUGMENT_TYPE_LINING = 1;

	public const int AUGMENT_TYPE_STIMULATOR = 2;

	public const int AUGMENT_TYPE_CHIP = 3;

	public const int AUGMENT_TYPE_COUNT = 4;

	private Dictionary<int, AugmentModifierRef> _modifiers;

	public List<AugmentModifierRef> modifiers => new List<AugmentModifierRef>(_modifiers.Values);

	private new string name => GetName(base.id);

	public AugmentTypeRef(int id, AugmentBookData.Type typeData)
		: base(id)
	{
		_modifiers = new Dictionary<int, AugmentModifierRef>();
		foreach (AugmentBookData.Modifier lstModifier in typeData.lstModifiers)
		{
			_modifiers.Add(lstModifier.id, new AugmentModifierRef(lstModifier.id, lstModifier));
		}
		LoadDetails(typeData);
	}

	public AugmentModifierRef getModifier(int id)
	{
		if (_modifiers.ContainsKey(id))
		{
			return _modifiers[id];
		}
		return null;
	}

	public List<AugmentModifierRef> getRarityModifiers(int rarity)
	{
		List<AugmentModifierRef> list = new List<AugmentModifierRef>();
		foreach (KeyValuePair<int, AugmentModifierRef> modifier in _modifiers)
		{
			if (modifier.Value != null && modifier.Value.rarityRef.id == rarity)
			{
				list.Add(modifier.Value);
			}
		}
		return list;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public static string GetName(int id)
	{
		return id switch
		{
			0 => Language.GetString("ui_augment_a"), 
			1 => Language.GetString("ui_augment_b"), 
			2 => Language.GetString("ui_augment_c"), 
			3 => Language.GetString("ui_augment_d"), 
			_ => "AUGMENT_TYPE_" + id, 
		};
	}

	public bool Equals(AugmentTypeRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(AugmentTypeRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
