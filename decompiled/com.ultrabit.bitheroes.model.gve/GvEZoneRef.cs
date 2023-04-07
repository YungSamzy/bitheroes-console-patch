using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.gve;

[DebuggerDisplay("{asset} (GvEZoneRef)")]
public class GvEZoneRef : BaseRef, IEquatable<GvEZoneRef>, IComparable<GvEZoneRef>
{
	private string _asset;

	private List<GvEZoneNodeRef> _nodes;

	public List<GvEZoneNodeRef> nodes => _nodes;

	public string asset => _asset;

	public GvEZoneRef(int id, BaseEventBookData.Zone zoneData)
		: base(id)
	{
		_asset = ((zoneData.asset != null) ? zoneData.asset : "");
		_nodes = new List<GvEZoneNodeRef>();
		if (zoneData.nodes.lstNode != null)
		{
			foreach (BaseEventBookData.Node item in zoneData.nodes.lstNode)
			{
				GvEZoneNodeRef gvEZoneNodeRef = new GvEZoneNodeRef(item.id, zoneData.id, item);
				gvEZoneNodeRef.LoadDetails(item);
				_nodes.Add(gvEZoneNodeRef);
			}
		}
		LoadDetails(zoneData);
	}

	public List<GvEZoneNodeRef> getUnlockNodes(GvEZoneNodeRef nodeRef)
	{
		List<GvEZoneNodeRef> list = new List<GvEZoneNodeRef>();
		foreach (GvEZoneNodeRef node in _nodes)
		{
			if (node == null)
			{
				continue;
			}
			foreach (int unlockNode in node.unlockNodes)
			{
				GvEZoneNodeRef nodeRef2 = getNodeRef(unlockNode);
				if (nodeRef2 != null && nodeRef2.nodeID == nodeRef.nodeID)
				{
					list.Add(node);
				}
			}
		}
		return list;
	}

	public GvEZoneNodeRef getNodeRef(int id)
	{
		foreach (GvEZoneNodeRef node in _nodes)
		{
			if (node.nodeID == id)
			{
				return node;
			}
		}
		return null;
	}

	public Asset getAsset(bool center = false, float scale = 1f)
	{
		return null;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(GvEZoneRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(GvEZoneRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
