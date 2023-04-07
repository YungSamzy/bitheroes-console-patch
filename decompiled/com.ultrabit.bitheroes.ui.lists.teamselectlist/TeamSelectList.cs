using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.team;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.lists.teamselectlist;

public class TeamSelectList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private UnityAction<TeammateData> onSelectTeammate;

	private TeamSelectWindow teamSelectWindow;

	private int highestLevel;

	private int lowestLevel;

	private int highestPower;

	private int lowestPower;

	private int highestStamina;

	private int lowestStamina;

	private int highestAgility;

	private int lowestAgility;

	public SimpleDataHelper<TeamSelectListItem> Data { get; private set; }

	public void SetReferenceStats(int highestLevel, int lowestLevel, int highestPower, int lowestPower, int highestStamina, int lowestStamina, int highestAgility, int lowestAgility)
	{
		this.highestLevel = highestLevel;
		this.lowestLevel = lowestLevel;
		this.highestPower = highestPower;
		this.lowestPower = lowestPower;
		this.highestStamina = highestStamina;
		this.highestStamina = highestStamina;
		this.highestAgility = highestAgility;
		this.lowestAgility = lowestAgility;
	}

	public void InitList(UnityAction<TeammateData> onSelectTeammate, TeamSelectWindow teamSelectWindow)
	{
		this.onSelectTeammate = onSelectTeammate;
		this.teamSelectWindow = teamSelectWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<TeamSelectListItem>(this);
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

	protected override void Start()
	{
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
		TeamSelectListItem model = Data[newOrRecycled.ItemIndex];
		if (newOrRecycled.txtName != null)
		{
			newOrRecycled.txtName.text = model.teammateData.name;
		}
		int power = teamSelectWindow.GetPower(model.teammateData);
		int stamina = teamSelectWindow.GetStamina(model.teammateData);
		int agility = teamSelectWindow.GetAgility(model.teammateData);
		if (newOrRecycled.txtPower != null)
		{
			newOrRecycled.txtPower.text = Util.NumberFormat(power);
		}
		if (newOrRecycled.txtStamina != null)
		{
			newOrRecycled.txtStamina.text = Util.NumberFormat(stamina);
		}
		if (newOrRecycled.txtAgility != null)
		{
			newOrRecycled.txtAgility.text = Util.NumberFormat(agility);
		}
		if (newOrRecycled.txtPower != null)
		{
			newOrRecycled.txtPower.text = Util.colorString(Util.NumberFormat(power), Util.getCurrentColor(highestPower, power, lowestPower));
		}
		if (newOrRecycled.txtStamina != null)
		{
			newOrRecycled.txtStamina.text = Util.colorString(Util.NumberFormat(stamina), Util.getCurrentColor(highestStamina, stamina, lowestStamina));
		}
		if (newOrRecycled.txtAgility != null)
		{
			newOrRecycled.txtAgility.text = Util.colorString(Util.NumberFormat(agility), Util.getCurrentColor(highestAgility, agility, lowestAgility));
		}
		if (newOrRecycled.btnSelect != null)
		{
			newOrRecycled.btnSelect.onClick.AddListener(delegate
			{
				GameData.instance.audioManager.PlaySoundLink("buttonclick");
				onSelectTeammate(model.teammateData);
			});
		}
		if (newOrRecycled.btnSelectText != null)
		{
			newOrRecycled.btnSelectText.text = Language.GetString("ui_add");
		}
		bool flag = model.teammateData.type == 1 && ((CharacterData)model.teammateData.data).isIMXG0;
		newOrRecycled.backgroundImage.color = (flag ? ((CharacterData)model.teammateData.data).nftRarityColor : Color.white);
		newOrRecycled.avatarGenerationBanner.gameObject.SetActive(flag);
		newOrRecycled.avatarBackground.gameObject.SetActive(flag);
		if (flag)
		{
			newOrRecycled.avatarBackground.LoadDetails(((CharacterData)model.teammateData.data).nftBackground, ((CharacterData)model.teammateData.data).nftFrameSimple, ((CharacterData)model.teammateData.data).nftFrameSeparator);
			newOrRecycled.avatarGenerationBanner.LoadDetails(((CharacterData)model.teammateData.data).nftGeneration, ((CharacterData)model.teammateData.data).nftRarity);
		}
		float scale = 2f / teamSelectWindow.panel.transform.localScale.x;
		Transform asset = model.teammateData.getAsset(scale);
		CharacterDisplay characterDisplay = null;
		if (asset != null)
		{
			asset.transform.SetParent(newOrRecycled.placeholderAsset.transform, worldPositionStays: false);
			asset.transform.localPosition = Vector3.zero;
			characterDisplay = asset.GetComponent<CharacterDisplay>();
		}
		if (characterDisplay != null)
		{
			characterDisplay.SetLocalPosition(new Vector3(0f, -50f, 0f));
			characterDisplay.HideMaskedElements();
		}
		else
		{
			Vector3 localPosition = newOrRecycled.placeholderAsset.localPosition;
			localPosition.y = -14f;
			newOrRecycled.placeholderAsset.localPosition = localPosition;
			float x = asset.transform.localScale.x * model.teammateData.selectScale;
			float y = asset.transform.localScale.y * model.teammateData.selectScale;
			asset.transform.localScale = new Vector3(x, y, asset.transform.localScale.z);
			float x2 = model.teammateData.selectOffset.x * asset.localScale.x;
			float y2 = 0f - model.teammateData.selectOffset.y * asset.localScale.y;
			asset.transform.localPosition = new Vector3(x2, y2, 0f);
			asset.transform.localRotation = Quaternion.identity;
		}
		Util.ChangeLayer(asset.transform, "UI");
		SpriteRenderer[] componentsInChildren = asset.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		ParticleSystem[] componentsInChildren2 = asset.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].GetComponent<ParticleSystemRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		if (newOrRecycled.root == null)
		{
			return;
		}
		int num = 2 + newOrRecycled.root.transform.GetSiblingIndex() + teamSelectWindow.sortingLayer;
		newOrRecycled.avatarGenerationBanner.SetSpriteMaskRange(num, num - 1);
		if (newOrRecycled.assetMask0 != null)
		{
			newOrRecycled.assetMask0.frontSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask0.frontSortingOrder = num;
			newOrRecycled.assetMask0.backSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask0.backSortingOrder = num - 1;
		}
		if (newOrRecycled.assetMask1 != null)
		{
			newOrRecycled.assetMask1.frontSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask1.frontSortingOrder = num;
			newOrRecycled.assetMask1.backSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask1.backSortingOrder = num - 1;
		}
		if (newOrRecycled.assetMask2 != null)
		{
			newOrRecycled.assetMask2.frontSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask2.frontSortingOrder = num;
			newOrRecycled.assetMask2.backSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask2.backSortingOrder = num - 1;
		}
		if (newOrRecycled.assetMask3 != null)
		{
			newOrRecycled.assetMask3.frontSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask3.frontSortingOrder = num;
			newOrRecycled.assetMask3.backSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask3.backSortingOrder = num - 1;
		}
		SortingGroup sortingGroup = asset.gameObject.AddComponent<SortingGroup>();
		if (sortingGroup != null && sortingGroup.enabled)
		{
			sortingGroup.sortingLayerName = "UI";
			sortingGroup.sortingOrder = num;
		}
		if (!(newOrRecycled.background != null))
		{
			return;
		}
		newOrRecycled.background.onClick.AddListener(delegate
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			switch (model.teammateData.type)
			{
			case 1:
				GameData.instance.windowGenerator.ShowPlayer(model.teammateData.id);
				break;
			case 2:
				GameData.instance.windowGenerator.ShowFamiliar(model.teammateData.id, mine: true);
				break;
			}
		});
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
		inRecycleBinOrVisible.btnSelect.onClick.RemoveAllListeners();
		if (inRecycleBinOrVisible.placeholderAsset.childCount > 0)
		{
			Object.Destroy(inRecycleBinOrVisible.placeholderAsset.GetChild(0).gameObject);
		}
		inRecycleBinOrVisible.background.onClick.RemoveAllListeners();
	}

	public void AddItemsAt(int index, IList<TeamSelectListItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<TeamSelectListItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		TeamSelectListItem[] newItems = new TeamSelectListItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		TeamSelectListItem[] newItems = new TeamSelectListItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(TeamSelectListItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
