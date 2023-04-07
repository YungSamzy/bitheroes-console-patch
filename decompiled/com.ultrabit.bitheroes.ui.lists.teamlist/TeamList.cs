using System;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.lists.MultiplePrefabsTeamList;
using com.ultrabit.bitheroes.ui.team;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.teamlist;

public class TeamList : OSA<MyParams, MyViewsHolder>
{
	private UnityAction<int> onAddButtonClicked;

	private UnityAction onRemoveButtonClicked;

	private TeamWindow teamWindow;

	private int highestLevel;

	private int lowestLevel;

	private int highestPower;

	private int lowestPower;

	private int highestStamina;

	private int lowestStamina;

	private int highestAgility;

	private int lowestAgility;

	private bool _showArmoryButton;

	private AsianLanguageFontManager asianLangManager;

	public SimpleDataHelper<TeamListItemModel> Data { get; private set; }

	public void SetReferenceStats(int highestLevel, int lowestLevel, int highestPower, int lowestPower, int highestStamina, int lowestStamina, int highestAgility, int lowestAgility)
	{
		this.highestLevel = highestLevel;
		this.lowestLevel = lowestLevel;
		this.highestPower = highestPower;
		this.lowestPower = lowestPower;
		this.highestStamina = highestStamina;
		this.lowestStamina = lowestStamina;
		this.highestAgility = highestAgility;
		this.lowestAgility = lowestAgility;
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

	public void InitList(UnityAction onRemoveButtonClicked, UnityAction<int> onAddButtonClicked, TeamWindow teamWindow, bool showArmoryButton = false)
	{
		this.onAddButtonClicked = onAddButtonClicked;
		this.onRemoveButtonClicked = onRemoveButtonClicked;
		this.teamWindow = teamWindow;
		_showArmoryButton = showArmoryButton;
		if (Data == null)
		{
			Data = new SimpleDataHelper<TeamListItemModel>(this);
			base.Start();
		}
	}

	public List<TeammateData> GetTeammateDataList()
	{
		List<TeammateData> list = new List<TeammateData>();
		for (int i = 0; i < Data.Count; i++)
		{
			list.Add(Data[i].teammateData);
		}
		return list;
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

	private void SwapUp(RectTransform item)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		MyViewsHolder itemViewsHolderIfVisible = GetItemViewsHolderIfVisible(item);
		if (itemViewsHolderIfVisible != null && itemViewsHolderIfVisible.ItemIndex > 0)
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex - 1);
			for (int i = 0; i < _VisibleItemsCount; i++)
			{
				UpdateViewsHolder(_VisibleItems[i]);
			}
		}
	}

	private void SwapDown(RectTransform item)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		MyViewsHolder itemViewsHolderIfVisible = GetItemViewsHolderIfVisible(item);
		if (itemViewsHolderIfVisible != null && itemViewsHolderIfVisible.ItemIndex < Data.Count - 1)
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex + 1);
			for (int i = 0; i < _VisibleItemsCount; i++)
			{
				UpdateViewsHolder(_VisibleItems[i]);
			}
		}
	}

	private void RetrieveDataAndUpdate(int count)
	{
	}

	private void OnDataRetrieved(TeamListItemModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}

	protected override void CollectItemsSizes(ItemCountChangeMode changeMode, int count, int indexIfInsertingOrRemoving, ItemsDescriptor itemsDesc)
	{
		base.CollectItemsSizes(changeMode, count, indexIfInsertingOrRemoving, itemsDesc);
		if (changeMode == ItemCountChangeMode.RESET && count != 0)
		{
			int num = 0;
			int num2 = num + count;
			itemsDesc.BeginChangingItemsSizes(num);
			for (int i = num; i < num2; i++)
			{
				itemsDesc[i] = UnityEngine.Random.Range(_Params.DefaultItemSize / 3f, _Params.DefaultItemSize * 3f);
			}
			itemsDesc.EndChangingItemsSizes();
		}
	}

	protected override MyViewsHolder CreateViewsHolder(int itemIndex)
	{
		MyViewsHolder myViewsHolder = new MyViewsHolder();
		myViewsHolder.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
		return myViewsHolder;
	}

	public void UpdateListItem(MyViewsHolder item)
	{
		UpdateViewsHolder(item);
	}

	protected override void UpdateViewsHolder(MyViewsHolder newOrRecycled)
	{
		TeamListItemModel teamListItemModel = Data[newOrRecycled.ItemIndex];
		newOrRecycled.btnTxt.text = Language.GetString("ui_add");
		newOrRecycled.removeBtnTxt.text = Language.GetString("ui_x");
		newOrRecycled.armoryTxt.text = Language.GetString("ui_armory");
		bool empty = teamListItemModel == null || teamListItemModel.teammateData == null;
		SetSlot(teamListItemModel, newOrRecycled, empty);
	}

	private void SetSlot(TeamListItemModel model, MyViewsHolder newOrRecycled, bool empty)
	{
		newOrRecycled.addBtn.gameObject.SetActive(empty);
		newOrRecycled.numberTxt.gameObject.SetActive(empty);
		newOrRecycled.nameTxt.gameObject.SetActive(!empty);
		newOrRecycled.powerTxt.gameObject.SetActive(!empty);
		newOrRecycled.staminaTxt.gameObject.SetActive(!empty);
		newOrRecycled.agilityTxt.gameObject.SetActive(!empty);
		newOrRecycled.upBtn.gameObject.SetActive(!empty);
		newOrRecycled.downBtn.gameObject.SetActive(!empty);
		newOrRecycled.removeBtn.gameObject.SetActive(!empty);
		newOrRecycled.armoryBtn.gameObject.SetActive(!empty);
		newOrRecycled.placeholderAsset.gameObject.SetActive(!empty);
		newOrRecycled.assetMask0.gameObject.SetActive(!empty);
		newOrRecycled.assetMask1.gameObject.SetActive(!empty);
		newOrRecycled.assetMask2.gameObject.SetActive(!empty);
		newOrRecycled.assetMask3.gameObject.SetActive(!empty);
		newOrRecycled.addBtn.GetComponent<Button>().onClick.RemoveAllListeners();
		newOrRecycled.armoryBtn.GetComponent<Button>().onClick.RemoveAllListeners();
		Button button = newOrRecycled.background.GetComponent<Button>();
		if (button == null)
		{
			button = newOrRecycled.background.gameObject.AddComponent<Button>();
		}
		button.onClick.RemoveAllListeners();
		if (empty)
		{
			newOrRecycled.background.color = Color.white;
			newOrRecycled.avatarGenerationBanner.gameObject.SetActive(value: false);
			newOrRecycled.avatarBackground.gameObject.SetActive(value: false);
			newOrRecycled.numberTxt.text = (newOrRecycled.ItemIndex + 1).ToString();
			newOrRecycled.addBtn.GetComponent<Button>().onClick.AddListener(delegate
			{
				GameData.instance.audioManager.PlaySoundLink("buttonclick");
				onAddButtonClicked(newOrRecycled.ItemIndex);
			});
			button.onClick.AddListener(delegate
			{
				GameData.instance.audioManager.PlaySoundLink("buttonclick");
				onAddButtonClicked(newOrRecycled.ItemIndex);
			});
			UpdateSlotsStats();
		}
		else
		{
			bool flag = model.teammateData.type == 1;
			Util.SetButton(newOrRecycled.upBtn.GetComponent<Button>(), newOrRecycled.ItemIndex != 0);
			Util.SetButton(newOrRecycled.downBtn.GetComponent<Button>(), newOrRecycled.ItemIndex != teamWindow.teamRules.size - 1);
			if (model.teammateData.id == GameData.instance.PROJECT.character.id && flag)
			{
				Util.SetButton(newOrRecycled.removeBtn.GetComponent<Button>(), enabled: false);
			}
			else
			{
				Util.SetButton(newOrRecycled.removeBtn.GetComponent<Button>());
				newOrRecycled.removeBtn.GetComponent<Button>().onClick.AddListener(delegate
				{
					GameData.instance.audioManager.PlaySoundLink("buttonclick");
					Data[newOrRecycled.ItemIndex].teammateData = null;
					onRemoveButtonClicked();
					SetSlot(null, newOrRecycled, empty: true);
				});
			}
			if (flag && _showArmoryButton)
			{
				if (model.teammateData.level >= GameData.instance.PROJECT.armoryMinLevelRequired)
				{
					newOrRecycled.armoryBtn.GetComponent<Button>().onClick.AddListener(delegate
					{
						GameData.instance.audioManager.PlaySoundLink("buttonclick");
						CharacterData charData = model.teammateData.data as CharacterData;
						if (GetArmoryCountByCharData(charData) > 0)
						{
							GameData.instance.windowGenerator.NewTeammateArmoryWindow(charData).ARMORY_TEAMMATE_SELECT.AddListener(delegate(object id)
							{
								OnArmoryTeamSelect(id, model, newOrRecycled);
							});
						}
						else
						{
							GameData.instance.windowGenerator.NewMessageWindow(DialogWindow.TYPE.TYPE_OK, Language.GetString("ui_message"), Language.GetString("ui_armory_not_set"));
						}
					});
				}
				else
				{
					Util.SetButton(newOrRecycled.armoryBtn.GetComponent<Button>(), enabled: false);
				}
			}
			else
			{
				newOrRecycled.armoryBtn.gameObject.SetActive(value: false);
			}
			if (newOrRecycled.placeholderAsset.childCount > 0)
			{
				for (int i = 0; i < newOrRecycled.placeholderAsset.childCount; i++)
				{
					UnityEngine.Object.Destroy(newOrRecycled.placeholderAsset.GetChild(i).gameObject);
				}
			}
			bool flag2 = flag && ((CharacterData)model.teammateData.data).isIMXG0;
			newOrRecycled.background.color = (flag2 ? ((CharacterData)model.teammateData.data).nftRarityColor : Color.white);
			newOrRecycled.avatarGenerationBanner.gameObject.SetActive(flag2);
			newOrRecycled.avatarBackground.gameObject.SetActive(flag2);
			if (flag2)
			{
				newOrRecycled.avatarBackground.LoadDetails(((CharacterData)model.teammateData.data).nftBackground, ((CharacterData)model.teammateData.data).nftFrameSimple, ((CharacterData)model.teammateData.data).nftFrameSeparator);
				newOrRecycled.avatarGenerationBanner.LoadDetails(((CharacterData)model.teammateData.data).nftGeneration, ((CharacterData)model.teammateData.data).nftRarity);
			}
			float scale = 2f / teamWindow.panel.transform.localScale.x;
			Transform asset = model.teammateData.getAsset(scale);
			if (asset != null)
			{
				asset.transform.SetParent(newOrRecycled.placeholderAsset.transform, worldPositionStays: false);
				CharacterDisplay component = asset.GetComponent<CharacterDisplay>();
				if (component != null)
				{
					Vector3 localPosition = newOrRecycled.placeholderAsset.localPosition;
					localPosition.y = 0f;
					newOrRecycled.placeholderAsset.localPosition = localPosition;
					component.SetLocalPosition(new Vector3(0f, -63f, 0f));
					component.HideMaskedElements();
				}
				else
				{
					Vector3 localPosition2 = newOrRecycled.placeholderAsset.localPosition;
					localPosition2.y = -14f;
					newOrRecycled.placeholderAsset.localPosition = localPosition2;
					float x = asset.transform.localScale.x * model.teammateData.selectScale;
					float y = asset.transform.localScale.y * model.teammateData.selectScale;
					asset.transform.localScale = new Vector3(x, y, asset.transform.localScale.z);
					float x2 = model.teammateData.selectOffset.x * asset.localScale.x;
					float y2 = 0f - model.teammateData.selectOffset.y * asset.localScale.y;
					asset.transform.localPosition = new Vector3(x2, y2, 0f);
					asset.transform.localRotation = Quaternion.identity;
				}
			}
			newOrRecycled.nameTxt.text = model.teammateData.name;
			int power = teamWindow.GetPower(model.teammateData);
			int stamina = teamWindow.GetStamina(model.teammateData);
			int agility = teamWindow.GetAgility(model.teammateData);
			newOrRecycled.powerTxt.text = Util.colorString(Util.NumberFormat(power), Util.getCurrentColor(highestPower, power, lowestPower));
			newOrRecycled.staminaTxt.text = Util.colorString(Util.NumberFormat(stamina), Util.getCurrentColor(highestStamina, stamina, lowestStamina));
			newOrRecycled.agilityTxt.text = Util.colorString(Util.NumberFormat(agility), Util.getCurrentColor(highestAgility, agility, lowestAgility));
			if (asset != null)
			{
				Util.ChangeLayer(asset.transform, "UI");
				SpriteRenderer[] componentsInChildren = asset.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
				}
				ParticleSystem[] componentsInChildren2 = asset.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					componentsInChildren2[j].GetComponent<ParticleSystemRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
				}
			}
			int num = 2 + newOrRecycled.root.transform.GetSiblingIndex() + teamWindow.sortingLayer;
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
			if (asset != null)
			{
				SortingGroup sortingGroup = asset.gameObject.AddComponent<SortingGroup>();
				if (sortingGroup != null && sortingGroup.enabled)
				{
					sortingGroup.sortingLayerName = "UI";
					sortingGroup.sortingOrder = num;
				}
			}
			button.onClick.AddListener(delegate
			{
				GameData.instance.audioManager.PlaySoundLink("buttonclick");
				switch (model.teammateData.type)
				{
				case 1:
					GameData.instance.windowGenerator.ShowTeammate(model.teammateData);
					break;
				case 2:
					GameData.instance.windowGenerator.ShowFamiliar(model.teammateData.id, mine: true);
					break;
				}
			});
		}
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

	private void UpdateSlotsStats()
	{
		int num2;
		int num;
		int num3 = (num2 = (num = int.MaxValue));
		int num5;
		int num4;
		int num6 = (num5 = (num4 = int.MinValue));
		foreach (TeamListItemModel item in (IEnumerable<TeamListItemModel>)Data)
		{
			if (item != null && item.teammateData != null)
			{
				if (item.teammateData.power > num6)
				{
					num6 = item.teammateData.power;
				}
				if (item.teammateData.power < num3)
				{
					num3 = item.teammateData.power;
				}
				if (item.teammateData.stamina > num5)
				{
					num5 = item.teammateData.stamina;
				}
				if (item.teammateData.power < num2)
				{
					num2 = item.teammateData.stamina;
				}
				if (item.teammateData.agility > num4)
				{
					num4 = item.teammateData.agility;
				}
				if (item.teammateData.power < num)
				{
					num = item.teammateData.agility;
				}
			}
		}
		foreach (MyViewsHolder visibleItem in _VisibleItems)
		{
			if (visibleItem == null)
			{
				continue;
			}
			int itemIndex = visibleItem.ItemIndex;
			TeamListItemModel teamListItemModel = Data[itemIndex];
			if (teamListItemModel != null && teamListItemModel.teammateData != null && visibleItem.removeBtn.gameObject.activeInHierarchy && teamListItemModel != null && teamListItemModel.teammateData != null)
			{
				int power = teamListItemModel.teammateData.power;
				int stamina = teamListItemModel.teammateData.stamina;
				int agility = teamListItemModel.teammateData.agility;
				if (visibleItem.powerTxt != null)
				{
					visibleItem.powerTxt.text = Util.colorString(Util.NumberFormat(power), Util.getCurrentColor(num6, power, num3));
				}
				if (visibleItem.staminaTxt != null)
				{
					visibleItem.staminaTxt.text = Util.colorString(Util.NumberFormat(stamina), Util.getCurrentColor(num5, stamina, num2));
				}
				if (visibleItem.agilityTxt != null)
				{
					visibleItem.agilityTxt.text = Util.colorString(Util.NumberFormat(agility), Util.getCurrentColor(num4, agility, num));
				}
			}
		}
	}

	private int GetArmoryCountByCharData(CharacterData charData)
	{
		int num = 0;
		foreach (ArmoryEquipment armoryEquipmentSlot in charData.armory.armoryEquipmentSlots)
		{
			if (armoryEquipmentSlot.unlocked)
			{
				if (charData.charID == GameData.instance.PROJECT.character.id)
				{
					num++;
				}
				else if (!armoryEquipmentSlot.pprivate)
				{
					num++;
				}
			}
		}
		return num;
	}

	private void OnArmoryTeamSelect(object id, TeamListItemModel model, MyViewsHolder newOrRecycled)
	{
		int num = Convert.ToInt32(id);
		TeammateData teammateData = model.teammateData;
		teammateData.armoryID = num;
		if (teammateData.id == GameData.instance.PROJECT.character.id)
		{
			GameData.instance.PROJECT.character.armory.battleArmorySelected = true;
		}
		SetSlot(model, newOrRecycled, empty: false);
	}

	public void RemoveModel(int index)
	{
		Data.List[index] = null;
		for (int i = 0; i < _VisibleItemsCount; i++)
		{
			UpdateViewsHolder(_VisibleItems[i]);
		}
	}
}
