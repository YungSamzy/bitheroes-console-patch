using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.admincharacterpurchaseslist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.admin;

public class AdminCharacterPurchasesWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Image loadingIcon;

	public Button refreshBtn;

	public TMP_InputField limitTxt;

	public AdminCharacterPurchasesList adminCharacterPurchasesList;

	private int DEFAULT_LIMIT = 50;

	private int _charID;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(int charID = 0)
	{
		_charID = charID;
		topperTxt.text = "Purchases";
		limitTxt.characterLimit = 3;
		limitTxt.contentType = TMP_InputField.ContentType.IntegerNumber;
		limitTxt.text = DEFAULT_LIMIT.ToString();
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_refresh");
		limitTxt.onSubmit.AddListener(OnRefreshBtn);
		adminCharacterPurchasesList.InitList(OnTileSelect);
		DoCharacterPurchases();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnRefreshBtn(string args)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoCharacterPurchases();
	}

	private void DoCharacterPurchases()
	{
		adminCharacterPurchasesList.ClearList();
		Util.SetButton(refreshBtn, enabled: false);
		loadingIcon.gameObject.SetActive(value: true);
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(18), OnCharacterPurchases);
		AdminDALC.instance.doCharacterPurchases(_charID, GetLimit());
	}

	private void OnCharacterPurchases(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Util.SetButton(refreshBtn);
		loadingIcon.gameObject.SetActive(value: false);
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(18), OnCharacterPurchases);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<CharacterPurchaseData> purchases = CharacterPurchaseData.listFromSFSObject(sfsob);
		CreateTiles(purchases);
	}

	private void CreateTiles(List<CharacterPurchaseData> purchases)
	{
		adminCharacterPurchasesList.ClearList();
		for (int i = 0; i < purchases.Count; i++)
		{
			CharacterPurchaseData purchaseData = purchases[i];
			adminCharacterPurchasesList.Data.InsertOneAtEnd(new AdminPurchaseItem
			{
				purchaseData = purchaseData
			});
		}
	}

	private void OnTileSelect(CharacterPurchaseData purchaseData)
	{
		if (purchaseData.itemsAdded.Count <= 0)
		{
			GameData.instance.windowGenerator.ShowError("No items were received from this purchase");
		}
		else
		{
			GameData.instance.windowGenerator.ShowItems(purchaseData.itemsAdded, compare: false, added: false, "Items Received");
		}
	}

	private int GetLimit()
	{
		int num = int.Parse(limitTxt.text);
		if (num < 0)
		{
			num = 0;
		}
		if (num > int.MaxValue)
		{
			num = int.MaxValue;
		}
		return num;
	}

	public override void DoDestroy()
	{
		limitTxt.onSubmit.RemoveListener(OnRefreshBtn);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		refreshBtn.interactable = true;
		limitTxt.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		refreshBtn.interactable = false;
		limitTxt.interactable = false;
	}
}
