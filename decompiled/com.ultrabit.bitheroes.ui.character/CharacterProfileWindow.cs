using System;
using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.friend;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterProfileWindow : WindowsMain
{
	private const string BLANK = "-";

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI guildLabelTxt;

	public TextMeshProUGUI guildInitialsTxt;

	public TextMeshProUGUI guildNameTxt;

	public TextMeshProUGUI loginTxt;

	public Button heroTagBtn;

	public Button historyNamesBtn;

	public Button moderatorBtn;

	public Button armoryBtn;

	public Button guildApplyBtn;

	public Button guildInviteBtn;

	public Button friendAddBtn;

	public Button friendRemoveBtn;

	public Button friendAcceptBtn;

	public Button ignoreBtn;

	public Button unignoreBtn;

	public Button chatBtn;

	public Button abilitiesBtn;

	public Button duelBtn;

	public Button runesBtn;

	public Button enchantsBtn;

	public Image onlineIcon;

	public Image offlineIcon;

	private CharacterData _characterData;

	private bool _online;

	public CharacterEquipmentPanel _equipmentPanel;

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	private bool _Iam;

	private float _loadPerc;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(CharacterData characterData, bool online = false)
	{
		_characterData = characterData;
		_online = online;
		topperTxt.text = characterData.name;
		guildLabelTxt.text = Language.GetString("ui_guild");
		guildInitialsTxt.text = Util.ParseGuildInitials((characterData.guildInfo != null) ? characterData.guildInfo.initials : "-");
		guildNameTxt.text = ((characterData.guildInfo != null) ? characterData.guildInfo.name : "-");
		loginTxt.text = _characterData.getLoginText(online);
		heroTagBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_herotag_btn");
		friendAddBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_friend_add");
		friendRemoveBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_friend_remove");
		friendAcceptBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_friend_accept");
		guildApplyBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_guild_apply");
		guildInviteBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_guild_invite");
		ignoreBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ignore");
		unignoreBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_unignore");
		chatBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_message");
		duelBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_duel");
		abilitiesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ability_plural_name");
		armoryBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_armory_title");
		runesBtn.GetComponentInChildren<TextMeshProUGUI>().text = ItemRef.GetItemNamePlural(9);
		enchantsBtn.GetComponentInChildren<TextMeshProUGUI>().text = ItemRef.GetItemNamePlural(11);
		historyNamesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_historyname");
		historyNamesBtn.gameObject.SetActive(characterData.nameHasChanged);
		if (GameData.instance.PROJECT.character.moderator)
		{
			moderatorBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_m");
		}
		else
		{
			moderatorBtn.gameObject.SetActive(value: false);
		}
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(24), OnGetOnline);
		GameData.instance.PROJECT.character.AddListener("FRIEND_CHANGE", OnChange);
		GameData.instance.PROJECT.character.AddListener("REQUEST_CHANGE", OnChange);
		GameData.instance.PROJECT.character.AddListener("GUILD_CHANGE", OnChange);
		GameData.instance.PROJECT.character.AddListener("GUILD_RANK_CHANGE", OnChange);
		GameData.instance.PROJECT.character.AddListener("GUILD_PERMISSIONS_CHANGE", OnChange);
		GameData.instance.PROJECT.character.AddListener("CHAT_CHANGE", OnChange);
		_equipmentPanel.LoadDetails(_characterData);
		_Iam = _characterData.charID == GameData.instance.PROJECT.character.id;
		UpdateButtons();
		if (!_online)
		{
			DoGetOnline();
		}
		ListenForBack(OnClose);
		CreateWindow();
	}

	public CharacterData GetCharacterData()
	{
		return _characterData;
	}

	public void OverrideCharacterStats(int power, int stamina, int agility)
	{
		_equipmentPanel.SetStats(power, stamina, agility);
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		_equipmentPanel.UpdateLayer();
	}

	public void OnFriendAddBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameRequirement gameRequirement = VariableBook.GetGameRequirement(5);
		if (gameRequirement == null || gameRequirement.RequirementsMet())
		{
			GameData.instance.PROJECT.DoSendRequestByID(_characterData.charID);
			return;
		}
		string requirementsText = gameRequirement.GetRequirementsText();
		if (requirementsText != null)
		{
			DialogRef dialogLocked = gameRequirement.GetDialogLocked();
			if (dialogLocked != null)
			{
				GameData.instance.windowGenerator.NewDialogPopup(dialogLocked);
			}
			else
			{
				GameData.instance.windowGenerator.ShowError(requirementsText);
			}
		}
	}

	public void OnFriendRemoveBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoFriendRemoveConfirm(_characterData.charID);
	}

	public void OnFriendAcceptBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoAcceptRequest(_characterData.charID);
	}

	public void OnGuildApplyBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoGuildApply(_characterData.guildInfo.id);
	}

	public void OnGuildInviteBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoGuildInviteByID(_characterData.charID);
	}

	public void OnIgnoreBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoIgnore();
	}

	public void OnUnignoreBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoUnignore();
	}

	public void OnChatBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowConversation(_characterData.charID, _characterData.name);
	}

	public void OnAbilitiesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewAbilityListWindow(_characterData.getAbilities(), _characterData.getTotalPower(), GameModifier.getTypeTotal(_characterData.getModifiers(), 17), base.layer);
	}

	public void OnDuelBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoDuelSend(_characterData.charID, _online);
	}

	public void OnRunesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewRunesWindow(_characterData.runes, changeable: false, base.gameObject, base.layer);
	}

	public void OnEnchantsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEnchantsWindow(_characterData.enchants, changeable: false, base.gameObject, base.layer);
	}

	public void OnModeratorBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewChatPlayerWindow(_characterData.charID, _characterData.name, "", base.gameObject, base.layer);
	}

	public void OnHeroTagBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewCharacterHerotagWindow(_characterData.name, _characterData.herotag, changeable: false, base.gameObject, base.layer);
	}

	public void OnHistoryNamesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowHistoryNames(_characterData.charID);
	}

	public void OnArmoryBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.GetFirstDialog().OnClose();
		GameData.instance.windowGenerator.GetLastDialog().OnClose();
		if (!GameData.instance.PROJECT.IsArmoryLoaded(_characterData))
		{
			StartCoroutine(CleanScene("player_armory"));
		}
	}

	private IEnumerator CleanScene(string name)
	{
		D.Log("___________CleanScene____________: " + name);
		GameObject[] rootObjects = SceneManager.GetSceneByName(name).GetRootGameObjects();
		if (rootObjects != null)
		{
			int count = rootObjects.Length;
			int destroyCount = 0;
			_loadPerc = 0f;
			for (int i = 0; i < count; i++)
			{
				UnityEngine.Object.Destroy(rootObjects[i]);
				destroyCount++;
				_loadPerc = Mathf.Round((float)destroyCount * 100f / (float)count);
				yield return new WaitForEndOfFrame();
			}
		}
		yield return new WaitForEndOfFrame();
		Resources.UnloadUnusedAssets();
		GC.Collect();
		yield return new WaitForEndOfFrame();
		GameData.instance.PROJECT.DoEnterInstance(InstanceBook.GetFirstInstanceByType(5), transition: false, _characterData);
	}

	public void DoGetOnline()
	{
		_online = false;
		UpdateButtons();
		CharacterDALC.instance.doGetOnline(_characterData.charID);
	}

	private void OnGetOnline(BaseEvent e)
	{
		int @int = (e as DALCEvent).sfsob.GetInt("cha1");
		if (_characterData.charID == @int)
		{
			_online = true;
			UpdateButtons();
		}
	}

	private void OnChange()
	{
		UpdateButtons();
	}

	private void UpdateButtons()
	{
		FriendData friendData = GameData.instance.PROJECT.character.getFriendData(_characterData.charID, -1, duplicateData: false);
		RequestData requestData = GameData.instance.PROJECT.character.getRequestData(_characterData.charID);
		GuildMemberData guildMemberData = ((GameData.instance.PROJECT.character.guildData != null) ? GameData.instance.PROJECT.character.guildData.getMember(_characterData.charID, -1, duplicateData: false) : null);
		Util.SetButton(armoryBtn, enabled: false);
		if (friendData != null)
		{
			_online = friendData.online;
			_characterData.loginMilliseconds = (long)friendData.loginMilliseconds;
			if (GameData.instance.PROJECT.dungeon == null && _characterData.level >= GameData.instance.PROJECT.armoryMinLevelRequired && _characterData.armory.armoryEquipmentSlots.Count > 0)
			{
				Util.SetButton(armoryBtn);
			}
		}
		else if (guildMemberData != null)
		{
			_online = guildMemberData.online;
			_characterData.loginMilliseconds = (long)guildMemberData.loginMilliseconds;
		}
		if (requestData != null)
		{
			friendAddBtn.gameObject.SetActive(value: false);
			friendRemoveBtn.gameObject.SetActive(value: false);
			friendAcceptBtn.gameObject.SetActive(value: true);
		}
		else if (friendData != null)
		{
			friendAddBtn.gameObject.SetActive(value: false);
			friendRemoveBtn.gameObject.SetActive(value: true);
			friendAcceptBtn.gameObject.SetActive(value: false);
		}
		else
		{
			friendAddBtn.gameObject.SetActive(value: true);
			friendRemoveBtn.gameObject.SetActive(value: false);
			friendAcceptBtn.gameObject.SetActive(value: false);
		}
		if (GameData.instance.PROJECT.character.guildData != null)
		{
			guildInviteBtn.gameObject.SetActive(value: true);
			guildApplyBtn.gameObject.SetActive(value: false);
			if (_characterData.guildInfo == null && GameData.instance.PROJECT.character.guildData.hasPermission(1) && _characterData.charID != GameData.instance.PROJECT.character.id)
			{
				Util.SetButton(guildInviteBtn);
			}
			else
			{
				Util.SetButton(guildInviteBtn, enabled: false);
			}
		}
		else
		{
			guildInviteBtn.gameObject.SetActive(value: false);
			guildApplyBtn.gameObject.SetActive(value: true);
			if (_characterData.guildInfo != null && _characterData.charID != GameData.instance.PROJECT.character.id && VariableBook.GameRequirementMet(8))
			{
				Util.SetButton(guildApplyBtn);
			}
			else
			{
				Util.SetButton(guildApplyBtn, enabled: false);
			}
		}
		onlineIcon.gameObject.SetActive(_online);
		offlineIcon.gameObject.SetActive(!onlineIcon.gameObject.activeSelf);
		ignoreBtn.gameObject.SetActive(GameData.instance.PROJECT.character.getChatIgnore(_characterData.charID) == null);
		unignoreBtn.gameObject.SetActive(!ignoreBtn.gameObject.activeSelf);
		loginTxt.text = _characterData.getLoginText(_online);
		if (_online && !_Iam && (GameData.instance.PROJECT.character.admin || friendData != null || guildMemberData != null) && VariableBook.GameRequirementMet(4))
		{
			Util.SetButton(chatBtn);
		}
		else
		{
			Util.SetButton(chatBtn, enabled: false);
		}
		if (_online)
		{
			Util.SetButton(duelBtn);
		}
		else
		{
			Util.SetButton(duelBtn, enabled: false);
		}
		if (GameData.instance.PROJECT.battle != null)
		{
			Util.SetButton(armoryBtn, enabled: false);
		}
	}

	private void DoIgnore()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		ChatDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnIgnore);
		ChatDALC.instance.doIgnore(_characterData.charID);
	}

	private void OnIgnore(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		ChatDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnIgnore);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		ChatPlayerData chatData = ChatPlayerData.fromSFSObject(sfsob);
		GameData.instance.PROJECT.character.addChatIgnore(chatData);
	}

	private void DoUnignore()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		ChatDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(4), OnUnignore);
		ChatDALC.instance.doUnignore(_characterData.charID);
	}

	private void OnUnignore(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		ChatDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(4), OnUnignore);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		int @int = sfsob.GetInt("cha1");
		GameData.instance.PROJECT.character.removeChatIgnore(@int);
	}

	public override void DoDestroy()
	{
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(24), OnGetOnline);
		GameData.instance.PROJECT.character.RemoveListener("FRIEND_CHANGE", OnChange);
		GameData.instance.PROJECT.character.RemoveListener("REQUEST_CHANGE", OnChange);
		GameData.instance.PROJECT.character.RemoveListener("GUILD_CHANGE", OnChange);
		GameData.instance.PROJECT.character.RemoveListener("GUILD_RANK_CHANGE", OnChange);
		GameData.instance.PROJECT.character.RemoveListener("GUILD_PERMISSIONS_CHANGE", OnChange);
		GameData.instance.PROJECT.character.RemoveListener("CHAT_CHANGE", OnChange);
		base.DoDestroy();
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
		if (moderatorBtn != null && moderatorBtn.gameObject != null)
		{
			moderatorBtn.interactable = state;
		}
		if (guildApplyBtn != null && guildApplyBtn.gameObject != null)
		{
			guildApplyBtn.interactable = state;
		}
		if (guildInviteBtn != null && guildInviteBtn.gameObject != null)
		{
			guildInviteBtn.interactable = state;
		}
		if (friendAddBtn != null && friendAddBtn.gameObject != null)
		{
			friendAddBtn.interactable = state;
		}
		if (friendRemoveBtn != null && friendRemoveBtn.gameObject != null)
		{
			friendRemoveBtn.interactable = state;
		}
		if (friendAcceptBtn != null && friendAcceptBtn.gameObject != null)
		{
			friendAcceptBtn.interactable = state;
		}
		if (ignoreBtn != null && ignoreBtn.gameObject != null)
		{
			ignoreBtn.interactable = state;
		}
		if (unignoreBtn != null && unignoreBtn.gameObject != null)
		{
			unignoreBtn.interactable = state;
		}
		if (chatBtn != null && chatBtn.gameObject != null)
		{
			chatBtn.interactable = state;
		}
		if (abilitiesBtn != null && abilitiesBtn.gameObject != null)
		{
			abilitiesBtn.interactable = state;
		}
		if (duelBtn != null && duelBtn.gameObject != null)
		{
			duelBtn.interactable = state;
		}
		if (runesBtn != null && runesBtn.gameObject != null)
		{
			runesBtn.interactable = state;
		}
		if (enchantsBtn != null && enchantsBtn.gameObject != null)
		{
			enchantsBtn.interactable = state;
		}
		if (heroTagBtn != null && heroTagBtn.gameObject != null)
		{
			heroTagBtn.interactable = state;
		}
		if (historyNamesBtn != null && historyNamesBtn.gameObject != null)
		{
			historyNamesBtn.interactable = state;
		}
		if (armoryBtn != null && armoryBtn.gameObject != null)
		{
			armoryBtn.interactable = state;
		}
	}
}
