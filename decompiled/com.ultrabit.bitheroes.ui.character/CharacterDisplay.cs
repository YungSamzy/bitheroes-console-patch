using System;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterDisplay : Asset
{
	public enum PuppetType
	{
		defaultPuppet,
		imxG0Puppet
	}

	public CharacterPuppetDefault characterPuppetDefaultPrefab;

	public CharacterPuppetIMXG0 characterPuppetIMXG0Prefab;

	[SerializeField]
	private CharacterPuppet _characterPuppet;

	[SerializeField]
	private GameObject _loading;

	public SpriteMask[] iconMasks;

	private MountRef _mountRef;

	private SWFAsset _mount;

	private Mounts _mounts;

	private Vector3 cPuppetScale;

	public const int Y_POS_DEFAULT_PUPPET = 63;

	public const int Y_POS_DUNGEON_PUPPET = 42;

	private Bounds _bounds;

	public Action BOUNDS_UPDATED;

	public CharacterPuppet characterPuppet => _characterPuppet;

	public Bounds bounds => _bounds;

	private bool placeholderExists
	{
		get
		{
			Transform placeholder = GetPlaceholder();
			if (placeholder != null)
			{
				SpriteRenderer component = placeholder.GetComponent<SpriteRenderer>();
				if (component != null)
				{
					SortingGroup sortingGroup = _characterPuppet.GetComponent<SortingGroup>();
					if (sortingGroup == null)
					{
						sortingGroup = _characterPuppet.gameObject.AddComponent<SortingGroup>();
					}
					if (base.isActiveAndEnabled && sortingGroup.enabled)
					{
						sortingGroup.sortingOrder = component.sortingOrder;
					}
					SpriteRenderer[] componentsInChildren = _mount.transform.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
					foreach (SpriteRenderer spriteRenderer in componentsInChildren)
					{
						if (component != spriteRenderer && spriteRenderer.enabled && spriteRenderer.sortingOrder >= component.sortingOrder)
						{
							spriteRenderer.sortingOrder += 5;
						}
					}
					SortingGroup sortingGroup2 = placeholder.gameObject.AddComponent<SortingGroup>();
					if (placeholder.gameObject.activeInHierarchy && sortingGroup2 != null && sortingGroup2.enabled)
					{
						sortingGroup2.sortingLayerName = component.sortingLayerName;
						sortingGroup2.sortingOrder = component.sortingOrder;
					}
					component.enabled = false;
				}
				_characterPuppet.transform.parent = placeholder;
				Invoke("ResetPosition", 0.1f);
				return true;
			}
			return false;
		}
	}

	public Transform mainhandAsset => characterPuppet.mainHandAsset;

	public Transform petAsset => characterPuppet.petAsset;

	public override void Awake()
	{
		base.Awake();
		if (_characterPuppet == null)
		{
			D.LogError(string.Format("{0}.{1}() :: {2} is not set.", GetType(), "Awake", "_characterPuppet"));
		}
		SpriteMask[] array = iconMasks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(value: false);
		}
	}

	public void SetCharacterDisplay(CharacterPuppetInfo puppetInfo)
	{
		_loading.SetActive(value: false);
		SpriteMask[] array = iconMasks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].transform.localScale.Scale(new Vector3(puppetInfo.scale, puppetInfo.scale, 1f));
		}
		_characterPuppet.SetCharacterPuppet(puppetInfo);
		CharacterPuppet obj = _characterPuppet;
		obj.ON_ENABLED = (Action)Delegate.Combine(obj.ON_ENABLED, (Action)delegate
		{
			BOUNDS_UPDATED?.Invoke();
		});
		if (puppetInfo.showMount && puppetInfo.mounts != null)
		{
			SetMount(puppetInfo.mounts);
		}
		int num = ((puppetInfo.scale != 2f) ? 63 : 42);
		_characterPuppet.transform.localPosition = new Vector3(0f, num, 0f);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireCube(new Vector2(base.transform.position.x, base.transform.position.y + _bounds.extents.y), _bounds.size);
	}

	[ContextMenu("GetChildRenderersBounds")]
	private Bounds GetChildRenderersBounds()
	{
		float num = float.MaxValue;
		float num2 = float.MinValue;
		Renderer[] array = ((_mount != null) ? _mount.GetComponentsInChildren<Renderer>(includeInactive: false) : _characterPuppet.GetComponentsInChildren<Renderer>(includeInactive: false));
		if (array.Length != 0)
		{
			int i = 1;
			for (int num3 = array.Length; i < num3; i++)
			{
				if (array[i].gameObject.activeInHierarchy)
				{
					num = Math.Min(num, array[i].transform.position.y - array[i].bounds.extents.y);
					num2 = Math.Max(num2, array[i].transform.position.y + array[i].bounds.extents.y);
				}
			}
			return new Bounds(new Vector2(base.transform.position.x, (num2 + num) / 2f), new Vector2(0f, num2 - num));
		}
		return default(Bounds);
	}

	public void SetLocalPosition(Vector3 newPos)
	{
		base.transform.localPosition = newPos;
	}

	public void HideMaskedElements()
	{
		_characterPuppet.HideMaskedElements();
	}

	private void SetMount(Mounts mounts)
	{
		if (mounts != null)
		{
			_mounts = mounts;
			cPuppetScale = _characterPuppet.transform.localScale;
			_mounts.OnChange.AddListener(UpdateMount);
			UpdateMount();
		}
	}

	private void UpdateMount()
	{
		_characterPuppet.transform.parent = base.transform;
		_characterPuppet.ShowShadow(show: true);
		if (_mount != null)
		{
			UnityEngine.Object.Destroy(_mount.gameObject);
			_mount = null;
		}
		MountData mountEquipped = _mounts.getMountEquipped();
		if (mountEquipped == null)
		{
			return;
		}
		MountRef mountRef = ((!(_mounts.cosmetic != null)) ? mountEquipped.mountRef : _mounts.cosmetic);
		GameObject gOAsset = mountRef.getGOAsset();
		if (!(gOAsset == null))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(gOAsset);
			gameObject.transform.localScale = cPuppetScale;
			Util.ChangeLayer(gameObject.transform, LayerMask.LayerToName(_characterPuppet.gameObject.layer));
			_mountRef = mountRef;
			_mount = gameObject.GetComponent<SWFAsset>();
			if (!(_mount == null))
			{
				OnMountLoaded();
			}
		}
	}

	private void OnMountLoaded()
	{
		_mount.PlayAnimation("idle");
		_mount.transform.parent = base.transform;
		_mount.transform.localPosition = Vector3.zero;
		_mount.transform.localScale = cPuppetScale;
		float num = _mount.transform.localScale.x;
		ParticleSystem[] componentsInChildren = _mount.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem obj in componentsInChildren)
		{
			ParticleSystem.MainModule main = obj.main;
			main.startSize = new ParticleSystem.MinMaxCurve(main.startSize.constantMax * num, main.startSize.constantMax * num);
			main.gravityModifier = new ParticleSystem.MinMaxCurve(main.gravityModifier.constantMax * num, main.gravityModifier.constantMax * num);
			main.startSpeed = new ParticleSystem.MinMaxCurve(main.startSpeed.constantMax * num, main.startSpeed.constantMax * num);
			ParticleSystem.ShapeModule shape = obj.shape;
			Vector3 position = shape.position;
			position.x *= num;
			position.y *= num;
			position.z *= num;
			shape.position = position;
			Vector3 scale = shape.scale;
			scale.x *= num;
			scale.y *= num;
			scale.z *= num;
			shape.scale = scale;
			shape.radius *= num / 100f;
		}
		if (!placeholderExists)
		{
			UnityEngine.Object.Destroy(_mount.gameObject);
			_mountRef = null;
			_mount = null;
		}
		else
		{
			_characterPuppet.ShowLeg(!_mountRef.hideFoot);
			_characterPuppet.ShowShadow(show: false);
		}
	}

	private void ResetPosition()
	{
		Vector3 zero = Vector3.zero;
		zero.y = 0.069f;
		_characterPuppet.transform.localPosition = zero;
		_characterPuppet.transform.localEulerAngles = Vector3.zero;
		_bounds = GetChildRenderersBounds();
		BOUNDS_UPDATED?.Invoke();
	}

	private Transform GetPlaceholder()
	{
		Transform placeholderRecursive = GetPlaceholderRecursive(_mount.transform);
		if (placeholderRecursive != null)
		{
			return placeholderRecursive;
		}
		return null;
	}

	private Transform GetPlaceholderRecursive(Transform mountPart)
	{
		if (mountPart.childCount <= 0)
		{
			return null;
		}
		for (int i = 0; i < mountPart.childCount; i++)
		{
			Transform child = mountPart.GetChild(i);
			Transform transform = ((!(child.name == "character")) ? GetPlaceholderRecursive(child) : child);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	public void SetEquipmentVisibility(EquipmentRef equipmentRef, bool vis)
	{
	}

	public bool hasMountEquipped()
	{
		return _mount != null;
	}

	internal Asset convertToIcon(bool center, float scale)
	{
		throw new Exception("Error --> CONTROL");
	}

	public void ConvertToIcon(int sortingLayer, Transform parent)
	{
		characterPuppet.StopAllAnimations();
		base.transform.SetParent(parent, worldPositionStays: false);
		SetLocalPosition(new Vector3(0f, -63f, 0f));
		Util.ChangeLayer(base.transform, "UI");
		Debug.LogFormat("Log: <color=yellow>{0}</color>", "CONVERT TO ICON");
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		SpriteMask[] array = iconMasks;
		foreach (SpriteMask obj in array)
		{
			obj.gameObject.SetActive(value: true);
			obj.frontSortingLayerID = SortingLayer.NameToID("UI");
			obj.frontSortingOrder = sortingLayer + 1;
			obj.backSortingLayerID = SortingLayer.NameToID("UI");
			obj.backSortingOrder = sortingLayer;
			obj.transform.SetParent(base.transform.parent, worldPositionStays: true);
		}
		SortingGroup sortingGroup = base.gameObject.AddComponent<SortingGroup>();
		if (base.isActiveAndEnabled && sortingGroup.enabled)
		{
			sortingGroup.sortingLayerName = "UI";
			sortingGroup.sortingOrder = sortingLayer + 1;
		}
	}

	private void OnDestroy()
	{
		SpriteMask[] array = iconMasks;
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i].gameObject);
		}
		CharacterPuppet obj = _characterPuppet;
		obj.ON_ENABLED = (Action)Delegate.Remove(obj.ON_ENABLED, (Action)delegate
		{
			BOUNDS_UPDATED?.Invoke();
		});
	}

	internal void remove()
	{
		throw new NotImplementedException();
	}
}
