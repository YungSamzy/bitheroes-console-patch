using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml.common;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.promo;

public class PromoObjectRef
{
	private AssetDisplayRef _displayRef;

	private Vector2 _position;

	private AssetDisplayData objectData;

	public AssetDisplayRef displayRef => _displayRef;

	public Vector2 position => _position;

	public bool hasAnimation
	{
		get
		{
			if (objectData.animation != null)
			{
				return !objectData.animation.Equals("");
			}
			return false;
		}
	}

	public string animation => objectData.animation;

	public PromoObjectRef(AssetDisplayData objectData)
	{
		this.objectData = objectData;
		_displayRef = new AssetDisplayRef(objectData);
		_position = Util.pointFromString(objectData.position);
	}
}
