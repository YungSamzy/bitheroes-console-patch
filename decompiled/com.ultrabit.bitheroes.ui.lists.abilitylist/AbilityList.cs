using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.familiar;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.abilitylist;

public class AbilityList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private UnityAction<AbilityRef> _clickCallback;

	private FamiliarSkillsPanel _familiarSkillsPanel;

	private AsianLanguageFontManager asianLangManager;

	public SimpleDataHelper<AbilityListModel> Data { get; private set; }

	public void InitList(FamiliarSkillsPanel familiarSkillsPanel = null, UnityAction<AbilityRef> clickCallback = null)
	{
		_clickCallback = clickCallback;
		_familiarSkillsPanel = familiarSkillsPanel;
		if (Data == null)
		{
			Data = new SimpleDataHelper<AbilityListModel>(this);
			base.Start();
		}
	}

	public void ClearList()
	{
		if (Data != null && Data.Count > 0)
		{
			Data.RemoveItems(0, Data.Count);
		}
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	protected override MyListItemViewsHolder CreateViewsHolder(int itemIndex)
	{
		MyListItemViewsHolder myListItemViewsHolder = new MyListItemViewsHolder();
		myListItemViewsHolder.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
		return myListItemViewsHolder;
	}

	protected override void UpdateViewsHolder(MyListItemViewsHolder newOrRecycled)
	{
		AbilityListModel model = Data[newOrRecycled.ItemIndex];
		if (_familiarSkillsPanel != null)
		{
			model.clickable = _familiarSkillsPanel.skillsEnabled;
			model.enabled = _familiarSkillsPanel.skillsEnabled;
		}
		newOrRecycled.abilityIcon.overrideSprite = model.abilityRef.GetSpriteIcon();
		int tile = Mathf.RoundToInt((float)model.abilityRef.meterCost / (float)VariableBook.battleMeterMax * 4f);
		newOrRecycled.abilityOverlay.SetTile(tile);
		newOrRecycled.hoverImages.ForceStart();
		newOrRecycled.hoverImages.GetOwnTexts();
		newOrRecycled.root.GetComponent<Button>().enabled = model.clickable;
		newOrRecycled.root.GetComponent<Button>().interactable = model.clickable;
		newOrRecycled.root.GetComponent<Button>().onClick.AddListener(delegate
		{
			OnAbilityClick(model);
		});
		if (model.clickable)
		{
			newOrRecycled.hoverImages.active = true;
		}
		else
		{
			newOrRecycled.hoverImages.OnExit();
			newOrRecycled.hoverImages.active = false;
		}
		if (model.showDesc)
		{
			newOrRecycled.description.gameObject.SetActive(value: true);
			newOrRecycled.bg.gameObject.SetActive(value: true);
			newOrRecycled.description.text = model.abilityRef.getTooltipText(model.power, model.bonus);
		}
		else
		{
			newOrRecycled.description.gameObject.SetActive(value: false);
			newOrRecycled.bg.gameObject.SetActive(value: false);
			newOrRecycled.description.text = "";
		}
		if (model.enabled)
		{
			newOrRecycled.bg.color = new Color(newOrRecycled.bg.color.r, newOrRecycled.bg.color.g, newOrRecycled.bg.color.b, 1f);
			newOrRecycled.description.color = new Color(newOrRecycled.description.color.r, newOrRecycled.description.color.g, newOrRecycled.description.color.b, 1f);
			newOrRecycled.abilityBack.color = new Color(newOrRecycled.abilityBack.color.r, newOrRecycled.abilityBack.color.g, newOrRecycled.abilityBack.color.b, 1f);
			newOrRecycled.abilityIcon.color = new Color(newOrRecycled.abilityIcon.color.r, newOrRecycled.abilityIcon.color.g, newOrRecycled.abilityIcon.color.b, 1f);
			newOrRecycled.loadingIcon.color = new Color(newOrRecycled.loadingIcon.color.r, newOrRecycled.loadingIcon.color.g, newOrRecycled.loadingIcon.color.b, 1f);
			newOrRecycled.abilityOverlay.SetAlpha(1f);
		}
		else
		{
			newOrRecycled.bg.color = new Color(newOrRecycled.bg.color.r, newOrRecycled.bg.color.g, newOrRecycled.bg.color.b, 0.5f);
			newOrRecycled.description.color = new Color(newOrRecycled.description.color.r, newOrRecycled.description.color.g, newOrRecycled.description.color.b, 0.5f);
			newOrRecycled.abilityBack.color = new Color(newOrRecycled.abilityBack.color.r, newOrRecycled.abilityBack.color.g, newOrRecycled.abilityBack.color.b, 0.5f);
			newOrRecycled.abilityIcon.color = new Color(newOrRecycled.abilityIcon.color.r, newOrRecycled.abilityIcon.color.g, newOrRecycled.abilityIcon.color.b, 0.5f);
			newOrRecycled.loadingIcon.color = new Color(newOrRecycled.loadingIcon.color.r, newOrRecycled.loadingIcon.color.g, newOrRecycled.loadingIcon.color.b, 0.5f);
			newOrRecycled.abilityOverlay.SetAlpha(0.5f);
		}
	}

	private void OnAbilityClick(AbilityListModel model)
	{
		if (_clickCallback != null)
		{
			_clickCallback(model.abilityRef);
		}
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.root.GetComponent<Button>().onClick.RemoveAllListeners();
		inRecycleBinOrVisible.root.GetComponent<HoverImages>().OnExit();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<AbilityListModel> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<AbilityListModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		AbilityListModel[] newItems = new AbilityListModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		AbilityListModel[] newItems = new AbilityListModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(AbilityListModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = base.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
	}
}
