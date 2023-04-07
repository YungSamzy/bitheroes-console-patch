using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.tutorial;

public class TutorialManager : MonoBehaviour
{
	public const float BACKGROUND_OPACITY = 0.5f;

	public Transform tutorialPopUpPrefab;

	public Transform tutorialContainerPrefab;

	private GameObject _canvas;

	private GameObject _targetObject;

	private Transform _targetParent;

	private GameObject _targetPlaceholder;

	private Button _targetButton;

	private EventTrigger _targetEventTrigger;

	private EventTrigger.Entry _targetEventEntry;

	private EventTriggerType? _targetEventType;

	private bool _addedEntry;

	private bool _funcSameAsTargetFunc;

	private UnityAction<object> _func;

	private UnityAction<object> _alwaysCalledFunc;

	private TutorialPopup _tutorialPopUp;

	private StageTrigger _stageTrigger;

	private EventTrigger _stageEventTrigger;

	private RectTransform _tutorialContainer;

	private Image _stageImage;

	private bool cleared;

	public bool hasPopup => _tutorialContainer != null;

	public GameObject canvas => _canvas;

	private void Awake()
	{
		GameData.instance.tutorialManager = this;
		_canvas = GameObject.Find("Canvas");
		Tutorial.Init();
	}

	public void ShowTutorialForButton(GameObject theObject, TutorialPopUpSettings popUpSettings = null, bool stageTrigger = true, EventTriggerType? stageEventType = EventTriggerType.PointerClick, bool funcSameAsTargetFunc = false, UnityAction<object> func = null, bool shadow = true, bool tween = false, UnityAction<object> alwaysCalledFunc = null, bool closeTooltip = true)
	{
		ClearTooltip(closeTooltip);
		ClearTutorial();
		_targetObject = theObject;
		_funcSameAsTargetFunc = funcSameAsTargetFunc;
		_func = func;
		_alwaysCalledFunc = alwaysCalledFunc;
		if (_tutorialContainer == null)
		{
			CreateTutorialContainer();
		}
		StartCoroutine(AddTutorialButton(popUpSettings, stageTrigger, stageEventType ?? EventTriggerType.PointerClick, shadow, tween));
	}

	public void ShowTutorialForEventTrigger(GameObject theObject, TutorialPopUpSettings popUpSettings = null, EventTriggerType? targetEventType = EventTriggerType.PointerClick, bool stageTrigger = true, EventTriggerType? stageEventType = EventTriggerType.PointerClick, bool funcSameAsTargetFunc = false, UnityAction<object> func = null, bool shadow = true, bool tween = false, UnityAction<object> alwaysCalledFunc = null, bool closeTooltip = true)
	{
		ClearTooltip(closeTooltip);
		ClearTutorial();
		_targetObject = theObject;
		_funcSameAsTargetFunc = funcSameAsTargetFunc;
		_func = func;
		_alwaysCalledFunc = alwaysCalledFunc;
		if (_tutorialContainer == null)
		{
			CreateTutorialContainer();
		}
		StartCoroutine(AddTutorialTrigger(popUpSettings, targetEventType ?? EventTriggerType.PointerClick, stageTrigger, stageEventType ?? EventTriggerType.PointerClick, shadow, tween));
	}

	private void ClearTooltip(bool close)
	{
		if (close)
		{
			Transform windowTooltipContainer = GameData.instance.windowTooltipContainer;
			if (windowTooltipContainer != null && windowTooltipContainer.gameObject.activeSelf)
			{
				windowTooltipContainer.gameObject.SetActive(value: false);
				windowTooltipContainer.GetComponent<ItemTooltipContainer>().HideComparisson();
			}
		}
	}

	private void CreateTutorialContainer()
	{
		Transform transform = Object.Instantiate(tutorialContainerPrefab);
		Main.CONTAINER.AddToLayer(transform.gameObject, 12);
		_tutorialContainer = transform.GetComponent<RectTransform>();
		_stageTrigger = transform.GetComponent<StageTrigger>();
		_stageEventTrigger = transform.GetComponent<EventTrigger>();
		_stageImage = transform.GetComponent<Image>();
		Canvas component = transform.GetComponent<Canvas>();
		if (component != null)
		{
			SortingGroup sortingGroup = transform.gameObject.AddComponent<SortingGroup>();
			sortingGroup.sortingLayerName = component.sortingLayerName;
			sortingGroup.sortingOrder = component.sortingOrder + 1000;
		}
	}

	private void SetStageTrigger(GameObject targetObject, bool stageTrigger, EventTriggerType stageEventType = EventTriggerType.PointerClick)
	{
		if (stageTrigger)
		{
			_stageEventTrigger.enabled = true;
			_stageImage.raycastTarget = true;
			switch (stageEventType)
			{
			case EventTriggerType.PointerClick:
				_stageTrigger.POINTER_CLICK.AddListener(delegate
				{
					OnActionTriggered(calledFromTarget: false);
				});
				break;
			case EventTriggerType.PointerDown:
				_stageTrigger.POINTER_DOWN.AddListener(delegate
				{
					OnActionTriggered(calledFromTarget: false);
				});
				break;
			}
		}
		else
		{
			_stageEventTrigger.enabled = false;
		}
		if (_targetObject != null)
		{
			BringTargetToFront(targetObject);
		}
	}

	private void CreatePopUp(TutorialPopUpSettings popUpSettings)
	{
		if (popUpSettings == null)
		{
			return;
		}
		Transform transform = Object.Instantiate(tutorialPopUpPrefab);
		transform.SetParent(_tutorialContainer.transform, worldPositionStays: false);
		_tutorialPopUp = transform.GetComponent<TutorialPopup>();
		_tutorialPopUp.SetParentRectTransform(_tutorialContainer);
		_tutorialPopUp.LoadDetails(popUpSettings.text, popUpSettings.arrowPosition, popUpSettings.target, popUpSettings.offset, popUpSettings.indicator, popUpSettings.button, popUpSettings.glow, popUpSettings.width, popUpSettings.position);
		if (_tutorialPopUp.closeBtn.gameObject.activeSelf)
		{
			_tutorialPopUp.closeBtn.onClick.AddListener(delegate
			{
				OnActionTriggered(calledFromTarget: false);
			});
		}
	}

	private void BringTargetToFront(GameObject targetObject)
	{
		int? num = null;
		_targetParent = targetObject.transform.parent;
		num = targetObject.transform.GetSiblingIndex();
		_targetPlaceholder = new GameObject();
		_targetPlaceholder.AddComponent<DestroyedEvent>().DESTROYED.AddListener(OnPlaceholderDestroyed);
		_targetPlaceholder.name = "Target tutorial placeholder";
		RectTransform component = targetObject.GetComponent<RectTransform>();
		if (component != null)
		{
			RectTransform rectTransform = _targetPlaceholder.AddComponent<RectTransform>();
			rectTransform.anchorMin = component.anchorMin;
			rectTransform.anchorMax = component.anchorMax;
			rectTransform.sizeDelta = component.sizeDelta;
		}
		_targetPlaceholder.transform.position = targetObject.transform.position;
		targetObject.transform.SetParent(_tutorialContainer);
		int sortingOrder = _tutorialContainer.GetComponent<SortingGroup>().sortingOrder;
		Canvas[] componentsInChildren = targetObject.GetComponentsInChildren<Canvas>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].sortingOrder += sortingOrder;
		}
		_targetPlaceholder.transform.SetParent(_targetParent);
		_targetPlaceholder.transform.SetSiblingIndex(num.Value);
		LayoutElement component2 = targetObject.GetComponent<LayoutElement>();
		if (component2 != null)
		{
			LayoutElement layoutElement = _targetPlaceholder.AddComponent<LayoutElement>();
			layoutElement.preferredHeight = component2.preferredHeight;
			layoutElement.preferredWidth = component2.preferredWidth;
		}
	}

	private void OnPlaceholderDestroyed(object placeholder)
	{
		DestroyedEvent component = (placeholder as GameObject).GetComponent<DestroyedEvent>();
		if (component != null)
		{
			component.DESTROYED.RemoveListener(OnPlaceholderDestroyed);
		}
	}

	private void BackgroundTween()
	{
		float a = 0.5f;
		float num = 1f;
		if (AppInfo.TESTING)
		{
			num *= 3f;
		}
		_stageImage.color = new Color(0f, 0f, 0f, 0f);
		DOTweenModuleUI.DOColor(endValue: new Color(0f, 0f, 0f, a), target: _stageImage, duration: 0.4f / num);
	}

	private EventTrigger.Entry CheckTargetEventTrigger(EventTrigger targetEventTrigger, EventTriggerType targetEventType)
	{
		foreach (EventTrigger.Entry trigger in targetEventTrigger.triggers)
		{
			if (trigger.eventID == _targetEventType)
			{
				return trigger;
			}
		}
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = targetEventType;
		targetEventTrigger.triggers.Add(entry);
		_addedEntry = true;
		return entry;
	}

	private void OnActionTriggered(bool calledFromTarget, BaseEventData eventData = null)
	{
		bool funcSameAsTargetFunc = _funcSameAsTargetFunc;
		UnityAction<object> func = _func;
		Button targetButton = _targetButton;
		EventTrigger.Entry targetEventEntry = _targetEventEntry;
		UnityAction<object> alwaysCalledFunc = _alwaysCalledFunc;
		ClearTutorial();
		if (func != null)
		{
			if (calledFromTarget)
			{
				if (!funcSameAsTargetFunc)
				{
					func(null);
				}
			}
			else
			{
				func(null);
			}
		}
		else if (!calledFromTarget && funcSameAsTargetFunc)
		{
			if (targetButton != null)
			{
				targetButton.onClick.Invoke();
			}
			targetEventEntry?.callback.Invoke(null);
		}
		alwaysCalledFunc?.Invoke(null);
	}

	private IEnumerator AddTutorialButton(TutorialPopUpSettings popUpSettings = null, bool stageTrigger = true, EventTriggerType stageEventType = EventTriggerType.PointerClick, bool shadow = true, bool tween = false)
	{
		yield return new WaitForEndOfFrame();
		SetStageTrigger(_targetObject, stageTrigger, stageEventType);
		CreatePopUp(popUpSettings);
		if (!(_targetObject != null))
		{
			yield break;
		}
		_targetButton = _targetObject.GetComponent<Button>();
		_targetButton.onClick.AddListener(delegate
		{
			OnActionTriggered(calledFromTarget: true);
		});
		if (shadow)
		{
			_stageImage.color = new Color(0f, 0f, 0f, 0.5f);
			if (tween)
			{
				BackgroundTween();
			}
		}
		else
		{
			_stageImage.color = new Color(0f, 0f, 0f, 0f);
		}
	}

	private IEnumerator AddTutorialTrigger(TutorialPopUpSettings popUpSettings = null, EventTriggerType targetEventType = EventTriggerType.PointerClick, bool stageTrigger = true, EventTriggerType stageEventType = EventTriggerType.PointerClick, bool shadow = true, bool tween = false)
	{
		yield return new WaitForEndOfFrame();
		SetStageTrigger(_targetObject, stageTrigger, stageEventType);
		CreatePopUp(popUpSettings);
		if (!(_targetObject != null))
		{
			yield break;
		}
		_targetEventType = targetEventType;
		if (!_targetEventType.HasValue)
		{
			_targetEventType = EventTriggerType.PointerClick;
		}
		_targetEventTrigger = _targetObject.GetComponent<EventTrigger>();
		if (_targetEventTrigger == null)
		{
			_targetEventTrigger = _targetObject.AddComponent<EventTrigger>();
		}
		_targetEventEntry = CheckTargetEventTrigger(_targetEventTrigger, _targetEventType.Value);
		_targetEventEntry.callback.AddListener(delegate(BaseEventData data)
		{
			OnActionTriggered(calledFromTarget: true, data);
		});
		if (shadow)
		{
			_stageImage.color = new Color(0f, 0f, 0f, 0.5f);
			if (tween)
			{
				BackgroundTween();
			}
		}
		else
		{
			_stageImage.color = new Color(0f, 0f, 0f, 0f);
		}
	}

	public bool TargetObjectEquals(GameObject other)
	{
		if (_targetObject != null)
		{
			return _targetObject.Equals(other);
		}
		return false;
	}

	public void ClearTutorial()
	{
		if (_targetObject != null)
		{
			int? num = null;
			if (_targetPlaceholder != null)
			{
				num = _targetPlaceholder.transform.GetSiblingIndex();
				Object.Destroy(_targetPlaceholder.gameObject);
			}
			_targetObject.transform.SetParent(_targetParent);
			int sortingOrder = _tutorialContainer.GetComponent<SortingGroup>().sortingOrder;
			Canvas[] componentsInChildren = _targetObject.GetComponentsInChildren<Canvas>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].sortingOrder -= sortingOrder;
			}
			if (num.HasValue)
			{
				_targetObject.transform.SetSiblingIndex(num.Value);
			}
		}
		if (_tutorialPopUp != null)
		{
			_tutorialPopUp.closeBtn.onClick.RemoveListener(delegate
			{
				OnActionTriggered(calledFromTarget: false);
			});
		}
		if (_targetButton != null)
		{
			_targetButton.onClick.RemoveListener(delegate
			{
				OnActionTriggered(calledFromTarget: true);
			});
		}
		if (_targetEventTrigger != null && _targetEventEntry != null)
		{
			_targetEventEntry.callback.RemoveListener(delegate(BaseEventData data)
			{
				OnActionTriggered(calledFromTarget: true, data);
			});
			if (_addedEntry)
			{
				for (int j = 0; j < _targetEventTrigger.triggers.Count; j++)
				{
					if (_targetEventTrigger.triggers[j].eventID == _targetEventEntry.eventID)
					{
						_targetEventTrigger.triggers.RemoveAt(j);
					}
				}
			}
		}
		if (_stageTrigger != null)
		{
			_stageTrigger.POINTER_CLICK.RemoveListener(delegate
			{
				OnActionTriggered(calledFromTarget: false);
			});
			_stageTrigger.POINTER_DOWN.RemoveListener(delegate
			{
				OnActionTriggered(calledFromTarget: false);
			});
		}
		if (_tutorialContainer != null)
		{
			Object.Destroy(_tutorialContainer.gameObject);
		}
		_targetObject = null;
		_targetParent = null;
		_targetPlaceholder = null;
		_targetButton = null;
		_targetEventTrigger = null;
		_targetEventEntry = null;
		_targetEventType = null;
		_addedEntry = false;
		_funcSameAsTargetFunc = false;
		_func = null;
		_alwaysCalledFunc = null;
		_tutorialPopUp = null;
		_stageTrigger = null;
		_stageEventTrigger = null;
		_tutorialContainer = null;
		_stageImage = null;
	}
}
