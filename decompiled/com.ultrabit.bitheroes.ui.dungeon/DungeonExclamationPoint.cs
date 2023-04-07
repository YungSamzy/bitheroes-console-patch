using System.Collections;
using com.ultrabit.bitheroes.core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.dungeon;

public class DungeonExclamationPoint : MonoBehaviour
{
	private Material spriteMaterial;

	private SpriteRenderer _spriteRenderer;

	private IEnumerator _flashTimer;

	private Shader spriteBrightnessShader;

	private IEnumerator _alphaTimer;

	public SpriteRenderer spriteRenderer
	{
		get
		{
			GetSpriteRenderer();
			return _spriteRenderer;
		}
	}

	public void DoFlash()
	{
		if (spriteBrightnessShader == null)
		{
			spriteBrightnessShader = GameData.instance.main.assetLoader.GetAsset<Shader>("Shader/SpriteBrightness");
		}
		if (spriteMaterial == null)
		{
			GetSpriteRenderer();
			spriteMaterial = _spriteRenderer.material;
			_spriteRenderer.material = new Material(spriteMaterial);
		}
		if (_flashTimer != null)
		{
			StopCoroutine(_flashTimer);
			_flashTimer = null;
		}
		_spriteRenderer.material.color = new Color(_spriteRenderer.material.color.r, _spriteRenderer.material.color.g, _spriteRenderer.material.color.b, 0f);
		_spriteRenderer.material.shader = spriteBrightnessShader;
		_spriteRenderer.material.SetFloat("_BrightnessAmount", 1f);
		_flashTimer = FlashTimer();
		StartCoroutine(_flashTimer);
	}

	private IEnumerator FlashTimer()
	{
		yield return new WaitForSeconds(0.05f);
		_spriteRenderer.material.SetFloat("_BrightnessAmount", 0f);
		_spriteRenderer.material.shader = spriteMaterial.shader;
	}

	public void ZoomIn(UnityAction func)
	{
		Vector3 localScale = base.transform.localScale;
		Vector3 localScale2 = base.transform.localScale * 3f;
		base.transform.localScale = localScale2;
		base.transform.DOScale(localScale, 0.5f).SetEase(Ease.InOutBack).OnComplete(delegate
		{
			if (func != null)
			{
				func();
			}
		});
		ChangeAlpha(0f, 1f, 0.5f, Ease.InOutBack, 0f, null);
	}

	public void ChangeAlpha(float? from, float to, float duration, Ease tweenScaleFunctions, float delay, UnityAction callback)
	{
		if (_alphaTimer != null)
		{
			StopCoroutine(_alphaTimer);
			_alphaTimer = null;
		}
		_alphaTimer = DoChangeAlpha(from, to, duration, tweenScaleFunctions, delay, callback);
		if (base.gameObject.activeSelf)
		{
			StartCoroutine(_alphaTimer);
		}
	}

	private IEnumerator DoChangeAlpha(float? from, float to, float duration, Ease tweenScaleFunctions, float delay, UnityAction callback)
	{
		yield return new WaitForSeconds(delay);
		GetSpriteRenderer();
		Color color = _spriteRenderer.material.color;
		float a = _spriteRenderer.material.color.a;
		if (from.HasValue)
		{
			a = from.Value;
		}
		_spriteRenderer.material.color = new Color(color.r, color.g, color.b, a);
		if (tweenScaleFunctions == Ease.Unset)
		{
			tweenScaleFunctions = Ease.Linear;
		}
		Color color2 = new Color(color.r, color.g, color.b, a);
		_spriteRenderer.material.color = color2;
		ShortcutExtensions.DOColor(endValue: new Color(color.r, color.g, color.b, to), target: _spriteRenderer.material, duration: duration).SetEase(tweenScaleFunctions).OnComplete(delegate
		{
			if (callback != null)
			{
				callback();
			}
		});
	}

	private void GetSpriteRenderer()
	{
		if (_spriteRenderer == null)
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}
	}

	private void OnDestroy()
	{
		if (_spriteRenderer != null)
		{
			Object.Destroy(_spriteRenderer.material);
		}
	}
}
