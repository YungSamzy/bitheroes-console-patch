using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.language;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.dialog;

public class DialogFrame : MonoBehaviour
{
	public TextMeshProUGUI nameTxt;

	public Image nameBg;

	public TextMeshProUGUI contentTxt;

	public Image placeholderAsset;

	public SpriteMask assetMask;

	public Image loadingIcon;

	public Button arrow;

	public Image _contentBG;

	private DialogFrameRef _frameRef;

	private DialogPopup _dialogPopup;

	private DialogFrameContentRef _contentRef;

	private Transform _asset;

	private AsianLanguageFontManager asianLangManager;

	public DialogFrameRef frameRef => _frameRef;

	public void LoadDetails(DialogFrameRef frameRef, DialogPopup dialogPopup)
	{
		_frameRef = frameRef;
		_dialogPopup = dialogPopup;
		if (frameRef.position == 2)
		{
			nameTxt.rectTransform.localPosition = new Vector2(nameTxt.rectTransform.localPosition.x * -1f, nameTxt.rectTransform.localPosition.y);
			nameBg.rectTransform.localPosition = new Vector2(nameBg.rectTransform.localPosition.x * -1f, nameBg.rectTransform.localPosition.y);
			contentTxt.rectTransform.localPosition = new Vector2(contentTxt.rectTransform.localPosition.x * -1f, contentTxt.rectTransform.localPosition.y);
			placeholderAsset.rectTransform.localPosition = new Vector2(placeholderAsset.rectTransform.localPosition.x * -1f, placeholderAsset.rectTransform.localPosition.y);
			loadingIcon.rectTransform.localPosition = new Vector2(loadingIcon.rectTransform.localPosition.x * -1f, loadingIcon.rectTransform.localPosition.y);
		}
		string text = (frameRef.portrait ? GameData.instance.PROJECT.character.name : Util.ParseString(frameRef.name));
		if (text.Length > 0)
		{
			nameTxt.text = Util.colorString(text, "#FFFF00");
		}
		else
		{
			nameTxt.gameObject.SetActive(value: false);
			nameBg.gameObject.SetActive(value: false);
		}
		switch (_frameRef.position)
		{
		case 1:
			contentTxt.rectTransform.localPosition = new Vector2(contentTxt.rectTransform.localPosition.x + (float)(_frameRef.textOffset / 3 / 2), contentTxt.rectTransform.localPosition.y);
			contentTxt.rectTransform.sizeDelta = new Vector2(contentTxt.rectTransform.sizeDelta.x - (float)(_frameRef.textOffset / 3), contentTxt.rectTransform.sizeDelta.y);
			break;
		case 2:
			contentTxt.rectTransform.localPosition = new Vector2(contentTxt.rectTransform.localPosition.x - (float)(_frameRef.textOffset / 3 / 2), contentTxt.rectTransform.localPosition.y);
			contentTxt.rectTransform.sizeDelta = new Vector2(contentTxt.rectTransform.sizeDelta.x - (float)(_frameRef.textOffset / 3), contentTxt.rectTransform.sizeDelta.y);
			break;
		}
		if ((bool)placeholderAsset)
		{
			placeholderAsset.gameObject.SetActive(value: false);
			if (frameRef.portrait)
			{
				_asset = GameData.instance.PROJECT.character.toCharacterDisplay(8f, displayMount: false, enableLoading: false).transform;
				GameData.instance.PROJECT.character.toCharacterData();
			}
			else
			{
				_asset = _frameRef.displayRef.getAsset(center: true, 8f);
			}
			OnAssetLoaded();
		}
		_contentBG.gameObject.SetActive(value: false);
		ShowContent(_frameRef.getContent(0));
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = base.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
	}

	public void UpdateLayers()
	{
		if (_asset != null && _asset.GetComponent<SortingGroup>() != null)
		{
			_asset.GetComponent<SortingGroup>().sortingOrder = 1 + _dialogPopup.sortingLayer;
			StartCoroutine(WaitToFixOrder(contentTxt.GetComponent<Canvas>(), _contentBG.GetComponent<Canvas>(), 1 + _dialogPopup.sortingLayer));
			assetMask.frontSortingLayerID = SortingLayer.NameToID("UI");
			assetMask.frontSortingOrder = 1 + _dialogPopup.sortingLayer;
			assetMask.backSortingLayerID = SortingLayer.NameToID("UI");
			assetMask.backSortingOrder = _dialogPopup.sortingLayer;
		}
	}

	private void OnAssetLoaded()
	{
		loadingIcon.gameObject.SetActive(value: false);
		placeholderAsset.gameObject.SetActive(value: true);
		_asset.SetParent(placeholderAsset.transform, worldPositionStays: false);
		_asset.localPosition = Vector3.zero;
		_asset.localScale = new Vector3(_asset.localScale.x / 3f, _asset.localScale.y / 3f, 1f);
		if (_frameRef.position == 1)
		{
			_asset.transform.localScale = new Vector3(_asset.transform.localScale.x, _asset.transform.localScale.y, _asset.transform.localScale.z);
		}
		else
		{
			_asset.transform.localScale = new Vector3(0f - _asset.transform.localScale.x, _asset.transform.localScale.y, _asset.transform.localScale.z);
		}
		Util.ChangeLayer(_asset, "UI");
		SortingGroup sortingGroup = _asset.gameObject.AddComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = 1 + _dialogPopup.sortingLayer;
		SpriteRenderer[] componentsInChildren = _asset.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		Mask[] componentsInChildren2 = _asset.GetComponentsInChildren<Mask>(includeInactive: true);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].gameObject.SetActive(value: false);
		}
		SpriteRenderer[] array = componentsInChildren;
		foreach (SpriteRenderer spriteRenderer in array)
		{
			if (spriteRenderer.maskInteraction == SpriteMaskInteraction.VisibleInsideMask)
			{
				spriteRenderer.gameObject.SetActive(value: false);
			}
			else
			{
				spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
			}
		}
		assetMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		assetMask.frontSortingOrder = sortingGroup.sortingOrder;
		assetMask.backSortingLayerID = SortingLayer.NameToID("UI");
		assetMask.backSortingOrder = sortingGroup.sortingOrder - 1;
		_asset.transform.localPosition += new Vector3(0f, -33f, 0f);
		if ((bool)_asset.GetComponentInChildren<CharacterDisplay>())
		{
			_asset.transform.localPosition += new Vector3(0f, 37f, 0f);
		}
		if (_frameRef.displayRef != null)
		{
			_asset.transform.localPosition += new Vector3(_frameRef.displayRef.point.x / 3f, (0f - _frameRef.displayRef.point.y) / 3f, 0f);
		}
		if (_asset.GetComponent<SWFAsset>() != null)
		{
			_asset.GetComponent<SWFAsset>().PlayAnimation("idle");
		}
		Canvas canvas = contentTxt.GetComponent<Canvas>();
		if (canvas == null)
		{
			canvas = contentTxt.gameObject.AddComponent<Canvas>();
		}
		canvas.overrideSorting = true;
		Canvas canvas2 = _contentBG.GetComponent<Canvas>();
		if (canvas2 == null)
		{
			canvas2 = _contentBG.gameObject.AddComponent<Canvas>();
		}
		canvas2.overrideSorting = true;
		StartCoroutine(WaitToFixOrder(canvas, canvas2, sortingGroup.sortingOrder));
	}

	private IEnumerator WaitToFixOrder(Canvas textCanvas, Canvas textBGCanvas, int lastPlace)
	{
		yield return new WaitForSeconds(0.1f);
		textCanvas.sortingLayerName = "UI";
		textCanvas.sortingOrder = lastPlace + 2;
		textBGCanvas.sortingLayerName = "UI";
		textBGCanvas.sortingOrder = lastPlace + 1;
	}

	public void ShowContent(DialogFrameContentRef contentRef)
	{
		_contentRef = contentRef;
		if (_contentRef != null)
		{
			contentTxt.text = Util.ParseString(_contentRef.text);
			if (_frameRef.textBG)
			{
				ShowContentBG();
			}
		}
	}

	private void ShowContentBG()
	{
		contentTxt.ForceMeshUpdate();
		_contentBG.rectTransform.sizeDelta = contentTxt.GetRenderedValues(onlyVisibleCharacters: true);
		_contentBG.rectTransform.sizeDelta += new Vector2(3f, 3f);
		_contentBG.rectTransform.localPosition = new Vector2(contentTxt.rectTransform.localPosition.x - contentTxt.rectTransform.sizeDelta.x / 2f - 1.5f, contentTxt.rectTransform.localPosition.y + contentTxt.rectTransform.sizeDelta.y / 2f + 1.5f);
		_contentBG.color = new Color(0f, 0f, 0f, 0.3f);
		_contentBG.gameObject.SetActive(value: true);
	}

	public DialogFrameContentRef GetNextContent()
	{
		return _frameRef.getContent(_contentRef.index + 1);
	}
}
