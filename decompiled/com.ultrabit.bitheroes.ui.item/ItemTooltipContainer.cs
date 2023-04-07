using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.bait;
using com.ultrabit.bitheroes.model.boober;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.item.action;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemTooltipContainer : MonoBehaviour
{
	public TextMeshProUGUI nameTxt;

	public TextMeshProUGUI rarityTxt;

	public TextMeshProUGUI typeTxt;

	public TextMeshProUGUI bindTypeTxt;

	public TextMeshProUGUI tierTxt;

	public TextMeshProUGUI descTxt;

	public TextMeshProUGUI equippedTxt;

	public TextMeshProUGUI tooltipTxt;

	public Image nameContainer;

	public Image itemDataContainer;

	public Button selectBtn;

	public Image equippedBG;

	public Image nameBG;

	public Image contentBG;

	public Image containerBG;

	public Sprite containerEquippedBGSprite;

	public Sprite containerBGSprite;

	public Image placeholderIcon;

	public Image placeholderStats;

	public Image icon;

	public static string BUTTON_CHANGE_SELECTED = "buttonChangeSelected";

	public ItemIcon itemIcon;

	public ItemIcon reference;

	public ItemTooltipStatTile[] itemToolTipStats;

	private BaseModelData itemData;

	private ItemData cosmetic;

	private ItemActionBase itemActionBase;

	private int _actionBaseInt;

	private CharacterData _characterData;

	private ArmoryEquipment _armoryEquipment;

	private ItemTooltipContainer _equippedContainer;

	private bool _flipped;

	private AsianLanguageFontManager asianLangManager;

	private Color REGURAL_BG = new Color(0.4f, 0.4f, 0.4f);

	private Color NFT_BG = new Color(0.88f, 0.48f, 0.98f);

	private Messenger _dispatcher;

	public Messenger dispatcher => _dispatcher;

	public void OnDisable()
	{
		if (AppInfo.IsMobile())
		{
			EventTrigger component = base.gameObject.GetComponent<EventTrigger>();
			if (component != null)
			{
				component.triggers.Clear();
				Object.Destroy(component);
			}
		}
	}

	public void PrepareForRecycle()
	{
		for (int i = 0; i < itemToolTipStats.Length; i++)
		{
			itemToolTipStats[i].gameObject.SetActive(value: true);
		}
		nameTxt.gameObject.SetActive(value: true);
		rarityTxt.gameObject.SetActive(value: true);
		typeTxt.gameObject.SetActive(value: true);
		bindTypeTxt.gameObject.SetActive(value: true);
		tierTxt.gameObject.SetActive(value: true);
		descTxt.gameObject.SetActive(value: true);
		equippedTxt.gameObject.SetActive(value: true);
		tooltipTxt.gameObject.SetActive(value: true);
		equippedBG.gameObject.SetActive(value: true);
		selectBtn.gameObject.SetActive(value: false);
		HideComparisson();
		nameContainer.color = REGURAL_BG;
		itemDataContainer.color = REGURAL_BG;
	}

	public void SetItemData(ItemIcon reference, CharacterData characterData, ArmoryEquipment armoryEquipment)
	{
		PrepareForRecycle();
		if (AppInfo.IsMobile())
		{
			EventSystem.current.SetSelectedGameObject(selectBtn.gameObject);
		}
		_dispatcher = new Messenger();
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = base.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
		itemData = reference.item;
		cosmetic = reference.cosmetic;
		itemActionBase = reference.itemActionBase;
		this.reference = reference;
		_actionBaseInt = itemActionBase.tooltipType;
		_characterData = characterData;
		_armoryEquipment = armoryEquipment;
		SetupBasicInformation();
		ItemRef itemRef = itemData.itemRef;
		if (itemRef.isNFT)
		{
			if (nameContainer != null)
			{
				nameContainer.color = NFT_BG;
			}
			if (itemDataContainer != null)
			{
				itemDataContainer.color = NFT_BG;
			}
		}
		if (nameTxt != null)
		{
			nameTxt.text = itemRef.coloredName;
		}
		if (itemRef.rarityRef != null && rarityTxt != null)
		{
			rarityTxt.text = itemRef.rarityRef.coloredName;
		}
		if (typeTxt != null)
		{
			typeTxt.text = GetDisplayType(itemRef.rarityRef != null);
		}
		if (bindTypeTxt != null)
		{
			bindTypeTxt.text = itemRef.bindTypeString;
		}
		string text = ((itemData is ItemData && (itemData as ItemData).isCosmetic) ? string.Empty : GetDescription());
		if (text == null || text.Equals(""))
		{
			if (descTxt != null)
			{
				descTxt.gameObject.SetActive(value: false);
			}
		}
		else if (descTxt != null)
		{
			descTxt.text = text;
		}
		int itemTier = getItemTier();
		if (itemTier > 0)
		{
			if (tierTxt != null)
			{
				tierTxt.text = Language.GetString("ui_tier_count", new string[1] { itemTier.ToString() });
			}
		}
		else if (tierTxt != null)
		{
			tierTxt.gameObject.SetActive(value: false);
		}
		switch (itemRef.itemType)
		{
		case 1:
			SetStats();
			break;
		case 16:
			SetStats(isArmory: true);
			break;
		case 6:
		{
			FamiliarRef obj2 = itemRef as FamiliarRef;
			int totalStats = GameData.instance.PROJECT.character.getTotalStats();
			int power = obj2.getPower(totalStats);
			int stamina = obj2.getStamina(totalStats);
			int agility = obj2.getAgility(totalStats);
			itemToolTipStats[0].LoadDetails(ItemTooltipStatTile.STAT_TYPE.POWER, power, power);
			itemToolTipStats[1].LoadDetails(ItemTooltipStatTile.STAT_TYPE.STAMINA, stamina, stamina);
			itemToolTipStats[2].LoadDetails(ItemTooltipStatTile.STAT_TYPE.AGILITY, agility, agility);
			equippedBG.gameObject.SetActive(value: false);
			break;
		}
		case 8:
		{
			MountRef mountRef2 = itemRef as MountRef;
			int itemTier3 = getItemTier();
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			if (itemData is MountData)
			{
				MountData obj3 = itemData as MountData;
				num4 = obj3.getPower(itemTier3);
				num5 = obj3.getStamina(itemTier3);
				num6 = obj3.getAgility(itemTier3);
			}
			else if (mountRef2.stats != null)
			{
				int stats2 = mountRef2.mountRarityRef.getStats(0, itemTier3);
				num4 = Mathf.RoundToInt((float)stats2 * mountRef2.stats.power);
				num5 = Mathf.RoundToInt((float)stats2 * mountRef2.stats.stamina);
				num6 = Mathf.RoundToInt((float)stats2 * mountRef2.stats.agility);
			}
			int equipped4 = num4;
			int equipped5 = num5;
			int equipped6 = num6;
			if (num4 > 0 || num5 > 0 || num6 > 0)
			{
				MountData mountEquipped = GameData.instance.PROJECT.character.mounts.getMountEquipped();
				if (mountEquipped != null)
				{
					equipped4 = mountEquipped.getPower(GameData.instance.PROJECT.character.tier);
					equipped5 = mountEquipped.getStamina(GameData.instance.PROJECT.character.tier);
					equipped6 = mountEquipped.getAgility(GameData.instance.PROJECT.character.tier);
				}
				itemToolTipStats[0].LoadDetails(ItemTooltipStatTile.STAT_TYPE.POWER, num4, equipped4);
				itemToolTipStats[1].LoadDetails(ItemTooltipStatTile.STAT_TYPE.STAMINA, num5, equipped5);
				itemToolTipStats[2].LoadDetails(ItemTooltipStatTile.STAT_TYPE.AGILITY, num6, equipped6);
			}
			else
			{
				HideStats();
			}
			if (equippedBG != null)
			{
				equippedBG.gameObject.SetActive(value: false);
			}
			break;
		}
		case 17:
		{
			MountRef mountRef = itemRef as MountRef;
			int itemTier2 = getItemTier();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (itemData is MountData)
			{
				MountData obj = itemData as MountData;
				num = obj.getPower(itemTier2);
				num2 = obj.getStamina(itemTier2);
				num3 = obj.getAgility(itemTier2);
			}
			else if (mountRef.stats != null)
			{
				int stats = mountRef.mountRarityRef.getStats(0, itemTier2);
				num = Mathf.RoundToInt((float)stats * mountRef.stats.power);
				num2 = Mathf.RoundToInt((float)stats * mountRef.stats.stamina);
				num3 = Mathf.RoundToInt((float)stats * mountRef.stats.agility);
			}
			int equipped = num;
			int equipped2 = num2;
			int equipped3 = num3;
			if (num > 0 || num2 > 0 || num3 > 0)
			{
				MountData currentArmoryMountData = ArmoryEquipment.GetCurrentArmoryMountData();
				if (currentArmoryMountData != null)
				{
					equipped = currentArmoryMountData.getPower(GameData.instance.PROJECT.character.tier);
					equipped2 = currentArmoryMountData.getStamina(GameData.instance.PROJECT.character.tier);
					equipped3 = currentArmoryMountData.getAgility(GameData.instance.PROJECT.character.tier);
				}
				itemToolTipStats[0].LoadDetails(ItemTooltipStatTile.STAT_TYPE.POWER, num, equipped);
				itemToolTipStats[1].LoadDetails(ItemTooltipStatTile.STAT_TYPE.STAMINA, num2, equipped2);
				itemToolTipStats[2].LoadDetails(ItemTooltipStatTile.STAT_TYPE.AGILITY, num3, equipped3);
			}
			else
			{
				HideStats();
			}
			if (equippedBG != null)
			{
				equippedBG.gameObject.SetActive(value: false);
			}
			break;
		}
		case 11:
		case 18:
			SetStats(isArmory: false, forceHideEquipped: true);
			break;
		default:
			if (equippedBG != null)
			{
				equippedBG.gameObject.SetActive(value: false);
			}
			if (equippedTxt != null)
			{
				equippedTxt.gameObject.SetActive(value: false);
			}
			HideStats();
			if (tooltipTxt != null)
			{
				tooltipTxt.gameObject.SetActive(value: false);
			}
			break;
		}
		if (AppInfo.IsMobile())
		{
			if (tooltipTxt != null)
			{
				tooltipTxt.gameObject.SetActive(value: false);
			}
			if (itemActionBase == null || itemActionBase.getTypeName() == null || itemActionBase.tooltipType == 0)
			{
				selectBtn.gameObject.SetActive(value: false);
				EventTrigger eventTrigger = base.gameObject.AddComponent<EventTrigger>();
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.Deselect;
				entry.callback.AddListener(delegate
				{
					OnDeselect();
				});
				eventTrigger.triggers.Add(entry);
				EventSystem.current.SetSelectedGameObject(base.gameObject);
			}
			else if (selectBtn != null)
			{
				selectBtn.gameObject.SetActive(value: true);
				selectBtn.GetComponentInChildren<TextMeshProUGUI>().text = itemActionBase.getTypeName();
				selectBtn.onClick.RemoveAllListeners();
				selectBtn.onClick.AddListener(delegate
				{
					GameData.instance.audioManager.PlaySoundLink("buttonclick");
					if (itemActionBase.getType() == 1)
					{
						reference.ShowEquippedText();
					}
					itemActionBase.Execute();
					base.gameObject.SetActive(value: false);
					HideComparisson();
					_dispatcher.Broadcast(BUTTON_CHANGE_SELECTED);
				});
			}
		}
		else
		{
			if (selectBtn != null)
			{
				selectBtn.gameObject.SetActive(value: false);
			}
			string typeDesc = itemActionBase.getTypeDesc();
			if (typeDesc != null)
			{
				if (tooltipTxt != null)
				{
					tooltipTxt.gameObject.SetActive(value: true);
					tooltipTxt.text = Util.ParseString(typeDesc);
				}
			}
			else if (tooltipTxt != null)
			{
				tooltipTxt.gameObject.SetActive(value: false);
			}
		}
		if (itemIcon != null)
		{
			itemIcon.SetItemData(reference.item, reference.alphaLocked, reference.item.qty);
			itemIcon.SetItemActionType(0);
			itemIcon.ResetHidden();
			if (reference != null && reference.item != null && reference.item.itemRef != null)
			{
				itemIcon.SetHidden(reference.item.itemRef.isHidden());
			}
		}
		StartCoroutine(DrawOnPlace());
		StartCoroutine(DrawOnPlace(0.101f));
	}

	private int getItemTier()
	{
		int itemType = itemData.itemRef.itemType;
		if (itemType == 8 || itemType == 17)
		{
			if (_characterData == null)
			{
				return GameData.instance.PROJECT.character.tier;
			}
			return _characterData.tier;
		}
		return itemData.itemRef.tier;
	}

	public void SetItemDataForComparisson(BaseModelData itemData)
	{
		PrepareForRecycle();
		this.itemData = itemData;
		_actionBaseInt = 0;
		SetupBasicInformation();
		ItemRef itemRef = itemData.itemRef;
		nameTxt.text = itemRef.coloredName;
		if (itemRef.rarityRef != null)
		{
			rarityTxt.text = itemRef.rarityRef.coloredName;
		}
		typeTxt.text = GetDisplayType(itemRef.rarityRef != null);
		if (bindTypeTxt != null)
		{
			bindTypeTxt.text = itemRef.bindTypeString;
		}
		string description = GetDescription();
		if (description == null || description.Equals(""))
		{
			descTxt.gameObject.SetActive(value: false);
		}
		else
		{
			descTxt.text = description;
		}
		int itemTier = getItemTier();
		if (itemTier > 0)
		{
			tierTxt.text = Language.GetString("ui_tier_count", new string[1] { itemTier.ToString() });
		}
		else
		{
			tierTxt.gameObject.SetActive(value: false);
		}
		switch (itemRef.itemType)
		{
		case 1:
			SetStats();
			break;
		case 16:
			SetStats(isArmory: true);
			break;
		case 6:
		{
			FamiliarRef obj3 = itemRef as FamiliarRef;
			int totalStats = GameData.instance.PROJECT.character.getTotalStats();
			int power = obj3.getPower(totalStats);
			int stamina = obj3.getStamina(totalStats);
			int agility = obj3.getAgility(totalStats);
			itemToolTipStats[0].LoadDetails(ItemTooltipStatTile.STAT_TYPE.POWER, power, power);
			itemToolTipStats[1].LoadDetails(ItemTooltipStatTile.STAT_TYPE.STAMINA, stamina, stamina);
			itemToolTipStats[2].LoadDetails(ItemTooltipStatTile.STAT_TYPE.AGILITY, agility, agility);
			equippedBG.gameObject.SetActive(value: false);
			break;
		}
		case 8:
		{
			MountRef mountRef2 = itemRef as MountRef;
			int itemTier3 = getItemTier();
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			if (this.itemData is MountData)
			{
				MountData obj2 = this.itemData as MountData;
				num4 = obj2.getPower(itemTier3);
				num5 = obj2.getStamina(itemTier3);
				num6 = obj2.getAgility(itemTier3);
			}
			else if (mountRef2.stats != null)
			{
				int stats2 = mountRef2.mountRarityRef.getStats(0, itemTier3);
				num4 = Mathf.RoundToInt((float)stats2 * mountRef2.stats.power);
				num5 = Mathf.RoundToInt((float)stats2 * mountRef2.stats.stamina);
				num6 = Mathf.RoundToInt((float)stats2 * mountRef2.stats.agility);
			}
			int equipped4 = num4;
			int equipped5 = num5;
			int equipped6 = num6;
			if (num4 > 0 || num5 > 0 || num6 > 0)
			{
				MountData mountEquipped = GameData.instance.PROJECT.character.mounts.getMountEquipped();
				if (mountEquipped != null)
				{
					equipped4 = mountEquipped.getPower(GameData.instance.PROJECT.character.tier);
					equipped5 = mountEquipped.getStamina(GameData.instance.PROJECT.character.tier);
					equipped6 = mountEquipped.getAgility(GameData.instance.PROJECT.character.tier);
				}
				itemToolTipStats[0].LoadDetails(ItemTooltipStatTile.STAT_TYPE.POWER, num4, equipped4);
				itemToolTipStats[1].LoadDetails(ItemTooltipStatTile.STAT_TYPE.STAMINA, num5, equipped5);
				itemToolTipStats[2].LoadDetails(ItemTooltipStatTile.STAT_TYPE.AGILITY, num6, equipped6);
			}
			else
			{
				HideStats();
			}
			equippedBG.gameObject.SetActive(value: false);
			break;
		}
		case 17:
		{
			MountRef mountRef = itemRef as MountRef;
			int itemTier2 = getItemTier();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (this.itemData is MountData)
			{
				MountData obj = this.itemData as MountData;
				num = obj.getPower(itemTier2);
				num2 = obj.getStamina(itemTier2);
				num3 = obj.getAgility(itemTier2);
			}
			else if (mountRef.stats != null)
			{
				int stats = mountRef.mountRarityRef.getStats(0, itemTier2);
				num = Mathf.RoundToInt((float)stats * mountRef.stats.power);
				num2 = Mathf.RoundToInt((float)stats * mountRef.stats.stamina);
				num3 = Mathf.RoundToInt((float)stats * mountRef.stats.agility);
			}
			int equipped = num;
			int equipped2 = num2;
			int equipped3 = num3;
			if (num > 0 || num2 > 0 || num3 > 0)
			{
				MountData currentArmoryMountData = ArmoryEquipment.GetCurrentArmoryMountData();
				if (currentArmoryMountData != null)
				{
					equipped = currentArmoryMountData.getPower(GameData.instance.PROJECT.character.tier);
					equipped2 = currentArmoryMountData.getStamina(GameData.instance.PROJECT.character.tier);
					equipped3 = currentArmoryMountData.getAgility(GameData.instance.PROJECT.character.tier);
				}
				itemToolTipStats[0].LoadDetails(ItemTooltipStatTile.STAT_TYPE.POWER, num, equipped);
				itemToolTipStats[1].LoadDetails(ItemTooltipStatTile.STAT_TYPE.STAMINA, num2, equipped2);
				itemToolTipStats[2].LoadDetails(ItemTooltipStatTile.STAT_TYPE.AGILITY, num3, equipped3);
			}
			else
			{
				HideStats();
			}
			equippedBG.gameObject.SetActive(value: false);
			break;
		}
		case 11:
		case 18:
			SetStats(isArmory: false, forceHideEquipped: true);
			break;
		default:
			equippedBG.gameObject.SetActive(value: false);
			equippedTxt.gameObject.SetActive(value: false);
			HideStats();
			tooltipTxt.gameObject.SetActive(value: false);
			break;
		}
		tooltipTxt.gameObject.SetActive(value: false);
		selectBtn.gameObject.SetActive(value: false);
		if (itemIcon != null)
		{
			itemIcon.SetItemData(itemData, reference != null && reference.alphaLocked, itemData.qty);
			itemIcon.SetItemActionType(0);
			itemIcon.ResetHidden();
			if (reference != null && reference.item != null && reference.item.itemRef != null)
			{
				itemIcon.SetHidden(reference.item.itemRef.isHidden());
			}
		}
	}

	private IEnumerator DrawOnPlace(float waitTime = 0.1f)
	{
		RectTransform rectTransformTooltip = base.transform.Find("ContainerBG").GetComponent<RectTransform>();
		base.transform.Find("ContainerBG").GetComponent<VerticalLayoutGroup>();
		LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransformTooltip);
		yield return new WaitForSeconds(waitTime);
		if (equippedBG.gameObject.activeSelf)
		{
			_ = equippedBG.GetComponent<RectTransform>().rect.height;
		}
		float num = base.transform.localScale.x * GameData.instance.windowGenerator.canvas.transform.localScale.x;
		float num2 = base.transform.localScale.y * GameData.instance.windowGenerator.canvas.transform.localScale.y;
		float num3 = rectTransformTooltip.rect.width * num;
		float num4 = rectTransformTooltip.rect.height * num2;
		if (!(reference == null))
		{
			RectTransform component = reference.GetComponent<RectTransform>();
			float num5 = component.rect.width * num;
			float num6 = component.rect.height * num2;
			Vector3 position = reference.transform.position;
			if (position.x < GameData.instance.main.uiCamera.transform.position.x)
			{
				_flipped = true;
				position.x += (num5 + num3) * 0.5f;
			}
			else
			{
				_flipped = false;
				position.x -= (num5 + num3) * 0.5f;
			}
			position.y += (num6 + num4) * 0.5f;
			float num7 = GameData.instance.main.uiCamera.transform.position.y - (float)Screen.height * GameData.instance.windowGenerator.canvas.transform.localScale.y / Main.STAGE_SCALE / 2f;
			float num8 = GameData.instance.main.uiCamera.transform.position.y + (float)Screen.height * GameData.instance.windowGenerator.canvas.transform.localScale.y / Main.STAGE_SCALE / 2f;
			if (position.y > num8)
			{
				position.y = num8;
			}
			if (num4 + num6 > (float)Screen.height)
			{
				position.y = num7;
				position.y += num4 + num6 / 2f;
			}
			if (position.y - (num4 + num6) < num7)
			{
				position.y = num7;
				position.y += num4 + num6 / 2f;
			}
			base.transform.position = position;
			CheckEquipped(num3);
			CheckTutorial();
		}
	}

	private string GetDisplayType(bool hasRarity)
	{
		string text = "";
		switch (itemData.itemRef.itemType)
		{
		case 1:
		case 16:
		{
			EquipmentRef equipmentRef = itemData.itemRef as EquipmentRef;
			EquipmentSubtypeRef subtypeTooltip = equipmentRef.getSubtypeTooltip();
			if (hasRarity)
			{
				if (subtypeTooltip != null)
				{
					return equipmentRef.rarityRef.ConvertString(subtypeTooltip.name);
				}
				return equipmentRef.rarityRef.ConvertString(EquipmentRef.getEquipmentTypeName(equipmentRef.equipmentType));
			}
			break;
		}
		case 18:
			text = ItemRef.GetItemName(11);
			break;
		case 17:
			text = ItemRef.GetItemName(8);
			break;
		default:
			text = ItemRef.GetItemName(itemData.itemRef.itemType);
			break;
		}
		if (hasRarity)
		{
			return itemData.itemRef.rarityRef.ConvertString(text);
		}
		return text;
	}

	private void SetupBasicInformation()
	{
		equippedTxt.text = Language.GetString("ui_equipped");
	}

	private void HideStats()
	{
		for (int i = 0; i < itemToolTipStats.Length; i++)
		{
			itemToolTipStats[i].gameObject.SetActive(value: false);
		}
	}

	private void SetStats(bool isArmory = false, bool forceHideEquipped = false)
	{
		int equipped = itemData.power;
		int equipped2 = itemData.stamina;
		int equipped3 = itemData.agility;
		if (itemData.itemRef.itemType.Equals(1) || itemData.itemRef.itemType.Equals(16))
		{
			EquipmentRef equipmentRef = itemData.itemRef as EquipmentRef;
			int availableSlot = Equipment.getAvailableSlot(equipmentRef.equipmentType);
			EquipmentRef equipmentRef2 = GameData.instance.PROJECT.character.equipment.getEquipmentSlot(availableSlot);
			if (isArmory && GameData.instance.PROJECT.character.armory != null && GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot != null)
			{
				equipmentRef2 = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.GetArmoryEquipmentSlot(availableSlot);
			}
			equipped = ((equipmentRef2 != null) ? equipmentRef2.power : 0);
			equipped2 = ((equipmentRef2 != null) ? equipmentRef2.stamina : 0);
			equipped3 = ((equipmentRef2 != null) ? equipmentRef2.agility : 0);
			equippedBG.gameObject.SetActive(equipmentRef2 != null && equipmentRef2.id == equipmentRef.id);
			Image component = base.transform.Find("ContainerBG").GetComponent<Image>();
			if (equipmentRef2 != null && equipmentRef2.id == equipmentRef.id)
			{
				component.sprite = containerEquippedBGSprite;
			}
			else
			{
				component.sprite = containerBGSprite;
			}
		}
		if (forceHideEquipped)
		{
			equippedBG.gameObject.SetActive(value: false);
		}
		if (itemData.power == 0 && itemData.stamina == 0 && itemData.agility == 0)
		{
			HideStats();
			return;
		}
		itemToolTipStats[0].LoadDetails(ItemTooltipStatTile.STAT_TYPE.POWER, itemData.power, equipped);
		itemToolTipStats[1].LoadDetails(ItemTooltipStatTile.STAT_TYPE.STAMINA, itemData.stamina, equipped2);
		itemToolTipStats[2].LoadDetails(ItemTooltipStatTile.STAT_TYPE.AGILITY, itemData.agility, equipped3);
	}

	private string GetDescription()
	{
		string text = "";
		if (cosmetic != null)
		{
			text += "<br>";
			text = text + Language.GetString("ui_cosmetic") + ":<br>" + cosmetic.itemRef.coloredName;
		}
		List<GameModifier> itemModifiers = GetItemModifiers();
		if (itemModifiers != null && itemModifiers.Count > 0)
		{
			string text2 = "";
			int num = 0;
			foreach (GameModifier item in itemModifiers)
			{
				if ((item.type != 0 || item.desc != null) && item.tooltip)
				{
					if (text.Length > 0 && text2.Length <= 0)
					{
						text2 += "<br>";
					}
					if (num > 0)
					{
						text2 += "<br>";
					}
					string text3 = ((item.desc != null) ? Util.ParseModifierString(item, itemData) : ((!item.tooltipFull) ? GameModifier.getTypeDescriptionLong(item.type, item.value) : GameModifier.getTypeDescriptionShort(item.type, item.value)));
					text2 += text3;
					num++;
				}
			}
			text += text2;
		}
		text += GetExtraString(text);
		int power = GameData.instance.PROJECT.character.getTotalPower();
		List<GameModifier> modifiers = GameData.instance.PROJECT.character.getModifiers();
		if (itemData.itemRef.itemType == 6)
		{
			FamiliarRef obj = itemData.itemRef as FamiliarRef;
			power = obj.getPower(GameData.instance.PROJECT.character.getTotalStats());
			modifiers = obj.modifiers;
		}
		string text4 = ((itemData is AugmentData) ? "" : Util.ParseItemString(itemData.itemRef.desc, itemData.itemRef, power, GameModifier.getTypeTotal(modifiers, 17), color: true, itemData.data));
		if (text4 != null && text4.Length > 0)
		{
			if (text.Length > 0)
			{
				text += "<br><br>";
			}
			text += text4;
		}
		EquipmentSetRef itemSet = GetItemSet();
		if (itemSet != null)
		{
			if (text.Length > 0)
			{
				text += "<br><br>";
			}
			Equipment equipment = ((_armoryEquipment == null) ? ((_characterData == null) ? GameData.instance.PROJECT.character.equipment : _characterData.equipment) : ArmoryEquipment.ArmoryEquipmentToEquipment(_armoryEquipment));
			CharacterData characterData = ((_characterData == null) ? GameData.instance.PROJECT.character.toCharacterData() : _characterData);
			int equipmentSetCount = equipment.getEquipmentSetCount(itemSet);
			int count = itemSet.equipment.Count;
			int textSize = itemSet.textSize;
			text = text + itemSet.coloredName + " (" + Util.NumberFormat(equipmentSetCount) + "/" + Util.NumberFormat(count) + "):";
			foreach (EquipmentRef item2 in itemSet.equipment)
			{
				bool itemSetEquipped = equipment.getItemSetEquipped(item2);
				text = text + "<br> " + GetResizedText(itemSet.GetEnabledColor(item2.name, itemSetEquipped), textSize);
			}
			if (itemSet.bonuses != null && itemSet.bonuses.Count > 0)
			{
				if (text.Length > 0)
				{
					text += "<br>";
				}
				foreach (EquipmentSetBonusRef bonuse in itemSet.bonuses)
				{
					bool bonusEnabled = bonuse.getBonusEnabled(equipmentSetCount);
					string text5 = "<br>" + itemSet.GetEnabledColor("(" + Util.NumberFormat(bonuse.count) + "):", bonusEnabled);
					if (bonuse.desc != null)
					{
						string text6 = " " + Util.ParseString(bonuse.desc, bonusEnabled);
						text6 = Util.ParseAbilityString(text6, bonuse.getFirstAbility(), characterData.getTotalPower(), GameModifier.getTypeTotal(characterData.getModifiers(), 17), bonusEnabled);
						if (!bonusEnabled)
						{
							text6 = itemSet.GetEnabledColor(text6, bonusEnabled);
						}
						text5 += text6;
					}
					if (bonuse.abilities != null)
					{
						for (int i = 0; i < bonuse.abilities.Count; i++)
						{
							if (i > 0)
							{
								text5 += ",";
							}
							AbilityRef abilityRef = bonuse.abilities[i];
							string text7 = " " + Util.ParseAbilityString(abilityRef.desc, abilityRef, characterData.getTotalPower(), GameModifier.getTypeTotal(characterData.getModifiers(), 17), bonusEnabled);
							if (!bonusEnabled)
							{
								text7 = itemSet.GetEnabledColor(text7, bonusEnabled);
							}
							text5 += text7;
						}
					}
					int num2 = 0;
					for (int j = 0; j < bonuse.modifiers.Count; j++)
					{
						GameModifier gameModifier = bonuse.modifiers[j];
						if (gameModifier.tooltip)
						{
							if (num2 > 0)
							{
								text5 += ",";
							}
							string text8 = " " + (gameModifier.tooltipFull ? GameModifier.getTypeDescriptionLong(gameModifier.type, gameModifier.value, bonusEnabled) : GameModifier.getTypeDescriptionShort(gameModifier.type, gameModifier.value, bonusEnabled));
							if (!bonusEnabled)
							{
								text8 = itemSet.GetEnabledColor(text8, bonusEnabled);
							}
							text5 += text8;
							num2++;
						}
					}
					text += GetResizedText(text5, textSize);
				}
			}
		}
		if (!text.Trim().Equals(""))
		{
			return Util.ParseString(text);
		}
		return null;
	}

	private string GetExtraString(string itemDesc)
	{
		string text = "";
		switch (itemData.itemRef.itemType)
		{
		case 11:
		case 18:
			if (!(itemData is EnchantData))
			{
				EnchantRef enchantRef = itemData.itemRef as EnchantRef;
				text = GetMinMaxText(itemDesc, enchantRef.statsMin, enchantRef.statsMax, enchantRef.modsMin, enchantRef.modsMax);
			}
			break;
		case 8:
		case 17:
		{
			MountRef mountRef = itemData.itemRef as MountRef;
			if (!(itemData is MountData))
			{
				int num = mountRef.mountRarityRef.getStats(0, itemData.itemRef.tier);
				float num2 = mountRef.mountRarityRef.modsMin;
				float num3 = mountRef.mountRarityRef.modsMax;
				if (mountRef.stats != null)
				{
					num = 0;
				}
				if (mountRef.baseModifiers.Count > 0)
				{
					num2 = 0f;
					num3 = 0f;
				}
				text = GetMinMaxText(itemDesc, num, num, (int)num2, (int)num3);
			}
			if (mountRef.abilities == null || mountRef.abilities.Count <= 0)
			{
				break;
			}
			if (itemDesc.Length > 0 || text.Length > 0)
			{
				text += "<br><br>";
			}
			for (int i = 0; i < mountRef.abilities.Count; i++)
			{
				if (i > 0)
				{
					text += ",";
				}
				string tooltipText = mountRef.abilities[i].getTooltipText(GameData.instance.PROJECT.character.getTotalPower(), GameModifier.getTypeTotal(GameData.instance.PROJECT.character.getModifiers(), 17), showSP: true);
				text += tooltipText;
			}
			break;
		}
		case 15:
			if (itemData is AugmentData)
			{
				AugmentData augmentData = itemData as AugmentData;
				if (augmentData.familiarRef != null)
				{
					text += "<br><br>";
					text += Language.GetString("ui_equipped_to", new string[1] { augmentData.familiarRef.coloredName });
				}
				text += "<br><br>";
				text += Language.GetString("ui_augment_stable_info");
			}
			else
			{
				AugmentRef augmentRef = itemData.itemRef as AugmentRef;
				text = GetMinMaxText(itemDesc, 0, 0, augmentRef.mods, augmentRef.mods);
			}
			break;
		}
		return text;
	}

	private string GetMinMaxText(string desc, int statsMin, int statsMax, int modsMin, int modsMax)
	{
		string text = "";
		int num = 0;
		if (statsMin > 0 || statsMax > 0)
		{
			if (desc.Length > 0 && text.Length <= 0)
			{
				text += "<br><br>";
			}
			if (num > 0)
			{
				text += "<br>";
			}
			string text2 = Util.NumberColor(Util.NumberFormat(statsMin)) + ((statsMin != statsMax) ? ("-" + Util.NumberColor(Util.NumberFormat(statsMax))) : "") + " " + Language.GetString("ui_stats");
			text += text2;
			num++;
		}
		if (modsMin > 0 || modsMax > 0)
		{
			if (desc.Length > 0 && text.Length <= 0)
			{
				text += "<br><br>";
			}
			if (num > 0)
			{
				text += "<br>";
			}
			string text3 = Util.NumberColor(Util.NumberFormat(modsMin)) + ((modsMin != modsMax) ? ("-" + Util.NumberColor(Util.NumberFormat(modsMax))) : "") + " " + ((modsMin == 1 && modsMax == 1) ? Language.GetString("ui_bonus") : Language.GetString("ui_bonuses"));
			text += text3;
			num++;
		}
		return text;
	}

	private string GetResizedText(string text, int size)
	{
		size = Mathf.RoundToInt((float)size / 2.5f);
		if (size > 0)
		{
			return "<size=" + size + ">" + text + "</size>";
		}
		return "<size=" + 6 + ">" + text + "</size>";
	}

	private EquipmentSetRef GetItemSet()
	{
		int itemType = itemData.itemRef.itemType;
		if (itemType == 1 || itemType == 16)
		{
			EquipmentRef equipmentRef = itemData.itemRef as EquipmentRef;
			if (equipmentRef != null)
			{
				return equipmentRef.equipmentSet;
			}
		}
		return null;
	}

	private List<GameModifier> GetItemModifiers()
	{
		switch (itemData.itemRef.itemType)
		{
		case 6:
			return (itemData.itemRef as FamiliarRef).modifiers;
		case 1:
		case 16:
			return (itemData.itemRef as EquipmentRef).modifiers;
		case 9:
			return (itemData.itemRef as RuneRef).modifiers;
		case 11:
		case 18:
			if (itemData == null || !(itemData is EnchantData))
			{
				return null;
			}
			return (itemData as EnchantData).getGameModifiers();
		case 8:
		case 17:
			if (itemData != null && itemData is MountData)
			{
				return (itemData as MountData).getGameModifiers();
			}
			return (itemData.itemRef as MountRef).getGameModifiers();
		case 14:
			return (itemData.itemRef as BobberRef).modifiers;
		case 13:
			return (itemData.itemRef as BaitRef).modifiers;
		case 15:
			if (itemData != null && itemData is AugmentData)
			{
				AugmentData obj = itemData as AugmentData;
				return obj.getGameModifiers(obj.getRank(GameData.instance.PROJECT.character.familiarStable));
			}
			break;
		}
		return null;
	}

	public void OnDeselect()
	{
		if (AppInfo.IsMobile() && !GameData.instance.tutorialManager.hasPopup)
		{
			base.gameObject.SetActive(value: false);
			HideComparisson();
		}
	}

	private void CheckEquipped(float width)
	{
		if (!reference.compare || itemData.qty <= 0)
		{
			return;
		}
		switch (itemData.itemRef.itemType)
		{
		case 1:
		{
			EquipmentRef equipmentRef2 = EquipmentBook.Lookup(itemData.itemRef.id);
			EquipmentRef equipmentSlot = GameData.instance.PROJECT.character.equipment.getEquipmentSlot(Equipment.getAvailableSlot(equipmentRef2.equipmentType));
			if (equipmentSlot != null && equipmentSlot.id != equipmentRef2.id)
			{
				_equippedContainer = GameData.instance.windowGenerator.NewItemToolTipContainerComparisson(new ItemData(equipmentSlot, 0), base.gameObject);
			}
			break;
		}
		case 16:
		{
			EquipmentRef equipmentRef = EquipmentBook.Lookup(itemData.itemRef.id);
			EquipmentRef armoryEquipmentSlot = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.GetArmoryEquipmentSlot(Equipment.getAvailableSlot(equipmentRef.equipmentType));
			if (armoryEquipmentSlot != null && armoryEquipmentSlot.id != equipmentRef.id)
			{
				_equippedContainer = GameData.instance.windowGenerator.NewItemToolTipContainerComparisson(new ItemData(armoryEquipmentSlot, 0), base.gameObject);
			}
			break;
		}
		case 8:
		{
			MountRef mountRef2 = MountBook.Lookup(itemData.itemRef.id);
			MountData mountEquipped = GameData.instance.PROJECT.character.mounts.getMountEquipped();
			if (mountEquipped != null && mountEquipped.itemRef.id != mountRef2.id)
			{
				_equippedContainer = GameData.instance.windowGenerator.NewItemToolTipContainerComparisson(mountEquipped, base.gameObject);
			}
			break;
		}
		case 17:
		{
			MountRef mountRef = MountBook.Lookup(itemData.itemRef.id);
			MountData mountDataEquipped = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.GetMountDataEquipped();
			if (mountDataEquipped != null && mountDataEquipped.itemRef.id != mountRef.id)
			{
				_equippedContainer = GameData.instance.windowGenerator.NewItemToolTipContainerComparisson(mountDataEquipped, base.gameObject);
			}
			break;
		}
		}
		if (_equippedContainer != null)
		{
			StartCoroutine(WaitToRefreshCompare(width));
		}
	}

	private IEnumerator WaitToRefreshCompare(float width)
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(_equippedContainer.GetComponent<RectTransform>());
		yield return new WaitForSeconds(0.2f);
		_equippedContainer.transform.position = new Vector3(_flipped ? (base.transform.position.x + width) : (base.transform.position.x - width), base.transform.position.y, base.transform.position.z);
		Vector3 position = _equippedContainer.transform.position;
		RectTransform component = _equippedContainer.transform.Find("ContainerBG").GetComponent<RectTransform>();
		float y = _equippedContainer.transform.GetComponent<RectTransform>().anchoredPosition.y;
		float num = _equippedContainer.transform.localScale.y * GameData.instance.windowGenerator.canvas.transform.localScale.y;
		RectTransform component2 = _equippedContainer.itemIcon.GetComponent<RectTransform>();
		float num2 = component.rect.height * _equippedContainer.transform.localScale.y;
		float num3 = component2.rect.height * _equippedContainer.transform.localScale.y;
		float y2 = GameData.instance.main.uiCamera.transform.position.y - (float)Screen.height * GameData.instance.windowGenerator.canvas.transform.localScale.y / Main.STAGE_SCALE / 2f;
		if (y + num2 + num3 > (float)Screen.height)
		{
			position.y = y2;
			position.y += component.rect.height * num + component2.rect.height * num / 2f;
			_equippedContainer.transform.position = position;
			Vector3 position2 = base.transform.position;
			position2.y = position.y;
			if (base.transform.position.y < position.y)
			{
				base.transform.position = position2;
			}
			else
			{
				position.y = base.transform.position.y;
				_equippedContainer.transform.position = position;
			}
		}
		_equippedContainer.CheckTutorial();
	}

	public void HideComparisson()
	{
		if (_equippedContainer != null && _equippedContainer.gameObject != null)
		{
			_equippedContainer.gameObject.SetActive(value: false);
		}
	}

	private void CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null || !selectBtn.gameObject.activeSelf)
		{
			return;
		}
		switch (_actionBaseInt)
		{
		case 1:
			if (!GameData.instance.PROJECT.character.tutorial.GetState(13) && GameData.instance.PROJECT.character.tutorial.GetState(12) && !GameData.instance.PROJECT.character.tutorial.GetState(28))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(13);
				base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = false;
				GameData.instance.tutorialManager.ShowTutorialForButton(selectBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(13), 4, selectBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, BackToNormal, closeTooltip: false);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
			else if (!GameData.instance.PROJECT.character.tutorial.GetState(118) && GameData.instance.PROJECT.character.tutorial.GetState(78) && !GameData.instance.PROJECT.character.tutorial.GetState(79))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(118);
				base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = false;
				GameData.instance.tutorialManager.ShowTutorialForButton(selectBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(118), 4, selectBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, BackToNormal, closeTooltip: false);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
			else if (!GameData.instance.PROJECT.character.tutorial.GetState(119) && GameData.instance.PROJECT.character.tutorial.GetState(82) && !GameData.instance.PROJECT.character.tutorial.GetState(83))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(119);
				base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = false;
				GameData.instance.tutorialManager.ShowTutorialForButton(selectBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(119), 4, selectBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, BackToNormal, closeTooltip: false);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
			break;
		case 9:
			if (!GameData.instance.PROJECT.character.tutorial.GetState(33) && GameData.instance.PROJECT.character.tutorial.GetState(25) && !GameData.instance.PROJECT.character.tutorial.GetState(26))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(33);
				base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = false;
				GameData.instance.tutorialManager.ShowTutorialForButton(selectBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(33), 4, selectBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, BackToNormal, closeTooltip: false);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
			break;
		case 3:
			if (!GameData.instance.PROJECT.character.tutorial.GetState(110) && GameData.instance.PROJECT.character.tutorial.GetState(104) && !GameData.instance.PROJECT.character.tutorial.GetState(105))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(110);
				base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = false;
				GameData.instance.tutorialManager.ShowTutorialForButton(selectBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(110), 4, selectBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, BackToNormal, closeTooltip: false);
			}
			break;
		case 14:
			if (!GameData.instance.PROJECT.character.tutorial.GetState(111) && GameData.instance.PROJECT.character.tutorial.GetState(95) && !GameData.instance.PROJECT.character.tutorial.GetState(97))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(111);
				base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = false;
				GameData.instance.tutorialManager.ShowTutorialForButton(selectBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(111), 4, selectBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, BackToNormal, closeTooltip: false);
			}
			else if (!GameData.instance.PROJECT.character.tutorial.GetState(114) && GameData.instance.PROJECT.character.tutorial.GetState(73) && !GameData.instance.PROJECT.character.tutorial.GetState(75))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(114);
				base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = false;
				GameData.instance.tutorialManager.ShowTutorialForButton(selectBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(114), 4, selectBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, BackToNormal, closeTooltip: false);
			}
			break;
		case 4:
			if (!GameData.instance.PROJECT.character.tutorial.GetState(30) && GameData.instance.PROJECT.character.tutorial.GetState(29) && !GameData.instance.PROJECT.character.tutorial.GetState(31))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(30);
				base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = false;
				GameData.instance.tutorialManager.ShowTutorialForButton(selectBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(30), 4, selectBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, BackToNormal, closeTooltip: false);
			}
			break;
		case 16:
			if (!GameData.instance.PROJECT.character.tutorial.GetState(112) && GameData.instance.PROJECT.character.tutorial.GetState(51))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(112);
				base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = false;
				GameData.instance.tutorialManager.ShowTutorialForButton(selectBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(112), 4, selectBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, BackToNormal, closeTooltip: false);
			}
			break;
		case 17:
			if (!GameData.instance.PROJECT.character.tutorial.GetState(113) && GameData.instance.PROJECT.character.tutorial.GetState(91))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(113);
				base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = false;
				GameData.instance.tutorialManager.ShowTutorialForButton(selectBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(113), 4, selectBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, BackToNormal, closeTooltip: false);
			}
			break;
		case 10:
			if (GameData.instance.PROJECT.character.tutorial.GetState(116) || !GameData.instance.PROJECT.character.tutorial.GetState(115))
			{
				break;
			}
			if (GameData.instance.PROJECT.character.zoneCompleted > 3)
			{
				int[] array = new int[6] { 100, 24, 27, 115, 116, 117 };
				foreach (int id in array)
				{
					GameData.instance.PROJECT.character.tutorial.SetState(id);
					GameData.instance.PROJECT.CheckTutorialChanges();
				}
			}
			else
			{
				GameData.instance.PROJECT.character.tutorial.SetState(116);
				base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = false;
				GameData.instance.tutorialManager.ShowTutorialForButton(selectBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(116), 4, selectBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, delegate(object e)
				{
					BackToNormal(e);
				}, closeTooltip: false);
			}
			break;
		case 6:
			if (reference.forceConsume && !GameData.instance.PROJECT.character.tutorial.GetState(120))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(120);
				base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = false;
				GameData.instance.tutorialManager.ShowTutorialForButton(selectBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(120), 4, selectBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, delegate(object e)
				{
					BackToNormal(e);
				}, closeTooltip: false);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
			break;
		case 12:
			if (GameData.instance.PROJECT.character.tutorial.GetState(124) && !GameData.instance.PROJECT.character.tutorial.GetState(125) && !GameData.instance.PROJECT.character.tutorial.GetState(126))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(125);
				base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = false;
				GameData.instance.tutorialManager.ShowTutorialForButton(selectBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(125), 4, selectBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, delegate(object e)
				{
					BackToNormal(e);
				}, closeTooltip: false);
			}
			break;
		case 2:
		case 5:
		case 7:
		case 8:
		case 11:
		case 13:
		case 15:
			break;
		}
	}

	private void BackToNormal(object e)
	{
		base.transform.Find("ContainerBG").GetComponent<ContentSizeFitter>().enabled = true;
	}
}
