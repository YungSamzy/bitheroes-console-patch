using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.promo;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.zone;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.raid;

[DebuggerDisplay("{name} (RaidRef)")]
public class RaidRef : BaseRef, IEquatable<RaidRef>, IComparable<RaidRef>
{
	private List<RaidDifficultyRef> _difficulties;

	private PromoRef _promoRef;

	private int _requiredZone;

	private string _textColor;

	private string _dialogEnter;

	public string coloredName => ConvertString(Language.GetString(name));

	public List<RaidDifficultyRef> difficulties => _difficulties;

	public PromoRef promoRef => _promoRef;

	public int requiredZone => _requiredZone;

	public RaidRef(int id, RaidBookData.Raid raidData)
		: base(id)
	{
		_difficulties = new List<RaidDifficultyRef>();
		foreach (RaidBookData.Difficulty item in raidData.difficulties.lstDifficulty)
		{
			_difficulties.Add(new RaidDifficultyRef(id, item));
		}
		_promoRef = new PromoRef(raidData.promo.id, raidData.promo);
		_requiredZone = raidData.requiredZone;
		_textColor = raidData.textColor;
		_dialogEnter = raidData.dialogEnter;
		LoadDetails(raidData);
	}

	public bool getUnlocked()
	{
		ZoneRef zoneRef = ZoneBook.Lookup(_requiredZone);
		if (zoneRef == null)
		{
			return true;
		}
		if (GameData.instance.PROJECT.character.zones.zoneIsCompleted(zoneRef))
		{
			return true;
		}
		return false;
	}

	public RaidDifficultyRef getDifficultyRef(int difficulty)
	{
		if (difficulty < 0 || difficulty >= _difficulties.Count)
		{
			return null;
		}
		return _difficulties[difficulty];
	}

	public RaidDifficultyRef getLastDifficultyRef()
	{
		return _difficulties[_difficulties.Count - 1];
	}

	public DialogRef getDialogEnter()
	{
		return DialogBook.Lookup(_dialogEnter);
	}

	public string ConvertString(string theString)
	{
		return "<color=#" + _textColor + ">" + theString + "</color>";
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(RaidRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(RaidRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
