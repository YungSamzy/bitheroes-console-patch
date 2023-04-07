using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.armory.rune;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.rune;

public class ArmoryRuneTile : ItemIcon
{
	public Transform[] runeSlotsPrefabs;

	public const float ASSET_SCALE = 3f;

	public Image _asset;

	private GameObject slotIcon;

	private int _slot;

	private bool _changeable;

	private ArmoryRunesWindow _runesWindow;

	private ArmoryRunes _runes;

	private RuneRef _runeRef;

	private int _runeType;

	public int slot => _slot;

	public ArmoryRunesWindow runesWindow => _runesWindow;

	public RuneRef runeRef => _runeRef;

	public int runeType => _runeType;

	public void LoadDetails(ArmoryRunesWindow runesWindow, int slot, ArmoryRunes runes, RuneRef runeRef = null, bool changeable = false, int runeType = -1)
	{
		_runesWindow = runesWindow;
		_slot = slot;
		_changeable = changeable;
		_runeType = runeType;
		slotIcon = Object.Instantiate(runeSlotsPrefabs[Runes.getSlotType(_slot) - 1]).gameObject;
		slotIcon.transform.SetParent(base.transform, worldPositionStays: false);
		slotIcon.transform.SetAsFirstSibling();
		SetRunes(runes);
		SetRune(runeRef);
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (AppInfo.IsMobile())
		{
			if (base.itemActionBase != null)
			{
				CreateRuneTooltip();
			}
			else
			{
				DoSelected();
			}
		}
		else
		{
			DoSelected();
		}
	}

	private void CreateRuneTooltip()
	{
		if (base.itemActionBase != null)
		{
			base.itemActionBase.SetTooltipType(18);
		}
		GameData.instance.windowGenerator.NewItemToolTipContainer(this, base.characterData, null, 11).GetComponent<ItemTooltipContainer>().dispatcher.AddListener(ItemTooltipContainer.BUTTON_CHANGE_SELECTED, OnChangeButtonSelected);
	}

	private void OnChangeButtonSelected()
	{
		DoSelected();
	}

	private void DoSelected()
	{
		if (_changeable)
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			List<RuneRef> runeSlotMemory = _runes.getRuneSlotMemory(_slot);
			if (runeSlotMemory != null && runeSlotMemory.Count > 0)
			{
				ShowChangeableRunes();
			}
			else
			{
				GameData.instance.windowGenerator.NewMessageWindow(DialogWindow.TYPE.TYPE_OK, Language.GetString("error_name"), Language.GetString("error_rune_insert_unavailable"));
			}
		}
	}

	public void ShowChangeableRunes(object parent = null)
	{
		List<ItemData> changeableArmoryRunes = GameData.instance.PROJECT.character.runes.getChangeableArmoryRunes(_slot);
		if (changeableArmoryRunes.Count <= 0)
		{
			GameData.instance.windowGenerator.NewMessageWindow(DialogWindow.TYPE.TYPE_OK, Language.GetString("error_name"), Language.GetString("error_rune_change_unavailable"));
			return;
		}
		for (int i = 0; i < changeableArmoryRunes.Count; i++)
		{
			(changeableArmoryRunes[i].itemRef as RuneRef).amoryRuneTile = this;
			(changeableArmoryRunes[i].itemRef as RuneRef).runeAction = 20;
		}
		GameData.instance.windowGenerator.NewItemListWindow(changeableArmoryRunes, compare: false, added: false, Util.FirstCharToUpper(RuneRef.getRuneTypeName(runeType)), large: false, forceNonEquipment: false, select: true, null, parent as GameObject);
	}

	public void SetRunes(ArmoryRunes runes)
	{
		_runes = runes;
	}

	public void SetRune(RuneRef runeRef)
	{
		_runeRef = runeRef;
		HoverImages hoverImages = GetComponent<HoverImages>();
		if (hoverImages == null)
		{
			hoverImages = base.gameObject.AddComponent<HoverImages>();
		}
		if (_runeRef == null)
		{
			_asset.gameObject.SetActive(value: false);
			hoverImages.ForceStart();
			return;
		}
		_asset.gameObject.SetActive(value: true);
		SetItemActionType(0);
		SetRuneData(runeRef);
		hoverImages.ForceStart();
	}

	public void Animate()
	{
		StartCoroutine(Zoom(0.25f));
	}

	private IEnumerator Zoom(float delay)
	{
		yield return new WaitForSeconds(delay);
		DoTileAnimation();
	}

	private void DoTileAnimation()
	{
		Vector3 localScale = _asset.transform.localScale;
		_ = _asset.transform.localScale * 7f;
		_asset.transform.DOScale(localScale, 0.5f);
		Color endValue = new Color(_asset.color.r, _asset.color.g, _asset.color.b, 1f);
		_asset.color = new Color(_asset.color.r, _asset.color.g, _asset.color.b, 0f);
		_asset.DOColor(endValue, 0.5f).SetEase(Ease.OutCubic);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
