using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.fishing;

public class FishingCaptureWindow : WindowsMain
{
	public TextMeshProUGUI successTxt;

	public TextMeshProUGUI itemTxt;

	public TextMeshProUGUI weightNameTxt;

	public TextMeshProUGUI weightTxt;

	public RectTransform placeholderIcon;

	private FishingItemRef _itemRef;

	private List<ItemData> _items;

	private bool _closing;

	private CanvasGroup canvasGroup;

	public override void Start()
	{
		base.Start();
	}

	public void LoadDetails(FishingItemRef itemRef, List<ItemData> items, int weight)
	{
		Disable();
		_itemRef = itemRef;
		_items = items;
		canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
		successTxt.text = Language.GetString("battle_capture_success");
		itemTxt.text = itemRef.itemRef.coloredName;
		FishingEventRef currentEventRef = FishingEventBook.GetCurrentEventRef();
		if (currentEventRef != null && weight > 0)
		{
			weightNameTxt.text = Language.GetString("ui_weight") + ":";
			weightTxt.text = Language.GetString("ui_weight_value", new string[1] { Util.NumberFormat((float)weight / currentEventRef.divider, abbreviate: false) });
		}
		else
		{
			weightNameTxt.gameObject.SetActive(value: false);
			weightTxt.gameObject.SetActive(value: false);
		}
		Transform obj = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/item/" + typeof(ItemIcon).Name));
		obj.SetParent(placeholderIcon, worldPositionStays: false);
		obj.localPosition = Vector3.zero;
		obj.GetComponent<RectTransform>().localScale = new Vector3(2f, 2f, 1f);
		ItemIcon itemIcon = obj.gameObject.AddComponent<ItemIcon>();
		itemIcon.isThumbnail = true;
		itemIcon.SetItemData(new ItemData(itemRef.itemRef, 1), locked: false, 1);
		itemIcon.SetItemActionType(0);
		float delay = 0.1f;
		float duration = 0.5f;
		AddBG(0.9f);
		ListenForBack(OnClose);
		ListenForForward(OnClose);
		canvasGroup.alpha = 0f;
		StartCoroutine(StartAnimation(delay, duration));
		CreateWindow(closeWord: true, Language.GetString("ui_trade"), scroll: false);
		Enable();
	}

	private IEnumerator StartAnimation(float delay, float duration)
	{
		yield return new WaitForSeconds(delay);
		DoAlpha(1f, duration);
	}

	public void DoAlpha(float finalAlpha, float duration)
	{
		float currentAlpha = canvasGroup.alpha;
		DOTween.To(() => currentAlpha, delegate(float x)
		{
			currentAlpha = x;
		}, finalAlpha, duration).SetEase(Ease.Linear).OnUpdate(delegate
		{
			canvasGroup.alpha = currentAlpha;
		})
			.OnComplete(delegate
			{
				if (finalAlpha == 0f)
				{
					DoDestroy();
				}
			});
	}

	public override void OnClose()
	{
		if (!_closing)
		{
			_closing = true;
			base.OnClose();
			GameData.instance.PROJECT.character.addItems(_items);
			GameData.instance.windowGenerator.ShowItems(_items, compare: true, added: true);
			DoAlpha(0f, 0.5f);
		}
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}
}
