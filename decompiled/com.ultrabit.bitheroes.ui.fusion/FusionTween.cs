using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.fusion;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.sound;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.fusion;

public class FusionTween : MonoBehaviour
{
	private const float SOUND_DELAY = 0.5f;

	public Transform placeholderRequirementA;

	public Transform placeholderRequirementB;

	public Transform placeholderRequirementC;

	public Transform placeholderResult;

	public SpriteMask mask;

	private FusionRef _fusionRef;

	private int _layer;

	private UnityAction _onCompleteCallback;

	private List<Transform> _requirementAssets;

	private Transform _resultAsset;

	private float[] times = new float[3] { 0f, 0.3f, 0.1f };

	private int indexTime;

	public Transform resultAsset => _resultAsset;

	public void LoadDetails(FusionRef fusionRef, UnityAction onCompleteCallback)
	{
		_fusionRef = fusionRef;
		_onCompleteCallback = onCompleteCallback;
		Util.ChangeLayer(base.transform, "UI");
		CreateAssets();
		_resultAsset.gameObject.SetActive(value: false);
		GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 0.5f, delegate
		{
			GameData.instance.audioManager.PlaySound(SoundBook.Lookup("fusion"));
		});
		StartCoroutine(FakeAnimationTime());
	}

	private IEnumerator FakeAnimationTime()
	{
		yield return new WaitForSeconds(7.15f);
		foreach (Transform requirementAsset in _requirementAssets)
		{
			requirementAsset.gameObject.SetActive(value: false);
		}
		_resultAsset.gameObject.SetActive(value: true);
		StartCoroutine(EndAnimationTime());
	}

	private IEnumerator EndAnimationTime()
	{
		yield return new WaitForSeconds(5f);
		_onCompleteCallback();
	}

	private void CreateAssets()
	{
		_resultAsset = GetItemAsset(_fusionRef.tradeRef.resultItem.itemRef);
		_resultAsset.transform.localScale = new Vector3(0f - _resultAsset.transform.localScale.x, _resultAsset.transform.localScale.y, _resultAsset.transform.localScale.z);
		_resultAsset.SetParent(placeholderResult, worldPositionStays: false);
		_resultAsset.localPosition = Vector3.zero;
		_requirementAssets = new List<Transform>();
		foreach (ItemData requiredItem in _fusionRef.tradeRef.requiredItems)
		{
			Transform transform = null;
			if (placeholderRequirementA.childCount <= 0)
			{
				transform = placeholderRequirementA;
			}
			else if (placeholderRequirementB.childCount <= 0)
			{
				transform = placeholderRequirementB;
			}
			else if (placeholderRequirementC.childCount <= 0)
			{
				transform = placeholderRequirementC;
			}
			if (!transform || transform.childCount > 0)
			{
				break;
			}
			Transform itemAsset = GetItemAsset(requiredItem.itemRef);
			if (!(itemAsset == null))
			{
				itemAsset.SetParent(transform, worldPositionStays: false);
				itemAsset.localPosition = Vector3.zero;
				_requirementAssets.Add(itemAsset);
				SetAsset(itemAsset);
			}
		}
		SetAsset(_resultAsset);
	}

	private Transform GetItemAsset(ItemRef itemRef)
	{
		if (itemRef.itemType == 6)
		{
			return (itemRef as FamiliarRef).displayRef.getAsset(center: true, 0.01f);
		}
		GameObject obj = new GameObject();
		obj.AddComponent<SpriteRenderer>().sprite = itemRef.GetSpriteIcon();
		obj.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
		return obj.transform;
	}

	private void SetAsset(Transform asset)
	{
		if ((bool)asset.GetComponent<SWFAsset>())
		{
			asset.GetComponent<SWFAsset>().PlayAnimation("idle");
		}
		HoverAndGlowEntity hoverAndGlowEntity = asset.gameObject.AddComponent<HoverAndGlowEntity>();
		hoverAndGlowEntity.GetTargetAssets();
		hoverAndGlowEntity.Brightness(0.15f);
		StartCoroutine(DoFlashOn(asset));
	}

	private IEnumerator DoFlashOn(Transform asset)
	{
		yield return new WaitForSeconds(times[indexTime]);
		HoverAndGlowEntity component = asset.GetComponent<HoverAndGlowEntity>();
		component.active = true;
		component.OnMouseOver();
		component.active = false;
		indexTime++;
		if (indexTime < times.Length - 1)
		{
			StartCoroutine(DoFlashOn(asset));
		}
	}

	private IEnumerator DoFlashOff(Transform asset)
	{
		yield return new WaitForSeconds(0.1f);
		HoverAndGlowEntity component = asset.GetComponent<HoverAndGlowEntity>();
		component.active = true;
		component.Brightness(0f);
		component.active = false;
	}

	public void SetAssetLayers(int layer)
	{
		_layer = layer;
		foreach (Transform requirementAsset in _requirementAssets)
		{
			SetAssetLayer(requirementAsset);
		}
		if (_resultAsset != null)
		{
			SetAssetLayer(_resultAsset);
		}
	}

	private void SetAssetLayer(Transform asset)
	{
		Util.ChangeLayer(asset.transform, "UI");
		SpriteRenderer[] componentsInChildren = asset.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
		}
		SortingGroup sortingGroup = asset.gameObject.GetComponent<SortingGroup>();
		if (sortingGroup == null)
		{
			sortingGroup = asset.gameObject.AddComponent<SortingGroup>();
		}
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = _layer + 1;
		mask.frontSortingLayerID = SortingLayer.NameToID("UI");
		mask.frontSortingOrder = _layer + 1;
		mask.backSortingLayerID = SortingLayer.NameToID("UI");
		mask.backSortingOrder = _layer;
	}
}
