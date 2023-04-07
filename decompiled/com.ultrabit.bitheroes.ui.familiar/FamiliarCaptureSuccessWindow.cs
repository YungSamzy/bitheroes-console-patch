using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.game;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.familiar;

public class FamiliarCaptureSuccessWindow : WindowsMain
{
	private const int ASSET_SCALE = 1;

	public TextMeshProUGUI successText;

	public TextMeshProUGUI nameText;

	public TextMeshProUGUI powerTxt;

	public TextMeshProUGUI staminaTxt;

	public TextMeshProUGUI agilityTxt;

	public Transform placeholderModifiers;

	public RectTransform placeholderAsset;

	public GameModifierBtn gameModifierPrefab;

	private FamiliarRef _familiarRef;

	private Transform _familiarAsset;

	private List<GameModifierBtn> _modifiers;

	private SWFAsset _asset;

	public void LoadDetails(FamiliarRef familiarRef)
	{
		Disable();
		_familiarRef = familiarRef;
		successText.text = Language.GetString("ui_familiar_capture_success");
		nameText.text = _familiarRef.coloredName;
		SetStats();
		CreateModifiers();
		CreateAsset();
		ListenForBack(OnClose);
		ListenForForward(OnClose);
		AddBG();
		CreateWindow(closeWord: true, "", scroll: false);
		Enable();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		if (_familiarAsset != null)
		{
			_familiarAsset.GetComponent<SortingGroup>().sortingOrder = base.sortingLayer + 1;
		}
	}

	private void SetStats()
	{
		int totalStats = GameData.instance.PROJECT.character.getTotalStats();
		powerTxt.text = Util.NumberFormat(_familiarRef.getPower(totalStats));
		staminaTxt.text = Util.NumberFormat(_familiarRef.getStamina(totalStats));
		agilityTxt.text = Util.NumberFormat(_familiarRef.getAgility(totalStats));
	}

	private void CreateModifiers()
	{
		if (_modifiers != null)
		{
			return;
		}
		_modifiers = new List<GameModifierBtn>();
		List<GameModifier> modifiers = _familiarRef.modifiers;
		if (modifiers == null || modifiers.Count <= 0)
		{
			return;
		}
		foreach (GameModifier item in modifiers)
		{
			if (item.type != 0)
			{
				GameModifierBtn gameModifierBtn = Object.Instantiate(gameModifierPrefab);
				gameModifierBtn.transform.SetParent(placeholderModifiers, worldPositionStays: false);
				gameModifierBtn.SetText(item.GetTileDesc());
				_modifiers.Add(gameModifierBtn);
			}
		}
	}

	private void CreateAsset()
	{
		if (!(_familiarAsset != null))
		{
			_familiarAsset = _familiarRef.displayRef.getAsset(center: true, 1f, placeholderAsset.transform);
			if (_familiarAsset != null)
			{
				_familiarAsset.localScale = new Vector3(-1f, 1f, 1f);
				OnAssetLoaded();
			}
		}
	}

	private void OnAssetLoaded()
	{
		_asset = _familiarAsset.gameObject.AddComponent<SWFAsset>();
		if (_asset != null)
		{
			_asset.PlayAnimation("idle");
		}
		Util.ChangeLayer(_familiarAsset.transform, "UI");
		SortingGroup sortingGroup = _familiarAsset.gameObject.AddComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = base.sortingLayer + 1;
	}

	public override void OnClose()
	{
		base.OnClose();
		DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}
}
