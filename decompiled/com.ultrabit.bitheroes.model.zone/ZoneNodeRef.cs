using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml.zone;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.zone;

public class ZoneNodeRef : BaseRef
{
	private int _zoneID;

	private List<int> _requiredNodes;

	private List<int> _unlockNodes;

	private List<ZoneNodePathRef> _paths;

	private List<ZoneNodeDifficultyRef> _difficulties;

	private int _nodeID;

	private Vector2 _assetOffset;

	private Vector2 _position;

	private bool _hidden;

	private bool _repeatable;

	private bool _completeZone;

	private int _missionID;

	private int _requiredStars;

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

	public Vector2 assetOffset => _assetOffset;

	public Vector2 position => _position;

	public bool hidden => _hidden;

	public bool repeatable => _repeatable;

	public bool completeZone => _completeZone;

	public int missionID => _missionID;

	public int requiredStars => _requiredStars;

	public List<int> requiredNodes => _requiredNodes;

	public List<int> unlockNodes => _unlockNodes;

	public List<ZoneNodeDifficultyRef> difficulties => _difficulties;

	public ZoneNodeRef(int zoneID, int nodeID, ZoneXMLData.Node nodeData)
		: base(nodeID)
	{
		_zoneID = zoneID;
		_nodeID = nodeData.id;
		_assetOffset = Util.GetVector2FromStringProperty(nodeData.assetOffset);
		_position = Util.GetVector2FromStringProperty(nodeData.position);
		_hidden = Util.GetBoolFromStringProperty(nodeData.hidden);
		_repeatable = Util.GetBoolFromStringProperty(nodeData.repeatable);
		_completeZone = Util.GetBoolFromStringProperty(nodeData.completeZone);
		_missionID = Util.GetIntFromStringProperty(nodeData.missionID);
		_requiredStars = Util.GetIntFromStringProperty(nodeData.requiredStars);
		_requiredNodes = Util.StringToIntList(nodeData.requiredNodes);
		_unlockNodes = Util.StringToIntList(nodeData.unlockNodes);
		_paths = new List<ZoneNodePathRef>();
		if (nodeData.paths != null && nodeData.paths.lstPath != null && nodeData.paths.lstPath.Count > 0)
		{
			foreach (ZoneXMLData.Path item in nodeData.paths.lstPath)
			{
				_paths.Add(new ZoneNodePathRef(zoneID, nodeID, item));
			}
		}
		_difficulties = new List<ZoneNodeDifficultyRef>();
		if (nodeData.difficulties != null && nodeData.difficulties.lstDifficulty != null && nodeData.difficulties.lstDifficulty.Count > 0)
		{
			foreach (ZoneXMLData.Difficulty item2 in nodeData.difficulties.lstDifficulty)
			{
				_difficulties.Add(new ZoneNodeDifficultyRef(zoneID, nodeID, item2));
			}
		}
		LoadDetails(nodeData);
	}

	public int getDifficultyCount()
	{
		int num = 0;
		foreach (ZoneNodeDifficultyRef difficulty in _difficulties)
		{
			if (difficulty != null)
			{
				num++;
			}
		}
		return num;
	}

	public int getStarCount()
	{
		int num = 0;
		foreach (ZoneNodeDifficultyRef difficulty in _difficulties)
		{
			if (difficulty != null && difficulty.getNodeRef().repeatable)
			{
				num++;
			}
		}
		return num;
	}

	public ZoneNodeDifficultyRef getDifficultyRef(int difficulty)
	{
		if (difficulty < 0 || difficulty >= _difficulties.Count)
		{
			return null;
		}
		return _difficulties[difficulty];
	}

	public ZoneNodeDifficultyRef getFirstDifficultyRef()
	{
		return _difficulties[0];
	}

	public ZoneNodeDifficultyRef getLastDifficultyRef()
	{
		return _difficulties[_difficulties.Count - 1];
	}

	public bool getActive()
	{
		return true;
	}

	public void clearPaths()
	{
		_paths = null;
	}

	public Asset getAsset(bool center = false, float scale = 1f)
	{
		return null;
	}

	public ZoneRef getZoneRef()
	{
		return ZoneBook.Lookup(_zoneID);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
