using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.instance;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.armory;

public class ArmoryManekin : MonoBehaviour
{
	public GameObject placeholderFirst;

	public GameObject placeholderSecond;

	public GameObject placeholderThird;

	private InstanceObject _instanceObject;

	private InstanceObjectRef _instanceObjRef;

	private CharacterDisplay _display;

	public void LoadDetails(InstanceObject instanceObject, InstanceObjectRef instanceObjRef)
	{
		_instanceObject = instanceObject;
		_instanceObjRef = instanceObjRef;
		Create();
	}

	public void Create(bool isUpdate = false)
	{
		CharacterData characterData;
		if (GameData.instance.PROJECT.playerData != null)
		{
			characterData = GameData.instance.PROJECT.playerData;
		}
		else
		{
			characterData = GameData.instance.PROJECT.character.toCharacterData();
			GameData.instance.PROJECT.playerData = characterData;
		}
		if (isUpdate && characterData.charID == GameData.instance.PROJECT.character.id)
		{
			characterData = GameData.instance.PROJECT.character.toCharacterData();
			GameData.instance.PROJECT.playerData = characterData;
		}
		List<ArmoryEquipment> armoryEquipmentSlots = characterData.armory.armoryEquipmentSlots;
		int num = int.Parse(_instanceObjRef.value);
		if (num >= armoryEquipmentSlots.Count || !armoryEquipmentSlots[num].unlocked)
		{
			placeholderFirst.gameObject.SetActive(value: false);
			return;
		}
		if (characterData.charID != GameData.instance.PROJECT.character.id && armoryEquipmentSlots[num].pprivate)
		{
			D.Log("nacho", "IS PRIVATE NOT SHOW " + armoryEquipmentSlots[num].name);
			placeholderFirst.gameObject.SetActive(value: false);
			return;
		}
		Equipment equipment = ArmoryEquipment.ArmoryEquipmentToEquipment(armoryEquipmentSlots[num]);
		if (equipment == null)
		{
			placeholderFirst.gameObject.SetActive(value: false);
			return;
		}
		characterData.setEquipment(equipment);
		if (_display != null)
		{
			Object.Destroy(_display.gameObject);
		}
		_display = characterData.toCharacterDisplay();
		_display.transform.SetParent(placeholderFirst.transform, worldPositionStays: false);
		_display.SetLocalPosition(new Vector3(3f, 3f, 0f));
		OnDisplayLoaded();
	}

	public CharacterDisplay GetDisplay()
	{
		return _display;
	}

	public InstanceObjectRef GetObjectRef()
	{
		return _instanceObjRef;
	}

	private void OnDisplayLoaded()
	{
		if (_instanceObject != null)
		{
			_instanceObject.AddCollidersAndHover();
		}
		_display.characterPuppet.PlayAnimation("idle");
	}
}
