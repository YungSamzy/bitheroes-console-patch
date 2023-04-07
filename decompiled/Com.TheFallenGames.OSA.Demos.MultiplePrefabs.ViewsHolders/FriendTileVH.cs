using System;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.friend;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui;
using com.ultrabit.bitheroes.ui.avatar;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.friend;
using com.ultrabit.bitheroes.ui.utility;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;

public class FriendTileVH : BaseVH
{
	private FriendTileModel _model;

	private TextMeshProUGUI nameText;

	private TextMeshProUGUI levelText;

	private TextMeshProUGUI loginText;

	private RectTransform offlineIcon;

	private Button button;

	private Button acceptButton;

	private TextMeshProUGUI acceptText;

	private Button declineButton;

	private TextMeshProUGUI declineText;

	private Image uiFrameImage;

	private HoverImage uiFrameHover;

	private AvatarBackground avatarBackground;

	private AvatarGenerationBanner avatarGenerationBanner;

	private RectTransform placeholderAsset;

	private SpriteMask assetMask0;

	private SpriteMask assetMask1;

	private SpriteMask assetMask2;

	private SpriteMask assetMask3;

	private int uILayerID;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<Button>("TileBackground", out button);
		root.GetComponentAtPath<Image>("TileBackground", out uiFrameImage);
		root.GetComponentAtPath<HoverImage>("TileBackground", out uiFrameHover);
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out nameText);
		root.GetComponentAtPath<TextMeshProUGUI>("LoginTxt", out loginText);
		root.GetComponentAtPath<TextMeshProUGUI>("LevelTxt", out levelText);
		root.GetComponentAtPath<RectTransform>("OfflineIcon", out offlineIcon);
		root.GetComponentAtPath<Button>("AcceptBtn", out acceptButton);
		root.GetComponentAtPath<TextMeshProUGUI>("AcceptBtn/BtnTxt", out acceptText);
		root.GetComponentAtPath<Button>("DenyBtn", out declineButton);
		root.GetComponentAtPath<TextMeshProUGUI>("DenyBtn/BtnTxt", out declineText);
		root.GetComponentAtPath<RectTransform>("PlaceholderAsset", out placeholderAsset);
		root.GetComponentAtPath<AvatarBackground>("AvatarBackground", out avatarBackground);
		root.GetComponentAtPath<AvatarGenerationBanner>("AvatarGenerationBanner", out avatarGenerationBanner);
		root.GetComponentAtPath<SpriteMask>("Masks/AssetMask0", out assetMask0);
		root.GetComponentAtPath<SpriteMask>("Masks/AssetMask1", out assetMask1);
		root.GetComponentAtPath<SpriteMask>("Masks/AssetMask2", out assetMask2);
		root.GetComponentAtPath<SpriteMask>("Masks/AssetMask3", out assetMask3);
		uILayerID = SortingLayer.NameToID("UI");
	}

	public override bool CanPresentModelType(Type modelType)
	{
		return modelType == typeof(FriendTileModel);
	}

	public override void UpdateViews(BaseModel model)
	{
		base.UpdateViews(model);
		_model = model as FriendTileModel;
		if (_model.requestData == null)
		{
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(delegate
			{
				OnItemClick(_model.id);
			});
		}
		else
		{
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(delegate
			{
				OnItemClick(_model.requestData.characterData.charID);
			});
			if (acceptButton != null)
			{
				acceptButton.onClick.RemoveAllListeners();
				acceptButton.onClick.AddListener(delegate
				{
					OnAccept(_model.requestData);
				});
				acceptText.text = Language.GetString("ui_accept");
			}
			if (declineButton != null)
			{
				declineButton.onClick.RemoveAllListeners();
				declineButton.onClick.AddListener(delegate
				{
					OnDecline(_model.requestData);
				});
				declineText.text = Language.GetString("ui_decline");
			}
		}
		nameText.text = _model.name;
		levelText.text = _model.level;
		loginText.text = _model.login;
		offlineIcon.gameObject.SetActive(!_model.online);
		FriendWindow componentInParent = root.GetComponentInParent<FriendWindow>();
		FriendRequestWindow componentInParent2 = root.GetComponentInParent<FriendRequestWindow>();
		WindowsMain windowsMain = null;
		if (componentInParent != null)
		{
			windowsMain = componentInParent;
		}
		else if (componentInParent2 != null)
		{
			windowsMain = componentInParent2;
		}
		for (int i = 0; i < placeholderAsset.transform.childCount; i++)
		{
			if (windowsMain is FriendWindow)
			{
				(windowsMain as FriendWindow).DestroySomething(placeholderAsset.GetChild(i).gameObject);
			}
			else if (windowsMain is FriendRequestWindow)
			{
				(windowsMain as FriendRequestWindow).DestroySomething(placeholderAsset.GetChild(i).gameObject);
			}
		}
		uiFrameImage.color = (_model.characterData.isIMXG0 ? _model.characterData.nftRarityColor : Color.white);
		avatarGenerationBanner.gameObject.SetActive(_model.characterData.isIMXG0);
		avatarBackground.gameObject.SetActive(_model.characterData.isIMXG0);
		if (_model.characterData.isIMXG0)
		{
			avatarBackground.LoadDetails(_model.characterData.nftBackground, _model.characterData.nftFrameSimple, _model.characterData.nftFrameSeparator);
			avatarGenerationBanner.LoadDetails(_model.characterData.nftGeneration, _model.characterData.nftRarity);
		}
		CharacterDisplay characterDisplay = _model.characterData.toCharacterDisplay(2f / windowsMain.panel.transform.localScale.x);
		characterDisplay.transform.SetParent(placeholderAsset.transform, worldPositionStays: false);
		characterDisplay.transform.localPosition = Vector3.zero;
		characterDisplay.HideMaskedElements();
		characterDisplay.SetLocalPosition(new Vector3(0f, -63f, 0f));
		com.ultrabit.bitheroes.model.utility.Util.ChangeLayer(characterDisplay.transform, "UI");
		SpriteRenderer[] componentsInChildren = characterDisplay.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		ParticleSystemRenderer[] componentsInChildren2 = characterDisplay.GetComponentsInChildren<ParticleSystemRenderer>(includeInactive: true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		int num = 2 + ItemIndex + windowsMain.sortingLayer;
		avatarGenerationBanner.SetSpriteMaskRange(num, num - 1);
		SpriteMask[] array = new SpriteMask[4] { assetMask0, assetMask1, assetMask2, assetMask3 };
		for (int j = 0; j < array.Length; j++)
		{
			SetMaskSortingOrder(array[j], uILayerID, num, uILayerID, num - 1);
		}
		SortingGroup sortingGroup = characterDisplay.gameObject.AddComponent<SortingGroup>();
		if (sortingGroup != null && sortingGroup.enabled)
		{
			sortingGroup.sortingLayerName = "UI";
			sortingGroup.sortingOrder = num;
		}
		static void SetMaskSortingOrder(SpriteMask mask, int frontSortingLayerID, int frontSortingOrder, int backSortingLayerID, int backSortingOrder)
		{
			if (!(mask == null))
			{
				mask.frontSortingLayerID = frontSortingLayerID;
				mask.frontSortingOrder = frontSortingOrder;
				mask.backSortingLayerID = backSortingLayerID;
				mask.backSortingOrder = backSortingOrder;
			}
		}
	}

	public void OnAccept(RequestData requestData)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoAcceptRequest(requestData.characterData.charID);
	}

	public void OnDecline(RequestData requestData)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoDenyRequest(requestData.characterData.charID);
	}

	public void OnItemClick(int id)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowPlayer(id);
	}
}
