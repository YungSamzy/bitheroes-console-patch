using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.dialog;

public class DialogFrameRef : BaseRef
{
	public const float DEFAULT_SCALE = 8f;

	public const int DEFAULT_OFFSET_X = 0;

	public const int DEFAULT_OFFSET_Y = 100;

	public const int POSITION_LEFT = 1;

	public const int POSITION_RIGHT = 2;

	public const int POSITION_CENTER = 3;

	private static Dictionary<string, int> POSITIONS = new Dictionary<string, int>
	{
		["left"] = 1,
		["right"] = 2,
		["center"] = 3
	};

	private int _position;

	private bool _portrait;

	private int _textOffset;

	private bool _textBG;

	private AssetDisplayRef _displayRef;

	private List<DialogFrameContentRef> _contents;

	public int position => _position;

	public bool portrait => _portrait;

	public int textOffset => _textOffset;

	public bool textBG => _textBG;

	public AssetDisplayRef displayRef => _displayRef;

	public List<DialogFrameContentRef> contents => _contents;

	public DialogFrameRef(int id, DialogBookData.Frame frameData)
		: base(id)
	{
		_position = getPosition(frameData.position);
		_portrait = Util.GetBoolFromStringProperty(frameData.portrait);
		_textOffset = frameData.textOffset;
		_textBG = Util.GetBoolFromStringProperty(frameData.textBG);
		if (frameData.asset != null)
		{
			_displayRef = DialogBook.GetAssetDisplayRefFromLink(frameData.asset);
		}
		if (frameData.lstContent == null)
		{
			return;
		}
		_contents = new List<DialogFrameContentRef>();
		foreach (DialogBookData.Content item2 in frameData.lstContent)
		{
			int count = _contents.Count;
			string text = Util.parseMultiLine(Language.GetString(item2.link));
			DialogFrameContentRef item = new DialogFrameContentRef(count, text);
			_contents.Add(item);
		}
	}

	public void loadAssets()
	{
		if (_displayRef != null)
		{
			_displayRef.loadAssets();
		}
	}

	public DialogFrameContentRef getContent(int index)
	{
		if (index < 0 || index >= _contents.Count)
		{
			return null;
		}
		return _contents[index];
	}

	public static int getPosition(string position)
	{
		return POSITIONS[position.ToLowerInvariant()];
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
