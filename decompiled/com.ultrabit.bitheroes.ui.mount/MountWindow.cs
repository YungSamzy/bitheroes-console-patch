using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.ability;
using com.ultrabit.bitheroes.ui.game;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.tutorial;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.mount;

public class MountWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI rerollDescTxt;

	public TextMeshProUGUI costTxt;

	public TextMeshProUGUI rerollTxt;

	public TextMeshProUGUI txtPower;

	public TextMeshProUGUI txtStamina;

	public TextMeshProUGUI txtAgility;

	public GameObject modifierBase;

	public GameObject rerollItemBase;

	private List<GameObject> instancesCreated;

	public AbilityTile abilityTile;

	public Button helpBtn;

	public Button rerollBtn;

	public Button equipBtn;

	public Button unequipBtn;

	public Button destroyBtn;

	public Button upgradeBtn;

	public MountDataTile mountDataTile;

	public Transform costList;

	private MountData _mountData;

	private int _tier;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(MountData mountData, int tier)
	{
		_mountData = mountData;
		_tier = tier;
		_mountData.CHANGE.AddListener(OnMountChange);
		UpdateInformation();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && !GameData.instance.PROJECT.character.tutorial.GetState(52) && GameData.instance.PROJECT.character.mounts.getMountEquipped() != _mountData && equipBtn.gameObject.activeSelf)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(52);
			GameData.instance.tutorialManager.ShowTutorialForButton(equipBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(52), 4, equipBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	public void OnMountChange()
	{
		UpdateInformation();
	}

	private void UpdateInformation()
	{
		topperTxt.text = _mountData.mountRef.coloredName;
		rerollDescTxt.text = Language.GetString("ui_reroll_desc", new string[1] { ItemRef.GetItemName(8) });
		costTxt.text = Language.GetString("ui_cost") + ":";
		if (instancesCreated == null)
		{
			instancesCreated = new List<GameObject>();
		}
		else
		{
			foreach (GameObject item in instancesCreated)
			{
				if (!(item == null))
				{
					Object.Destroy(item);
				}
			}
		}
		instancesCreated.Clear();
		rerollTxt.text = Language.GetString("ui_reroll");
		if (_mountData.rerolls > 0)
		{
			TextMeshProUGUI textMeshProUGUI = rerollTxt;
			textMeshProUGUI.text = textMeshProUGUI.text + " (" + Util.NumberFormat(_mountData.rerolls) + ")";
		}
		MountData mountEquipped = GameData.instance.PROJECT.character.mounts.getMountEquipped();
		if (mountEquipped != null && mountEquipped.Equals(_mountData))
		{
			equipBtn.gameObject.SetActive(value: false);
			unequipBtn.gameObject.SetActive(value: true);
		}
		else
		{
			equipBtn.gameObject.SetActive(value: true);
			unequipBtn.gameObject.SetActive(value: false);
		}
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		rerollBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_reroll");
		equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_equip");
		unequipBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_unequip");
		destroyBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_release");
		upgradeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_upgrade");
		txtPower.text = Util.NumberFormat(_mountData.power);
		txtStamina.text = Util.NumberFormat(_mountData.stamina);
		txtAgility.text = Util.NumberFormat(_mountData.agility);
		CraftRerollRef craftRerollRef = (_mountData.mountRef.rerollable ? CraftBook.GetItemRerollRef(_mountData.mountRef) : null);
		if (craftRerollRef != null)
		{
			foreach (ItemData requiredItem in craftRerollRef.requiredItems)
			{
				GameObject gameObject = Object.Instantiate(rerollItemBase);
				gameObject.transform.SetParent(costList, worldPositionStays: false);
				gameObject.GetComponent<ItemCraftTile>().LoadDetails(requiredItem, locked: false, _mountData.rerolls + 1);
				instancesCreated.Add(gameObject);
			}
		}
		bool flag = craftRerollRef != null && craftRerollRef.RequirementsMet(_mountData.rerolls) && !_mountData.mountRef.isNFT;
		bool flag2 = _mountData.rank < _mountData.rankMax && !_mountData.mountRef.isNFT;
		bool flag3 = !_mountData.mountRef.isNFT;
		Util.SetButton(rerollBtn, flag);
		Util.SetButton(upgradeBtn, flag2);
		Util.SetButton(destroyBtn, flag3);
		foreach (GameModifier gameModifier in _mountData.getGameModifiers())
		{
			GameObject gameObject2 = Object.Instantiate(modifierBase);
			gameObject2.GetComponent<GameModifierBtn>().SetText(gameModifier.GetTileDesc());
			gameObject2.transform.parent = modifierBase.transform.parent;
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.SetActive(value: true);
			instancesCreated.Add(gameObject2);
		}
		bool flag4 = _mountData.mountRef.abilities != null && _mountData.mountRef.abilities.Count > 0;
		abilityTile.gameObject.SetActive(flag4);
		if (_mountData.mountRef.abilities != null && _mountData.mountRef.abilities.Count > 0)
		{
			abilityTile.LoadDetails(_mountData.mountRef.abilities[0], null);
			abilityTile.onOverridePointerEnter = delegate
			{
			};
			abilityTile.onOverridePointerExit = delegate
			{
			};
			abilityTile.onOverrideClick = delegate
			{
				GameData.instance.windowGenerator.NewAbilityListWindow(_mountData.mountRef.abilities, GameData.instance.PROJECT.character.getTotalPower(), GameModifier.getTypeTotal(GameData.instance.PROJECT.character.getModifiers(), 17));
			};
		}
		mountDataTile.SetMountDataTime(_mountData, _tier, flag4);
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		mountDataTile.SetLayer(layer);
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(ItemRef.GetItemNamePlural(8), Util.parseMultiLine(Language.GetString("mount_help_desc")), base.gameObject);
	}

	public void OnRerollBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (GameData.instance.PROJECT.dungeon != null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(109);
			return;
		}
		if (GameData.instance.PROJECT.battle != null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(110);
			return;
		}
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_reroll_confirm", new string[1] { Language.GetString(_mountData.mountRef.coloredName) }), null, null, delegate
		{
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(40), OnMountReroll);
			CharacterDALC.instance.doMountReroll(_mountData);
		});
	}

	private void OnMountReroll(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(40), OnMountReroll);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		MountData mountData = MountData.fromSFSObject(sfsob);
		List<ItemData> items = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite5"));
		GameData.instance.PROJECT.character.removeItems(items);
		GameData.instance.PROJECT.character.mounts.updateMount(mountData);
		_mountData = mountData;
		UpdateInformation();
		ItemListWindow itemListWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(ItemListWindow)) as ItemListWindow;
		if ((bool)itemListWindow)
		{
			itemListWindow.RemoveItems(items);
		}
		GameData.instance.audioManager.PlaySoundLink("exchange");
	}

	public void OnEquipBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoMountEquip(_mountData);
	}

	public void OnUnequipBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoMountEquip(null);
	}

	private void DoMountEquip(MountData mountData)
	{
		if (GameData.instance.PROJECT.dungeon != null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(109);
			return;
		}
		if (GameData.instance.PROJECT.battle != null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(110);
			return;
		}
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(38), OnMountEquip);
		CharacterDALC.instance.doMountEquip(mountData);
	}

	private void OnMountEquip(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(38), OnMountEquip);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		long @long = sfsob.GetLong("moun6");
		GameData.instance.PROJECT.character.mounts.setEquipped(GameData.instance.PROJECT.character.mounts.getMount(@long));
		if (GameData.instance.PROJECT.character.mounts.getMountEquippedUID() == _mountData.uid)
		{
			GameData.instance.audioManager.PlaySoundLink("equip");
		}
		else
		{
			GameData.instance.audioManager.PlaySoundLink("unequip");
		}
		UpdateInformation();
	}

	public void OnDestroyBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (GameData.instance.PROJECT.dungeon != null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(109);
			return;
		}
		if (GameData.instance.PROJECT.battle != null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(110);
			return;
		}
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_release_confirm", new string[1] { Language.GetString(_mountData.mountRef.coloredName) }), null, null, delegate
		{
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(39), OnMountDestroy);
			CharacterDALC.instance.doMountDestroy(_mountData);
		});
	}

	private void OnMountDestroy(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(39), OnMountDestroy);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		long @long = sfsob.GetLong("moun1");
		long long2 = sfsob.GetLong("moun6");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		GameData.instance.PROJECT.character.mounts.setEquipped(GameData.instance.PROJECT.character.mounts.getMount(long2));
		GameData.instance.PROJECT.character.mounts.removeMount(@long);
		if (list != null && list.Count > 0)
		{
			GameData.instance.PROJECT.character.addItems(list);
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true);
		}
		base.OnClose();
	}

	public void OnUpgradeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewMountUpgradeWindow(_mountData, base.gameObject);
	}

	public override void DoDestroy()
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		helpBtn.interactable = true;
		rerollBtn.interactable = true;
		equipBtn.interactable = true;
		unequipBtn.interactable = true;
		destroyBtn.interactable = true;
		upgradeBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		helpBtn.interactable = false;
		rerollBtn.interactable = false;
		equipBtn.interactable = false;
		unequipBtn.interactable = false;
		destroyBtn.interactable = false;
		upgradeBtn.interactable = false;
	}
}
