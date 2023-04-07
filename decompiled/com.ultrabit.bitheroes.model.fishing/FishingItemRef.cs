using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using DG.Tweening;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.fishing;

public class FishingItemRef : BaseRef
{
	private ItemRef _itemRef;

	private Vector2 _speed;

	private Vector2 _distance;

	private float _width;

	private float _time;

	private string _ease;

	private FishingBarRef _barRef;

	public ItemRef itemRef => _itemRef;

	public float time => _time;

	public FishingBarRef barRef => _barRef;

	public FishingItemRef(int id, FishingBookData.Item data)
		: base(id)
	{
		_itemRef = ItemBook.Lookup(data.id, data.type);
		_speed = Util.numberPointFromString(data.speed);
		_distance = Util.numberPointFromString(data.distance);
		_width = data.width;
		_time = data.time;
		_ease = data.ease;
		_barRef = FishingBook.LookupBar(data.bar);
		LoadDetails(data);
	}

	public float getRandomSpeed()
	{
		return Util.RandomNumber(_speed.x, _speed.y);
	}

	public float getRandomDistance()
	{
		return Util.RandomNumber(_distance.x, _distance.y);
	}

	public Ease GetEase()
	{
		return _ease switch
		{
			"back" => Ease.InOutBack, 
			"bounce" => Ease.InOutBounce, 
			"circular" => Ease.InOutCirc, 
			"cubic" => Ease.InOutCubic, 
			"elastic" => Ease.InOutElastic, 
			"exponential" => Ease.InOutExpo, 
			"linear" => Ease.Linear, 
			"quadratic" => Ease.InOutQuad, 
			"quartic" => Ease.InOutQuart, 
			"quintic" => Ease.InOutQuint, 
			"sine" => Ease.InOutSine, 
			"random" => Util.RandomEase(), 
			_ => Util.RandomEase(), 
		};
	}

	public float getWidth(List<GameModifier> modifiers)
	{
		float typeTotal = GameModifier.getTypeTotal(modifiers, 54);
		return _width * (1f + typeTotal);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
