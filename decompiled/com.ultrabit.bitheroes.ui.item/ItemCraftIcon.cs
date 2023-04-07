using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.fusion;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.item.action;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemCraftIcon : MonoBehaviour
{
	private ItemData _item;

	private ItemData _cosmetic;

	private Transform tooltip;

	private ItemActionBase _itemIconAction;

	private ItemIconComparision itemIconComparision;

	private ItemRef _itemRef;

	private Image background;

	private GameObject result;

	private Button btnCraft;

	private TextMeshProUGUI txtBtnCraft;

	private Transform ingredientsCont;

	private Transform quantityCont;

	public void SetItemData(ItemRef itemRef)
	{
		if (background == null)
		{
			background = GetComponent<Image>();
		}
		if (result == null)
		{
			result = base.transform.GetChild(0).gameObject;
		}
		if (btnCraft == null)
		{
			btnCraft = base.transform.GetChild(1).GetComponent<Button>();
		}
		if (txtBtnCraft == null)
		{
			txtBtnCraft = base.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
		}
		if (ingredientsCont == null)
		{
			ingredientsCont = base.transform.GetChild(2);
		}
		if (quantityCont == null)
		{
			quantityCont = base.transform.GetChild(3);
		}
		_itemRef = itemRef;
		if (_itemRef.itemType != 7)
		{
			return;
		}
		txtBtnCraft.text = Language.GetString("ui_fuse");
		btnCraft.onClick.AddListener(delegate
		{
			FusionDialog(_itemRef);
		});
		background = base.transform.GetComponentInChildren<Image>();
		Color color = Color.white;
		ColorUtility.TryParseHtmlString("#" + _itemRef.rarityRef.objectColor, out color);
		color.a = ((_itemRef as FusionRef).tradeRef.requirementsMet() ? 0.5f : 1f);
		background.color = color;
		MonoBehaviour.print((_itemRef as FusionRef).tradeRef.requiredItems.Count);
		Util.SetButton(btnCraft, (_itemRef as FusionRef).tradeRef.requirementsMet());
		ItemIcon itemIcon = result.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = result.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData((_itemRef as FusionRef).tradeRef.resultItem);
		for (int i = 0; i < (_itemRef as FusionRef).tradeRef.requiredItems.Count; i++)
		{
			ingredientsCont.GetChild(i).gameObject.SetActive(value: true);
			quantityCont.GetChild(i).gameObject.SetActive(value: true);
			quantityCont.GetChild(i).GetComponent<TextMeshProUGUI>().text = (_itemRef as FusionRef).tradeRef.requiredItems[i].qty.ToString();
			ItemIcon itemIcon2 = ingredientsCont.GetChild(i).gameObject.GetComponent<ItemIcon>();
			if (itemIcon2 == null)
			{
				itemIcon2 = ingredientsCont.GetChild(i).gameObject.AddComponent<ItemIcon>();
			}
			itemIcon2.SetItemData((_itemRef as FusionRef).tradeRef.requiredItems[i]);
		}
	}

	private void FusionDialog(ItemRef _item)
	{
		if (_item == _itemRef)
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_fuse_confirm", new string[1] { (_itemRef as FusionRef).tradeRef.requiredItems[0].itemRef.coloredName + ", " + (_itemRef as FusionRef).tradeRef.requiredItems[1].itemRef.coloredName }), Language.GetString("ui_confirm"), Language.GetString("ui_cancel"), OnCraftConfirm);
		}
	}

	private void OnCraftConfirm()
	{
		DoCraft();
	}

	private void DoCraft()
	{
		if (_itemRef is FusionRef)
		{
			FusionRef fusionRef = _itemRef as FusionRef;
			DoItemFusion(fusionRef);
		}
	}

	private void DoItemFusion(FusionRef fusionRef)
	{
		GameData.instance.main.ShowLoading();
		MerchantDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_FUSION), onFusion);
		MerchantDALC.instance.doItemFusion(fusionRef.id);
	}

	private void onFusion(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		MerchantDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_FUSION), onFusion);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		ItemData itemData = ItemData.fromSFSObject(sfsob);
		MonoBehaviour.print(itemData.id);
		GameData.instance.PROJECT.character.addItem(itemData);
		GameData.instance.PROJECT.character.removeItems((_itemRef as FusionRef).tradeRef.requiredItems);
		List<ItemData> list = new List<ItemData>();
		list.Add(itemData);
		KongregateAnalytics.checkEconomyTransaction("Fusion", (_itemRef as FusionRef).tradeRef.requiredItems, list, sfsob, "Craft");
		_ = _itemRef;
	}
}
