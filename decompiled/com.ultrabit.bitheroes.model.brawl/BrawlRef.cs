using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.music;
using com.ultrabit.bitheroes.model.promo;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.brawl;

[DebuggerDisplay("{name} (BrawlRef)")]
public class BrawlRef : BaseRef, IEquatable<BrawlRef>, IComparable<BrawlRef>
{
	private string _dialogEnter;

	private string _textColor;

	private List<string> _battleBGs;

	private MusicRef _battleMusic;

	private int _slots;

	private List<BrawlTierRef> _tiers;

	private PromoRef _promoRef;

	public string coloredName => convertString(name);

	public MusicRef battleMusic => _battleMusic;

	public int slots => _slots;

	public List<BrawlTierRef> tiers => _tiers;

	public PromoRef promoRef => _promoRef;

	public string battleBGURL
	{
		get
		{
			string text = _battleBGs[Util.randomInt(0, _battleBGs.Count - 1)];
			if (text == null)
			{
				return null;
			}
			return text;
		}
	}

	public BrawlRef(int id, BrawlBookData.Brawl brawlData)
		: base(id)
	{
		_dialogEnter = brawlData.dialogEnter;
		_textColor = brawlData.textColor;
		_battleBGs = Util.GetStringListFromStringProperty(brawlData.battleBGs);
		_battleMusic = MusicBook.Lookup((brawlData.battleMusic != null) ? brawlData.battleMusic : "boss");
		_slots = brawlData.slots;
		_tiers = new List<BrawlTierRef>();
		for (int i = 0; i < brawlData.lstTier.Count; i++)
		{
			_tiers.Add(new BrawlTierRef(brawlData.lstTier[i].id, brawlData.lstTier[i]));
		}
		_promoRef = new PromoRef(brawlData.Promo.id, brawlData.Promo);
		foreach (BrawlTierRef tier in _tiers)
		{
			tier.setBrawlRef(this);
		}
	}

	public BrawlTierRef getFirstTier()
	{
		foreach (BrawlTierRef tier in _tiers)
		{
			if (tier != null)
			{
				return tier;
			}
		}
		return null;
	}

	public BrawlTierRef getTier(int id)
	{
		foreach (BrawlTierRef tier in _tiers)
		{
			if (tier.id == id)
			{
				return tier;
			}
		}
		return null;
	}

	public DialogRef getDialogEnter()
	{
		return DialogBook.Lookup(_dialogEnter);
	}

	public bool requirementsMet()
	{
		foreach (BrawlTierRef tier in _tiers)
		{
			if (tier.requirementsMet())
			{
				return true;
			}
		}
		return false;
	}

	public string convertString(string theString)
	{
		return "<color=#" + _textColor + ">" + theString + "</color>";
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(BrawlRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(BrawlRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
