using System;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.mount;

public class MountDataTile : MonoBehaviour
{
	private MountData _mountData;

	private int _tier;

	private MountRef _mountRef;

	private SWFAsset _mount;

	private int _layer = -1;

	private const float PUPPET_MAX_WORLD_Y_SIZE = 50f;

	private bool _hasAbilities;

	private Bounds _bounds;

	private Transform _content;

	public Bounds bounds => _bounds;

	private void Awake()
	{
		_content = base.transform.Find("Content");
	}

	public void SetMountDataTime(MountData mountData, int tier, bool hasAbilities = false)
	{
		_hasAbilities = hasAbilities;
		_mountData = mountData;
		_tier = tier;
		if (hasAbilities && _content != null)
		{
			_content.transform.localPosition = Vector3.zero;
		}
		_mountRef = _mountData.mountRef;
		if (_mount != null)
		{
			UnityEngine.Object.Destroy(_mount.gameObject);
			_mount = null;
		}
		SetMount();
	}

	private void SetMount()
	{
		if (_mountRef == null)
		{
			return;
		}
		GameObject gOAsset = _mountRef.getGOAsset();
		if (gOAsset == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(gOAsset);
		_mount = gameObject.GetComponent<SWFAsset>();
		if (!(_mount == null))
		{
			_mount.ChangeLayer("Overall");
			_mount.PlayAnimation("idle");
			_mount.transform.parent = base.transform;
			_bounds = GetChildRenderersBounds();
			if (_hasAbilities)
			{
				_mount.transform.localPosition = new Vector3(0f, 10f, 0f);
			}
			else
			{
				_mount.transform.localPosition = new Vector3(0f, -11f, 0f);
			}
			CheckAndUpdatePuppetSize();
			if (_layer > -1)
			{
				SetLayer(_layer);
			}
		}
	}

	private Bounds GetChildRenderersBounds()
	{
		float num = float.MaxValue;
		float num2 = float.MinValue;
		Renderer[] componentsInChildren = _mount.GetComponentsInChildren<Renderer>(includeInactive: false);
		if (componentsInChildren.Length != 0)
		{
			int i = 1;
			for (int num3 = componentsInChildren.Length; i < num3; i++)
			{
				if (componentsInChildren[i].gameObject.activeInHierarchy)
				{
					num = Math.Min(num, componentsInChildren[i].transform.position.y - componentsInChildren[i].bounds.extents.y);
					num2 = Math.Max(num2, componentsInChildren[i].transform.position.y + componentsInChildren[i].bounds.extents.y);
				}
			}
			return new Bounds(new Vector2(base.transform.position.x, (num2 + num) / 2f), new Vector2(0f, num2 - num));
		}
		return default(Bounds);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireCube(new Vector2(base.transform.position.x, base.transform.position.y + _bounds.extents.y), _bounds.size);
	}

	private void CheckAndUpdatePuppetSize()
	{
		if (bounds.size.y * 100f > 50f)
		{
			_mount.transform.localScale = Vector2.one * 50f / bounds.size.y;
		}
		else
		{
			_mount.transform.localScale = Vector3.one * 100f;
		}
	}

	public void SetLayer(int layer)
	{
		_layer = layer;
		if (_mountData != null && !(_mount == null))
		{
			Util.ChangeLayer(_mount.transform, "UI");
			SortingGroup sortingGroup = _mount.gameObject.GetComponent<SortingGroup>();
			if (sortingGroup == null)
			{
				sortingGroup = _mount.gameObject.AddComponent<SortingGroup>();
			}
			if (sortingGroup.enabled)
			{
				sortingGroup.sortingLayerName = "UI";
				sortingGroup.sortingOrder = _layer + 1;
			}
			Transform transform = Util.RecursiveFindChild(_mount.transform, "character");
			if (transform != null)
			{
				transform.gameObject.SetActive(value: false);
			}
		}
	}
}
