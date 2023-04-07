using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.instance.fishing;

public class InstanceFishingCatchingScreen : MonoBehaviour
{
	private const float DISABLE_ALPHA = 0.4f;

	public TextMeshProUGUI percTxt;

	public Image percBg;

	public Button catchBtn;

	public Image barBorder;

	public RectTransform placeholderPanel;

	public RectTransform placeholderFish;

	public RectTransform placeholderBar;

	public TimeBarColor _timer;

	public RectTransform placeholderTime;

	private InstanceFishingInterface _fishingInterface;

	private FishingItemRef _itemRef;

	private InstanceFishingCatchingBarTab savedTab;

	private InstanceFishingCatchingFish _fish;

	private InstanceFishingCatchingBar _bar;

	private bool _catching;

	private bool doUpdate;

	private float time;

	public void LoadDetails(InstanceFishingInterface fishingInterface, FishingItemRef itemRef)
	{
		_fishingInterface = fishingInterface;
		_itemRef = itemRef;
		time = _itemRef.time;
		catchBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_catch");
		CreateObjects();
		_timer.SetShowDescription(show: false);
		_timer.SetMaxValueSeconds(_itemRef.time);
		_timer.SetCurrentValueSeconds(time + 1f);
		_timer.COMPLETE.AddListener(OnTimerComplete);
		UpdateTab();
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.AddListener(OnUpdate);
		StartCoroutine(DelayCheckTutorial(0.1f));
	}

	public void OnCatchBtn()
	{
		if (catchBtn.enabled)
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			DoCatch();
		}
	}

	private void OnTimerComplete()
	{
		_timer.COMPLETE.RemoveListener(OnTimerComplete);
		DoCatchSend();
	}

	private IEnumerator DelayCheckTutorial(float delay)
	{
		yield return new WaitForSeconds(delay);
		CheckTutorial();
	}

	public void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && !GameData.instance.PROJECT.character.tutorial.GetState(56) && catchBtn.enabled && catchBtn.interactable)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(56);
			GameData.instance.tutorialManager.ShowTutorialForEventTrigger(catchBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(56), 0, catchBtn.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(0f, 180f)), EventTriggerType.PointerClick, stageTrigger: true, null, funcSameAsTargetFunc: false, OnCatchEventTrigger, shadow: false, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	private void OnCatchEventTrigger(object arg0)
	{
		OnCatchBtn();
	}

	public void OnForward()
	{
		if (!GameData.instance.tutorialManager.hasPopup && catchBtn.enabled && catchBtn.interactable)
		{
			DoCatch();
		}
	}

	private void OnUpdate(object e)
	{
		UpdatePosition(e);
	}

	public void UpdatePosition(object e)
	{
		_bar.UpdatePosition(e);
		UpdateTab();
	}

	public void UpdateTab()
	{
		InstanceFishingCatchingBarTab instanceFishingCatchingBarTab = (savedTab = GetCurrentTab());
		foreach (InstanceFishingCatchingBarTab tab in _bar.tabs)
		{
			tab.tab.color = new Color(tab.tab.color.r, tab.tab.color.g, tab.tab.color.b, (tab == instanceFishingCatchingBarTab) ? 1f : 0.4f);
		}
		percTxt.text = (instanceFishingCatchingBarTab ? instanceFishingCatchingBarTab.chanceRef.getPercString() : Util.colorString("0", BattleText.COLOR_RED)) + "%";
	}

	private InstanceFishingCatchingBarTab GetCurrentTab()
	{
		return _bar.GetTabByPosition(_fish.transform.position.x);
	}

	private int GetCurrentOffset()
	{
		return (int)(_bar.GetComponent<RectTransform>().anchoredPosition.x - _fish.GetComponent<RectTransform>().anchoredPosition.x);
	}

	private void CreateObjects()
	{
		if (!(_bar != null) && !(_fish != null))
		{
			float randomPosition = GetRandomPosition();
			Transform transform = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/instance/fishing/" + typeof(InstanceFishingCatchingBar).Name));
			transform.SetParent(placeholderPanel, worldPositionStays: false);
			transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(placeholderBar.GetComponent<RectTransform>().anchoredPosition.x - placeholderPanel.GetComponent<RectTransform>().anchoredPosition.x + randomPosition, 0f - (placeholderBar.GetComponent<RectTransform>().anchoredPosition.y - placeholderPanel.GetComponent<RectTransform>().anchoredPosition.y));
			_bar = transform.GetComponent<InstanceFishingCatchingBar>();
			_bar.LoadDetails(_itemRef, _fishingInterface.GetPlayer(), placeholderPanel.GetComponent<RectTransform>().sizeDelta.x, this);
			randomPosition *= -1f;
			transform = null;
			transform = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/instance/fishing/" + typeof(InstanceFishingCatchingFish).Name));
			transform.SetParent(placeholderPanel, worldPositionStays: false);
			transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(placeholderFish.GetComponent<RectTransform>().anchoredPosition.x - placeholderPanel.GetComponent<RectTransform>().anchoredPosition.x + randomPosition, 0f - (placeholderFish.GetComponent<RectTransform>().anchoredPosition.y - placeholderPanel.GetComponent<RectTransform>().anchoredPosition.y));
			_fish = transform.GetComponent<InstanceFishingCatchingFish>();
			_fish.LoadDetails(_itemRef, placeholderPanel.GetComponent<RectTransform>().sizeDelta.x);
			_fish.COMPLETE.AddListener(OnFishComplete);
		}
	}

	private void OnFishComplete()
	{
		InstanceFishingCatchingBarTab instanceFishingCatchingBarTab = savedTab;
		DoCatchSend(instanceFishingCatchingBarTab ? instanceFishingCatchingBarTab.chanceRef : null);
	}

	private void DoCatchSend(FishingBarChanceRef chanceRef = null)
	{
		GameData.instance.tutorialManager.ClearTutorial();
		StopMovement();
		_fishingInterface.DoCatchSend(chanceRef);
	}

	private void SetCatching(bool v)
	{
		_catching = v;
		Util.SetButton(catchBtn, !_catching);
	}

	private void DoCatch()
	{
		SetCatching(v: true);
		_timer.CancelInvoke("UpdateSeconds");
		if ((bool)GetCurrentTab())
		{
			_bar.SetFocus(_fish.gameObject, GetCurrentOffset());
			_fish.StartCapture();
		}
		else
		{
			StopMovement();
			DoCatchSend();
		}
	}

	private void StopMovement()
	{
		SetCatching(v: true);
		_fish.StopMovement();
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnUpdate);
	}

	private float GetRandomPosition()
	{
		float num = placeholderPanel.GetComponent<RectTransform>().sizeDelta.x / 4f;
		return (float)Util.randomInt((int)(0f - num), (int)num) / GetComponent<RectTransform>().localScale.x;
	}

	private void OnDestroy()
	{
		if (_fish != null)
		{
			_fish.COMPLETE.RemoveListener(OnFishComplete);
		}
		if (_timer != null)
		{
			_timer.COMPLETE.RemoveListener(OnTimerComplete);
		}
		StopMovement();
	}
}
