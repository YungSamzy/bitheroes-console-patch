using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.events.game;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.leaderboard;
using com.ultrabit.bitheroes.model.news;
using com.ultrabit.bitheroes.model.playervoting;
using com.ultrabit.bitheroes.model.pvp;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.armory;
using com.ultrabit.bitheroes.ui.craft;
using com.ultrabit.bitheroes.ui.daily;
using com.ultrabit.bitheroes.ui.dialog;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.events;
using com.ultrabit.bitheroes.ui.grid;
using com.ultrabit.bitheroes.ui.instance.fishing;
using com.ultrabit.bitheroes.ui.leaderboard;
using com.ultrabit.bitheroes.ui.playervoting;
using com.ultrabit.bitheroes.ui.rune;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace com.ultrabit.bitheroes.ui.instance;

public class Instance : GridMap, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	private static int MOUSE_TIME_LIMIT = 60000;

	public static int OBJECT_DISTANCE = 2;

	public static float OBJECT_SCALE = 3f;

	private static int AUTO_TRAVEL_TYPE = -1;

	private static bool FORCE_OFFERWALL = false;

	public Transform instanceInterfacePrefab;

	public InstancePlayer instancePlayerPrefab;

	public EventStatues eventStatuesPrefab;

	public ArmoryManekin armoryManekinPrefab;

	private InstanceExtension _extension;

	private InstanceRef _instanceRef;

	private object _data;

	private bool _initialized;

	private InstanceInterface _instanceInterface;

	private InstanceGuildHallInterface _instanceGuildHallInterface;

	private DialogWindow _guildHallNotAvailableDialog;

	private InstanceFishingInterface _instanceFishingInterface;

	private InstanceObject _target;

	private InstanceObject _object;

	private List<InstancePlayer> _players = new List<InstancePlayer>();

	private GameObject _asset;

	private PvPEventData _previousPvPEventData;

	private FishingEventData _previousFishingEventData;

	private bool _setWalkable;

	private bool _setSpawn;

	private int _setFootstepID;

	private float _mouseTime;

	private bool _mouseDown;

	private Tile _mouseTile;

	private Vector2 _direction;

	private Tile _directionTile;

	private bool _offerwallChecked;

	private bool _allowMovement = true;

	private int _playerID;

	private BoxCollider2D _mapCollider;

	private Scene _instanceScene;

	private Tile selectedTile;

	private static Instance _instance;

	private InstanceObjectRef objectRefEnterInstance;

	public float guildCount;

	public static Instance instance => _instance;

	public int playerID
	{
		get
		{
			return _playerID;
		}
		set
		{
			_playerID = value;
		}
	}

	public InstanceExtension extension => _extension;

	public InstanceGuildHallInterface instanceGuildHallInterface => _instanceGuildHallInterface;

	public InstanceFishingInterface instanceFishingInterface => _instanceFishingInterface;

	public InstanceInterface instanceInterface => _instanceInterface;

	public object data => _data;

	public InstanceObject obj => _object;

	public PvPEventData previousPvPEventData => _previousPvPEventData;

	public FishingEventData previousFishingEventData => _previousFishingEventData;

	public InstanceRef instanceRef => _instanceRef;

	public Scene instanceScene => _instanceScene;

	public void InitInstance()
	{
		if (_instance == null)
		{
			_instance = this;
		}
	}

	public void Create(InstanceExtension extension, InstanceRef instanceRef, object data, int pplayerID = -1)
	{
		InitInstance();
		_mapCollider = base.gameObject.AddComponent<BoxCollider2D>();
		_mapCollider.size = new Vector2(instanceRef.width / instanceRef.size * instanceRef.size, instanceRef.height / instanceRef.size * instanceRef.size);
		base.transform.position = new Vector2((int)_mapCollider.size.x / 2, -(int)_mapCollider.size.y / 2);
		LoadDetails(instanceRef.width, instanceRef.height, instanceRef.size, lockCamera: true, instanceRef.footstepsDefault);
		_extension = extension;
		_extension.SetInstance(this);
		_instanceRef = instanceRef;
		_playerID = pplayerID;
		if (_playerID != -1)
		{
			CharacterData characterData = null;
			if (GameData.instance.PROJECT.character.getFriendData(_playerID) != null)
			{
				characterData = GameData.instance.PROJECT.character.getFriendData(_playerID).characterData;
			}
			else if (GameData.instance.PROJECT.character.guildData != null && GameData.instance.PROJECT.character.guildData.getMember(_playerID) != null)
			{
				characterData = GameData.instance.PROJECT.character.guildData.getMember(_playerID).characterData;
			}
			if (characterData != null)
			{
				GameData.instance.PROJECT.playerData = characterData;
				GameData.instance.SAVE_STATE.playerID = _playerID;
			}
			else
			{
				GameData.instance.PROJECT.playerData = GameData.instance.PROJECT.character.toCharacterData();
			}
		}
		else
		{
			GameData.instance.PROJECT.playerData = GameData.instance.PROJECT.character.toCharacterData();
		}
		SetTiles();
		SetData(data);
		LoadAssets();
		CreateObjects();
		SetMouse(value: true);
	}

	public bool DoBack()
	{
		if (GameData.instance.tutorialManager.hasPopup)
		{
			return false;
		}
		onBack();
		return true;
	}

	public bool DoForward()
	{
		if (GameData.instance.tutorialManager.hasPopup)
		{
			return false;
		}
		onForward();
		return true;
	}

	public void AddedToStage(Scene instanceScene)
	{
		ClearOfferwallChecked();
		_instanceScene = instanceScene;
		SceneManager.MoveGameObjectToScene(base.gameObject, _instanceScene);
		int count = base.objects.Count;
		int num = 0;
		foreach (GridObject @object in base.objects)
		{
			SceneManager.MoveGameObjectToScene(@object.gameObject, _instanceScene);
			num++;
			if (GameData.instance.PROJECT != null)
			{
				GameData.instance.PROJECT.UpdateTransitionScreenProgress(Mathf.Round((float)count * 100f / (float)num));
			}
		}
		Init();
	}

	private void Init()
	{
		if (_initialized)
		{
			return;
		}
		_initialized = true;
		GameData.instance.main.mainCamera.backgroundColor = Color.black;
		Transform transform = UnityEngine.Object.Instantiate(instanceInterfacePrefab);
		_instanceInterface = transform.GetComponent<InstanceInterface>();
		Main.CONTAINER.AddToLayer(_instanceInterface.gameObject, 2);
		_instanceInterface.LoadDetails(this);
		CheckPopups();
		foreach (InstancePlayer player in _players)
		{
			if (player.isMe)
			{
				SetFocus(player);
				player.MOVEMENT_END.AddListener(OnFocusMovementEnd);
				player.MOVEMENT_CHANGE.AddListener(OnFocusMovementChange);
				UpdateCamera(tween: false);
				break;
			}
		}
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.AddListener(OnFrameUpdate);
		if (AUTO_TRAVEL_TYPE >= 0)
		{
			MoveToObjectRefType(AUTO_TRAVEL_TYPE, random: true, execute: false);
			ResetAutoTravelType();
		}
		if (_instanceRef.type == 4)
		{
			GameData.instance.PROJECT.character.AddListener("armoryEquipmentChange", OnArmoryEquipmentChange);
		}
	}

	private void OnArmoryEquipmentChange()
	{
		UpdateData();
	}

	public void SetData(object data)
	{
		UpdateItemObjects(add: false);
		UpdateObjectLinks(add: false);
		_data = data;
		UpdateData();
	}

	public void UpdateData()
	{
		foreach (InstanceObject @object in base.objects)
		{
			if ((bool)@object)
			{
				@object.UpdateObjectLinks(add: false);
				@object.DoUpdate();
			}
		}
		UpdateItemObjects(add: true);
		if (_instanceGuildHallInterface != null && _instanceGuildHallInterface.editBar != null)
		{
			_instanceGuildHallInterface.editBar.UpdateSelectedType(glow: false);
			_instanceGuildHallInterface.editBar.UpdateSelectedCosmetic();
		}
	}

	public void UpdateItemObjects(bool add)
	{
		if (_data == null || !(_data is GuildHallData))
		{
			return;
		}
		foreach (GuildItemRef item in (_data as GuildHallData).getItems())
		{
			foreach (string value in item.values)
			{
				CheckObjectLink(value, add);
			}
		}
	}

	public void CheckObjectLink(string link, bool add)
	{
		InstanceObject objectByLink = GetObjectByLink(link);
		if (!add)
		{
			if (objectByLink != null)
			{
				RemoveObject(objectByLink);
			}
		}
		else if (!(objectByLink != null))
		{
			InstanceObjectRef objectByLink2 = _instanceRef.getObjectByLink(link);
			if (objectByLink2 != null)
			{
				createObjectRef(objectByLink2);
			}
		}
	}

	public void CheckObjectLinkWithParent(string link, GameObject parentGO)
	{
		if (!(GetObjectByLink(link) != null))
		{
			InstanceObjectRef objectByLink = _instanceRef.getObjectByLink(link);
			if (objectByLink != null)
			{
				createObjectRefWithParent(objectByLink, parentGO);
			}
		}
	}

	public void UpdateObjectLinks(bool add)
	{
		foreach (InstanceObject @object in base.objects)
		{
			if (!(@object == null))
			{
				@object.UpdateObjectLinks(add);
			}
		}
	}

	private void SetTiles()
	{
		int num = 0;
		for (int i = 0; i < base.xCount; i++)
		{
			for (int j = 0; j < base.yCount; j++)
			{
				Tile tileByPosition = getTileByPosition(i, j);
				bool walkable = instanceRef.getWalkable(num);
				tileByPosition.SetWalkable(walkable);
				tileByPosition.SetSpawn(instanceRef.getSpawn(num));
				tileByPosition.SetOffset(_instanceRef.getTileOffset(num));
				tileByPosition.SetFootstep(_instanceRef.getFootstep(num));
				if (!walkable)
				{
					DeleteTile(tileByPosition);
				}
				num++;
			}
		}
	}

	public bool CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null)
		{
			return false;
		}
		if (GameData.instance.windowGenerator.GetDialogCountWithout(typeof(DungeonUI)) > 0 || GameData.instance.tutorialManager.canvas == null)
		{
			return false;
		}
		if (GameData.instance.PROJECT.dungeon != null || GameData.instance.PROJECT.battle != null)
		{
			return false;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(36) && VariableBook.GameRequirementMet(11))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(36);
			if (CheckPopups())
			{
				GameData.instance.PROJECT.CheckTutorialChanges();
				return true;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(38) && VariableBook.GameRequirementMet(12))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(38);
			if (CheckPopups())
			{
				GameData.instance.PROJECT.CheckTutorialChanges();
				return true;
			}
		}
		if (_instanceInterface != null && _instanceInterface.CheckTutorial())
		{
			return true;
		}
		if (AppInfo.allowRatings && !GameData.instance.SAVE_STATE.ratingsSeen && VariableBook.GameRequirementMet(28, def: false) && !VariableBook.GameRequirementMet(29))
		{
			GameData.instance.SAVE_STATE.ratingsSeen = true;
			DoRatingsStartPopup();
			return true;
		}
		if (!_offerwallChecked && AppInfo.DoCheckOfferwallCredits(FORCE_OFFERWALL))
		{
			_offerwallChecked = true;
			FORCE_OFFERWALL = false;
		}
		if (GameData.instance.PROJECT.checkPreRegistration())
		{
			return true;
		}
		if (_instanceRef.type == 3 && GameData.instance.PROJECT.DoDailyFishingRewardCheck())
		{
			return true;
		}
		return false;
	}

	private void DoRatingsStartPopup()
	{
		GameData.instance.windowGenerator.NewPromptMessageWindowBig(Language.GetString("game_name"), Language.GetString("feedback_question", new string[1] { Language.GetString("game_name") }, color: true), null, null, delegate
		{
			DoRatingsFinishPopup(Language.GetString("feedback_question_yes_confirm"), flipped: false);
		}, delegate
		{
			DoRatingsFinishPopup(Language.GetString("feedback_question_no_confirm"), flipped: true);
		});
	}

	private void DoRatingsFinishPopup(string desc, bool flipped)
	{
		if (!flipped)
		{
			GameData.instance.windowGenerator.NewPromptMessageWindowBig(Language.GetString("game_name"), desc, Language.GetString("ui_rate"), Language.GetString("ui_feedback"), delegate
			{
				OnRatingsFinishPopupYes();
			}, delegate
			{
				OnRatingsFinishPopupNo();
			});
		}
		else
		{
			GameData.instance.windowGenerator.NewPromptMessageWindowBig(Language.GetString("game_name"), desc, Language.GetString("ui_feedback"), Language.GetString("ui_rate"), delegate
			{
				OnRatingsFinishPopupNo();
			}, delegate
			{
				OnRatingsFinishPopupYes();
			});
		}
	}

	private void OnRatingsFinishPopupYes()
	{
		GameData.instance.windowGenerator.ShowPlatform();
	}

	private void OnRatingsFinishPopupNo()
	{
		GameData.instance.windowGenerator.ShowForums();
	}

	private void DoNBPDialog()
	{
		DialogRef dialogRef = DialogBook.Lookup(VariableBook.nbpDialog);
		if (dialogRef != null)
		{
			GameData.instance.windowGenerator.NewDialogPopup(dialogRef).CLEAR.AddListener(OnNBPDialog);
		}
		else
		{
			GameData.instance.windowGenerator.ShowNBP();
		}
	}

	private void OnNBPDialog(object e)
	{
		(e as DialogPopup).CLEAR.RemoveListener(OnNBPDialog);
		GameData.instance.windowGenerator.ShowNBP();
	}

	private void OnPopupClosed()
	{
		CheckPopups();
	}

	public bool CheckPopups()
	{
		if (GameData.instance.PROJECT.character == null)
		{
			return false;
		}
		if (GameData.instance.PROJECT.IsNFTInFrozenCache(GameData.instance.PROJECT.character))
		{
			GameData.instance.windowGenerator.NewHeroSelectWindow(null, showCloseBtn: false, forceRelog: true);
			return true;
		}
		if (GameData.instance.tutorialManager == null)
		{
			return false;
		}
		if (GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null)
		{
			return false;
		}
		foreach (int characterBaseForceConsumableId in VariableBook.CharacterBaseForceConsumableIds)
		{
			DoCheckForcedConsumablesById(characterBaseForceConsumableId);
		}
		string dailyRewardStep = GameData.instance.PROJECT.character.extraInfo.getDailyRewardStep();
		if (GameData.instance.PROJECT.character.tutorial.GetState(36) && DailyRewardBook.Lookup(GameData.instance.PROJECT.character.dailyID + 1, dailyRewardStep) != null)
		{
			DateTime date = ServerExtension.instance.GetDate();
			DateTime dailyDate = GameData.instance.PROJECT.character.dailyDate;
			if (date.Year > dailyDate.Year || date.Month > dailyDate.Month || date.Day > dailyDate.Day)
			{
				GameData.instance.windowGenerator.NewDailyRewardWindow(delegate
				{
					OnPopupClosed();
				});
				return true;
			}
		}
		if (GameData.instance.PROJECT.character.tutorial.GetState(38))
		{
			if (NewsBook.size > 0 && GameData.instance.SAVE_STATE.newsVersion != NewsBook.VERSION && NewsBook.VIEWED != NewsBook.VERSION)
			{
				NewsBook.SetViewed(NewsBook.VERSION);
				GameData.instance.windowGenerator.NewNewsWindow().DESTROYED.AddListener(delegate
				{
					OnPopupClosed();
				});
				return true;
			}
			if (DoCheckForcedConsumables())
			{
				return true;
			}
			if (DoCheckLoginRewards())
			{
				return true;
			}
		}
		return false;
	}

	private bool DoCheckForcedConsumables()
	{
		foreach (ItemData item in GameData.instance.PROJECT.character.inventory.GetItemsByType(4))
		{
			if (item == null || item.itemRef == null)
			{
				continue;
			}
			ConsumableRef consumableRef = item.itemRef as ConsumableRef;
			if (consumableRef.forceConsume && item.qty > 0)
			{
				ConsumableManager.instance.SetupConsumable(consumableRef, 1);
				ConsumableManager.instance.DoUseConsumable(1, delegate
				{
					CheckPopups();
				});
				return true;
			}
		}
		return false;
	}

	private bool DoCheckForcedConsumablesById(int id)
	{
		ItemData item = GameData.instance.PROJECT.character.inventory.getItem(id, 4);
		if (item == null || item.itemRef == null)
		{
			return false;
		}
		ConsumableRef consumableRef = item.itemRef as ConsumableRef;
		if (consumableRef.forceConsume && item.qty > 0)
		{
			ConsumableManager.instance.SetupConsumable(consumableRef, 1);
			ConsumableManager.instance.DoUseConsumable(1, delegate
			{
				CheckPopups();
			});
			return true;
		}
		return false;
	}

	private bool DoCheckLoginRewards()
	{
		foreach (ItemRewardRef item in VariableBook.GetItemRewardsByType(1))
		{
			if (item.isAvailable)
			{
				ExecuteRewardRef(item);
				return true;
			}
		}
		return false;
	}

	private void onScreenUpdate(GameEvent e)
	{
		throw new Exception("Error --> CONTROL.");
	}

	private void OnFrameUpdate(object e)
	{
		if (!CheckDirectionMovement())
		{
			CheckMouseMovement();
		}
	}

	private void OnRightMouseDown()
	{
		Tile tileFromMouse = getTileFromMouse();
		if (tileFromMouse != null)
		{
			_setWalkable = !tileFromMouse.walkable;
			_setSpawn = !tileFromMouse.spawn;
			bool ctrl = false;
			if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
			{
				ctrl = true;
			}
			if (Input.GetKeyDown(KeyCode.RightCommand) || Input.GetKeyDown(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand) || Input.GetKey(KeyCode.LeftCommand))
			{
				ctrl = true;
			}
			OnRightMouseTrigger(ctrl);
		}
	}

	private void onRightMouseUp(MouseEvent e)
	{
		throw new Exception("Error --> CONTROL.");
	}

	private void onRightMouseMove(MouseEvent e = null)
	{
		throw new Exception("Error --> CONTROL.");
	}

	private void OnRightMouseTrigger(bool ctrl)
	{
		Tile tileFromMouse = getTileFromMouse();
		if (tileFromMouse != null)
		{
			if (ctrl)
			{
				tileFromMouse.SetSpawn(_setSpawn);
			}
			else
			{
				tileFromMouse.SetWalkable(_setWalkable);
			}
		}
	}

	private void OnFocusMovementEnd(object e)
	{
		if ((bool)_target)
		{
			bool flag = false;
			if (_target.objectRef != null && _target.objectRef.type == 26)
			{
				flag = true;
			}
			if (flag || !GameData.instance.windowGenerator.hasPopup)
			{
				ExecuteObject(_target);
			}
		}
	}

	private void OnFocusMovementChange(object e)
	{
	}

	private bool CheckDirectionMovement()
	{
		if (!_allowMovement)
		{
			return false;
		}
		if (_direction.x == 0f && _direction.y == 0f)
		{
			return false;
		}
		if (base.focus == null || _target != null || _mouseDown)
		{
			return false;
		}
		if (GameData.instance.windowGenerator.hasPopup)
		{
			return false;
		}
		Vector2 vector = new Vector2(base.focus.x, base.focus.y);
		Tile tile = base.focus.tile;
		if (tile != null)
		{
			vector = new Vector2(vector.x - (tile.offset.HasValue ? tile.offset.Value.x : 0f), vector.y - (tile.offset.HasValue ? (tile.offset.Value.y * -1f) : 0f));
		}
		Tile tileFromPoint = getTileFromPoint(vector.x, vector.y);
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
		if (!tileByPosition.walkable)
		{
			return false;
		}
		_directionTile = tileByPosition;
		moveToTile(tileByPosition, 0, indicator: false);
		return true;
	}

	private bool CheckMouseMovement()
	{
		if (!_allowMovement)
		{
			return false;
		}
		if (!_mouseDown)
		{
			return false;
		}
		if (Time.realtimeSinceStartup * 1000f - _mouseTime >= (float)MOUSE_TIME_LIMIT)
		{
			return false;
		}
		Tile tileFromMouse = getTileFromMouse();
		if (tileFromMouse == _mouseTile)
		{
			return false;
		}
		_mouseTile = tileFromMouse;
		moveToTile(_mouseTile, 0, indicator: false);
		return true;
	}

	public void SetMouse(bool value)
	{
	}

	public void SetMovement(bool value)
	{
		_allowMovement = value;
	}

	private void ExecuteObject(InstanceObject obj)
	{
		if (!(obj == null) && !GameData.instance.PROJECT.guildHallEditMode)
		{
			_object = obj;
			if (obj.data is InstanceObjectRef)
			{
				InstanceObjectRef objectRef = obj.data as InstanceObjectRef;
				ExecuteObjectRef(objectRef);
			}
		}
	}

	public void ExecuteObjectRef(InstanceObjectRef objectRef)
	{
		if (GameData.instance.PROJECT.guildHallEditMode)
		{
			return;
		}
		ItemRewardRef availableReward = objectRef.getAvailableReward();
		if (availableReward != null)
		{
			ExecuteRewardRef(availableReward);
			_target = null;
			return;
		}
		bool flag = false;
		switch (objectRef.type)
		{
		case 35:
			if (CraftBook.GetItemsRevealedByCrafter("armory_forge").Count <= 0)
			{
				flag = true;
			}
			break;
		case 31:
			if (CraftBook.GetItemsRevealedByCrafter("mythic_augment").Count <= 0)
			{
				flag = true;
			}
			break;
		case 32:
			if (CraftBook.GetItemsRevealedByCrafter("mythic_enchant").Count <= 0)
			{
				flag = true;
			}
			break;
		case 33:
			if (CraftBook.GetItemsRevealedByCrafter("mythic_rune").Count <= 0)
			{
				flag = true;
			}
			break;
		}
		if (flag)
		{
			DialogRef blockedDialogRef = objectRef.getBlockedDialogRef();
			if (blockedDialogRef != null)
			{
				GameData.instance.windowGenerator.NewDialogPopup(blockedDialogRef, objectRef, showKongButton: false, -1, setSeen: false);
				_target = null;
				return;
			}
		}
		else
		{
			DialogRef dialogRef = objectRef.getDialogRef();
			if (dialogRef != null && !dialogRef.seen)
			{
				GameData.instance.windowGenerator.NewDialogPopup(dialogRef, objectRef).CLEAR.AddListener(OnExecuteObjectRefDialogClosed);
				_target = null;
				return;
			}
		}
		switch (objectRef.type)
		{
		case 1:
			if (GameData.instance.PROJECT.CheckGameRequirement(0))
			{
				GameData.instance.PROJECT.ShowZoneWindow();
			}
			break;
		case 2:
			if (GameData.instance.PROJECT.CheckGameRequirement(1))
			{
				GameData.instance.PROJECT.ShowPvPWindow();
			}
			break;
		case 3:
			if (GameData.instance.windowGenerator.GetDialogByClass(typeof(LeaderboardWindow)) == null && GameData.instance.PROJECT.CheckGameRequirement(7))
			{
				GameData.instance.windowGenerator.NewLeaderboardWindow();
			}
			break;
		case 4:
			if (_previousPvPEventData != null && _previousPvPEventData.eventRef != null && GameData.instance.windowGenerator.GetDialogByClass(typeof(EventLeaderboardWindow)) == null)
			{
				GameData.instance.windowGenerator.NewEventLeaderboardWindow(1, _previousPvPEventData.eventRef.id, allowSegmented: false);
			}
			break;
		case 5:
			if (GameData.instance.windowGenerator.GetDialogByClass(typeof(CraftWindow)) == null && GameData.instance.PROJECT.CheckGameRequirement(2))
			{
				GameData.instance.windowGenerator.NewCraftWindow();
			}
			break;
		case 6:
			if (GameData.instance.PROJECT.CheckGameRequirement(6))
			{
				GameData.instance.PROJECT.ShowShopWindow();
			}
			break;
		case 7:
			if (GameData.instance.PROJECT.CheckGameRequirement(3))
			{
				GameData.instance.PROJECT.ShowRaidWindow();
			}
			break;
		case 8:
			if (GameData.instance.PROJECT.CheckGameRequirement(17) && GameData.instance.windowGenerator.GetDialogByClass(typeof(DailyQuestsWindow)) == null)
			{
				GameData.instance.windowGenerator.NewDailyQuestsWindow();
			}
			break;
		case 28:
			if (GameData.instance.PROJECT.CheckGameRequirement(37) && GameData.instance.windowGenerator.GetDialogByClass(typeof(PlayerVotingWindow)) == null)
			{
				if (PlayerVotingBook.activeVoting)
				{
					GameData.instance.windowGenerator.NewPlayerVotingWindow();
				}
				else
				{
					GameData.instance.windowGenerator.ShowError(Language.GetString("voting_no_active_voting"));
				}
			}
			break;
		case 9:
			if (GameData.instance.PROJECT.CheckGameRequirement(19))
			{
				GameData.instance.PROJECT.ShowFusionWindow();
			}
			break;
		case 10:
			if (GameData.instance.PROJECT.CheckGameRequirement(20))
			{
				GameData.instance.PROJECT.ShowMountWindow();
			}
			break;
		case 11:
			if (GameData.instance.PROJECT.CheckGameRequirement(21))
			{
				GameData.instance.PROJECT.ShowRiftWindow();
			}
			break;
		case 12:
			if (GameData.instance.PROJECT.CheckGameRequirement(22) && GameData.instance.windowGenerator.GetDialogByClass(typeof(RunesWindow)) == null)
			{
				if (GameData.instance.PROJECT.character.hasRuneOrHad())
				{
					GameData.instance.windowGenerator.NewRunesWindow(GameData.instance.PROJECT.character.runes, changeable: true);
					break;
				}
				GameData.instance.windowGenerator.ShowError(Language.GetString("error_requirement_item_type", new string[2]
				{
					Util.NumberFormat(1f),
					ItemRef.GetItemNamePlural(9)
				}, color: true));
			}
			break;
		case 13:
			if (GameData.instance.PROJECT.CheckGameRequirement(23))
			{
				GameData.instance.PROJECT.ShowGauntletWindow();
			}
			break;
		case 14:
			if (GameData.instance.PROJECT.CheckGameRequirement(25))
			{
				GameData.instance.PROJECT.ShowFishingWindow();
			}
			break;
		case 15:
			if (int.Parse(objectRef.value) == 3 && GameData.instance.PROJECT.character.armory.armoryEquipmentSlots.Count == 0)
			{
				if (!(_guildHallNotAvailableDialog == null))
				{
					break;
				}
				if (GameData.instance.PROJECT.character.level < GameData.instance.PROJECT.armoryMinLevelRequired)
				{
					_guildHallNotAvailableDialog = GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_message"), Language.GetString("ui_armory_locked", new string[1] { GameData.instance.PROJECT.armoryMinLevelRequired.ToString() }), null, delegate
					{
						_guildHallNotAvailableDialog = null;
					});
				}
				else
				{
					_guildHallNotAvailableDialog = GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_message"), Language.GetString("ui_armory_not_set"), null, delegate
					{
						_guildHallNotAvailableDialog = null;
					});
				}
			}
			else if (objectRefEnterInstance == null || !(objectRef.value == objectRef.value))
			{
				objectRefEnterInstance = objectRef;
				if (int.Parse(objectRef.value) == 1)
				{
					WaitAndExecute(InstanceBook.GetFirstInstanceByType(2), transition: false);
				}
				else
				{
					WaitAndExecute(InstanceBook.Lookup(int.Parse(objectRef.value)));
				}
			}
			break;
		case 17:
		{
			DialogRef dialogRef2 = DialogBook.Lookup(objectRef.value);
			if (dialogRef2 != null)
			{
				GameData.instance.windowGenerator.NewDialogPopup(dialogRef2);
			}
			break;
		}
		case 18:
			if (GameData.instance.PROJECT.CheckGameRequirement(31))
			{
				GameData.instance.PROJECT.ShowFamiliarStableWindow();
			}
			break;
		case 19:
			if (GameData.instance.PROJECT.CheckGameRequirement(32))
			{
				GameData.instance.PROJECT.ShowEnchantsWindow();
			}
			break;
		case 20:
			if (GameData.instance.PROJECT.CheckGameRequirement(33))
			{
				GameData.instance.PROJECT.ShowInvasionWindow();
			}
			break;
		case 21:
			if (GameData.instance.PROJECT.CheckGameRequirement(34))
			{
				GameData.instance.PROJECT.ShowBrawlWindow();
			}
			break;
		case 29:
		{
			uint idx2 = uint.Parse(objectRef.value);
			GameData.instance.windowGenerator.NewCharacterArmoryWindow(idx2);
			break;
		}
		case 30:
		{
			uint idx = uint.Parse(objectRef.value);
			GameData.instance.windowGenerator.NewTeammateArmoryWindow(GameData.instance.PROJECT.playerData, (int)idx, showSelect: false);
			break;
		}
		case 23:
			if (_previousFishingEventData != null && _previousFishingEventData.eventRef != null && GameData.instance.windowGenerator.GetDialogByClass(typeof(EventLeaderboardWindow)) == null)
			{
				GameData.instance.windowGenerator.NewEventLeaderboardWindow(6, _previousFishingEventData.eventRef.id);
			}
			break;
		case 24:
			GameData.instance.windowGenerator.NewFishingEventWindow();
			break;
		case 25:
			GameData.instance.PROJECT.ShowFishingShop();
			break;
		case 26:
			if (GameData.instance.PROJECT.CheckGameRequirement(25))
			{
				GameData.instance.PROJECT.ShowFishingMode();
			}
			break;
		case 27:
			if (GameData.instance.PROJECT.CheckGameRequirement(36))
			{
				GameData.instance.PROJECT.ShowAugmentsWindow();
			}
			break;
		case 35:
			if (GameData.instance.windowGenerator.GetDialogByClass(typeof(CraftWindow)) == null && CraftBook.GetItemsByCrafter("armory_forge").Count > 0)
			{
				GameData.instance.windowGenerator.NewCraftTradeWindow(CraftBook.GetItemsByCrafter("armory_forge"));
			}
			break;
		case 31:
			if (GameData.instance.windowGenerator.GetDialogByClass(typeof(CraftWindow)) == null && CraftBook.GetItemsByCrafter("mythic_augment").Count > 0)
			{
				GameData.instance.windowGenerator.NewCraftTradeWindow(CraftBook.GetItemsByCrafter("mythic_augment"));
			}
			break;
		case 32:
			if (GameData.instance.windowGenerator.GetDialogByClass(typeof(CraftWindow)) == null && CraftBook.GetItemsByCrafter("mythic_enchant").Count > 0)
			{
				GameData.instance.windowGenerator.NewCraftTradeWindow(CraftBook.GetItemsByCrafter("mythic_enchant"));
			}
			break;
		case 33:
			if (GameData.instance.windowGenerator.GetDialogByClass(typeof(CraftWindow)) == null && CraftBook.GetItemsByCrafter("mythic_rune").Count > 0)
			{
				GameData.instance.windowGenerator.NewCraftTradeWindow(CraftBook.GetItemsByCrafter("mythic_rune"));
			}
			break;
		case 34:
			if (GameData.instance.windowGenerator.GetDialogByClass(typeof(CraftWindow)) == null && GameData.instance.PROJECT.CheckGameRequirement(2))
			{
				GameData.instance.windowGenerator.NewCraftTradeWindow(CraftBook.GetItemsByCrafter("npc_sets"));
			}
			break;
		case 36:
			GameData.instance.windowGenerator.ShowDialogMessage("???", Language.GetString("npc_set_dead"));
			break;
		case 39:
			GameData.instance.PROJECT.ShowEventSalesShop("Event_Character");
			_instanceInterface.UpdateTiles();
			break;
		case 40:
			if (GameData.instance.windowGenerator.GetDialogByClass(typeof(CraftWindow)) == null && CraftBook.GetItemsByCrafter("event_shop_exchange").Count > 0)
			{
				GameData.instance.windowGenerator.NewCraftTradeWindow(CraftBook.GetItemsByCrafter("event_shop_exchange"));
			}
			break;
		}
		_target = null;
	}

	private void WaitAndExecute(InstanceRef instRef, bool transition = true)
	{
		GameData.instance.PROJECT.DoEnterInstance(instRef, transition);
	}

	private void ExecuteRewardRef(ItemRewardRef rewardRef)
	{
		if (rewardRef.dialog != null)
		{
			GameData.instance.windowGenerator.NewDialogPopup(rewardRef.dialog, rewardRef).CLEAR.AddListener(OnExecuteRewardRef);
		}
		else
		{
			DoItemReward(rewardRef);
		}
	}

	private void OnExecuteObjectRefDialogClosed(object e)
	{
		DialogPopup obj = e as DialogPopup;
		InstanceObjectRef objectRef = obj.data as InstanceObjectRef;
		obj.CLEAR.RemoveListener(OnExecuteObjectRefDialogClosed);
		ExecuteObjectRef(objectRef);
	}

	private void OnExecuteRewardRef(object e)
	{
		DialogPopup obj = e as DialogPopup;
		ItemRewardRef rewardRef = obj.data as ItemRewardRef;
		obj.CLEAR.RemoveListener(OnExecuteRewardRef);
		DoItemReward(rewardRef);
	}

	private void DoItemReward(ItemRewardRef rewardRef)
	{
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(36), OnItemReward);
		CharacterDALC.instance.doItemReward(rewardRef);
	}

	private void OnItemReward(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(36), OnItemReward);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		ConsumableRef consumableRef = null;
		foreach (ItemData item in list)
		{
			if (item.itemRef.itemType == 4)
			{
				ConsumableRef consumableRef2 = item.itemRef as ConsumableRef;
				if (consumableRef2.forceConsume && item.qty > 0)
				{
					consumableRef = consumableRef2;
				}
			}
		}
		GameData.instance.PROJECT.character.addItems(list);
		if (consumableRef != null)
		{
			ConsumableManager.instance.SetupConsumable(consumableRef, 1);
			ConsumableManager.instance.DoUseConsumable(1);
		}
		else if (list.Count > 0)
		{
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true);
		}
	}

	public override void Update()
	{
		base.Update();
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

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			OnRightMouseDown();
		}
		else
		{
			if (!_allowMovement || base.focus == null)
			{
				return;
			}
			Tile tile = null;
			SetDirection();
			if (!GameData.instance.PROJECT.guildHallEditMode)
			{
				_mapCollider.enabled = false;
				RaycastHit2D raycastHit2D = Physics2D.Raycast(GameData.instance.main.mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				_mapCollider.enabled = true;
				if ((bool)raycastHit2D && raycastHit2D.collider != null)
				{
					InstanceObject instanceObject = raycastHit2D.collider.GetComponentInParent<InstanceObject>();
					InstancePlayer componentInParent = raycastHit2D.collider.GetComponentInParent<InstancePlayer>();
					if (instanceObject != null && _target != null && _target.identifier.Equals(instanceObject.identifier))
					{
						return;
					}
					_target = null;
					if (instanceObject != null || componentInParent != null)
					{
						if (componentInParent != null)
						{
							instanceObject = componentInParent;
						}
						switch (instanceObject.type)
						{
						case 2:
							break;
						case 1:
							if (componentInParent.characterData.charID != GameData.instance.PROJECT.character.id)
							{
								GameData.instance.windowGenerator.ShowPlayer(componentInParent.characterData.charID);
							}
							return;
						default:
							return;
						}
						tile = instanceObject.tile;
						_target = instanceObject;
					}
				}
				else
				{
					_target = null;
				}
			}
			else
			{
				_target = null;
			}
			if (tile != null)
			{
				moveToTile(tile, GetDistance(_target), indicator: true, executeIfThere: true);
			}
			else
			{
				moveToMouse();
			}
			if (_target == null)
			{
				_mouseTime = Time.realtimeSinceStartup * 1000f;
				_mouseDown = true;
			}
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_mouseTime = 0f;
		_mouseDown = false;
	}

	private void moveToMouse()
	{
		if (_allowMovement && (bool)base.focus)
		{
			Tile tileFromMouse = getTileFromMouse();
			if (tileFromMouse != null)
			{
				moveToTile(tileFromMouse);
			}
		}
	}

	public void moveToTile(Tile tile, int distance = 0, bool indicator = true, bool executeIfThere = false)
	{
		if (!_allowMovement || tile == null)
		{
			return;
		}
		if (selectedTile != null && tile.id == selectedTile.id)
		{
			if (executeIfThere)
			{
				ExecuteObject(_target);
			}
			return;
		}
		selectedTile = tile;
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
		while (list.Count > 0 && distance > 0)
		{
			list.RemoveAt(list.Count - 1);
			distance--;
		}
		if (list.Count > 0)
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
				_extension.DoPlayerMovement(tile2, base.focus.speedMult);
				base.focus.SetPath(list);
			}
		}
		else if (_target != null)
		{
			ExecuteObject(_target);
		}
	}

	public bool MoveToObjectRefType(int type, bool random = false, bool execute = true, bool closest = false, bool executeIfThere = false)
	{
		InstanceObject instanceObject = (closest ? GetNearestObjectByRefType(base.focus, type) : GetObjectByRefType(type, random));
		if (instanceObject == null || instanceObject.tile == null)
		{
			return false;
		}
		if (execute)
		{
			ExecuteObject(instanceObject);
		}
		else
		{
			_target = instanceObject;
		}
		moveToTile(instanceObject.tile, GetDistance(instanceObject), indicator: true, executeIfThere);
		return true;
	}

	private InstanceObject GetNearestObjectByRefType(GridObject source, int type)
	{
		float num = -1f;
		InstanceObject result = null;
		foreach (InstanceObject @object in base.objects)
		{
			if (!(@object == null) && @object.objectRef != null && @object.objectRef.type == type)
			{
				float distance = Util.GetDistance(source.x, source.y, @object.x, @object.y);
				if (num < 0f || distance < num)
				{
					num = distance;
					result = @object;
				}
			}
		}
		return result;
	}

	private void CreateObjects()
	{
		foreach (InstanceObjectRef @object in _instanceRef.objects)
		{
			if (@object != null && !@object.hidden)
			{
				createObjectRef(@object);
			}
		}
		UpdateObjects(clear: true);
	}

	private void createObjectRefWithParent(InstanceObjectRef objectRef, GameObject parentGO)
	{
		if (objectRef != null && objectRef.getActive() && parentGO.transform.Find(objectRef.definition) != null)
		{
			GameObject gameObject = parentGO.transform.Find(objectRef.definition).gameObject;
			gameObject.SetActive(value: true);
			InstanceObject instanceObject = gameObject.AddComponent<InstanceObject>();
			Tile tileByID = getTileByID(objectRef.tileID);
			instanceObject.SetGlobalData(this, 2, tileByID, objectRef.speed, objectRef.getClickable(), objectRef);
			SortingGroup componentInParent = parentGO.GetComponentInParent<SortingGroup>();
			if (componentInParent != null)
			{
				gameObject.transform.parent = componentInParent.transform.parent;
			}
		}
	}

	public void createObjectRef(InstanceObjectRef objectRef)
	{
		if (objectRef == null || !objectRef.getActive())
		{
			return;
		}
		InstanceObject instanceObject = new GameObject("object" + objectRef.tileID).AddComponent<InstanceObject>();
		Tile tileByID = getTileByID(objectRef.tileID);
		instanceObject.CreateInstanceObject(this, 2, tileByID, objectRef.speed, objectRef.getClickable(), objectRef);
		Scene? scene = null;
		if (_instance != null && _instance.instanceRef != null)
		{
			scene = SceneManager.GetSceneByName(_instance.instanceRef.typeName);
			if (!scene.Value.IsValid())
			{
				scene = null;
			}
		}
		AddObject(instanceObject, scene, updateObjects: false);
	}

	public override void UpdateObjectCollision(GridObject obj, bool add)
	{
		base.UpdateObjectCollision(obj, add);
		if (obj == null || !(obj.data is InstanceObjectRef))
		{
			return;
		}
		foreach (int item in (obj.data as InstanceObjectRef).collision)
		{
			Tile tileByID = getTileByID(item);
			if (tileByID != null)
			{
				if (add)
				{
					tileByID.SetWalkable(walkable: false);
				}
				else if (_instanceRef.getWalkable(item))
				{
					tileByID.SetWalkable(walkable: true);
				}
			}
		}
	}

	private void LoadAssets()
	{
		if (!(_asset != null))
		{
			_asset = new GameObject();
			_asset.transform.SetParent(base.transform);
			_asset.transform.localPosition = Vector3.zero;
			_asset.transform.localScale = new Vector3(OBJECT_SCALE, OBJECT_SCALE, 1f);
			SpriteRenderer spriteRenderer = _asset.AddComponent<SpriteRenderer>();
			spriteRenderer.sortingOrder = -1;
			Sprite spriteAsset = GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.INSTANCE, _instanceRef.asset);
			spriteRenderer.sprite = spriteAsset;
		}
	}

	public void reloadAssets()
	{
	}

	public void AddPlayer(ISFSObject sfsobPlayer)
	{
		if (sfsobPlayer.ContainsKey("cha1") && !(GetPlayer(sfsobPlayer.GetInt("cha1")) != null))
		{
			InstancePlayer instancePlayer = InstancePlayer.FromSFSObject(sfsobPlayer, this);
			_players.Add(instancePlayer);
			_ = _instanceScene;
			if (_instanceScene.IsValid())
			{
				AddObject(instancePlayer, _instanceScene);
			}
			else
			{
				AddObject(instancePlayer);
			}
		}
	}

	public InstancePlayer GetPlayer(int charID)
	{
		foreach (InstancePlayer player in _players)
		{
			if (!(player == null) && player.characterData.charID == charID)
			{
				return player;
			}
		}
		return null;
	}

	public void RemovePlayer(int charID)
	{
		for (int i = 0; i < _players.Count; i++)
		{
			InstancePlayer instancePlayer = _players[i];
			if (instancePlayer.characterData.charID == charID)
			{
				_players.RemoveAt(i);
				RemoveObject(instancePlayer);
				if (instancePlayer.isMe)
				{
					SetFocus(null);
					instancePlayer.MOVEMENT_END.RemoveListener(OnFocusMovementEnd);
				}
				break;
			}
		}
	}

	public InstanceObject GetObjectByLink(string link)
	{
		foreach (InstanceObject @object in base.objects)
		{
			if (!(@object == null) && @object.type == 2)
			{
				InstanceObjectRef instanceObjectRef = @object.data as InstanceObjectRef;
				if (instanceObjectRef.link != null && instanceObjectRef.link.ToLowerInvariant() == link.ToLowerInvariant())
				{
					return @object;
				}
			}
		}
		return null;
	}

	public InstanceObject GetObjectByRefType(int type, bool random = false)
	{
		List<InstanceObject> list = new List<InstanceObject>();
		foreach (InstanceObject @object in base.objects)
		{
			if (!(@object == null) && @object.type == 2 && (@object.data as InstanceObjectRef).type == type)
			{
				if (!random)
				{
					return @object;
				}
				list.Add(@object);
			}
		}
		if (list.Count > 0 && random)
		{
			return list[Util.randomInt(0, list.Count - 1)];
		}
		return null;
	}

	public void SetPreviousPvPEventData(PvPEventData previousPvPEventData)
	{
		_previousPvPEventData = previousPvPEventData;
		if (_previousPvPEventData != null && _previousPvPEventData.leaders != null)
		{
			InstanceObject objectByRefType = GetObjectByRefType(4);
			if (!(objectByRefType == null))
			{
				objectByRefType.LoadAssets();
			}
		}
	}

	public void SetPreviousFishingEventData(FishingEventData previousFishingEventData)
	{
		_previousFishingEventData = previousFishingEventData;
		if (_previousFishingEventData != null && _previousFishingEventData.leaders != null)
		{
			InstanceObject objectByRefType = GetObjectByRefType(23);
			if (!(objectByRefType == null))
			{
				objectByRefType.LoadAssets();
			}
		}
	}

	public void onBack()
	{
		if (_instanceFishingInterface != null)
		{
			_instanceFishingInterface.DoBack();
		}
		else if (AppInfo.IsDesktop() || AppInfo.IsMobile())
		{
			doExitConfirm();
		}
		else
		{
			GameData.instance.PROJECT.doLogoutConfirm();
		}
	}

	public void onForward()
	{
		if (_instanceFishingInterface != null)
		{
			_instanceFishingInterface.DoForward();
		}
	}

	public void doExitConfirm()
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_exit_confirm", new string[1] { Language.GetString("game_name") }), null, null, delegate
		{
			OnExitConfirm();
		});
	}

	private void OnExitConfirm()
	{
		try
		{
			Application.Quit();
		}
		catch (Exception)
		{
		}
	}

	public void SetGuildHallInterface(bool value)
	{
		if (value)
		{
			if (_instanceGuildHallInterface == null)
			{
				_instanceGuildHallInterface = GameData.instance.windowGenerator.NewInstanceGuildHallInterface(this);
			}
			Main.CONTAINER.AddToLayer(_instanceGuildHallInterface.gameObject, 2);
		}
		else if (_instanceGuildHallInterface != null)
		{
			if (_instanceGuildHallInterface.editBar != null)
			{
				_instanceGuildHallInterface.editBar.ClearGlowingObjects();
			}
			UnityEngine.Object.Destroy(_instanceGuildHallInterface.gameObject);
		}
	}

	public void SetFishingInterface(bool value)
	{
		if (value)
		{
			if (_instanceFishingInterface == null)
			{
				_instanceFishingInterface = GameData.instance.windowGenerator.NewInstanceFishingInterface(this, _object);
			}
			Main.CONTAINER.AddToLayer(_instanceFishingInterface.gameObject, 2);
		}
		else if ((bool)_instanceFishingInterface)
		{
			UnityEngine.Object.Destroy(_instanceFishingInterface.gameObject);
			_instanceFishingInterface = null;
		}
	}

	public List<GameObject> GetAssets()
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(_asset);
		foreach (InstanceObject @object in base.objects)
		{
			if ((bool)@object && (bool)@object.asset)
			{
				list.Add(@object.asset);
			}
		}
		return list;
	}

	public void ClearOfferwallChecked()
	{
		_offerwallChecked = false;
	}

	private int GetDistance(InstanceObject obj)
	{
		if (obj == null || obj.objectRef == null)
		{
			return OBJECT_DISTANCE;
		}
		if (obj.objectRef.type == 26)
		{
			return 0;
		}
		return OBJECT_DISTANCE;
	}

	public override void OnDestroy()
	{
		if (_extension != null)
		{
			_extension.Clear();
		}
		_extension = null;
		if (_instanceInterface != null && _instanceInterface.gameObject != null)
		{
			UnityEngine.Object.Destroy(_instanceInterface.gameObject);
		}
		_instanceInterface = null;
		if (_instanceRef.type == 4)
		{
			GameData.instance.PROJECT.character.RemoveListener("armoryEquipmentChange", OnArmoryEquipmentChange);
		}
		_instanceRef = null;
		foreach (InstancePlayer player in _players)
		{
			if (player != null && player.gameObject != null)
			{
				UnityEngine.Object.Destroy(player.gameObject);
			}
		}
		_players = null;
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnFrameUpdate);
		base.OnDestroy();
	}

	public void ShowInstance(bool enabled)
	{
		base.gameObject.SetActive(enabled);
		_instanceInterface.gameObject.SetActive(enabled);
		foreach (GridObject @object in base.objects)
		{
			@object.gameObject.SetActive(enabled);
			if (enabled)
			{
				@object.CheckActions();
			}
		}
	}

	public static InstanceRef GetInstanceRefFromSFSObject(ISFSObject sfsob)
	{
		return InstanceBook.Lookup(sfsob.GetInt("ins0"));
	}

	public static Instance FromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("ins0");
		int int2 = sfsob.GetInt("cha1");
		InstanceRef instanceRef = InstanceBook.Lookup(@int);
		int int3 = sfsob.GetInt("roo1");
		int int4 = sfsob.GetInt("roo3");
		ISFSArray sFSArray = sfsob.GetSFSArray("ins1");
		InstanceExtension instanceExtension = new InstanceExtension(int3, int4);
		object obj = DataFromSFSObject(sfsob, instanceRef);
		if (instanceRef.type == 5 && int2 == -1)
		{
			int2 = GameData.instance.SAVE_STATE.playerID;
			if (int2 == -1)
			{
				GameData.instance.PROJECT.DoEnterInstance(InstanceBook.GetFirstInstanceByType(1));
				return null;
			}
		}
		Instance component = GameData.instance.windowGenerator.GetFromResources("ui/instance/" + typeof(Instance).Name).GetComponent<Instance>();
		component.Create(instanceExtension, instanceRef, obj, int2);
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			component.AddPlayer(sFSObject);
		}
		component.UpdateCamera(tween: false);
		component.SetPreviousPvPEventData(PvPEventData.fromSFSObject(sfsob));
		component.SetPreviousFishingEventData(FishingEventData.fromSFSObject(sfsob));
		return component;
	}

	public static object DataFromSFSObject(ISFSObject sfsob, InstanceRef instanceRef)
	{
		if (instanceRef.type == 2)
		{
			return GuildHallData.fromSFSObject(sfsob);
		}
		return null;
	}

	public static void ResetForceOfferwall()
	{
		FORCE_OFFERWALL = true;
	}

	public static void ResetAutoTravelType()
	{
		AUTO_TRAVEL_TYPE = -1;
	}

	public static void SetAutoTravelType(int type)
	{
		AUTO_TRAVEL_TYPE = type;
	}

	public EventStatues AddEventStatusObject(InstanceObject parent, List<LeaderboardData> leaders, EventRef eventRef)
	{
		EventStatues eventStatues = UnityEngine.Object.Instantiate(eventStatuesPrefab, parent.transform);
		eventStatues.transform.localPosition = Vector3.zero;
		eventStatues.LoadDetails(parent, leaders, eventRef);
		return eventStatues;
	}

	public ArmoryManekin AddArmoryManekinObject(InstanceObject parent, InstanceObjectRef instanceObjRef)
	{
		ArmoryManekin armoryManekin = UnityEngine.Object.Instantiate(armoryManekinPrefab, parent.transform);
		armoryManekin.transform.localPosition = Vector3.zero;
		armoryManekin.LoadDetails(parent, instanceObjRef);
		return armoryManekin;
	}
}
