using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.date;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.xml.common;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.promo;

[DebuggerDisplay("{id} (PromoRef)")]
public class PromoRef : IEquatable<PromoRef>, IComparable<PromoRef>
{
	protected int _id;

	protected string _asset;

	protected List<PromoObjectRef> _objects;

	protected List<GameTextRef> _texts;

	private bool _loadLocal;

	private DateRef _dateRef;

	protected ShopBookData.Promo promoData;

	public int id => _id;

	public List<PromoObjectRef> objects => _objects;

	public List<GameTextRef> texts => _texts;

	public string url => AssetURL.PROMO + promoData.asset;

	public PromoRef(int id, ShopBookData.Promo promoData)
	{
		_id = id;
		this.promoData = promoData;
		_objects = new List<PromoObjectRef>();
		foreach (AssetDisplayData item in promoData.lstObject)
		{
			_objects.Add(new PromoObjectRef(item));
		}
		_texts = new List<GameTextRef>();
		foreach (ShopBookData.Text item2 in promoData.lstText)
		{
			_texts.Add(new GameTextRef(item2));
		}
		if (promoData.startDate != null && promoData.endDate != null)
		{
			_dateRef = new DateRef(promoData.startDate, promoData.endDate);
		}
		_asset = promoData.asset;
	}

	public Transform getAsset(bool center = false, Transform parent = null)
	{
		Transform transform = null;
		transform = GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.PROMO, _asset);
		if (transform == null)
		{
			return null;
		}
		if (parent != null)
		{
			transform.transform.SetParent(parent, worldPositionStays: false);
		}
		if (center)
		{
			if (parent != null)
			{
				transform.transform.localPosition = Vector3.zero;
			}
			else
			{
				transform.transform.position = Vector3.zero;
			}
		}
		return transform;
	}

	public bool getActive(DateTime? customDate = null)
	{
		if (_dateRef != null)
		{
			return _dateRef.getActive(customDate);
		}
		return true;
	}

	public bool Equals(PromoRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(PromoRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
