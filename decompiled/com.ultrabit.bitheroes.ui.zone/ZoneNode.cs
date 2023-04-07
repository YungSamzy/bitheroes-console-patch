using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.zone;

public class ZoneNode : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	public const string NODE_TYPE_FLAG = "flag";

	public const string NODE_TYPE_DUNGEON = "dungeon";

	public const string NODE_TYPE_SPECIAL = "special";

	private const float DELAY = 0.1f;

	private string nodeType;

	public ZoneNodeRef zoneNodeRef;

	private bool completed;

	private bool unlocked;

	private ZoneNodeStar zoneNodestar;

	private List<GameObject> _previousPath = new List<GameObject>();

	private int _pathIndex;

	private Vector3 originalNodeScale;

	private ZoneWindow _zoneWindow;

	private ZonePanel _zonePanel;

	private bool _onAnimation;

	public bool onAnimation => _onAnimation;

	public void Setup(ZoneNodeRef zoneNodeRef, ZonePanel zonePanel, ZoneWindow zoneWindow)
	{
		nodeType = base.gameObject.name;
		this.zoneNodeRef = zoneNodeRef;
		_zoneWindow = zoneWindow;
		_zonePanel = zonePanel;
		zoneNodestar = GetComponentInChildren<ZoneNodeStar>();
		unlocked = GameData.instance.PROJECT.character.zones.nodeIsUnlocked(zoneNodeRef);
		completed = GameData.instance.PROJECT.character.zones.nodeIsCompleted(zoneNodeRef);
		if (base.transform != null && base.transform.GetSiblingIndex() > 0)
		{
			for (int i = 0; i < zonePanel.transform.Find("nodes").GetChild(base.transform.GetSiblingIndex() - 1).childCount; i++)
			{
				GameObject gameObject = zonePanel.transform.Find("nodes").GetChild(base.transform.GetSiblingIndex() - 1).GetChild(i)
					.gameObject;
				if (gameObject.name != "Stars")
				{
					_previousPath.Add(gameObject);
				}
			}
		}
		if (completed)
		{
			SetupNodeCompleted();
		}
		else if (unlocked && GameData.instance.PROJECT.character.zones.zoneIsUnlocked(zoneNodeRef.getZoneRef()))
		{
			SetupNodeUnlocked();
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void SetupNodeCompleted()
	{
		if (nodeType.Equals("flag"))
		{
			Image component = GetComponent<Image>();
			Color color = component.color;
			color.a = 0.5f;
			component.color = color;
			return;
		}
		if (zoneNodestar != null)
		{
			zoneNodestar.Setup(GameData.instance.PROJECT.character.zones.getNodeStars(zoneNodeRef));
		}
		HoverImages hoverImages = base.gameObject.AddComponent<HoverImages>();
		hoverImages.ForceStart();
		hoverImages.GetOwnTexts();
	}

	private void SetupNodeUnlocked()
	{
		HoverImages hoverImages = base.gameObject.AddComponent<HoverImages>();
		hoverImages.ForceStart();
		hoverImages.GetOwnTexts();
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = base.transform.GetChild(i).gameObject;
			if (!(zoneNodestar != null) || !zoneNodestar.gameObject.Equals(gameObject))
			{
				gameObject.SetActive(value: false);
			}
		}
		if (GameData.instance.zoneWindowTween)
		{
			GameData.instance.lastNode = zoneNodeRef;
			TweenPathIn();
		}
	}

	private void TweenPathIn()
	{
		_onAnimation = true;
		if (base.gameObject != null && GetComponent<RectTransform>() != null)
		{
			originalNodeScale = GetComponent<RectTransform>().localScale;
			GetComponent<RectTransform>().localScale = Vector3.zero;
		}
		if (_previousPath.Count > 0)
		{
			for (int i = 0; i < _previousPath.Count; i++)
			{
				_previousPath[i].SetActive(value: false);
			}
			Invoke("StartTween", 0.2f);
		}
		else
		{
			TweenNodeIn();
		}
	}

	private void StartTween()
	{
		StartCoroutine(TweenPointIn());
	}

	private IEnumerator TweenPointIn()
	{
		yield return new WaitForSeconds(0.05f);
		GameObject obj = _previousPath[_pathIndex];
		_previousPath[_pathIndex].SetActive(value: true);
		_previousPath.RemoveAt(0);
		RectTransform component = obj.GetComponent<RectTransform>();
		Vector3 endValue = Vector3.one;
		if (component != null)
		{
			endValue = component.localScale;
		}
		component.localScale = new Vector3(2.5f, 2.5f, 1f);
		component.DOScale(endValue, 0.5f).SetEase(Ease.InCubic).OnComplete(delegate
		{
			if (_pathIndex < _previousPath.Count)
			{
				if (base.gameObject != null && base.gameObject.activeInHierarchy)
				{
					StartCoroutine(TweenPointIn());
				}
			}
			else
			{
				TweenNodeIn();
			}
		});
	}

	private void TweenNodeIn()
	{
		RectTransform component = GetComponent<RectTransform>();
		component.localScale = new Vector3(2.5f, 2.5f, 1f);
		component.DOScale(originalNodeScale, 0.5f).SetEase(Ease.InCubic).OnComplete(delegate
		{
			GameData.instance.zoneWindowTween = false;
			_onAnimation = false;
			_zoneWindow.CheckTutorialAndDefeated();
		});
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
	}

	public void OnPointerExit(PointerEventData eventData)
	{
	}

	public void OnPointerDown(PointerEventData eventData)
	{
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if ((!(_zoneWindow != null) || (!_zoneWindow.scrollingIn && !_zoneWindow.scrollingOut && _zoneWindow.isShown && !_zoneWindow.isOnTween)) && !_onAnimation && unlocked)
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			if (zoneNodeRef.difficulties.Count <= 0 || !zoneNodeRef.getActive())
			{
				GameData.instance.windowGenerator.ShowError(Language.GetString("ui_coming_soon"));
			}
			else if (!GameData.instance.PROJECT.character.zones.nodeIsCompleted(zoneNodeRef))
			{
				GameData.instance.windowGenerator.NewZoneNodeWindow(zoneNodeRef, _zoneWindow.gameObject, new int[1] { 2 });
			}
			else if (zoneNodeRef.difficulties.Count == 1)
			{
				GameData.instance.windowGenerator.NewZoneNodeSingleWindow(zoneNodeRef, _zoneWindow.gameObject, new int[1] { 2 });
			}
			else
			{
				GameData.instance.windowGenerator.NewZoneNodeDifficultyWindow(zoneNodeRef, _zoneWindow.gameObject, new int[1] { 2 });
			}
		}
	}
}
