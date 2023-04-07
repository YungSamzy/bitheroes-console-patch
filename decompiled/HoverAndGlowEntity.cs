using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.grid;
using com.ultrabit.bitheroes.ui.instance;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class HoverAndGlowEntity : MonoBehaviour
{
	private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();

	private List<Material> _oldMaterial = new List<Material>();

	private Material newMaterial;

	private bool _active = true;

	private bool _clickable = true;

	private GridMap _grid;

	private Color tint = new Color(1f, 1f, 1f, 0.2470588f);

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

	public void SetHover(bool clickable)
	{
		_clickable = clickable;
	}

	public void SetGrid(GridMap grid)
	{
		_grid = grid;
	}

	public void OnMouseDown()
	{
		if (_grid != null)
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			if (_grid is Instance)
			{
				(_grid as Instance).OnPointerDown(eventData);
			}
			if (_grid is Dungeon)
			{
				(_grid as Dungeon).OnPointerDown(eventData);
			}
		}
	}

	public void OnMouseUp()
	{
		if (_grid != null)
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			if (_grid is Instance)
			{
				(_grid as Instance).OnPointerUp(eventData);
			}
			if (_grid is Dungeon)
			{
				(_grid as Dungeon).OnPointerUp(eventData);
			}
		}
	}

	public void Brightness(float value)
	{
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			if (_spriteRenderers[i] != null)
			{
				_spriteRenderers[i].material.SetFloat("Brightness", value);
			}
		}
	}

	public void OnMouseOver()
	{
		if (!_active || !_clickable)
		{
			return;
		}
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			if (_spriteRenderers[i] != null)
			{
				_spriteRenderers[i].material = newMaterial;
			}
		}
	}

	public void OnMouseExit()
	{
		if (!_active || !_clickable)
		{
			return;
		}
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			if (_spriteRenderers[i] != null)
			{
				_spriteRenderers[i].material = _oldMaterial[i];
			}
		}
	}

	public void GetTargetAssets(List<SpriteRenderer> renderers = null)
	{
		if (newMaterial == null)
		{
			newMaterial = GameData.instance.main.assetLoader.GetAsset<Material>("Shader/SpriteGlowBrightness");
		}
		if (renderers != null)
		{
			_spriteRenderers = renderers;
		}
		else
		{
			_spriteRenderers = new List<SpriteRenderer>(base.gameObject.GetComponentsInChildren<SpriteRenderer>(includeInactive: true));
		}
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			_oldMaterial.Add(_spriteRenderers[i].material);
		}
	}

	public void MakeGray()
	{
		_active = false;
		Shader shader = Shader.Find("Sprites/GrayscaleSprites");
		_oldMaterial.Clear();
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			_spriteRenderers[i].material.shader = shader;
			_oldMaterial.Add(_spriteRenderers[i].material);
		}
	}

	private void RemoveAddedSpriteMaterials()
	{
	}

	private void OnDestroy()
	{
	}
}
