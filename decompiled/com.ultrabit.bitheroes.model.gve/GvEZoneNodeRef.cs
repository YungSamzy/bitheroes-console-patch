using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.gve;

[DebuggerDisplay("{name} (GvEZoneNodeRef)")]
public class GvEZoneNodeRef : BaseRef
{
	private int _zoneID;

	private int _nodeID;

	private int _points;

	private string _asset;

	private Vector2 _assetOffset;

	private Vector2 _position;

	private bool _hidden;

	private List<int> _requiredNodes;

	private List<int> _unlockNodes;

	private List<ZoneNodePathRef> _paths;

	private List<ItemData> _rewards;

	public List<ZoneNodePathRef> paths
	{
		get
		{
			_ = _paths;
			return _paths;
		}
	}

	public int zoneID => _zoneID;

	public int nodeID => _nodeID;

	public int points => _points;

	public string asset => _asset;

	public Vector2 assetOffset => _assetOffset;

	public Vector2 position => _position;

	public bool hidden => _hidden;

	public List<int> requiredNodes => _requiredNodes;

	public List<int> unlockNodes => _unlockNodes;

	public List<ItemData> rewards => _rewards;

	public GvEZoneNodeRef(int id, int zoneID, BaseEventBookData.Node nodeData)
		: base(id)
	{
		_zoneID = zoneID;
		_nodeID = nodeData.id;
		_points = nodeData.points;
		_asset = ((nodeData.asset != null) ? nodeData.asset : "");
		_assetOffset = ((nodeData.assetOffset != null) ? Util.pointFromString(nodeData.assetOffset) : new Vector2(0f, 0f));
		_position = Util.pointFromString(nodeData.position);
		_hidden = Util.GetBoolFromStringProperty(nodeData.hidden);
		_requiredNodes = ((nodeData.requiredNodes != null) ? Util.GetIntListFromStringProperty(nodeData.requiredNodes) : new List<int>());
		_unlockNodes = ((nodeData.unlockNodes != null) ? Util.GetIntListFromStringProperty(nodeData.unlockNodes) : new List<int>());
		if (nodeData.zoneRewards.lstItem != null)
		{
			_rewards = new List<ItemData>();
			foreach (BaseEventBookData.Item item in nodeData.zoneRewards.lstItem)
			{
				ItemRef itemRef = ItemBook.Lookup(item.id, item.type);
				_rewards.Add(new ItemData(itemRef, item.qty));
			}
		}
		if (nodeData.paths.lstPath == null)
		{
			return;
		}
		_paths = new List<ZoneNodePathRef>();
		foreach (BaseEventBookData.Path item2 in nodeData.paths.lstPath)
		{
			_paths.Add(new ZoneNodePathRef(_zoneID, _nodeID, item2));
		}
	}

	public void clearPaths()
	{
		_paths = null;
	}

	public Asset getAsset(bool center = false, float scale = 1f)
	{
		return null;
	}

	public GvEZoneRef GetZoneRef()
	{
		return GvEEventBook.LookupZone(_zoneID);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
