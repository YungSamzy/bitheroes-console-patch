using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.dungeon;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.gauntlet;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.invasion;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.music;
using com.ultrabit.bitheroes.model.npc;
using com.ultrabit.bitheroes.model.particle;
using com.ultrabit.bitheroes.model.pvp;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.ui.ability;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.familiar;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.menu;
using com.ultrabit.bitheroes.ui.tutorial;
using com.ultrabit.bitheroes.ui.utility;
using com.ultrabit.bitheroes.ui.victory;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Newtonsoft.Json;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.battle;

public class Battle : MonoBehaviour
{
	private static Battle _instance;

	public const int TYPE_NONE = 0;

	public const int TYPE_ZONE = 1;

	public const int TYPE_PVP_EVENT = 2;

	public const int TYPE_PVP_DUEL = 3;

	public const int TYPE_RAID = 4;

	public const int TYPE_RIFT_EVENT = 5;

	public const int TYPE_GAUNTLET_EVENT = 6;

	public const int TYPE_GVG_EVENT = 7;

	public const int TYPE_INVASION_EVENT = 8;

	public const int TYPE_BRAWL = 9;

	public const int TYPE_TEST = 10;

	public const int TYPE_GVE_EVENT = 11;

	private static List<string> TYPE_IDENTIFIER = new List<string>();

	public const string COMPLETE_TYPE_WIN = "Win";

	public const string COMPLETE_TYPE_LOSE = "Lose";

	public const string COMPLETE_TYPE_QUIT = "Quit";

	public const float ASSET_SCALE = 3f;

	public const int ABILITY_TILE_TWEEN_OFFSET = 50;

	public const float ABILITY_TILE_TWEEN_DURATION = 0.25f;

	public const float ABILITY_TILE_TWEEN_DELAY = 0.05f;

	private const int TEXT_OFFSET = 30;

	private const int TEXT_STACK_OFFSET = 75;

	private const float TEXT_SCALE = 0.8f;

	private const float TEXT_SCALE_SMALL = 0.6f;

	private const float TEXT_DISTANCE = -30f;

	public const float ENTITY_DEFAULT_MOVEMENT_SPEED = 0.5f;

	public const int ENTITY_POSITION_OFFSET_X = 200;

	public const int ENTITY_POSITION_OFFSET_Y = -75;

	public const int ENTITY_POSITION_START_OFFSET = 150;

	public const int ENTITY_SPREAD_X = 260;

	public const int ENTITY_SPREAD_Y = -65;

	public const int ENTITY_TURN_OFFSET_X = 80;

	public const int ENTITY_TURN_OFFSET_Y = -70;

	public static readonly int[] ENTITY_POSITIONS = new int[5] { -75, -107, -43, -91, -59 };

	public static readonly float[] BATTLE_SPEEDS = new float[3] { 1f, 3f, 5f };

	public const int ACTION_DELAY = 1;

	public const int ACTION_HEALTH_CHANGE = 2;

	public const int ACTION_METER_CHANGE = 3;

	public const int ACTION_METER_GAIN = 4;

	public const int ACTION_BEGIN = 5;

	public const int ACTION_ABILITY = 6;

	public const int ACTION_TURN_START = 7;

	public const int ACTION_TURN_END = 8;

	public const int ACTION_DEATH_CHANGE = 9;

	public const int ACTION_COMPLETE = 10;

	public const int ACTION_VICTORY = 11;

	public const int ACTION_DEFEAT = 12;

	public const int ACTION_ITEM_USED = 13;

	public const int ACTION_ORDER = 14;

	public const int ACTION_CAPTURE_SET = 15;

	public const int ACTION_CAPTURE_COMPLETE = 16;

	public const int ACTION_ENTITY_VALUES = 17;

	public const int ACTION_ENTITIES_REMOVE = 18;

	public const int ACTION_ENTITIES_ADD = 19;

	public const int ACTION_TEAM_DATA_CHANGE = 20;

	public const int ACTION_RESULTS = 21;

	public const int ACTION_ABILITY_DATA = 22;

	public const int ACTION_STACK_APPLIED = 23;

	public const int ACTION_SKIP_TURN = 24;

	public const int ACTION_RESTORE_TURN = 25;

	public const int ACTION_BLOCK_SKILLS = 26;

	public const int ACTION_BARRIER_DAMAGE = 27;

	public const int ACTION_ROOT_SKILLS = 28;

	public const int ACTION_DEFENSE_IGNORE_SKILLS = 29;

	public const int ACTION_TRIGGER_ABILITY = 30;

	public const int ACTION_BUFF_DAMAGE = 33;

	[Header("Arena")]
	public GameObject arenaContainer;

	public BoxCollider2D backgroundHitbox;

	[Header("Prefabs")]
	public BattleEntity battleEntityPrefab;

	public BattleText battleTextPrefab;

	public Transform battleArenaPrefab;

	private int _id;

	private int _type;

	private bool _active;

	private bool _boss;

	private bool _replay;

	private BattleRules _battleRules;

	private List<BattleEntity> _entities;

	private List<SFSObject> _queue;

	private BattleTeamData _attackerData;

	private BattleTeamData _defenderData;

	private object _data;

	private string _uid;

	private bool _trackedStart;

	private bool _trackedEnd;

	private MusicRef _music;

	private GameObject _asset;

	private bool _started;

	private float _time;

	private float _timeBefore;

	private float _timeAfter;

	private int _currentAction;

	private BattleEntity _currentEntity;

	private List<AbilityTile> _abilityTiles;

	private bool _abilityTween;

	private bool _completed;

	private bool _defeated;

	private bool _results;

	private bool _movement;

	private BattleEntity _waitEntity;

	private UnityAction _waitFunc;

	private bool _realtime;

	private EventRef _eventRef;

	private List<BattleEntity> _captureEntities = new List<BattleEntity>();

	private bool _battleText = true;

	private bool _battleEffects;

	private bool _battleAnimations;

	private bool _autoEnrage;

	private bool _autoPilotDeathDisable;

	private bool _turnTimerStarted;

	private bool _isTimerRunning;

	protected EventDispatcher _dispatcher;

	private bool executeUpdate;

	public Transform playerPuppet;

	private BattleUI _battleUI;

	private List<BattleStat> _battleStats = new List<BattleStat>();

	private ItemSelectWindow itemSelectWindow;

	private Rect _focusBounds;

	private Rect _otherBounds;

	private bool noTargetsOpened;

	private Coroutine _abilityTilesCoroutine;

	private int _pveEnergyStart;

	private int _pvpEnergyStart;

	private TweenerCore<float, float, FloatOptions> tweenAlpha;

	private int key = -1;

	private FamiliarWindow _familiarWindow;

	private CharacterProfileWindow _characterProfileWindow;

	private BattleEntitySelectWindow _selectWindow;

	private int fullCount;

	public bool defeated => _defeated;

	public static Battle instance => _instance;

	public int id => _id;

	public int type => _type;

	public BattleRules battleRules => _battleRules;

	public MusicRef music => _music;

	public bool battleText => _battleText;

	public bool battleAnimations => _battleAnimations;

	public bool realtime => _realtime;

	public BattleUI battleUI
	{
		get
		{
			return _battleUI;
		}
		set
		{
			_battleUI = value;
		}
	}

	public void init()
	{
		if (_instance == null)
		{
			_instance = this;
		}
	}

	private void Awake()
	{
		init();
		_battleText = GameData.instance.SAVE_STATE.battleText;
		_battleAnimations = GameData.instance.SAVE_STATE.animations && GameData.instance.SAVE_STATE.GetAnimationsType(GameData.instance.PROJECT.character.id, 2, GameData.instance.SAVE_STATE.GetAnimationsTypes(GameData.instance.PROJECT.character.id));
		_autoEnrage = GameData.instance.SAVE_STATE.autoEnrage;
		_autoPilotDeathDisable = GameData.instance.SAVE_STATE.autoPilotDeathDisable;
		_battleEffects = !GameData.instance.SAVE_STATE.reducedEffects;
		TYPE_IDENTIFIER.Add("???");
		TYPE_IDENTIFIER.Add("Zone");
		TYPE_IDENTIFIER.Add("PvP");
		TYPE_IDENTIFIER.Add("Duel");
		TYPE_IDENTIFIER.Add("Raid");
		TYPE_IDENTIFIER.Add("Rift");
		TYPE_IDENTIFIER.Add("Gauntlet");
		TYPE_IDENTIFIER.Add("GvG");
		TYPE_IDENTIFIER.Add("Invasion");
		TYPE_IDENTIFIER.Add("Brawl");
		TYPE_IDENTIFIER.Add("Test");
		TYPE_IDENTIFIER.Add("GvE");
		_uid = Util.RandomString();
	}

	public void LoadDetails(int id, int type, bool active, bool boss, bool replay, BattleRules battleRules, List<BattleEntity> entities, List<SFSObject> queue, BattleTeamData attackerData, BattleTeamData defenderData, object data)
	{
		_id = id;
		_type = type;
		_active = active;
		_boss = boss;
		_replay = replay;
		_battleRules = battleRules;
		_entities = entities;
		_queue = queue;
		_attackerData = attackerData;
		_defenderData = defenderData;
		_data = data;
		if (MustSaveBattleStats())
		{
			for (int i = 0; i < entities.Count; i++)
			{
				BattleEntity battleEntity = entities[i];
				if (!(battleEntity != null))
				{
					continue;
				}
				int num = battleEntity.id;
				int num2 = ((battleEntity.type == 1) ? 1 : 2);
				if (battleEntity.type == 2)
				{
					NPCRef nPCRef = NPCBook.Lookup(battleEntity.id);
					num = FamiliarBook.GetFamiliarIdFromNPCLink(nPCRef.link);
					if (num == 0)
					{
						num = nPCRef.familiar;
					}
				}
				while (_battleStats.Count <= battleEntity.index)
				{
					_battleStats.Add(null);
				}
				_battleStats[battleEntity.index] = new BattleStat(num, num2, battleEntity.index, battleEntity.attacker);
			}
		}
		backgroundHitbox.size = new Vector2(Main.BOUNDS.width, Main.BOUNDS.height);
		GameData.instance.windowGenerator.NewBattleUI(this);
		_battleUI.LoadMenu();
		_battleUI.Show(show: false);
		BattleDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnQueue);
		GameData.instance.PROJECT.character.AddListener("AUTO_PILOT_CHANGE", OnAutoPilotChange);
		LoadAssets();
		_instance.SetEntityFocus();
		_instance.SetEntityPositions(_entities);
		_battleUI.LoadAditionalSettings();
		_dispatcher = new EventDispatcher();
		GameData.instance.tutorialManager.ClearTutorial();
		StatLoudout();
	}

	private bool MustSaveBattleStats()
	{
		switch (_type)
		{
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 9:
		case 11:
			return true;
		default:
			return false;
		}
	}

	public bool DoBack()
	{
		if (GameData.instance.tutorialManager.hasPopup || _battleUI.disabled || _battleUI.scrollingIn || _battleUI.scrollingOut)
		{
			return false;
		}
		DoExitConfirm();
		return true;
	}

	public bool DoForward()
	{
		if (GameData.instance.tutorialManager.hasPopup || _battleUI.disabled || _battleUI.scrollingIn || _battleUI.scrollingOut || GameData.instance.windowGenerator.HasDialogByClass(typeof(VictoryWindow)))
		{
			return false;
		}
		DoToggleAutoPilot();
		return true;
	}

	public void MoveToScene()
	{
		SceneManager.MoveGameObjectToScene(base.gameObject, SceneManager.GetSceneByName("Battle"));
		if (_asset != null)
		{
			_asset.gameObject.SetActive(value: true);
			PlayAssetAnimation("walk");
		}
	}

	private void OnAssetLoaded()
	{
		PlayAssetAnimation("idle");
	}

	private void OnUpdate(object e)
	{
		UpdateEntityParents();
	}

	public void UpdateEntityParents()
	{
		List<BattleEntity> list = new List<BattleEntity>();
		List<BattleEntity> list2 = new List<BattleEntity>();
		List<BattleEntity> list3 = Util.SortVector(_entities, new string[1] { "y" }, Util.ARRAY_DESCENDING);
		for (int i = 0; i < list3.Count; i++)
		{
			BattleEntity battleEntity = list3[i];
			UpdateEntityParent(battleEntity, i + 1);
			if (battleEntity.classParent != null && battleEntity.classParent == typeof(BattleEntitySelectWindow))
			{
				list2.Add(battleEntity);
			}
			if (battleEntity.highlighted)
			{
				list.Add(battleEntity);
			}
		}
		foreach (BattleEntity item in list)
		{
			UpdateEntityParent(item, 20);
		}
		UpdateEntityOverlays(list3);
		if (list2.Count <= 0)
		{
			return;
		}
		foreach (BattleEntity item2 in list2)
		{
			UpdateEntityParentInParentClass(item2);
		}
	}

	public void UpdateEntityOverlays(List<BattleEntity> entities)
	{
		foreach (BattleEntity entity in entities)
		{
			entity.overlay.Add();
		}
	}

	private void UpdateEntityParentInParentClass(BattleEntity entity)
	{
		BattleEntitySelectWindow battleEntitySelectWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(BattleEntitySelectWindow)) as BattleEntitySelectWindow;
		if (battleEntitySelectWindow != null && entity.asset != null)
		{
			SortingGroup sortingGroup = entity.asset.GetComponent<SortingGroup>();
			if (sortingGroup == null)
			{
				sortingGroup = entity.asset.AddComponent<SortingGroup>();
			}
			SortingGroup sortingGroup2 = entity.overlay.gameObject.GetComponent<SortingGroup>();
			if (sortingGroup2 == null)
			{
				sortingGroup2 = entity.overlay.gameObject.AddComponent<SortingGroup>();
			}
			int sortingOrder = sortingGroup.sortingOrder;
			sortingOrder = ((!entity.highlighted) ? battleEntitySelectWindow.GetPositionByID(entity.highlightID) : battleEntitySelectWindow.GetHighlightPosition());
			if (sortingGroup.enabled)
			{
				sortingGroup.sortingOrder = sortingOrder;
			}
			if (sortingGroup2.enabled)
			{
				sortingGroup2.sortingOrder = sortingOrder;
				sortingGroup2.sortingLayerName = sortingGroup.sortingLayerName;
			}
		}
	}

	private void UpdateEntityParent(BattleEntity entity, int pos)
	{
		if (entity.classParent == null && entity.asset != null)
		{
			SortingGroup sortingGroup = entity.asset.GetComponent<SortingGroup>();
			if (sortingGroup == null)
			{
				sortingGroup = entity.asset.AddComponent<SortingGroup>();
			}
			SortingGroup sortingGroup2 = entity.overlay.gameObject.GetComponent<SortingGroup>();
			if (sortingGroup2 == null)
			{
				sortingGroup2 = entity.overlay.gameObject.AddComponent<SortingGroup>();
			}
			if (sortingGroup.enabled)
			{
				sortingGroup.sortingOrder = pos;
			}
			if (sortingGroup2.enabled)
			{
				sortingGroup2.sortingOrder = pos;
				sortingGroup2.sortingLayerName = sortingGroup.sortingLayerName;
			}
		}
	}

	private void OnQueue(BaseEvent e)
	{
		SFSObject sfsob = (e as DALCEvent).sfsob;
		bool flag = _queue.Count <= 0;
		foreach (SFSObject item in GetQueueFromSFSObject(sfsob))
		{
			_queue.Add(item);
		}
		Debug.Log("OnQueue Event ----> Count = " + _queue.Count + "  ---- start = " + flag);
		if (flag)
		{
			RunQueue();
		}
		else
		{
			Debug.Log("NOT RUN QUEUE !!!!");
		}
	}

	private void OnAutoPilotChange()
	{
		if (!GetLocked() && GameData.instance.PROJECT.character.autoPilot)
		{
			ClearWindows();
			CheckAutoPilot();
		}
	}

	public void ClearWindows()
	{
		BattleEntitySelectWindow battleEntitySelectWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(BattleEntitySelectWindow)) as BattleEntitySelectWindow;
		if (battleEntitySelectWindow != null)
		{
			battleEntitySelectWindow.DoDestroy();
		}
		ItemSelectWindow itemSelectWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(ItemSelectWindow)) as ItemSelectWindow;
		if (itemSelectWindow != null && !itemSelectWindow.scrollingOut)
		{
			itemSelectWindow.DoDestroy();
		}
	}

	private void PlayAssetAnimation(string label, bool loop = false)
	{
		if (!(_asset == null))
		{
			Animator componentInChildren = _asset.GetComponentInChildren<Animator>();
			if (!(componentInChildren == null) && componentInChildren.gameObject.activeInHierarchy)
			{
				componentInChildren.speed = GetSpeed();
				componentInChildren.Play(label);
			}
		}
	}

	private void LoadAssets()
	{
		string asset = null;
		switch (_type)
		{
		case 1:
		case 4:
		{
			DungeonRef dungeonRef3 = _data as DungeonRef;
			asset = dungeonRef3.battleBGURL;
			_music = (_boss ? dungeonRef3.bossMusic : dungeonRef3.battleMusic);
			break;
		}
		case 5:
		{
			DungeonRef dungeonRef2 = _data as DungeonRef;
			asset = dungeonRef2.battleBGURL;
			_music = (_boss ? dungeonRef2.bossMusic : dungeonRef2.battleMusic);
			_eventRef = RiftEventBook.GetCurrentEventRef();
			break;
		}
		case 6:
		{
			EventRef eventRef = _data as EventRef;
			asset = eventRef.battleBGURL;
			_music = eventRef.battleMusic;
			_eventRef = eventRef;
			break;
		}
		case 7:
		{
			EventRef eventRef4 = _data as EventRef;
			asset = eventRef4.battleBGURL;
			_music = eventRef4.battleMusic;
			_eventRef = eventRef4;
			break;
		}
		case 2:
		{
			EventRef eventRef3 = _data as EventRef;
			asset = eventRef3.battleBGURL;
			_music = eventRef3.battleMusic;
			_eventRef = eventRef3;
			break;
		}
		case 8:
		{
			EventRef eventRef2 = _data as EventRef;
			asset = eventRef2.battleBGURL;
			_music = eventRef2.battleMusic;
			_eventRef = eventRef2;
			break;
		}
		case 9:
		{
			BrawlRef brawlRef = _data as BrawlRef;
			asset = brawlRef.battleBGURL;
			_music = brawlRef.battleMusic;
			break;
		}
		case 3:
			asset = VariableBook.pvpDuelBattleBGURL;
			_music = VariableBook.pvpDuelBattleMusic;
			break;
		case 11:
		{
			DungeonRef dungeonRef = _data as DungeonRef;
			asset = dungeonRef.battleBGURL;
			_music = (_boss ? dungeonRef.bossMusic : dungeonRef.battleMusic);
			_eventRef = GvGEventBook.GetCurrentEventRef();
			break;
		}
		}
		Transform transformAsset = GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.BATTLE, asset);
		if (transformAsset != null)
		{
			_asset = transformAsset.gameObject;
		}
		if (_asset != null)
		{
			_asset.gameObject.name = "BattleBG";
			_asset.transform.SetParent(base.transform, worldPositionStays: false);
			_asset.transform.localScale = new Vector3(3f, 3f, 1f);
			Vector3 localPosition = new Vector3(0f, 20f, 0f);
			_asset.transform.localPosition = localPosition;
			_asset.gameObject.SetActive(value: false);
			SortingGroup sortingGroup = _asset.gameObject.AddComponent<SortingGroup>();
			if (_asset.activeInHierarchy && sortingGroup.enabled)
			{
				sortingGroup.sortingLayerID = SortingLayer.NameToID("Default");
				sortingGroup.sortingOrder = -5000;
			}
			OnAssetLoaded();
			ResizeSpriteToScreen(_asset);
		}
		LoadEntities(_entities);
	}

	private void ResizeSpriteToScreen(GameObject theSprite)
	{
		SpriteRenderer componentInChildren = GetComponentInChildren<SpriteRenderer>();
		Camera mainCamera = GameData.instance.main.mainCamera;
		theSprite.transform.localScale = new Vector3(1f, 1f, 1f);
		float x = componentInChildren.sprite.bounds.size.x;
		float y = componentInChildren.sprite.bounds.size.y;
		float num = (float)((double)mainCamera.orthographicSize * 2.0);
		float num2 = num / (float)Screen.height * (float)Screen.width / x;
		float num3 = num / y;
		Vector3 one = Vector3.one;
		if (num2 > num3)
		{
			one *= num2;
		}
		else
		{
			one *= num3;
		}
		one.z = 1f;
		theSprite.transform.localScale = one;
	}

	public void LoadEntities(List<BattleEntity> entities)
	{
		foreach (BattleEntity entity in entities)
		{
			if (entity.controller > 0)
			{
				entity.GetAbilityTiles();
			}
			foreach (AbilityRef ability in entity.abilities)
			{
				foreach (AbilityActionRef action in ability.actions)
				{
					action.loadAssets();
				}
			}
			if (entity.characterData == null)
			{
				continue;
			}
			foreach (KeyValuePair<int, EquipmentRef> equipmentSlot in entity.characterData.equipment.equipmentSlots)
			{
				EquipmentRef value = equipmentSlot.Value;
				if (value == null)
				{
					continue;
				}
				foreach (GameModifier modifier in value.modifiers)
				{
					foreach (BattleTriggerRef trigger in modifier.triggers)
					{
						trigger.loadAssets();
					}
				}
			}
		}
	}

	public float GetSpeed(int? speedIndex = null)
	{
		int num = (speedIndex.HasValue ? speedIndex.Value : _battleUI.battleSpeedTile.speedIndex);
		float num2 = ((num < 0 || num >= BATTLE_SPEEDS.Length) ? BATTLE_SPEEDS[0] : BATTLE_SPEEDS[num]);
		float typeTotal = GameModifier.getTypeTotal(GameData.instance.PROJECT.character.getModifiers(), 9);
		if (AppInfo.TESTING)
		{
			num2 *= 3f;
		}
		if (typeTotal > 0f && num != 0)
		{
			num2 += num2 * typeTotal;
		}
		return num2;
	}

	public void OnDamageGainSelect()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoUseDamageGain();
	}

	public bool DoExitConfirm(bool clicked = false)
	{
		if (!clicked && GameData.instance.PROJECT.character.autoPilot)
		{
			GameData.instance.PROJECT.character.autoPilot = !GameData.instance.PROJECT.character.autoPilot;
			CharacterDALC.instance.doSaveConfig(GameData.instance.PROJECT.character);
		}
		if (!_battleUI.closeBtn.interactable || !_battleUI.closeBtn.gameObject.activeSelf)
		{
			return false;
		}
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_exit"), Language.GetString("battle_exit_confirm"), null, null, delegate
		{
			BattleDALC.instance.doQuit();
			TrackEnd("Quit");
		});
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

	public void DoConsumablesSelect()
	{
		if (!GetConsumablesAllowed())
		{
			GameData.instance.windowGenerator.ShowErrorCode(69);
			return;
		}
		List<BattleEntity> controlledEntities = GetControlledEntities();
		List<BattleEntity> list = new List<BattleEntity>();
		foreach (BattleEntity item in controlledEntities)
		{
			if (GetConsumablesAllowed(item) && (item.healthCurrent < item.healthTotal || (item.shieldCurrent < item.shieldTotal && GameData.instance.PROJECT.character.inventory.getItemTypeQty(4, 8) > 0)))
			{
				list.Add(item);
			}
		}
		if (list.Count <= 0)
		{
			GameData.instance.windowGenerator.ShowErrorCode(78);
			return;
		}
		Transform obj = GameData.instance.windowGenerator.NewBattleEntitySelectWindow();
		obj.GetComponent<BattleEntitySelectWindow>().SELECT.AddListener(OnConsumablesSelect);
		obj.GetComponent<BattleEntitySelectWindow>().CLOSE.AddListener(OnBattleEntitySelectWindowClose);
		obj.GetComponent<BattleEntitySelectWindow>().LoadDetails(list, Language.GetString("battle_entity_select_potion"));
	}

	public void OnBattleEntitySelectWindowClose(object e)
	{
		BattleEntitySelectWindow obj = e as BattleEntitySelectWindow;
		obj.CLOSE.RemoveListener(OnBattleEntitySelectWindowClose);
		obj.SELECT.RemoveListener(OnConsumablesSelect);
	}

	private void OnConsumablesSelect(object e)
	{
		if (itemSelectWindow != null)
		{
			return;
		}
		BattleEntity selectedEntity = (e as BattleEntitySelectWindow).selectedEntity;
		if (selectedEntity.consumables <= 0)
		{
			GameData.instance.windowGenerator.ShowErrorCode(31);
			return;
		}
		itemSelectWindow = null;
		if (selectedEntity.isDead)
		{
			Transform transform = GameData.instance.windowGenerator.NewItemSelectWindow();
			itemSelectWindow = transform.GetComponent<ItemSelectWindow>();
			itemSelectWindow.SELECT.AddListener(OnItemSelected);
			itemSelectWindow.CLOSE.AddListener(OnItemSelectWindowClose);
			itemSelectWindow.LoadDetails(ConsumableBook.GetConsumablesByTypes(new int[2] { 8, 3 }), "Battle", selectedEntity);
		}
		else if (selectedEntity.healthCurrent < selectedEntity.healthTotal)
		{
			Transform transform2 = GameData.instance.windowGenerator.NewItemSelectWindow();
			itemSelectWindow = transform2.GetComponent<ItemSelectWindow>();
			itemSelectWindow.GetComponent<ItemSelectWindow>().SELECT.AddListener(OnItemSelected);
			itemSelectWindow.GetComponent<ItemSelectWindow>().CLOSE.AddListener(OnItemSelectWindowClose);
			itemSelectWindow.GetComponent<ItemSelectWindow>().LoadDetails(ConsumableBook.GetConsumablesByTypes(new int[2] { 8, 2 }), "Battle", selectedEntity);
		}
		else if (selectedEntity.shieldCurrent < selectedEntity.shieldTotal && GameData.instance.PROJECT.character.inventory.getItemTypeQty(4, 8) > 0)
		{
			Transform transform3 = GameData.instance.windowGenerator.NewItemSelectWindow();
			itemSelectWindow = transform3.GetComponent<ItemSelectWindow>();
			itemSelectWindow.GetComponent<ItemSelectWindow>().SELECT.AddListener(OnItemSelected);
			itemSelectWindow.GetComponent<ItemSelectWindow>().CLOSE.AddListener(OnItemSelectWindowClose);
			itemSelectWindow.GetComponent<ItemSelectWindow>().LoadDetails(ConsumableBook.GetConsumablesByTypes(new int[1] { 8 }), "Battle", selectedEntity);
		}
		else
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("battle_entity_no_potions_needed"));
		}
	}

	private void OnItemSelected(object e)
	{
		if (!(itemSelectWindow == null))
		{
			BattleEntity entity = itemSelectWindow.data as BattleEntity;
			BattleDALC.instance.doConsumable(entity, itemSelectWindow.selectedItem.id);
			itemSelectWindow = null;
		}
	}

	public void OnItemSelectWindowClose(object e)
	{
		if (!(itemSelectWindow == null))
		{
			itemSelectWindow.CLOSE.RemoveListener(OnItemSelectWindowClose);
			itemSelectWindow.SELECT.RemoveListener(OnItemSelected);
			itemSelectWindow = null;
		}
	}

	public List<BattleEntity> GetControlledEntities()
	{
		List<BattleEntity> list = new List<BattleEntity>();
		foreach (BattleEntity entity in _entities)
		{
			if (entity.controller == GameData.instance.PROJECT.character.id)
			{
				list.Add(entity);
			}
		}
		return list;
	}

	public void DoStart()
	{
		if (!_started)
		{
			_battleUI.Show(show: true);
			_started = true;
			Debug.Log("             DoStart()   add [RunQueue]  event       ");
			TweenEntityPositions(_entities, RunQueue);
			TrackStart();
		}
	}

	private void RunQueue()
	{
		StopTimeoutTimer();
		_movement = _queue.Count > 0;
		Debug.Log("RunQueue() = " + _queue.Count);
		if (_queue.Count <= 0)
		{
			if (GameData.instance.windowGenerator.HasDialogByClass(typeof(BattleCaptureWindow)))
			{
				Debug.Log(" -------- HasDialogByClass ----------");
				return;
			}
			if (_captureEntities.Count > 0 && !GameData.instance.windowGenerator.HasDialogByClass(typeof(BattleCaptureWindow)))
			{
				BattleEntity battleEntity = _captureEntities[0];
				if (GameData.instance.PROJECT.character.getItemQty(battleEntity.captureFamiliarRef) > 0 && GameData.instance.SAVE_STATE.declineFamiliarDupes && GameData.instance.SAVE_STATE.GetDeclineFamiliarRarity(GameData.instance.PROJECT.character.id, battleEntity.captureFamiliarRef.rarityRef, GameData.instance.SAVE_STATE.GetDeclineFamiliarRarities(GameData.instance.PROJECT.character.id)))
				{
					BattleDALC.instance.doCaptureDecline(battleEntity);
				}
				else
				{
					GameData.instance.windowGenerator.NewBattleCaptureWindow(battleEntity, new int[2] { 0, 1 });
				}
				Debug.Log(" -------- 11111111111111 ----------");
			}
			else if (_completed && !_results && !BattleDALC.instance.waiting)
			{
				Debug.Log(" -------- IF 2 ----------");
				if (!BattleDALC.instance.doingResult)
				{
					Debug.Log(" -------- IF doResults ----------");
					BattleDALC.instance.doResults();
				}
			}
			else if (!GetLocked() && _currentEntity != null && _currentEntity.controller == GameData.instance.PROJECT.character.id && GameData.instance.PROJECT.character.autoPilot)
			{
				CheckAutoPilot();
				Debug.Log(" -------- CheckAutoPilot() ----------");
			}
			else
			{
				UpdateButtons();
				Debug.Log(" -------- UpdateButtons()  ----------");
			}
			Debug.Log(" -------- RETURN ---------- : _queue.Count = " + _queue.Count);
			return;
		}
		BattleDALC.instance.doingResult = false;
		SFSObject sFSObject = _queue[0];
		_queue.RemoveAt(0);
		int @int = sFSObject.GetInt("act0");
		StartTimeoutTimer();
		UpdateButtons(tutorial: false);
		_currentAction = @int;
		D.Log("all", "------------- RunQueue Parsing " + @int);
		switch (@int)
		{
		case 1:
			DoActionDelay(sFSObject);
			break;
		case 2:
			DoActionHealthChange(sFSObject);
			break;
		case 3:
			DoActionMeterChange(sFSObject);
			break;
		case 4:
			DoActionMeterGain(sFSObject);
			break;
		case 5:
			DoActionBegin(sFSObject);
			break;
		case 6:
			DoActionAbility(sFSObject);
			break;
		case 7:
			DoActionTurnStart(sFSObject);
			break;
		case 8:
			DoActionTurnEnd(sFSObject);
			break;
		case 9:
			DoActionDeathChange(sFSObject);
			break;
		case 10:
			DoActionComplete(sFSObject);
			break;
		case 11:
			DoActionVictory(sFSObject);
			break;
		case 12:
			DoActionDefeat(sFSObject);
			break;
		case 13:
			DoActionItemUsed(sFSObject);
			break;
		case 14:
			DoActionOrder(sFSObject);
			break;
		case 15:
			DoActionCaptureSet(sFSObject);
			break;
		case 16:
			DoActionCaptureComplete(sFSObject);
			break;
		case 17:
			DoActionEntityValues(sFSObject);
			break;
		case 18:
			DoActionEntitiesRemove(sFSObject);
			break;
		case 19:
			DoActionEntitiesAdd(sFSObject);
			break;
		case 20:
			DoActionTeamDataChange(sFSObject);
			break;
		case 21:
			DoActionResults(sFSObject);
			break;
		case 22:
			DoActionAbilityData(sFSObject);
			break;
		case 23:
			DoStackApplied(sFSObject);
			break;
		case 24:
			DoSkipTurn(sFSObject);
			break;
		case 25:
			DoRestoreTurn(sFSObject);
			break;
		case 26:
			queueBlockSkills(sFSObject);
			break;
		case 27:
			queueBarrierSkills(sFSObject);
			break;
		case 28:
			queueRootSkills(sFSObject);
			break;
		case 29:
			queuedefenseIgnoreSkills(sFSObject);
			break;
		case 33:
			queueBuffDamageSkills(sFSObject);
			break;
		case 30:
			queueTriggerAbilitySkills(sFSObject);
			break;
		default:
			RunQueue();
			break;
		}
	}

	private void AddAbilityTiles()
	{
		Debug.Log("++++++++++++++++++++++++++++ AddAbilityTiles()");
		ClearAbilityTiles();
		if (_currentEntity == null || _completed || _currentEntity.controller != GameData.instance.PROJECT.character.id)
		{
			return;
		}
		if (_abilityTiles == null)
		{
			_abilityTiles = new List<AbilityTile>();
		}
		RectTransform component = _battleUI.placeholderAbilities.GetComponent<RectTransform>();
		foreach (AbilityTile abilityTile in _currentEntity.GetAbilityTiles())
		{
			abilityTile.transform.SetParent(_battleUI.placeholderAbilities, worldPositionStays: false);
			abilityTile.transform.localPosition = new Vector3((float)_abilityTiles.Count * component.sizeDelta.x, 0f, 0f);
			abilityTile.gameObject.SetActive(value: true);
			abilityTile.RefreshAsset();
			abilityTile.onClicked = DoAbilitySelect;
			_abilityTiles.Add(abilityTile);
		}
		UpdateAbilityTiles();
		foreach (AbilityTile abilityTile2 in _abilityTiles)
		{
			abilityTile2.transform.localPosition -= new Vector3((float)(_abilityTiles.Count - 1) * (component.sizeDelta.x / 2f), 0f, 0f);
		}
		TweenAbilityTiles();
	}

	public bool AllowAbility(BattleEntity entity, AbilityRef abilityRef)
	{
		if (entity.meter < abilityRef.meterCost)
		{
			return false;
		}
		if (entity.GetAbilityDisabled(abilityRef))
		{
			return false;
		}
		if (abilityRef.meterCost > 0 && _currentEntity.getBlockSkillsData())
		{
			return false;
		}
		return true;
	}

	public void UpdateAbilityTiles()
	{
		bool locked = GetLocked();
		if (_abilityTiles != null)
		{
			foreach (AbilityTile abilityTile in _abilityTiles)
			{
				abilityTile.SetClickable(!locked && AllowAbility(_currentEntity, abilityTile.abilityRef));
				abilityTile.SetDisabled(_currentEntity.GetAbilityDisabled(abilityTile.abilityRef));
			}
		}
		else
		{
			Debug.Log(" _abilityTiles is null ");
		}
		UpdateCurrentEntity(!locked);
		UpdateDamageGain(locked);
	}

	public void UpdateDamageGain(bool locked)
	{
		if (!locked && (bool)_currentEntity && _currentEntity.damageGained > 0f && !float.IsNaN(_currentEntity.damageGained) && _currentEntity.controller == GameData.instance.PROJECT.character.id)
		{
			_battleUI.damageGainTile.gameObject.SetActive(value: true);
			_battleUI.damageGainTile.SetDamageGain((int)_currentEntity.damageGained);
		}
		else if (_battleUI != null && _battleUI.damageGainTile != null)
		{
			_battleUI.damageGainTile.gameObject.SetActive(value: false);
		}
	}

	public void TweenAbilityTiles(bool inward = true)
	{
		Debug.Log("--------- TweenAbilityTiles() inward = " + inward);
		if (_abilityTiles.Count <= 0)
		{
			Debug.Log("                              _abilityTiles.Count <= 0");
			return;
		}
		_abilityTween = true;
		float num = 0.25f / GetSpeed();
		float num2 = 0.05f / GetSpeed();
		int num3 = (inward ? 50 : (-50));
		float endAlpha = (inward ? 1 : 0);
		for (int i = 0; i < _abilityTiles.Count; i++)
		{
			AbilityTile abilityTile = _abilityTiles[i];
			abilityTile.StopTween();
			abilityTile.TweenAbility(inward, num2 * (float)i, num, num3, endAlpha);
		}
		float seconds = num + num2 * (float)(_abilityTiles.Count - 1);
		Debug.Log("--------- TweenAbilityTiles() total = " + seconds);
		StartCoroutine(DoTimer(seconds, delegate
		{
			OnTweenAbilityTilesComplete();
		}));
	}

	private IEnumerator DoTimer(float seconds, UnityAction callback)
	{
		yield return new WaitForSecondsRealtime(seconds);
		if (callback != null && callback.Target != null)
		{
			callback();
		}
	}

	private void OnTweenAbilityTilesComplete()
	{
		_abilityTween = false;
		UpdateButtons();
		CheckAutoPilot();
		Debug.LogFormat("Log: <color=orange>{0}</color>", ">>> OnTweenAbilityTilesComplete <<<");
	}

	private void CheckAutoPilot()
	{
		if (!GetLocked() && GetMyTurn() && GameData.instance.PROJECT.character.autoPilot)
		{
			BattleDALC.instance.doAuto(_autoEnrage);
		}
	}

	private void ClearAbilityTiles()
	{
		if (_abilityTiles == null)
		{
			return;
		}
		TweenAbilityTiles(inward: false);
		foreach (AbilityTile abilityTile in _abilityTiles)
		{
			abilityTile.SetClickable(clickable: false);
			abilityTile.onClicked = null;
		}
		_abilityTiles.Clear();
	}

	private void OnAbilitySelect(object e)
	{
		if (!GetLocked())
		{
			AbilityRef abilityRef = (e as AbilityTile).abilityRef;
			DoAbilitySelect(abilityRef);
		}
	}

	private void DoAbilitySelect(AbilityRef abilityRef)
	{
		if (GetLocked())
		{
			return;
		}
		if (abilityRef.select == 0)
		{
			BattleDALC.instance.doAbility(abilityRef.id);
			return;
		}
		List<BattleEntity> targets = GetTargets(abilityRef.select, _currentEntity, abilityRef.selectDead, abilityRef.selectAlive);
		if (targets.Count <= 0)
		{
			DialogWindow dialogWindow = GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("battle_no_targets"));
			noTargetsOpened = true;
			dialogWindow.SCROLL_OUT_START.AddListener(delegate
			{
				noTargetsOpened = false;
			});
		}
		else if (targets.Count == 1)
		{
			BattleEntity selectEntity = targets[0];
			targets.RemoveAt(0);
			BattleDALC.instance.doAbility(abilityRef.id, selectEntity);
		}
		else
		{
			Transform obj = GameData.instance.windowGenerator.NewBattleEntitySelectWindow();
			obj.GetComponent<BattleEntitySelectWindow>().SELECT.AddListener(OnAbilityEntitySelect);
			obj.GetComponent<BattleEntitySelectWindow>().LoadDetails(targets, Language.GetString("battle_select_ability_target"), drag: false, abilityRef, tutorial: true);
		}
	}

	private void OnAbilityEntitySelect(object e)
	{
		BattleEntitySelectWindow obj = e as BattleEntitySelectWindow;
		BattleEntity selectedEntity = obj.selectedEntity;
		AbilityRef abilityRef = obj.data as AbilityRef;
		if (!(selectedEntity == null) && abilityRef != null)
		{
			BattleDALC.instance.doAbility(abilityRef.id, selectedEntity);
		}
	}

	public void DoUseDamageGain()
	{
		if (!GetLocked() && GameData.instance.windowGenerator.GetDialogCountWithout(typeof(BattleUI), typeof(DungeonUI)) <= 0)
		{
			BattleDALC.instance.doUseDamageGain();
		}
	}

	public void UpdateButtons(bool tutorial = true)
	{
		UpdateAbilityTiles();
		bool locked = GetLocked();
		bool flag = _currentEntity != null && _currentEntity.controller == GameData.instance.PROJECT.character.id;
		if (_battleUI != null && _battleUI.closeBtn != null)
		{
			Util.SetButton(_battleUI.closeBtn, !locked);
		}
		if (locked || !flag)
		{
			StartFreezeTimer();
			if (_battleUI != null && _battleUI.battleConsumablesTile != null && _battleUI.battleConsumablesTile.gameObject != null)
			{
				Util.SetButton(_battleUI.battleConsumablesTile.gameObject.GetComponent<Button>(), enabled: false);
			}
			if (_battleUI != null && _battleUI.battleFormationTile != null && _battleUI.battleFormationTile.gameObject != null)
			{
				Util.SetButton(_battleUI.battleFormationTile.gameObject.GetComponent<Button>(), enabled: false);
			}
			SetTurnTime();
		}
		else
		{
			StopFreezeTimer();
			if (_battleUI != null && _battleUI.battleConsumablesTile != null && _battleUI.battleConsumablesTile.gameObject != null)
			{
				Util.SetButton(_battleUI.battleConsumablesTile.gameObject.GetComponent<Button>());
			}
			if (_battleUI != null && _battleUI.battleFormationTile != null && _battleUI.battleFormationTile.gameObject != null)
			{
				Util.SetButton(_battleUI.battleFormationTile.gameObject.GetComponent<Button>());
			}
			if (!_turnTimerStarted)
			{
				SetTurnTime(Mathf.RoundToInt(VariableBook.pvpDuelTurnSeconds));
				_turnTimerStarted = true;
			}
			else if (_battleUI != null && _battleUI.timeBar != null)
			{
				_battleUI.timeBar.SetMaxValueSeconds(Mathf.RoundToInt(VariableBook.pvpDuelTurnSeconds));
				SetTurnTime(Mathf.RoundToInt(_battleUI.timeBar.GetCurrentValueSeconds()), exception: true);
			}
		}
		if (tutorial && !locked && flag)
		{
			CheckTutorial();
		}
	}

	private void SetTurnTime(int seconds = 0, bool exception = false)
	{
		if (_battleUI.timeBar == null)
		{
			return;
		}
		if (seconds <= 0 && !exception)
		{
			_battleUI.timeBar.CancelInvoke("UpdateSeconds");
			_isTimerRunning = false;
			_battleUI.placeholderDisplay.gameObject.SetActive(value: false);
			return;
		}
		_battleUI.placeholderDisplay.gameObject.SetActive(value: true);
		_battleUI.timeBar.SetMaxValueSeconds(seconds);
		_battleUI.timeBar.SetCurrentValueSeconds(seconds);
		if (!_isTimerRunning)
		{
			_battleUI.timeBar.InvokeRepeating("UpdateSeconds", 0f, 1f);
			_isTimerRunning = true;
		}
	}

	private void CheckTutorial()
	{
		if (GameData.instance.tutorialManager.hasPopup || !base.gameObject.activeSelf || GameData.instance.PROJECT.character.autoPilot || _replay)
		{
			return;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(8))
		{
			AbilityTile abilityTileBasic = GetFirstAbilityTile(cost: false);
			if (abilityTileBasic != null && abilityTileBasic.clickable)
			{
				abilityTileBasic.RemoveTweenKeys();
				Vector3 position = abilityTileBasic.transform.position;
				abilityTileBasic.SetShimmer(enabled: false);
				GameData.instance.PROJECT.character.tutorial.SetState(8);
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(abilityTileBasic.gameObject, new TutorialPopUpSettings(Tutorial.GetText(8), 4, abilityTileBasic.gameObject), EventTriggerType.PointerClick, stageTrigger: false, null, funcSameAsTargetFunc: false, delegate
				{
					abilityTileBasic.OnPointerClick(null);
				}, shadow: true, tween: true);
				abilityTileBasic.SetShimmer();
				abilityTileBasic.transform.position = position;
				return;
			}
		}
		if (GameData.instance.PROJECT.character.tutorial.GetState(8) && !GameData.instance.PROJECT.character.tutorial.GetState(9))
		{
			AbilityTile abilityTileAdvanced = GetFirstAbilityTile(cost: true);
			if ((bool)abilityTileAdvanced && abilityTileAdvanced.clickable)
			{
				abilityTileAdvanced.RemoveTweenKeys();
				Vector3 position2 = abilityTileAdvanced.transform.position;
				abilityTileAdvanced.SetShimmer(enabled: false);
				GameData.instance.PROJECT.character.tutorial.SetState(9);
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(abilityTileAdvanced.gameObject, new TutorialPopUpSettings(Tutorial.GetText(9), 4, abilityTileAdvanced.gameObject), EventTriggerType.PointerClick, stageTrigger: false, null, funcSameAsTargetFunc: false, delegate
				{
					abilityTileAdvanced.OnPointerClick(null);
				}, shadow: true, tween: true);
				abilityTileAdvanced.SetShimmer();
				abilityTileAdvanced.transform.position = position2;
				return;
			}
		}
		if (GameData.instance.PROJECT.character.tutorial.GetState(9) && !GameData.instance.PROJECT.character.tutorial.GetState(47) && AppInfo.IsMobile())
		{
			AbilityTile firstAbilityTile = GetFirstAbilityTile(cost: false);
			if ((bool)firstAbilityTile && firstAbilityTile.clickable)
			{
				firstAbilityTile.RemoveTweenKeys();
				Vector3 position3 = firstAbilityTile.transform.position;
				firstAbilityTile.SetShimmer(enabled: false);
				GameData.instance.PROJECT.character.tutorial.SetState(47);
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(firstAbilityTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(47), 4, firstAbilityTile.gameObject), EventTriggerType.PointerDown, stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: false, delegate
				{
					GameData.instance.tutorialManager.ClearTutorial();
				});
				firstAbilityTile.SetShimmer();
				firstAbilityTile.transform.position = position3;
				return;
			}
		}
		if (GameData.instance.PROJECT.character.tutorial.GetState(9) && !GameData.instance.PROJECT.character.tutorial.GetState(10) && _abilityTiles.Count > 2)
		{
			AbilityTile firstAbilityTile2 = GetFirstAbilityTile(cost: false);
			if (firstAbilityTile2 != null && firstAbilityTile2.clickable)
			{
				firstAbilityTile2.RemoveTweenKeys();
				Vector3 position4 = firstAbilityTile2.transform.position;
				firstAbilityTile2.SetShimmer(enabled: false);
				GameData.instance.PROJECT.character.tutorial.SetState(10);
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(firstAbilityTile2.gameObject, new TutorialPopUpSettings(Tutorial.GetText(10), 4, firstAbilityTile2.gameObject), EventTriggerType.PointerClick, stageTrigger: true, null, funcSameAsTargetFunc: false, delegate
				{
					GameData.instance.tutorialManager.ClearTutorial();
				}, shadow: false);
				firstAbilityTile2.SetShimmer();
				firstAbilityTile2.transform.position = position4;
				return;
			}
		}
		if (GameData.instance.PROJECT.character.tutorial.GetState(11) || !GetConsumablesAllowed())
		{
			return;
		}
		foreach (BattleEntity controlledEntity in GetControlledEntities())
		{
			if (controlledEntity.healthPerc <= 0.4f && _battleUI.battleConsumablesTile.enabled && _battleUI.battleConsumablesTile.gameObject.activeSelf && GetConsumablesAllowed(controlledEntity))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(11);
				GameData.instance.tutorialManager.ShowTutorialForButton(_battleUI.battleConsumablesTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(11), 4, _battleUI.battleConsumablesTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: false, delegate
				{
					CheckTutorial();
				});
				break;
			}
		}
	}

	public bool GetConsumablesAllowed(BattleEntity entity = null)
	{
		if (_type != 6 && GameData.instance.PROJECT.dungeon == null)
		{
			return false;
		}
		if (entity != null && entity.consumables <= 0)
		{
			return false;
		}
		return true;
	}

	private AbilityTile GetFirstAbilityTile(bool cost)
	{
		foreach (AbilityTile abilityTile in _abilityTiles)
		{
			if (cost && abilityTile.abilityRef.meterCost > 0)
			{
				return abilityTile;
			}
			if (!cost && abilityTile.abilityRef.meterCost <= 0)
			{
				return abilityTile;
			}
		}
		return null;
	}

	private bool GetLocked()
	{
		if (_movement || _completed || _queue.Count > 0 || BattleDALC.instance.waiting || _captureEntities.Count > 0 || _abilityTween)
		{
			return true;
		}
		return false;
	}

	private bool GetMyTurn()
	{
		if ((bool)_currentEntity && _currentEntity.controller == GameData.instance.PROJECT.character.id)
		{
			return true;
		}
		return false;
	}

	private bool HasCaptureEntity(BattleEntity entity)
	{
		foreach (BattleEntity captureEntity in _captureEntities)
		{
			if (captureEntity == entity)
			{
				return true;
			}
		}
		return false;
	}

	private void AddCaptureEntity(BattleEntity entity)
	{
		if (!HasCaptureEntity(entity))
		{
			_captureEntities.Add(entity);
		}
	}

	private void RemoveCaptureEntity(BattleEntity entity)
	{
		for (int i = 0; i < _captureEntities.Count; i++)
		{
			if (_captureEntities[i] == entity)
			{
				_captureEntities.RemoveAt(i);
				break;
			}
		}
	}

	public BattleTeamData GetTeamData(bool attacker)
	{
		if (attacker)
		{
			return _attackerData;
		}
		return _defenderData;
	}

	public List<BattleEntity> GetTargets(int target, BattleEntity entity, bool getDead, bool getAlive)
	{
		List<BattleEntity> list = new List<BattleEntity>();
		switch (target)
		{
		case 1:
		{
			foreach (BattleEntity entity2 in _entities)
			{
				if (entity2.isDead == getDead && entity2.isAlive == getAlive)
				{
					list.Add(entity2);
				}
			}
			return list;
		}
		case 2:
			if (entity.isDead == getDead && entity.isAlive == getAlive)
			{
				list.Add(entity);
			}
			break;
		case 3:
		{
			foreach (BattleEntity entity3 in _entities)
			{
				if (entity3.attacker == entity.attacker && entity3.isDead == getDead && entity3.isAlive == getAlive)
				{
					list.Add(entity3);
					return list;
				}
			}
			return list;
		}
		case 4:
		{
			BattleEntity battleEntity2 = null;
			foreach (BattleEntity entity4 in _entities)
			{
				if (entity4.attacker == entity.attacker && entity4.isDead == getDead && entity4.isAlive == getAlive)
				{
					battleEntity2 = entity4;
				}
			}
			if (battleEntity2 != null)
			{
				list.Add(battleEntity2);
			}
			break;
		}
		case 5:
		{
			foreach (BattleEntity entity5 in _entities)
			{
				if (entity5.attacker == entity.attacker && entity5.isDead == getDead && entity5.isAlive == getAlive)
				{
					list.Add(entity5);
				}
			}
			return list;
		}
		case 6:
		{
			foreach (BattleEntity entity6 in _entities)
			{
				if (entity6.index != entity.index && entity6.attacker == entity.attacker && entity6.isDead == getDead && entity6.isAlive == getAlive)
				{
					list.Add(entity6);
				}
			}
			return list;
		}
		case 7:
		{
			foreach (BattleEntity entity7 in _entities)
			{
				if (entity7.attacker != entity.attacker && entity7.isDead == getDead && entity7.isAlive == getAlive)
				{
					list.Add(entity7);
					return list;
				}
			}
			return list;
		}
		case 8:
		{
			BattleEntity battleEntity = null;
			foreach (BattleEntity entity8 in _entities)
			{
				if (entity8.attacker != entity.attacker && entity8.isDead == getDead && entity8.isAlive == getAlive)
				{
					battleEntity = entity8;
				}
			}
			if (battleEntity != null)
			{
				list.Add(battleEntity);
			}
			break;
		}
		case 9:
		{
			foreach (BattleEntity entity9 in _entities)
			{
				if (entity9.attacker != entity.attacker && entity9.isDead == getDead && entity9.isAlive == getAlive)
				{
					list.Add(entity9);
				}
			}
			return list;
		}
		case 10:
		{
			foreach (BattleEntity entity10 in _entities)
			{
				BattleEntity enemy = GetEnemy(entity);
				if ((enemy == null || enemy.index != entity10.index) && entity10.attacker != entity.attacker && entity10.isDead == getDead && entity10.isAlive == getAlive)
				{
					list.Add(entity10);
				}
			}
			return list;
		}
		}
		return list;
	}

	public BattleEntity GetEnemy(BattleEntity entity)
	{
		foreach (BattleEntity entity2 in _entities)
		{
			if (entity2.attacker != entity.attacker)
			{
				return entity2;
			}
		}
		return null;
	}

	private void StartTimeoutTimer()
	{
		if (!(this == null) && base.isActiveAndEnabled && !(base.gameObject == null))
		{
			StopTimeoutTimer();
			Invoke("OnTimeoutTimer", 10f);
		}
	}

	private void StopTimeoutTimer()
	{
		if (!(this == null) && base.isActiveAndEnabled && !(base.gameObject == null) && IsInvoking("OnTimeoutTimer"))
		{
			CancelInvoke("OnTimeoutTimer");
		}
	}

	private void ClearTimeoutTimer()
	{
		StopTimeoutTimer();
	}

	private void OnTimeoutTimer()
	{
		if (this != null && base.isActiveAndEnabled && base.gameObject != null)
		{
			RunQueue();
		}
	}

	private void StartFreezeTimer()
	{
	}

	private void StopFreezeTimer()
	{
	}

	private void ClearFreezeTimer()
	{
		StopFreezeTimer();
	}

	private void OnFreezeTimer()
	{
		if (!_completed && !GameData.instance.windowGenerator.HasDialogByClass(typeof(BattleCaptureWindow)))
		{
			BattleDALC.instance.doAuto();
		}
	}

	public void OnTurnTimer()
	{
		SetTurnTime();
		if (!GetLocked() && GetMyTurn())
		{
			BattleDALC.instance.doAuto();
		}
	}

	public void UpdateCountBar()
	{
		if ((bool)_battleUI.countBar)
		{
			BattleTeamData teamData = GetTeamData(!GetAttackerFocus());
			_battleUI.countBar.SetCurrent(teamData.poolCurrent, draw: false);
			_battleUI.countBar.SetTotal(teamData.poolTotal, draw: false);
			_battleUI.countBar.DoUpdate();
		}
	}

	private void DoActionDelay(SFSObject sfsob)
	{
		sfsob.GetInt("bat7");
		int @int = sfsob.GetInt("bat4");
		SetTime(@int);
	}

	private void DoActionHealthChange(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (!entity)
		{
			RunQueue();
			return;
		}
		int int2 = sfsob.GetInt("bat11");
		int int3 = sfsob.GetInt("bat13");
		int int4 = sfsob.GetInt("bat51");
		int int5 = sfsob.GetInt("bat53");
		int int6 = sfsob.GetInt("bat56");
		int int7 = sfsob.GetInt("bat63");
		int int8 = sfsob.GetInt("bat16");
		int num = int3 + int5 + int7;
		bool flag = sfsob.ContainsKey("bat17") && sfsob.GetBool("bat17");
		bool flag2 = sfsob.ContainsKey("bat18") && sfsob.GetBool("bat18");
		bool flag3 = sfsob.ContainsKey("bat19") && sfsob.GetBool("bat19");
		bool flag4 = sfsob.ContainsKey("bat55") && sfsob.GetBool("bat55");
		bool flag5 = sfsob.ContainsKey("bat40") && sfsob.GetBool("bat40");
		bool flag6 = sfsob.ContainsKey("bat48") && sfsob.GetBool("bat48");
		bool flag7 = sfsob.ContainsKey("bat49") && sfsob.GetBool("bat49");
		bool flag8 = sfsob.ContainsKey("bat50") && sfsob.GetBool("bat50");
		bool flag9 = sfsob.ContainsKey("bat54") && sfsob.GetBool("bat54");
		float value = (sfsob.ContainsKey("bat61") ? sfsob.GetFloat("bat61") : 0f);
		AbilityActionRef abilityActionRef = (sfsob.ContainsKey("bat23") ? AbilityBook.LookupAction(sfsob.GetInt("bat23")) : null);
		if (abilityActionRef != null && int8 >= 0)
		{
			_ = abilityActionRef.effectEnd;
		}
		entity.SetShieldCurrent(int4);
		entity.SetHealthCurrent(int2);
		if (MustSaveBattleStats() && (bool)_currentEntity && (bool)entity)
		{
			_ = "[" + _currentEntity.id + "|" + _currentEntity.index + "|" + _currentEntity.attacker + "]";
			_ = "[" + entity.id + "|" + entity.index + "|" + entity.attacker + "]";
			int index = _currentEntity.index;
			int index2 = entity.index;
			int num2 = Mathf.Abs(num);
			if (num < 0)
			{
				if (flag)
				{
					_battleStats[index2].damageBlocked += num2;
				}
				_battleStats[index].damageDone += num2;
				_battleStats[index2].damageTaken += num2;
			}
			else if (int3 > 0)
			{
				_battleStats[index].healingDone += num2;
				_battleStats[index2].healingTaken += num2;
			}
			if (int5 > 0)
			{
				_battleStats[index].shielding += num2;
			}
		}
		string text = ((int8 < 0) ? "+" : "");
		string text2 = BattleText.COLOR_GREEN;
		float scale = 1f;
		Vector2 center = Util.GetCenter(entity.transform);
		if (int8 >= 0)
		{
			if (!(flag3 || flag4) || flag)
			{
				text2 = ((!(!flag3 && !flag4 && flag)) ? BattleText.COLOR_RED : BattleText.COLOR_PURPLE);
			}
			else
			{
				text2 = BattleText.COLOR_ORANGE;
				scale = 1.25f;
			}
		}
		if (int5 > 0)
		{
			text2 = BattleText.COLOR_PINK;
		}
		else if (int7 != 0)
		{
			text2 = BattleText.COLOR_TEAL;
		}
		CharacterDisplay componentInChildren = entity.GetComponentInChildren<CharacterDisplay>();
		if (componentInChildren != null)
		{
			if (componentInChildren.hasMountEquipped())
			{
				BoxCollider2D componentInChildren2 = entity.GetComponentInChildren<BoxCollider2D>();
				if (componentInChildren2 != null)
				{
					center.y += componentInChildren2.size.y;
				}
			}
			BoxCollider2D componentInChildren3 = componentInChildren.GetComponentInChildren<BoxCollider2D>();
			if (componentInChildren3 != null)
			{
				center.y += componentInChildren3.size.y * componentInChildren3.transform.localScale.y / 2f;
			}
		}
		else
		{
			BoxCollider2D componentInChildren4 = entity.GetComponentInChildren<BoxCollider2D>();
			if (componentInChildren4 != null)
			{
				center.y += componentInChildren4.size.y / 2f;
			}
		}
		float num3 = center.y + 30f;
		float num4 = 0.1f;
		float num5 = 0f;
		if (flag3 && num != 0)
		{
			if (_battleText)
			{
				AddBattleTextObj().LoadDetails(Language.GetString("battle_critical"), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
				num3 += 30f;
				num5 += num4;
			}
			GameData.instance.audioManager.PlaySoundLink("critical");
		}
		if (flag4 && num != 0)
		{
			if (_battleText)
			{
				AddBattleTextObj().LoadDetails(Language.GetString("battle_empowered"), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
				num3 += 30f;
				num5 += num4;
			}
			GameData.instance.audioManager.PlaySoundLink("critical");
		}
		if (flag)
		{
			if (_battleText)
			{
				AddBattleTextObj().LoadDetails(Language.GetString("battle_block"), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
				num3 += 30f;
				num5 += num4;
			}
			GameData.instance.audioManager.PlaySoundLink("block");
		}
		if (flag2)
		{
			if (_battleText)
			{
				AddBattleTextObj().LoadDetails(Language.GetString("battle_evade"), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
				num3 += 30f;
				num5 += num4;
			}
			GameData.instance.audioManager.PlaySoundLink("evade");
		}
		if (flag5)
		{
			if (_battleAnimations)
			{
				entity.PlayAnimation("hit");
			}
			if (_battleText)
			{
				AddBattleTextObj().LoadDetails(Language.GetString("battle_deflect"), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
				num3 += 30f;
				num5 += num4;
			}
			GameData.instance.audioManager.PlaySoundLink("block");
		}
		if (flag6 && _battleText)
		{
			AddBattleTextObj().LoadDetails(Language.GetString("battle_bonus") + GetChangePercText(value), BattleText.COLOR_CYAN, 3f, 0f, center.x, num3, 0.6f, -30f, num5);
			num3 += 30f;
			num5 += num4;
		}
		string text3 = "";
		if (sfsob.ContainsKey("bat64"))
		{
			text3 = "bat64";
		}
		else if (sfsob.ContainsKey("bat65"))
		{
			text3 = "bat65";
		}
		else if (sfsob.ContainsKey("bat66"))
		{
			text3 = "bat66";
		}
		else if (sfsob.ContainsKey("bat67"))
		{
			text3 = "bat67";
		}
		else if (sfsob.ContainsKey("bat68"))
		{
			text3 = "bat68";
		}
		if (flag6 && _battleText)
		{
			string link = "battle_bonus";
			string color = BattleText.COLOR_CYAN;
			switch (text3)
			{
			case "bat64":
				color = "#FD5400";
				break;
			case "bat65":
				color = "#1342EF";
				break;
			case "bat66":
				color = "#E7DA83";
				break;
			case "bat67":
				color = "#5FC608";
				break;
			case "bat68":
				color = "#B5D4F0";
				break;
			}
			AddBattleTextObj().LoadDetails(Language.GetString(link) + GetChangePercText(value), color, 3f, 0f, center.x, num3, 0.6f, -30f, num5);
			num3 -= 30f;
			num5 += num4;
		}
		if (flag7 && _battleText)
		{
			AddBattleTextObj().LoadDetails(Language.GetString("battle_reduced") + GetChangePercText(value), BattleText.COLOR_CYAN, 3f, 0f, center.x, num3, 0.6f, -30f, num5);
			num3 += 30f;
			num5 += num4;
		}
		if (flag8)
		{
			if (_battleText)
			{
				AddBattleTextObj().LoadDetails(Language.GetString("battle_redirect"), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
				num3 += 30f;
				num5 += num4;
			}
			GameData.instance.audioManager.PlaySoundLink("block");
		}
		if (flag9)
		{
			if (_battleText)
			{
				AddBattleTextObj().LoadDetails(Language.GetString("battle_absorb"), BattleText.COLOR_YELLOW, 3f, 0f, center.x, num3, 0.8f, -30f, num5);
				num3 += 30f;
				num5 += num4;
			}
			GameData.instance.audioManager.PlaySoundLink("block");
		}
		if (num != 0)
		{
			string link2 = "";
			string color2 = text2;
			float num6 = 20f;
			float num7 = center.y + num6;
			if (num < 0)
			{
				color2 = BattleText.COLOR_RED;
				switch (text3)
				{
				case "bat64":
					link2 = "battle_fire_damage";
					color2 = "#FD5400";
					num7 -= num6;
					break;
				case "bat65":
					link2 = "battle_water_damage";
					color2 = "#1342EF";
					num7 -= num6;
					break;
				case "bat66":
					link2 = "battle_electric_damage";
					color2 = "#E7DA83";
					num7 -= num6;
					break;
				case "bat67":
					link2 = "battle_earth_damage";
					color2 = "#5FC608";
					num7 -= num6;
					break;
				case "bat68":
					link2 = "battle_air_damage";
					color2 = "#B5D4F0";
					num7 -= num6;
					break;
				}
			}
			link2 = Language.GetString(link2);
			if (_battleText)
			{
				string text4 = text + Util.NumberFormat(-int8) + " " + link2;
				AddBattleTextObj().LoadDetails(text4, color2, 3f, 10f, center.x, num7, scale, -30f, num5);
			}
			if (_battleEffects)
			{
				SpritesFlash spritesFlash = entity.GetComponent<SpritesFlash>();
				if (spritesFlash == null)
				{
					spritesFlash = entity.gameObject.AddComponent<SpritesFlash>();
				}
				spritesFlash.DoFlash();
				SpritesFlash spritesFlash2 = entity.overlay.GetComponent<SpritesFlash>();
				if (spritesFlash2 == null)
				{
					spritesFlash2 = entity.overlay.gameObject.AddComponent<SpritesFlash>();
				}
				spritesFlash2.DoFlash();
			}
			if (num < 0)
			{
				if (_battleAnimations)
				{
					entity.PlayAnimation("hit");
				}
				GameData.instance.audioManager.PlaySoundPoolLink("damage");
			}
			else if (int3 > 0)
			{
				GameData.instance.audioManager.PlaySoundLink("heal");
			}
			else if (int5 > 0)
			{
				GameData.instance.audioManager.PlaySoundLink("sheen");
			}
		}
		if (int7 != 0)
		{
			entity.SetDamageGained(int6);
		}
		float seconds = (abilityActionRef?.duration ?? 0.05f) / GetSpeed();
		if (flag3 && int3 != 0)
		{
			Util.Shake(entity.gameObject, GetSpeed(), new Vector2(entity.x, entity.y));
		}
		StartCoroutine(DoTimer(seconds, RunQueue));
	}

	private void DoStackApplied(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		if (_battleText)
		{
			int int2 = sfsob.GetInt("bat70");
			int int3 = sfsob.GetInt("bat71");
			int int4 = sfsob.GetInt("bat74");
			string cOLOR_YELLOW = BattleText.COLOR_YELLOW;
			Vector2 center = Util.GetCenter(entity.transform);
			float num = center.y + 75f;
			float num2 = 0.1f;
			float num3 = 0f;
			string stackLabel = AbilityActionRef.getStackLabel(int2, int3, int4);
			if (stackLabel == null)
			{
				RunQueue();
				return;
			}
			string @string = Language.GetString(stackLabel, new string[1] { int3.ToString() });
			CharacterDisplay componentInChildren = entity.GetComponentInChildren<CharacterDisplay>();
			if (componentInChildren != null)
			{
				if (componentInChildren.hasMountEquipped())
				{
					BoxCollider2D componentInChildren2 = entity.GetComponentInChildren<BoxCollider2D>();
					if (componentInChildren2 != null)
					{
						center.y += componentInChildren2.size.y;
					}
				}
				BoxCollider2D componentInChildren3 = componentInChildren.GetComponentInChildren<BoxCollider2D>();
				if (componentInChildren3 != null)
				{
					center.y += componentInChildren3.size.y / 2f;
				}
			}
			if (@string != stackLabel)
			{
				AddBattleTextObj().LoadDetails(@string, cOLOR_YELLOW, 3f, 0f, center.x, num, 0.8f, -30f, num3);
				num -= 30f;
				num3 += num2;
			}
		}
		float seconds = 0.05f / GetSpeed();
		StartCoroutine(DoTimer(seconds, RunQueue));
	}

	private void DoSkipTurn(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		if (_battleText)
		{
			sfsob.GetInt("bat70");
			sfsob.GetInt("bat71");
			string cOLOR_YELLOW = BattleText.COLOR_YELLOW;
			Vector2 center = Util.GetCenter(entity.transform);
			float num = center.y + 75f;
			float num2 = 0.1f;
			float num3 = 0f;
			CharacterDisplay componentInChildren = entity.GetComponentInChildren<CharacterDisplay>();
			if (componentInChildren != null)
			{
				if (componentInChildren.hasMountEquipped())
				{
					BoxCollider2D componentInChildren2 = entity.GetComponentInChildren<BoxCollider2D>();
					if (componentInChildren2 != null)
					{
						center.y += componentInChildren2.size.y;
					}
				}
				BoxCollider2D componentInChildren3 = componentInChildren.GetComponentInChildren<BoxCollider2D>();
				if (componentInChildren3 != null)
				{
					center.y += componentInChildren3.size.y / 2f;
				}
			}
			string @string = Language.GetString("stack_ability_freeze_frozen");
			AddBattleTextObj().LoadDetails(@string, cOLOR_YELLOW, 3f, 0f, center.x, num, 0.8f, -30f, num3);
			num -= 30f;
			num3 += num2;
		}
		float seconds = 0.05f / GetSpeed();
		StartCoroutine(DoTimer(seconds, RunQueue));
	}

	private void DoRestoreTurn(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		if (_battleText)
		{
			sfsob.GetInt("bat70");
			string cOLOR_YELLOW = BattleText.COLOR_YELLOW;
			Vector2 center = Util.GetCenter(entity.transform);
			float num = center.y + 75f;
			float num2 = 0.1f;
			float num3 = 0f;
			CharacterDisplay componentInChildren = entity.GetComponentInChildren<CharacterDisplay>();
			if (componentInChildren != null)
			{
				if (componentInChildren.hasMountEquipped())
				{
					BoxCollider2D componentInChildren2 = entity.GetComponentInChildren<BoxCollider2D>();
					if (componentInChildren2 != null)
					{
						center.y += componentInChildren2.size.y;
					}
				}
				BoxCollider2D componentInChildren3 = componentInChildren.GetComponentInChildren<BoxCollider2D>();
				if (componentInChildren3 != null)
				{
					center.y += componentInChildren3.size.y / 2f;
				}
			}
			string @string = Language.GetString("stack_ability_freeze_unfreeze");
			AddBattleTextObj().LoadDetails(@string, cOLOR_YELLOW, 3f, 0f, center.x, num, 0.8f, -30f, num3);
			num -= 30f;
			num3 += num2;
		}
		float seconds = 0.05f / GetSpeed();
		StartCoroutine(DoTimer(seconds, RunQueue));
	}

	private void queueBlockSkills(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		if (_battleText)
		{
			bool @bool = sfsob.GetBool("bat75");
			entity.setBlockSkillsData(@bool);
			string cOLOR_WHITE = BattleText.COLOR_WHITE;
			Vector2 center = Util.GetCenter(entity.transform);
			float num = center.y + 75f;
			float num2 = 0.1f;
			float num3 = 0f;
			CharacterDisplay componentInChildren = entity.GetComponentInChildren<CharacterDisplay>();
			if (componentInChildren != null)
			{
				if (componentInChildren.hasMountEquipped())
				{
					BoxCollider2D componentInChildren2 = entity.GetComponentInChildren<BoxCollider2D>();
					if (componentInChildren2 != null)
					{
						center.y += componentInChildren2.size.y;
					}
				}
				BoxCollider2D componentInChildren3 = componentInChildren.GetComponentInChildren<BoxCollider2D>();
				if (componentInChildren3 != null)
				{
					center.y += componentInChildren3.size.y / 2f;
				}
			}
			string text = "Cursed";
			AddBattleTextObj().LoadDetails(text, cOLOR_WHITE, 3f, 0f, center.x, num, 0.8f, -30f, num3);
			num -= 30f;
			num3 += num2;
		}
		float seconds = 0.05f / GetSpeed();
		StartCoroutine(DoTimer(seconds, RunQueue));
	}

	private void queueRootSkills(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		if (_battleText)
		{
			string cOLOR_GREEN = BattleText.COLOR_GREEN;
			Vector2 center = Util.GetCenter(entity.transform);
			float num = center.y + 75f;
			float num2 = 0.1f;
			float num3 = 0f;
			CharacterDisplay componentInChildren = entity.GetComponentInChildren<CharacterDisplay>();
			if (componentInChildren != null)
			{
				if (componentInChildren.hasMountEquipped())
				{
					BoxCollider2D componentInChildren2 = entity.GetComponentInChildren<BoxCollider2D>();
					if (componentInChildren2 != null)
					{
						center.y += componentInChildren2.size.y;
					}
				}
				BoxCollider2D componentInChildren3 = componentInChildren.GetComponentInChildren<BoxCollider2D>();
				if (componentInChildren3 != null)
				{
					center.y += componentInChildren3.size.y / 2f;
				}
			}
			string text = "Root";
			AddBattleTextObj().LoadDetails(text, cOLOR_GREEN, 3f, 0f, center.x, num, 0.8f, -30f, num3);
			num -= 30f;
			num3 += num2;
		}
		float seconds = 0.05f / GetSpeed();
		StartCoroutine(DoTimer(seconds, RunQueue));
	}

	private void queueBarrierSkills(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		if (_battleText)
		{
			string cOLOR_WHITE = BattleText.COLOR_WHITE;
			Vector2 center = Util.GetCenter(entity.transform);
			float num = center.y + 75f;
			float num2 = 0.1f;
			float num3 = 0f;
			CharacterDisplay componentInChildren = entity.GetComponentInChildren<CharacterDisplay>();
			if (componentInChildren != null)
			{
				if (componentInChildren.hasMountEquipped())
				{
					BoxCollider2D componentInChildren2 = entity.GetComponentInChildren<BoxCollider2D>();
					if (componentInChildren2 != null)
					{
						center.y += componentInChildren2.size.y;
					}
				}
				BoxCollider2D componentInChildren3 = componentInChildren.GetComponentInChildren<BoxCollider2D>();
				if (componentInChildren3 != null)
				{
					center.y += componentInChildren3.size.y / 2f;
				}
			}
			string text = "Barrier";
			AddBattleTextObj().LoadDetails(text, cOLOR_WHITE, 3f, 0f, center.x, num, 0.8f, -30f, num3);
			num -= 30f;
			num3 += num2;
		}
		float seconds = 0.05f / GetSpeed();
		StartCoroutine(DoTimer(seconds, RunQueue));
	}

	private void queueTriggerAbilitySkills(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		if (_battleText)
		{
			string utfString = sfsob.GetUtfString("bat80");
			string utfString2 = sfsob.GetUtfString("bat78");
			string utfString3 = sfsob.GetUtfString("bat79");
			string color = BattleText.COLOR_ORANGE;
			if (utfString3 != null)
			{
				color = utfString3;
			}
			Vector2 center = Util.GetCenter(entity.transform);
			float num = center.y + 75f;
			float num2 = 0.1f;
			float num3 = 0f;
			if (utfString2 == null)
			{
				RunQueue();
				return;
			}
			string text = Language.GetString(utfString2);
			if (utfString != null)
			{
				text = utfString + " " + text;
			}
			CharacterDisplay componentInChildren = entity.GetComponentInChildren<CharacterDisplay>();
			if (componentInChildren != null)
			{
				if (componentInChildren.hasMountEquipped())
				{
					BoxCollider2D componentInChildren2 = entity.GetComponentInChildren<BoxCollider2D>();
					if (componentInChildren2 != null)
					{
						center.y += componentInChildren2.size.y;
					}
				}
				BoxCollider2D componentInChildren3 = componentInChildren.GetComponentInChildren<BoxCollider2D>();
				if (componentInChildren3 != null)
				{
					center.y += componentInChildren3.size.y / 2f;
				}
			}
			AddBattleTextObj().LoadDetails(text, color, 3f, 0f, center.x, num, 0.8f, -30f, num3);
			num -= 30f;
			num3 += num2;
		}
		float seconds = 0.05f / GetSpeed();
		StartCoroutine(DoTimer(seconds, RunQueue));
	}

	private void queuedefenseIgnoreSkills(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		if (_battleText)
		{
			string cOLOR_BLUE = BattleText.COLOR_BLUE;
			Vector2 center = Util.GetCenter(entity.transform);
			float num = center.y + 75f;
			float num2 = 0.1f;
			float num3 = 0f;
			CharacterDisplay componentInChildren = entity.GetComponentInChildren<CharacterDisplay>();
			if (componentInChildren != null)
			{
				if (componentInChildren.hasMountEquipped())
				{
					BoxCollider2D componentInChildren2 = entity.GetComponentInChildren<BoxCollider2D>();
					if (componentInChildren2 != null)
					{
						center.y += componentInChildren2.size.y;
					}
				}
				BoxCollider2D componentInChildren3 = componentInChildren.GetComponentInChildren<BoxCollider2D>();
				if (componentInChildren3 != null)
				{
					center.y += componentInChildren3.size.y / 2f;
				}
			}
			string text = "Braveheart";
			AddBattleTextObj().LoadDetails(text, cOLOR_BLUE, 3f, 0f, center.x, num, 0.8f, -30f, num3);
			num -= 30f;
			num3 += num2;
		}
		float seconds = 0.05f / GetSpeed();
		StartCoroutine(DoTimer(seconds, RunQueue));
	}

	private void queueBuffDamageSkills(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		int int2 = sfsob.GetInt("bat77");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		if (_battleText)
		{
			string cOLOR_BLUE = BattleText.COLOR_BLUE;
			Vector2 center = Util.GetCenter(entity.transform);
			float num = center.y + 75f;
			float num2 = 0.1f;
			float num3 = 0f;
			CharacterDisplay componentInChildren = entity.GetComponentInChildren<CharacterDisplay>();
			if (componentInChildren != null)
			{
				if (componentInChildren.hasMountEquipped())
				{
					BoxCollider2D componentInChildren2 = entity.GetComponentInChildren<BoxCollider2D>();
					if (componentInChildren2 != null)
					{
						center.y += componentInChildren2.size.y;
					}
				}
				BoxCollider2D componentInChildren3 = componentInChildren.GetComponentInChildren<BoxCollider2D>();
				if (componentInChildren3 != null)
				{
					center.y += componentInChildren3.size.y / 2f;
				}
			}
			string text = "Fury";
			AddBattleTextObj().LoadDetails(text, cOLOR_BLUE, 3f, 0f, center.x, num + 20f, 0.8f, -30f, num3);
			AddBattleTextObj().LoadDetails("+" + int2, cOLOR_BLUE, 3f, 0f, center.x, num, 0.8f, -30f, num3);
			num -= 30f;
			num3 += num2;
		}
		float seconds = 0.05f / GetSpeed();
		StartCoroutine(DoTimer(seconds, RunQueue));
	}

	private string GetChangePercText(float value)
	{
		string text = "";
		if (value == 0f)
		{
			return text;
		}
		int num = Mathf.RoundToInt(value * 100f);
		if (num == 0)
		{
			return text;
		}
		text += " (";
		if (num > 0)
		{
			text += "+";
		}
		return text + Util.NumberFormat(num) + "%)";
	}

	private void DoActionMeterChange(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		int int2 = sfsob.GetInt("bat30");
		entity.SetMeter(int2);
		RunQueue();
	}

	private void DoActionMeterGain(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		int int2 = sfsob.GetInt("bat30");
		entity.AddMeterGain(int2);
		RunQueue();
	}

	private void DoActionBegin(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		int int2 = sfsob.GetInt("bat41");
		BattleEntity entity2 = GetEntity(int2);
		if (entity2 == null)
		{
			RunQueue();
			return;
		}
		int int3 = sfsob.GetInt("bat21");
		BattleEntity entity3 = GetEntity(int3);
		if (entity3 == null)
		{
			RunQueue();
			return;
		}
		AbilityActionRef abilityActionRef = AbilityBook.LookupAction(sfsob.GetInt("bat23"));
		if (abilityActionRef == null)
		{
			RunQueue();
			return;
		}
		BattleProjectileRef projectileRef = abilityActionRef.projectileRef;
		ParticleRef effectStart = abilityActionRef.effectStart;
		if (projectileRef == null && effectStart == null)
		{
			RunQueue();
			return;
		}
		Util.GetCenter(entity.transform);
		if (projectileRef != null && _battleAnimations)
		{
			Vector2 startPoint = (projectileRef.center ? entity.GetProjectilePoint(abilityActionRef.projectileSource, abilityActionRef) : new Vector2(entity.x, entity.y));
			Vector2 vector = entity3.transform.localPosition;
			startPoint.x += (entity.focused ? projectileRef.offset.x : (0f - projectileRef.offset.x));
			startPoint.y -= projectileRef.offset.y;
			if (projectileRef.spread > 0f)
			{
				vector = Util.spreadPoint(vector, projectileRef.spread, projectileRef.spread);
			}
			GameObject obj = new GameObject();
			obj.name = "BattleProjectile";
			obj.transform.SetParent(arenaContainer.transform, worldPositionStays: false);
			obj.transform.localPosition = Vector3.zero;
			BattleProjectile battleProjectile = obj.AddComponent<BattleProjectile>();
			battleProjectile.LoadDetails(projectileRef, GetSpeed(), startPoint, vector, entity2, entity);
			battleProjectile.COMPLETE.AddListener(OnProjectileComplete);
			if (entity.currentAbility != null && abilityActionRef.animate)
			{
				if (!entity.PlayAnimation(entity.currentAbility.animation))
				{
					battleProjectile.OnAddedToStage();
					return;
				}
				entity.SetCurrentProjectile(battleProjectile);
				if (entity.asset != null && entity.swfAsset != null)
				{
					entity.swfAsset.ANIMATION_TRIGGER.AddListener(OnAbilityAnimateAnimation);
					entity.swfAsset.ANIMATION_END.AddListener(OnAbilityAnimateAnimation);
				}
			}
			else
			{
				battleProjectile.OnAddedToStage();
			}
		}
		else
		{
			RunQueue();
		}
	}

	private void OnAbilityAnimateAnimation(SWFAsset asset)
	{
		asset.ANIMATION_TRIGGER.RemoveListener(OnAbilityAnimateAnimation);
		asset.ANIMATION_END.RemoveListener(OnAbilityAnimateAnimation);
		BattleEntity battleEntity = Util.GetParentClass(asset.gameObject, typeof(BattleEntity)) as BattleEntity;
		if (battleEntity != null && battleEntity.currentProjectile != null)
		{
			battleEntity.currentProjectile.OnAddedToStage();
			battleEntity.SetCurrentProjectile(null);
		}
		else
		{
			RunQueue();
		}
	}

	private void OnProjectileComplete(object e)
	{
		(e as BattleProjectile).COMPLETE.RemoveListener(OnProjectileComplete);
		RunQueue();
	}

	private void DoActionAbility(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		if (!entity.IsIdle())
		{
			StartCoroutine(DoWaitTillIdle(entity, delegate
			{
				DoActionAbility(sfsob);
			}));
			return;
		}
		bool @bool = sfsob.GetBool("bat31");
		int int2 = sfsob.GetInt("bat30");
		entity.SetMeter(int2);
		int int3 = sfsob.GetInt("bat22");
		AbilityRef abilityRef = AbilityBook.Lookup(int3);
		if (abilityRef == null)
		{
			RunQueue();
			return;
		}
		if (!@bool)
		{
			entity.SetCurrentAbility(abilityRef);
		}
		AbilityPositionRef position = abilityRef.position;
		Vector2 position2 = GetPosition(entity, position);
		if (position == null || (position2.x == entity.x && position2.y == entity.y))
		{
			DoActionAbilityPositionComplete(entity, abilityRef);
			return;
		}
		float num = (position.startSpeedScale ? GetMovementDuration(entity.position.x, position2.x, entity.position.y, position2.y, position.startSpeed) : (position.startSpeed / GetSpeed()));
		float delay = ((position.startDelay > 0f) ? (position.startDelay / GetSpeed()) : 0f);
		if (!_battleAnimations)
		{
			RunQueue();
		}
		else if (position.startAnimation.Length > 0)
		{
			entity.PlayAnimation(position.startAnimation);
			UnityAction<List<object>> onComplete = delegate
			{
				DoActionAbilityPositionComplete(entity, abilityRef);
			};
			if (num < 0.1f)
			{
				DoActionAbilityPositionComplete(entity, abilityRef);
			}
			else
			{
				com.ultrabit.bitheroes.model.utility.Tween.StartLocalMovement(entity.gameObject, position2.x, position2.y, num, delay, onComplete);
			}
		}
		else if (entity.asset != null)
		{
			if (!entity.PlayAnimation(abilityRef.animation))
			{
				entity.PlayAnimation("idle");
				RunQueue();
				return;
			}
			entity.swfAsset.ANIMATION_TRIGGER.AddListener(OnAbilityAnimation);
			entity.swfAsset.ANIMATION_END.AddListener(OnAbilityAnimation);
			com.ultrabit.bitheroes.model.utility.Tween.StartLocalMovement(entity.gameObject, position2.x, position2.y, num, delay);
			GameData.instance.audioManager.PlaySound(abilityRef.sound);
		}
		else
		{
			UnityAction<List<object>> onComplete2 = delegate
			{
				DoActionAbilityPositionComplete(entity, abilityRef);
			};
			com.ultrabit.bitheroes.model.utility.Tween.StartLocalMovement(entity.gameObject, position2.x, position2.y, num, delay, onComplete2);
		}
	}

	private void DoActionAbilityPositionComplete(BattleEntity entity, AbilityRef abilityRef)
	{
		if (!_battleAnimations)
		{
			RunQueue();
			return;
		}
		if (entity.asset != null && entity.swfAsset != null && abilityRef.animation != null && !abilityRef.animation.Equals(""))
		{
			entity.swfAsset.ANIMATION_TRIGGER.AddListener(OnAbilityAnimation);
			entity.swfAsset.ANIMATION_END.AddListener(OnAbilityAnimation);
		}
		if (!entity.PlayAnimation(abilityRef.animation))
		{
			entity.PlayAnimation("idle");
			RunQueue();
		}
		else
		{
			GameData.instance.audioManager.PlaySound(abilityRef.sound);
		}
	}

	private Vector2 GetPosition(BattleEntity sourceEntity, AbilityPositionRef positionRef)
	{
		if (positionRef != null)
		{
			Vector2 vector = (sourceEntity.focused ? positionRef.offset : new Vector2(0f - positionRef.offset.x, positionRef.offset.y));
			Rect rect = (sourceEntity.focused ? _focusBounds : _otherBounds);
			Rect rect2 = (sourceEntity.focused ? _otherBounds : _focusBounds);
			BattleEntity nextEffectedEntity = GetNextEffectedEntity();
			Vector2 vector2 = ((nextEffectedEntity != null) ? new Vector2(nextEffectedEntity.x, nextEffectedEntity.y) : new Vector2(sourceEntity.x, sourceEntity.y));
			switch (positionRef.type)
			{
			case 0:
				return new Vector2(sourceEntity.x + vector.x, sourceEntity.y + vector.y);
			case 1:
				return new Vector2(0f + vector.x, rect.y + vector.y);
			case 2:
				return new Vector2(vector2.x + vector.x, vector2.y + vector.y);
			case 3:
				return new Vector2(vector2.x + vector.x, vector2.y + vector.y);
			case 5:
				return new Vector2(rect2.x + vector.x, rect2.y + vector.y);
			case 10:
				return new Vector2(rect.x + vector.x, rect.y + vector.y);
			}
		}
		return new Vector2(sourceEntity.x, sourceEntity.y);
	}

	private BattleEntity GetNextEffectedEntity()
	{
		foreach (SFSObject item in _queue)
		{
			if (item.GetInt("act0") == 2)
			{
				int @int = item.GetInt("bat7");
				return GetEntity(@int);
			}
		}
		return null;
	}

	private void OnAbilityAnimation(SWFAsset asset)
	{
		asset.ANIMATION_TRIGGER.RemoveListener(OnAbilityAnimation);
		asset.ANIMATION_END.RemoveListener(OnAbilityAnimation);
		RunQueue();
	}

	private void DoActionTurnStart(SFSObject sfsob)
	{
		Debug.Log("======================== DoActionTurnStart ============================");
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		_turnTimerStarted = false;
		SetCurrentEntity(entity);
		AddAbilityTiles();
		RunQueue();
	}

	public float GetMovementDuration(float startX, float endX, float startY, float endY, float duration)
	{
		return GetBattleMovementDuration(startX, endX, startY, endY, duration) / GetSpeed();
	}

	public static float GetBattleMovementDuration(float startX, float endX, float startY, float endY, float duration)
	{
		int num = 300;
		float num2 = Util.GetDistance(startX, startY, endX, endY) / (float)num;
		return duration * num2;
	}

	private void DoActionTurnEnd(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (!entity)
		{
			RunQueue();
			return;
		}
		if (!entity.IsIdle())
		{
			StartCoroutine(DoWaitTillIdle(entity, delegate
			{
				DoActionTurnEnd(sfsob);
			}));
			return;
		}
		if (sfsob.ContainsKey("bat62") && sfsob.GetBool("bat62"))
		{
			ClearAbilityTiles();
			entity.SetTimeElapsed(0f);
		}
		AbilityRef currentAbility = entity.currentAbility;
		if (currentAbility == null)
		{
			RunQueue();
			return;
		}
		if (!_battleAnimations || currentAbility.position == null)
		{
			RunQueue();
		}
		else
		{
			float num = (currentAbility.position.endSpeedScale ? GetMovementDuration(entity.x, entity.position.x, entity.y, entity.position.y, currentAbility.position.endSpeed) : (currentAbility.position.endSpeed / GetSpeed()));
			float num2 = ((currentAbility.position.endDelay > 0f) ? (currentAbility.position.endDelay / GetSpeed()) : 0f);
			if (num == 0f)
			{
				RunQueue();
			}
			else
			{
				UnityAction<List<object>> onComplete = delegate
				{
					OnActionTurnEndComplete(entity);
				};
				if (num >= 0.1f || num2 >= 0.1f)
				{
					entity.PlayAnimation(currentAbility.position.endAnimation);
					com.ultrabit.bitheroes.model.utility.Tween.StartLocalMovement(entity.gameObject, entity.position.x, entity.position.y, num, num2, onComplete);
				}
				else
				{
					entity.transform.localPosition = new Vector3(entity.position.x, entity.position.y, 0f);
					OnActionTurnEndComplete(entity);
				}
			}
		}
		entity.SetCurrentAbility(null);
	}

	private void OnActionTurnEndComplete(BattleEntity entity)
	{
		OnEntityStop(entity);
		RunQueue();
	}

	private void DoActionDeathChange(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		bool @bool = sfsob.GetBool("bat25");
		float duration = 0.15f / GetSpeed();
		float num = 1f;
		if (entity.type == 1 || entity.type == 3)
		{
			entity.UpdateAsset();
		}
		else if (@bool)
		{
			num = 0f;
		}
		if (_autoPilotDeathDisable && GameData.instance.PROJECT.character.autoPilot && @bool && entity.controller == GameData.instance.PROJECT.character.id)
		{
			GameData.instance.PROJECT.character.autoPilot = false;
		}
		TweenEntityAlpha(num, duration, entity, num == 0f, @bool);
	}

	public void TweenEntityAlpha(float alpha, float duration, BattleEntity entity, bool disableAnimators, bool isDead = false)
	{
		if (tweenAlpha != null)
		{
			tweenAlpha.Kill();
		}
		if (disableAnimators)
		{
			SpriteRenderer[] renderers = entity.GetComponentsInChildren<SpriteRenderer>();
			Animator[] componentsInChildren = entity.GetComponentsInChildren<Animator>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			ParticleSystem[] componentsInChildren2 = entity.GetComponentsInChildren<ParticleSystem>();
			if (componentsInChildren2 != null)
			{
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					ParticleSystem.EmissionModule emission = componentsInChildren2[j].emission;
					emission.enabled = false;
				}
			}
			float alphaInit = 1f;
			tweenAlpha = DOTween.To(() => alphaInit, delegate(float x)
			{
				alphaInit = x;
			}, alpha, duration).SetEase(Ease.Linear).OnUpdate(delegate
			{
				for (int k = 0; k < renderers.Length; k++)
				{
					if (renderers[k] != null)
					{
						Color color = renderers[k].color;
						color.a = alphaInit;
						renderers[k].color = color;
						renderers[k].material.SetColor("_Color", renderers[k].color);
					}
				}
			})
				.OnComplete(delegate
				{
					RunQueue();
					if (isDead)
					{
						RemoveEntity(entity);
						if (entity != null && entity.gameObject != null)
						{
							Object.Destroy(entity.gameObject);
						}
					}
				});
		}
		else
		{
			RunQueue();
		}
	}

	private void DoActionComplete(SFSObject sfsob)
	{
		_completed = true;
		GameData.instance.PROJECT.CheckTutorialChanges();
		RunQueue();
	}

	private void DoActionVictory(SFSObject sfsob)
	{
		if (_replay)
		{
			DoReplayComplete();
			return;
		}
		if (sfsob.GetInt("bat5") != GameData.instance.PROJECT.character.id)
		{
			RunQueue();
			return;
		}
		_results = true;
		long @long = sfsob.GetLong("cha5");
		int @int = sfsob.GetInt("cha9");
		int int2 = sfsob.GetInt("cha4");
		int int3 = sfsob.GetInt("cha19");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		long num = @long - GameData.instance.PROJECT.character.exp;
		int num2 = @int - GameData.instance.PROJECT.character.gold;
		int creditsGained = 0;
		_ = GameData.instance.PROJECT.character.level;
		_ = GameData.instance.PROJECT.character.points;
		if (GameData.instance.PROJECT.dungeon != null)
		{
			KongregateAnalytics.checkEconomyTransaction(KongregateAnalytics.getBattleEconomyType(this), null, list, sfsob, KongregateAnalytics.getBattleEconomyContext(this), 2, _type != 2);
		}
		Dungeon dungeon = GameData.instance.PROJECT.dungeon;
		if (dungeon != null)
		{
			GameData.instance.PROJECT.character.addDungeonLoot(new ItemData(ItemBook.Lookup(3, 3), (int)num), dungeon.type, dungeon.dungeonRef.id);
			GameData.instance.PROJECT.character.addDungeonLoot(new ItemData(ItemBook.Lookup(1, 3), num2), dungeon.type, dungeon.dungeonRef.id);
		}
		GameData.instance.PROJECT.character.exp = @long;
		GameData.instance.PROJECT.character.gold = @int;
		GameData.instance.PROJECT.character.level = int2;
		GameData.instance.PROJECT.character.points = int3;
		GameData.instance.PROJECT.character.addItems(list, isDungeonLoot: true);
		Transform transform = GameData.instance.windowGenerator.NewVictoryWindow();
		transform.GetComponent<VictoryWindow>().ON_CLOSE.AddListener(OnDialogClose);
		string @string;
		bool flag;
		switch (_type)
		{
		case 2:
		case 3:
		case 6:
		case 7:
		case 9:
			@string = Language.GetString("ui_town");
			flag = true;
			break;
		default:
			@string = Language.GetString("ui_continue");
			flag = false;
			break;
		}
		VictoryWindow component = transform.GetComponent<VictoryWindow>();
		int num3 = _type;
		List<BattleStat> battleStats = _battleStats;
		bool isCloseRed = flag;
		component.LoadDetails(num3, num, num2, list, battleStats, shouldPlayMusic: true, null, null, @string, isVictorious: true, isCloseRed);
		if (_type == 2)
		{
			GameData.instance.PROJECT.character.analytics.incrementValue(BHAnalytics.PVP_BATTLES_WON);
		}
		TrackEnd("Win", creditsGained, num2);
		RunQueue();
	}

	private void DoActionDefeat(SFSObject sfsob)
	{
		if (_replay)
		{
			DoReplayComplete();
			return;
		}
		if (sfsob.GetInt("bat5") != GameData.instance.PROJECT.character.id)
		{
			RunQueue();
			return;
		}
		_results = true;
		_defeated = true;
		Transform obj = GameData.instance.windowGenerator.NewDefeatableVictoryWindow();
		List<ItemData> items = ((GameData.instance.PROJECT.dungeon != null) ? GameData.instance.PROJECT.character.dungeonLootItems : null);
		obj.GetComponent<DefeatableVictoryWindow>().ON_CLOSE.AddListener(OnDialogClose);
		obj.GetComponent<DefeatableVictoryWindow>().LoadDetails(_type, isVictorious: false, 0L, 0, items);
		TrackEnd("Lose");
		RunQueue();
	}

	private void DoActionItemUsed(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		int int2 = sfsob.GetInt("bat32");
		ItemData itemData = ItemData.fromSFSObject(sfsob);
		entity.SetConsumables(int2);
		if (entity.controller == GameData.instance.PROJECT.character.id && !_replay)
		{
			GameData.instance.PROJECT.character.removeItem(itemData);
			if (!GameData.instance.PROJECT.character.tutorial.GetState(11))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(11);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
		}
		RunQueue();
	}

	private void DoActionOrder(SFSObject sfsob)
	{
		List<int> list = Util.arrayToIntegerVector(sfsob.GetIntArray("bat2"));
		List<int> list2 = new List<int>();
		list2.AddRange(list);
		List<BattleEntity> list3 = new List<BattleEntity>();
		for (int i = 0; i < _entities.Count; i++)
		{
			BattleEntity battleEntity = _entities[i];
			bool flag = false;
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j] == battleEntity.index)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				list3.Add(GetEntity(list2[0]));
				list2.RemoveAt(0);
			}
			else
			{
				list3.Add(battleEntity);
			}
		}
		_entities.Clear();
		foreach (BattleEntity item in list3)
		{
			_entities.Add(item);
		}
		RunQueue();
	}

	private List<Point> GetEntityPositions()
	{
		List<Point> list = new List<Point>();
		foreach (BattleEntity entity in _entities)
		{
			list.Add(Point.create(entity.position.x, entity.position.y));
		}
		return list;
	}

	private void DoActionCaptureSet(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		if (sfsob.GetBool("bat33"))
		{
			AddCaptureEntity(entity);
		}
		else
		{
			RemoveCaptureEntity(entity);
		}
		RunQueue();
	}

	private void DoActionCaptureComplete(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		bool @bool = sfsob.GetBool("bat34");
		ItemData itemData = ItemData.fromSFSObject(sfsob);
		float speed = 1f;
		if (itemData != null)
		{
			GameData.instance.PROJECT.character.addItem(itemData, dispatch: true, isDungeonLoot: true);
		}
		float num = 0.25f / GetSpeed();
		if (@bool)
		{
			GameData.instance.audioManager.PlaySoundLink("familiarsuccess");
			if (GameData.instance.SAVE_STATE.autopersuadeFamiliarsGems || GameData.instance.SAVE_STATE.autopersuadeFamiliarsGold)
			{
				AddBattleTextObj().LoadDetails(@bool ? Language.GetString("battle_capture_success") : Language.GetString("battle_capture_failed"), @bool ? BattleText.COLOR_YELLOW : BattleText.COLOR_ORANGE, 5f / speed, 0f, entity.transform.position.x, entity.transform.position.y + entity.hitbox.size.y);
				SuccessWalk(num, entity);
			}
			else
			{
				entity.StopAnimation();
				entity.Remove();
				GameData.instance.windowGenerator.NewFamiliarCaptureSuccessWindow(entity.captureFamiliarRef).DESTROYED.AddListener(delegate(object e)
				{
					OnCaptureSuccessWindowClosed(e, speed, entity);
				});
				StopTimeoutTimer();
			}
			GameData.instance.PROJECT.character.updateAchievements();
		}
		else
		{
			AddBattleTextObj().LoadDetails(@bool ? Language.GetString("battle_capture_success") : Language.GetString("battle_capture_failed"), @bool ? BattleText.COLOR_YELLOW : BattleText.COLOR_ORANGE, 5f / speed, 0f, entity.transform.position.x, entity.transform.position.y + entity.hitbox.size.y);
			GameData.instance.audioManager.PlaySoundLink("familiarfail");
			entity.PlayAnimation("hit");
			StartCoroutine(DoTimer(num, RunQueue));
		}
	}

	private void OnCaptureSuccessWindowClosed(object e, float speed, BattleEntity entity)
	{
		(e as FamiliarCaptureSuccessWindow).DESTROYED.RemoveListener(delegate(object a)
		{
			OnCaptureSuccessWindowClosed(a, speed, entity);
		});
		AddBattleTextObj().LoadDetails(Language.GetString("battle_capture_success"), BattleText.COLOR_YELLOW, 5f / speed, 0f, entity.transform.position.x, entity.transform.position.y + entity.hitbox.size.y);
		RunQueue();
	}

	private void SuccessWalk(float delay, BattleEntity entity)
	{
		float offscreenPosition = GetOffscreenPosition(left: true);
		float movementDuration = GetMovementDuration(entity.transform.position.x, offscreenPosition, entity.y, entity.y, 0.5f);
		DoTimer(delay, delegate
		{
			entity.PlayAnimation("walk");
		});
		List<object> list = new List<object>();
		list.Add(entity);
		com.ultrabit.bitheroes.model.utility.Tween.StartMovement(entity.gameObject, offscreenPosition, entity.transform.position.y, movementDuration, delay, OnCaptureEntityComplete, list);
	}

	private void DoActionEntityValues(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		int int2 = sfsob.GetInt("bat11");
		int int3 = sfsob.GetInt("bat51");
		int int4 = sfsob.GetInt("bat56");
		int int5 = sfsob.GetInt("bat30");
		long @long = sfsob.GetLong("bat38");
		if (entity.damageGained > (float)int4 && _battleText)
		{
			Vector2 center = Util.GetCenter(entity.transform);
			CharacterDisplay componentInChildren = entity.GetComponentInChildren<CharacterDisplay>();
			if (componentInChildren != null)
			{
				if (componentInChildren.hasMountEquipped())
				{
					BoxCollider2D componentInChildren2 = entity.GetComponentInChildren<BoxCollider2D>();
					if (componentInChildren2 != null)
					{
						center.y += componentInChildren2.size.y;
					}
				}
				BoxCollider2D componentInChildren3 = componentInChildren.GetComponentInChildren<BoxCollider2D>();
				if (componentInChildren3 != null)
				{
					center.y += componentInChildren3.size.y / 2f;
				}
			}
			AddBattleTextObj().LoadDetails(Language.GetString("battle_enrage"), BattleText.COLOR_ORANGE, 3f, 0f, center.x, center.y + 30f, 0.8f);
			GameData.instance.audioManager.PlaySoundLink("critical");
		}
		entity.SetShieldCurrent(int3);
		entity.SetHealthCurrent(int2);
		entity.SetDamageGained(int4);
		entity.SetMeter(int5);
		entity.SetTimeElapsed(@long);
		RunQueue();
	}

	private void DoActionEntitiesRemove(SFSObject sfsob)
	{
		int[] intArray = sfsob.GetIntArray("bat2");
		if (intArray == null || intArray.Length == 0)
		{
			RunQueue();
			return;
		}
		int[] array = intArray;
		foreach (int index in array)
		{
			BattleEntity entity = GetEntity(index);
			if (entity != null)
			{
				RemoveEntity(entity);
			}
		}
		RunQueue();
	}

	private void DoActionEntitiesAdd(SFSObject sfsob)
	{
		List<BattleEntity> list = BattleEntity.ListFromSFSObject(sfsob);
		if (list == null || list.Count <= 0)
		{
			RunQueue();
			return;
		}
		LoadEntities(list);
		foreach (BattleEntity item in list)
		{
			AddEntity(item);
			item.UpdateAsset();
		}
		Debug.Log("             DoActionEntitiesAdd  add      [RunQueue] event            ");
		SetEntityPositions(list);
		TweenEntityPositions(list, RunQueue);
	}

	private void DoActionTeamDataChange(SFSObject sfsob)
	{
		BattleTeamData battleTeamData = BattleTeamData.fromSFSObject(sfsob);
		BattleTeamData teamData = GetTeamData(battleTeamData.attacker);
		if (battleTeamData.attacker)
		{
			_attackerData = battleTeamData;
		}
		else
		{
			_defenderData = battleTeamData;
		}
		UpdateCountBar();
		if (_type == 8)
		{
			if (!GetTeamFocused(battleTeamData.attacker) && battleTeamData.poolCurrent > teamData.poolCurrent)
			{
				DoScreenMove();
			}
			else
			{
				RunQueue();
			}
		}
		else
		{
			RunQueue();
		}
	}

	private void DoScreenMove()
	{
		float duration = 1.5f / GetSpeed();
		int num = Mathf.RoundToInt((float)((double)GameData.instance.main.mainCamera.orthographicSize * 2.0) / (float)Screen.height * (float)Screen.width);
		List<BattleEntity> focusedEntities = GetFocusedEntities();
		foreach (BattleEntity item in focusedEntities)
		{
			item.PlayAnimation("walk");
		}
		List<object> list = new List<object>();
		list.Add(focusedEntities);
		list.Add(num);
		com.ultrabit.bitheroes.model.utility.Tween.StartMovement(_asset, _asset.transform.localPosition.x - (float)num, _asset.transform.localPosition.y, duration, 0f, OnScreenMoveComplete, list, "true");
	}

	private void OnScreenMoveComplete(List<object> parameters)
	{
		List<BattleEntity> obj = parameters[0] as List<BattleEntity>;
		int value = (parameters[1] as int?).Value;
		foreach (BattleEntity item in obj)
		{
			item.PlayAnimation("idle");
		}
		if (_asset.transform.localPosition.x - (float)(value * 2) <= 0f - Mathf.Round(_asset.GetComponentInChildren<SpriteRenderer>().sprite.rect.width * 3f))
		{
			_asset.transform.localPosition = new Vector3(0f, _asset.transform.localPosition.y, _asset.transform.localPosition.z);
		}
		RunQueue();
	}

	private void DoActionResults(SFSObject sfsob)
	{
		if (_replay)
		{
			DoReplayComplete();
			return;
		}
		if (sfsob.GetInt("bat5") != GameData.instance.PROJECT.character.id)
		{
			RunQueue();
			return;
		}
		_results = true;
		bool @bool = sfsob.GetBool("bat47");
		long @long = sfsob.GetLong("cha5");
		int @int = sfsob.GetInt("cha9");
		int int2 = sfsob.GetInt("cha4");
		int int3 = sfsob.GetInt("cha19");
		List<ItemData> items = ItemData.listFromSFSObject(sfsob);
		int num = (int)(@long - GameData.instance.PROJECT.character.exp);
		int num2 = @int - GameData.instance.PROJECT.character.gold;
		int creditsGained = 0;
		_ = GameData.instance.PROJECT.character.level;
		_ = GameData.instance.PROJECT.character.points;
		GameData.instance.PROJECT.character.exp = @long;
		GameData.instance.PROJECT.character.gold = @int;
		GameData.instance.PROJECT.character.level = int2;
		GameData.instance.PROJECT.character.points = int3;
		GameData.instance.PROJECT.character.addItems(items, isDungeonLoot: true);
		Transform obj = GameData.instance.windowGenerator.NewDefeatableVictoryWindow();
		obj.GetComponent<DefeatableVictoryWindow>().ON_CLOSE.AddListener(OnDialogClose);
		obj.GetComponent<DefeatableVictoryWindow>().LoadDetails(_type, @bool, num, num2, items);
		TrackEnd(@bool ? "Win" : "Lose", creditsGained, num2);
		RunQueue();
	}

	private void DoActionAbilityData(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat7");
		BattleEntity entity = GetEntity(@int);
		if (entity == null)
		{
			RunQueue();
			return;
		}
		List<BattleAbilityData> abilityData = BattleAbilityData.listFromSFSObject(sfsob);
		entity.SetAbilityData(abilityData);
		RunQueue();
	}

	private bool RemoveEntity(BattleEntity entityRemoved)
	{
		for (int i = 0; i < _entities.Count; i++)
		{
			if (_entities[i] == entityRemoved)
			{
				_entities.RemoveAt(i);
				entityRemoved.Remove();
				return true;
			}
		}
		return false;
	}

	private bool AddEntity(BattleEntity entityAdded)
	{
		if ((bool)GetEntity(entityAdded.index))
		{
			return false;
		}
		_entities.Add(entityAdded);
		UpdateEntityParents();
		return true;
	}

	private void OnCaptureEntityComplete(List<object> objs)
	{
		BattleEntity obj = objs[0] as BattleEntity;
		obj.StopAnimation();
		obj.Remove();
		RunQueue();
	}

	private void DoReplayComplete()
	{
		_results = true;
		GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_complete"), Language.GetString("battle_replay_complete"), null, delegate
		{
			OnDialogClose();
		});
	}

	private void OnDialogRerun()
	{
		OnDialogClose();
		GameData.instance.PROJECT.dungeon.extension.OnExitRerun();
	}

	private void OnDialogClose(object e = null)
	{
		DispatchEvent(new CustomSFSXEvent(CustomSFSXEvent.COMPLETE));
	}

	private void SetTime(long time)
	{
		_time = time;
		_timeBefore = Time.realtimeSinceStartup * 1000f;
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.AddListener(OnNextFrame);
	}

	private void Update()
	{
		base.transform.position = ((GameData.instance.main.mainCamera != null) ? new Vector3(GameData.instance.main.mainCamera.transform.position.x, GameData.instance.main.mainCamera.transform.position.y, base.transform.position.z) : Vector3.zero);
		if ((EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null) || getLocked() || GameData.instance.tutorialManager.hasPopup)
		{
			return;
		}
		key = -1;
		if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Keypad1))
		{
			key = 1;
		}
		if (Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Keypad2))
		{
			key = 2;
		}
		if (Input.GetKeyUp(KeyCode.Alpha3) || Input.GetKeyUp(KeyCode.Keypad3))
		{
			key = 3;
		}
		if (Input.GetKeyUp(KeyCode.Alpha4) || Input.GetKeyUp(KeyCode.Keypad4))
		{
			key = 4;
		}
		if (Input.GetKeyUp(KeyCode.Alpha5) || Input.GetKeyUp(KeyCode.Keypad5))
		{
			key = 5;
		}
		if (Input.GetKeyUp(KeyCode.Alpha6) || Input.GetKeyUp(KeyCode.Keypad6))
		{
			key = 6;
		}
		if (Input.GetKeyUp(KeyCode.Alpha7) || Input.GetKeyUp(KeyCode.Keypad7))
		{
			key = 7;
		}
		if (Input.GetKeyUp(KeyCode.Alpha8) || Input.GetKeyUp(KeyCode.Keypad8))
		{
			key = 8;
		}
		if (Input.GetKeyUp(KeyCode.Alpha9) || Input.GetKeyUp(KeyCode.Keypad9))
		{
			key = 9;
		}
		if (key >= 0)
		{
			if (noTargetsOpened)
			{
				return;
			}
			_familiarWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(FamiliarWindow)) as FamiliarWindow;
			_characterProfileWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(CharacterProfileWindow)) as CharacterProfileWindow;
			_selectWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(BattleEntitySelectWindow)) as BattleEntitySelectWindow;
			if (_familiarWindow != null || _characterProfileWindow != null)
			{
				return;
			}
			if (_selectWindow != null)
			{
				int index = key - 1;
				_selectWindow.DoSelectIndex(index);
				return;
			}
			foreach (AbilityTile abilityTile in _abilityTiles)
			{
				if (!(abilityTile == null) && abilityTile.key != null && abilityTile.clickable && int.TryParse(abilityTile.key, out var result) && result == key)
				{
					DoAbilitySelect(abilityTile.abilityRef);
				}
			}
		}
		if (Input.GetKeyUp(KeyCode.E) && _currentEntity != null && _currentEntity.controller == GameData.instance.PROJECT.character.id && _currentEntity.damageGained > 0f)
		{
			DoUseDamageGain();
		}
	}

	private void OnNextFrame(object e)
	{
		_timeAfter = Time.realtimeSinceStartup * 1000f;
		float num = (_timeAfter - _timeBefore) * GetSpeed() * VariableBook.battleSpeedMultiplier;
		if (_time - num <= 0f)
		{
			num = _time;
		}
		_time -= num;
		foreach (BattleEntity entity in _entities)
		{
			if (!entity.isDead)
			{
				float num2 = num / (float)GameData.instance.main.DISPATCHER.currentFPS;
				float num3 = VariableBook.battleMilliseconds / (float)entity.cooldown * num2;
				int num4 = (int)((float)entity.meterGain * num3);
				int num5 = entity.SetMeterGain(entity.meterGain - num4);
				entity.AddMeter(-num5);
				entity.AddTimeElapsed(num);
			}
		}
		if (_time <= 0f)
		{
			ClearMeterGain();
			GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnNextFrame);
			RunQueue();
		}
		_timeBefore = _timeAfter;
	}

	private void ClearMeterGain()
	{
		foreach (BattleEntity entity in _entities)
		{
			entity.AddMeter(entity.meterGain);
			entity.SetMeterGain(0);
		}
	}

	public void SetEntityFocus()
	{
		List<int> list = new List<int>();
		bool attackerFocus = GetAttackerFocus();
		bool flag = !attackerFocus;
		foreach (BattleEntity entity in _entities)
		{
			if (entity.controller > 0 && !Util.intVectorContainsInt(list, entity.controller))
			{
				list.Add(entity.controller);
			}
			entity.SetFocused(entity.attacker ? attackerFocus : flag);
			entity.UpdateAsset();
		}
		_realtime = list.Count > 1;
	}

	public bool GetAttackerFocus()
	{
		foreach (BattleEntity entity in _entities)
		{
			if (entity.controller == GameData.instance.PROJECT.character.id)
			{
				return entity.attacker;
			}
		}
		return true;
	}

	public void SetEntityPositions(List<BattleEntity> entities)
	{
		List<BattleEntity> focusTeam = GetFocusTeam(focused: true, entities);
		int num = ((focusTeam.Count == 1) ? 2 : focusTeam.Count);
		int num2 = ((focusTeam.Count > 0) ? (260 - 260 / focusTeam.Count) : 0);
		int num3 = num2 / (num - 1);
		int num4 = 0;
		int num5 = -200;
		int num6 = GetEntityPosition(num4);
		for (int i = 0; i < focusTeam.Count; i++)
		{
			BattleEntity battleEntity = focusTeam[i];
			if (battleEntity.GetAnchorTop())
			{
				num6 = -43;
			}
			Point offset = battleEntity.GetOffset();
			battleEntity.x = (float)(num5 - i * num3) + offset.x;
			battleEntity.y = (float)num6 + offset.y;
			battleEntity.SetPosition(new Vector2(Mathf.Round(battleEntity.x), Mathf.Round(battleEntity.y)));
			if (num4 >= ENTITY_POSITIONS.Length)
			{
				num4 = 0;
			}
			num4++;
			num6 = GetEntityPosition(num4);
		}
		foreach (BattleEntity item in focusTeam)
		{
			item.x += (focusTeam.Count - 1) * (num3 / 2);
		}
		List<BattleEntity> focusTeam2 = GetFocusTeam(focused: false, entities);
		int num7 = ((focusTeam2.Count > 0) ? (260 - 260 / focusTeam2.Count) : 0);
		int num8 = num7 / ((focusTeam2.Count - 1 == 0) ? 1 : (focusTeam2.Count - 1));
		int num9 = 0;
		int num10 = 200;
		int num11 = GetEntityPosition(num9);
		for (int j = 0; j < focusTeam2.Count; j++)
		{
			BattleEntity battleEntity2 = focusTeam2[j];
			if (battleEntity2.GetAnchorTop())
			{
				num11 = -43;
			}
			Point offset2 = battleEntity2.GetOffset();
			battleEntity2.x = (float)(num10 + j * num8) + offset2.x * -1f;
			battleEntity2.y = (float)num11 + offset2.y;
			battleEntity2.SetPosition(new Vector2(Mathf.Round(battleEntity2.x), Mathf.Round(battleEntity2.y)));
			if (num9 >= ENTITY_POSITIONS.Length)
			{
				num9 = 0;
			}
			num9++;
			num11 = GetEntityPosition(num9);
		}
		foreach (BattleEntity item2 in focusTeam2)
		{
			item2.x -= (focusTeam2.Count - 1) * (num8 / 2);
		}
		_focusBounds = CreateBounds(-200, -75, num2, -65);
		_otherBounds = CreateBounds(200, -75, num7, -65);
		foreach (BattleEntity entity in entities)
		{
			entity.SetBattle(this);
			entity.SetPosition(new Vector2(Mathf.Round(entity.x), Mathf.Round(entity.y)));
			if (!entity.isDead)
			{
				entity.gameObject.SetActive(value: true);
				entity.x = GetOffscreenPosition(entity.focused);
			}
		}
		OnUpdate(null);
	}

	private Rect CreateBounds(int x, int y, int w, int h)
	{
		return new Rect(x, y, w, h);
	}

	private float GetOffscreenPosition(bool left)
	{
		float num = (float)((double)GameData.instance.main.mainCamera.orthographicSize * 2.0) / (float)Screen.height * (float)Screen.width / 2f + 150f;
		if (!left)
		{
			return num;
		}
		return 0f - num;
	}

	private void TweenEntityPositions(List<BattleEntity> entities, UnityAction complete)
	{
		List<BattleEntity> focusTeam = GetFocusTeam(focused: true, entities);
		List<BattleEntity> focusTeam2 = GetFocusTeam(focused: false, entities);
		bool flag = true;
		float num = 0.3f / GetSpeed();
		BattleEntity animatingEntity = null;
		while (focusTeam.Count > 0 || focusTeam2.Count > 0)
		{
			if (flag && focusTeam.Count > 0)
			{
				animatingEntity = focusTeam[0];
				focusTeam.RemoveAt(0);
			}
			else if (!flag && focusTeam2.Count > 0)
			{
				animatingEntity = focusTeam2[0];
				focusTeam2.RemoveAt(0);
			}
			if (animatingEntity == null)
			{
				if (focusTeam.Count > 0)
				{
					animatingEntity = focusTeam[0];
					focusTeam.RemoveAt(0);
				}
				else if (focusTeam2.Count > 0)
				{
					animatingEntity = focusTeam2[0];
					focusTeam2.RemoveAt(0);
				}
			}
			if (animatingEntity.x != animatingEntity.position.x || animatingEntity.y != animatingEntity.position.y)
			{
				float movementDuration = GetMovementDuration(animatingEntity.x, animatingEntity.position.x, animatingEntity.y, animatingEntity.position.y, 0.5f);
				StartCoroutine(DoTimer(num, delegate
				{
					OnEntityStart(animatingEntity);
				}));
				UnityAction<List<object>> onComplete = delegate
				{
					OnEntityStop(animatingEntity);
				};
				com.ultrabit.bitheroes.model.utility.Tween.StartLocalMovement(animatingEntity.gameObject, animatingEntity.position.x, animatingEntity.position.y, movementDuration, num, onComplete);
				num += movementDuration / 2f;
			}
			flag = !flag;
			animatingEntity = null;
		}
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.AddListener(OnUpdate);
		OnUpdate(null);
		num += 0.4f;
		Debug.Log(">>>>>>>>>>> Start the queue after the animations finished <<<<<<<<<<<<<<<<");
		StartCoroutine(DoTimer(num, delegate
		{
			complete?.Invoke();
		}));
	}

	private void OnEntityStart(BattleEntity entity)
	{
		if (entity != null)
		{
			entity.gameObject.SetActive(value: true);
			entity.PlayAnimation("walk");
		}
	}

	private void OnEntityStop(BattleEntity entity)
	{
		if (entity != null)
		{
			entity.PlayAnimation("idle");
		}
	}

	private IEnumerator DoWaitTillIdle(BattleEntity entity, UnityAction func)
	{
		if (!(entity != null))
		{
			yield break;
		}
		int count = 0;
		while (!entity.IsIdle())
		{
			count++;
			if (count >= 10)
			{
				entity.ForceIdle();
			}
			yield return null;
		}
		func();
	}

	private void SetCurrentEntity(BattleEntity entity)
	{
		_currentEntity = entity;
	}

	public void UpdateCurrentEntity(bool show = true)
	{
		foreach (BattleEntity entity in _entities)
		{
			bool turn = show && entity == _currentEntity;
			entity.SetTurn(turn);
		}
	}

	private int GetEntityPosition(int index)
	{
		if (index < 0 || index >= ENTITY_POSITIONS.Length)
		{
			return ENTITY_POSITIONS[0];
		}
		return ENTITY_POSITIONS[index];
	}

	public List<BattleEntity> GetTeam(bool attacker)
	{
		List<BattleEntity> list = new List<BattleEntity>();
		foreach (BattleEntity entity in _entities)
		{
			if (entity.attacker == attacker)
			{
				list.Add(entity);
			}
		}
		return list;
	}

	public bool GetTeamFocused(bool attacker)
	{
		foreach (BattleEntity entity in _entities)
		{
			if (entity.attacker == attacker)
			{
				return entity.focused;
			}
		}
		return false;
	}

	public List<BattleEntity> GetFocusTeam(bool focused, List<BattleEntity> entities)
	{
		List<BattleEntity> list = new List<BattleEntity>();
		foreach (BattleEntity entity in entities)
		{
			if (entity.focused == focused)
			{
				list.Add(entity);
			}
		}
		return list;
	}

	public List<BattleEntity> GetFocusedEntities()
	{
		List<BattleEntity> list = new List<BattleEntity>();
		foreach (BattleEntity entity in _entities)
		{
			if (entity.focused)
			{
				list.Add(entity);
			}
		}
		return list;
	}

	public BattleEntity GetEntity(int index)
	{
		foreach (BattleEntity entity in _entities)
		{
			if (entity.index == index)
			{
				return entity;
			}
		}
		return null;
	}

	public BattleEntity GetPlayerEntity()
	{
		foreach (BattleEntity entity in _entities)
		{
			if (entity.type == 1 && entity.controller == GameData.instance.PROJECT.character.id && entity.id == GameData.instance.PROJECT.character.id)
			{
				return entity;
			}
		}
		return null;
	}

	private Vector2 GetCenterPoint(List<BattleEntity> entities)
	{
		Vector2 vector = default(Vector2);
		foreach (BattleEntity entity in entities)
		{
			vector += entity.position;
		}
		return vector /= (float)entities.Count;
	}

	private void TrackStart()
	{
		if (!_replay && !_trackedStart && !(GameData.instance.PROJECT.dungeon != null))
		{
			_trackedStart = true;
			KongregateAnalytics.trackPlayStarts(_uid, GetTypeIdentifier(_type), GetEnemyCharID());
		}
	}

	private void TrackEnd(string type, int creditsGained = 0, int goldGained = 0)
	{
		if (!_replay && !_trackedEnd)
		{
			_trackedEnd = true;
			if (GameData.instance.PROJECT.dungeon != null)
			{
				GameData.instance.PROJECT.dungeon.AddCreditsGained(creditsGained);
				GameData.instance.PROJECT.dungeon.AddGoldGained(goldGained);
			}
			else
			{
				int pveEnergyChange = _pveEnergyStart - ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.energy : GameData.instance.SAVE_STATE.characterEnergy);
				int pvpEnergyChange = _pvpEnergyStart - ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.tickets : GameData.instance.SAVE_STATE.characterTickets);
				KongregateAnalytics.trackPlayEnds(_uid, GetTypeIdentifier(_type), type, creditsGained, goldGained, pveEnergyChange, pvpEnergyChange, GetHighestPlayerTeammateStats(), GetHighestEnemyTeammateStats(), GetEnemyCharID(), -1, null, GetEventID());
			}
		}
	}

	public void Clear()
	{
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnUpdate);
		ClearTimeoutTimer();
		ClearFreezeTimer();
		_battleUI.DoDestroy();
		BattleDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnQueue);
		Debug.Log("REMOVE RemoveEventListener OnQueue!!!!");
		GameData.instance.PROJECT.character.RemoveListener("AUTO_PILOT_CHANGE", OnAutoPilotChange);
		Object.Destroy(base.gameObject);
	}

	public static List<SFSObject> GetQueueFromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("bat3");
		List<SFSObject> list = new List<SFSObject>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			SFSObject item = (SFSObject)sFSArray.GetSFSObject(i);
			list.Add(item);
		}
		return list;
	}

	public static Battle fromSFSObject(ISFSObject sfsob)
	{
		GameData.instance.windowGenerator.ClearAllWindows(null, removeChat: false);
		Battle component;
		if (instance == null)
		{
			component = GameData.instance.windowGenerator.GetFromResources("ui/battle/" + typeof(Battle).Name + "Arena").gameObject.GetComponent<Battle>();
			component.init();
		}
		else
		{
			_ = instance.gameObject;
			component = instance;
		}
		int @int = sfsob.GetInt("bat0");
		int int2 = sfsob.GetInt("bat1");
		bool @bool = sfsob.GetBool("bat29");
		bool bool2 = sfsob.GetBool("bat37");
		bool bool3 = sfsob.GetBool("bat20");
		GameData.instance.main.logManager.AddBreadcrumb($"Battle::OnEnterBattle::ID:{@int}/type:{int2}");
		BattleRules battleRules = BattleRules.fromSFSObject(sfsob);
		List<BattleEntity> entities = BattleEntity.ListFromSFSObject(sfsob);
		BattleTeamData attackerData = BattleTeamData.fromSFSObject(sfsob.GetSFSObject("bat42"));
		BattleTeamData defenderData = BattleTeamData.fromSFSObject(sfsob.GetSFSObject("bat43"));
		List<SFSObject> queueFromSFSObject = GetQueueFromSFSObject(sfsob);
		object data = new object();
		switch (int2)
		{
		case 1:
		case 4:
		case 5:
		case 11:
			data = DungeonBook.Lookup(sfsob.GetInt("dun15"));
			break;
		case 2:
			data = PvPEventBook.Lookup(sfsob.GetInt("eve0"));
			break;
		case 6:
			data = GauntletEventBook.Lookup(sfsob.GetInt("eve0"));
			break;
		case 7:
			data = GvGEventBook.Lookup(sfsob.GetInt("eve0"));
			break;
		case 8:
			data = InvasionEventBook.Lookup(sfsob.GetInt("eve0"));
			break;
		case 9:
			data = BrawlBook.Lookup(sfsob.GetInt("bra2"));
			break;
		}
		component.LoadDetails(@int, int2, @bool, bool2, bool3, battleRules, entities, queueFromSFSObject, attackerData, defenderData, data);
		return component;
	}

	public BattleText AddBattleTextObj()
	{
		if (GameData.instance.battleTextPool.transform.childCount > 0)
		{
			for (int i = 0; i < GameData.instance.battleTextPool.transform.childCount; i++)
			{
				Transform child = GameData.instance.battleTextPool.transform.GetChild(i);
				if (child != null)
				{
					BattleText component = child.GetComponent<BattleText>();
					if (component != null && component.readyForUse)
					{
						component.gameObject.SetActive(value: true);
						return component;
					}
				}
			}
		}
		BattleText obj = Object.Instantiate(battleTextPrefab, GameData.instance.battleTextPool.transform);
		fullCount++;
		obj.name = "battleText " + fullCount;
		return obj;
	}

	public void AddEventListener(string type, EventListenerDelegate listener)
	{
		_dispatcher.AddEventListener(type, listener);
	}

	public void RemoveEventListener(string type, EventListenerDelegate listener)
	{
		_dispatcher.RemoveEventListener(type, listener);
	}

	public void DispatchEvent(BaseEvent e)
	{
		_dispatcher.DispatchEvent(e);
	}

	public static string GetTypeIdentifier(int type)
	{
		return TYPE_IDENTIFIER[type];
	}

	private int GetEnemyCharID()
	{
		BattleEntity playerEntity = GetPlayerEntity();
		if (playerEntity == null)
		{
			return -1;
		}
		List<BattleEntity> team = GetTeam(!playerEntity.attacker);
		if (team == null || team.Count <= 0)
		{
			return -1;
		}
		foreach (BattleEntity item in team)
		{
			if (item.type == 1)
			{
				return item.characterData.charID;
			}
		}
		return -1;
	}

	private int GetHighestPlayerTeammateStats()
	{
		int num = 0;
		BattleEntity playerEntity = GetPlayerEntity();
		if (playerEntity == null)
		{
			return num;
		}
		List<BattleEntity> team = GetTeam(playerEntity.attacker);
		if (team == null || team.Count <= 0)
		{
			return num;
		}
		foreach (BattleEntity item in team)
		{
			if (item.id != playerEntity.id && item.type == 1)
			{
				int totalStats = item.characterData.getTotalStats();
				if (totalStats > num)
				{
					num = totalStats;
				}
			}
		}
		return num;
	}

	private int GetHighestEnemyTeammateStats()
	{
		int num = 0;
		BattleEntity playerEntity = GetPlayerEntity();
		if (playerEntity == null)
		{
			return num;
		}
		List<BattleEntity> team = GetTeam(!playerEntity.attacker);
		if (team == null || team.Count <= 0)
		{
			return num;
		}
		foreach (BattleEntity item in team)
		{
			if (item.id != playerEntity.id && item.type == 1)
			{
				int totalStats = item.characterData.getTotalStats();
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

	public void StatLoudout()
	{
		_pveEnergyStart = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.energy : GameData.instance.SAVE_STATE.characterEnergy);
		_pvpEnergyStart = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.tickets : GameData.instance.SAVE_STATE.characterTickets);
		KongregateAnalyticsSchema.LoadOutStats loadOutStats = new KongregateAnalyticsSchema.LoadOutStats();
		loadOutStats.mode = _type;
		loadOutStats.loadout_context = "Battle_" + GetTypeIdentifier(_type);
		loadOutStats.settings = GameData.instance.GetLoadOut();
		int num = 0;
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		for (int i = 0; i < _entities.Count; i++)
		{
			switch (_entities[i].type)
			{
			case 3:
				list.Add(_entities[i].statEntityFamiliar());
				break;
			case 1:
			{
				if (_entities[i].characterData.charID != GameData.instance.PROJECT.character.id)
				{
					num++;
					loadOutStats.friends.Add(new KongregateAnalyticsSchema.LoadOutStatsFriend
					{
						index = num,
						stats = _entities[i].characterData.getTotalStats()
					});
					break;
				}
				loadOutStats.player_total_stats = _entities[i].characterData.getTotalStats();
				List<KongregateAnalyticsSchema.ItemStat> list2 = _entities[i].characterData.runes.statAllRunes();
				if (list2.Count > 0)
				{
					loadOutStats.runes = list2;
				}
				List<KongregateAnalyticsSchema.ItemStat> gear = _entities[i].characterData.equipment.statAllEquipement();
				if (list2.Count > 0)
				{
					loadOutStats.gear = gear;
				}
				List<Dictionary<string, object>> list3 = GameData.instance.PROJECT.character.inventory.statItems(GameData.instance.PROJECT.character, 4);
				if (list3.Count > 0)
				{
					loadOutStats.boosts = list3;
				}
				break;
			}
			}
		}
		loadOutStats.familiars = list;
		string jsonMap = JsonConvert.SerializeObject(loadOutStats);
		AppInfo.doKongregateAnalyticsEvent("loadouts", jsonMap);
	}

	public bool getLocked()
	{
		if (_movement || _completed || _queue.Count > 0 || BattleDALC.instance.waiting || _captureEntities.Count > 0 || _abilityTween)
		{
			return true;
		}
		return false;
	}

	public BattleEntity getEntityByID(int id)
	{
		foreach (BattleEntity entity in _entities)
		{
			if (entity.id == id)
			{
				return entity;
			}
		}
		return null;
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
		if (GameData.instance.battleTextPool != null && GameData.instance.battleTextPool.transform != null)
		{
			for (int i = 0; i < GameData.instance.battleTextPool.transform.childCount; i++)
			{
				GameData.instance.battleTextPool.transform.GetChild(i).gameObject.SetActive(value: false);
			}
		}
	}
}
