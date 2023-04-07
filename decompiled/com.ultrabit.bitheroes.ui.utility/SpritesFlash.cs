using System.Collections;
using com.ultrabit.bitheroes.core;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

public class SpritesFlash : MonoBehaviour
{
	private Material[] spriteMaterial;

	private SpriteRenderer[] _spriteRenderer;

	private IEnumerator _flashTimer;

	private Shader spriteBrightnessShader;

	private IEnumerator _alphaTimer;

	public void DoFlash()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (spriteBrightnessShader == null)
		{
			spriteBrightnessShader = GameData.instance.main.assetLoader.GetAsset<Shader>("Shader/SpriteBrightness");
		}
		if (spriteMaterial == null)
		{
			GetSpriteRenderer();
			for (int i = 0; i < _spriteRenderer.Length; i++)
			{
				if (!(_spriteRenderer[i] == null))
				{
					spriteMaterial[i] = _spriteRenderer[i].material;
					_spriteRenderer[i].material = new Material(spriteMaterial[i]);
				}
			}
		}
		if (_flashTimer != null)
		{
			StopCoroutine(_flashTimer);
			_flashTimer = null;
		}
		for (int j = 0; j < _spriteRenderer.Length; j++)
		{
			if (!(_spriteRenderer[j] == null))
			{
				_spriteRenderer[j].material.shader = spriteBrightnessShader;
				_spriteRenderer[j].material.SetFloat("_BrightnessAmount", 1f);
			}
		}
		_flashTimer = FlashTimer();
		StartCoroutine(_flashTimer);
	}

	private IEnumerator FlashTimer()
	{
		float seconds = 0.05f / GameData.instance.PROJECT.battle.GetSpeed();
		yield return new WaitForSeconds(seconds);
		for (int i = 0; i < _spriteRenderer.Length; i++)
		{
			if (!(_spriteRenderer[i] == null))
			{
				_spriteRenderer[i].material.SetFloat("_BrightnessAmount", 0f);
				_spriteRenderer[i].material.shader = Shader.Find("Sprites/Default");
				_spriteRenderer[i].material.SetFloat("_BrightnessAmount", 0f);
			}
		}
	}

	private void GetSpriteRenderer()
	{
		if (_spriteRenderer == null)
		{
			_spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
		}
		spriteMaterial = new Material[_spriteRenderer.Length];
	}

	private void OnDisable()
	{
		if (_spriteRenderer == null)
		{
			return;
		}
		for (int i = 0; i < _spriteRenderer.Length; i++)
		{
			if (_spriteRenderer[i] != null)
			{
				_spriteRenderer[i].material = _spriteRenderer[i].sharedMaterial;
			}
		}
	}

	private void OnDestroy()
	{
		if (_spriteRenderer == null)
		{
			return;
		}
		for (int i = 0; i < _spriteRenderer.Length; i++)
		{
			if (_spriteRenderer[i] != null)
			{
				Object.Destroy(_spriteRenderer[i].material);
			}
		}
	}
}
