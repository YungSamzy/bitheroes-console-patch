using System;
using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemTile : ItemIcon
{
	public Transform itemIcon;

	public Transform ribbon;

	public Transform creditsRawIcon;

	public Transform goldRawIcon;

	public Transform goldIcon;

	public Transform creditsIcon;

	public Transform shine;

	public TextMeshProUGUI nameTxt;

	public TextMeshProUGUI percentageTxt;

	public TextMeshProUGUI costRawTxt;

	public TextMeshProUGUI costTxt;

	public TextMeshProUGUI ribbonTxt;

	public RectTransform redLine;

	public Button buttonImage;

	public Image itemIconColor;

	private ItemRef _itemRef;

	private ShopWindow _shopWindow;

	public void Init(ItemRef itemRef, ShopWindow shopWindow)
	{
		if (!(itemRef == null))
		{
			_shopWindow = shopWindow;
			nameTxt.text = itemRef.coloredName;
			ribbonTxt.text = Language.GetString("ui_sale").ToUpper();
			setItem(itemRef);
			DoUpdate();
		}
	}

	public void setShine(bool vis)
	{
		shine.gameObject.SetActive(vis);
	}

	public void setItem(ItemRef itemRef)
	{
		_itemRef = itemRef;
		updateTooltip();
		nameTxt.text = _itemRef.coloredName;
		clearAssets();
	}

	private void updateTooltip()
	{
	}

	private int getQty()
	{
		if (_itemRef.allowQty && (bool)_shopWindow)
		{
			return _shopWindow.GetQty();
		}
		return 1;
	}

	private void clearAssets()
	{
	}

	public void loadAssets()
	{
	}

	public virtual void DoUpdate()
	{
		int num = getQty();
		SetItemData(new ItemData(_itemRef, num), locked: false, -1, tintRarity: true, itemIconColor);
		SetItemActionType(_itemRef.hasContents() ? 7 : 4);
		ShopSaleRef itemSaleRef = ShopBook.GetItemSaleRef(_itemRef, GameData.instance.PROJECT.character.shopRotationID);
		PaymentData paymentData = _itemRef.getPaymentData();
		bool flag = itemSaleRef != null && paymentData == null && (double)itemSaleRef.mult != 1.0;
		string text = "";
		shine.gameObject.SetActive(flag);
		ribbon.gameObject.SetActive(flag);
		int num2 = ((paymentData == null) ? ((_itemRef.costCreditsRaw <= 0) ? 1 : 2) : 0);
		int num3 = ((paymentData == null) ? ((num2 == 2) ? _itemRef.costCredits : _itemRef.costGold) : 0);
		int num4 = ((paymentData == null) ? ((num2 == 2) ? _itemRef.costCreditsRaw : _itemRef.costGoldRaw) : 0);
		num3 *= num;
		num4 *= num;
		redLine.gameObject.SetActive(flag);
		goldIcon.gameObject.SetActive(num2 == 1);
		goldRawIcon.gameObject.SetActive(flag && num2 == 1);
		creditsIcon.gameObject.SetActive(num2 == 2);
		creditsRawIcon.gameObject.SetActive(flag && num2 == 2);
		if (paymentData != null)
		{
			text = paymentData.price;
		}
		else
		{
			text = Util.NumberFormat(num3);
			if (flag)
			{
				text = Util.ParseString("^" + text + "^");
			}
		}
		costTxt.text = text;
		if (flag)
		{
			percentageTxt.text = Language.GetString("ui_shop_discount", new string[1] { Math.Round((1f - itemSaleRef.mult) * 100f).ToString() }, color: true);
			costRawTxt.text = Util.NumberFormat(num4);
			percentageTxt.gameObject.SetActive(value: true);
			costRawTxt.gameObject.SetActive(value: true);
			ribbonTxt.gameObject.SetActive(value: true);
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(WaitToFixRedLine());
			}
		}
		else
		{
			percentageTxt.gameObject.SetActive(value: false);
			costRawTxt.gameObject.SetActive(value: false);
			ribbonTxt.gameObject.SetActive(value: false);
		}
		updateTooltip();
	}

	private IEnumerator WaitToFixRedLine()
	{
		yield return new WaitForEndOfFrame();
		redLine.sizeDelta = new Vector2(costRawTxt.renderedWidth + 9f, redLine.sizeDelta.y);
	}

	public override void OnAssetRefreshed()
	{
		base.OnAssetRefreshed();
		ScrollRect componentInParent = GetComponentInParent<ScrollRect>();
		if (componentInParent != null)
		{
			componentInParent.enabled = false;
			componentInParent.enabled = true;
		}
	}

	public new virtual void OnDestroy()
	{
	}
}
