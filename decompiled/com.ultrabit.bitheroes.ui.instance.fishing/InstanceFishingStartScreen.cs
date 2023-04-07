using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.bait;
using com.ultrabit.bitheroes.model.boober;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.instance.fishing;

public class InstanceFishingStartScreen : MonoBehaviour
{
	public Button startBtn;

	public Image tilesBorder;

	public RectTransform placeholderRod;

	public RectTransform placeholderBobber;

	public RectTransform placeholderBait;

	private InstanceFishingInterface _fishingInterface;

	private ItemIcon _rodTile;

	private ItemIcon _bobberTile;

	private ItemIcon _baitTile;

	public void LoadDetails(InstanceFishingInterface fishingInterface)
	{
		_fishingInterface = fishingInterface;
		startBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_start");
		CreateTiles();
		StartCoroutine(DelayCheckTutorial(0.1f));
	}

	public void OnStartBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoFishingCasting();
	}

	private IEnumerator DelayCheckTutorial(float delay)
	{
		yield return new WaitForSeconds(delay);
		CheckTutorial();
	}

	public void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && !GameData.instance.PROJECT.character.tutorial.GetState(54) && startBtn.enabled && startBtn.interactable)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(54);
			GameData.instance.tutorialManager.ShowTutorialForButton(startBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(54), 0, startBtn.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(0f, 170f)), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	public void OnForward()
	{
		if (!GameData.instance.tutorialManager.hasPopup && startBtn.interactable && startBtn.enabled)
		{
			DoFishingCasting();
		}
	}

	private void CreateTiles()
	{
		if (_rodTile != null)
		{
			Object.Destroy(_rodTile.gameObject);
			_rodTile = null;
		}
		if (_bobberTile != null)
		{
			Object.Destroy(_bobberTile.gameObject);
			_bobberTile = null;
		}
		if (_baitTile != null)
		{
			Object.Destroy(_baitTile.gameObject);
			_baitTile = null;
		}
		EquipmentRef fishingRod = GameData.instance.PROJECT.character.getFishingRod();
		BobberRef fishingBobber = GameData.instance.PROJECT.character.getFishingBobber();
		BaitRef fishingBait = GameData.instance.PROJECT.character.getFishingBait();
		Transform transform = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/item/" + typeof(ItemIcon).Name));
		transform.SetParent(placeholderRod, worldPositionStays: false);
		transform.transform.localPosition = Vector3.zero;
		_rodTile = transform.gameObject.AddComponent<ItemIcon>();
		_rodTile.SetEquipmentData(fishingRod, null, showComparision: false);
		_rodTile.SetItemActionType(10, DoRodSelect);
		Transform transform2 = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/item/" + typeof(ItemIcon).Name));
		transform2.SetParent(placeholderBobber, worldPositionStays: false);
		transform2.transform.localPosition = Vector3.zero;
		_bobberTile = transform2.gameObject.AddComponent<ItemIcon>();
		_bobberTile.SetItemData(new ItemData(fishingBobber, GameData.instance.PROJECT.character.getItemQty(fishingBobber)));
		_bobberTile.SetItemActionType(10, DoBobberSelect);
		Transform transform3 = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/item/" + typeof(ItemIcon).Name));
		transform3.SetParent(placeholderBait, worldPositionStays: false);
		transform3.transform.localPosition = Vector3.zero;
		_baitTile = transform3.gameObject.AddComponent<ItemIcon>();
		_baitTile.SetItemData(new ItemData(fishingBait, GameData.instance.PROJECT.character.getItemQty(fishingBait)));
		_baitTile.SetItemActionType(10, DoBaitSelect);
		if (fishingRod == null)
		{
			Util.SetButton(_rodTile, enabled: false);
		}
		if (fishingBobber == null)
		{
			Util.SetButton(_bobberTile, enabled: false);
		}
		if (fishingBait == null)
		{
			Util.SetButton(_baitTile, enabled: false);
		}
		if (!GetAvailable())
		{
			Util.SetButton(startBtn, enabled: false);
		}
		else
		{
			Util.SetButton(startBtn);
		}
	}

	private void DoRodSelect(BaseModelData data)
	{
		ItemListWindow itemListWindow = GameData.instance.windowGenerator.NewItemListWindow(GameData.instance.PROJECT.character.getFishingRods(), compare: false, added: false, Language.GetString("ui_select"), large: false, forceNonEquipment: true, select: true);
		itemListWindow.DESTROYED.AddListener(OnRodClose);
		itemListWindow.SELECT.AddListener(OnRodSelect);
	}

	private void OnRodClose(object e)
	{
		ItemListWindow obj = e as ItemListWindow;
		obj.DESTROYED.RemoveListener(OnRodClose);
		obj.SELECT.RemoveListener(OnRodSelect);
	}

	private void OnRodSelect(object e)
	{
		object[] obj = e as object[];
		ItemListWindow itemListWindow = obj[0] as ItemListWindow;
		EquipmentRef equipmentRef = obj[1] as EquipmentRef;
		itemListWindow.OnClose();
		GameData.instance.SAVE_STATE.SetFishingRod(GameData.instance.PROJECT.character.id, equipmentRef.id);
		_fishingInterface.SendFishingStart();
		CreateTiles();
	}

	private void DoBobberSelect(BaseModelData data)
	{
		ItemListWindow itemListWindow = GameData.instance.windowGenerator.NewItemListWindow(GameData.instance.PROJECT.character.inventory.GetItemsByType(14), compare: false, added: false, Language.GetString("ui_select"), large: false, forceNonEquipment: true, select: true);
		itemListWindow.DESTROYED.AddListener(OnBobberClose);
		itemListWindow.SELECT.AddListener(OnBobberSelect);
	}

	private void OnBobberClose(object e)
	{
		ItemListWindow obj = e as ItemListWindow;
		obj.DESTROYED.RemoveListener(OnBobberClose);
		obj.SELECT.RemoveListener(OnBobberSelect);
	}

	private void OnBobberSelect(object e)
	{
		object[] obj = e as object[];
		ItemListWindow itemListWindow = obj[0] as ItemListWindow;
		BobberRef bobberRef = obj[1] as BobberRef;
		itemListWindow.OnClose();
		GameData.instance.SAVE_STATE.SetFishingBobber(GameData.instance.PROJECT.character.id, bobberRef.id);
		_fishingInterface.SendFishingStart();
		CreateTiles();
	}

	private void DoBaitSelect(BaseModelData data)
	{
		ItemListWindow itemListWindow = GameData.instance.windowGenerator.NewItemListWindow(GameData.instance.PROJECT.character.inventory.GetItemsByType(13), compare: false, added: false, Language.GetString("ui_select"), large: false, forceNonEquipment: true, select: true);
		itemListWindow.DESTROYED.AddListener(OnBaitClose);
		itemListWindow.SELECT.AddListener(OnBaitSelect);
	}

	private void OnBaitClose(object e)
	{
		ItemListWindow obj = e as ItemListWindow;
		obj.DESTROYED.RemoveListener(OnBaitClose);
		obj.SELECT.RemoveListener(OnBaitSelect);
	}

	private void OnBaitSelect(object e)
	{
		object[] obj = e as object[];
		ItemListWindow itemListWindow = obj[0] as ItemListWindow;
		BaitRef baitRef = obj[1] as BaitRef;
		itemListWindow.OnClose();
		GameData.instance.SAVE_STATE.SetFishingBait(GameData.instance.PROJECT.character.id, baitRef.id);
		_fishingInterface.SendFishingStart();
		CreateTiles();
	}

	private bool GetAvailable()
	{
		EquipmentRef fishingRod = GameData.instance.PROJECT.character.getFishingRod();
		BobberRef fishingBobber = GameData.instance.PROJECT.character.getFishingBobber();
		BaitRef fishingBait = GameData.instance.PROJECT.character.getFishingBait();
		if (fishingRod != null && fishingBobber != null && fishingBait != null)
		{
			return FishingEventBook.GetCurrentEventRef() != null;
		}
		return false;
	}

	private void DoFishingCasting()
	{
		_fishingInterface.DoFishingCasting();
	}
}
