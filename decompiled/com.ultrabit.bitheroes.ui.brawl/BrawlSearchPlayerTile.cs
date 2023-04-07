using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.avatar;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.brawl;

public class BrawlSearchPlayerTile : MonoBehaviour
{
	public Image bg;

	public RectTransform placeholderAsset;

	public Image statBG;

	public Image powerIcon;

	public Image staminaIcon;

	public Image agilityIcon;

	public Image loadingIcon;

	public AvatarBackground avatarBackground;

	public AvatarGenerationBanner avatarGenerationBanner;

	public SpriteMask[] assetMasks;

	private BrawlPlayer _player;

	private int _sortLayer;

	private bool _loaded;

	public Button button;

	public void LoadDetails(BrawlPlayer player, int sortLayer)
	{
		_player = player;
		_sortLayer = sortLayer;
		LoadAssets();
	}

	public void LoadAssets()
	{
		if (_loaded)
		{
			return;
		}
		_loaded = true;
		bool flag = _player != null;
		SpriteMask[] array = assetMasks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(flag);
		}
		avatarGenerationBanner.gameObject.SetActive(value: false);
		statBG.gameObject.SetActive(flag);
		if (!flag)
		{
			placeholderAsset.gameObject.SetActive(value: false);
			powerIcon.gameObject.SetActive(value: false);
			staminaIcon.gameObject.SetActive(value: false);
			agilityIcon.gameObject.SetActive(value: false);
			loadingIcon.gameObject.SetActive(value: false);
			Util.SetImageAlpha(bg, alpha: true);
			button.enabled = false;
			Object.Destroy(GetComponent<HoverImages>());
			return;
		}
		switch (_player.characterData.getHighestStat())
		{
		case 0:
			staminaIcon.gameObject.SetActive(value: false);
			agilityIcon.gameObject.SetActive(value: false);
			break;
		case 1:
			powerIcon.gameObject.SetActive(value: false);
			agilityIcon.gameObject.SetActive(value: false);
			break;
		case 2:
			powerIcon.gameObject.SetActive(value: false);
			staminaIcon.gameObject.SetActive(value: false);
			break;
		}
		bg.color = (_player.characterData.isIMXG0 ? _player.characterData.nftRarityColor : Color.white);
		avatarGenerationBanner.gameObject.SetActive(_player.characterData.isIMXG0);
		avatarBackground.gameObject.SetActive(_player.characterData.isIMXG0);
		if (_player.characterData.isIMXG0)
		{
			avatarBackground.LoadDetails(_player.characterData.nftBackground, _player.characterData.nftFrameSimple, _player.characterData.nftFrameSeparator);
			avatarGenerationBanner.LoadDetails(_player.characterData.nftGeneration, _player.characterData.nftRarity);
		}
		CharacterDisplay characterDisplay = _player.characterData.toCharacterDisplay(0.66666f);
		characterDisplay.transform.SetParent(placeholderAsset.transform, worldPositionStays: false);
		characterDisplay.transform.localPosition = Vector3.zero;
		characterDisplay.SetLocalPosition(new Vector3(0f, -63f, 0f));
		Util.ChangeLayer(characterDisplay.transform, "UI");
		SpriteRenderer[] componentsInChildren = characterDisplay.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		ParticleSystemRenderer[] componentsInChildren2 = characterDisplay.GetComponentsInChildren<ParticleSystemRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		array = assetMasks;
		foreach (SpriteMask obj in array)
		{
			obj.frontSortingLayerID = SortingLayer.NameToID("UI");
			obj.frontSortingOrder = _sortLayer;
			obj.backSortingLayerID = SortingLayer.NameToID("UI");
			obj.backSortingOrder = _sortLayer - 1;
		}
		int num = 2 + _sortLayer;
		avatarGenerationBanner.SetSpriteMaskRange(num, num - 1);
		SortingGroup sortingGroup = characterDisplay.gameObject.AddComponent<SortingGroup>();
		if (sortingGroup.enabled)
		{
			sortingGroup.sortingLayerName = "UI";
			sortingGroup.sortingOrder = _sortLayer;
		}
		if (loadingIcon != null)
		{
			loadingIcon.gameObject.SetActive(value: false);
		}
		button.enabled = true;
	}

	public void OnTileClick()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowPlayer(_player.characterData.charID);
	}
}
