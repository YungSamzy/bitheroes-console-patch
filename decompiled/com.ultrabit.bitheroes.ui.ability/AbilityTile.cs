using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.game;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.ability;

public class AbilityTile : GameTextSprite
{
	public Animator shimmer;

	public Image border;

	public AbilityTileOverlay abilityTileOverlay;

	public HoverImages hoverImages;

	public GameObject nftBorder;

	public TextMeshProUGUI keyTxt;

	public Image keyBG;

	public Image placeholderIcon;

	private AbilityRef _abilityRef;

	private int _index;

	private string _key;

	public GameObject _textTooltip;

	private GameObject tooltip;

	private string tooltipText;

	private bool _clickable;

	private RectTransform _rectTransform;

	public UnityAction<AbilityRef> onClicked;

	public UnityAction onOverrideClick;

	public UnityAction onOverridePointerEnter;

	public UnityAction onOverridePointerExit;

	private IEnumerator tweenCoroutine;

	private Sequence abttSeq;

	private bool _disabled;

	public bool clickable => _clickable;

	public string key => _key;

	public AbilityRef abilityRef => _abilityRef;

	public RectTransform rectTransform => _rectTransform;

	public void LoadDetails(AbilityRef abilityRef, BattleEntity entity, int index = 0, string key = null)
	{
		LoadGameTextSprite(abilityRef.getTooltipText((entity != null) ? entity.GetTotalPower() : 0, (!(entity != null)) ? 0f : ((entity.characterData != null) ? GameModifier.getTypeTotal(entity.characterData.getModifiers(), 17) : 0f)), click: false, 4, base.gameObject, 100f, 70f);
		_abilityRef = abilityRef;
		_index = index;
		_rectTransform = GetComponent<RectTransform>();
		int tile = Mathf.RoundToInt((float)abilityRef.meterCost / (float)VariableBook.battleMeterMax * 4f);
		abilityTileOverlay.SetTile(tile);
		float bonus = ((entity != null && entity.characterData != null) ? GameModifier.getTypeTotal(entity.characterData.getModifiers(), 17) : 0f);
		tooltipText = ((entity != null) ? abilityRef.getTooltipText(entity.GetTotalPower(), bonus) : "");
		SetClickable();
		SetKey(key);
		RefreshAsset();
	}

	public void SetClickable(bool clickable = true)
	{
		_clickable = clickable;
		abilityTileOverlay.OverlayVisible(clickable);
		abilityTileOverlay.SetColor(clickable);
		if (clickable)
		{
			if (hoverImages != null)
			{
				hoverImages.enabled = true;
			}
			if (border != null)
			{
				border.color = Color.white;
			}
			if (placeholderIcon != null)
			{
				placeholderIcon.color = Color.white;
			}
			shimmer.gameObject.SetActive(value: true);
			float normalizedTime = (float)(_index % 20) * 0.1f;
			shimmer.Play("UI_ShimmerMask_Animation", 0, normalizedTime);
		}
		else
		{
			if (hoverImages != null)
			{
				hoverImages.enabled = false;
			}
			if (border != null)
			{
				border.color = Color.gray;
			}
			if (placeholderIcon != null)
			{
				placeholderIcon.color = Util.WHITE_ALPHA_50;
			}
			if (shimmer != null && shimmer.gameObject != null)
			{
				shimmer.gameObject.SetActive(value: false);
			}
		}
		if (!clickable)
		{
			base.OnPointerExit(null);
		}
		UpdateKey();
	}

	public void SetDisabled(bool disabled = false)
	{
		Color color;
		Color color2;
		if (clickable)
		{
			color = Color.white;
			color2 = Color.white;
		}
		else if (disabled)
		{
			color = new Color(1f, 0f, 0f, 1f);
			color2 = new Color(1f, 0f, 0f, 0.5f);
		}
		else
		{
			color = new Color(0.5f, 0.5f, 0.5f, 1f);
			color2 = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		}
		border.color = color;
		placeholderIcon.color = color2;
	}

	public void SetKey(string key)
	{
		_key = key;
		if (key != null)
		{
			keyTxt.text = key;
		}
		UpdateKey();
	}

	private void UpdateKey()
	{
		if (_key != null && clickable)
		{
			if (keyBG != null && keyBG.gameObject != null)
			{
				keyBG.gameObject.SetActive(value: true);
			}
			if (keyTxt != null && keyTxt.gameObject != null)
			{
				keyTxt.gameObject.SetActive(value: true);
			}
		}
		else
		{
			if (keyBG != null && keyBG.gameObject != null)
			{
				keyBG.gameObject.SetActive(value: false);
			}
			if (keyTxt != null && keyTxt.gameObject != null)
			{
				keyTxt.gameObject.SetActive(value: false);
			}
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (onOverridePointerEnter != null)
		{
			onOverridePointerEnter();
		}
		else if (clickable)
		{
			base.OnPointerEnter(eventData);
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (onOverridePointerExit != null)
		{
			onOverridePointerExit();
		}
		else if (clickable)
		{
			base.OnPointerExit(eventData);
		}
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (onOverrideClick != null)
		{
			onOverrideClick();
		}
		else if ((!AppInfo.IsMobile() || !base.active) && clickable)
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			if (onClicked != null)
			{
				onClicked(abilityRef);
			}
			base.OnPointerClick(eventData);
		}
	}

	public void RefreshAsset()
	{
		placeholderIcon.overrideSprite = abilityRef.GetSpriteIcon();
	}

	public void TweenAbility(bool inward, float delay, float duration, float offset, float endAlpha)
	{
		if (base.gameObject.activeSelf)
		{
			tweenCoroutine = DoTweenAbility(inward, delay, duration, offset, endAlpha);
			StartCoroutine(tweenCoroutine);
		}
	}

	public void StopTween()
	{
		if (tweenCoroutine != null)
		{
			StopCoroutine(tweenCoroutine);
			tweenCoroutine = null;
		}
	}

	public void RemoveTweenKeys()
	{
		if (abttSeq != null)
		{
			abttSeq.Kill(complete: true);
		}
	}

	private IEnumerator DoTweenAbility(bool inward, float delay, float duration, float offset, float endAlpha)
	{
		if (abttSeq != null)
		{
			abttSeq.Kill();
		}
		CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
		if (canvasGroup == null)
		{
			canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
		}
		if (inward)
		{
			canvasGroup.alpha = 0f;
			base.transform.localPosition -= new Vector3(0f, offset, 0f);
		}
		yield return new WaitForSeconds(delay);
		_ = base.transform.localPosition;
		Vector3 endValue = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + offset, base.transform.localPosition.z);
		abttSeq = DOTween.Sequence();
		abttSeq.Insert(0f, base.transform.DOLocalMove(endValue, duration).SetEase(Ease.Linear));
		abttSeq.Insert(0f, canvasGroup.DOFade(inward ? 1 : 0, duration).SetEase(Ease.Linear));
		abttSeq.OnComplete(delegate
		{
			if (!inward)
			{
				base.gameObject.SetActive(value: false);
			}
		});
	}

	public void SetShimmer(bool enabled = true)
	{
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
	}
}
