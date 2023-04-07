using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.vipgorlist;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.vipgor;

public class VipGorWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public Button helpBtn;

	public VipGorList vipGorList;

	private ItemData _itemCheck;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(ShopSaleRef highlightedRef = null)
	{
		topperTxt.text = Language.GetString("ui_vipgor").ToUpperInvariant();
		descTxt.text = Language.GetString("ui_vipgor_desc");
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		GameData.instance.PROJECT.PauseDungeon();
		vipGorList.InitList();
		CreateTiles(highlightedRef);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void CreateTiles(ShopSaleRef highlightedRef = null)
	{
		List<ItemRef> list = new List<ItemRef>();
		for (int i = 0; i < ShopBook.vipgorSize; i++)
		{
			ShopSaleRef shopSaleRef = ShopBook.LookupVipgor(i);
			if (shopSaleRef.getActive() || AppInfo.TESTING)
			{
				list.Add(shopSaleRef.itemRef);
			}
		}
		foreach (ItemRef item in list)
		{
			vipGorList.Data.InsertOneAtEnd(new VipgorItem
			{
				itemRef = item
			});
		}
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(Language.GetString("vipgor_help_title"), Util.parseMultiLine(Language.GetString("vipgor_help_desc")));
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.ResumeDungeon();
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		helpBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		helpBtn.interactable = false;
	}
}
