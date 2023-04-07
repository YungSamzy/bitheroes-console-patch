using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class HoverImages : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private const float GLOW_VALUE_MAX = 1.6f;

	public const float BRIGHT_VALUE = 1.5f;

	private Image[] image;

	private Material[] mat;

	private bool _active = true;

	private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();

	private bool _glowing;

	private float _glowValue;

	private bool _canGlow;

	private bool forceStart;

	private Shader grayscaleShader;

	private bool _noStopOnHover;

	private bool _restartGlowonExit;

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

	private void Start()
	{
		if (!forceStart)
		{
			grayscaleShader = GameData.instance.main.assetLoader.GetAsset<Shader>("Shader/Grayscale");
			image = GetComponentsInChildren<Image>();
			mat = new Material[image.Length];
			for (int i = 0; i < image.Length; i++)
			{
				mat[i] = image[i].material;
				image[i].material = new Material(mat[i]);
			}
		}
	}

	public void ForceStart()
	{
		forceStart = true;
		grayscaleShader = GameData.instance.main.assetLoader.GetAsset<Shader>("Shader/Grayscale");
		image = GetComponentsInChildren<Image>();
		mat = new Material[image.Length];
		for (int i = 0; i < image.Length; i++)
		{
			mat[i] = image[i].material;
			image[i].material = new Material(mat[i]);
		}
	}

	public void GetOwnTexts()
	{
		TextMeshProUGUI[] componentsInChildren = GetComponentsInChildren<TextMeshProUGUI>();
		texts.Clear();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			texts.Add(componentsInChildren[i]);
		}
	}

	public void OnEnter()
	{
		if (!_active)
		{
			return;
		}
		if (!_noStopOnHover)
		{
			StopGlowing();
		}
		_canGlow = false;
		for (int i = 0; i < image.Length; i++)
		{
			Util.adjustBrightness(image[i], 1.5f);
		}
		for (int j = 0; j < texts.Count; j++)
		{
			if (texts[j] != null && (bool)texts[j].GetComponent<TextBrightness>())
			{
				texts[j].GetComponent<TextBrightness>().AddBrightness();
			}
		}
	}

	public void OnExit()
	{
		if (!_active)
		{
			return;
		}
		if (image != null)
		{
			for (int i = 0; i < image.Length; i++)
			{
				Util.adjustBrightness(image[i]);
			}
		}
		if (texts != null)
		{
			for (int j = 0; j < texts.Count; j++)
			{
				if (texts[j] != null && (bool)texts[j].GetComponent<TextBrightness>())
				{
					texts[j].GetComponent<TextBrightness>().ClearBrightness();
				}
			}
		}
		if (_restartGlowonExit)
		{
			canGlow = true;
			StartGlow(_noStopOnHover, _restartGlowonExit);
		}
	}

	public void OnPointerEnter(PointerEventData pointer)
	{
		if (!_active)
		{
			return;
		}
		if (!_noStopOnHover)
		{
			StopGlowing();
		}
		_canGlow = false;
		for (int i = 0; i < image.Length; i++)
		{
			Util.adjustBrightness(image[i], 1.5f);
		}
		for (int j = 0; j < texts.Count; j++)
		{
			if (texts[j] != null && (bool)texts[j].GetComponent<TextBrightness>())
			{
				texts[j].GetComponent<TextBrightness>().AddBrightness();
			}
		}
	}

	public void OnPointerExit(PointerEventData pointer)
	{
		if (!_active)
		{
			return;
		}
		for (int i = 0; i < image.Length; i++)
		{
			Util.adjustBrightness(image[i]);
		}
		for (int j = 0; j < texts.Count; j++)
		{
			if (texts[j] != null && (bool)texts[j].GetComponent<TextBrightness>())
			{
				texts[j].GetComponent<TextBrightness>().ClearBrightness();
			}
		}
		if (_restartGlowonExit)
		{
			canGlow = true;
			StartGlow(_noStopOnHover, _restartGlowonExit);
		}
	}

	public void ClearBrightness()
	{
		for (int i = 0; i < image.Length; i++)
		{
			Util.adjustBrightness(image[i]);
		}
		for (int j = 0; j < texts.Count; j++)
		{
			if ((bool)texts[j].GetComponent<TextBrightness>())
			{
				texts[j].GetComponent<TextBrightness>().ClearBrightness();
			}
		}
	}

	public void SetTexts(List<TextMeshProUGUI> lstTexts)
	{
		texts.Clear();
		texts.AddRange(lstTexts);
	}

	public void ClearAndGetOwnTexts()
	{
		texts.Clear();
		GetOwnTexts();
	}

	public void StartGlow(bool noStopOnHover = false, bool restartGlowonExit = false)
	{
		_noStopOnHover = noStopOnHover;
		_restartGlowonExit = restartGlowonExit;
		if (_glowing || !active || !_canGlow)
		{
			return;
		}
		_glowing = true;
		TweenGlowIncreased();
		for (int i = 0; i < texts.Count; i++)
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
		for (int i = 0; i < image.Length; i++)
		{
			Util.adjustBrightness(image[i]);
		}
		for (int j = 0; j < texts.Count; j++)
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
		DOTween.To(() => initAlpha, delegate(float x)
		{
			initAlpha = x;
		}, 1.6f, 1f).SetEase(Ease.Linear).OnUpdate(delegate
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
		DOTween.To(() => initAlpha, delegate(float x)
		{
			initAlpha = x;
		}, 1f, 1f).SetEase(Ease.Linear).OnUpdate(delegate
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

	public void AddGrayscale(float alphaValue = 0.5f)
	{
		for (int i = 0; i < image.Length; i++)
		{
			Util.AddGrayscale(image[i], grayscaleShader);
		}
		for (int j = 0; j < texts.Count; j++)
		{
			texts[j].color = new Color(texts[j].color.r, texts[j].color.g, texts[j].color.b, alphaValue);
		}
	}

	public void ClearGrayscale(float alphaValue = 1f)
	{
		if (image != null)
		{
			for (int i = 0; i < image.Length; i++)
			{
				Util.ClearGrayscale(image[i]);
			}
			for (int j = 0; j < texts.Count; j++)
			{
				texts[j].color = new Color(texts[j].color.r, texts[j].color.g, texts[j].color.b, alphaValue);
			}
		}
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
