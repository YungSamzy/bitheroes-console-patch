using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.sound;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.instance;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.grid;

public class GridObject : MonoBehaviour
{
	public const int DEFAULT_SPEED = 250;

	public const int DEFAULT_POSITION = -1;

	public const int FOOTSTEP_FRAMES = 10;

	public const float FOOTSTEP_VOLUME = 0.4f;

	private GridMap _grid;

	private Tile _tile;

	private float _speed;

	private bool _clickable;

	private object _data;

	private int _order;

	private int _layerPosition = -1;

	private GameObject _asset;

	private Vector2? _offset;

	private List<Tile> _path = new List<Tile>();

	private bool _moving;

	private int _soundFrames;

	private float _speedMult = 1f;

	private CharacterDisplay _characterDisplay;

	private HoverAndGlowEntity _hoverSprites;

	private float lastScale = 1f;

	private float lastYRotation;

	private bool _loop;

	private string _label;

	private string _endlabel;

	private SortingGroup _sortingGroup;

	[HideInInspector]
	public UnityCustomEvent MOVEMENT_CHANGE = new UnityCustomEvent();

	[HideInInspector]
	public UnityCustomEvent MOVEMENT_UPDATE = new UnityCustomEvent();

	[HideInInspector]
	public UnityCustomEvent MOVEMENT_START = new UnityCustomEvent();

	[HideInInspector]
	public UnityCustomEvent MOVEMENT_END = new UnityCustomEvent();

	[HideInInspector]
	public UnityCustomEvent MOVEMENT_STOP = new UnityCustomEvent();

	[HideInInspector]
	public UnityCustomEvent PATH_UPDATE = new UnityCustomEvent();

	[HideInInspector]
	public UnityCustomEvent POSITION_UPDATE = new UnityCustomEvent();

	[HideInInspector]
	public UnityCustomEvent ENTER_FRAME = new UnityCustomEvent();

	public GridMap grid => _grid;

	public GameObject asset => _asset;

	public List<Tile> path => _path;

	public bool moving => _moving;

	public Tile tile => _tile;

	public object data => _data;

	public int order => _order;

	public float speedMult => _speedMult;

	public int layerPosition => _layerPosition;

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
			if (this != null && base.transform != null)
			{
				return base.transform.position.y;
			}
			return 0f;
		}
		set
		{
			base.transform.position = new Vector3(base.transform.position.x, value, base.transform.position.z);
		}
	}

	public float localX
	{
		get
		{
			return base.transform.localPosition.x;
		}
		set
		{
			base.transform.localPosition = new Vector3(value, base.transform.localPosition.y, base.transform.localPosition.z);
		}
	}

	public float localY
	{
		get
		{
			return base.transform.localPosition.y;
		}
		set
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, value, base.transform.localPosition.z);
		}
	}

	public HoverAndGlowEntity hoverSprites => _hoverSprites;

	public void LoadDetails(GridMap grid, Tile tile, float speed = 250f, bool clickable = true, object data = null, int order = 0)
	{
		_tile = tile;
		_speed = speed;
		_data = data;
		_order = order;
		_characterDisplay = GetComponentInChildren<CharacterDisplay>();
		if (_characterDisplay != null)
		{
			Vector3 localPosition = _characterDisplay.transform.localPosition;
			localPosition -= new Vector3(0f, 20f, 0f);
			_characterDisplay.transform.localPosition = localPosition;
		}
		_clickable = clickable;
		base.gameObject.layer = LayerMask.NameToLayer("Mouse");
		SetupSortingLayer();
		SetClickable(clickable, force: true);
		SetData(data);
		SetGrid(grid);
	}

	public void SetupSortingLayer()
	{
		_sortingGroup = base.gameObject.AddComponent<SortingGroup>();
	}

	public void setPosition(int position)
	{
		_layerPosition = position;
		if (_sortingGroup != null && _sortingGroup.enabled)
		{
			_sortingGroup.sortingOrder = position;
		}
	}

	public Vector2 GetCameraPoint()
	{
		Vector3 position = base.transform.position;
		if (_characterDisplay != null && _grid is Instance)
		{
			return new Vector2(position.x + 0f, y - 20f / _grid.scale);
		}
		return position;
	}

	public void SetClickable(bool clickable, bool force = false)
	{
		if (_clickable != clickable || force)
		{
			_clickable = clickable;
			if (_hoverSprites != null)
			{
				_hoverSprites.SetHover(_clickable);
			}
		}
	}

	public void SetGrid(GridMap grid)
	{
		if (_grid != null)
		{
			return;
		}
		_grid = grid;
		if (!(_grid == null))
		{
			if (_hoverSprites != null)
			{
				_hoverSprites.SetGrid(_grid);
			}
			SetTile(_tile, tween: false);
		}
	}

	public virtual void SetExclamation(bool enabled = false)
	{
	}

	public virtual void SetData(object data)
	{
		_data = data;
	}

	public virtual void SetAsset(Asset asset, Vector2? offset = null)
	{
	}

	public virtual void SetAsset(GameObject asset, Vector2? offset = null)
	{
		float yRotation = lastYRotation;
		if (_asset != null)
		{
			float num = _asset.transform.rotation.eulerAngles.y % 360f;
			yRotation = ((num > 90f && num <= 270f) ? 180 : 0);
			if ((bool)_hoverSprites)
			{
				Object.Destroy(_hoverSprites);
			}
			Object.Destroy(_asset);
			_asset = null;
		}
		_asset = asset;
		if (_asset != null)
		{
			SetYRotation(yRotation);
			if (base.gameObject.activeInHierarchy && _asset != null)
			{
				StartCoroutine(WaitForObjectCreation());
			}
		}
	}

	private IEnumerator WaitForObjectCreation()
	{
		yield return new WaitForEndOfFrame();
		OnAssetLoaded();
	}

	public void SetOffset(Vector2 offset)
	{
		_offset = offset;
		Vector2 targetPoint = GetTargetPoint();
		x = targetPoint.x;
		y = targetPoint.y;
	}

	public Vector2 GetTargetPoint()
	{
		Vector2 result = ((tile != null) ? tile.GetPosition(this) : new Vector2(0f, 0f));
		if (_offset.HasValue)
		{
			result.x += _offset.Value.x;
			result.y -= _offset.Value.y;
		}
		return result;
	}

	private void OnAssetLoaded()
	{
		if (!(_asset == null))
		{
			AddCollidersAndHover();
		}
	}

	public void AddCollidersAndHover()
	{
		CharacterDisplay componentInChildren = GetComponentInChildren<CharacterDisplay>();
		if (componentInChildren != null && componentInChildren.hasMountEquipped())
		{
			BoxCollider2D[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoxCollider2D>();
			foreach (BoxCollider2D boxCollider2D in componentsInChildren)
			{
				if (boxCollider2D != null)
				{
					_hoverSprites = boxCollider2D.gameObject.GetComponent<HoverAndGlowEntity>();
					if (_hoverSprites == null)
					{
						_hoverSprites = boxCollider2D.gameObject.AddComponent<HoverAndGlowEntity>();
					}
					if (_hoverSprites != null)
					{
						_hoverSprites.GetTargetAssets(new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>(includeInactive: true)));
						_hoverSprites.SetHover(_clickable);
					}
				}
			}
			return;
		}
		BoxCollider2D componentInChildren2 = base.gameObject.GetComponentInChildren<BoxCollider2D>();
		if (componentInChildren2 != null)
		{
			_hoverSprites = componentInChildren2.gameObject.GetComponent<HoverAndGlowEntity>();
			if (_hoverSprites == null)
			{
				_hoverSprites = componentInChildren2.gameObject.AddComponent<HoverAndGlowEntity>();
			}
			if (_hoverSprites != null)
			{
				_hoverSprites.GetTargetAssets();
				_hoverSprites.SetHover(_clickable);
			}
		}
	}

	public void SetSpeedMult(float mult)
	{
		_speedMult = mult;
	}

	public void SetTile(Tile tile, bool tween = true)
	{
		_tile = tile;
		if (_tile == null)
		{
			return;
		}
		Vector2 targetPoint = GetTargetPoint();
		float speed = GetSpeed();
		if (tween)
		{
			if (x != targetPoint.x || y != targetPoint.y)
			{
				float distance = Util.GetDistance(x, y, targetPoint.x, targetPoint.y);
				float num = (float)_grid.size / distance;
				float num2 = 1f / (speed / (float)_grid.size) / num;
				if (AppInfo.TESTING)
				{
					num2 /= 3f;
				}
				Tween.StartMovement(base.gameObject, targetPoint.x, targetPoint.y, num2, 0f, EndMovement);
				GameData.instance.main.DISPATCHER.FRAME_UPDATE.AddListener(OnMovement);
				_moving = true;
			}
		}
		else
		{
			if (_characterDisplay != null)
			{
				_characterDisplay.characterPuppet.PlayAnimation(CharacterPuppet.AnimationSequence.idle);
			}
			x = targetPoint.x;
			y = targetPoint.y;
		}
	}

	public virtual void SetPath(List<Tile> path)
	{
		_path = path;
		StartMovement();
	}

	public Tile GetTargetTile()
	{
		if (_path.Count <= 0)
		{
			return null;
		}
		return _path[_path.Count - 1];
	}

	public void PlayAnimation(string label, bool loop = true, string endLabel = null)
	{
		_label = label;
		_endlabel = endLabel;
		_loop = loop;
		SWFAsset sWFAsset;
		if (_asset != null && (bool)_asset.GetComponent<SWFAsset>())
		{
			CharacterDisplay componentInChildren = _asset.GetComponentInChildren<CharacterDisplay>();
			if (!(componentInChildren != null) || !componentInChildren.hasMountEquipped())
			{
				goto IL_00c1;
			}
			switch (label)
			{
			case "fishingCast":
			case "fishingCastingIdle":
			case "fishingCastingStart":
			case "fishingCatchingIdle":
			case "fishingCatchingStart":
				break;
			default:
				goto IL_00c1;
			}
			sWFAsset = componentInChildren.characterPuppet.GetComponent<SWFAsset>();
			if (sWFAsset == null)
			{
				sWFAsset = componentInChildren.characterPuppet.gameObject.AddComponent<SWFAsset>();
			}
			goto IL_00cd;
		}
		return;
		IL_00c1:
		sWFAsset = _asset.GetComponent<SWFAsset>();
		goto IL_00cd;
		IL_00cd:
		if (!(sWFAsset.label == label))
		{
			float speed = ((label == "walk") ? (GetSpeed() / 250f) : 1f);
			sWFAsset.StopAnimation();
			sWFAsset.ANIMATION_END.RemoveAllListeners();
			if (_loop)
			{
				sWFAsset.ANIMATION_END.AddListener(OnAnimationEnd);
			}
			sWFAsset.PlayAnimation(label, speed, endLabel);
		}
	}

	private void OnAnimationEnd(SWFAsset asset)
	{
		if (_endlabel != null)
		{
			_label = _endlabel;
			_endlabel = null;
		}
		float speed = ((_label == "walk") ? (GetSpeed() / 250f) : 1f);
		asset.PlayAnimation(_label, speed);
	}

	public virtual void CheckActions()
	{
	}

	public void StartMovement()
	{
		if (_path.Count <= 0)
		{
			PlayAnimation("idle");
			CheckActions();
		}
		else if (!_moving)
		{
			Tile tile = _path[0];
			_path.RemoveAt(0);
			SetTile(tile);
			if ((float)tile.x < x)
			{
				SetYRotation(180f);
			}
			else if ((float)tile.x > x)
			{
				SetYRotation(0f);
			}
			PlayAnimation("walk");
			MOVEMENT_START.Invoke(this);
			PATH_UPDATE.Invoke(this);
			POSITION_UPDATE.Invoke(this);
		}
	}

	public void SetScale(float scale)
	{
		lastScale = scale;
		if (!(_asset == null))
		{
			_asset.transform.localScale = new Vector3(Mathf.Abs(_asset.transform.localScale.x) * scale, _asset.transform.localScale.y, _asset.transform.localScale.z);
		}
	}

	public void SetYRotation(float yEulerAngle)
	{
		lastYRotation = yEulerAngle;
		if (!(_asset == null))
		{
			_asset.transform.rotation = Quaternion.Euler(_asset.transform.rotation.eulerAngles.x, yEulerAngle, _asset.transform.rotation.eulerAngles.z);
		}
	}

	private void EndMovement(List<object> e = null)
	{
		_moving = false;
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnMovement);
		OnMovement();
		Tile arg = tile;
		if (_path.Count <= 0)
		{
			_soundFrames = 0;
			PlayAnimation("idle");
			MOVEMENT_END.Invoke(this);
		}
		MOVEMENT_STOP.Invoke(this);
		StartMovement();
		MOVEMENT_CHANGE.Invoke(arg);
	}

	private void OnMovement(object e = null)
	{
		CheckFootstep();
		MOVEMENT_UPDATE.Invoke(this);
	}

	private void CheckFootstep()
	{
		if (_grid == null || GameData.instance == null)
		{
			return;
		}
		_soundFrames++;
		int num = Mathf.RoundToInt(1f / Time.smoothDeltaTime * 10f / (float)Main.FPS_TARGET);
		if (_soundFrames < num || !(_grid.focus != null))
		{
			return;
		}
		Tile tileFromPoint = _grid.getTileFromPoint(x, y);
		if (tileFromPoint == null)
		{
			return;
		}
		SoundPoolRef soundPoolRef = tileFromPoint.soundPoolRef ?? _grid.footstepsDefault;
		if (soundPoolRef == null)
		{
			return;
		}
		SoundRef randomSound = soundPoolRef.getRandomSound();
		if (randomSound != null)
		{
			float speed = GetSpeed();
			_ = 250f / speed;
			float volume = 1f;
			if (base.gameObject.GetInstanceID() != _grid.focus.gameObject.GetInstanceID())
			{
				float num2 = Util.GetDistance(_grid.focus.x, _grid.focus.y, x, y) / Main.BOUNDS.height;
				volume = (1f - num2) * 0.4f;
			}
			GameData.instance.audioManager.PlaySound(randomSound, volume);
			_soundFrames = 0;
		}
	}

	public float GetSpeed()
	{
		return _speed * _speedMult;
	}

	public void StopTween()
	{
		Tween.StopMovement(base.gameObject);
	}

	public void ClearPath(bool finish = true)
	{
		_path.Clear();
		if (finish)
		{
			EndMovement();
		}
	}

	public void ChangeColliders(bool enabled)
	{
		BoxCollider2D component = GetComponent<BoxCollider2D>();
		if (component != null)
		{
			component.enabled = enabled;
		}
		BoxCollider2D[] componentsInChildren = GetComponentsInChildren<BoxCollider2D>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = enabled;
		}
	}

	public virtual void OnDestroy()
	{
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnMovement);
		StopTween();
		ClearPath(finish: false);
	}
}
