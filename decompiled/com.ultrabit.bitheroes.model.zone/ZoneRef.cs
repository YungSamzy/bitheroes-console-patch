using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml.zone;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.zone;

public class ZoneRef : BaseRef
{
	private List<int> _requiredZones;

	private List<ZoneNodeRef> _nodes;

	private List<ZoneNotification> _notifications;

	private List<string> _paymentsIDNBPZone;

	private bool _starter;

	private ZoneNodeRef _completeNode;

	private string _asset;

	public bool starter => _starter;

	public List<int> requiredZones => _requiredZones;

	public List<ZoneNodeRef> nodes => _nodes;

	public List<ZoneNotification> notifications => _notifications;

	public List<string> paymentsZoneID => _paymentsIDNBPZone;

	public string asset => _asset;

	public ZoneRef(int id, ZoneXMLData.Zone zoneData)
		: base(id)
	{
		_asset = ((zoneData.asset != null) ? zoneData.asset : "");
		_starter = Util.GetBoolFromStringProperty(zoneData.starter);
		_nodes = new List<ZoneNodeRef>();
		foreach (ZoneXMLData.Node lstNode in zoneData.nodes.lstNodes)
		{
			_nodes.Add(new ZoneNodeRef(id, lstNode.id, lstNode));
		}
		_requiredZones = Util.StringToIntList(zoneData.requiredZones);
		_notifications = new List<ZoneNotification>();
		_notifications.Add(new ZoneNotification(zoneData.notification));
		_paymentsIDNBPZone = new List<string>();
		if (zoneData.boosters != null)
		{
			foreach (ZoneXMLData.Payment item in zoneData.boosters.lstPayment)
			{
				_paymentsIDNBPZone.Add(item.id);
			}
		}
		LoadDetails(zoneData);
	}

	public int getDifficultyCount()
	{
		int num = 0;
		foreach (ZoneNodeRef node in _nodes)
		{
			if (node != null)
			{
				num += node.getDifficultyCount();
			}
		}
		return num;
	}

	public int getStarCount()
	{
		int num = 0;
		foreach (ZoneNodeRef node in _nodes)
		{
			if (node != null && node.repeatable)
			{
				num += node.getDifficultyCount();
			}
		}
		return num;
	}

	public ZoneNodeRef getCompleteNode()
	{
		if (_completeNode != null)
		{
			return _completeNode;
		}
		foreach (ZoneNodeRef node in _nodes)
		{
			if (node != null && node.completeZone)
			{
				_completeNode = node;
				return _completeNode;
			}
		}
		return null;
	}

	public List<ZoneNodeRef> getUnlockNodes(ZoneNodeRef nodeRef)
	{
		List<ZoneNodeRef> list = new List<ZoneNodeRef>();
		foreach (ZoneNodeRef node in _nodes)
		{
			if (node == null)
			{
				continue;
			}
			foreach (int unlockNode in node.unlockNodes)
			{
				ZoneNodeRef nodeRef2 = getNodeRef(unlockNode);
				if (nodeRef2 != null && nodeRef2.nodeID == nodeRef.nodeID)
				{
					list.Add(node);
				}
			}
		}
		return list;
	}

	public ZoneNodeRef getNodeRef(int id)
	{
		return _nodes.Find((ZoneNodeRef item) => item.id == id);
	}

	public List<ZoneNotification> getNotificationsByType(int type)
	{
		List<ZoneNotification> list = new List<ZoneNotification>();
		foreach (ZoneNotification notification in _notifications)
		{
			if (notification.type == type)
			{
				list.Add(notification);
			}
		}
		return list;
	}

	public ZoneNotification getNotificationByType(int type)
	{
		foreach (ZoneNotification notification in _notifications)
		{
			if (notification.type == type)
			{
				return notification;
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
}
