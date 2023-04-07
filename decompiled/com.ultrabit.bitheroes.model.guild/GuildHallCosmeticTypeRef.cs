using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.guild;

[DebuggerDisplay("{displayName} (GuildHallCosmeticTypeRef)")]
public class GuildHallCosmeticTypeRef : BaseRef, IEquatable<GuildHallCosmeticTypeRef>, IComparable<GuildHallCosmeticTypeRef>
{
	private bool _autoSelect;

	private string _locationName;

	private string _displayName;

	public bool autoSelect => _autoSelect;

	public string locationName => _locationName;

	public string displayName => _displayName;

	public GuildHallCosmeticTypeRef(int id, GuildHallBookData.Type data)
		: base(id)
	{
		_link = data.link;
		_autoSelect = Util.GetBoolFromStringProperty(data.AutoSelect);
		_locationName = ((data.locationName != null) ? Language.GetString(data.locationName) : null);
		_displayName = ((data.displayName != null) ? Language.GetString(data.displayName) : Language.GetString(data.name));
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(GuildHallCosmeticTypeRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(GuildHallCosmeticTypeRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
