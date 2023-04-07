using System.Collections;
using com.ultrabit.bitheroes.core;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

public class SpriteFlash : MonoBehaviour
{
	private Material spriteMaterial;

	private SpriteRenderer _spriteRenderer;

	private IEnumerator _flashTimer;

	private Shader spriteBrightnessShader;

	private IEnumerator _alphaTimer;

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
