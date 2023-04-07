using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.avatar;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.item;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterEquipmentPanel : MonoBehaviour
{
	private const float DISPLAY_OFFSET = -18f;

	private const float DISPLAY_MOUNT_OFFSET = 2f;

	private const float PUPPET_MAX_WORLD_Y_SIZE = 50f;

	public RectTransform baseBackground;

	public RectTransform nftLevelPlaceholder;

	public AvatarBackground nftBackground;

	public AvatarGenerationBanner nftGenBanner;

	public TextMeshProUGUI powerTxt;

	public TextMeshProUGUI staminaTxt;

	public TextMeshProUGUI agilityTxt;

	public TextMeshProUGUI levelTxt;

	public Button cosmeticMainhandBtn;

	public Button cosmeticOffhandBtn;

	public Button cosmeticHeadBtn;

	public Button cosmeticBodyBtn;

	public GameObject placeholderHead;

	public GameObject placeholderBody;

	public GameObject placeholderMainhand;

	public GameObject placeholderOffhand;

	public GameObject placeholderNeck;

	public GameObject placeholderRing;

	public GameObject placeholderAccessory;

	public GameObject placeholderPet;

	public GameObject placeholderMount;

	public GameObject placeholderDisplay;

	public GameObject overSprite;

	private CharacterData _characterData;

	private bool _editable;

	private CharacterDisplay _display;

	private Dictionary<int, ItemIcon> _slots = new Dictionary<int, ItemIcon>();

	private ItemIcon _mountSlot;

	public Transform iconPrefab;

	private ItemIcon mountItemIcon;

	private CharacterWindow _characterWindow;

	public CharacterWindow characterWindow
	{
		get
		{
			return _characterWindow;
		}
		set
		{
			_characterWindow = value;
		}
	}

	public static bool IsPetPlaceHolderAvailable => GameData.instance.PROJECT.character.inventory.GetItemsByType(1, 8).Count > 0;

	public static bool IsAccessoryPlaceholderAvailable => GameData.instance.PROJECT.character.inventory.GetItemsByType(1, 7).Count > 0;

	public static bool IsMountPlaceholderAvailable
	{
		get
		{
			List<ItemData> itemsByType = GameData.instance.PROJECT.character.inventory.GetItemsByType(8);
			Mounts mounts = GameData.instance.PROJECT.character.mounts;
			if (itemsByType.Count <= 0)
			{
				return mounts.mounts.Count > 0;
			}
			return true;
		}
	}

	public static List<(int, bool)> SlotsAvailable => new List<(int, bool)>
	{
		(0, true),
		(1, true),
		(2, true),
		(3, true),
		(4, true),
		(5, true),
		(6, IsAccessoryPlaceholderAvailable),
		(7, IsPetPlaceHolderAvailable),
		(8, IsMountPlaceholderAvailable)
	};

	public Dictionary<int, ItemIcon> slots => _slots;

	public void LoadDetails(CharacterData characterData, bool editable = false)
	{
		_editable = editable;
		levelTxt.text = Language.GetString("ui_current_level", new string[1] { Util.NumberFormat(characterData.level) });
		placeholderDisplay.SetActive(value: false);
		if (!editable)
		{
			cosmeticMainhandBtn.gameObject.SetActive(value: false);
			cosmeticOffhandBtn.gameObject.SetActive(value: false);
			cosmeticHeadBtn.gameObject.SetActive(value: false);
			cosmeticBodyBtn.gameObject.SetActive(value: false);
		}
		GameData.instance.PROJECT.character.mounts.OnChange.AddListener(UpdateMount);
		SetCharacterData(characterData);
		CreateSlots();
	}

	public void UpdateLayer()
	{
		if (_display != null)
		{
			SortingGroup component = _display.GetComponent<SortingGroup>();
			if (component != null && component.enabled)
			{
				component.sortingOrder = 1 + GetComponentInParent<WindowsMain>().sortingLayer;
			}
		}
	}

	public void OnCosmeticMainhandBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ShowCosmeticSlot(0);
	}

	public void OnCosmeticOffhandBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ShowCosmeticSlot(1);
	}

	public void OnCosmeticHeadBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ShowCosmeticSlot(2);
	}

	public void OnCosmeticBodyBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ShowCosmeticSlot(3);
	}

	public void SetCharacterData(CharacterData characterData)
	{
		_characterData = characterData;
		SetDisplay(_characterData.toCharacterDisplay(1f, displayMount: true));
		SetStats(characterData.getTotalPower(), characterData.getTotalStamina(), characterData.getTotalAgility());
		if (_characterData.isIMXG0)
		{
			SetIMXDisplay();
		}
		UpdateSlots();
		CheckAllButtons();
	}

	private void UpdateCharacterDisplayPosition()
	{
		if (_display.hasMountEquipped())
		{
			_display.transform.localPosition = placeholderDisplay.transform.localPosition + new Vector3(0f, 2f, 0f);
			SWFAsset componentInChildren = _display.GetComponentInChildren<SWFAsset>();
			if (componentInChildren != null)
			{
				componentInChildren.transform.localPosition = Vector3.zero;
				componentInChildren.transform.localRotation = Quaternion.Euler(Vector3.zero);
				componentInChildren.transform.localScale = Vector3.one * 100f;
			}
		}
		else
		{
			_display.transform.localPosition = placeholderDisplay.transform.localPosition + new Vector3(0f, -18f, 0f);
			CharacterPuppet characterPuppet = _display.characterPuppet;
			if (characterPuppet != null)
			{
				characterPuppet.transform.localPosition = new Vector3(0f, 40f, 0f);
				characterPuppet.transform.localRotation = Quaternion.Euler(Vector3.zero);
				characterPuppet.transform.localScale = Vector3.one * 100f;
			}
		}
	}

	private void ShowCosmeticSlot(int slot)
	{
		EquipmentRef equipmentSlot = _characterData.equipment.getEquipmentSlot(slot);
		if (equipmentSlot == null)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_equipment_cosmetic"));
		}
		else
		{
			GameData.instance.windowGenerator.NewCosmeticsWindow(equipmentSlot);
		}
	}

	private void CreateSlots()
	{
		if (_slots != null)
		{
			_slots.Clear();
		}
		_slots = new Dictionary<int, ItemIcon>();
		SetSlot(placeholderHead, 2);
		SetSlot(placeholderBody, 3);
		SetSlot(placeholderMainhand, 0);
		SetSlot(placeholderOffhand, 1);
		SetSlot(placeholderNeck, 4);
		SetSlot(placeholderRing, 5);
		SetSlot(placeholderAccessory, 6);
		SetSlot(placeholderPet, 7);
		if (placeholderMount == null)
		{
			return;
		}
		if (MountBook.size <= 0)
		{
			placeholderMount.SetActive(value: false);
			return;
		}
		Transform transform = UnityEngine.Object.Instantiate(iconPrefab);
		transform.SetParent(placeholderMount.transform, worldPositionStays: false);
		transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
		ItemIcon itemIcon = transform.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = transform.gameObject.AddComponent<ItemIcon>();
		}
		mountItemIcon = itemIcon;
		mountItemIcon.SetCharacterData(_characterData);
		UpdateMount();
	}

	public void OnUpdateMountIcon()
	{
		if (mountItemIcon.itemRef != null)
		{
			mountItemIcon.itemRef.OverrideItemType(8);
		}
	}

	private void UpdateMount()
	{
		if (mountItemIcon == null || !placeholderMount.activeInHierarchy)
		{
			return;
		}
		mountItemIcon.onClick.RemoveAllListeners();
		MountData mountEquipped = _characterData.mounts.getMountEquipped();
		if (mountEquipped != null && mountEquipped.mountRef != null)
		{
			mountItemIcon.SetMountData(mountEquipped, _characterData.mounts.cosmetic);
			mountItemIcon.SetCompare(!_editable);
			if (_editable)
			{
				mountItemIcon.SetItemActionType(3);
			}
			else
			{
				mountItemIcon.SetItemActionType(0);
			}
		}
		else
		{
			mountItemIcon.SetMountData(null);
			if (_editable && GameData.instance.PROJECT.character.mounts != null)
			{
				mountItemIcon.onClick.AddListener(delegate
				{
					GameData.instance.windowGenerator.NewMountSelectWindow(GameData.instance.PROJECT.character.mounts, changeable: true, equippable: true);
				});
			}
		}
		Invoke("UpdateCharacterDisplayPosition", 0.5f);
	}

	private void SetSlot(GameObject placeholder, int slot)
	{
		Transform obj = UnityEngine.Object.Instantiate(iconPrefab);
		obj.SetParent(placeholder.transform, worldPositionStays: false);
		obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
		obj.name = slot.ToString();
		ItemIcon orAddComponent = Util.GetOrAddComponent<ItemIcon>(obj.gameObject);
		EquipmentRef equipmentSlot = _characterData.equipment.getEquipmentSlot(slot);
		EquipmentRef equipmentRef = _characterData.equipment.getCosmeticSlot(slot);
		if (equipmentRef == equipmentSlot)
		{
			equipmentRef = null;
		}
		orAddComponent.SetEquipmentData(equipmentSlot, equipmentRef, showComparision: false, 1);
		orAddComponent.SetCompare(!_editable);
		if (equipmentSlot == null)
		{
			if (_editable)
			{
				orAddComponent.onClick.AddListener(delegate
				{
					GameData.instance.windowGenerator.NewEquipmentWindow(slot, (_characterWindow != null) ? _characterWindow.gameObject : null);
				});
			}
		}
		else if (_editable)
		{
			orAddComponent.SetItemActionTypeWithName(3, Language.GetString("ui_view"));
		}
		else
		{
			orAddComponent.SetItemActionType(0);
		}
		orAddComponent.SetCharacterData(_characterData);
		_slots.Add(slot, orAddComponent);
	}

	public void SetDisplay(CharacterDisplay display)
	{
		if (_display != null)
		{
			UnityEngine.Object.Destroy(_display.gameObject);
			CharacterDisplay display2 = _display;
			display2.BOUNDS_UPDATED = (Action)Delegate.Remove(display2.BOUNDS_UPDATED, (Action)delegate
			{
				CheckAndUpdatePuppetSize();
			});
			_display = null;
		}
		_display = display;
		CharacterDisplay display3 = _display;
		display3.BOUNDS_UPDATED = (Action)Delegate.Combine(display3.BOUNDS_UPDATED, (Action)delegate
		{
			CheckAndUpdatePuppetSize();
		});
		_display.transform.SetParent(base.transform, worldPositionStays: false);
		_display.transform.position = base.transform.position;
		UpdateCharacterDisplayPosition();
		Util.ChangeLayer(_display.transform, "UI");
		SortingGroup sortingGroup = _display.gameObject.AddComponent<SortingGroup>();
		if (sortingGroup.enabled)
		{
			sortingGroup.sortingLayerName = "UI";
			sortingGroup.sortingOrder = 1 + GetComponentInParent<WindowsMain>().sortingLayer;
		}
	}

	public void SetIMXDisplay()
	{
		nftBackground.gameObject.SetActive(value: true);
		GameObject gameObject = _characterData.nftBackground;
		GameObject nftFrame = _characterData.nftFrame;
		baseBackground.gameObject.SetActive(nftFrame == null);
		if (gameObject != null || nftFrame != null)
		{
			nftBackground.LoadDetails(gameObject, nftFrame);
		}
		levelTxt.rectTransform.SetParent(nftLevelPlaceholder, worldPositionStays: false);
		nftGenBanner.gameObject.SetActive(value: true);
		nftGenBanner.LoadDetails(_characterData.nftGeneration, _characterData.nftRarity);
	}

	public void SetStats(int power, int stamina, int agility)
	{
		powerTxt.text = Util.NumberFormat(power);
		staminaTxt.text = Util.NumberFormat(stamina);
		agilityTxt.text = Util.NumberFormat(agility);
	}

	public void SetNewStats(int statType, int previous)
	{
		TextMeshProUGUI animatedText = null;
		int num = 0;
		int statEnd = 0;
		switch (statType)
		{
		case 0:
			animatedText = powerTxt;
			statEnd = GameData.instance.PROJECT.character.getTotalPower();
			break;
		case 1:
			animatedText = staminaTxt;
			statEnd = GameData.instance.PROJECT.character.getTotalStamina();
			break;
		case 2:
			animatedText = agilityTxt;
			statEnd = GameData.instance.PROJECT.character.getTotalAgility();
			break;
		}
		if (animatedText == null)
		{
			return;
		}
		num = statEnd - previous;
		if (num != 0)
		{
			Transform transform = UnityEngine.Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/battle/BattleTextUIFix"));
			transform.SetParent(base.transform, worldPositionStays: false);
			transform.GetComponent<TextMeshProUGUI>().fontSize = 10f;
			BattleTextUI battleTextUI = transform.GetComponent<BattleTextUI>();
			string text = "";
			int num2 = 0;
			if (num > 0)
			{
				text = BattleText.COLOR_GREEN;
				num2 = -10;
			}
			else
			{
				text = BattleText.COLOR_RED;
				num2 = 10;
			}
			battleTextUI.LoadDetails(Util.NumberFormat(Mathf.Abs(num)), text, 1.5f, 0f, animatedText.transform.position.x, animatedText.transform.position.y, 0.5f, num2, 0f, doFlash: false);
			battleTextUI.destroyed.AddListener(delegate
			{
				ShowOriginalText(battleTextUI, animatedText, statEnd);
			});
			Canvas component = overSprite.GetComponent<Canvas>();
			if (component != null)
			{
				Canvas canvas = battleTextUI.gameObject.AddComponent<Canvas>();
				canvas.overrideSorting = true;
				canvas.sortingLayerName = component.sortingLayerName;
				canvas.sortingOrder = component.sortingOrder + 1;
			}
		}
	}

	private void ShowOriginalText(BattleTextUI destroyed, TextMeshProUGUI og, int endText)
	{
		destroyed.destroyed.RemoveAllListeners();
		og.text = Util.NumberFormat(endText);
	}

	private void CheckAndUpdatePuppetSize()
	{
		if (_display.bounds.size.y > 50f)
		{
			_display.transform.localScale = Vector2.one * 50f / _display.bounds.size.y;
		}
	}

	public void UpdateSlots()
	{
		foreach (KeyValuePair<int, ItemIcon> entry in _slots)
		{
			ItemIcon value = entry.Value;
			if (!(value != null))
			{
				continue;
			}
			EquipmentRef equipmentSlot = _characterData.equipment.getEquipmentSlot(entry.Key);
			EquipmentRef equipmentRef = _characterData.equipment.getCosmeticSlot(entry.Key);
			if (equipmentRef == equipmentSlot)
			{
				equipmentRef = null;
			}
			value.SetEquipmentData(equipmentSlot, equipmentRef, showComparision: false, 1);
			value.SetCompare(!_editable);
			value.onClick.RemoveAllListeners();
			if (equipmentSlot == null)
			{
				value.onClick.AddListener(delegate
				{
					GameData.instance.windowGenerator.NewEquipmentWindow(entry.Key, (_characterWindow != null) ? _characterWindow.gameObject : null);
				});
			}
			else
			{
				value.SetItemActionType(3);
			}
		}
		_ = _mountSlot != null;
	}

	public void DoEnable()
	{
		if (cosmeticMainhandBtn != null)
		{
			cosmeticMainhandBtn.interactable = true;
		}
		if (cosmeticOffhandBtn != null)
		{
			cosmeticOffhandBtn.interactable = true;
		}
		if (cosmeticHeadBtn != null)
		{
			cosmeticHeadBtn.interactable = true;
		}
		if (cosmeticBodyBtn != null)
		{
			cosmeticBodyBtn.interactable = true;
		}
		if (_slots != null)
		{
			for (int i = 0; i < _slots.Count; i++)
			{
			}
		}
	}

	public void DoDisable()
	{
		if (cosmeticMainhandBtn != null)
		{
			cosmeticMainhandBtn.interactable = false;
		}
		if (cosmeticOffhandBtn != null)
		{
			cosmeticOffhandBtn.interactable = false;
		}
		if (cosmeticHeadBtn != null)
		{
			cosmeticHeadBtn.interactable = false;
		}
		if (cosmeticBodyBtn != null)
		{
			cosmeticBodyBtn.interactable = false;
		}
		if (_slots != null)
		{
			for (int i = 0; i < _slots.Count; i++)
			{
			}
		}
	}

	public void CheckAllButtons()
	{
		placeholderPet.SetActive(IsPetPlaceHolderAvailable);
		placeholderAccessory.SetActive(IsAccessoryPlaceholderAvailable);
		if (placeholderMount != null)
		{
			placeholderMount.SetActive(IsMountPlaceholderAvailable);
		}
		CheckAllCosmeticButtons();
	}

	private void CheckAllCosmeticButtons()
	{
		if (!_editable)
		{
			if (cosmeticMainhandBtn != null)
			{
				cosmeticMainhandBtn.gameObject.SetActive(value: false);
			}
			if (cosmeticOffhandBtn != null)
			{
				cosmeticOffhandBtn.gameObject.SetActive(value: false);
			}
			if (cosmeticHeadBtn != null)
			{
				cosmeticHeadBtn.gameObject.SetActive(value: false);
			}
			if (cosmeticBodyBtn != null)
			{
				cosmeticBodyBtn.gameObject.SetActive(value: false);
			}
		}
		else
		{
			if (cosmeticMainhandBtn != null)
			{
				cosmeticMainhandBtn.gameObject.SetActive(CheckMultipleCosmeticCount(GameData.instance.PROJECT.character.equipment.equipmentSlots[0]));
			}
			if (cosmeticOffhandBtn != null)
			{
				cosmeticOffhandBtn.gameObject.SetActive(CheckMultipleCosmeticCount(GameData.instance.PROJECT.character.equipment.equipmentSlots[1]));
			}
			if (cosmeticHeadBtn != null)
			{
				cosmeticHeadBtn.gameObject.SetActive(CheckMultipleCosmeticCount(GameData.instance.PROJECT.character.equipment.equipmentSlots[2]));
			}
			if (cosmeticBodyBtn != null)
			{
				cosmeticBodyBtn.gameObject.SetActive(CheckMultipleCosmeticCount(GameData.instance.PROJECT.character.equipment.equipmentSlots[3]));
			}
		}
	}

	private bool CheckMultipleCosmeticCount(EquipmentRef equipmentRef)
	{
		List<ItemRef> list = new List<ItemRef>();
		if (equipmentRef != null)
		{
			List<EquipmentRef> fullEquipmentList = EquipmentBook.GetFullEquipmentList();
			for (int i = 0; i < fullEquipmentList.Count; i++)
			{
				EquipmentRef equipmentRef2 = fullEquipmentList[i];
				if (equipmentRef2 != null && equipmentRef2.cosmetic && equipmentRef2.rank <= 0 && equipmentRef.equipmentType == equipmentRef2.equipmentType && equipmentRef.subtypesMatch(equipmentRef2))
				{
					list.Add(equipmentRef2);
				}
			}
			int num = 0;
			foreach (ItemRef item in list)
			{
				if (GameData.instance.PROJECT.character.inventory.hasOwnedItem(item))
				{
					num++;
					if (num > 1)
					{
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	private void OnDestroy()
	{
		if (_display != null)
		{
			CharacterDisplay display = _display;
			display.BOUNDS_UPDATED = (Action)Delegate.Remove(display.BOUNDS_UPDATED, (Action)delegate
			{
				CheckAndUpdatePuppetSize();
			});
		}
	}
}
