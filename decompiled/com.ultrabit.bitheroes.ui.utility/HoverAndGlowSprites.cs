using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.grid;
using com.ultrabit.bitheroes.ui.instance;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.ui.utility;

public class HoverAndGlowSprites : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	private const float GLOW_VALUE_MAX = 0.3f;

	public const float BRIGHT_VALUE_SPRITES = 0.3f;

	private List<Material> _spriteOgMaterials = new List<Material>();

	private List<Material> _spriteNewMaterials = new List<Material>();

	private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();

	private Shader spriteBrightnessShader;

	private List<Material> _meshOgMaterials = new List<Material>();

	private List<Material> _meshNewMaterials = new List<Material>();

	private List<MeshRenderer> _meshRenderers = new List<MeshRenderer>();

	private bool _active = true;

	private bool _glowing;

	private float _glowValue;

	private bool _clickable;

	private bool _canGlow = true;

	private GridMap _grid;

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

	private void Start()
	{
		D.Log("Initializing");
	}

	public void SetGrid(GridMap grid)
	{
		_grid = grid;
	}

	public void SetHover(bool clickable)
	{
		_clickable = clickable;
	}

	public void OnPointerDown(PointerEventData pointer)
	{
		if (_grid != null)
		{
			if (_grid is Instance)
			{
				(_grid as Instance).OnPointerDown(pointer);
			}
			if (_grid is Dungeon)
			{
				(_grid as Dungeon).OnPointerDown(pointer);
			}
		}
	}

	public void OnPointerUp(PointerEventData pointer)
	{
		if (_grid != null)
		{
			if (_grid is Instance)
			{
				(_grid as Instance).OnPointerUp(pointer);
			}
			if (_grid is Dungeon)
			{
				(_grid as Dungeon).OnPointerUp(pointer);
			}
		}
	}

	public void OnPointerEnter(PointerEventData pointer)
	{
		if (!_active || !_clickable)
		{
			return;
		}
		StopGlowing();
		_canGlow = false;
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			_spriteRenderers[i].material.SetFloat("_BrightnessAmount", 0.3f);
		}
		for (int j = 0; j < _meshRenderers.Count; j++)
		{
			if (_meshNewMaterials[j] != null)
			{
				_meshRenderers[j].material.SetVector("_CustomColorOffset", new Vector4(0.3f, 0.3f, 0.3f, 0f));
			}
		}
	}

	public void OnPointerExit(PointerEventData pointer)
	{
		if (!_active || !_clickable)
		{
			return;
		}
		_canGlow = true;
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			_spriteRenderers[i].material.SetFloat("_BrightnessAmount", 0f);
		}
		for (int j = 0; j < _meshRenderers.Count; j++)
		{
			if (_meshNewMaterials[j] != null)
			{
				_meshRenderers[j].material.SetVector("_CustomColorOffset", new Vector4(0f, 0f, 0f, 0f));
			}
		}
	}

	public void GetTargetAssets()
	{
		if (spriteBrightnessShader == null)
		{
			spriteBrightnessShader = GameData.instance.main.assetLoader.GetAsset<Shader>("Shader/SpriteBrightness");
		}
		if (_spriteRenderers.Count > 0)
		{
			RemoveAddedSpriteMaterials();
		}
		if (_meshRenderers.Count > 0)
		{
			RemoveAddedMeshMaterials();
		}
		base.gameObject.GetComponentsInChildren(_spriteRenderers);
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			_spriteOgMaterials.Add(_spriteRenderers[i].sharedMaterial);
			_spriteNewMaterials.Add(new Material(_spriteOgMaterials[i]));
			_spriteNewMaterials[i].shader = spriteBrightnessShader;
			_spriteRenderers[i].material = _spriteNewMaterials[i];
		}
		base.gameObject.GetComponentsInChildren(_meshRenderers);
		for (int j = 0; j < _meshRenderers.Count; j++)
		{
			_meshOgMaterials.Add(_meshRenderers[j].sharedMaterial);
			if (_meshRenderers[j].sharedMaterial != null)
			{
				_meshNewMaterials.Add(new Material(_meshOgMaterials[j]));
				_meshNewMaterials[j].name = "SpriteHoverOrGlow";
				_meshRenderers[j].material = _meshNewMaterials[j];
			}
			else
			{
				_meshNewMaterials.Add(null);
			}
		}
	}

	public void StartGlow()
	{
		if ((_spriteRenderers.Count != 0 || _meshRenderers.Count != 0) && !_glowing && _active && _canGlow)
		{
			_glowing = true;
			TweenGlowIncreased();
		}
	}

	public void StopGlowing()
	{
		if (!_glowing)
		{
			return;
		}
		_glowing = false;
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			_spriteRenderers[i].material.SetFloat("_BrightnessAmount", 0f);
		}
		for (int j = 0; j < _meshRenderers.Count; j++)
		{
			if (_meshNewMaterials[j] != null)
			{
				_meshRenderers[j].material.SetVector("_CustomColorOffset", new Vector4(0f, 0f, 0f, 0f));
			}
		}
	}

	public void TweenGlowIncreased()
	{
		float initAlpha = 0f;
		DOTween.To(() => initAlpha, delegate(float x)
		{
			initAlpha = x;
		}, 0.3f, 1f).SetEase(Ease.OutCubic).OnUpdate(delegate
		{
			_glowValue = initAlpha;
			for (int i = 0; i < _spriteRenderers.Count; i++)
			{
				_spriteRenderers[i].material.SetFloat("_BrightnessAmount", _glowValue);
			}
			for (int j = 0; j < _meshRenderers.Count; j++)
			{
				if (_meshNewMaterials[j] != null)
				{
					_meshRenderers[j].material.SetVector("_CustomColorOffset", new Vector4(_glowValue, _glowValue, _glowValue, 0f));
				}
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
		}, 0f, 1f).SetEase(Ease.OutCubic).OnUpdate(delegate
		{
			_glowValue = initAlpha;
			for (int i = 0; i < _spriteRenderers.Count; i++)
			{
				_spriteRenderers[i].material.SetFloat("_BrightnessAmount", _glowValue);
			}
			for (int j = 0; j < _meshRenderers.Count; j++)
			{
				if (_meshNewMaterials[j] != null)
				{
					_meshRenderers[j].material.SetVector("_CustomColorOffset", new Vector4(_glowValue, _glowValue, _glowValue, 0f));
				}
			}
		})
			.OnComplete(delegate
			{
				if (_glowing)
				{
					TweenGlowIncreased();
				}
			});
	}

	public void MakeGray()
	{
		Shader shader = Shader.Find("Sprites/GrayscaleSprites");
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			_spriteRenderers[i].material.shader = shader;
		}
	}

	private void RemoveAddedSpriteMaterials()
	{
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			if (!(_spriteRenderers[i] == null))
			{
				_spriteRenderers[i].sharedMaterial = _spriteOgMaterials[i];
				Object.Destroy(_spriteNewMaterials[i]);
			}
		}
		_spriteRenderers.Clear();
		_spriteOgMaterials.Clear();
		_spriteNewMaterials.Clear();
	}

	private void RemoveAddedMeshMaterials()
	{
		for (int i = 0; i < _meshRenderers.Count; i++)
		{
			_meshRenderers[i].sharedMaterial = _meshOgMaterials[i];
			Object.Destroy(_meshNewMaterials[i]);
		}
		_meshOgMaterials.Clear();
		_meshNewMaterials.Clear();
		_meshRenderers.Clear();
	}

	private void OnDestroy()
	{
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			if (_spriteRenderers[i] != null)
			{
				_spriteRenderers[i].material.SetFloat("_BrightnessAmount", 0f);
			}
		}
		for (int j = 0; j < _meshRenderers.Count; j++)
		{
			if (_meshNewMaterials[j] != null)
			{
				_meshRenderers[j].material.SetVector("_CustomColorOffset", new Vector4(0f, 0f, 0f, 0f));
			}
		}
		if (_spriteRenderers.Count > 0)
		{
			RemoveAddedSpriteMaterials();
		}
		if (_meshRenderers.Count > 0)
		{
			RemoveAddedMeshMaterials();
		}
	}
}
