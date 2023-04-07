using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.avatar;
using com.ultrabit.bitheroes.ui.item;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character.armory;

public class CharacterArmoryEquipmentPanel : MonoBehaviour
{
	private const float DISPLAY_OFFSET = -53f;

	private const float DISPLAY_MOUNT_OFFSET = -10f;

	private const float PUPPET_MAX_WORLD_Y_SIZE = 40f;

	public RectTransform baseBackground;

	public RectTransform nftLevelPlaceholder;

	public AvatarBackground nftBackground;

	public AvatarGenerationBanner nftGenBanner;

	public TextMeshProUGUI powerTxt;

	public TextMeshProUGUI staminaTxt;

	public TextMeshProUGUI agilityTxt;

	public TextMeshProUGUI levelTxt;

	public Image swordMC;

	public Image heartMC;

	public Image agilityMC;

	public Image swordLines;

	public Image heartLines;

	public Image agilityLines;

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

	public Transform iconPrefab;

	public GameObject OverSprite;

	private CharacterData _characterData;

	private bool _editable;

	private ArmoryEquipment _armoryEquipment;

	private Character _character;

	private CharacterDisplay _display;

	private Dictionary<int, ItemIcon> _slots;

	private ItemIcon _mountSlot;

	private int _characterSortingLayer;

	private CharacterArmoryWindow _characterWindow;

	private SortingGroup _sortingGroup;

	public CharacterArmoryWindow characterArmoryWindow
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

	public void LoadDetails(ArmoryEquipment armoryEquip, Character character = null, bool editable = false, int characterSortingLayer = 5000)
	{
		_characterSortingLayer = characterSortingLayer;
		_armoryEquipment = armoryEquip;
		_editable = editable;
		_character = character;
		placeholderDisplay.GetComponent<Image>().enabled = false;
		if (!_editable)
		{
			cosmeticMainhandBtn.gameObject.SetActive(value: false);
			cosmeticOffhandBtn.gameObject.SetActive(value: false);
			cosmeticHeadBtn.gameObject.SetActive(value: false);
			cosmeticBodyBtn.gameObject.SetActive(value: false);
		}
		if (character != null)
		{
			SetCharacterData(character.toCharacterData(duplicateMounts: true));
		}
		Canvas component = OverSprite.GetComponent<Canvas>();
		if (component != null)
		{
			component.sortingLayerName = "UI";
			component.sortingOrder = _characterSortingLayer;
		}
		if (OverSprite.GetComponent<GraphicRaycaster>() == null)
		{
			OverSprite.AddComponent<GraphicRaycaster>();
		}
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnArmoryEquipmentChange()
	{
		SetCharacterData(_character.toCharacterData(duplicateMounts: true));
		UpdateSlots();
	}

	public void SetCharacterData(CharacterData characterData)
	{
		_characterData = characterData;
		Equipment equipment = ArmoryEquipment.ArmoryEquipmentToEquipment(_armoryEquipment);
		_characterData.setEquipment(equipment);
		if (_characterData.isIMXG0)
		{
			SetIMXDisplay();
		}
		bool showMount = false;
		MountData mountData = null;
		MountRef mountRef = null;
		_characterData.mounts.setCosmetic(null);
		for (int i = 0; i < _characterData.mounts.mounts.Count; i++)
		{
			MountData mountData2 = _characterData.mounts.mounts[i];
			if (mountData2.uid == _armoryEquipment.mount)
			{
				mountData = mountData2;
				showMount = true;
			}
			if (mountData2.mountRef.id == _armoryEquipment.mountCosmetic)
			{
				mountRef = mountData2.mountRef;
			}
		}
		if (mountRef == null)
		{
			foreach (MountRef fullMount in MountBook.GetFullMountList())
			{
				if (!(fullMount == null) && fullMount.cosmetic && fullMount.id == _armoryEquipment.mountCosmetic)
				{
					mountRef = fullMount;
					break;
				}
			}
		}
		if (mountData != null)
		{
			_characterData.mounts.setEquipped(mountData, doDispatch: false);
		}
		if (mountRef != null)
		{
			_characterData.mounts.setCosmetic(mountRef);
		}
		_characterData.showMount = showMount;
		if (_display != null)
		{
			UnityEngine.Object.Destroy(_display.gameObject);
			CharacterDisplay display = _display;
			display.BOUNDS_UPDATED = (Action)Delegate.Remove(display.BOUNDS_UPDATED, (Action)delegate
			{
				CheckAndUpdatePuppetSize();
			});
			_display = null;
		}
		_display = _characterData.toCharacterDisplay(1f, displayMount: true);
		_display.transform.SetParent(base.transform, worldPositionStays: false);
		CharacterDisplay display2 = _display;
		display2.BOUNDS_UPDATED = (Action)Delegate.Combine(display2.BOUNDS_UPDATED, (Action)delegate
		{
			CheckAndUpdatePuppetSize();
		});
		_display.SetLocalPosition(_display.hasMountEquipped() ? new Vector3(0f, -10f, 0f) : new Vector3(0f, -53f, 0f));
		Util.ChangeLayer(_display.transform, "UI");
		_sortingGroup = _display.gameObject.AddComponent<SortingGroup>();
		_sortingGroup.sortingLayerName = "UI";
		_sortingGroup.sortingOrder = _characterSortingLayer;
		levelTxt.text = Language.GetString("ui_current_level", new string[1] { Util.NumberFormat(characterData.level) });
		SetStats(_characterData.getTotalPower(forceCalc: true), _characterData.getTotalStamina(forceCalc: true), _characterData.getTotalAgility(forceCalc: true));
		UpdateData();
		CreateSlots();
	}

	private void CheckAndUpdatePuppetSize()
	{
		if (_display.bounds.size.y > 40f)
		{
			_display.transform.localScale = Vector2.one * 40f / _display.bounds.size.y;
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

	public void UpdateSortingLayer(int layer)
	{
		if (_sortingGroup != null)
		{
			_sortingGroup.sortingOrder = layer;
		}
		Canvas component = OverSprite.GetComponent<Canvas>();
		if (component != null)
		{
			component.sortingLayerName = "UI";
			component.sortingOrder = layer;
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

	private void ShowCosmeticSlot(int slot)
	{
		ArmoryRef armoryEquipmentSlot = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.GetArmoryEquipmentSlot(slot);
		if (armoryEquipmentSlot == null)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_equipment_cosmetic"));
		}
		else
		{
			GameData.instance.windowGenerator.NewArmoryCosmeticsWindow(armoryEquipmentSlot);
		}
	}

	private void CreateSlots()
	{
		if (_slots == null)
		{
			_slots = new Dictionary<int, ItemIcon>();
			SetSlot(placeholderHead, 2);
			SetSlot(placeholderBody, 3);
			SetSlot(placeholderMainhand, 0);
			SetSlot(placeholderOffhand, 1);
			SetSlot(placeholderNeck, 4);
			SetSlot(placeholderRing, 5);
			SetSlot(placeholderAccessory, 6);
			SetSlot(placeholderPet, 7);
			Transform transform = UnityEngine.Object.Instantiate(iconPrefab);
			transform.SetParent(placeholderMount.transform, worldPositionStays: false);
			transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
			ItemIcon itemIcon = transform.gameObject.GetComponent<ItemIcon>();
			if (itemIcon == null)
			{
				itemIcon = transform.gameObject.AddComponent<ItemIcon>();
			}
			_mountSlot = itemIcon;
			UpdateMount();
		}
	}

	private void UpdateMount()
	{
		MountData mountData = null;
		MountRef mountRef = null;
		for (int i = 0; i < _characterData.mounts.mounts.Count; i++)
		{
			MountData mountData2 = _characterData.mounts.mounts[i];
			if (_armoryEquipment.mount == mountData2.uid)
			{
				mountData = mountData2;
			}
		}
		foreach (MountRef fullMount in MountBook.GetFullMountList())
		{
			if (_armoryEquipment.mountCosmetic == fullMount.id)
			{
				mountRef = fullMount;
			}
		}
		if (mountData != null && _characterData.charID != GameData.instance.PROJECT.character.id)
		{
			_characterData.mounts.setEquipped(mountData, doDispatch: false);
		}
		MountRef mountRef2 = mountData?.mountRef;
		if (mountRef == null)
		{
			mountRef = mountRef2;
		}
		if (mountRef2 != null)
		{
			mountRef2.OverrideItemType(17);
		}
		if (mountData != null && mountData.mountRef != null)
		{
			_mountSlot.SetMountData(mountData, mountRef);
			_mountSlot.SetCompare(!_editable);
			if (_editable)
			{
				_mountSlot.SetItemActionType(3);
			}
			else
			{
				_mountSlot.SetItemActionType(0);
			}
		}
		else
		{
			_mountSlot.SetMountData(null);
			if (_editable && GameData.instance.PROJECT.character.mounts != null)
			{
				_mountSlot.onClick.AddListener(delegate
				{
					GameData.instance.windowGenerator.NewArmoryMountSelectWindow(GameData.instance.PROJECT.character.mounts, changeable: true, equippable: true);
				});
			}
		}
		if (mountRef2 == null)
		{
			if (_characterData.mounts.mounts.Count <= 0 || !_editable)
			{
				_mountSlot.HideComparison();
			}
		}
		else if (mountRef != null && mountRef.id != mountRef2.id)
		{
			_mountSlot.PlayComparison("cosmetic");
		}
		else
		{
			_mountSlot.HideComparison();
		}
	}

	private void SetSlot(GameObject placeholder, int slot)
	{
		Transform obj = UnityEngine.Object.Instantiate(iconPrefab);
		obj.SetParent(placeholder.transform, worldPositionStays: false);
		obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
		ItemIcon orAddComponent = Util.GetOrAddComponent<ItemIcon>(obj.gameObject);
		ArmoryRef armoryEquipmentSlot = _armoryEquipment.GetArmoryEquipmentSlot(slot);
		ArmoryRef armoryRef = _armoryEquipment.GetCosmeticSlot(slot);
		if (armoryRef == armoryEquipmentSlot)
		{
			armoryRef = null;
		}
		orAddComponent.SetEquipmentData(armoryEquipmentSlot, armoryRef, showComparision: false);
		if (armoryEquipmentSlot == null && _editable)
		{
			orAddComponent.onClick.AddListener(delegate
			{
				GameData.instance.windowGenerator.NewArmoryEquipmentWindow(slot, _characterWindow.gameObject);
			});
		}
		else if (_editable)
		{
			orAddComponent.SetItemActionType(3);
		}
		else
		{
			orAddComponent.SetItemActionType(0);
		}
		orAddComponent.SetCharacterData(_characterData);
		orAddComponent.SetArmoryEquipment(_armoryEquipment);
		_slots.Add(slot, orAddComponent);
	}

	public void UpdateSlots()
	{
		foreach (KeyValuePair<int, ItemIcon> slot in _slots)
		{
			ArmoryRef armoryEquipmentSlot = _armoryEquipment.GetArmoryEquipmentSlot(slot.Key);
			ArmoryRef armoryRef = _armoryEquipment.GetCosmeticSlot(slot.Key);
			if (armoryRef == armoryEquipmentSlot)
			{
				armoryRef = null;
			}
			slot.Value.SetEquipmentData(armoryEquipmentSlot, armoryRef);
		}
	}

	public void UpdateData()
	{
	}
}
