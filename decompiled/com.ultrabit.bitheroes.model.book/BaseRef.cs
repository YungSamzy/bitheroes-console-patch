using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.book;

[DebuggerDisplay("{name} (BaseRef)")]
public abstract class BaseRef : IEquatable<BaseRef>, IComparable<BaseRef>
{
	protected int _id;

	protected string _link;

	protected string _rawName;

	protected string _name;

	protected string _rawDesc;

	protected string _desc;

	protected string _rawBox;

	protected string _box;

	protected string _thumbnail;

	protected string _icon;

	protected bool _loadLocal;

	protected string _statLabel;

	public int id => _id;

	public virtual string name
	{
		get
		{
			if (_name == null)
			{
				_name = ((_rawName != null) ? Language.GetString(_rawName) : "");
				if (_name.Equals("") || _name == null)
				{
					_name = _rawName;
				}
			}
			return _name;
		}
	}

	public virtual string desc
	{
		get
		{
			if (_desc == null)
			{
				_desc = ((_rawDesc != null) ? Language.GetString(_rawDesc) : "");
			}
			return _desc;
		}
	}

	public string thumbnail => _thumbnail;

	public virtual string icon => _icon;

	public bool loadLocal => _loadLocal;

	public string box
	{
		get
		{
			if (_box == null)
			{
				return "";
			}
			return Language.GetString(_box);
		}
	}

	public string rawBox => _rawBox;

	public string link => _link;

	public virtual string statName => _statLabel;

	public BaseRef(int id)
	{
		_id = id;
	}

	public virtual void LoadDetails(BaseBookItem baseBookItem)
	{
		createStatNameFromXml(baseBookItem);
		_link = ((baseBookItem.link != null) ? baseBookItem.link.ToLowerInvariant() : null);
		_rawName = ((baseBookItem.name != null) ? baseBookItem.name : null);
		_rawDesc = ((baseBookItem.desc != null) ? baseBookItem.desc : null);
		_rawBox = ((baseBookItem.box != null) ? baseBookItem.box : null);
		_box = baseBookItem.box;
		_thumbnail = baseBookItem.thumbnail;
		_icon = baseBookItem.icon;
		_loadLocal = baseBookItem.loadLocal;
	}

	public abstract Sprite GetSpriteIcon();

	private void createStatNameFromXml(BaseBookItem xml)
	{
		if (xml.statname != null)
		{
			_statLabel = xml.statname;
		}
		else if (xml.name != null)
		{
			_statLabel = xml.name;
		}
	}

	public bool Equals(BaseRef other)
	{
		if (other == null)
		{
			return false;
		}
		if (id.Equals(other.id))
		{
			return link.Equals(other.link);
		}
		return false;
	}

	public int CompareTo(BaseRef other)
	{
		if (other == null)
		{
			return -1;
		}
		int num = id.CompareTo(other.id);
		if (num == 0)
		{
			return link.CompareTo(other.link);
		}
		return num;
	}
}
