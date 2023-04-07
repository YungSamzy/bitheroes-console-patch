using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.equipment;

[DebuggerDisplay("{id} (EquipmentSetRef)")]
public class EquipmentSetRef : BaseRef, IEquatable<EquipmentSetRef>, IComparable<EquipmentSetRef>
{
	private RarityRef _rarityRef;

	private List<EquipmentRef> _equipment;

	private List<EquipmentSetBonusRef> _bonuses;

	private int _textSize;

	public string coloredName => _rarityRef.ConvertString(Language.GetString(name));

	public int textSize => _textSize;

	public RarityRef rarityRef => _rarityRef;

	public List<EquipmentRef> equipment => _equipment;

	public List<EquipmentSetBonusRef> bonuses => _bonuses;

	public EquipmentSetRef(int id, EquipmentBookData.Equipmentset equipmentSetData)
		: base(id)
	{
		_rarityRef = RarityBook.Lookup(equipmentSetData.rarity);
		_equipment = new List<EquipmentRef>();
		foreach (EquipmentBookData.Equipment item in equipmentSetData.lstEquipment)
		{
			EquipmentRef equipmentRef = EquipmentBook.Lookup(item.id);
			equipmentRef.SetEquipmentSet(this);
			_equipment.Add(equipmentRef);
		}
		_bonuses = new List<EquipmentSetBonusRef>();
		foreach (EquipmentBookData.Bonus lstBonu in equipmentSetData.lstBonus)
		{
			_bonuses.Add(new EquipmentSetBonusRef(lstBonu));
		}
		_textSize = equipmentSetData.textSize;
		base.LoadDetails(equipmentSetData);
	}

	public string GetEnabledColor(string text, bool equipped)
	{
		if (equipped)
		{
			return _rarityRef.ConvertString(text);
		}
		string replacement = "$1#999999$3";
		RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline;
		string text2 = new Regex("(color=)(.*?)(\\>)", options).Replace(text, replacement);
		return "<color=#999999>" + text2 + "</color>";
	}

	public static bool listHasSet(List<EquipmentSetRef> list, EquipmentSetRef setRef)
	{
		foreach (EquipmentSetRef item in list)
		{
			if (item == setRef)
			{
				return true;
			}
		}
		return false;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(EquipmentSetRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(EquipmentSetRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
