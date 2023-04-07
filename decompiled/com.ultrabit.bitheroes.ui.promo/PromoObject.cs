using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.promo;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.promo;

public class PromoObject : MonoBehaviour
{
	public enum TYPE
	{
		BRAWL,
		RAID,
		NEWS,
		SHOP
	}

	public GameObject loadingIcon;

	private PromoRef _promoRef;

	private Transform _placeholderPromo;

	private TYPE _type;

	private Transform _asset;

	private List<Transform> _assetObjects = new List<Transform>();

	private float backgroundWidth;

	private float backgroundHeight;

	private float xPerc;

	private float yPerc;

	private float backgroundOffsetX;

	private float backgroundOffsetY;

	public int assetsCount => _assetObjects.Count;

	public void LoadDetails(PromoRef promoRef, Transform placeholderPromo, TYPE type, bool clickable = true)
	{
		_promoRef = promoRef;
		_placeholderPromo = placeholderPromo;
		_type = type;
		SetupMeasures(type);
	}

	private void SetupMeasures(TYPE type)
	{
		float num = 0f;
		float num2 = 0f;
		backgroundWidth = 0f;
		backgroundHeight = 0f;
		switch (type)
		{
		case TYPE.BRAWL:
		case TYPE.RAID:
			num = 420f;
			num2 = 260f;
			break;
		case TYPE.NEWS:
		case TYPE.SHOP:
			num = 380f;
			num2 = 220f;
			break;
		}
		backgroundWidth = num / 3f;
		backgroundHeight = num2 / 3f;
		xPerc = backgroundWidth * 100f / num;
		yPerc = backgroundHeight * 100f / num2;
		backgroundOffsetX = backgroundWidth * 0.5f;
		backgroundOffsetY = backgroundHeight * 0.5f;
	}

	public void CreateAssets(int sortingOrder)
	{
		if (_asset != null)
		{
			return;
		}
		Transform asset = _promoRef.getAsset();
		if (asset != null)
		{
			asset.SetParent(_placeholderPromo);
			asset.localPosition = Vector3.zero;
			if (asset.GetComponent<SpriteRenderer>() != null)
			{
				SpriteRenderer component = asset.GetComponent<SpriteRenderer>();
				Sprite sprite = component.sprite;
				Image image = asset.gameObject.AddComponent<Image>();
				image.sprite = sprite;
				image.GetComponent<RectTransform>().sizeDelta = new Vector2(backgroundWidth, backgroundHeight);
				image.gameObject.transform.localScale = Vector3.one;
				component.enabled = false;
				image.gameObject.transform.SetSiblingIndex(0);
			}
			else if (_type == TYPE.NEWS)
			{
				RectTransform rectTransform = asset.transform as RectTransform;
				Vector2 vector = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
				Vector2 vector2 = new Vector2(backgroundWidth * 3f, backgroundHeight * 3f);
				if (Vector2.Dot(vector.normalized, vector2.normalized) != 1f)
				{
					D.LogWarning($"A News Promo's size is not multiple of {vector2.x}x{vector2.y}, it might show stretched. ({asset.name})");
				}
				float num = vector2.x / vector.x * xPerc;
				float num2 = vector2.y / vector.y * yPerc;
				num /= 100f;
				num2 /= 100f;
				asset.localScale = new Vector3(num, num2, 1f);
			}
		}
		foreach (PromoObjectRef @object in _promoRef.objects)
		{
			Transform transform = null;
			bool flag = true;
			Vector3 localScale = Vector3.one;
			if (@object.hasAnimation)
			{
				Transform asset2 = @object.displayRef.getAsset(center: true);
				transform = GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.GetPath(AssetURL.PROMO_OBJECT, @object.animation));
				float num3 = 0f;
				if (@object.displayRef.scale > 0f)
				{
					num3 = @object.displayRef.scale / 10f;
				}
				localScale = new Vector3(transform.localScale.x + num3, transform.localScale.y + num3, transform.localScale.z + num3);
				flag = false;
				if (transform != null && asset2 != null)
				{
					asset2.SetParent(transform.Find("Animator/icon"));
					asset2.localPosition = Vector3.zero;
					asset2.localScale = Vector3.one;
				}
			}
			else
			{
				transform = @object.displayRef.getAsset(center: true, @object.displayRef.scale);
			}
			if (@object.displayRef.getItemRef() != null && @object.displayRef.getItemRef().itemType == 8)
			{
				Transform transform2 = Util.RecursiveFindChild(transform.transform, "character");
				if (transform2 != null)
				{
					Object.Destroy(transform2.gameObject);
				}
				localScale = Vector3.one * (@object.displayRef.scale * 100f) / 3f;
				flag = false;
			}
			if (transform != null)
			{
				transform.SetParent(base.transform);
				transform.localPosition = Vector3.zero;
				if (flag)
				{
					Vector3 localScale2 = transform.localScale / @object.displayRef.scale;
					localScale2.x *= ((!@object.displayRef.flipped) ? 1 : (-1));
					localScale2 /= 3f;
					localScale2 *= 1.15f;
					transform.localScale = localScale2;
				}
				else
				{
					transform.localScale = localScale;
				}
				Vector3 zero = Vector3.zero;
				float num4 = @object.displayRef.position.x * xPerc / 100f;
				float num5 = @object.displayRef.position.y * yPerc / 100f;
				zero.x = backgroundOffsetX * -1f + num4;
				zero.y = (num5 - backgroundOffsetY) * -1f;
				if (@object.displayRef.equipment != null)
				{
					zero.y += 15f;
				}
				if (@object.displayRef.mounts != null)
				{
					zero.y -= 15f;
				}
				transform.localPosition = zero;
				Util.ChangeLayer(transform, "UI");
				SortingGroup sortingGroup = transform.gameObject.AddComponent<SortingGroup>();
				if (sortingGroup != null && sortingGroup.enabled)
				{
					sortingGroup.sortingLayerName = "UI";
					sortingGroup.sortingOrder = sortingOrder;
				}
				SpriteRenderer[] componentsInChildren = transform.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
				}
				_assetObjects.Add(transform);
				CheckObjectAsset(transform);
			}
		}
		_assetObjects = _assetObjects.OrderByDescending((Transform item) => item.transform.position.y).ToList();
		for (int j = 0; j < _assetObjects.Count; j++)
		{
			SortingGroup component2 = _assetObjects[j].gameObject.GetComponent<SortingGroup>();
			if (component2 != null && component2.enabled)
			{
				component2.sortingOrder += j;
			}
		}
	}

	private void CheckObjectAsset(Transform asset)
	{
		if (asset.GetComponent<SWFAsset>() != null)
		{
			asset.GetComponent<SWFAsset>().PlayAnimation("idle");
		}
	}
}
