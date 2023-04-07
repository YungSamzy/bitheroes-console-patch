using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.gve;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.zone;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.gve;

public class GvEEventZoneWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI refreshTxt;

	public Button refreshBtn;

	public GameObject zoneBg;

	public Transform gveNodePrefab;

	public Transform gveZonePathPrefab;

	public GameObject questMap;

	private GvEEventRef _eventRef;

	private int _nodeID;

	private bool _checkingData;

	private List<GvEEventZoneNode> _nodes = new List<GvEEventZoneNode>();

	private List<GvEEventZoneNodePath> _paths = new List<GvEEventZoneNodePath>();

	private IEnumerator _refreshTimer;

	private int seconds = 10;

	private GvEEventZoneData _zoneData;

	private bool _loaded;

	private bool _bgLoaded;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(GvEEventRef eventRef, int nodeID = -1)
	{
		_eventRef = eventRef;
		_nodeID = nodeID;
		topperTxt.text = _eventRef.zoneRef.name;
		CreatePaths();
		CreateNodes();
		CreateZoneBg();
		CheckLoaded();
		DoEventZoneData();
		if (_nodeID >= 0)
		{
			GameData.instance.main.ShowLoading();
		}
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckDefaultNode();
	}

	public void CheckLoaded()
	{
		if (_loaded)
		{
			GameData.instance.main.HideLoading();
			return;
		}
		if (!_bgLoaded)
		{
			GameData.instance.main.ShowLoading();
			return;
		}
		foreach (GvEEventZoneNode node in _nodes)
		{
			if (!node.assetLoaded)
			{
				GameData.instance.main.ShowLoading();
				return;
			}
		}
		_loaded = true;
		GameData.instance.main.HideLoading();
	}

	private void DoEventZoneData()
	{
		if (!_checkingData)
		{
			RestartRefreshTimer();
			_checkingData = true;
			GvEDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(4), OnEventZoneData);
			GvEDALC.instance.doEventZoneData(_eventRef.id);
		}
	}

	private void OnEventZoneData(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		_checkingData = false;
		GvEDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(4), OnEventZoneData);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GvEEventZoneData zoneData = GvEEventZoneData.fromSFSObject(sfsob);
		SetZoneData(zoneData);
		CheckDefaultNode();
	}

	private void CheckDefaultNode()
	{
		if (base.scrollingIn || _zoneData == null)
		{
			return;
		}
		GameData.instance.main.HideLoading();
		if (_nodeID >= 0)
		{
			GvEEventZoneNode node = GetNode(_nodeID);
			if (node != null)
			{
				node.ShowNodeWindow();
			}
			_nodeID = -1;
		}
	}

	private GvEEventZoneNode GetNode(int nodeID)
	{
		foreach (GvEEventZoneNode node in _nodes)
		{
			if (!(node == null) && node.zoneNodeRef.nodeID == nodeID)
			{
				return node;
			}
		}
		return null;
	}

	public void SetZoneData(GvEEventZoneData zoneData)
	{
		_zoneData = zoneData;
		foreach (GvEEventZoneNode node in _nodes)
		{
			node.SetZoneData(zoneData);
		}
		foreach (GvEEventZoneNodePath path in _paths)
		{
			path.SetZoneData(zoneData);
		}
	}

	private void CreateNodes()
	{
		foreach (GvEEventZoneNode node in _nodes)
		{
			Object.Destroy(node.gameObject);
		}
		_nodes.Clear();
		foreach (GvEZoneNodeRef node2 in _eventRef.zoneRef.nodes)
		{
			if (node2 != null)
			{
				Transform transform = Object.Instantiate(gveNodePrefab);
				transform.name = node2.name + " (" + node2.GetType().Name + ")";
				transform.SetParent(questMap.transform, worldPositionStays: false);
				transform.GetComponent<GvEEventZoneNode>().LoadDetails(_eventRef, node2, this);
				transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(node2.position.x / panel.transform.localScale.x, node2.position.y / (0f - panel.transform.localScale.y));
				_nodes.Add(transform.GetComponent<GvEEventZoneNode>());
			}
		}
	}

	private void CreatePaths()
	{
		foreach (GvEEventZoneNodePath path in _paths)
		{
			Object.Destroy(path.gameObject);
		}
		_paths.Clear();
		foreach (GvEZoneNodeRef node in _eventRef.zoneRef.nodes)
		{
			if (node == null)
			{
				continue;
			}
			foreach (ZoneNodePathRef path2 in node.paths)
			{
				if (path2 != null)
				{
					Transform obj = Object.Instantiate(gveZonePathPrefab);
					obj.SetParent(questMap.transform, worldPositionStays: false);
					GvEEventZoneNodePath component = obj.GetComponent<GvEEventZoneNodePath>();
					component.LoadDetails(node, path2, this);
					_paths.Add(component);
				}
			}
		}
	}

	private void CreateZoneBg()
	{
		zoneBg.GetComponent<Image>().overrideSprite = GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.ZONE, _eventRef.zoneRef.asset);
	}

	private void OnBgAssetLoaded()
	{
		_bgLoaded = true;
		CheckLoaded();
	}

	private void RestartRefreshTimer()
	{
		if (_refreshTimer != null)
		{
			StopCoroutine(_refreshTimer);
			_refreshTimer = null;
		}
		seconds = 10;
		_refreshTimer = OnRefreshTimer();
		StartCoroutine(_refreshTimer);
		Util.SetButton(refreshBtn, enabled: false);
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = Util.NumberFormat(seconds);
	}

	private IEnumerator OnRefreshTimer()
	{
		yield return new WaitForSeconds(1f);
		seconds--;
		if (seconds <= 0)
		{
			Util.SetButton(refreshBtn);
			refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = "";
			StopCoroutine(_refreshTimer);
			_refreshTimer = null;
		}
		else
		{
			refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = Util.NumberFormat(seconds);
			_refreshTimer = OnRefreshTimer();
			StartCoroutine(_refreshTimer);
		}
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoEventZoneData();
	}

	public override void DoDestroy()
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		refreshBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		refreshBtn.interactable = false;
	}
}
