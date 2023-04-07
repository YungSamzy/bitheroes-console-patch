using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.ability;
using com.ultrabit.bitheroes.ui.game;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.mount;

public class ArmoryMountWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button destroyBtn;

	public Button equipBtn;

	public Button unequipBtn;

	public Button helpBtn;

	public MountDataTile mountDataTile;

	public TextMeshProUGUI txtPower;

	public TextMeshProUGUI txtStamina;

	public TextMeshProUGUI txtAgility;

	public AbilityTile abilityTile;

	public GameObject modifierBase;

	private List<GameObject> instancesCreated;

	private MountData _mountData;

	private int _tier;

	public override void Start()
	{
		base.Start();
		Disable();
		destroyBtn.gameObject.SetActive(value: false);
	}

	public void LoadDetails(MountData mountData, int tier)
	{
		_mountData = mountData;
		_tier = tier;
		UpdateInformation();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
	}

	private void UpdateInformation()
	{
		topperTxt.text = _mountData.mountRef.coloredName;
		if (instancesCreated == null)
		{
			instancesCreated = new List<GameObject>();
		}
		else
		{
			foreach (GameObject item in instancesCreated)
			{
				Object.Destroy(item);
			}
		}
		instancesCreated.Clear();
		MountData mountDataEquipped = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.GetMountDataEquipped();
		if (mountDataEquipped != null && mountDataEquipped.Equals(_mountData))
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
		equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_equip");
		unequipBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_unequip");
		destroyBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_release");
		txtPower.text = Util.NumberFormat(_mountData.power);
		txtStamina.text = Util.NumberFormat(_mountData.stamina);
		txtAgility.text = Util.NumberFormat(_mountData.agility);
		foreach (GameModifier gameModifier in _mountData.getGameModifiers())
		{
			GameObject gameObject = Object.Instantiate(modifierBase);
			gameObject.GetComponent<GameModifierBtn>().SetText(gameModifier.GetTileDesc());
			gameObject.transform.parent = modifierBase.transform.parent;
			gameObject.transform.localScale = Vector3.one;
			gameObject.SetActive(value: true);
			instancesCreated.Add(gameObject);
		}
		bool flag = _mountData.mountRef.abilities != null && _mountData.mountRef.abilities.Count > 0;
		abilityTile.gameObject.SetActive(flag);
		if (flag)
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
		mountDataTile.SetMountDataTime(_mountData, _tier, flag);
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		mountDataTile.SetLayer(layer);
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
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(48), OnMountEquip);
		CharacterDALC.instance.doArmoryMountEquip(mountData);
	}

	private void OnMountEquip(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(48), OnMountEquip);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		long mount = sfsob.GetInt("mount");
		GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.mount = mount;
		MountData mountDataEquipped = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.GetMountDataEquipped();
		if (mountDataEquipped != null && mountDataEquipped.uid == _mountData.uid)
		{
			GameData.instance.audioManager.PlaySoundLink("equip");
		}
		else
		{
			GameData.instance.audioManager.PlaySoundLink("unequip");
		}
		UpdateInformation();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		destroyBtn.interactable = true;
		equipBtn.interactable = true;
		unequipBtn.interactable = true;
		helpBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		destroyBtn.interactable = false;
		equipBtn.interactable = false;
		unequipBtn.interactable = false;
		helpBtn.interactable = false;
	}
}
