using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.filter;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.service;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.transaction;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildCreateWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI guildNameTitleTxt;

	public TextMeshProUGUI guildInitialsTitleTxt;

	public TextMeshProUGUI costTxt;

	public Button createBtn;

	public TMP_InputField guildNameTxt;

	public TMP_InputField guildInitialsTxt;

	public Image goldIcon;

	public Image creditsIcon;

	private ServiceRef _serviceRef;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_guild");
		guildNameTitleTxt.text = Language.GetString("ui_name");
		guildInitialsTitleTxt.text = Language.GetString("ui_initials");
		_serviceRef = ServiceBook.GetFirstServiceByType(7);
		guildNameTxt.text = "";
		guildNameTxt.characterLimit = VariableBook.guildNameLength;
		guildInitialsTxt.text = "";
		guildInitialsTxt.characterLimit = VariableBook.guildInitialsLength;
		Debug.LogWarning("Check InputText Submit on mobile");
		guildNameTxt.onSubmit.AddListener(delegate
		{
			DoGuildCreate();
		});
		guildInitialsTxt.onSubmit.AddListener(delegate
		{
			DoGuildCreate();
		});
		createBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_create");
		GameData.instance.PROJECT.character.AddListener("SHOP_ROTATION_ID_CHANGE", OnShopRotationChange);
		UpdateCost();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnShopRotationChange()
	{
		UpdateCost();
	}

	private void UpdateCost()
	{
		ShopSaleRef itemSaleRef = ShopBook.GetItemSaleRef(_serviceRef, GameData.instance.PROJECT.character.shopRotationID);
		int num = ((_serviceRef.costCreditsRaw <= 0) ? 1 : 2);
		int num2 = ((num == 2) ? _serviceRef.costCredits : _serviceRef.costGold);
		goldIcon.gameObject.SetActive(num == 1);
		creditsIcon.gameObject.SetActive(num == 2);
		string text = Util.NumberFormat(num2);
		if (itemSaleRef != null)
		{
			text = Util.ParseString("^" + text + "^");
		}
		costTxt.text = text;
	}

	public void OnValueChangedName()
	{
		guildNameTxt.text = Util.FilterUnicodeCharacters(guildNameTxt.text, "-_'.,;:<>[]()|&`\u00b4*¿?¡!\u00a8/@<>#*+$%~{}=€^\"\\");
	}

	public void OnValueChangedInitials()
	{
		guildInitialsTxt.text = Util.FilterUnicodeCharacters(guildInitialsTxt.text, null, Util.FORBIDDEN_GUILDINITIALS_CHAR_CATEGORIES);
		for (int i = 0; i < guildInitialsTxt.text.Length; i++)
		{
			if (!Util.GuildInitialsAllowed(guildInitialsTxt.text[i]))
			{
				guildInitialsTxt.text = guildInitialsTxt.text.Remove(i, 1);
				guildInitialsTxt.caretPosition = i;
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Tab))
		{
			if (guildInitialsTxt.isFocused)
			{
				guildNameTxt.ActivateInputField();
				guildNameTxt.selectionAnchorPosition = 0;
				guildNameTxt.selectionFocusPosition = guildNameTxt.text.Length;
			}
			if (guildNameTxt.isFocused)
			{
				guildInitialsTxt.ActivateInputField();
				guildInitialsTxt.selectionAnchorPosition = 0;
				guildInitialsTxt.selectionFocusPosition = guildInitialsTxt.text.Length;
			}
		}
	}

	public void OnCreateBtn(string args = null)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoGuildCreate();
	}

	private void DoGuildCreate()
	{
		string text = Util.removeExtraWhiteSpace(guildNameTxt.text);
		string text2 = Util.trimSpacing(guildInitialsTxt.text);
		guildNameTxt.text = text;
		guildInitialsTxt.text = text2;
		if (text == "" || text.Length <= 0)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_name"));
			return;
		}
		if (text2 == "" || text2.Length <= 0)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_initials"));
			return;
		}
		if (!Filter.allow(text))
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_invalid_name"));
			return;
		}
		if (!Filter.allow(text2))
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_invalid_initials"));
			return;
		}
		Dictionary<string, string> dictionaryData = new Dictionary<string, string>
		{
			["name"] = text,
			["initials"] = text2
		};
		int num = ((_serviceRef.costCreditsRaw <= 0) ? 1 : 2);
		CurrencyRef currencyRef = CurrencyBook.Lookup(num);
		int cost = _serviceRef.getCost(num);
		TransactionManager.PurchaseObject data = new TransactionManager.PurchaseObject();
		data.currencyID = num;
		data.currencyCost = cost;
		data.context = "Guild";
		string @string = Language.GetString("purchase_guild_confirm", new string[3]
		{
			text,
			Util.NumberFormat(cost),
			currencyRef.name
		});
		TransactionManager.instance.ConfirmItemPurchase(_serviceRef, "Guild", 1, @string, delegate
		{
			OnGuildCreateConfirm(dictionaryData, data);
		}, data);
	}

	private void OnGuildCreateConfirm(Dictionary<string, string> dictionaryData, TransactionManager.PurchaseObject data)
	{
		Disable();
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.doCreate(dictionaryData["name"], dictionaryData["initials"], data.currencyID, data.currencyCost);
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnGuildCreate);
	}

	private void OnGuildCreate(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnGuildCreate);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			CharacterGuildData guildData = CharacterGuildData.fromSFSObject(sfsob);
			GameData.instance.PROJECT.character.guildData = guildData;
			GameData.instance.PROJECT.character.updateAchievements();
			KongregateAnalytics.TrackCPEEvent("kong_join_guild");
			GameData.instance.audioManager.PlaySoundLink("purchase");
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_success"), Language.GetString("ui_guild_create_success"));
		}
		KongregateAnalytics.checkEconomyTransaction("Guild Create", null, null, sfsob, "Guild", 1, currencyUpdate: false);
		GameData.instance.PROJECT.character.checkCurrencyChanges(sfsob, update: true);
	}

	public override void DoDestroy()
	{
		guildNameTxt.onSubmit.RemoveListener(delegate
		{
			DoGuildCreate();
		});
		guildInitialsTxt.onSubmit.RemoveListener(delegate
		{
			DoGuildCreate();
		});
		GameData.instance.PROJECT.character.RemoveListener("SHOP_ROTATION_ID_CHANGE", OnShopRotationChange);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		createBtn.interactable = true;
		guildNameTxt.interactable = true;
		guildInitialsTxt.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		createBtn.interactable = false;
		guildNameTxt.interactable = false;
		guildInitialsTxt.interactable = false;
	}
}
