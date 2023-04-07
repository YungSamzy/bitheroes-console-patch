using com.ultrabit.bitheroes.model.utility;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class GlowNoOver : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private const float GLOW_VALUE_MAX = 1.8f;

	private float _glowValue;

	private bool _glowing;

	private bool _active = true;

	private bool _canGlow = true;

	private Image[] image;

	private Material[] mat;

	private TextMeshProUGUI[] texts;

	[HideInInspector]
	public UnityEvent ON_ENTER = new UnityEvent();

	[HideInInspector]
	public UnityEvent ON_EXIT = new UnityEvent();

	private TweenerCore<float, float, FloatOptions> _tweenIncreased;

	private TweenerCore<float, float, FloatOptions> _tweenDecreased;

	public bool active
	{
		get
		{
			return _active;
		}
		set
		{
			_active = value;
		}
	}

	public bool canGlow
	{
		get
		{
			return _canGlow;
		}
		set
		{
			_canGlow = value;
		}
	}

	public void ForceStart(bool forceTextsComponents = false)
	{
		image = GetComponentsInChildren<Image>();
		mat = new Material[image.Length];
		for (int i = 0; i < image.Length; i++)
		{
			mat[i] = image[i].material;
			image[i].material = new Material(mat[i]);
		}
		texts = GetComponentsInChildren<TextMeshProUGUI>();
		if (forceTextsComponents)
		{
			ForceTextsComponents();
		}
	}

	private void ForceTextsComponents()
	{
		for (int i = 0; i < texts.Length; i++)
		{
			if (texts[i].GetComponent<TextBrightness>() == null)
			{
				texts[i].gameObject.AddComponent<TextBrightness>();
			}
		}
	}

	public void OnEnter()
	{
		if (_active)
		{
			StopGlowing();
			_canGlow = false;
			ON_ENTER.Invoke();
		}
	}

	public void OnExit()
	{
		if (_active)
		{
			ON_EXIT.Invoke();
		}
	}

	public void OnPointerEnter(PointerEventData pointer)
	{
		if (_active)
		{
			StopGlowing();
			_canGlow = false;
			ON_ENTER.Invoke();
		}
	}

	public void OnPointerExit(PointerEventData pointer)
	{
		if (_active)
		{
			ON_EXIT.Invoke();
		}
	}

	public void StartGlow()
	{
		if (_glowing || !active || !_canGlow)
		{
			return;
		}
		_glowing = true;
		TweenGlowIncreased();
		for (int i = 0; i < texts.Length; i++)
		{
			if ((bool)texts[i].GetComponent<TextBrightness>())
			{
				texts[i].GetComponent<TextBrightness>().StartGlow();
			}
		}
	}

	public void StopGlowing()
	{
		if (!_glowing)
		{
			return;
		}
		_glowing = false;
		_tweenIncreased?.Kill();
		_tweenDecreased?.Kill();
		for (int i = 0; i < image.Length; i++)
		{
			Util.adjustBrightness(image[i]);
		}
		for (int j = 0; j < texts.Length; j++)
		{
			if ((bool)texts[j].GetComponent<TextBrightness>())
			{
				texts[j].GetComponent<TextBrightness>().StopGlow();
			}
		}
	}

	public void TweenGlowIncreased()
	{
		float initAlpha = 1f;
		_tweenIncreased = DOTween.To(() => initAlpha, delegate(float x)
		{
			initAlpha = x;
		}, 1.8f, 1f).SetEase(Ease.OutCubic).OnUpdate(delegate
		{
			_glowValue = initAlpha;
			for (int i = 0; i < image.Length; i++)
			{
				Util.adjustBrightness(image[i], _glowValue);
			}
		})
			.OnComplete(delegate
			{
				TweenGlowDecreased();
			});
	}

	public void TweenGlowDecreased()
	{
		float initAlpha = _glowValue;
		_tweenDecreased = DOTween.To(() => initAlpha, delegate(float x)
		{
			initAlpha = x;
		}, 1.8f, 1f).SetEase(Ease.OutCubic).OnUpdate(delegate
		{
			_glowValue = initAlpha;
			for (int i = 0; i < image.Length; i++)
			{
				Util.adjustBrightness(image[i], _glowValue);
			}
		})
			.OnComplete(delegate
			{
				TweenGlowIncreased();
			});
	}

	private void OnDestroy()
	{
		if (image == null)
		{
			return;
		}
		for (int i = 0; i < image.Length; i++)
		{
			if (image[i].material != null)
			{
				Object.Destroy(image[i].material);
			}
		}
	}
}
