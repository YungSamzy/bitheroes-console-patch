using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.language;

[DebuggerDisplay("{name} (LanguageRef)")]
public class LanguageRef : BaseRef, IEquatable<LanguageRef>, IComparable<LanguageRef>
{
	private string _lang;

	public string lang => _lang;

	public LanguageRef(int id, LanguageBookData.Language languageData)
		: base(id)
	{
		_lang = languageData.lang;
		LoadDetails(languageData);
	}

	public bool Equals(LanguageRef other)
	{
		if (other == null)
		{
			return false;
		}
		return lang.Equals(other.lang);
	}

	public int CompareTo(LanguageRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
