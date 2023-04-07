using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.grid;
using com.ultrabit.bitheroes.model.sound;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.instance;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace com.ultrabit.bitheroes.ui.grid;

public class GridMap : MonoBehaviour
{
	public const float CAMERA_DELAY = 10f;

	public const float SCALE_DEFAULT = 1f;

	public const float SCALE_MIN = 0.75f;

	public const float SCALE_MAX = 1.25f;

	public const float SCALE_THRESHOLD = 0.25f;

	public const bool DEBUG = false;

	public const bool DEBUG_FOOTSTEPS = false;

	public Transform tilePrefab;

	public GameObject animClickPoint;

	[HideInInspector]
	public UnityCustomEvent SCALE_UPDATE = new UnityCustomEvent();

	private int _width;

	private int _height;

	private int _size;

	private bool _lockCamera;

	private SoundPoolRef _footstepsDefault;

	private int _xCount;

	private int _yCount;

	private Vector2 _cameraPosition;

	internal Gridpathfinder _pathfinder;

	private Rect _gridBounds;

	private Rect _cameraBounds;

	private float _scale;

	private GridObject _focus;

	private Tile[,] _tiles;

	private List<GridObject> _objects = new List<GridObject>();

	public List<GridObject> _dungeonObjects = new List<GridObject>();

	private float _cameraXMin;

	private float _cameraXMax;

	private float _focusRadius = 9f;

	private bool _cameraNearFocus;

	private int decimals = 3;

	private string tweenKey;

	public float width => _width;

	public float height => _height;

	public float scale => _scale;

	public int size => _size;

	public SoundPoolRef footstepsDefault => _footstepsDefault;

	public int xCount => _xCount;

	public int yCount => _yCount;

	public GridObject focus => _focus;

	public Tile[,] tiles => _tiles;

	public List<GridObject> objects => _objects;

	private void Start()
	{
		if (base.gameObject.name == "Map")
		{
			base.enabled = false;
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<BoxCollider2D>().enabled = false;
		}
		tweenKey = "MoveCameraTween" + GameData.instance.main.mainCamera.GetInstanceID();
	}

	public void LoadDetails(int width, int height, int size, bool lockCamera = true, SoundPoolRef footstepsDefault = null)
	{
		GameData.instance.grid = this;
		_width = width;
		_height = height;
		_size = size;
		_lockCamera = lockCamera;
		_footstepsDefault = footstepsDefault;
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 10f);
		_xCount = Mathf.RoundToInt(_width / _size);
		_yCount = Mathf.RoundToInt(_height / _size);
		_pathfinder = new Gridpathfinder(this);
		_gridBounds = new Rect(0f, -_height, _width, _height);
		UpdateCameraBounds();
		CreateTiles();
		float num = (AppInfo.IsMobile() ? (Main.SCREEN_SCALE / 1f) : 1f);
		SetScale(num);
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.AddListener(OnUpdate);
	}

	public virtual void Update()
	{
		if (AppInfo.TESTING)
		{
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				AdjustScale(-1);
			}
			else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				AdjustScale(1);
			}
		}
		UpdateCamera();
		UpdateObjects();
	}

	public void OnUpdate(object e)
	{
	}

	private void DoUpdate(float delta = 1f)
	{
	}

	public void UpdateCameraBounds()
	{
		if (_lockCamera)
		{
			_cameraBounds = new Rect(0f, 0f, Main.BOUNDS.width, Main.BOUNDS.height);
		}
	}

	public Tile GetNearestWalkableTile(Tile currentTile)
	{
		List<Tile> list = new List<Tile>();
		int num = 1;
		while (list.Count <= 0)
		{
			int num2 = 8 * num;
			int num3 = num2 / 4;
			int num4 = currentTile.xPos - num;
			int num5 = currentTile.yPos - num;
			for (int i = 0; i < num2; i++)
			{
				Tile tileByPosition = getTileByPosition(num4, num5);
				if (tileByPosition != null && tileByPosition.walkable)
				{
					list.Add(tileByPosition);
				}
				if (i >= num3 * 3)
				{
					num5--;
				}
				else if (i >= num3 * 2)
				{
					num4--;
				}
				else if (i >= num3)
				{
					num5++;
				}
				else
				{
					num4++;
				}
			}
			num++;
		}
		Tile tile = null;
		float num6 = 0f;
		if (list.Count > 0)
		{
			foreach (Tile item in list)
			{
				float distance = Util.GetDistance(focus.x, focus.y, item.x, item.y);
				if (tile == null || distance < num6)
				{
					tile = item;
					num6 = distance;
				}
			}
			return tile;
		}
		return tile;
	}

	private void CreateTiles()
	{
		_tiles = new Tile[_xCount, _yCount];
		int num = 0;
		for (int i = 0; i < _xCount; i++)
		{
			for (int j = 0; j < _yCount; j++)
			{
				Tile tile = new Tile(num, this, i, j);
				_tiles[i, j] = tile;
				tile.x = tile.xPos * _size + _size / 2;
				tile.y = -(tile.yPos * _size) - _size / 2;
				num++;
			}
		}
	}

	public Tile getTileFromMouse()
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(GameData.instance.main.mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		if ((bool)raycastHit2D)
		{
			int num = (int)Math.Floor(raycastHit2D.point.x / (float)size);
			int num2 = (int)Math.Floor((0f - raycastHit2D.point.y) / (float)size);
			if (_tiles.Length > 0 && _tiles.GetLength(0) > num && _tiles.GetLength(1) > num2)
			{
				return _tiles[num, num2];
			}
		}
		return null;
	}

	public Tile getTileFromPoint(float x, float y)
	{
		int xPos = Mathf.RoundToInt((x - (float)(size / 2)) / (float)size);
		int value = Mathf.RoundToInt((y + (float)(size / 2)) / (float)size);
		getTileByPosition(xPos, Mathf.Abs(value));
		return getTileByPosition(xPos, Mathf.Abs(value));
	}

	public Tile getTileByPosition(int xPos, int yPos)
	{
		if (xPos < 0)
		{
			return null;
		}
		if (xPos >= _xCount)
		{
			return null;
		}
		if (yPos < 0)
		{
			return null;
		}
		if (yPos >= _yCount)
		{
			return null;
		}
		return _tiles[xPos, yPos];
	}

	public List<Tile> GetNearbyTiles(Tile tile)
	{
		List<Tile> list = new List<Tile>();
		if (tile == null)
		{
			return list;
		}
		list.Add(tile);
		list.Add(getTileByPosition(tile.xPos + 1, tile.yPos));
		list.Add(getTileByPosition(tile.xPos - 1, tile.yPos));
		list.Add(getTileByPosition(tile.xPos, tile.yPos + 1));
		list.Add(getTileByPosition(tile.xPos, tile.yPos - 1));
		list.Add(getTileByPosition(tile.xPos + 1, tile.yPos - 1));
		list.Add(getTileByPosition(tile.xPos - 1, tile.yPos + 1));
		list.Add(getTileByPosition(tile.xPos + 1, tile.yPos + 1));
		list.Add(getTileByPosition(tile.xPos - 1, tile.yPos - 1));
		return list;
	}

	public Tile getTileByID(int id)
	{
		int num = Mathf.FloorToInt(id / _yCount);
		int yPos = id - _yCount * num;
		return getTileByPosition(num, yPos);
	}

	public List<Tile> GeneratePath(GridObject theObject, Tile tile)
	{
		if (tile == null)
		{
			return new List<Tile>();
		}
		if (theObject.tile.id == tile.id)
		{
			return new List<Tile>();
		}
		bool walkable = tile.walkable;
		tile.SetWalkable(walkable: true);
		if (_pathfinder == null)
		{
			_pathfinder = new Gridpathfinder(this);
		}
		List<Tile> path = _pathfinder.getPath(theObject.tile, tile);
		tile.SetWalkable(walkable);
		if (path == null)
		{
			return new List<Tile>();
		}
		if (path.Count > 0 && !path[path.Count - 1].walkable)
		{
			path.RemoveAt(path.Count - 1);
		}
		return path;
	}

	public void AddObject(GridObject theObject, Scene? scene = null, bool updateObjects = true)
	{
		_objects.Add(theObject);
		if (GameData.instance.PROJECT.instance != null)
		{
			theObject.gameObject.SetActive(GameData.instance.PROJECT.instance.gameObject.activeSelf);
		}
		UpdateObjectCollision(theObject, add: true);
		if (updateObjects)
		{
			UpdateObjects(clear: true);
		}
		if (scene.HasValue)
		{
			SceneManager.MoveGameObjectToScene(theObject.gameObject, scene.Value);
		}
	}

	public void AddDungeonObject(GridObject theObject, Scene? scene = null)
	{
		_dungeonObjects.Add(theObject);
		UpdateObjectCollision(theObject, add: true);
		UpdateObjects(clear: true);
		if (scene.HasValue)
		{
			SceneManager.MoveGameObjectToScene(theObject.gameObject, scene.Value);
		}
	}

	public void RemoveObject(GridObject gridObject)
	{
		for (int i = 0; i < _objects.Count; i++)
		{
			if (_objects[i].GetInstanceID() == gridObject.GetInstanceID())
			{
				_objects.RemoveAt(i);
				UpdateObjectCollision(gridObject, add: false);
				UnityEngine.Object.Destroy(gridObject.gameObject);
				UpdateObjects(clear: true);
				break;
			}
		}
	}

	public void RemoveDungeonObject(GridObject gridObject)
	{
		for (int i = 0; i < _dungeonObjects.Count; i++)
		{
			if (_dungeonObjects[i].GetInstanceID() == gridObject.GetInstanceID())
			{
				_dungeonObjects.RemoveAt(i);
				break;
			}
		}
	}

	public virtual void UpdateObjectCollision(GridObject theObject, bool add)
	{
	}

	public void AdjustScale(int value)
	{
		SetScale(_scale + (float)value * 0.25f);
	}

	public void SetScale(float scale)
	{
		if ((!(_scale <= 0.75f) || !(scale <= 0.75f)) && (!(_scale >= 1.25f) || !(scale >= 1.25f)))
		{
			_scale = scale;
			if (_scale < 0.75f)
			{
				_scale = 0.75f;
			}
			if (_scale > 1.25f)
			{
				_scale = 1.25f;
			}
			_ = _cameraBounds;
			if (_gridBounds.height * _scale < _cameraBounds.height)
			{
				_scale = _cameraBounds.height / _gridBounds.height;
			}
			if (_gridBounds.width * _scale < _cameraBounds.width)
			{
				_scale = _cameraBounds.width / _gridBounds.width;
			}
			GameData.instance.main.mainCamera.orthographicSize = Main.DEFAULT_BOUNDS.height / 2f * _scale;
			UpdateCamera(tween: false);
			SCALE_UPDATE.Invoke(this);
		}
	}

	public void SetFocus(GridObject obj)
	{
		_focus = obj;
	}

	public void UpdateCamera(bool tween = true, float delta = 1f)
	{
		if (_focus == null)
		{
			return;
		}
		Vector2 cameraPoint = _focus.GetCameraPoint();
		_cameraXMin = Camera.allCameras[0].orthographicSize * Camera.allCameras[0].aspect;
		_cameraXMax = (float)(xCount * size) - Camera.allCameras[0].orthographicSize * Camera.allCameras[0].aspect;
		if (Vector2.Distance(GameData.instance.main.mainCamera.transform.position, cameraPoint) < _focusRadius)
		{
			_cameraNearFocus = true;
			return;
		}
		_cameraNearFocus = false;
		if (tween)
		{
			SetTween(0.5f, cameraPoint);
			return;
		}
		GameData.instance.main.mainCamera.transform.position = new Vector3(cameraPoint.x, cameraPoint.y, GameData.instance.main.mainCamera.transform.position.z);
		if (this is Instance)
		{
			GameData.instance.main.mainCamera.transform.position = new Vector3((float)Math.Round(Mathf.Clamp(GameData.instance.main.mainCamera.transform.position.x, _cameraXMin, _cameraXMax), decimals), (float)Math.Round(Mathf.Clamp(GameData.instance.main.mainCamera.transform.position.y, (float)(-(yCount * size)) + GameData.instance.main.mainCamera.orthographicSize, 0f - GameData.instance.main.mainCamera.orthographicSize), decimals), GameData.instance.main.mainCamera.transform.position.z);
		}
	}

	private void SetTween(float duration, Vector2 focusPoint)
	{
		Vector3 endValue = new Vector3(focusPoint.x, focusPoint.y, GameData.instance.main.mainCamera.transform.position.z);
		_ = GameData.instance.main.mainCamera.transform.position;
		GameData.instance.main.mainCamera.transform.DOMove(endValue, duration).SetEase(Ease.OutQuad).OnUpdate(delegate
		{
			if (!_cameraNearFocus && !(GameData.instance.main.mainCamera == null) && this is Instance)
			{
				GameData.instance.main.mainCamera.transform.position = new Vector3((float)Math.Round(Mathf.Clamp(GameData.instance.main.mainCamera.transform.position.x, _cameraXMin, _cameraXMax), decimals), (float)Math.Round(Mathf.Clamp(GameData.instance.main.mainCamera.transform.position.y, (float)(-(yCount * size)) + GameData.instance.main.mainCamera.orthographicSize, 0f - GameData.instance.main.mainCamera.orthographicSize), decimals), GameData.instance.main.mainCamera.transform.position.z);
			}
		});
	}

	public void DeleteTile(Tile tile)
	{
	}

	public void PrintData()
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		for (int i = 0; i < _xCount; i++)
		{
			for (int j = 0; j < _yCount; j++)
			{
				Tile tile = _tiles[i, j];
				text += (tile.walkable ? "0" : "1");
				text3 += tile.footstepID;
				if (tile.spawn)
				{
					if (text2.Length > 0)
					{
						text2 += ",";
					}
					text2 += tile.id;
				}
			}
		}
	}

	public void UpdateObjects(bool clear = false)
	{
		if (_objects == null)
		{
			return;
		}
		if (clear)
		{
			if (this is Dungeon)
			{
				ClearDungeonObjectPositions();
			}
			else
			{
				ClearObjectPositions();
			}
		}
		List<GridObject> list = new List<GridObject>();
		_dungeonObjects.RemoveAll((GridObject item) => item == null);
		list = ((!(this is Dungeon)) ? Util.SortVector(_objects, new string[1] { "y" }, Util.ARRAY_DESCENDING) : Util.SortVector(_dungeonObjects, new string[1] { "y" }, Util.ARRAY_DESCENDING));
		bool flag = CheckObjectPositionChange(list);
		for (int i = 0; i < list.Count; i++)
		{
			GridObject gridObject = list[i];
			if (gridObject == null)
			{
				continue;
			}
			if (gridObject.transform.childCount > 0)
			{
				Transform child = gridObject.transform.GetChild(0);
				if (child.name.Contains("GuildStairs") && child.parent != null)
				{
					SortingGroup component = child.parent.GetComponent<SortingGroup>();
					if (component != null && component.enabled && component.sortingOrder <= 0)
					{
						component.sortingOrder = 20;
					}
				}
			}
			if (gridObject.tile != null && flag)
			{
				gridObject.setPosition(i);
			}
		}
	}

	private bool CheckObjectPositionChange(List<GridObject> sorted)
	{
		for (int i = 0; i < sorted.Count; i++)
		{
			if (sorted[i].layerPosition != i)
			{
				return true;
			}
		}
		return false;
	}

	public void ClearObjectPositions()
	{
		foreach (GridObject @object in _objects)
		{
			@object.setPosition(-1);
		}
	}

	public void ClearDungeonObjectPositions()
	{
		foreach (GridObject dungeonObject in _dungeonObjects)
		{
			if (dungeonObject != null)
			{
				dungeonObject.setPosition(-1);
			}
		}
	}

	public virtual void OnDestroy()
	{
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnUpdate);
	}
}
