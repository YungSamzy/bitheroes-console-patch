using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.events.ad;
using com.ultrabit.bitheroes.model.ad;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.probability;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.dungeon;

public class DungeonAdWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI dialogTxt;

	public Image placeholderAsset;

	public Button useBtn;

	public Button declineBtn;

	public Button helpBtn;

	private Dungeon _dungeon;

	private DungeonPlayer _player;

	private DungeonObject _object;

	private bool _watching;

	public void LoadDetails(Dungeon dungeon, DungeonPlayer player, DungeonObject theObject)
	{
		_dungeon = dungeon;
		_player = player;
		_object = theObject;
		topperTxt.text = Language.GetString("dungeon_treasure_name");
		dialogTxt.text = Language.GetString("ad_dungeon_desc");
		useBtn.GetComponentInChildren<TextMeshProUGUI>().text = (GameData.instance.PROJECT.character.toCharacterData().nftIsAdFree ? Language.GetString("ui_get") : Language.GetString("ui_watch"));
		declineBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_decline");
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		placeholderAsset.overrideSprite = _object.objectRef.GetSpriteIcon();
		_dungeon.extension.SetResolvingAd(resolvingAd: true);
		GameData.instance.PROJECT.PauseDungeon();
		ListenForBack(DoDecline);
		CreateWindow();
	}

	public void OnProbabilityClick()
	{
		AdItemRef adItemRef = AdBook.lookupChestTier(GameData.instance.PROJECT.character.tier);
		if (adItemRef == null)
		{
			return;
		}
		int offset = 60;
		bool outline = true;
		string text = "#FFFFFF";
		List<CharacterInfoData> list = new List<CharacterInfoData>();
		CharacterInfoData characterInfoData = new CharacterInfoData(Language.GetString("ui_dungeon_ad_title"), outline, offset, text);
		foreach (ProbabilityLine item in adItemRef.items)
		{
			characterInfoData.addValue(Language.GetString(item.link), Util.NumberFormat(item.perc) + "%", text);
		}
		list.Add(characterInfoData);
		GameData.instance.windowGenerator.NewCharacterInfoListWindow(list, Language.GetString("ui_dungeon_ad"));
	}

	public void OnAssetLoaded()
	{
	}

	public void OnUseBtn()
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
				GameData.instance.windowGenerator.ShowDialogMessage(Language.GetString("dungeon_treasure_name"), Language.GetString("no_ads_available_desc"));
				return;
			}
			_watching = true;
			AppInfo.adEventDispatcher.AddListener(OnAdCompleted);
			AppInfo.ShowAd("Dungeon");
		}
	}

	public override void OnClose()
	{
		_dungeon.extension.SetResolvingAd(resolvingAd: false);
		GameData.instance.PROJECT.ResumeDungeon();
		base.OnClose();
	}

	private void OnAdCompleted(AdEvent response)
	{
		AppInfo.adEventDispatcher.RemoveListener(OnAdCompleted);
		_watching = false;
		if (response.success)
		{
			DoAdReward((int)response.duration);
		}
		_dungeon.extension.SetResolvingAd(resolvingAd: false);
		GameData.instance.PROJECT.ResumeDungeon();
	}

	private void DoAdReward(int duration)
	{
		_dungeon.ActivateObject(_player, _object, wait: true, -1, duration);
		KongregateAnalytics.trackAdEnd(null, "Dungeon", "Rewarded Video");
	}

	public void OnDeclineBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoDecline();
	}

	private void DoDecline()
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_decline_confirm"), null, null, delegate
		{
			OnDeclineConfirm();
		});
	}

	private void OnDeclineConfirm()
	{
		_dungeon.ActivateObject(_player, _object, wait: true, 0, 0);
		_dungeon.extension.SetResolvingAd(resolvingAd: false);
		GameData.instance.PROJECT.ResumeDungeon();
		base.OnClose();
	}

	public override void DoDestroy()
	{
		if (!GameData.instance.windowGenerator.HasDialogByClass(typeof(ItemListWindow)))
		{
			_dungeon.CheckAutoPilot();
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		useBtn.interactable = true;
		declineBtn.interactable = true;
		helpBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		useBtn.interactable = false;
		declineBtn.interactable = false;
		helpBtn.interactable = false;
	}
}
