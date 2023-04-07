using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.avatar;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceCharacterTile : MainUIButton
{
	public Image expBar;

	public Image upgradeIcon;

	public Image statsIcon;

	public TextMeshProUGUI levelTxt;

	public GameObject placeholderAsset;

	public RectTransform baseBackground;

	public AvatarBackground avatarBackground;

	public AvatarGenerationBanner avatarGenerationBanner;

	[SerializeField]
	private HoverImages hoverImages;

	private RegularBarFill regularBarFill;

	private CharacterDisplay _display;

	private CharacterData _characterData;

	public SpriteMask[] masks;

	public override void Create()
	{
		LoadDetails(Language.GetString("ui_player"));
		GameData.instance.PROJECT.character.AddListener("EXP_CHANGE", OnExpChange);
		GameData.instance.PROJECT.character.AddListener("LEVEL_CHANGE", OnLevelChange);
		GameData.instance.PROJECT.character.AddListener("APPEARANCE_CHANGE", OnAppearanceChange);
		GameData.instance.PROJECT.character.AddListener("POINTS_CHANGE", OnStatsChange);
		GameData.instance.PROJECT.character.AddListener("STATS_CHANGE", OnStatsChange);
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnItemChange);
		GameData.instance.PROJECT.character.equipment.OnChange.AddListener(OnEquipmentChange);
		regularBarFill = expBar.gameObject.GetComponent<RegularBarFill>();
		if (regularBarFill == null)
		{
			regularBarFill = expBar.gameObject.AddComponent<RegularBarFill>();
		}
		regularBarFill.Init();
		DoUpdate();
		OnExpChange();
		OnLevelChange();
	}

	private void OnExpChange()
	{
		long levelExp = Character.getLevelExp(GameData.instance.PROJECT.character.level);
		long num = GameData.instance.PROJECT.character.exp - levelExp;
		long num2 = Character.getLevelExp(GameData.instance.PROJECT.character.level + 1) - levelExp;
		regularBarFill.UpdateBar(num, num2);
	}

	private void OnLevelChange()
	{
		levelTxt.text = Util.NumberFormat(GameData.instance.PROJECT.character.level);
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.windowGenerator.NewCharacterWindow();
	}

	private void OnAppearanceChange()
	{
		DoUpdate();
	}

	private void OnStatsChange()
	{
		DoUpdate();
	}

	private void OnItemChange()
	{
		DoUpdate();
	}

	private void OnEquipmentChange()
	{
		DoUpdate();
	}

	public override void DoUpdate()
	{
		statsIcon.gameObject.SetActive(GameData.instance.PROJECT.character.points > 0);
		upgradeIcon.gameObject.SetActive(GameData.instance.PROJECT.character.GetUpgradeEquipment() != null);
		if (_display != null)
		{
			Object.Destroy(_display.gameObject);
		}
		_characterData = GameData.instance.PROJECT.character.toCharacterData();
		baseBackground.gameObject.SetActive(!_characterData.isIMXG0);
		avatarBackground.gameObject.SetActive(_characterData.isIMXG0);
		avatarGenerationBanner.gameObject.SetActive(_characterData.isIMXG0);
		if (_characterData.isIMXG0)
		{
			avatarBackground.LoadDetails(_characterData.nftBackground, _characterData.nftFrameMenuInterface);
			avatarGenerationBanner.LoadDetails(_characterData.nftGeneration, _characterData.nftRarity);
		}
		_display = GameData.instance.PROJECT.character.toCharacterDisplay(1.5f, displayMount: false, enableLoading: false);
		_display.transform.SetParent(placeholderAsset.transform, worldPositionStays: false);
		_display.SetLocalPosition(new Vector3(0f, -45f, 0f));
		_display.HideMaskedElements();
		Util.ChangeLayer(_display.transform, "UI");
		SortingGroup sortingGroup = _display.gameObject.AddComponent<SortingGroup>();
		if (sortingGroup != null && sortingGroup.enabled)
		{
			sortingGroup.sortingLayerName = "UI";
			sortingGroup.sortingOrder = 1012;
		}
		SpriteMask[] array = masks;
		foreach (SpriteMask spriteMask in array)
		{
			if (sortingGroup != null && sortingGroup.enabled)
			{
				int backSortingLayerID = (spriteMask.frontSortingLayerID = sortingGroup.sortingLayerID);
				spriteMask.backSortingLayerID = backSortingLayerID;
				spriteMask.frontSortingOrder = sortingGroup.sortingOrder;
				spriteMask.backSortingOrder = sortingGroup.sortingOrder - 1;
			}
		}
		_display.characterPuppet.StopAllAnimations();
		SpriteRenderer[] componentsInChildren = _display.gameObject.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		hoverImages.ForceStart();
	}

	public new void OnDestroy()
	{
		if (GameData.instance.PROJECT.character != null)
		{
			GameData.instance.PROJECT.character.RemoveListener("EXP_CHANGE", OnExpChange);
			GameData.instance.PROJECT.character.RemoveListener("LEVEL_CHANGE", OnLevelChange);
			GameData.instance.PROJECT.character.RemoveListener("APPEARANCE_CHANGE", OnAppearanceChange);
			GameData.instance.PROJECT.character.RemoveListener("POINTS_CHANGE", OnStatsChange);
			GameData.instance.PROJECT.character.RemoveListener("STATS_CHANGE", OnStatsChange);
			GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnItemChange);
			GameData.instance.PROJECT.character.equipment.OnChange.RemoveListener(OnEquipmentChange);
		}
	}
}
