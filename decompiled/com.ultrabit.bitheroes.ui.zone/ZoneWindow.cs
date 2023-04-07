using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.tutorial;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.zone;

public class ZoneWindow : WindowsMain
{
	private const int ID_Z1F12 = 12;

	public Transform questMap;

	public Button previousBtn;

	public Button nextBtn;

	public Button zonesBtn;

	private GameObject currentZoneGO;

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	private ZoneRef _zoneRef;

	private Transform window;

	private bool _defeated;

	public TextMeshProUGUI txtZoneName;

	public TextMeshProUGUI txtStars;

	private bool _complete;

	private bool _checkForTween;

	private bool zoneBtnAnim;

	private ZonePanel _zonePanel;

	private bool _isOnTween;

	public bool isOnTween => _isOnTween;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails()
	{
		ZoneRef zoneRef = GameData.instance.PROJECT.character.getZoneRef();
		if (GameData.instance.lastNode == null)
		{
			for (int i = 0; i < zoneRef.nodes.Count; i++)
			{
				if (GameData.instance.PROJECT.character.zones.nodeIsUnlocked(zoneRef.nodes[i]))
				{
					GameData.instance.lastNode = zoneRef.nodes[i];
				}
			}
		}
		CreateWindow();
		SetZoneRef(zoneRef);
		zonesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_zones");
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
	}

	public void SetDefeated(bool defeated)
	{
		_defeated = defeated;
	}

	private void SetZoneRef(ZoneRef zoneRef)
	{
		if (zoneRef != null)
		{
			_zoneRef = zoneRef;
			_complete = GameData.instance.PROJECT.character.zones.zoneIsCompleted(zoneRef);
			if (GameData.instance.PROJECT.character.zoneID != zoneRef.id)
			{
				GameData.instance.PROJECT.character.zoneID = zoneRef.id;
				CharacterDALC.instance.doSaveZoneID(zoneRef.id);
			}
			UpdateZoneView();
			UpdateButtons();
		}
	}

	private void UpdateButtons()
	{
		ZoneRef previousZone = GetPreviousZone();
		ZoneRef nextZone = GetNextZone();
		if (previousZone != null)
		{
			previousBtn.enabled = true;
			previousBtn.image.color = Color.white;
		}
		else
		{
			previousBtn.enabled = false;
			previousBtn.image.color = alpha;
		}
		if (nextZone != null)
		{
			nextBtn.enabled = true;
			nextBtn.image.color = Color.white;
		}
		else
		{
			nextBtn.enabled = false;
			nextBtn.image.color = alpha;
		}
		bool active = GameData.instance.PROJECT.character.zones.getZoneCompleteCount() > 0;
		zonesBtn.gameObject.SetActive(active);
	}

	public void UpdateZoneView()
	{
		if (currentZoneGO != null)
		{
			Object.Destroy(currentZoneGO);
		}
		txtZoneName.text = _zoneRef.name;
		txtStars.text = GameData.instance.PROJECT.character.zones.getZoneStars(_zoneRef) + "/" + _zoneRef.getStarCount();
		currentZoneGO = GameData.instance.windowGenerator.UpdateWindowZone(questMap, _zoneRef.id);
		if (!(currentZoneGO == null))
		{
			_zonePanel = currentZoneGO.AddComponent<ZonePanel>();
			_zonePanel.LoadDetails(_zoneRef, this);
			if (!base.scrollingIn && !base.scrollingOut)
			{
				CheckChanges();
			}
		}
	}

	public void OnPreviousBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetZoneRef(GetPreviousZone());
	}

	public void OnNextBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetZoneRef(GetNextZone());
		CheckZonesBtn();
	}

	public void OnZonesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_zones"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _zoneRef.id, OnZoneSelected);
		componentInChildren.ClearList();
		GameData.instance.PROJECT.character.zones.getHighestCompletedZoneID();
		for (int i = 0; i <= ZoneBook.size; i++)
		{
			ZoneRef zoneRef = ZoneBook.Lookup(i);
			if (zoneRef != null)
			{
				bool locked = false;
				componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
				{
					id = zoneRef.id,
					title = zoneRef.name,
					locked = locked
				});
			}
		}
		componentInChildren.ScrollTo(_zoneRef.id - 1, 0f, -1f);
	}

	private void OnZoneSelected(MyDropdownItemModel model)
	{
		SetZoneRef(ZoneBook.Lookup(model.id));
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public void CheckChanges()
	{
		bool complete = _complete;
		bool flag = GameData.instance.PROJECT.character.zones.zoneIsCompleted(_zoneRef);
		bool flag2 = !complete && flag;
		_complete = flag;
		if (flag2)
		{
			GameData.instance.PROJECT.character.updateAchievements();
		}
		if (!CheckNotifications(flag2))
		{
			CheckTutorialAndDefeated();
		}
	}

	public bool CheckNotifications(bool completed)
	{
		if (GameData.instance.tutorialManager.hasPopup || _zoneRef == null)
		{
			return false;
		}
		if (completed)
		{
			ZoneNotification notificationByType = _zoneRef.getNotificationByType(1);
			if (notificationByType != null)
			{
				notificationByType.DoNotification().DESTROYED.AddListener(OnNotificationClose);
				return true;
			}
		}
		return false;
	}

	private void OnNotificationClose(object e)
	{
		(e as DialogWindow).DESTROYED.RemoveListener(OnNotificationClose);
		CheckChanges();
	}

	public void CheckTutorialAndDefeated()
	{
		CheckTutorial();
		if (!_isOnTween)
		{
			SetDefeated(defeated: false);
		}
	}

	private bool CheckTutorial()
	{
		_isOnTween = false;
		if (GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null)
		{
			return false;
		}
		foreach (ZoneNode node in _zonePanel.nodes)
		{
			if (node.onAnimation)
			{
				_isOnTween = true;
				return false;
			}
		}
		UpdateButtons();
		if (!GameData.instance.PROJECT.character.tutorial.GetState(3))
		{
			ZoneNode firstNode = GetFirstAvailableNode();
			if (firstNode != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(3);
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(firstNode.gameObject, new TutorialPopUpSettings(Tutorial.GetText(3), 4, firstNode.gameObject), EventTriggerType.PointerClick, stageTrigger: false, null, funcSameAsTargetFunc: false, delegate
				{
					firstNode.OnPointerClick(null);
				}, shadow: true, tween: true, OnTutorialClose);
				return true;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(5))
		{
			ZoneNode lastAvailableNode = GetLastAvailableNode();
			if (lastAvailableNode != null)
			{
				ZoneNodeDifficultyRef firstDifficultyRef = lastAvailableNode.zoneNodeRef.getFirstDifficultyRef();
				if (firstDifficultyRef != null && firstDifficultyRef.teamRules.allowFamiliars && firstDifficultyRef.teamRules.slots > 1)
				{
					GameData.instance.PROJECT.character.tutorial.SetState(5);
					GameData.instance.tutorialManager.ShowTutorialForEventTrigger(lastAvailableNode.gameObject, new TutorialPopUpSettings(Tutorial.GetText(5), 4, lastAvailableNode.gameObject), EventTriggerType.PointerClick, stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: false, delegate(object e)
					{
						GameData.instance.tutorialManager.ClearTutorial();
						OnTutorialClose(e);
					});
					GameData.instance.PROJECT.CheckTutorialChanges();
					return true;
				}
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(6))
		{
			ZoneNode lastAvailableNode2 = GetLastAvailableNode();
			if (lastAvailableNode2 != null)
			{
				ZoneNodeDifficultyRef firstDifficultyRef2 = lastAvailableNode2.zoneNodeRef.getFirstDifficultyRef();
				if (firstDifficultyRef2 != null && firstDifficultyRef2.teamRules.allowFriends && firstDifficultyRef2.teamRules.allowGuildmates && firstDifficultyRef2.teamRules.slots > 1)
				{
					GameData.instance.PROJECT.character.tutorial.SetState(6);
					GameData.instance.tutorialManager.ShowTutorialForEventTrigger(lastAvailableNode2.gameObject, new TutorialPopUpSettings(Tutorial.GetText(6), 4, lastAvailableNode2.gameObject), EventTriggerType.PointerClick, stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: false, delegate(object e)
					{
						GameData.instance.tutorialManager.ClearTutorial();
						OnTutorialClose(e);
					});
					GameData.instance.PROJECT.CheckTutorialChanges();
					return true;
				}
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(46) && _defeated)
		{
			ZoneNode firstRepeatableNode = GetFirstRepeatableNode();
			if (firstRepeatableNode != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(46);
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(firstRepeatableNode.gameObject, new TutorialPopUpSettings(Tutorial.GetText(46), 4, firstRepeatableNode.gameObject), EventTriggerType.PointerClick, stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: false, delegate(object e)
				{
					GameData.instance.tutorialManager.ClearTutorial();
					OnTutorialClose(e);
				});
				GameData.instance.PROJECT.CheckTutorialChanges();
				return true;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(43) && GetNextZone() != null && GameData.instance.PROJECT.character.zones.zoneIsUnlocked(GetNextZone()))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(43);
			GameData.instance.tutorialManager.ShowTutorialForButton(nextBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(43), 2, nextBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true, OnTutorialClose);
			GameData.instance.PROJECT.CheckTutorialChanges();
			return true;
		}
		return false;
	}

	private void OnTutorialClose(object e)
	{
	}

	private void OnScrollInComplete(object e)
	{
		CheckChanges();
		CheckZonesBtn();
	}

	private void CheckZonesBtn()
	{
		if (GameData.instance.PROJECT.character.zones.getZoneCompleteCount() > 0 && !GameData.instance.SAVE_STATE.zonesBtnAnimation)
		{
			GameData.instance.SAVE_STATE.zonesBtnAnimation = true;
			AnimateZonesBtn();
		}
	}

	public override void OnClose()
	{
		CancelInvoke("CheckChanges");
		base.OnClose();
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
		if (previousBtn != null)
		{
			previousBtn.interactable = true;
		}
		if (nextBtn != null)
		{
			nextBtn.interactable = true;
		}
		if (zonesBtn != null)
		{
			zonesBtn.interactable = true;
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		previousBtn.interactable = false;
		nextBtn.interactable = false;
		zonesBtn.interactable = false;
	}

	private ZoneRef GetPreviousZone()
	{
		if (_zoneRef == null)
		{
			return null;
		}
		if (_zoneRef.requiredZones.Count <= 0)
		{
			return null;
		}
		foreach (int requiredZone in _zoneRef.requiredZones)
		{
			ZoneRef zoneRef = ZoneBook.Lookup(requiredZone);
			if (zoneRef != null)
			{
				return zoneRef;
			}
		}
		return null;
	}

	private ZoneRef GetNextZone()
	{
		if (_zoneRef == null)
		{
			return null;
		}
		ZoneRef zoneRef = ZoneBook.Lookup(_zoneRef.id + 1);
		if (zoneRef == null)
		{
			return null;
		}
		foreach (int requiredZone in zoneRef.requiredZones)
		{
			if (ZoneBook.Lookup(requiredZone).id == _zoneRef.id)
			{
				return zoneRef;
			}
		}
		return null;
	}

	private ZoneNode GetFirstAvailableNode()
	{
		ZoneNode[] componentsInChildren = questMap.GetComponentsInChildren<ZoneNode>();
		foreach (ZoneNode zoneNode in componentsInChildren)
		{
			if (!GameData.instance.PROJECT.character.zones.nodeIsCompleted(zoneNode.zoneNodeRef) && GameData.instance.PROJECT.character.zones.nodeIsUnlocked(zoneNode.zoneNodeRef))
			{
				return zoneNode;
			}
		}
		return null;
	}

	private ZoneNode GetFirstRepeatableNode()
	{
		ZoneNode[] componentsInChildren = questMap.GetComponentsInChildren<ZoneNode>();
		foreach (ZoneNode zoneNode in componentsInChildren)
		{
			if (GameData.instance.PROJECT.character.zones.nodeIsCompleted(zoneNode.zoneNodeRef) && GameData.instance.PROJECT.character.zones.nodeIsUnlocked(zoneNode.zoneNodeRef))
			{
				return zoneNode;
			}
		}
		return null;
	}

	private ZoneNode GetLastAvailableNode()
	{
		ZoneNode[] componentsInChildren = questMap.GetComponentsInChildren<ZoneNode>();
		ZoneNode result = null;
		ZoneNode[] array = componentsInChildren;
		foreach (ZoneNode zoneNode in array)
		{
			if (!GameData.instance.PROJECT.character.zones.nodeIsCompleted(zoneNode.zoneNodeRef) && GameData.instance.PROJECT.character.zones.nodeIsUnlocked(zoneNode.zoneNodeRef))
			{
				result = zoneNode;
			}
		}
		return result;
	}

	private void AnimateZonesBtn()
	{
		if (!zoneBtnAnim)
		{
			zoneBtnAnim = true;
			Vector3 localScale = zonesBtn.transform.localScale;
			Vector3 middleScale = new Vector3(0.9f, 0.9f, 1f);
			Vector3 localScale2 = new Vector3(2f, 2f, 1f);
			Sequence s = DOTween.Sequence();
			zonesBtn.transform.localScale = localScale2;
			s.Insert(0f, zonesBtn.transform.DOScale(localScale, 0.15f).OnComplete(delegate
			{
				zonesBtn.transform.localScale = middleScale;
			}));
			s.Insert(0.15f, zonesBtn.transform.DOScale(localScale, 0.15f));
		}
	}

	private void OnDestroy()
	{
	}
}
