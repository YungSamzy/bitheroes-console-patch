using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.tutorial;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.enchant;

public class EnchantsWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button viewBtn;

	public Button createBtn;

	public RectTransform placeholderSlotA;

	public RectTransform placeholderSlotB;

	public RectTransform placeholderSlotC;

	public RectTransform placeholderSlotD;

	public RectTransform placeholderSlotE;

	public RectTransform placeholderSlotF;

	public RectTransform placeholderAsset;

	public Transform enchantTilePrefab;

	private Enchants _enchants;

	private bool _changeable;

	private bool _isArmory;

	private CharacterData _friendcharData;

	private Dictionary<int, EnchantTile> _tiles;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(Enchants enchants, bool changeable = false, bool isArmory = false, CharacterData friendcharData = null)
	{
		_enchants = enchants;
		_changeable = changeable;
		_isArmory = isArmory;
		_friendcharData = friendcharData;
		topperTxt.text = ItemRef.GetItemNamePlural(11);
		viewBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_browse");
		createBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_identify");
		if (_changeable)
		{
			if (_isArmory)
			{
				GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.enchants.OnChange.AddListener(OnEnchantsChange);
			}
			else
			{
				GameData.instance.PROJECT.character.enchants.OnChange.AddListener(OnEnchantsChange);
			}
		}
		else
		{
			viewBtn.gameObject.SetActive(value: false);
			createBtn.gameObject.SetActive(value: false);
		}
		if (_isArmory)
		{
			viewBtn.gameObject.SetActive(value: false);
			createBtn.gameObject.SetActive(value: false);
		}
		CreateTiles();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		forceAnimation = true;
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && GameData.instance.PROJECT.character.tutorial.GetState(95) && !GameData.instance.PROJECT.character.tutorial.GetState(96))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(96);
			GameData.instance.tutorialManager.ShowTutorialForButton(createBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(96), 4, createBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
		}
	}

	private void OnEnchantsChange()
	{
		UpdateTiles();
	}

	private void CreateTiles()
	{
		_tiles = new Dictionary<int, EnchantTile>();
		CreateTile(0, placeholderSlotA);
		CreateTile(1, placeholderSlotB);
		CreateTile(2, placeholderSlotC);
		CreateTile(3, placeholderSlotD);
		CreateTile(4, placeholderSlotE);
		CreateTile(5, placeholderSlotF);
	}

	private void CreateTile(int slot, RectTransform placeholder)
	{
		Transform obj = Object.Instantiate(enchantTilePrefab);
		obj.SetParent(placeholder, worldPositionStays: false);
		EnchantTile component = obj.GetComponent<EnchantTile>();
		if (_isArmory)
		{
			if (_friendcharData == null)
			{
				component.LoadDetails(slot, GameData.instance.PROJECT.character.enchants, _enchants.getSlot(slot), _changeable, selectable: false, _isArmory);
			}
			else
			{
				EnchantData enchantData = null;
				if (slot < _enchants.slots.Count)
				{
					long num = _enchants.slots[slot];
					foreach (EnchantData enchant in _enchants.enchants)
					{
						if (enchant.uid == num)
						{
							enchantData = enchant;
							break;
						}
					}
				}
				component.LoadDetails(slot, _friendcharData.enchants, enchantData, _changeable, selectable: false, _isArmory);
			}
		}
		else
		{
			component.LoadDetails(slot, _enchants, _enchants.getSlot(slot), _changeable);
		}
		_tiles.Add(slot, component);
	}

	public void ShowEnchantSelectWindow(int slot)
	{
		if (!_changeable)
		{
			return;
		}
		if (!_isArmory)
		{
			if (_enchants.enchants.Count <= 0)
			{
				GameData.instance.windowGenerator.ShowError(Language.GetString("error_enchant_insert_unavailable"));
				return;
			}
		}
		else if (GameData.instance.PROJECT.character.enchants.enchants.Count <= 0)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_enchant_insert_unavailable"));
			return;
		}
		GameData.instance.windowGenerator.NewEnchantSelectWindow(_enchants, changeable: true, slot, base.gameObject, -1, _isArmory);
	}

	public void OnViewBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEnchantSelectWindow(GameData.instance.PROJECT.character.enchants, changeable: true, -1, base.gameObject, -1, _isArmory);
	}

	public void OnCreateBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (GameData.instance.PROJECT.character.enchants.enchants.Count >= VariableBook.enchantMax)
		{
			GameData.instance.windowGenerator.ShowErrorCode(112);
			return;
		}
		List<ItemData> itemsByType = GameData.instance.PROJECT.character.inventory.GetItemsByType(11);
		GameData.instance.windowGenerator.ShowItems(itemsByType, compare: false, added: true, Language.GetString("ui_identify"), large: true);
	}

	public void UpdateTiles()
	{
		if (_isArmory)
		{
			_enchants = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.enchants;
		}
		else
		{
			_enchants = GameData.instance.PROJECT.character.enchants;
		}
		ClearTiles();
	}

	public void ClearTiles()
	{
		foreach (KeyValuePair<int, EnchantTile> tile in _tiles)
		{
			Object.Destroy(tile.Value.gameObject);
		}
		CreateTiles();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		viewBtn.interactable = true;
		createBtn.interactable = true;
		if (_tiles == null)
		{
			return;
		}
		for (int i = 0; i < _tiles.Count; i++)
		{
			if (_tiles[i] != null)
			{
				_tiles[i].interactable = true;
			}
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		viewBtn.interactable = false;
		createBtn.interactable = false;
		if (_tiles == null)
		{
			return;
		}
		for (int i = 0; i < _tiles.Count; i++)
		{
			if (_tiles[i] != null)
			{
				_tiles[i].interactable = false;
			}
		}
	}

	private void OnDestroy()
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		if (_isArmory)
		{
			GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.enchants.OnChange.RemoveListener(OnEnchantsChange);
		}
		else
		{
			GameData.instance.PROJECT.character.enchants.OnChange.RemoveListener(OnEnchantsChange);
		}
	}

	public void OnChangeEnchantSelected(int slot, EnchantData enchantData)
	{
		DoEnchantEquip(slot, enchantData);
	}

	public void DoEnchantEquip(int slot, EnchantData enchantData)
	{
		if (_isArmory)
		{
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(51), OnArmoryEnchantEquip);
			CharacterDALC.instance.doArmoryEnchantEquip(slot, enchantData);
		}
		else
		{
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(30), OnEnchantEquip);
			CharacterDALC.instance.doEnchantEquip(slot, enchantData);
		}
	}

	private void OnArmoryEnchantEquip(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(51), OnArmoryEnchantEquip);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<long> enchantSlots = Util.arrayToNumberVector(sfsob.GetLongArray("ench12"));
		GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.enchants.setEnchantSlots(enchantSlots);
	}

	private void OnEnchantEquip(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(30), OnEnchantEquip);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<long> enchantSlots = Util.arrayToNumberVector(sfsob.GetLongArray("ench8"));
		GameData.instance.PROJECT.character.enchants.setEnchantSlots(enchantSlots);
	}
}
