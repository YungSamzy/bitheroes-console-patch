using System;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.game;

public class GameTextRef
{
	private string _content;

	private int _size;

	private string _color;

	private int _width;

	private string _align;

	private string _autoSize;

	private bool _multiline;

	private bool _center;

	private Vector2 _position;

	private float _rotation;

	private DateTime? _date;

	public string content => _content;

	public string autoSize => _autoSize;

	public int size => _size;

	public string color => _color;

	public int width => _width;

	public string align => _align;

	public bool multiline => _multiline;

	public Vector2 position => _position;

	public DateTime? date => _date;

	public GameTextRef(ShopBookData.Text textData)
	{
		_content = Language.GetString(textData.content);
		_size = textData.size;
		_color = ((textData.color != null) ? textData.color : "FFFFFF");
		_width = ((textData.width != null) ? int.Parse(textData.width) : 100);
		_align = ((textData.align != null) ? textData.align : "center");
		_autoSize = ((textData.autoSize != null) ? textData.autoSize : "none");
		_multiline = Util.parseBoolean(textData.multiline, defaultVal: false);
		_center = Util.parseBoolean(textData.center, defaultVal: false);
		_position = ((textData.position != null) ? Util.pointFromString(textData.position) : default(Vector2));
		_rotation = ((textData.rotation != null) ? Util.ParseFloat(textData.rotation) : 0f);
		if (textData.date != null)
		{
			_date = Util.GetDateFromString(textData.date);
		}
		else
		{
			_date = null;
		}
	}
}
