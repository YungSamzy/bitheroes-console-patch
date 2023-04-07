using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.fusion;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.game;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.fusion;

public class FusionCompleteWindow : WindowsMain
{
	public TextMeshProUGUI fusionText;

	public TextMeshProUGUI nameText;

	public TextMeshProUGUI powerTxt;

	public TextMeshProUGUI staminaTxt;

	public TextMeshProUGUI agilityTxt;

	public Button viewBtn;

	public Transform placeholderModifiers;

	public GameModifierBtn gameModifierPrefab;

	public Transform placeholderPosition;

	private FusionRef _fusionRef;

	private Transform _asset;

	private List<GameModifierBtn> _modifiers;

	public override void Start()
	{
		base.Start();
	}

	public void LoadDetails(FusionRef fusionRef, Transform asset)
	{
		Disable();
		_fusionRef = fusionRef;
		_asset = asset;
		_asset.SetParent(panel.transform, worldPositionStays: true);
		_asset.transform.position = new Vector3(_asset.transform.position.x, _asset.transform.position.y, 0f);
		_asset.transform.localScale = new Vector3(-1f, 1f, 1f);
		fusionText.text = Language.GetString("ui_fusion_success");
		nameText.text = _fusionRef.tradeRef.resultItem.itemRef.coloredName;
		SetStats();
		CreateModifiers();
		viewBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_view");
		GameData.instance.audioManager.PlaySoundLink("upgradeitem");
		float delay = 0.1f;
		float battleMovementDuration = Battle.GetBattleMovementDuration(asset.transform.position.x, placeholderPosition.transform.position.x, asset.transform.position.y, placeholderPosition.transform.position.y, 1f);
		Tween.StartMovement(asset.gameObject, placeholderPosition.transform.position.x, placeholderPosition.transform.position.y, battleMovementDuration, delay, OnAssetStop);
		GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, delay, CoroutineTimer.TYPE.SECONDS, delegate
		{
			PlayAnimation("walk");
		});
		ListenForBack(DoDestroy);
		CreateWindow(closeWord: true, "", scroll: false);
		Enable();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		SpriteRenderer[] componentsInChildren = _asset.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].maskInteraction = SpriteMaskInteraction.None;
		}
		SortingGroup sortingGroup = _asset.gameObject.GetComponent<SortingGroup>();
		if (sortingGroup == null)
		{
			sortingGroup = _asset.gameObject.AddComponent<SortingGroup>();
		}
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = layer + 1;
	}

	private void SetStats()
	{
		if (_fusionRef.tradeRef.resultItem.itemRef.itemType == 6)
		{
			int totalStats = GameData.instance.PROJECT.character.getTotalStats();
			FamiliarRef familiarRef = _fusionRef.tradeRef.resultItem.itemRef as FamiliarRef;
			powerTxt.text = Util.NumberFormat(familiarRef.getPower(totalStats));
			staminaTxt.text = Util.NumberFormat(familiarRef.getStamina(totalStats));
			agilityTxt.text = Util.NumberFormat(familiarRef.getAgility(totalStats));
		}
	}

	private void CreateModifiers()
	{
		if (_modifiers != null)
		{
			return;
		}
		_modifiers = new List<GameModifierBtn>();
		List<GameModifier> list = null;
		if (_fusionRef.tradeRef.resultItem.itemRef.itemType == 6)
		{
			list = (_fusionRef.tradeRef.resultItem.itemRef as FamiliarRef).modifiers;
		}
		if (list == null || list.Count <= 0)
		{
			return;
		}
		foreach (GameModifier item in list)
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

	public void OnViewBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowFamiliar(_fusionRef.tradeRef.resultItem.itemRef.id);
	}

	private void OnAssetStop(List<object> objs)
	{
		PlayAnimation("attack2");
	}

	private bool PlayAnimation(string animation)
	{
		if (_asset != null)
		{
			SWFAsset sWFAsset = _asset.GetComponent<SWFAsset>();
			if (sWFAsset == null)
			{
				sWFAsset = _asset.gameObject.AddComponent<SWFAsset>();
			}
			sWFAsset.StopAnimation();
			return sWFAsset.PlayAnimation(animation);
		}
		return false;
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
		viewBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		viewBtn.interactable = false;
	}
}
