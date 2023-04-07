using System;
using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.npc;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.ability;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.ui.battle;

public class BattleEntity : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
	public const int TYPE_NONE = 0;

	public const int TYPE_PLAYER = 1;

	public const int TYPE_NPC = 2;

	public const int TYPE_FAMILIAR = 3;

	private const int DRAG_THRESHOLD = 10;

	private const string DRAG_NAME = "BattleEntityDragShape";

	[HideInInspector]
	public UnityCustomEvent SELECT = new UnityCustomEvent();

	public Transform shadow;

	public Transform turnIndicator;

	public Transform placeholderOverlay;

	public BattleEntityOverlay battleEntityOverlayPrefab;

	private HoverAndGlowEntity hoverAndGlowEntity;

	private bool _attacker;

	private int _id;

	private int _type;

	private int _controller;

	private int _index;

	private int _power;

	private int _stamina;

	private int _agility;

	private long _cooldown;

	private int _meter;

	private int _meterTotal;

	private int _healthCurrent;

	private int _healthTotal;

	private int _shieldCurrent;

	private int _shieldTotal;

	private int _damageGained;

	private CharacterData _characterData;

	private List<AbilityRef> _abilities;

	private FamiliarRef _captureFamiliarRef;

	private Battle _battle;

	private BattleEntityOverlay _overlay;

	private GameObject _asset;

	private Vector2 _position;

	private List<AbilityTile> _abilityTiles;

	private AbilityRef _currentAbility;

	private Vector2 _dragPoint;

	private bool _dragging;

	private BattleEntity _dragEntity;

	private GameObject _dragSprite;

	private bool _dragBarVisible;

	private float _timeElapsed;

	private int _meterGain;

	private int _consumables;

	private int _teammates;

	private long _armoryID;

	private bool _clickable = true;

	private bool _draggable = true;

	private bool _focused;

	private bool _highlighted;

	private List<BattleAbilityData> _abilityData;

	private BattleProjectile _currentProjectile;

	private BoxCollider2D _hitbox;

	private BoxCollider2D _battleHitbox;

	private int _specialBehaviour;

	public AbilityTile abilityTilePrefab;

	public int highlightID;

	private bool listenMouseDown = true;

	private bool listenMouseUp;

	private bool listenMouseMove;

	private SWFAsset _swfAsset;

	private SWFAsset _swfMountAsset;

	private Type _classParent;

	private bool _blockSkills;

	public SWFAsset swfAsset
	{
		get
		{
			if (_swfAsset == null && _asset != null)
			{
				_swfAsset = _asset.GetComponent<SWFAsset>();
			}
			if (_swfAsset != null)
			{
				_swfAsset.entityID = _id;
			}
			return _swfAsset;
		}
	}

	public float x
	{
		get
		{
			if (base.transform != null)
			{
				return base.transform.localPosition.x;
			}
			return 0f;
		}
		set
		{
			UpdateGraphicalPosition(new Vector2(value, y));
		}
	}

	public float y
	{
		get
		{
			return base.transform.localPosition.y;
		}
		set
		{
			UpdateGraphicalPosition(new Vector2(x, value));
		}
	}

	public int consumables => _consumables;

	public bool isDead => _healthCurrent <= 0;

	public bool isAlive => !isDead;

	public bool isMe
	{
		get
		{
			if (_type == 1 && _id == GameData.instance.PROJECT.character.id)
			{
				return _controller == GameData.instance.PROJECT.character.id;
			}
			return false;
		}
	}

	public int controller => _controller;

	public int index => _index;

	public int id => _id;

	public int type => _type;

	public bool attacker => _attacker;

	public bool focused => _focused;

	public bool highlighted => _highlighted;

	public float timeElapsed => _timeElapsed;

	public int meterGain => _meterGain;

	public int meter => _meter;

	public long cooldown => _cooldown;

	public Vector2 position => _position;

	public GameObject asset => _asset;

	public Battle battle => _battle;

	public int healthCurrent => _healthCurrent;

	public int healthTotal => _healthTotal;

	public float healthPerc => (float)_healthCurrent / (float)_healthTotal;

	public int shieldCurrent => _shieldCurrent;

	public int shieldTotal => _shieldTotal;

	public float shieldPerc => _shieldCurrent / _shieldTotal;

	public float damageGained => _damageGained;

	public List<AbilityRef> abilities => _abilities;

	public FamiliarRef captureFamiliarRef => _captureFamiliarRef;

	public AbilityRef currentAbility => _currentAbility;

	public CharacterData characterData => _characterData;

	public BattleEntityOverlay overlay => _overlay;

	public BattleProjectile currentProjectile => _currentProjectile;

	public BoxCollider2D hitbox => _hitbox;

	public Type classParent
	{
		get
		{
			return _classParent;
		}
		set
		{
			_classParent = value;
		}
	}

	public void LoadDetails(bool attacker, int id, int type, int controller, int index, int power, int stamina, int agility, long cooldown, int meter, int meterTotal, int healthCurrent, int healthTotal, int shieldCurrent, int shieldTotal, int damageGained, CharacterData characterData, List<AbilityRef> abilities, FamiliarRef captureFamiliarRef, int armoryID = -1)
	{
		_attacker = attacker;
		_id = id;
		_type = type;
		_controller = controller;
		_index = index;
		_power = power;
		_stamina = stamina;
		_agility = agility;
		_cooldown = cooldown;
		_meter = meter;
		_meterTotal = meterTotal;
		_healthTotal = healthTotal;
		_shieldTotal = shieldTotal;
		_damageGained = damageGained;
		_characterData = characterData;
		_abilities = abilities;
		_captureFamiliarRef = captureFamiliarRef;
		_armoryID = armoryID;
		_overlay = UnityEngine.Object.Instantiate(battleEntityOverlayPrefab, placeholderOverlay);
		_overlay.gameObject.transform.localPosition = Vector3.zero;
		_hitbox = GetComponentInChildren<SpriteRenderer>().gameObject.AddComponent<BoxCollider2D>();
		_hitbox.offset = new Vector3(0f, 40f);
		_hitbox.size = new Vector2(60f, 120f);
		GetComponentInChildren<SpriteRenderer>().gameObject.layer = LayerMask.NameToLayer("Mouse");
		switch (_type)
		{
		case 1:
			if (_armoryID > 0)
			{
				SetBattleEntityCharacterArmory();
			}
			base.gameObject.name = _characterData.parsedName;
			break;
		case 2:
		{
			NPCRef nPCRef = NPCBook.Lookup(_id);
			base.gameObject.name = nPCRef.name;
			break;
		}
		case 3:
		{
			FamiliarRef familiarRef = FamiliarBook.Lookup(_id);
			base.gameObject.name = familiarRef.coloredName;
			break;
		}
		}
		SetTimeElapsed(0f);
		SetHealthCurrent(healthCurrent);
		SetShieldCurrent(shieldCurrent);
		SetMeter(meter);
		SetFocused(attacker);
		SetTurn();
	}

	public void SetSpecialBehaviour(int spb)
	{
		_specialBehaviour = spb;
		if (_specialBehaviour == 100)
		{
			_characterData.equipment.setEquipmentSlot(null, 7);
		}
	}

	private void SetBattleEntityCharacterArmory()
	{
		ArmoryEquipment armoryEquipmentByID = _characterData.armory.GetArmoryEquipmentByID(_armoryID);
		if (armoryEquipmentByID == null)
		{
			return;
		}
		Equipment equipment = ArmoryEquipment.ArmoryEquipmentToEquipment(armoryEquipmentByID);
		_characterData.setEquipment(equipment);
		_characterData.runes.setRuneSlots(armoryEquipmentByID.runes.runeSlots);
		_characterData.enchants.setEnchantSlots(armoryEquipmentByID.enchants.slots);
		MountData mount = _characterData.mounts.getMount(armoryEquipmentByID.mount);
		MountRef cosmeticMount = _characterData.mounts.getCosmeticMount((int)armoryEquipmentByID.mountCosmetic);
		if (mount != null)
		{
			_characterData.mounts.setEquipped(mount);
			if (cosmeticMount != null)
			{
				_characterData.mounts.setCosmetic(cosmeticMount);
			}
		}
		else
		{
			_characterData.mounts.setEquipped(null);
		}
		_abilities = _characterData.getAbilities();
	}

	public void SetBattle(Battle battle)
	{
		_battle = battle;
		_battleHitbox = _battle.backgroundHitbox;
		_teammates = _battle.GetTeam(_attacker).Count;
		SetClickable(clickable: false, draggable: false);
	}

	public void SetFocused(bool focus)
	{
		_focused = focus;
	}

	public bool GetAbilityDisabled(AbilityRef abilityRef)
	{
		if (abilityRef.uses > 0)
		{
			BattleAbilityData abilityData = GetAbilityData(abilityRef);
			if (abilityData != null && abilityData.uses >= abilityRef.uses)
			{
				return true;
			}
		}
		return false;
	}

	public void AddTimeElapsed(float time)
	{
		float num = _timeElapsed + time;
		if (num > (float)_cooldown)
		{
			num -= (float)_cooldown;
		}
		SetTimeElapsed(num);
	}

	public void SetTimeElapsed(float time)
	{
		_timeElapsed = time;
		float cooldownPerc = _timeElapsed / (float)_cooldown;
		_overlay.SetCooldownPerc(cooldownPerc);
	}

	public void AddMeterGain(int meter)
	{
		int num = _meterGain + meter;
		SetMeterGain(num);
	}

	public int SetMeterGain(int meter)
	{
		int num = _meterGain;
		_meterGain = meter;
		if (_meterGain < 0)
		{
			_meterGain = 0;
		}
		return _meterGain - num;
	}

	public void AddMeter(int added)
	{
		int num = _meter + added;
		if (num > _meterTotal)
		{
			num = _meterTotal;
		}
		SetMeter(num);
	}

	public void SetMeter(int meter)
	{
		_meter = meter;
		float meterPerc = (float)_meter / (float)VariableBook.battleMeterMax;
		_overlay.SetMeterPerc(meterPerc);
	}

	public void SetHealthCurrent(int health)
	{
		_healthCurrent = health;
		float num = (float)_healthCurrent / (float)_healthTotal;
		_overlay.SetHealthPerc(num);
	}

	public void SetShieldCurrent(int shield)
	{
		_shieldCurrent = shield;
		float perc = (float)_shieldCurrent / (float)_shieldTotal;
		float maxPerc = Mathf.Clamp01((float)_shieldTotal / (float)_healthTotal);
		_overlay.SetShieldPerc(perc, maxPerc);
	}

	public void SetDamageGained(int gained)
	{
		_damageGained = gained;
	}

	public void SetCurrentProjectile(BattleProjectile projectile)
	{
		_currentProjectile = projectile;
	}

	private void ClearAsset()
	{
		if (_asset != null)
		{
			UnityEngine.Object.Destroy(_asset);
			_asset = null;
		}
	}

	public void UpdateAsset()
	{
		ClearAsset();
		if (isDead)
		{
			if (GameData.instance.main == null || GameData.instance.main.assetLoader == null)
			{
				return;
			}
			_asset = GameData.instance.main.assetLoader.GetGameObjectAsset(AssetURL.BATTLE_OTHER, VariableBook.GetBattleDeathAsset().Split('/').Last());
			if (_asset == null)
			{
				return;
			}
			if (_asset.transform != null)
			{
				_asset.transform.SetParent(base.transform);
				_asset.transform.localPosition = Vector3.zero;
				_asset.transform.localScale = Vector3.one * 3f;
			}
			SetAsset(_asset);
		}
		else
		{
			switch (_type)
			{
			case 2:
			{
				NPCRef nPCRef = NPCBook.Lookup(_id);
				Transform transform2 = null;
				if (nPCRef != null && nPCRef.displayRef != null)
				{
					transform2 = nPCRef.displayRef.getAsset(center: true, 3f, base.transform);
				}
				if (transform2 != null)
				{
					_asset = transform2.gameObject;
					transform2.GetComponentInChildren<CharacterDisplay>();
				}
				if (_asset != null && _asset.gameObject != null)
				{
					_swfAsset = _asset.AddComponent<SWFAsset>();
				}
				SetAsset(_asset);
				break;
			}
			case 1:
			{
				CharacterDisplay characterDisplay = null;
				if (_characterData != null)
				{
					characterDisplay = _characterData.toCharacterDisplay(3f, _characterData.showMount, null, enableLoading: false);
				}
				if (!(characterDisplay != null) || !(characterDisplay.gameObject != null))
				{
					break;
				}
				UnityEngine.Object.Destroy(characterDisplay.characterPuppet.GetComponent<BoxCollider2D>());
				_asset = characterDisplay.gameObject;
				characterDisplay.transform.SetParent(base.transform);
				characterDisplay.SetLocalPosition(Vector3.zero);
				if (characterDisplay.hasMountEquipped())
				{
					CharacterPuppet componentInChildren = _asset.GetComponentInChildren<CharacterPuppet>();
					if (componentInChildren != null)
					{
						_swfAsset = componentInChildren.gameObject.GetComponent<SWFAsset>();
						if (_swfAsset == null)
						{
							_swfAsset = componentInChildren.gameObject.AddComponent<SWFAsset>();
						}
						_swfMountAsset = characterDisplay.gameObject.AddComponent<SWFAsset>();
					}
					else
					{
						_swfAsset = characterDisplay.gameObject.AddComponent<SWFAsset>();
					}
				}
				else
				{
					_swfAsset = characterDisplay.gameObject.AddComponent<SWFAsset>();
				}
				if (characterDisplay != null && characterDisplay.gameObject != null)
				{
					SetAsset(characterDisplay.gameObject);
				}
				break;
			}
			case 3:
			{
				FamiliarRef familiarRef = FamiliarBook.Lookup(_id);
				if (familiarRef != null)
				{
					Transform transform = familiarRef.displayRef.getAsset(center: true, 3f, base.transform);
					if (transform != null && transform.gameObject != null)
					{
						_asset = transform.gameObject;
						_swfAsset = _asset.AddComponent<SWFAsset>();
						transform.GetComponentInChildren<CharacterDisplay>();
					}
				}
				SetAsset(_asset);
				break;
			}
			}
		}
		if (_asset != null)
		{
			if (hoverAndGlowEntity != null)
			{
				UnityEngine.Object.Destroy(hoverAndGlowEntity);
			}
			BoxCollider2D componentInChildren2 = base.gameObject.GetComponentInChildren<BoxCollider2D>();
			if (componentInChildren2 != null && componentInChildren2.gameObject != null)
			{
				hoverAndGlowEntity = componentInChildren2.gameObject.AddComponent<HoverAndGlowEntity>();
				if (hoverAndGlowEntity != null)
				{
					hoverAndGlowEntity.GetTargetAssets(new List<SpriteRenderer>(_asset.GetComponentsInChildren<SpriteRenderer>()));
					hoverAndGlowEntity.SetHover(_clickable);
				}
			}
		}
		SetBars(isAlive);
	}

	public void LoadCaptureAsset()
	{
		string text = "";
		switch (_type)
		{
		case 2:
			text = NPCBook.Lookup(_id).displayRef.assetURL;
			break;
		case 3:
			text = FamiliarBook.Lookup(_id).displayRef.assetURL;
			break;
		}
		if (_captureFamiliarRef != null)
		{
			_ = text == _captureFamiliarRef.displayRef.assetURL;
		}
	}

	private void OnAssetLoaded(Event e)
	{
	}

	private void SetAsset(GameObject asset, float scale = 0f)
	{
		if (asset == null || this == null)
		{
			return;
		}
		Transform transform = base.transform.Find("Character");
		if (transform != null)
		{
			SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
		if (scale <= 0f)
		{
			scale = asset.transform.localScale.x;
		}
		BattleCaptureWindow battleCaptureWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(BattleCaptureWindow)) as BattleCaptureWindow;
		bool flag = battleCaptureWindow != null && battleCaptureWindow.entity == this;
		asset.transform.localScale = new Vector3((focused || (isDead && !flag)) ? scale : (0f - scale), asset.transform.localScale.y, asset.transform.localScale.z);
		if (asset.transform.localScale.x < 0f)
		{
			ParticleSystem[] componentsInChildren = asset.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Vector3 localScale = componentsInChildren[i].transform.localScale;
				localScale.x *= -1f;
				componentsInChildren[i].transform.localScale = localScale;
			}
		}
		PlayAnimation("idle");
	}

	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		_highlighted = true;
		_battle.UpdateEntityParents();
	}

	public void OnPointerExit(PointerEventData pointerEventData)
	{
		_highlighted = false;
		_battle.UpdateEntityParents();
	}

	public void SetBars(bool vis = true)
	{
		ToggleBarsVisibility(vis);
	}

	private void ToggleBarsVisibility(bool vis)
	{
		_overlay.gameObject.SetActive(vis);
	}

	public void SetClickable(bool clickable = true, bool draggable = true)
	{
		_clickable = clickable;
		_draggable = draggable;
		if (hoverAndGlowEntity != null)
		{
			hoverAndGlowEntity.SetHover(_clickable);
		}
	}

	public void OnPointerDown(PointerEventData pointerEventData)
	{
		if (_draggable && listenMouseDown)
		{
			_dragPoint = new Vector2(pointerEventData.position.x, pointerEventData.position.y);
			listenMouseDown = false;
			listenMouseMove = true;
			listenMouseUp = true;
		}
	}

	public void OnBeginDrag(BaseEventData baseEventData)
	{
	}

	public void OnDrag(BaseEventData baseEventData)
	{
		if (!_draggable || !listenMouseMove)
		{
			return;
		}
		PointerEventData pointerEventData = baseEventData as PointerEventData;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(GameData.instance.main.mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		if (!raycastHit2D)
		{
			return;
		}
		base.transform.position = new Vector3(raycastHit2D.point.x, raycastHit2D.point.y, base.transform.position.z);
		CheckDragObjects(pointerEventData);
		if (!_dragging)
		{
			GameData.instance.main.mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
			_ = _dragPoint;
			_ = _dragPoint;
			_dragBarVisible = _overlay.gameObject.activeSelf;
			_dragging = true;
			if (_dragBarVisible)
			{
				ToggleBarsVisibility(vis: false);
			}
		}
	}

	public void OnEndDrag()
	{
	}

	public void OnPointerUp(PointerEventData pointerEventData)
	{
		if (_dragging)
		{
			CheckDragObjects(pointerEventData);
			if (_dragEntity != null)
			{
				_dragEntity.x = _position.x;
				_dragEntity.y = _position.y;
				x = _dragEntity.position.x;
				y = _dragEntity.position.y;
				_dragEntity.SetPosition(new Vector2(_dragEntity.x, _dragEntity.y));
				SetPosition(new Vector2(x, y));
				ClearDragObjects();
				List<BattleEntity> list = ((!_focused) ? (from ent in _battle.GetTeam(_attacker)
					orderby ent.x
					select ent).ToList() : (from ent in _battle.GetTeam(_attacker)
					orderby ent.x descending
					select ent).ToList());
				int[] array = new int[list.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = list[i].index;
				}
				BattleDALC.instance.doOrder(array);
			}
			else
			{
				x = _position.x;
				y = _position.y;
			}
			if (_dragBarVisible)
			{
				ToggleBarsVisibility(vis: true);
			}
			_dragging = false;
			_dragBarVisible = false;
		}
		listenMouseDown = true;
		listenMouseUp = false;
		listenMouseMove = false;
	}

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (!_clickable)
		{
			if (GameData.instance.PROJECT.character.autoPilot || (bool)_dragEntity || (bool)_dragSprite || GameData.instance.PROJECT.battle.getLocked() || isDead || _draggable)
			{
				return;
			}
			switch (_type)
			{
			case 1:
				if (_id != GameData.instance.PROJECT.character.id)
				{
					GameData.instance.audioManager.PlaySoundLink("buttonclick");
					GameData.instance.windowGenerator.ShowPlayer(_characterData);
				}
				break;
			case 2:
			{
				NPCRef nPCRef = NPCBook.Lookup(_id);
				int num = FamiliarBook.GetFamiliarIdFromNPCLink(nPCRef.link);
				if (num == 0)
				{
					num = nPCRef.familiar;
				}
				FamiliarRef familiarRef = FamiliarBook.Lookup(num);
				if (familiarRef != null)
				{
					GameData.instance.audioManager.PlaySoundLink("buttonclick");
					GameData.instance.windowGenerator.NewFamiliarWindow(familiarRef, null, _attacker);
				}
				break;
			}
			case 3:
			{
				GameData.instance.audioManager.PlaySoundLink("buttonclick");
				FamiliarRef familiarRef = FamiliarBook.Lookup(_id);
				CharacterData characterData = null;
				bool mine = _attacker;
				if (!_attacker && _battle.type == 2)
				{
					foreach (BattleEntity item in _battle.GetTeam(_attacker))
					{
						if (item.type == 1)
						{
							characterData = item.characterData;
							break;
						}
					}
				}
				else if (_battle.type == 3)
				{
					BattleEntity entityByID = _battle.getEntityByID(_controller);
					if ((bool)entityByID)
					{
						characterData = entityByID.characterData;
					}
					if (characterData != null && characterData.charID != GameData.instance.PROJECT.character.id)
					{
						mine = false;
					}
				}
				GameData.instance.windowGenerator.NewFamiliarWindow(familiarRef, null, mine, characterData);
				break;
			}
			}
		}
		else
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			SELECT.Invoke(this);
		}
	}

	private void CheckDragObjects(PointerEventData pointerEventData)
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(new Vector3(pointerEventData.pointerCurrentRaycast.worldPosition.x, pointerEventData.pointerCurrentRaycast.worldPosition.y, GameData.instance.main.mainCamera.transform.position.z), Vector2.zero);
		if (!raycastHit2D)
		{
			return;
		}
		Vector2 point = default(Vector2);
		point.x = raycastHit2D.point.x;
		point.y = raycastHit2D.point.y;
		if (!_dragging)
		{
			return;
		}
		if (_dragSprite != null)
		{
			if (Util.GetObjectsUnderPointByName(base.transform, point, "BattleEntityDragShape", 1).Count <= 0)
			{
				ClearDragObjects();
			}
			return;
		}
		List<GameObject> list = new List<GameObject>();
		list.Add(base.gameObject);
		List<GameObject> objectsUnderPointByClass = Util.GetObjectsUnderPointByClass(base.transform, point, typeof(BattleEntity), 0, list);
		if (objectsUnderPointByClass.Count <= 0)
		{
			return;
		}
		while (objectsUnderPointByClass.Count > 0)
		{
			GameObject gameObject = objectsUnderPointByClass[0];
			objectsUnderPointByClass.RemoveAt(0);
			BattleEntity component = gameObject.GetComponent<BattleEntity>();
			bool flag = false;
			if (component.attacker == attacker && component.hitbox.bounds.Contains(new Vector3(point.x, point.y, component.transform.position.z)))
			{
				flag = true;
				_dragEntity = component;
				_dragSprite = new GameObject();
				_dragSprite.name = "BattleEntityDragShape";
				_dragSprite.transform.SetParent(component.transform.parent);
				_dragSprite.transform.position = component.transform.position;
				BoxCollider2D boxCollider2D = _dragSprite.AddComponent<BoxCollider2D>();
				boxCollider2D.size = component.hitbox.size;
				boxCollider2D.offset = component.hitbox.offset;
				_dragEntity.x = _position.x;
				_dragEntity.y = _position.y;
			}
			if (flag)
			{
				break;
			}
		}
	}

	private void ClearDragObjects()
	{
		if (_dragEntity != null)
		{
			_dragEntity.x = _dragEntity.position.x;
			_dragEntity.y = _dragEntity.position.y;
			_dragEntity = null;
		}
		if (_dragSprite != null)
		{
			UnityEngine.Object.Destroy(_dragSprite);
			_dragSprite = null;
		}
	}

	public void StopAnimation()
	{
		if (_asset != null)
		{
			swfAsset.StopAnimation();
			if (_swfMountAsset != null)
			{
				_swfMountAsset.StopAnimation();
			}
		}
	}

	public void StopSubAnimation()
	{
	}

	public bool PlayAnimation(string animation)
	{
		if (this == null)
		{
			return false;
		}
		if (animation == null || animation.Trim().Equals(""))
		{
			return false;
		}
		if (_asset != null)
		{
			if (_battle == null)
			{
				return false;
			}
			if (swfAsset != null)
			{
				swfAsset.StopAnimation();
				CharacterDisplay component = swfAsset.GetComponent<CharacterDisplay>();
				if (_swfMountAsset != null)
				{
					component = _swfMountAsset.GetComponent<CharacterDisplay>();
				}
				if (component != null)
				{
					if (component.hasMountEquipped())
					{
						switch (animation)
						{
						case "hit":
						case "walk":
							_swfMountAsset.PlayAnimation(animation, battle.GetSpeed());
							break;
						default:
							_swfMountAsset.PlayAnimation("idle");
							break;
						}
						return swfAsset.PlayAnimation(animation, (animation == "idle") ? 1f : battle.GetSpeed());
					}
					return swfAsset.PlayAnimation(animation, (animation == "idle") ? 1f : battle.GetSpeed());
				}
				return swfAsset.PlayAnimation(animation, (animation == "idle") ? 1f : battle.GetSpeed());
			}
		}
		return false;
	}

	public void SetPosition(Vector2 point, bool update = true)
	{
		_position = point;
		if (update)
		{
			base.transform.localPosition = _position;
		}
	}

	public void UpdateGraphicalPosition(Vector2 point)
	{
		base.transform.localPosition = point;
	}

	public void SetTurn(bool turn = false)
	{
		if (turnIndicator != null)
		{
			turnIndicator.gameObject.SetActive(turn);
		}
	}

	public List<AbilityTile> GetAbilityTiles()
	{
		if (_abilityTiles == null)
		{
			List<AbilityRef> list = new List<AbilityRef>();
			list.AddRange(_abilities);
			list.Sort((AbilityRef p1, AbilityRef p2) => p1.meterCost.CompareTo(p2.meterCost));
			_abilityTiles = new List<AbilityTile>();
			for (int i = 0; i < list.Count; i++)
			{
				string key = (AppInfo.allowKeycodes ? (i + 1).ToString() : null);
				AbilityRef abilityRef = list[i];
				AbilityTile abilityTile = UnityEngine.Object.Instantiate(abilityTilePrefab, Battle.instance.battleUI.placeholderAbilities);
				abilityTile.gameObject.name = "AbilityTile_" + i + "_EID_" + id;
				abilityTile.LoadDetails(abilityRef, this, i, key);
				_abilityTiles.Add(abilityTile);
				abilityTile.gameObject.SetActive(value: false);
			}
		}
		return _abilityTiles;
	}

	public Vector2 GetProjectilePoint(int source, AbilityActionRef actionRef)
	{
		Battle componentInParent = GetComponentInParent<Battle>();
		if (componentInParent == null)
		{
			return Vector2.zero;
		}
		Vector2 result = Vector2.zero;
		if (_asset != null && _asset.transform != null)
		{
			result = _asset.transform.localPosition;
		}
		result.x += x;
		result.y += y;
		result.x += (focused ? actionRef.projectileOffset.x : (0f - actionRef.projectileOffset.x));
		result.y += actionRef.projectileOffset.y;
		if (_type == 1)
		{
			CharacterDisplay component = _asset.GetComponent<CharacterDisplay>();
			Transform transform = null;
			EquipmentRef equipmentSlot;
			if (source == 8)
			{
				if (component != null)
				{
					transform = component.petAsset;
				}
				equipmentSlot = _characterData.equipment.getEquipmentSlot(7);
			}
			else
			{
				if (component != null)
				{
					transform = component.mainhandAsset;
				}
				equipmentSlot = _characterData.equipment.getEquipmentSlot(0);
			}
			if (transform != null && equipmentSlot != null)
			{
				Vector2 vector = new Vector2(equipmentSlot.projectileOffset.x, equipmentSlot.projectileOffset.y);
				if (equipmentSlot.projectileCenter)
				{
					vector.x += 0f;
					vector.y += 0f;
				}
				result = componentInParent.transform.InverseTransformVector(vector);
				result.x += (focused ? actionRef.projectileOffset.x : (0f - actionRef.projectileOffset.x));
				result.y += actionRef.projectileOffset.y;
			}
		}
		return result;
	}

	public void SetCurrentAbility(AbilityRef abilityRef)
	{
		_currentAbility = abilityRef;
	}

	public void SetConsumables(int consumables)
	{
		_consumables = consumables;
	}

	public BattleAbilityData GetAbilityData(AbilityRef abilityRef)
	{
		foreach (BattleAbilityData abilityDatum in _abilityData)
		{
			if (abilityDatum.abilityRef.id == abilityRef.id)
			{
				return abilityDatum;
			}
		}
		return null;
	}

	public void SetAbilityData(List<BattleAbilityData> abilityData)
	{
		_abilityData = abilityData;
	}

	public bool IsIdle()
	{
		if (_asset != null && _swfAsset != null)
		{
			return _swfAsset.IsIdle();
		}
		return true;
	}

	public void ForceIdle()
	{
		_swfAsset.PlayAnimation("idle");
	}

	public List<GameModifier> GetModifiers()
	{
		return _type switch
		{
			1 => _characterData.getModifiers(), 
			3 => FamiliarBook.Lookup(_id).modifiers, 
			_ => new List<GameModifier>(), 
		};
	}

	public int GetTotalPower()
	{
		return _type switch
		{
			1 => _characterData.getTotalPower(), 
			3 => FamiliarBook.Lookup(_id).getPower(GameData.instance.PROJECT.character.getTotalStats()), 
			_ => 0, 
		};
	}

	public int GetTotalStamina()
	{
		return _type switch
		{
			1 => _characterData.getTotalStamina(), 
			3 => FamiliarBook.Lookup(_id).getStamina(GameData.instance.PROJECT.character.getTotalStats()), 
			_ => 0, 
		};
	}

	public int GetTotalAgility()
	{
		return _type switch
		{
			1 => _characterData.getTotalAgility(), 
			3 => FamiliarBook.Lookup(_id).getAgility(GameData.instance.PROJECT.character.getTotalStats()), 
			_ => 0, 
		};
	}

	public Point GetOffset()
	{
		if (_type == 2)
		{
			return Point.fromVector2(NPCBook.Lookup(_id).offset);
		}
		return Point.create();
	}

	public bool GetAnchorTop()
	{
		if (_type == 2)
		{
			return NPCBook.Lookup(_id).anchorTop;
		}
		return false;
	}

	public void Remove()
	{
		base.gameObject.SetActive(value: false);
	}

	public static List<BattleEntity> ListFromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("bat2");
		List<BattleEntity> list = new List<BattleEntity>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(FromSFSObject(sFSObject));
		}
		return list;
	}

	public static BattleEntity FromSFSObject(ISFSObject sfsob)
	{
		bool @bool = sfsob.GetBool("bat14");
		int @int = sfsob.GetInt("bat5");
		int int2 = sfsob.GetInt("bat6");
		int int3 = sfsob.GetInt("bat28");
		int int4 = sfsob.GetInt("bat7");
		int int5 = sfsob.GetInt("bat8");
		int int6 = sfsob.GetInt("bat9");
		int int7 = sfsob.GetInt("bat10");
		int num = (int)sfsob.GetLong("bat15");
		int int8 = sfsob.GetInt("bat30");
		int meterTotal = (sfsob.ContainsKey("bat84") ? sfsob.GetInt("bat84") : VariableBook.battleMeterMax);
		int int9 = sfsob.GetInt("bat11");
		int int10 = sfsob.GetInt("bat12");
		int int11 = sfsob.GetInt("bat51");
		int int12 = sfsob.GetInt("bat52");
		int int13 = sfsob.GetInt("bat56");
		int int14 = sfsob.GetInt("bat32");
		int int15 = sfsob.GetInt("bat73");
		int int16 = sfsob.GetInt("charArmoryID");
		CharacterData characterData = CharacterData.fromSFSObject(sfsob);
		List<BattleAbilityData> abilityData = BattleAbilityData.listFromSFSObject(sfsob);
		List<AbilityRef> list = AbilityBook.LookupIDs(sfsob.GetIntArray("bat27"));
		FamiliarRef familiarRef = (sfsob.ContainsKey("bat35") ? FamiliarBook.Lookup(sfsob.GetInt("bat35")) : null);
		BattleEntity battleEntity = UnityEngine.Object.Instantiate(Battle.instance.battleEntityPrefab, Vector3.zero, Quaternion.identity, Battle.instance.arenaContainer.transform);
		battleEntity.LoadDetails(@bool, @int, int2, int3, int4, int5, int6, int7, num, int8, meterTotal, int9, int10, int11, int12, int13, characterData, list, familiarRef, int16);
		battleEntity.SetConsumables(int14);
		battleEntity.SetAbilityData(abilityData);
		battleEntity.SetSpecialBehaviour(int15);
		return battleEntity;
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}

	public bool getBlockSkillsData()
	{
		return _blockSkills;
	}

	public void setBlockSkillsData(bool data)
	{
		_blockSkills = data;
	}

	public Dictionary<string, object> statEntityFamiliar()
	{
		FamiliarRef familiarRef = FamiliarBook.Lookup(_id);
		foreach (AugmentData familiarAugmentSlot in GameData.instance.PROJECT.character.augments.getFamiliarAugmentSlots(familiarRef))
		{
			if (familiarAugmentSlot != null && familiarAugmentSlot.familiarID == familiarRef.id)
			{
				return new Dictionary<string, object>
				{
					{ "familiar_name", familiarRef.statName },
					{
						"familiar_aug_itemType",
						familiarAugmentSlot.familiarRef.itemType
					},
					{
						"familiar_aug_rarity",
						familiarAugmentSlot.familiarRef.rarity
					}
				};
			}
		}
		return new Dictionary<string, object>
		{
			{ "familiar_name", familiarRef.statName },
			{ "familiar_itemType", familiarRef.itemType },
			{ "familiar_rarity", familiarRef.rarity }
		};
	}
}
