using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.bait;
using com.ultrabit.bitheroes.model.boober;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.instance.fishing;

public class InstanceFishingInterface : WindowsMain
{
	public TextMeshProUGUI nameTxt;

	public Button eventBtn;

	public Image border;

	public Image nameplate;

	private Instance _instance;

	private InstanceObject _instanceObject;

	private bool _enabled = true;

	private InstanceFishingStartScreen _startScreen;

	private InstanceFishingCastingScreen _castingScreen;

	private InstanceFishingCatchingScreen _catchingScreen;

	public Instance instance => _instance;

	public InstanceObject instanceObject => _instanceObject;

	public void LoadDetails(Instance instance, InstanceObject instanceObject)
	{
		Disable();
		_instance = instance;
		_instanceObject = instanceObject;
		nameTxt.text = Language.GetString("ui_fishing");
		nameTxt.ForceMeshUpdate();
		StartCoroutine(WaitToFixText());
		Vector2 sizeDelta = GameData.instance.windowGenerator.canvas.GetComponent<RectTransform>().sizeDelta;
		border.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x / 3f, sizeDelta.y / 3f);
		ListenForBack(OnBack);
		ListenForForward(OnForward);
		DoFishingStart();
		CreateWindow(closeWord: false, "", scroll: false);
		Enable();
	}

	private IEnumerator WaitToFixText()
	{
		yield return new WaitForEndOfFrame();
		if (nameplate.GetComponent<RectTransform>().sizeDelta.x > 93f)
		{
			nameplate.GetComponent<HorizontalLayoutGroup>().childControlWidth = false;
			nameTxt.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60f, nameTxt.gameObject.GetComponent<RectTransform>().sizeDelta.y);
		}
	}

	public void OnEventBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewFishingEventWindow();
	}

	public override void OnClose()
	{
		DoFishingEnd();
	}

	private void OnBack()
	{
		if (_enabled && closeBtn.enabled)
		{
			DoFishingEnd();
		}
	}

	public void OnForward()
	{
		if (_startScreen != null)
		{
			_startScreen.OnForward();
		}
		else if (_castingScreen != null)
		{
			_castingScreen.OnForward();
		}
		else if (_catchingScreen != null)
		{
			_catchingScreen.OnForward();
		}
	}

	public void DoFishingStart()
	{
		if (!(_startScreen != null))
		{
			InstancePlayer player = GetPlayer();
			if (!(player == null))
			{
				SetButtons(enabled: true);
				player.SetFlipped(Util.parseBoolean(_instanceObject.objectRef.value));
				SendFishingStart();
				Transform transform = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/instance/fishing/" + typeof(InstanceFishingStartScreen).Name));
				transform.SetParent(base.transform, worldPositionStays: false);
				_startScreen = transform.GetComponent<InstanceFishingStartScreen>();
				_startScreen.LoadDetails(this);
			}
		}
	}

	public void SendFishingStart()
	{
		InstancePlayer player = GetPlayer();
		if (!(player == null))
		{
			EquipmentRef fishingRod = GameData.instance.PROJECT.character.getFishingRod();
			BobberRef fishingBobber = GameData.instance.PROJECT.character.getFishingBobber();
			BaitRef fishingBait = GameData.instance.PROJECT.character.getFishingBait();
			player.SetFishingData(new InstanceFishingData(fishingRod, fishingBobber, fishingBait, 0));
			_instance.extension.DoFishingStart(player.tile.id, fishingRod, fishingBobber, fishingBait, player.flipped);
		}
	}

	public void DoFishingCasting()
	{
		if (!(_startScreen == null) && !(_castingScreen != null))
		{
			InstancePlayer player = GetPlayer();
			if (!(player == null))
			{
				Object.Destroy(_startScreen.gameObject);
				_startScreen = null;
				player.fishingData.setState(2);
				player.UpdateAnimation();
				_instance.extension.DoFishingCasting();
				Transform transform = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/instance/fishing/" + typeof(InstanceFishingCastingScreen).Name));
				transform.SetParent(base.transform, worldPositionStays: false);
				_castingScreen = transform.GetComponent<InstanceFishingCastingScreen>();
				_castingScreen.LoadDetails(this);
			}
		}
	}

	public void DoFishingCatching()
	{
		InstancePlayer player = GetPlayer();
		if (!(player == null))
		{
			ItemData itemData = new ItemData(player.fishingData.baitRef, 1);
			GameData.instance.PROJECT.character.removeItem(itemData);
			player.BOBBER_COMPLETE.AddListener(OnFishingBobberLanded);
		}
	}

	private void OnFishingBobberLanded(object e)
	{
		InstancePlayer player = GetPlayer();
		if (!(player == null))
		{
			player.BOBBER_COMPLETE.RemoveListener(OnFishingBobberLanded);
			StartCoroutine(DelayFishingCatching(Util.randomInt(3, 4)));
		}
	}

	private IEnumerator DelayFishingCatching(int delay)
	{
		yield return new WaitForSeconds(delay);
		OnFishingCatching();
	}

	private void OnFishingCatching()
	{
		InstancePlayer player = GetPlayer();
		if (player == null)
		{
			return;
		}
		if (player.fishingData.itemRef.barRef != null)
		{
			if ((bool)player.bobber)
			{
				if (player.bobber.asset != null)
				{
					Util.Shake(player.bobber.asset);
				}
				player.bobber.SetExclamation(enabled: true);
			}
			StartCoroutine(DelayExtensionFishingCatching(1));
		}
		else
		{
			StopCasting();
			DoCatchSend();
		}
	}

	private IEnumerator DelayExtensionFishingCatching(int delay)
	{
		yield return new WaitForSeconds(delay);
		_instance.extension.DoFishingCatching();
	}

	public void DoCatchSend(FishingBarChanceRef chanceRef = null)
	{
		_instance.extension.DoFishingCatch(chanceRef);
	}

	private void StopCasting()
	{
		if (_castingScreen != null)
		{
			Object.Destroy(_castingScreen.gameObject);
			_castingScreen = null;
		}
		_instance.SetFocus(GetPlayer());
	}

	public void DoFishingCatchStart()
	{
		if (_castingScreen == null || _catchingScreen != null)
		{
			return;
		}
		InstancePlayer player = GetPlayer();
		if (!(player == null))
		{
			StopCasting();
			if (player.fishingData.itemRef.barRef != null)
			{
				Transform transform = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/instance/fishing/" + typeof(InstanceFishingCatchingScreen).Name));
				transform.SetParent(base.transform, worldPositionStays: false);
				_catchingScreen = transform.GetComponent<InstanceFishingCatchingScreen>();
				_catchingScreen.LoadDetails(this, player.fishingData.itemRef);
			}
			else
			{
				DoCatchSend();
			}
		}
	}

	public void DoFishingCatchComplete(FishingItemRef itemRef, List<ItemData> items, int weight, bool success)
	{
		if (_catchingScreen != null)
		{
			Object.Destroy(_catchingScreen.gameObject);
			_catchingScreen = null;
		}
		SetButtons(enabled: true);
		if (success)
		{
			GameData.instance.windowGenerator.NewFishingCaptureWindow(itemRef, items, weight);
		}
		else
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("battle_capture_failed"), Language.GetString("fishing_failed_desc"));
		}
		DoFishingStart();
	}

	public void DoFishingEnd()
	{
		InstancePlayer player = GetPlayer();
		if (!(player == null))
		{
			player.SetFishingData(null);
			_instance.extension.DoFishingEnd();
			_instance.SetFocus(player);
			player.ClearBobber();
			GameData.instance.PROJECT.ToggleFishingMode();
		}
	}

	public void SetButtons(bool enabled)
	{
		_enabled = enabled;
		Util.SetButton(closeBtn, enabled);
		Util.SetButton(eventBtn, enabled);
	}

	private void ShowInventory()
	{
		List<ItemData> fishingRods = GameData.instance.PROJECT.character.getFishingRods();
		List<ItemData> itemsByType = GameData.instance.PROJECT.character.inventory.GetItemsByType(14);
		List<ItemData> itemsByType2 = GameData.instance.PROJECT.character.inventory.GetItemsByType(13);
		List<ItemData> list = new List<ItemData>();
		foreach (ItemData item in fishingRods)
		{
			if (item.qty > 0)
			{
				list.Add(item);
			}
		}
		foreach (ItemData item2 in itemsByType)
		{
			if (item2.qty > 0)
			{
				list.Add(item2);
			}
		}
		foreach (ItemData item3 in itemsByType2)
		{
			if (item3.qty > 0)
			{
				list.Add(item3);
			}
		}
		GameData.instance.windowGenerator.ShowItems(list, compare: false, added: false, Language.GetString("ui_inventory"));
	}

	public InstancePlayer GetPlayer()
	{
		return _instance.GetPlayer(GameData.instance.PROJECT.character.id);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		eventBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		eventBtn.interactable = false;
	}
}
