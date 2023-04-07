using System;
using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.dungeon;
using com.ultrabit.bitheroes.model.encounter;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.daily;
using com.ultrabit.bitheroes.ui.dialog;
using com.ultrabit.bitheroes.ui.familiar;
using com.ultrabit.bitheroes.ui.friend;
using com.ultrabit.bitheroes.ui.game;
using com.ultrabit.bitheroes.ui.grid;
using com.ultrabit.bitheroes.ui.guild;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.menu;
using com.ultrabit.bitheroes.ui.shop;
using com.ultrabit.bitheroes.ui.tutorial;
using com.ultrabit.bitheroes.ui.utility;
using com.ultrabit.bitheroes.ui.victory;
using com.ultrabit.bitheroes.ui.vipgor;
using Newtonsoft.Json;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace com.ultrabit.bitheroes.ui.dungeon;

public class Dungeon : GridMap, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	private static Dungeon _instance;

	public const int TYPE_NONE = 0;

	public const int TYPE_ZONE = 1;

	public const int TYPE_RAID = 2;

	public const int TYPE_RIFT = 3;

	public const int TYPE_GVE = 4;

	public const float NODE_SCALE = 2f;

	public const int NODE_SIZE = 280;

	public const int NODE_BOUNDS = 180;

	public const int TILE_DENSITY = 7;

	public const float OBJECT_SCALE = 2f;

	private const float Z_POS_INVISIBLE = -150000f;

	private static Dictionary<int, string> TYPE_IDENTIFIER = new Dictionary<int, string>
	{
		[0] = "???",
		[1] = "Zone",
		[2] = "Raid",
		[3] = "Trial",
		[4] = "GvE"
	};

	public const string COMPLETE_TYPE_WIN = "Win";

	public const string COMPLETE_TYPE_LOSE = "Lose";

	public const string COMPLETE_TYPE_QUIT = "Quit";

	private DungeonExtension _extension;

	private DungeonRef _dungeonRef;

	private int _type;

	private int _rows;

	private int _columns;

	private List<DungeonNode> _nodeList;

	private List<DungeonPlayer> _players;

	private List<Asset> _blanks;

	private object _data;

	private string _uid;

	private bool _trackedStart;

	private bool _trackedEnd;

	private int _creditsGained;

	private int _goldGained;

	private bool _loaded;

	private GameObject _background;

	private DungeonObject _target;

	private List<List<DungeonNode>> _nodes = new List<List<DungeonNode>>();

	private List<DungeonNode> _objectNodes = new List<DungeonNode>();

	private DungeonNode _focusNode;

	private GameObject _asset;

	private bool _mouseDown;

	private Tile _mouseTile;

	private Vector2 _direction;

	private Tile _directionTile;

	private List<DungeonOverlay> _overlays;

	private int _musicPosition;

	private EventRef _eventRef;

	private IEnumerator _timerMoveToObject;

	private bool _sceneDestroyed;

	private BoxCollider2D _mapCollider;

	private Scene _dungeonScene;

	private DungeonUI _dungeonUI;

	public DungeonPlayer dungeonPlayerPrefab;

	public DungeonFollower dungeonFollowerPrefab;

	public DungeonObject dungeonObjectPrefab;

	public DungeonOverlay dungeonOverlayPrefab;

	[HideInInspector]
	public UnityCustomEvent COMPLETE = new UnityCustomEvent();

	private int _pveEnergyStart;

	private int _pvpEnergyStart;

	public static Dungeon instance => _instance;

	public DungeonRef dungeonRef => _dungeonRef;

	public int type => _type;

	public object data => _data;

	public GameObject asset => _asset;

	public DungeonExtension extension => _extension;

	public int musicPosition => _musicPosition;

	public Scene dungeonScene => _dungeonScene;

	public DungeonUI dungeonUI => _dungeonUI;

	public bool sceneDestroyed => _sceneDestroyed;

	public DungeonOverlay DungeonOverlay { get; private set; }

	public void InitDungeon()
	{
		if (_instance == null)
		{
			_instance = this;
		}
	}

	public void Create(DungeonExtension extension, DungeonRef dungeonRef, int type, int rows, int columns, List<DungeonNode> nodes, List<DungeonPlayer> players, object data)
	{
		LoadDetails(280 * columns, 280 * rows, 40, lockCamera: false, dungeonRef.footstepsDefault);
		_extension = extension;
		_dungeonRef = dungeonRef;
		_type = type;
		_rows = rows;
		_columns = columns;
		_nodeList = nodes;
		_players = players;
		_data = data;
		_dungeonUI = GameData.instance.windowGenerator.NewDungeonUI(this);
		_extension.SetDungeon(this);
		_extension.AddListener(CustomSFSXEvent.CHANGE, OnExtensionChange);
		_mapCollider = base.gameObject.AddComponent<BoxCollider2D>();
		_mapCollider.size = new Vector2(280 * columns / 40 * 40, 280 * rows / 40 * 40);
		_mapCollider.offset = new Vector2((int)_mapCollider.size.x / 2, -(int)_mapCollider.size.y / 2);
		GameData.instance.PROJECT.character.AddListener("AUTO_PILOT_CHANGE", OnAutoPilotChange);
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		switch (_type)
		{
		case 3:
			_eventRef = RiftEventBook.GetCurrentEventRef();
			break;
		case 4:
			_eventRef = GvEEventBook.GetCurrentEventRef();
			break;
		}
		ColorUtility.TryParseHtmlString("#" + _dungeonRef.color, out var color);
		GameData.instance.main.mainCamera.backgroundColor = color;
		LoadAsset();
		StatLoudout();
	}

	public bool DoBack()
	{
		if (GameData.instance.tutorialManager.hasPopup || _dungeonUI.disabled || _dungeonUI.scrollingIn || _dungeonUI.scrollingOut)
		{
			return false;
		}
		_dungeonUI.DoExitConfirm();
		return true;
	}

	public bool DoForward()
	{
		if (GameData.instance.tutorialManager.hasPopup || _dungeonUI.disabled || _dungeonUI.scrollingIn || _dungeonUI.scrollingOut || GameData.instance.windowGenerator.HasDialogByClass(typeof(VictoryWindow)) || GameData.instance.windowGenerator.HasDialogByClass(typeof(CharacterWindow)))
		{
			return false;
		}
		DoToggleAutoPilot();
		return true;
	}

	public void AddedToStage(Scene dungeonScene)
	{
		_dungeonScene = dungeonScene;
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.AddListener(OnFrameUpdate);
		Init();
		SceneManager.MoveGameObjectToScene(base.gameObject, _dungeonScene);
		int num = 0;
		if (_players != null)
		{
			num += _players.Count;
		}
		if (_nodes != null)
		{
			num += _nodes.Count;
		}
		int num2 = 0;
		foreach (DungeonPlayer player in _players)
		{
			SceneManager.MoveGameObjectToScene(player.gameObject, _dungeonScene);
			player.MoveFollowersToDungeonScene();
			num2++;
			if (GameData.instance.PROJECT != null)
			{
				GameData.instance.PROJECT.UpdateTransitionScreenProgress(Mathf.Round((float)num * 100f / (float)num2));
			}
		}
		foreach (List<DungeonNode> node in _nodes)
		{
			foreach (DungeonNode item in node)
			{
				if (!(item != null))
				{
					continue;
				}
				SceneManager.MoveGameObjectToScene(item.gameObject, _dungeonScene);
				if (item.obj != null)
				{
					SceneManager.MoveGameObjectToScene(item.obj.gameObject, _dungeonScene);
					item.obj.OnAddedToStage();
					num2++;
					if (GameData.instance.PROJECT != null)
					{
						GameData.instance.PROJECT.UpdateTransitionScreenProgress(Mathf.Round((float)num * 100f / (float)num2));
					}
				}
			}
		}
	}

	private void SetTiles()
	{
		DungeonPlayer dungeonPlayer = base.focus as DungeonPlayer;
		for (int i = 0; i < base.xCount; i++)
		{
			for (int j = 0; j < base.yCount; j++)
			{
				Tile tileByPosition = getTileByPosition(i, j);
				DungeonNode nodeByPoint = GetNodeByPoint(new Vector2(tileByPosition.x, tileByPosition.y));
				if (nodeByPoint == null)
				{
					tileByPosition.SetWalkable(walkable: false);
					DeleteTile(tileByPosition);
					continue;
				}
				nodeByPoint.AddTile(tileByPosition);
				Vector2 value = new Vector2((float)tileByPosition.x - nodeByPoint.x, (float)Mathf.Abs(tileByPosition.y) - Mathf.Abs(nodeByPoint.y));
				Vector2 nodeTargetPoint = dungeonPlayer.GetNodeTargetPoint(nodeByPoint, value);
				if ((float)tileByPosition.x != nodeTargetPoint.x || (float)tileByPosition.y != nodeTargetPoint.y)
				{
					tileByPosition.SetWalkable(walkable: false);
				}
			}
		}
	}

	private void LoadAsset()
	{
		_asset = GameData.instance.main.assetLoader.GetGameObjectAsset(AssetURL.DUNGEON, dungeonRef.asset, instantiate: false);
		OnAssetLoaded();
	}

	private void LoadAssets()
	{
	}

	public void CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null)
		{
			return;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(7))
		{
			DungeonNode objectNode = GetObjectNode(base.focus.gameObject);
			DungeonNode randomConnectedNode = objectNode.GetRandomConnectedNode();
			if (randomConnectedNode != null)
			{
				int arrowPosition = ((randomConnectedNode.y > objectNode.y) ? 3 : 4);
				Vector2 vector = new Vector2(randomConnectedNode.x, randomConnectedNode.y);
				GameData.instance.PROJECT.character.tutorial.SetState(7);
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(null, new TutorialPopUpSettings(Tutorial.GetText(7), arrowPosition, vector, 0f, indicator: true), null, stageTrigger: true, EventTriggerType.PointerDown);
				return;
			}
		}
		CheckAutoPilot();
	}

	public void CheckAutoPilot()
	{
		if (_extension == null || _extension.waiting || _extension.paused || _extension.defeated || _extension.resolvingAd || (base.focus is DungeonPlayer && (base.focus as DungeonPlayer).IsDead()))
		{
			return;
		}
		_target = null;
		base.focus.ClearPath(finish: false);
		if (IsCleared())
		{
			_extension.ShowCleared();
		}
		else
		{
			if (!GameData.instance.PROJECT.character.autoPilot)
			{
				return;
			}
			DungeonNode objectNode = GetObjectNode(base.focus.gameObject);
			List<DungeonNode> list = null;
			foreach (DungeonNode objectNode2 in _objectNodes)
			{
				if (objectNode2.gameObject.GetInstanceID() != objectNode.gameObject.GetInstanceID() && !objectNode2.empty && (!(objectNode2.obj != null) || !objectNode2.obj.ignorePath))
				{
					List<DungeonNode> shortestPath = GetShortestPath(objectNode, objectNode2);
					if (shortestPath != null && (list == null || shortestPath.Count < list.Count))
					{
						list = shortestPath;
					}
				}
			}
			if (list != null && list.Count > 0)
			{
				DungeonObject obj = list[list.Count - 1].obj;
				if (obj.clickable)
				{
					_target = obj;
				}
				base.focus.SetPath(GeneratePath(base.focus, obj.tile));
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
		}
	}

	public bool IsCleared()
	{
		if (_extension.defeated)
		{
			return false;
		}
		DungeonNode objectNode = GetObjectNode(base.focus.gameObject);
		foreach (DungeonNode objectNode2 in _objectNodes)
		{
			if (objectNode.GetInstanceID() != objectNode2.GetInstanceID() && (!(objectNode2.obj != null) || !objectNode2.obj.ignoreClear) && !objectNode2.empty)
			{
				return false;
			}
		}
		return true;
	}

	private void OnFrameUpdate(object e)
	{
		if (!CheckDirectionMovement())
		{
			CheckMouseMovement();
		}
		ScrollOverlays();
		UpdateVisibility();
	}

	private void OnInventoryChange()
	{
	}

	private void OnAutoPilotChange()
	{
		if (!(GameData.instance.PROJECT.battle != null) && GameData.instance.windowGenerator.GetDialogLayerCount(5, new Type[14]
		{
			typeof(DungeonUI),
			typeof(CharacterWindow),
			typeof(ItemSelectWindow),
			typeof(DailyQuestsWindow),
			typeof(DialogWindow),
			typeof(FamiliarsWindow),
			typeof(ShopWindow),
			typeof(FriendWindow),
			typeof(GameModifierTimeWindow),
			typeof(GameSettingsWindow),
			typeof(GuildlessWindow),
			typeof(GuildWindow),
			typeof(VipGorWindow),
			typeof(DungeonAdWindow)
		}) <= 0)
		{
			CheckAutoPilot();
		}
	}

	public override void Update()
	{
		base.Update();
		CheckLoaded();
		if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && _direction.y >= 0f)
		{
			_target = null;
			SetDirection((int)_direction.x, -1);
		}
		if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && _direction.y <= 0f)
		{
			_target = null;
			SetDirection((int)_direction.x, 1);
		}
		if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && _direction.x >= 0f)
		{
			_target = null;
			SetDirection(-1, (int)_direction.y);
		}
		if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && _direction.x <= 0f)
		{
			_target = null;
			SetDirection(1, (int)_direction.y);
		}
		if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) && _direction.y < 0f)
		{
			SetDirection((int)_direction.x);
			_directionTile = null;
		}
		if ((Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) && _direction.y > 0f)
		{
			SetDirection((int)_direction.x);
			_directionTile = null;
		}
		if ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) && _direction.x < 0f)
		{
			SetDirection(0, (int)_direction.y);
			_directionTile = null;
		}
		if ((Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) && _direction.x > 0f)
		{
			SetDirection(0, (int)_direction.y);
			_directionTile = null;
		}
	}

	private void SetDirection(int xPos = 0, int yPos = 0)
	{
		_direction = new Vector2(xPos, yPos);
	}

	private void OnExtensionChange()
	{
		CheckObjects();
	}

	public void CheckObjects()
	{
		if (_extension.waiting || _extension.paused)
		{
			_dungeonUI.SetObjects(flag: false);
		}
		else if (!_extension.waiting && !_extension.paused)
		{
			_dungeonUI.SetObjects(flag: true);
		}
	}

	private void OnAssetLoaded()
	{
		Init();
	}

	private void Init()
	{
		if (base.focus != null)
		{
			foreach (DungeonNode node in _nodeList)
			{
				node.UpdateAsset(GetNodeAsset(node.GetDefinitionName()));
			}
			return;
		}
		List<DungeonNode> list = Util.SortVector(_nodeList, new string[1] { "row" });
		foreach (DungeonNode item in list)
		{
			SetNode(item);
		}
		foreach (DungeonNode node2 in _nodeList)
		{
			node2.SetDungeon(this);
		}
		foreach (DungeonNode node3 in _nodeList)
		{
			node3.CheckConnectedNodes();
		}
		foreach (DungeonNode item2 in list)
		{
			GameObject nodeAsset = GetNodeAsset(item2.GetDefinitionName());
			nodeAsset.transform.SetParent(item2.transform);
			item2.x = 280 * item2.column + 140;
			item2.y = -(280 * item2.row) - 140;
			item2.UpdateAsset(nodeAsset);
			if (!item2.empty)
			{
				_objectNodes.Add(item2);
			}
		}
		foreach (DungeonNode node4 in _nodeList)
		{
			node4.UpdateNode();
		}
		foreach (DungeonPlayer player in _players)
		{
			player.SetDungeon(this);
			AddObject(player);
			if (player.charID == GameData.instance.PROJECT.character.id)
			{
				SetFocus(player);
			}
		}
		foreach (DungeonNode node5 in _nodeList)
		{
			node5.CheckConnectedNodes();
		}
		if (base.focus == null)
		{
			SetFocus(_players[0]);
		}
		base.focus.MOVEMENT_END.AddListener(OnFocusMovementEnd);
		base.focus.MOVEMENT_STOP.AddListener(OnFocusMovementChange);
		OnFocusMovementChange(null);
		_dungeonUI.CreateEntityTiles();
		CreateOverlays();
		CheckObjects();
		SetTiles();
		foreach (DungeonNode objectNode in _objectNodes)
		{
			if (objectNode.obj != null)
			{
				objectNode.obj.SetDungeon(this, objectNode);
			}
			objectNode.UpdateObject();
		}
		UpdateCamera(tween: false);
		TrackStart();
	}

	public void CheckLoaded()
	{
		if (_asset == null)
		{
			return;
		}
		if (!_loaded)
		{
			foreach (DungeonNode node in _nodeList)
			{
				if (node.asset == null)
				{
					return;
				}
			}
			_loaded = true;
		}
		UpdateVisibility();
	}

	private void CreateOverlays()
	{
		if (_dungeonRef.overlays == null || _dungeonRef.overlays.Count <= 0)
		{
			return;
		}
		_overlays = new List<DungeonOverlay>();
		foreach (DungeonOverlayRef overlay in _dungeonRef.overlays)
		{
			UnityEngine.Object.Instantiate(instance.dungeonOverlayPrefab).LoadDetails(this, overlay);
		}
	}

	private void ScrollOverlays()
	{
	}

	private void OnFocusMovementChange(object e)
	{
		DungeonNode objectNode = GetObjectNode(base.focus.gameObject);
		if (objectNode == null)
		{
			return;
		}
		DungeonPlayer dungeonPlayer = base.focus as DungeonPlayer;
		dungeonPlayer.AddFootstep(objectNode);
		objectNode.SetTouched(v: true);
		if (_focusNode == null || _focusNode.GetInstanceID() != objectNode.GetInstanceID())
		{
			_focusNode = objectNode;
			_dungeonObjects.Clear();
			_dungeonObjects.Add(dungeonPlayer);
			foreach (DungeonFollower follower in dungeonPlayer.followers)
			{
				_dungeonObjects.Add(follower);
			}
			if (objectNode.dungeonObject != null)
			{
				_dungeonObjects.Add(objectNode.dungeonObject);
			}
			foreach (DungeonNode node in _nodeList)
			{
				bool flag = NodeIsInVector(node, objectNode.connectedNodes);
				if (flag && node.dungeonObject != null)
				{
					_dungeonObjects.Add(node.dungeonObject);
				}
				else if (!flag && node.dungeonObject != null)
				{
					node.dungeonObject.setPosition(-1);
				}
				if (flag)
				{
					node.SetVisible(v: true);
				}
				else
				{
					node.SetVisible(node.GetInstanceID() == objectNode.GetInstanceID());
				}
				node.UpdateFog(draw: true, flag, objectNode.connectedNodes);
			}
			if (_focusNode.forced)
			{
				CheckPlayerNode();
				return;
			}
		}
		CheckPlayerTile();
	}

	private void OnFocusMovementEnd(object e)
	{
		if (_target != null)
		{
			if (_target.instant)
			{
				CheckPlayerTile();
			}
			else
			{
				ExecuteObject(_target);
			}
		}
	}

	private void CheckPlayerTile()
	{
		if (_extension.waiting || _extension.paused)
		{
			return;
		}
		Tile tile = (base.focus as DungeonPlayer).tile;
		DungeonObject focusObject = GetFocusObject();
		if (tile == null || focusObject == null)
		{
			return;
		}
		Tile tile2 = focusObject.tile;
		DungeonNode node = focusObject.node;
		if (node == null || (focusObject.distance <= 0 && tile2.id != tile.id))
		{
			return;
		}
		if (focusObject.distance > 0)
		{
			List<Tile> nearbyTiles = GetNearbyTiles(tile2);
			bool flag = false;
			foreach (Tile item in nearbyTiles)
			{
				if (item.id == tile.id)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		if (focusObject.instant)
		{
			ExecuteObject(focusObject, wait: false);
			if (focusObject.distance > 0)
			{
				node.SetObject(null, destroy: false);
				DoObjectToPlayer(focusObject);
			}
			else
			{
				TriggerObject(focusObject, destroy: false);
				node.SetObject(null);
			}
		}
	}

	private void DoObjectToPlayer(DungeonObject obj)
	{
		obj.ENTER_FRAME.AddListener(OnObjectToPlayer);
		UpdateObjectToPlayer(obj);
	}

	private void OnObjectToPlayer(object e)
	{
		DungeonObject obj = e as DungeonObject;
		UpdateObjectToPlayer(obj);
	}

	private void UpdateObjectToPlayer(DungeonObject obj)
	{
		DungeonPlayer dungeonPlayer = base.focus as DungeonPlayer;
		float distance = Util.GetDistance(obj.x, obj.y, dungeonPlayer.x, dungeonPlayer.y);
		if (distance < (float)base.size / 1.1f)
		{
			obj.ENTER_FRAME.RemoveListener(OnObjectToPlayer);
			TriggerObject(obj);
			return;
		}
		float num = distance / (float)base.size;
		float num2 = obj.GetSpeed() / 250f * 0.5f * num;
		Vector2 vector = new Vector2(dungeonPlayer.x - obj.x, dungeonPlayer.y - obj.y);
		if (AppInfo.TESTING)
		{
			num2 *= 3f;
		}
		obj.x += vector.x * num2;
		obj.y += vector.y * num2;
	}

	private void TriggerObject(DungeonObject obj, bool destroy = true)
	{
		DungeonPlayer dungeonPlayer = base.focus as DungeonPlayer;
		ItemData firstItem = obj.GetFirstItem();
		if (firstItem != null)
		{
			Transform obj2 = UnityEngine.Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/battle/BattleTextFix"));
			obj2.SetParent(base.transform, worldPositionStays: false);
			obj2.GetComponent<TextMeshPro>().fontSize = 200f;
			obj2.GetComponent<BattleText>().LoadDetails(Language.GetString("ui_plus") + Util.NumberFormat(firstItem.qty), "#" + ItemRef.getItemColorHex(firstItem.itemRef), 3f, 0f, dungeonPlayer.x, dungeonPlayer.y - 0f);
			GameData.instance.PROJECT.character.addItem(firstItem, dispatch: true, isDungeonLoot: true);
			if (firstItem.itemRef.itemType == 3)
			{
				switch (firstItem.itemRef.id)
				{
				case 1:
					AddGoldGained(firstItem.qty);
					break;
				case 2:
					AddCreditsGained(firstItem.qty);
					break;
				}
			}
			new List<ItemData>().Add(firstItem);
			GameData.instance.audioManager.PlaySoundLink(ItemRef.getItemSoundLink(firstItem.itemRef));
		}
		if (destroy)
		{
			obj.DoDestroy();
		}
		CheckAutoPilot();
	}

	private void CheckPlayerNode()
	{
		if (_extension.waiting || _extension.paused)
		{
			return;
		}
		DungeonPlayer dungeonPlayer = base.focus as DungeonPlayer;
		DungeonNode objectNode = GetObjectNode(base.focus.gameObject);
		if (objectNode.forced)
		{
			SetDirection();
			_target = null;
			DungeonObject obj = objectNode.obj;
			bool flag = false;
			int num = objectNode.obj.type;
			if (num == 0 || num == 2)
			{
				flag = true;
			}
			_extension.SetPaused(pause: true);
			EndMouseDown();
			base.focus.ClearPath();
			obj.StopPathing();
			GridObject gridObject = obj;
			GridObject gridObject2 = base.focus;
			if (flag)
			{
				dungeonUI.dungeonEntitiesList.CancelTeamListDrag();
				float num2 = 1f;
				if (AppInfo.TESTING)
				{
					num2 /= 3f;
				}
				_musicPosition = GameData.instance.audioManager.GetMusicPosition();
				GameData.instance.audioManager.StopMusic();
				GameData.instance.audioManager.PlaySoundLink("encounter");
				GameData.instance.windowGenerator.ClearAllWindows(null, removeChat: false);
				gridObject.SetExclamation(enabled: true);
				gridObject2.SetExclamation(enabled: true);
				if (dungeonPlayer.x != obj.x)
				{
					obj.SetYRotation((dungeonPlayer.x < obj.x) ? 180 : 0);
				}
				if (_timerMoveToObject != null)
				{
					StopCoroutine(_timerMoveToObject);
					_timerMoveToObject = null;
				}
				_timerMoveToObject = MoveToObject(num2, gridObject, gridObject2);
				StartCoroutine(_timerMoveToObject);
			}
			else
			{
				DoObjectMoveToObject(gridObject, gridObject2);
			}
		}
		CheckObjects();
	}

	private IEnumerator MoveToObject(float delay, GridObject moveSource, GridObject moveTarget)
	{
		yield return new WaitForSeconds(delay);
		DoObjectMoveToObject(moveSource, moveTarget);
	}

	private void DoObjectMoveToObject(GridObject source, GridObject target)
	{
		source.SetExclamation();
		target.SetExclamation();
		if (source is DungeonObject)
		{
			source.SetSpeedMult(2f);
		}
		List<Tile> list = GeneratePath(source, target.tile);
		if (list.Count > 0)
		{
			list.RemoveAt(list.Count - 1);
		}
		if (list.Count > 0)
		{
			source.SetPath(list);
			source.MOVEMENT_END.AddListener(OnObjectMoveToObjectEnd);
		}
		else
		{
			CompleteObjectMoveToObjectEnd(source);
		}
	}

	private void OnObjectMoveToObjectEnd(object e)
	{
		GridObject gridObject = e as GridObject;
		gridObject.MOVEMENT_END.RemoveListener(OnObjectMoveToObjectEnd);
		CompleteObjectMoveToObjectEnd(gridObject);
	}

	private void CompleteObjectMoveToObjectEnd(GridObject obj)
	{
		if (obj is DungeonObject)
		{
			obj.SetSpeedMult(1f);
		}
		ExecuteObject(GetFocusObject());
	}

	private DungeonObject GetFocusObject()
	{
		DungeonPlayer dungeonPlayer = base.focus as DungeonPlayer;
		return dungeonPlayer.footsteps[dungeonPlayer.footsteps.Count - 1].obj;
	}

	private void ExecuteObject(DungeonObject obj, bool wait = true)
	{
		DungeonPlayer dungeonPlayer = base.focus as DungeonPlayer;
		GetObjectNode(dungeonPlayer.gameObject);
		switch (obj.type)
		{
		case 1:
			dungeonUI.dungeonEntitiesList.CancelTeamListDrag();
			if (DungeonBook.LookupTreasure(obj.id).locked)
			{
				if (GameData.instance.PROJECT.character.autoPilot && GameData.instance.SAVE_STATE.declineTreasures)
				{
					ActivateObject(dungeonPlayer, obj, wait: true, 0, 0);
				}
				else
				{
					GameData.instance.windowGenerator.NewDungeonTreasureWindow(this, dungeonPlayer, obj);
				}
			}
			else
			{
				ActivateObject(dungeonPlayer, obj, wait);
			}
			break;
		case 3:
			dungeonUI.dungeonEntitiesList.CancelTeamListDrag();
			if (!GameData.instance.PROJECT.character.autoPilot)
			{
				DungeonShrineRef dungeonShrineRef = DungeonBook.LookupShrine(obj.id);
				bool flag = false;
				string text = "";
				if (dungeonShrineRef.shrineType == 0)
				{
					flag = dungeonPlayer.CheckTeamFullHealth();
					text = Language.GetString("dungeon_shrine_health_stat");
				}
				else if (dungeonShrineRef.shrineType == 1)
				{
					flag = dungeonPlayer.CheckTeamFullSP();
					text = Language.GetString("dungeon_shrine_sp_stat");
				}
				if (flag)
				{
					DoDialogUseConfirm(DungeonObject.GetTypeName(obj.type), Language.GetString("dungeon_object_full_confirm", new string[1] { dungeonShrineRef.name }) + " " + text, obj);
				}
				else
				{
					DoDialogUseConfirm(DungeonObject.GetTypeName(obj.type), Language.GetString("dungeon_object_confirm", new string[1] { dungeonShrineRef.name }), obj);
				}
				_extension.SetWaiting(wait: false);
				CheckObjects();
			}
			else
			{
				ActivateObject(dungeonPlayer, obj, wait);
			}
			break;
		case 5:
		{
			dungeonUI.dungeonEntitiesList.CancelTeamListDrag();
			ItemRef itemRef = obj.GetFirstItem().itemRef;
			if (GameData.instance.PROJECT.character.autoPilot && GameData.instance.SAVE_STATE.declineMerchants && GameData.instance.SAVE_STATE.GetDeclineMerchantsRarity(GameData.instance.PROJECT.character.id, itemRef.rarityRef, GameData.instance.SAVE_STATE.GetDeclineMerchantsRarities(GameData.instance.PROJECT.character.id)))
			{
				ActivateObject(dungeonPlayer, obj, wait: true, 0, 0);
			}
			else
			{
				GameData.instance.windowGenerator.NewDungeonMerchantWindow(this, dungeonPlayer, obj, itemRef.getCostClassesArray());
			}
			break;
		}
		case 6:
			dungeonUI.dungeonEntitiesList.CancelTeamListDrag();
			if (GameData.instance.PROJECT.character.autoPilot && GameData.instance.SAVE_STATE.adsDisabled)
			{
				ActivateObject(dungeonPlayer, obj, wait: true, 0, 0);
			}
			else
			{
				GameData.instance.windowGenerator.NewDungeonAdWindow(this, dungeonPlayer, obj);
			}
			break;
		case 0:
		{
			DungeonEnemyRef enemy = dungeonRef.getEnemy(obj.id);
			ExecuteEncounter(enemy.encounter, obj, wait);
			break;
		}
		case 2:
		{
			DungeonBossRef boss = dungeonRef.boss;
			ExecuteEncounter(boss.encounter, obj, wait);
			break;
		}
		case 4:
			if (!obj.instant)
			{
				dungeonUI.dungeonEntitiesList.CancelTeamListDrag();
			}
			ActivateObject(dungeonPlayer, obj, wait);
			break;
		default:
			ActivateObject(dungeonPlayer, obj, wait);
			break;
		}
	}

	private void ExecuteEncounter(EncounterRef encounterRef, DungeonObject obj, bool wait = true)
	{
		DungeonPlayer dungeonPlayer = base.focus as DungeonPlayer;
		DialogRef dialogStart = encounterRef.getDialogStart();
		if (dialogStart != null && !dialogStart.seen)
		{
			GameData.instance.windowGenerator.NewDialogPopup(dialogStart, new object[3] { dungeonPlayer, obj, wait }).CLEAR.AddListener(OnEncounterStartDialogClosed);
		}
		else
		{
			ActivateObject(dungeonPlayer, obj, wait);
		}
	}

	private void OnEncounterStartDialogClosed(object e)
	{
		DialogPopup obj = e as DialogPopup;
		obj.CLEAR.RemoveListener(OnEncounterStartDialogClosed);
		object[] array = obj.data as object[];
		ActivateObject(array[0] as DungeonPlayer, array[1] as DungeonObject, (array[2] as bool?).Value);
	}

	public void ActivateObject(DungeonPlayer player, DungeonObject obj, bool wait = true, int currencyID = -1, int currencyCost = -1)
	{
		_target = null;
		player.AddFootstep(obj.node);
		_extension.DoObjectActivate(player.footsteps, wait, currencyID, currencyCost);
		player.ClearFootsteps();
		player.AddFootstep(obj.node);
	}

	private void DoDialogUseConfirm(string name, string text, DungeonObject obj)
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(name, text, null, null, delegate
		{
			OnDialogUseYes(obj);
		}, delegate
		{
			OnDialogUseNo();
		});
	}

	private void OnDialogUseYes(DungeonObject obj)
	{
		DungeonPlayer player = base.focus as DungeonPlayer;
		ActivateObject(player, obj);
	}

	private void OnDialogUseNo()
	{
		CheckAutoPilot();
	}

	private void SetNode(DungeonNode node)
	{
		while (_nodes.Count <= _rows)
		{
			_nodes.Add(new List<DungeonNode>());
		}
		List<DungeonNode> list = _nodes[node.row];
		while (list.Count <= _columns)
		{
			list.Add(null);
		}
		list[node.column] = node;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (base.focus == null || _extension.waiting || _extension.paused)
		{
			return;
		}
		GameData.instance.PROJECT.CheckTutorialChanges();
		SetDirection();
		_target = null;
		_mapCollider.enabled = false;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(GameData.instance.main.mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		_mapCollider.enabled = true;
		if ((bool)raycastHit2D)
		{
			DungeonObject componentInParent = raycastHit2D.collider.GetComponentInParent<DungeonObject>();
			if (componentInParent != null && componentInParent.clickable)
			{
				_target = componentInParent;
				MoveToTile(componentInParent.tile, componentInParent.pathDistance);
				return;
			}
		}
		MoveToMouse();
		_mouseDown = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		EndMouseDown();
	}

	private void EndMouseDown()
	{
		if (_mouseDown)
		{
			_mouseDown = false;
			_mouseTile = null;
		}
	}

	private bool CheckDirectionMovement()
	{
		if (_direction.x == 0f && _direction.y == 0f)
		{
			return false;
		}
		if (base.focus == null || _extension.waiting || _extension.paused)
		{
			return false;
		}
		if (_target != null || _mouseDown)
		{
			return false;
		}
		if (GameData.instance.windowGenerator.GetDialogCountWithout(typeof(DungeonUI)) > 0)
		{
			return false;
		}
		Tile tileFromPoint = getTileFromPoint(base.focus.x, base.focus.y);
		if (tileFromPoint == null)
		{
			return false;
		}
		Tile tileByPosition = getTileByPosition(tileFromPoint.xPos + (int)_direction.x, tileFromPoint.yPos + (int)_direction.y);
		if (tileByPosition == null)
		{
			return false;
		}
		if (_directionTile != null && tileByPosition.id == _directionTile.id)
		{
			return false;
		}
		DungeonNode nodeByPoint = GetNodeByPoint(new Vector2(tileByPosition.x, tileByPosition.y));
		if (nodeByPoint == null)
		{
			return false;
		}
		if (nodeByPoint.obj != null && !nodeByPoint.obj.disabled && !nodeByPoint.obj.instant && nodeByPoint.obj.tile == tileByPosition)
		{
			ExecuteObject(nodeByPoint.obj);
			return true;
		}
		if (!tileByPosition.walkable)
		{
			return false;
		}
		_directionTile = tileByPosition;
		MoveToTile(tileByPosition, 0, indicator: false);
		return true;
	}

	private bool CheckMouseMovement()
	{
		if (base.focus == null || _extension.waiting || _extension.paused)
		{
			return false;
		}
		if (!_mouseDown)
		{
			return false;
		}
		Tile tileFromMouse = getTileFromMouse();
		if (tileFromMouse == null)
		{
			return false;
		}
		if (_mouseTile != null && tileFromMouse.id == _mouseTile.id)
		{
			return false;
		}
		_mouseTile = tileFromMouse;
		MoveToTile(_mouseTile, 0, indicator: false);
		return true;
	}

	private void MoveToMouse()
	{
		if (!(base.focus == null) && !_extension.waiting && !_extension.paused)
		{
			Tile tileFromMouse = getTileFromMouse();
			if (tileFromMouse != null)
			{
				MoveToTile(tileFromMouse);
			}
		}
	}

	public void MoveToTile(Tile tile, int distance = 0, bool indicator = true, bool controlled = true)
	{
		if (tile == null)
		{
			return;
		}
		_ = base.focus;
		List<Tile> list = null;
		if (!tile.walkable)
		{
			tile = GetNearestWalkableTile(tile);
			if (tile != null)
			{
				list = GeneratePath(base.focus, tile);
			}
		}
		else
		{
			list = GeneratePath(base.focus, tile);
		}
		if (list != null)
		{
			while (list.Count > 0 && distance > 0)
			{
				list.RemoveAt(list.Count - 1);
				distance--;
			}
		}
		if (list != null && list.Count > 0)
		{
			Tile tile2 = list[list.Count - 1];
			Tile targetTile = base.focus.GetTargetTile();
			if (targetTile == null || targetTile.id != tile2.id)
			{
				Vector2 position = tile2.GetPosition(base.focus);
				if (indicator)
				{
					UnityEngine.Object.Instantiate(animClickPoint).transform.position = new Vector3(position.x, position.y, 0f);
				}
				if (controlled && GameData.instance.PROJECT.character.autoPilot)
				{
					GameData.instance.PROJECT.character.autoPilot = false;
					CharacterDALC.instance.doSaveConfig(GameData.instance.PROJECT.character);
				}
				base.focus.SetPath(list);
			}
		}
		else if ((bool)_target)
		{
			if (_target.instant)
			{
				CheckPlayerTile();
				return;
			}
			ExecuteObject(_target);
			_target = null;
		}
	}

	private List<DungeonNode> CondensePath(List<DungeonNode> path)
	{
		for (int i = 0; i < path.Count; i++)
		{
			DungeonNode dungeonNode = path[i];
			if (i > 0)
			{
				_ = path[i - 1];
			}
			DungeonNode targetNode = ((i > 1) ? path[i - 2] : null);
			if (dungeonNode.NodeIsConnected(targetNode))
			{
				path.RemoveAt(i - 1);
				i--;
			}
		}
		return path;
	}

	private bool AllowAdd(DungeonNode targetNode, DungeonNode startNode, DungeonNode endNode, bool empty = false)
	{
		if (targetNode.GetInstanceID() == startNode.GetInstanceID())
		{
			return false;
		}
		if (empty && !targetNode.empty && targetNode.GetInstanceID() != endNode.GetInstanceID())
		{
			return false;
		}
		return true;
	}

	private void DoToggleAutoPilot()
	{
		MenuInterfaceAutoPilotTile menuInterfaceAutoPilotTile = GameData.instance.windowGenerator.GetBattleUI(typeof(MenuInterfaceAutoPilotTile)) as MenuInterfaceAutoPilotTile;
		if (menuInterfaceAutoPilotTile.available && !menuInterfaceAutoPilotTile.grayscale)
		{
			GameData.instance.PROJECT.character.autoPilot = !GameData.instance.PROJECT.character.autoPilot;
		}
	}

	private List<DungeonNode> GetShortestPath(DungeonNode startNode, DungeonNode endNode, bool empty = false)
	{
		if (startNode.GetInstanceID() == endNode.GetInstanceID())
		{
			return null;
		}
		List<List<DungeonNode>> list = new List<List<DungeonNode>>();
		List<DungeonNode> list2 = null;
		List<DungeonNode> list3 = new List<DungeonNode>();
		list3.Add(startNode);
		list.Add(list3);
		while (list.Count > 0)
		{
			List<DungeonNode> list4 = list[0];
			DungeonNode dungeonNode = list4[list4.Count - 1];
			DungeonNode dungeonNode2 = ((list4.Count > 1) ? list4[list4.Count - 2] : null);
			if (dungeonNode.GetInstanceID() == endNode.GetInstanceID())
			{
				list4 = CondensePath(list4);
				if (list2 == null || list4.Count < list2.Count)
				{
					list2 = list4;
				}
				list.RemoveAt(0);
				continue;
			}
			DungeonNode node = GetNode(dungeonNode.row - 1, dungeonNode.column);
			DungeonNode node2 = GetNode(dungeonNode.row + 1, dungeonNode.column);
			DungeonNode node3 = GetNode(dungeonNode.row, dungeonNode.column - 1);
			DungeonNode node4 = GetNode(dungeonNode.row, dungeonNode.column + 1);
			if (dungeonNode.points == 1 && dungeonNode2 == null)
			{
				if (dungeonNode.up && AllowAdd(node, startNode, endNode, empty))
				{
					list4.Add(node);
				}
				if (dungeonNode.down && AllowAdd(node2, startNode, endNode, empty))
				{
					list4.Add(node2);
				}
				if (dungeonNode.left && AllowAdd(node3, startNode, endNode, empty))
				{
					list4.Add(node3);
				}
				if (dungeonNode.right && AllowAdd(node4, startNode, endNode, empty))
				{
					list4.Add(node4);
				}
			}
			else if (dungeonNode.points > 1)
			{
				if (dungeonNode.up && node != null && !NodeIsInVector(node, list4) && AllowAdd(node, startNode, endNode, empty))
				{
					List<DungeonNode> list5 = CopyVector(list4);
					list5.Add(node);
					list.Add(list5);
				}
				if (dungeonNode.down && node2 != null && !NodeIsInVector(node2, list4) && AllowAdd(node2, startNode, endNode, empty))
				{
					List<DungeonNode> list6 = CopyVector(list4);
					list6.Add(node2);
					list.Add(list6);
				}
				if (dungeonNode.left && node3 != null && !NodeIsInVector(node3, list4) && AllowAdd(node3, startNode, endNode, empty))
				{
					List<DungeonNode> list7 = CopyVector(list4);
					list7.Add(node3);
					list.Add(list7);
				}
				if (dungeonNode.right && node4 != null && !NodeIsInVector(node4, list4) && AllowAdd(node4, startNode, endNode, empty))
				{
					List<DungeonNode> list8 = CopyVector(list4);
					list8.Add(node4);
					list.Add(list8);
				}
				list.RemoveAt(0);
			}
			else
			{
				list.RemoveAt(0);
			}
		}
		return list2;
	}

	public void UpdateVisibility()
	{
		foreach (DungeonNode node in _nodeList)
		{
			if (node == null)
			{
				break;
			}
			if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(GameData.instance.main.mainCamera), node.fog.GetComponent<SpriteRenderer>().bounds))
			{
				node.SetFogVisibility(v: true);
			}
			else
			{
				node.SetFogVisibility(v: false);
			}
		}
	}

	private bool NodeIsInVector(DungeonNode node, List<DungeonNode> vector)
	{
		foreach (DungeonNode item in vector)
		{
			if (item.GetInstanceID() == node.GetInstanceID())
			{
				return true;
			}
		}
		return false;
	}

	public List<DungeonNode> GetNodeVisible()
	{
		List<DungeonNode> list = new List<DungeonNode>();
		foreach (DungeonNode node in _nodeList)
		{
			if (node.vis)
			{
				list.Add(node);
			}
		}
		return list;
	}

	public DungeonNode GetNode(int row, int column)
	{
		if (row >= _rows || row < 0 || column >= _columns || column < 0)
		{
			return null;
		}
		return _nodes[row][column];
	}

	public DungeonNode GetNodeByPosition(int xPos, int yPos)
	{
		if (xPos < 0)
		{
			return null;
		}
		if (xPos >= _columns)
		{
			return null;
		}
		if (yPos < 0)
		{
			return null;
		}
		if (yPos >= _rows)
		{
			return null;
		}
		return _nodes[yPos][xPos];
	}

	public DungeonNode GetNodeByPoint(Vector2 point)
	{
		int xPos = Mathf.RoundToInt((point.x - 140f) / 280f);
		int value = Mathf.RoundToInt((point.y + 140f) / 280f);
		return GetNodeByPosition(xPos, Mathf.Abs(value));
	}

	public DungeonNode GetNodeByMouse()
	{
		return GetNodeByPoint(Input.mousePosition);
	}

	public DungeonNode GetObjectNode(GameObject obj)
	{
		return GetNodeByPoint(new Vector2(obj.transform.position.x, obj.transform.position.y));
	}

	public DungeonPlayer GetPlayer(int charID)
	{
		foreach (DungeonPlayer player in _players)
		{
			if (player.charID == charID)
			{
				return player;
			}
		}
		return null;
	}

	private int GetHighestPlayerTeammateStats()
	{
		int num = 0;
		DungeonPlayer player = GetPlayer(GameData.instance.PROJECT.character.id);
		if (player == null)
		{
			return num;
		}
		foreach (DungeonEntity entity in player.entities)
		{
			if (entity.type == 1 && entity.id != GameData.instance.PROJECT.character.id)
			{
				int totalStats = entity.characterData.getTotalStats();
				if (totalStats > num)
				{
					num = totalStats;
				}
			}
		}
		return num;
	}

	private int GetEventID()
	{
		if (_eventRef == null)
		{
			return 0;
		}
		return _eventRef.id;
	}

	public void RemovePlayer(int charID)
	{
		for (int i = 0; i < _players.Count; i++)
		{
			DungeonPlayer dungeonPlayer = _players[i];
			if (dungeonPlayer.charID == charID)
			{
				_players.RemoveAt(i);
				UnityEngine.Object.Destroy(dungeonPlayer.gameObject);
				break;
			}
		}
	}

	public List<DungeonNode> CopyVector(List<DungeonNode> vector)
	{
		List<DungeonNode> list = new List<DungeonNode>();
		list.AddRange(vector);
		return list;
	}

	public void ReloadAssets()
	{
		if (_asset != null)
		{
			_asset = null;
		}
		LoadAsset();
	}

	public void TrackStart()
	{
		if (!_trackedStart)
		{
			_trackedStart = true;
			ZoneNodeDifficultyRef dungeonZoneNodeDifficultyRef = ZoneBook.GetDungeonZoneNodeDifficultyRef(_dungeonRef);
			int difficulty = dungeonZoneNodeDifficultyRef?.difficultyRef.id ?? (-1);
			KongregateAnalytics.trackPlayStarts(_uid, GetTypeIdentifier(_type), 0, difficulty, dungeonZoneNodeDifficultyRef);
		}
	}

	public void TrackEnd(string type)
	{
		if (!_trackedEnd)
		{
			_trackedEnd = true;
			int difficulty = ZoneBook.GetDungeonZoneNodeDifficultyRef(_dungeonRef)?.difficultyRef.id ?? (-1);
			int pveEnergyChange = _pveEnergyStart - ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.energy : GameData.instance.SAVE_STATE.characterEnergy);
			int pvpEnergyChange = _pvpEnergyStart - ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.tickets : GameData.instance.SAVE_STATE.characterTickets);
			KongregateAnalytics.trackPlayEnds(_uid, GetTypeIdentifier(_type), type, _creditsGained, _goldGained, pveEnergyChange, pvpEnergyChange, GetHighestPlayerTeammateStats(), 0, 0, difficulty, ZoneBook.GetDungeonZoneNodeDifficultyRef(_dungeonRef), GetEventID());
			KongregateAnalytics.trackEconomyTransaction(KongregateAnalytics.getDungeonEconomyType(this), "", _creditsGained, _goldGained, KongregateAnalytics.getDungeonEconomyContext(this), 2);
		}
	}

	public void AddCreditsGained(int gain)
	{
		_creditsGained += gain;
	}

	public void AddGoldGained(int gain)
	{
		_goldGained += gain;
	}

	public void ShowDungeon(bool enabled)
	{
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(enabled);
		}
		if (_dungeonUI != null)
		{
			if (_dungeonUI.gameObject != null)
			{
				_dungeonUI.gameObject.SetActive(enabled);
			}
			_dungeonUI.ActiveUI(enabled);
		}
		if (_extension != null)
		{
			bool flag = _extension.QueueContains(9);
			if (enabled && GameData.instance.PROJECT.character.autoPilot && !IsCleared() && !flag)
			{
				GameData.instance.PROJECT.character.autoPilot = false;
				StartCoroutine(EnableAutoPilot());
			}
		}
		foreach (DungeonPlayer player in _players)
		{
			if (!(player == null))
			{
				if (player.gameObject != null)
				{
					player.gameObject.SetActive(enabled);
				}
				player.ShowFollowers(enabled);
			}
		}
		foreach (List<DungeonNode> node in _nodes)
		{
			if (node == null)
			{
				continue;
			}
			foreach (DungeonNode item in node)
			{
				if (!(item == null))
				{
					if (item.gameObject != null)
					{
						item.gameObject.SetActive(enabled);
					}
					if (item.obj != null && item.obj.gameObject != null)
					{
						item.obj.transform.localPosition = new Vector3(item.obj.transform.localPosition.x, item.obj.transform.localPosition.y, enabled ? 0f : (-150000f));
					}
				}
			}
		}
	}

	private IEnumerator EnableAutoPilot()
	{
		yield return new WaitForSeconds(1f);
		GameData.instance.PROJECT.character.autoPilot = true;
		CheckAutoPilot();
	}

	public void AvoidDestruction()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_sceneDestroyed = true;
		if (_dungeonUI != null)
		{
			UnityEngine.Object.Destroy(_dungeonUI.gameObject);
		}
	}

	public void DoDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public override void OnDestroy()
	{
		if (_dungeonUI != null && _dungeonUI.gameObject != null)
		{
			UnityEngine.Object.Destroy(_dungeonUI.gameObject);
		}
		if (_extension != null)
		{
			_extension.Clear();
			_extension.RemoveListener(CustomSFSXEvent.CHANGE, OnExtensionChange);
			_extension = null;
		}
		if (GameData.instance != null)
		{
			if (GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null)
			{
				GameData.instance.PROJECT.character.RemoveListener("AUTO_PILOT_CHANGE", OnAutoPilotChange);
				GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
			}
			if (GameData.instance.main != null && GameData.instance.main.DISPATCHER != null && GameData.instance.main.DISPATCHER.FRAME_UPDATE != null)
			{
				GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnFrameUpdate);
			}
		}
	}

	public static DungeonRef DungeonRefFromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("dun15");
		GameData.instance.main.logManager.AddBreadcrumb($"Dungeon::onEnterDungeon::ID:{@int}");
		return DungeonBook.Lookup(@int);
	}

	public static Dungeon FromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("roo1");
		sfsob.GetInt("roo3");
		int int2 = sfsob.GetInt("dun15");
		int int3 = sfsob.GetInt("dun16");
		int int4 = sfsob.GetInt("dun11");
		int int5 = sfsob.GetInt("dun12");
		Dungeon component = GameData.instance.windowGenerator.GetFromResources("ui/dungeon/" + typeof(Dungeon).Name).GetComponent<Dungeon>();
		component.InitDungeon();
		ISFSArray sFSArray = sfsob.GetSFSArray("dun0");
		List<DungeonNode> list = new List<DungeonNode>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(DungeonNode.FromSFSObject(sFSObject));
		}
		ISFSArray sFSArray2 = sfsob.GetSFSArray("dun7");
		List<DungeonPlayer> list2 = new List<DungeonPlayer>();
		for (int j = 0; j < sFSArray2.Size(); j++)
		{
			ISFSObject sFSObject2 = sFSArray2.GetSFSObject(j);
			list2.Add(DungeonPlayer.FromSFSObject(sFSObject2));
		}
		DungeonRef dungeonRef = DungeonBook.Lookup(int2);
		DungeonExtension dungeonExtension = new DungeonExtension(@int);
		object obj = null;
		switch (int3)
		{
		case 1:
			obj = ZoneBook.Lookup(sfsob.GetInt("zon0")).getNodeRef(sfsob.GetInt("zon1")).getDifficultyRef(sfsob.GetInt("zon2"));
			break;
		}
		component.Create(dungeonExtension, dungeonRef, int3, int4, int5, list, list2, obj);
		return component;
	}

	public GameObject GetNodeAsset(string definition)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(_asset.transform.Find(definition).gameObject);
		SpriteRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		if (componentsInChildren.Length != 0)
		{
			SpriteRenderer[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].sortingOrder -= 5000;
			}
		}
		gameObject.name = definition;
		return gameObject;
	}

	public static string GetTypeIdentifier(int type)
	{
		if (TYPE_IDENTIFIER.ContainsKey(type))
		{
			return TYPE_IDENTIFIER[type];
		}
		return null;
	}

	public void StatLoudout()
	{
		_pveEnergyStart = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.energy : GameData.instance.SAVE_STATE.characterEnergy);
		_pvpEnergyStart = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.tickets : GameData.instance.SAVE_STATE.characterTickets);
		KongregateAnalyticsSchema.LoadOutStats loadOutStats = new KongregateAnalyticsSchema.LoadOutStats();
		loadOutStats.mode = type;
		loadOutStats.loadout_context = "Dungeon_" + GetTypeIdentifier(_type);
		loadOutStats.settings = GameData.instance.GetLoadOut();
		int num = 0;
		foreach (DungeonPlayer player in _players)
		{
			if (player.charID != GameData.instance.PROJECT.character.id)
			{
				continue;
			}
			for (int i = 0; i < player.entities.Count; i++)
			{
				switch (player.entities[i].type)
				{
				case 3:
					loadOutStats.familiars.Add(player.entities[i].statEntityFamiliar());
					break;
				case 1:
				{
					if (player.entities[i].characterData.charID != GameData.instance.PROJECT.character.id)
					{
						num++;
						loadOutStats.friends.Add(new KongregateAnalyticsSchema.LoadOutStatsFriend
						{
							index = num,
							stats = player.entities[i].characterData.getTotalStats()
						});
						break;
					}
					loadOutStats.player_total_stats = player.entities[i].characterData.getTotalStats();
					List<KongregateAnalyticsSchema.ItemStat> list = player.entities[i].characterData.runes.statAllRunes();
					if (list.Count > 0)
					{
						loadOutStats.runes = list;
					}
					List<KongregateAnalyticsSchema.ItemStat> gear = player.entities[i].characterData.equipment.statAllEquipement();
					if (list.Count > 0)
					{
						loadOutStats.gear = gear;
					}
					List<Dictionary<string, object>> list2 = GameData.instance.PROJECT.character.inventory.statItems(GameData.instance.PROJECT.character, 4);
					if (list2.Count > 0)
					{
						loadOutStats.boosts = list2;
					}
					break;
				}
				}
			}
		}
		string jsonMap = JsonConvert.SerializeObject(loadOutStats);
		AppInfo.doKongregateAnalyticsEvent("loadouts", jsonMap);
	}

	public EventRef GetEventRef()
	{
		return _eventRef;
	}
}
