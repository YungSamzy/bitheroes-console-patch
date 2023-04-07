using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.ad;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.ad;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.probability;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.ad;

public class AdWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public Button rewardBtn;

	public Button skipBtn;

	public Button surpriseBtn;

	public Button probabilityBtn;

	private bool _watching;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ad_instance_name");
		descTxt.text = Language.GetString("ad_instance_desc");
		rewardBtn.GetComponentInChildren<TextMeshProUGUI>().text = (GameData.instance.PROJECT.character.toCharacterData().nftIsAdFree ? Language.GetString("ui_get") : Language.GetString("ui_watch"));
		skipBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_skip");
		probabilityBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		skipBtn.gameObject.SetActive(value: false);
		_watching = false;
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnProbabilityClick()
	{
		AdItemRef adItemRef = AdBook.lookupTier(GameData.instance.PROJECT.character.tier);
		if (adItemRef == null)
		{
			return;
		}
		int offset = 60;
		bool outline = true;
		string text = "#FFFFFF";
		List<CharacterInfoData> list = new List<CharacterInfoData>();
		CharacterInfoData characterInfoData = new CharacterInfoData(Language.GetString("ui_odds"), outline, offset, text);
		foreach (ProbabilityLine item in adItemRef.items)
		{
			characterInfoData.addValue(Language.GetString(item.link), Util.NumberFormat(item.perc) + "%", text);
		}
		list.Add(characterInfoData);
		GameData.instance.windowGenerator.NewCharacterInfoListWindow(list, Language.GetString("ad_instance_name"));
	}

	private void OnAdComplete(AdEvent adEvent)
	{
		AppInfo.adEventDispatcher.RemoveListener(OnAdComplete);
		_watching = false;
		if (adEvent.success)
		{
			DoAdReward(Mathf.FloorToInt(adEvent.duration));
		}
	}

	private void DoAdReward(int duration)
	{
		GameData.instance.main.AddBreadcrumb($"DoAdReward - {duration}");
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(15), OnAdReward);
		CharacterDALC.instance.doAdReward(duration);
	}

	private void OnAdReward(BaseEvent evt)
	{
		GameData.instance.main.HideLoading();
		DALCEvent obj = evt as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(15), OnAdReward);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			D.LogError("all", string.Format("ADFlow AdWindow:OnAdReward - {0}", sfsob.GetInt("err0")));
		}
		else
		{
			List<ItemData> list = ItemData.listFromSFSObject(sfsob);
			string currentEconomyContextLocation = KongregateAnalytics.GetCurrentEconomyContextLocation();
			GameData.instance.PROJECT.character.addItems(list);
			KongregateAnalytics.checkEconomyTransaction("Incentivized Ad", null, list, sfsob, currentEconomyContextLocation, 2);
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true);
			OnClose();
			D.LogError("all", "ADFlow AdWindow:OnAdReward - Reward Complete");
			KongregateAnalytics.trackAdEnd(list, currentEconomyContextLocation, "Rewarded Video");
		}
		if (sfsob.ContainsKey("cha65"))
		{
			GameData.instance.PROJECT.character.adMilliseconds = sfsob.GetLong("cha65");
		}
		if (sfsob.ContainsKey("cha66"))
		{
			GameData.instance.PROJECT.character.adItem = ItemData.fromSFSObject(sfsob.GetSFSObject("cha66"));
		}
		GameData.instance.PROJECT.character.checkTimerChanges(sfsob);
	}

	private void DoAdSkipConfirm()
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_skip"), Language.GetString("ui_skip_desc"), null, null, delegate
		{
			OnAdSkipConfirm();
		});
	}

	private void OnAdSkipConfirm()
	{
		DoAdSkip();
	}

	private void DoAdSkip()
	{
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(16), OnAdSkip);
		CharacterDALC.instance.doAdSkip();
	}

	private void OnAdSkip(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(16), OnAdSkip);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			OnClose();
		}
		if (sfsob.ContainsKey("cha65"))
		{
			GameData.instance.PROJECT.character.adMilliseconds = sfsob.GetLong("cha65");
		}
		if (sfsob.ContainsKey("cha66"))
		{
			GameData.instance.PROJECT.character.adItem = ItemData.fromSFSObject(sfsob.GetSFSObject("cha66"));
		}
	}

	public void OnRewardBtn()
	{
		if (!_watching)
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			if (GameData.instance.PROJECT.character.toCharacterData().nftIsAdFree)
			{
				DoAdReward(0);
				return;
			}
			if (!AppInfo.adsAvailable)
			{
				GameData.instance.windowGenerator.ShowDialogMessage(Language.GetString("ad_instance_name"), Language.GetString("no_ads_available_desc"));
				return;
			}
			AppInfo.adEventDispatcher.AddListener(OnAdComplete);
			AppInfo.ShowAd("FreeStuff");
			_watching = true;
		}
	}

	public void OnSkipBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoAdSkipConfirm();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		SetButtonsState(state: true);
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		SetButtonsState(state: false);
	}

	private void SetButtonsState(bool state)
	{
		if (rewardBtn != null)
		{
			rewardBtn.interactable = state;
		}
		if (skipBtn != null)
		{
			skipBtn.interactable = state;
		}
		if (surpriseBtn != null)
		{
			surpriseBtn.interactable = state;
		}
		if (probabilityBtn != null)
		{
			probabilityBtn.interactable = state;
		}
	}
}
