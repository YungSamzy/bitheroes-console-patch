using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.avatar;
using com.ultrabit.bitheroes.ui.character;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HeroTileRenderTexture : MonoBehaviour
{
	public Action<HeroTileRenderTexture> OnSelect;

	private const float DISPLAY_OFFSET = -14f;

	private const float DISPLAY_MOUNT_OFFSET = 8f;

	private const float PUPPET_MAX_WORLD_Y_SIZE = 40f;

	[SerializeField]
	private Image freezeOverlayPrefab;

	[SerializeField]
	private RawImage heroRawImagePrefab;

	[SerializeField]
	private Camera heroRenderCameraPrefab;

	[SerializeField]
	private RenderTexture heroRenderTexturePreset;

	[SerializeField]
	private Transform placeholderDisplay;

	[SerializeField]
	private GameObject frameSelected;

	[SerializeField]
	private GameObject freeze;

	[SerializeField]
	private GameObject baseBackground;

	[SerializeField]
	private TextMeshProUGUI label;

	private Image _freezeOverlay;

	private RawImage _heroRawImage;

	private Camera _heroRenderCamera;

	[SerializeField]
	private AvatarBackground nftBackground;

	[SerializeField]
	private AvatarGenerationBanner nftGenBanner;

	public static List<HeroTileRenderTexture> HERO_TILES = new List<HeroTileRenderTexture>();

	private int _currentID;

	private Dictionary<string, Color> originalColors = new Dictionary<string, Color>();

	private bool _isFrozen;

	private bool _isSelected;

	private CharacterData _characterData;

	private CharacterDisplay _display;

	public bool isSelected
	{
		get
		{
			return _isSelected;
		}
		set
		{
			_isSelected = value;
			frameSelected.SetActive(value);
			if (!value)
			{
				return;
			}
			foreach (HeroTileRenderTexture hERO_TILE in HERO_TILES)
			{
				if (!hERO_TILE.Equals(this))
				{
					hERO_TILE.isSelected = false;
				}
			}
			OnSelect?.Invoke(this);
		}
	}

	public CharacterData characterData => _characterData;

	public bool hasData => _characterData != null;

	private void SetFrozen(bool value)
	{
		_isFrozen = value;
		freeze.SetActive(_isFrozen);
		if (!(nftBackground.frameGameObject == null))
		{
			if (_freezeOverlay == null)
			{
				_freezeOverlay = UnityEngine.Object.Instantiate(freezeOverlayPrefab, nftBackground.transform);
				_freezeOverlay.transform.SetSiblingIndex(nftBackground.frameGameObject.transform.parent.GetSiblingIndex());
			}
			_freezeOverlay.gameObject.SetActive(_isFrozen);
			SetAllAnimators(!value);
		}
	}

	private void Awake()
	{
		SetFrozen(value: false);
		HERO_TILES.Add(this);
		_currentID = HERO_TILES.Count - 1;
		GetComponent<Button>().onClick.AddListener(OnClickTile);
	}

	private void OnClickTile()
	{
		if (!_isFrozen)
		{
			isSelected = true;
		}
	}

	public void LoadDetails(CharacterData characterData = null, bool selected = false)
	{
		SetCharacterData(characterData);
		isSelected = selected;
		if (characterData != null)
		{
			SetFrozen(characterData.nftState == Character.NFTState.bitverseHeroFrozen);
		}
	}

	private void SetCharacterData(CharacterData characterData)
	{
		_characterData = characterData;
		placeholderDisplay.gameObject.SetActive(_characterData == null);
		label.text = ((_characterData == null) ? Language.GetString("ui_new_hero") : (string.IsNullOrEmpty(_characterData.name) ? _characterData.nftName : _characterData.name));
		if (_characterData != null)
		{
			SetDisplay(_characterData.toCharacterDisplay(1f));
			if (!_characterData.isIMXG0)
			{
				_heroRawImage = UnityEngine.Object.Instantiate(heroRawImagePrefab, baseBackground.transform);
				_heroRawImage.transform.SetAsLastSibling();
			}
			else
			{
				LoadIMXDetails();
				_heroRawImage = UnityEngine.Object.Instantiate(heroRawImagePrefab, nftBackground.transform);
				_heroRawImage.transform.SetSiblingIndex(nftBackground.backgroundGameObject.transform.parent.GetSiblingIndex() + 1);
			}
			_heroRenderCamera.targetTexture = UnityEngine.Object.Instantiate(heroRenderTexturePreset);
			_heroRawImage.texture = _heroRenderCamera.targetTexture;
		}
	}

	private void LoadIMXDetails()
	{
		nftBackground.gameObject.SetActive(value: true);
		GameObject gameObject = _characterData.nftBackground;
		GameObject nftFrame = _characterData.nftFrame;
		baseBackground.gameObject.SetActive(nftFrame == null);
		if (gameObject != null || nftFrame != null)
		{
			nftBackground.LoadDetails(gameObject, nftFrame);
		}
		nftGenBanner.gameObject.SetActive(value: true);
		nftGenBanner.LoadDetails(_characterData.nftGeneration, _characterData.nftRarity);
	}

	private void SetDisplay(CharacterDisplay display)
	{
		if (_display != null)
		{
			CharacterDisplay display2 = _display;
			display2.BOUNDS_UPDATED = (Action)Delegate.Remove(display2.BOUNDS_UPDATED, (Action)delegate
			{
				CheckAndUpdatePuppetSize();
			});
			UnityEngine.Object.Destroy(_display.gameObject);
			_display = null;
		}
		_display = display;
		CharacterDisplay display3 = _display;
		display3.BOUNDS_UPDATED = (Action)Delegate.Combine(display3.BOUNDS_UPDATED, (Action)delegate
		{
			CheckAndUpdatePuppetSize();
		});
		_display.transform.SetParent(base.transform, worldPositionStays: false);
		_heroRenderCamera = UnityEngine.Object.Instantiate(heroRenderCameraPrefab, _display.transform);
		UpdateCharacterDisplayPosition();
		Util.ChangeLayer(_display.transform, "UI");
		SortingGroup sortingGroup = _display.gameObject.AddComponent<SortingGroup>();
		if (sortingGroup.enabled)
		{
			sortingGroup.sortingLayerName = "UI";
			sortingGroup.sortingOrder = 2 + GetComponentInParent<WindowsMain>().sortingLayer + _currentID;
		}
		if (_display.hasMountEquipped())
		{
			_ = _display.GetComponentInChildren<SWFAsset>().gameObject;
		}
		else
		{
			_ = _display.characterPuppet.gameObject;
		}
	}

	private void CheckAndUpdatePuppetSize()
	{
		if (_display.bounds.size.y > 40f)
		{
			_display.transform.localScale = Vector2.one * 40f / _display.bounds.size.y;
		}
	}

	private void UpdateCharacterDisplayPosition()
	{
		if (_display.hasMountEquipped())
		{
			_display.transform.localPosition = placeholderDisplay.transform.localPosition + new Vector3(2000 + _currentID * 100, 8f, 0f);
			SWFAsset componentInChildren = _display.GetComponentInChildren<SWFAsset>();
			if (componentInChildren != null)
			{
				componentInChildren.transform.localPosition = Vector3.zero;
				componentInChildren.transform.localRotation = Quaternion.Euler(Vector3.zero);
				componentInChildren.transform.localScale = Vector3.one * 100f;
			}
		}
		else
		{
			_display.transform.localPosition = placeholderDisplay.transform.localPosition + new Vector3(2000 + _currentID * 100, -14f, 0f);
			CharacterPuppet characterPuppet = _display.characterPuppet;
			if (characterPuppet != null)
			{
				characterPuppet.transform.localPosition = new Vector3(0f, 40f, 0f);
				characterPuppet.transform.localRotation = Quaternion.Euler(Vector3.zero);
				characterPuppet.transform.localScale = Vector3.one * 100f;
			}
		}
	}

	private void SetAllAnimators(bool enabled)
	{
		if (_display == null)
		{
			return;
		}
		List<Animator> list = new List<Animator>();
		Animator[] componentsInChildren = _display.GetComponentsInChildren<Animator>(includeInactive: true);
		if (componentsInChildren != null)
		{
			list.AddRange(componentsInChildren);
		}
		if (nftBackground.backgroundGameObject != null)
		{
			componentsInChildren = nftBackground.backgroundGameObject.GetComponentsInChildren<Animator>(includeInactive: true);
			if (componentsInChildren != null)
			{
				list.AddRange(componentsInChildren);
			}
		}
		if (list == null)
		{
			return;
		}
		foreach (Animator item in list)
		{
			item.enabled = enabled;
		}
	}
}
