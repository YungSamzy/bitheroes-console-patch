using System;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui;
using com.ultrabit.bitheroes.ui.avatar;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.lists.MultiplePrefabsTeamList;
using com.ultrabit.bitheroes.ui.team;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;

public class TeamTileVH : BaseVH
{
	private TeamListItemModel _model;

	private RectTransform scalableViews;

	private RectTransform teamObject;

	private RectTransform addBtn;

	private RectTransform armoryBtn;

	private TextMeshProUGUI btnTxt;

	private TextMeshProUGUI armoryTxt;

	private TextMeshProUGUI numberTxt;

	private TextMeshProUGUI nameTxt;

	private TextMeshProUGUI powerTxt;

	private TextMeshProUGUI staminaTxt;

	private TextMeshProUGUI agilityTxt;

	private RectTransform upBtn;

	private RectTransform downBtn;

	private RectTransform removeBtn;

	private TextMeshProUGUI removeBtnTxt;

	private Image background;

	public AvatarBackground avatarBackground;

	public AvatarGenerationBanner avatarGenerationBanner;

	private RectTransform placeholderAsset;

	private SpriteMask assetMask0;

	private SpriteMask assetMask1;

	private SpriteMask assetMask2;

	private SpriteMask assetMask3;

	private TeamWindow teamWindow;

	private static int _highestLevel;

	private static int _lowestLevel;

	private static int _highestPower;

	private static int _lowestPower;

	private static int _highestStamina;

	private static int _lowestStamina;

	private static int _highestAgility;

	private static int _lowestAgility;

	public static void SetReferenceStats(int highestLevel, int lowestLevel, int highestPower, int lowestPower, int highestStamina, int lowestStamina, int highestAgility, int lowestAgility)
	{
		_highestLevel = highestLevel;
		_lowestLevel = lowestLevel;
		_highestPower = highestPower;
		_lowestPower = lowestPower;
		_highestStamina = highestStamina;
		_highestStamina = highestStamina;
		_highestAgility = highestAgility;
		_lowestAgility = lowestAgility;
	}

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<RectTransform>("ScalableViews", out scalableViews);
		scalableViews.GetComponentAtPath<Image>("Background", out background);
		scalableViews.GetComponentAtPath<AvatarBackground>("AvatarBackground", out avatarBackground);
		scalableViews.GetComponentAtPath<AvatarGenerationBanner>("AvatarGenerationBanner", out avatarGenerationBanner);
		scalableViews.GetComponentAtPath<RectTransform>("TeamObject", out teamObject);
		scalableViews.GetComponentAtPath<RectTransform>("AddBtn", out addBtn);
		addBtn.GetComponentAtPath<TextMeshProUGUI>("BtnTxt", out btnTxt);
		btnTxt.text = Language.GetString("ui_add");
		scalableViews.GetComponentAtPath<TextMeshProUGUI>("NumberTxt", out numberTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out nameTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("PowerTxt", out powerTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("StaminaTxt", out staminaTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("AgilityTxt", out agilityTxt);
		teamObject.GetComponentAtPath<RectTransform>("UpBtn", out upBtn);
		teamObject.GetComponentAtPath<RectTransform>("DownBtn", out downBtn);
		teamObject.GetComponentAtPath<RectTransform>("RemoveBtn", out removeBtn);
		teamObject.GetComponentAtPath<RectTransform>("ArmoryBtn", out armoryBtn);
		armoryBtn.GetComponentAtPath<TextMeshProUGUI>("BtnTxt", out armoryTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("RemoveBtn/BtnTxt", out removeBtnTxt);
		teamObject.GetComponentAtPath<RectTransform>("PlaceholderAsset", out placeholderAsset);
		teamObject.GetComponentAtPath<SpriteMask>("AssetMask0", out assetMask0);
		teamObject.GetComponentAtPath<SpriteMask>("AssetMask1", out assetMask1);
		teamObject.GetComponentAtPath<SpriteMask>("AssetMask2", out assetMask2);
		teamObject.GetComponentAtPath<SpriteMask>("AssetMask3", out assetMask3);
		teamWindow = root.GetComponentInParent<TeamWindow>();
	}

	public override bool CanPresentModelType(Type modelType)
	{
		return modelType == typeof(TeamListItemModel);
	}

	public override void UpdateViews(BaseModel model)
	{
		base.UpdateViews(model);
		_model = model as TeamListItemModel;
		btnTxt.text = Language.GetString("ui_add");
		removeBtnTxt.text = Language.GetString("ui_x");
		armoryTxt.text = Language.GetString("ui_armory");
		bool empty = model == null || (model as TeamListItemModel).teammateData == null;
		SetSlot(model as TeamListItemModel, empty);
	}

	private void SetSlot(TeamListItemModel model, bool empty)
	{
		addBtn.gameObject.SetActive(empty);
		numberTxt.gameObject.SetActive(empty);
		nameTxt.gameObject.SetActive(!empty);
		powerTxt.gameObject.SetActive(!empty);
		staminaTxt.gameObject.SetActive(!empty);
		agilityTxt.gameObject.SetActive(!empty);
		upBtn.gameObject.SetActive(!empty);
		downBtn.gameObject.SetActive(!empty);
		removeBtn.gameObject.SetActive(!empty);
		armoryBtn.gameObject.SetActive(!empty);
		placeholderAsset.gameObject.SetActive(!empty);
		assetMask0.gameObject.SetActive(!empty);
		assetMask1.gameObject.SetActive(!empty);
		assetMask2.gameObject.SetActive(!empty);
		assetMask3.gameObject.SetActive(!empty);
		addBtn.GetComponent<Button>().onClick.RemoveAllListeners();
		armoryBtn.GetComponent<Button>().onClick.RemoveAllListeners();
		Button button = background.GetComponent<Button>();
		if (button == null)
		{
			button = background.gameObject.AddComponent<Button>();
		}
		button.onClick.RemoveAllListeners();
		if (empty)
		{
			background.color = Color.white;
			avatarGenerationBanner.gameObject.SetActive(value: false);
			avatarBackground.gameObject.SetActive(value: false);
			numberTxt.text = (_model.slot + 1).ToString();
			addBtn.GetComponent<Button>().onClick.AddListener(delegate
			{
				GameData.instance.audioManager.PlaySoundLink("buttonclick");
				_model.onAddButtonClicked(ItemIndex);
			});
			button.GetComponent<Button>().onClick.AddListener(delegate
			{
				GameData.instance.audioManager.PlaySoundLink("buttonclick");
				_model.onAddButtonClicked(ItemIndex);
			});
			return;
		}
		bool flag = model.teammateData.type == 1;
		int num = teamWindow.teamRules.size + teamWindow.teamRules.slots;
		com.ultrabit.bitheroes.model.utility.Util.SetButton(upBtn.GetComponent<Button>(), ItemIndex != 0);
		com.ultrabit.bitheroes.model.utility.Util.SetButton(downBtn.GetComponent<Button>(), ItemIndex != num);
		if (model.teammateData.id == GameData.instance.PROJECT.character.id && flag)
		{
			com.ultrabit.bitheroes.model.utility.Util.SetButton(removeBtn.GetComponent<Button>(), enabled: false);
		}
		else
		{
			com.ultrabit.bitheroes.model.utility.Util.SetButton(removeBtn.GetComponent<Button>());
			removeBtn.GetComponent<Button>().onClick.AddListener(delegate
			{
				GameData.instance.audioManager.PlaySoundLink("buttonclick");
				_model.teammateData = null;
				_model.onRemoveButtonClicked();
				SetSlot(null, empty: true);
			});
		}
		if (flag)
		{
			armoryBtn.GetComponent<Button>().onClick.AddListener(delegate
			{
				GameData.instance.audioManager.PlaySoundLink("buttonclick");
				CharacterData charData = model.teammateData.data as CharacterData;
				if (GetArmoryCountByCharData(charData) > 0)
				{
					GameData.instance.windowGenerator.NewTeammateArmoryWindow(charData).ARMORY_TEAMMATE_SELECT.AddListener(delegate(object id)
					{
						OnArmoryTeamSelect(id, model);
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
			com.ultrabit.bitheroes.model.utility.Util.SetButton(armoryBtn.GetComponent<Button>(), enabled: false);
		}
		nameTxt.text = model.teammateData.name;
		_ = model.id;
		_ = GameData.instance.PROJECT.character.id;
		bool flag2 = flag && ((CharacterData)model.teammateData.data).isIMXG0;
		background.color = (flag2 ? ((CharacterData)model.teammateData.data).nftRarityColor : Color.white);
		avatarGenerationBanner.gameObject.SetActive(flag2);
		avatarBackground.gameObject.SetActive(flag2);
		if (flag2)
		{
			avatarBackground.LoadDetails(((CharacterData)model.teammateData.data).nftBackground, ((CharacterData)model.teammateData.data).nftFrameSimple, ((CharacterData)model.teammateData.data).nftFrameSeparator);
			avatarGenerationBanner.LoadDetails(((CharacterData)model.teammateData.data).nftGeneration, ((CharacterData)model.teammateData.data).nftRarity);
		}
		int power = model.teamWindow.GetPower(model.teammateData);
		int stamina = model.teamWindow.GetStamina(model.teammateData);
		int agility = model.teamWindow.GetAgility(model.teammateData);
		powerTxt.text = com.ultrabit.bitheroes.model.utility.Util.colorString(com.ultrabit.bitheroes.model.utility.Util.NumberFormat(power), com.ultrabit.bitheroes.model.utility.Util.getCurrentColor(_highestPower, power, _lowestPower));
		staminaTxt.text = com.ultrabit.bitheroes.model.utility.Util.colorString(com.ultrabit.bitheroes.model.utility.Util.NumberFormat(stamina), com.ultrabit.bitheroes.model.utility.Util.getCurrentColor(_highestStamina, stamina, _lowestStamina));
		agilityTxt.text = com.ultrabit.bitheroes.model.utility.Util.colorString(com.ultrabit.bitheroes.model.utility.Util.NumberFormat(agility), com.ultrabit.bitheroes.model.utility.Util.getCurrentColor(_highestAgility, agility, _lowestAgility));
		if (placeholderAsset.childCount > 0)
		{
			for (int i = 0; i < placeholderAsset.childCount; i++)
			{
				teamWindow.DestroyAnything(placeholderAsset.GetChild(i).gameObject);
			}
		}
		Transform asset = model.teammateData.getAsset(2f / teamWindow.panel.transform.localScale.x);
		asset.transform.SetParent(placeholderAsset.transform, worldPositionStays: false);
		CharacterDisplay component = asset.GetComponent<CharacterDisplay>();
		if (component != null)
		{
			component.SetLocalPosition(new Vector3(0f, -48f, 0f));
			component.HideMaskedElements();
		}
		else
		{
			float x = model.teammateData.selectOffset.x * asset.localScale.x;
			float y = 0f - model.teammateData.selectOffset.y * asset.localScale.y;
			asset.transform.localPosition += new Vector3(x, y, 0f);
			asset.transform.localScale = new Vector3(asset.transform.localScale.x * model.teammateData.selectScale, asset.transform.localScale.y * model.teammateData.selectScale, asset.transform.localScale.z);
		}
		com.ultrabit.bitheroes.model.utility.Util.ChangeLayer(asset.transform, "UI");
		SpriteRenderer[] componentsInChildren = asset.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		int num2 = 2 + root.transform.GetSiblingIndex() + teamWindow.sortingLayer;
		avatarGenerationBanner.SetSpriteMaskRange(num2, num2 - 1);
		if (assetMask0 != null)
		{
			assetMask0.frontSortingLayerID = SortingLayer.NameToID("UI");
			assetMask0.frontSortingOrder = num2;
			assetMask0.backSortingLayerID = SortingLayer.NameToID("UI");
			assetMask0.backSortingOrder = num2 - 1;
		}
		if (assetMask1 != null)
		{
			assetMask1.frontSortingLayerID = SortingLayer.NameToID("UI");
			assetMask1.frontSortingOrder = num2;
			assetMask1.backSortingLayerID = SortingLayer.NameToID("UI");
			assetMask1.backSortingOrder = num2 - 1;
		}
		if (assetMask2 != null)
		{
			assetMask2.frontSortingLayerID = SortingLayer.NameToID("UI");
			assetMask2.frontSortingOrder = num2;
			assetMask2.backSortingLayerID = SortingLayer.NameToID("UI");
			assetMask2.backSortingOrder = num2 - 1;
		}
		if (assetMask3 != null)
		{
			assetMask3.frontSortingLayerID = SortingLayer.NameToID("UI");
			assetMask3.frontSortingOrder = num2;
			assetMask3.backSortingLayerID = SortingLayer.NameToID("UI");
			assetMask3.backSortingOrder = num2 - 1;
		}
		SortingGroup sortingGroup = asset.gameObject.AddComponent<SortingGroup>();
		if (sortingGroup != null && sortingGroup.enabled)
		{
			sortingGroup.sortingLayerName = "UI";
			sortingGroup.sortingOrder = num2;
		}
		button.GetComponent<Button>().onClick.AddListener(delegate
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			switch (_model.teammateData.type)
			{
			case 1:
				GameData.instance.windowGenerator.ShowTeammate(_model.teammateData);
				break;
			case 2:
				GameData.instance.windowGenerator.ShowFamiliar(_model.teammateData.id, mine: true);
				break;
			}
		});
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

	private void OnArmoryTeamSelect(object id, TeamListItemModel model)
	{
		int num = Convert.ToInt32(id);
		model.teammateData.armoryID = num;
		SetSlot(model, empty: false);
	}
}
