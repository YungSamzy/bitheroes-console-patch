using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.grid;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.dungeon;

public class DungeonNode : MonoBehaviour
{
	private const float ALPHA_HIDDEN = 1f;

	private const float ALPHA_SEEN = 0.5f;

	private const float ALPHA_VISIBLE = 0f;

	private const float Z_POS_INVISIBLE = -150000f;

	private const string OVERLAY_NAME = "overlay";

	private const string PROP_NAME = "prop";

	private const string FOG_NAME = "NodeFog";

	private const string FOG_NAME_PARTIAL = "NodeFogPartial";

	private int _row;

	private int _column;

	private bool _up;

	private bool _down;

	private bool _left;

	private bool _right;

	private Dungeon _dungeon;

	private Vector2 _point;

	private bool _empty;

	private DungeonObject _object;

	private GameObject _asset;

	private int _points;

	private bool _touched;

	private bool _seen;

	private bool _vis;

	private List<float> _fogAlphas = new List<float>();

	private GameObject _fog;

	private bool _changed;

	private List<DungeonNode> _connectedNodes;

	private List<DungeonNode> _nearbyNodes;

	private List<Tile> _tiles = new List<Tile>();

	private List<GameObject> _overlays;

	private List<GameObject> _connectors = new List<GameObject>();

	private Sprite nodeFog;

	private Sprite nodeFogPartial;

	public int row => _row;

	public int column => _column;

	public bool up => _up;

	public bool down => _down;

	public bool left => _left;

	public bool right => _right;

	public bool empty => _empty;

	public int points => _points;

	public bool seen => _seen;

	public bool touched => _touched;

	public bool vis => _vis;

	public DungeonObject obj => _object;

	public bool changed => _changed;

	public GameObject asset => _asset;

	public bool forced
	{
		get
		{
			if (_object != null)
			{
				return _object.objectRef == null;
			}
			return false;
		}
	}

	public List<DungeonNode> connectedNodes => _connectedNodes;

	public GameObject fog => _fog;

	public List<GameObject> connectors => _connectors;

	public DungeonObject dungeonObject => _object;

	public float x
	{
		get
		{
			return base.transform.position.x;
		}
		set
		{
			base.transform.position = new Vector3(value, base.transform.position.y, base.transform.position.z);
		}
	}

	public float y
	{
		get
		{
			return base.transform.position.y;
		}
		set
		{
			base.transform.position = new Vector3(base.transform.position.x, value, base.transform.position.z);
		}
	}

	public void LoadDetails(int row, int column, bool up, bool down, bool left, bool right, bool empty, bool seen)
	{
		_row = row;
		_column = column;
		_up = up;
		_down = down;
		_left = left;
		_right = right;
		_empty = empty;
		_seen = seen;
		nodeFog = GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.DUNGEON_OVERLAY, "NodeFog");
		nodeFogPartial = GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.DUNGEON_OVERLAY, "NodeFogPartial");
		_fog = new GameObject();
		_fog.name = "FOG- + R" + row + "-C" + column;
		_fog.transform.SetParent(base.transform);
		_fog.transform.localPosition = Vector3.zero;
		_fog.transform.localScale = new Vector3(2f, 2f, 1f);
		_fog.AddComponent<SpriteRenderer>().sprite = nodeFog;
		UpdatePoints();
	}

	public void SetDungeon(Dungeon dungeon)
	{
		if (!(_dungeon != null))
		{
			_dungeon = dungeon;
			SetVisible(v: false);
		}
	}

	public void UpdateNode()
	{
		_point = new Vector2(x, y);
		_fog.transform.position = new Vector3(_point.x, _point.y, _fog.transform.position.z);
		SetVisible(v: false);
	}

	public Tile GetTile()
	{
		if (_dungeon == null)
		{
			return null;
		}
		return _dungeon.getTileFromPoint(_point.x, _point.y);
	}

	public List<Tile> GetCenterTiles()
	{
		if (_dungeon == null)
		{
			return null;
		}
		new List<Tile>();
		Tile tile = GetTile();
		return _dungeon.GetNearbyTiles(tile);
	}

	public Tile GetRandomCenterTile(Tile exclude = null)
	{
		List<Tile> centerTiles = GetCenterTiles();
		if (exclude != null)
		{
			for (int i = 0; i < centerTiles.Count; i++)
			{
				if (centerTiles[i].id == exclude.id)
				{
					centerTiles.RemoveAt(i);
					break;
				}
			}
		}
		return centerTiles[Util.randomInt(0, centerTiles.Count - 1)];
	}

	public Tile GetRandomWalkableTile()
	{
		List<Tile> walkableTiles = GetWalkableTiles();
		if (walkableTiles.Count <= 0)
		{
			return null;
		}
		return walkableTiles[Util.randomInt(0, walkableTiles.Count - 1)];
	}

	public List<Tile> GetWalkableTiles()
	{
		List<Tile> list = new List<Tile>();
		foreach (Tile tile in _tiles)
		{
			if (tile.walkable)
			{
				list.Add(tile);
			}
		}
		return list;
	}

	public void AddTile(Tile tile)
	{
		foreach (Tile tile2 in _tiles)
		{
			if (tile2 == tile)
			{
				return;
			}
		}
		_tiles.Add(tile);
	}

	public void SetObject(DungeonObject obj, bool destroy = true)
	{
		if (_object != null)
		{
			if (destroy)
			{
				_object.DoDestroy();
			}
			_object = null;
		}
		_object = obj;
		if (_object != null)
		{
			UpdateFog(draw: false);
		}
		UpdateObject();
	}

	public void DisableObject()
	{
		if ((bool)_object)
		{
			_object.SetClickable(clickable: false);
			_object.DisableObject();
			UpdateObject();
		}
	}

	public void UpdateObject()
	{
		if (_object != null)
		{
			_object.tile?.SetWalkable(!_object.collision);
		}
		_empty = _object == null || _object.disabled;
		if (!(_object == null))
		{
			if (_seen && (bool)_dungeon)
			{
				_dungeon.AddObject(_object);
			}
			UpdateOverlays();
		}
	}

	private void UpdateOverlays()
	{
		if (_overlays != null || asset == null)
		{
			return;
		}
		_overlays = new List<GameObject>();
		if (!(asset.GetComponentInChildren<FrameNavigator>() != null))
		{
			return;
		}
		FrameNavigator componentInChildren = asset.GetComponentInChildren<FrameNavigator>();
		foreach (GameObject item in Util.GetChildrenByName(componentInChildren.transform.GetChild(componentInChildren.currentFrame - 1).gameObject, "overlay"))
		{
			if (_empty)
			{
				SortingGroup sortingGroup = item.AddComponent<SortingGroup>();
				sortingGroup.sortingLayerName = "Overall";
				sortingGroup.sortingOrder = 0;
				item.transform.localPosition = Vector3.zero;
				item.transform.rotation = asset.transform.rotation;
				item.transform.localScale = Vector3.one;
				_overlays.Add(item);
			}
			else
			{
				item.SetActive(value: false);
			}
		}
	}

	public List<float> GetUpdateFogAlphas()
	{
		List<float> list = new List<float>();
		float num = (seen ? 0.5f : 1f);
		DungeonNode node = _dungeon.GetNode(_row, _column - 1);
		DungeonNode node2 = _dungeon.GetNode(_row, _column + 1);
		DungeonNode node3 = _dungeon.GetNode(_row - 1, _column);
		DungeonNode node4 = _dungeon.GetNode(_row + 1, _column);
		bool flag = AreSidesConnected(this, node);
		bool flag2 = AreSidesConnected(this, node2);
		bool flag3 = AreSidesConnected(this, node3);
		bool flag4 = AreSidesConnected(this, node4);
		float item = ((_vis || ((bool)node && node.vis && flag)) ? 0f : ((_seen || ((bool)node && node.seen && flag)) ? 0.5f : num));
		float item2 = ((_vis || ((bool)node2 && node2.vis && flag2)) ? 0f : ((_seen || ((bool)node2 && node2.seen && flag2)) ? 0.5f : num));
		float item3 = ((_vis || ((bool)node3 && node3.vis && flag3)) ? 0f : ((_seen || ((bool)node3 && node3.seen && flag3)) ? 0.5f : num));
		float item4 = ((_vis || ((bool)node4 && node4.vis && flag4)) ? 0f : ((_seen || ((bool)node4 && node4.seen && flag4)) ? 0.5f : num));
		list.Add(num);
		list.Add(item);
		list.Add(item2);
		list.Add(item3);
		list.Add(item4);
		return list;
	}

	public void UpdateChanged()
	{
		List<float> updateFogAlphas = GetUpdateFogAlphas();
		if (updateFogAlphas.Count != _fogAlphas.Count)
		{
			_changed = true;
			return;
		}
		for (int i = 0; i < updateFogAlphas.Count; i++)
		{
			float num = _fogAlphas[i];
			float num2 = updateFogAlphas[i];
			if (num != num2)
			{
				_changed = true;
				return;
			}
		}
		_changed = false;
	}

	public void UpdateFogAlphas()
	{
		_fogAlphas = GetUpdateFogAlphas();
	}

	public void UpdateFogParent()
	{
	}

	public void UpdateFog(bool draw = true, bool connected = false, List<DungeonNode> playerNodeConnections = null)
	{
		if (_dungeon == null)
		{
			return;
		}
		if (draw)
		{
			if (_object != null && _object.asset != null)
			{
				Vector3 localPosition = _object.asset.transform.localPosition;
				Vector3 localPosition2 = ((!_seen) ? new Vector3(localPosition.x, localPosition.y, -150000f) : new Vector3(localPosition.x, localPosition.y, 0f));
				_object.asset.transform.localPosition = localPosition2;
			}
			float a = (connected ? 0f : (_seen ? 0.5f : 1f));
			SpriteRenderer component = _fog.GetComponent<SpriteRenderer>();
			ColorUtility.TryParseHtmlString("#" + _dungeon.dungeonRef.color, out var color);
			color = new Color(color.r, color.g, color.b, a);
			component.color = color;
			bool flag = false;
			float z = 0f;
			foreach (DungeonNode playerNodeConnection in playerNodeConnections)
			{
				if (NodeIsConnected(playerNodeConnection))
				{
					flag = true;
					z = GetFogRotation(playerNodeConnection);
					break;
				}
			}
			component.sprite = (flag ? nodeFogPartial : nodeFog);
			_fog.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, z));
			component.sortingLayerName = "Default";
			component.sortingOrder = -1;
			_changed = false;
		}
		UpdateFogParent();
	}

	private float GetFogRotation(DungeonNode target)
	{
		if (NodeSourceLeftToTarget(target, this))
		{
			return 90f;
		}
		if (NodeSourceRigthToTarget(target, this))
		{
			return -90f;
		}
		if (NodeSourceBelowToTarget(target, this))
		{
			return 180f;
		}
		return 0f;
	}

	public void SetVisible(bool v)
	{
		if (!_seen && v)
		{
			_seen = v;
		}
		_vis = v;
		_ = _seen;
		_ = _vis;
		UpdateFogParent();
		UpdateObject();
	}

	public void SetTouched(bool v)
	{
		_touched = touched;
	}

	public Rect GetPanelBounds()
	{
		Rect result = new Rect(x, y, 280f, 280f);
		result.xMin = x - 140f;
		result.xMax = x + 140f;
		result.yMin = y - 140f;
		result.yMax = y + 140f;
		return result;
	}

	public void CheckConnectedNodes(bool assets = false)
	{
		if (_connectors.Count > 0)
		{
			return;
		}
		_connectedNodes = new List<DungeonNode>();
		_nearbyNodes = new List<DungeonNode>();
		DungeonNode node = _dungeon.GetNode(_row, _column - 1);
		DungeonNode node2 = _dungeon.GetNode(_row, _column + 1);
		DungeonNode node3 = _dungeon.GetNode(_row - 1, _column);
		DungeonNode node4 = _dungeon.GetNode(_row + 1, _column);
		if ((bool)node)
		{
			_nearbyNodes.Add(node);
		}
		if ((bool)node2)
		{
			_nearbyNodes.Add(node2);
		}
		if ((bool)node3)
		{
			_nearbyNodes.Add(node3);
		}
		if ((bool)node4)
		{
			_nearbyNodes.Add(node4);
		}
		bool num = AreSidesConnected(this, node);
		bool flag = AreSidesConnected(this, node2);
		bool flag2 = AreSidesConnected(this, node3);
		bool flag3 = AreSidesConnected(this, node4);
		if (num)
		{
			_connectedNodes.Add(node);
		}
		if (flag)
		{
			_connectedNodes.Add(node2);
		}
		if (flag2)
		{
			_connectedNodes.Add(node3);
		}
		if (flag3)
		{
			_connectedNodes.Add(node4);
		}
		DungeonNode node5 = _dungeon.GetNode(row - 1, column - 1);
		if (AreCornersConnected(node5, _dungeon.GetNode(row - 1, column), _dungeon.GetNode(row, column - 1), this))
		{
			_connectedNodes.Add(node5);
			if (assets)
			{
				CreateCorner(new Vector2(-140f, -140f), 0f);
			}
		}
		DungeonNode node6 = _dungeon.GetNode(row - 1, column + 1);
		if (AreCornersConnected(_dungeon.GetNode(row - 1, column), node6, this, _dungeon.GetNode(row, column + 1)))
		{
			_connectedNodes.Add(node6);
			if (assets)
			{
				CreateCorner(new Vector2(140f, -140f), -90f);
			}
		}
		DungeonNode node7 = _dungeon.GetNode(row + 1, column + 1);
		if (AreCornersConnected(this, _dungeon.GetNode(row, column + 1), _dungeon.GetNode(row + 1, column), node7))
		{
			_connectedNodes.Add(node7);
			if (assets)
			{
				CreateCorner(new Vector2(140f, 140f), 180f);
			}
		}
		DungeonNode node8 = _dungeon.GetNode(row + 1, column - 1);
		if (AreCornersConnected(_dungeon.GetNode(row, column - 1), this, node8, _dungeon.GetNode(row + 1, column)))
		{
			_connectedNodes.Add(node8);
			if (assets)
			{
				CreateCorner(new Vector2(-140f, 140f), 90f);
			}
		}
		if ((bool)node5)
		{
			_nearbyNodes.Add(node5);
		}
		if ((bool)node6)
		{
			_nearbyNodes.Add(node6);
		}
		if ((bool)node7)
		{
			_nearbyNodes.Add(node7);
		}
		if ((bool)node8)
		{
			_nearbyNodes.Add(node8);
		}
	}

	private void CreateCorner(Vector2 position, float rotation)
	{
		GameObject gameObject = new GameObject();
		GameObject nodeAsset = _dungeon.GetNodeAsset("Connector");
		nodeAsset.transform.SetParent(gameObject.transform, worldPositionStays: false);
		nodeAsset.transform.localPosition = new Vector3(-120f, 120f, 0f);
		nodeAsset.transform.localPosition += new Vector3(5f, -5f, 0f);
		nodeAsset.transform.localScale = new Vector3(2f, 2f, 1f);
		_connectors.Add(gameObject);
		gameObject.transform.SetParent(base.transform, worldPositionStays: false);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		SpriteRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].sortingOrder = -4998;
		}
		gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
		SetCornerAsset(nodeAsset);
	}

	private void OnCornerAssetLoaded()
	{
	}

	private void SetCornerAsset(GameObject asset)
	{
		if (asset.GetComponentInChildren<FrameNavigator>() != null)
		{
			FrameNavigator componentInChildren = asset.GetComponentInChildren<FrameNavigator>();
			componentInChildren.GoToAndStop(Util.randomInt(1, componentInChildren.totalFrames));
			Util.PlayChildren(componentInChildren.gameObject, _dungeon.dungeonRef.playRandom);
			Util.RotateChildrenByName(componentInChildren.gameObject, Mathf.RoundToInt(0f - asset.transform.rotation.z), "prop");
		}
	}

	public bool AreCornersConnected(DungeonNode topLeft, DungeonNode topRight, DungeonNode bottomLeft, DungeonNode bottomRight)
	{
		if (topLeft == null || topRight == null || bottomLeft == null || bottomRight == null)
		{
			return false;
		}
		if (topLeft.down && topLeft.right && topRight.left && bottomLeft.up && topRight.down && topRight.left && topLeft.right && bottomRight.up && bottomLeft.up && bottomLeft.right && bottomRight.left && topLeft.down && bottomRight.up && bottomRight.left && bottomLeft.right && topRight.down)
		{
			return true;
		}
		return false;
	}

	private bool AreSidesConnected(DungeonNode sourceNode, DungeonNode targetNode)
	{
		if (sourceNode == null || targetNode == null)
		{
			return false;
		}
		if (sourceNode.x != targetNode.x && sourceNode.y != targetNode.y)
		{
			return false;
		}
		if (NodeSourceLeftToTarget(sourceNode, targetNode))
		{
			return true;
		}
		if (NodeSourceRigthToTarget(sourceNode, targetNode))
		{
			return true;
		}
		if (NodeSourceAboveToTarget(sourceNode, targetNode))
		{
			return true;
		}
		if (NodeSourceBelowToTarget(sourceNode, targetNode))
		{
			return true;
		}
		return false;
	}

	private bool NodeSourceLeftToTarget(DungeonNode sourceNode, DungeonNode targetNode)
	{
		if (sourceNode.x < targetNode.x && sourceNode.right)
		{
			return targetNode.left;
		}
		return false;
	}

	private bool NodeSourceRigthToTarget(DungeonNode sourceNode, DungeonNode targetNode)
	{
		if (sourceNode.x > targetNode.x && sourceNode.left)
		{
			return targetNode.right;
		}
		return false;
	}

	private bool NodeSourceAboveToTarget(DungeonNode sourceNode, DungeonNode targetNode)
	{
		if (sourceNode.y > targetNode.y && sourceNode.down)
		{
			return targetNode.up;
		}
		return false;
	}

	private bool NodeSourceBelowToTarget(DungeonNode sourceNode, DungeonNode targetNode)
	{
		if (sourceNode.y < targetNode.y && sourceNode.up)
		{
			return targetNode.down;
		}
		return false;
	}

	public bool NodeIsConnected(DungeonNode targetNode)
	{
		if (targetNode == null)
		{
			return false;
		}
		foreach (DungeonNode connectedNode in _connectedNodes)
		{
			if (connectedNode.gameObject.GetInstanceID() == targetNode.gameObject.GetInstanceID())
			{
				return true;
			}
		}
		return false;
	}

	private void UpdatePoints()
	{
		_points = 0;
		if (_up)
		{
			_points++;
		}
		if (_down)
		{
			_points++;
		}
		if (_left)
		{
			_points++;
		}
		if (_right)
		{
			_points++;
		}
	}

	public void UpdateAsset(GameObject asset)
	{
		if (_asset != null)
		{
			Object.Destroy(_asset.gameObject);
			_asset = null;
		}
		if (_overlays != null)
		{
			foreach (GameObject overlay in _overlays)
			{
				Object.Destroy(overlay);
			}
			_overlays = null;
		}
		_asset = asset.gameObject;
		_asset.transform.SetParent(base.transform);
		_asset.transform.localPosition = new Vector3(0f, 0f, 0f);
		OnAssetLoaded();
	}

	private void OnAssetLoaded()
	{
		FrameNavigator componentInChildren = _asset.GetComponentInChildren<FrameNavigator>();
		_asset.transform.localScale = new Vector3(2f, 2f, 1f);
		_asset.transform.rotation = Quaternion.Euler(0f, 0f, GetDefinitionRotation());
		if (componentInChildren != null)
		{
			componentInChildren.GoToAndStop(GetAvailableFrame(componentInChildren.totalFrames));
			if (componentInChildren.transform.childCount <= 0)
			{
				Object.Destroy(_asset.gameObject);
			}
		}
		UpdateOverlays();
		CheckConnectedNodes(assets: true);
		SetVisible(_vis);
	}

	public int GetAvailableFrame(int totalFrames)
	{
		List<int> list = new List<int>();
		foreach (DungeonNode nearbyNode in _nearbyNodes)
		{
			if (!(nearbyNode.asset != null) || nearbyNode.points != _points || !(nearbyNode.asset.GetComponentInChildren<FrameNavigator>() != null))
			{
				continue;
			}
			int currentFrame = nearbyNode.asset.GetComponentInChildren<FrameNavigator>().currentFrame;
			bool flag = true;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == currentFrame)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				list.Add(currentFrame);
			}
		}
		if (list.Count >= totalFrames)
		{
			return Util.randomInt(1, totalFrames);
		}
		List<int> list2 = new List<int>();
		for (int j = 1; j <= totalFrames; j++)
		{
			bool flag2 = true;
			for (int k = 0; k < list.Count; k++)
			{
				if (list[k] == j)
				{
					flag2 = false;
					break;
				}
			}
			if (flag2)
			{
				list2.Add(j);
			}
		}
		if (list2.Count <= 0)
		{
			return Util.randomInt(1, totalFrames);
		}
		return list2[Util.randomInt(0, list2.Count - 1)];
	}

	public string GetDefinitionName()
	{
		switch (_points)
		{
		case 1:
			return "End";
		case 2:
			if ((_up && down) || (_left && right))
			{
				return "Straight";
			}
			return "Corner";
		case 3:
			return "Directional";
		case 4:
			return "Intersection";
		default:
			return "";
		}
	}

	public float GetDefinitionRotation()
	{
		switch (_points)
		{
		case 1:
			if (_up)
			{
				return 0f;
			}
			if (_right)
			{
				return -90f;
			}
			if (_down)
			{
				return 180f;
			}
			if (_left)
			{
				return 90f;
			}
			break;
		case 2:
			if (_up && _down)
			{
				return 180 * Util.randomInt(0, 1);
			}
			if (_left && _right)
			{
				return -90 + 180 * Util.randomInt(0, 1);
			}
			if (_up && _left)
			{
				return 0f;
			}
			if (_up && _right)
			{
				return -90f;
			}
			if (_right && _down)
			{
				return 180f;
			}
			if (_down && _left)
			{
				return 90f;
			}
			break;
		case 3:
			if (_up && _left && _down)
			{
				return 0f;
			}
			if (_left && _up && _right)
			{
				return -90f;
			}
			if (_up && _right && _down)
			{
				return 180f;
			}
			if (_left && _right && _down)
			{
				return 90f;
			}
			break;
		}
		return -90 * Util.randomInt(0, 3);
	}

	private int GetDirectionCount()
	{
		int num = 0;
		if (_up)
		{
			num++;
		}
		if (_down)
		{
			num++;
		}
		if (_left)
		{
			num++;
		}
		if (_right)
		{
			num++;
		}
		return num;
	}

	public DungeonNode GetRandomConnectedNode()
	{
		if (_connectedNodes == null || _connectedNodes.Count <= 0)
		{
			return null;
		}
		return _connectedNodes[Util.randomInt(0, _connectedNodes.Count - 1)];
	}

	public void SetFogVisibility(bool v)
	{
		_fog.SetActive(v && !_vis);
	}

	public static DungeonNode FromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("dun1");
		int int2 = sfsob.GetInt("dun2");
		bool @bool = sfsob.GetBool("dun3");
		bool bool2 = sfsob.GetBool("dun4");
		bool bool3 = sfsob.GetBool("dun5");
		bool bool4 = sfsob.GetBool("dun6");
		bool bool5 = sfsob.GetBool("dun28");
		bool bool6 = sfsob.GetBool("dun31");
		DungeonNode dungeonNode = new GameObject
		{
			name = "NODE-R" + @int + "-C" + int2
		}.AddComponent<DungeonNode>();
		dungeonNode.LoadDetails(@int, int2, @bool, bool2, bool3, bool4, bool5, bool6);
		if (sfsob.ContainsKey("dun13"))
		{
			DungeonObject dungeonObject = DungeonObject.FromSFSObject(sfsob);
			if (dungeonObject.type == 6)
			{
				if (!GameData.instance.SAVE_STATE.adsDisabled && AppInfo.adsAvailable && AppInfo.allowAds)
				{
					dungeonNode.SetObject(dungeonObject);
				}
				else
				{
					dungeonObject.gameObject.SetActive(value: false);
				}
			}
			else
			{
				dungeonNode.SetObject(dungeonObject);
			}
		}
		return dungeonNode;
	}
}
