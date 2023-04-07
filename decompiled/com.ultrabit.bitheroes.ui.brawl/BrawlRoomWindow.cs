using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.brawlteamlist;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.brawl;

public class BrawlRoomWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI statsTxt;

	public TextMeshProUGUI tierTxt;

	public TextMeshProUGUI difficultyTxt;

	public TextMeshProUGUI sealsTxt;

	public Button statsBG;

	public Button energyBtn;

	public Button startBtn;

	public Button readyBtn;

	public Button unreadyBtn;

	public Toggle privateCheckBox;

	public BrawlTeamList brawlTeamList;

	public SpriteMask topMask;

	public SpriteMask bottomMask;

	public CurrencyBarFill currencyBarFill;

	private BrawlRoom _room;

	public BrawlRoomChatBox _chatBox;

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	public BrawlRoom room => _room;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(BrawlRoom room)
	{
		_room = room;
		topperTxt.text = _room.difficultyRef.brawlRef.name;
		tierTxt.text = Language.GetString("ui_tier_count", new string[1] { _room.tierID.ToString() });
		difficultyTxt.text = _room.difficultyRef.difficultyRef.coloredName;
		sealsTxt.text = Util.NumberFormat(_room.difficultyRef.difficultyRef.seals);
		startBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_start");
		readyBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ready");
		unreadyBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_unready");
		privateCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_private");
		_chatBox.Create(_room);
		brawlTeamList.InitList(delegate(BrawlPlayer player)
		{
			OnKickConfirm(player);
		}, OnInvite, this);
		BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnChange);
		BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(6), OnMessage);
		BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(9), OnRemoved);
		BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(11), OnBegin);
		CreateList();
		UpdateCheckbox(isFirstUpdate: true);
		UpdateButtons();
		UpdateStats();
		currencyBarFill.Init();
		ListenForBack(DoLeaveConfirm);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		topMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		topMask.frontSortingOrder = 1 + base.sortingLayer + brawlTeamList.Content.childCount;
		topMask.backSortingLayerID = SortingLayer.NameToID("UI");
		topMask.backSortingOrder = base.sortingLayer;
		bottomMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.frontSortingOrder = 1 + base.sortingLayer + brawlTeamList.Content.childCount;
		bottomMask.backSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.backSortingOrder = base.sortingLayer;
		foreach (MyViewsHolder visibleItem in brawlTeamList._VisibleItems)
		{
			brawlTeamList.UpdateListItem(visibleItem);
		}
	}

	public void OnReadyBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoReady(ready: true);
	}

	public void OnUnreadyBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoReady(ready: false);
	}

	public void OnStartBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoStartConfirm();
	}

	public void OnStatsBG()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_stats"), Language.GetString("ui_total_stats"));
	}

	public void OnEnergyBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowServiceType(15);
	}

	public void OnXealsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowServiceType(15);
	}

	public void OnPublicCheckBox()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.SAVE_STATE.SetBrawlPublicSelected(GameData.instance.PROJECT.character.id, !privateCheckBox.isOn);
		DoRules(!privateCheckBox.isOn);
	}

	private void UpdateCheckbox(bool isFirstUpdate = false)
	{
		privateCheckBox.SetIsOnWithoutNotify(!_room.rules.getPublic());
		if (_room.leader == GameData.instance.PROJECT.character.id)
		{
			privateCheckBox.interactable = true;
			privateCheckBox.enabled = true;
			privateCheckBox.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			privateCheckBox.image.color = Color.white;
			if (isFirstUpdate)
			{
				privateCheckBox.isOn = GameData.instance.PROJECT.character.lastBrawlPrivateCheckbox ?? (!GameData.instance.SAVE_STATE.GetBrawlPublicSelected(GameData.instance.PROJECT.character.id));
			}
		}
		else
		{
			privateCheckBox.interactable = false;
			privateCheckBox.enabled = false;
			privateCheckBox.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
			privateCheckBox.image.color = alpha;
		}
	}

	private void UpdateButtons()
	{
		if (_room == null)
		{
			return;
		}
		BrawlPlayer player = _room.getPlayer(GameData.instance.PROJECT.character.id);
		if (player == null)
		{
			return;
		}
		if (_room.leader == player.characterData.charID)
		{
			startBtn.gameObject.SetActive(value: true);
			readyBtn.gameObject.SetActive(value: false);
			unreadyBtn.gameObject.SetActive(value: false);
			if (_room.getReady())
			{
				Util.SetButton(startBtn);
			}
			else
			{
				Util.SetButton(startBtn, enabled: false);
			}
		}
		else if (player.ready)
		{
			startBtn.gameObject.SetActive(value: false);
			readyBtn.gameObject.SetActive(value: false);
			unreadyBtn.gameObject.SetActive(value: true);
		}
		else
		{
			startBtn.gameObject.SetActive(value: false);
			readyBtn.gameObject.SetActive(value: true);
			unreadyBtn.gameObject.SetActive(value: false);
		}
		if (player.ready)
		{
			Util.SetButton(closeBtn, enabled: false);
		}
		else
		{
			Util.SetButton(closeBtn);
		}
	}

	private void UpdateStats()
	{
		int num = 0;
		foreach (BrawlPlayer slot in _room.slots)
		{
			if (slot != null)
			{
				num += slot.characterData.getTotalStats();
			}
		}
		statsTxt.text = Util.NumberFormat(num, abbreviate: false);
	}

	public void CreateList()
	{
		if (!(brawlTeamList == null) && brawlTeamList.isActiveAndEnabled)
		{
			brawlTeamList.ClearList();
			for (int i = 0; i < _room.slots.Count; i++)
			{
				BrawlPlayer player = _room.slots[i];
				brawlTeamList.Data.InsertOneAtEnd(new BrawlTeamListItemData
				{
					slot = i,
					player = player,
					room = _room,
					roomWindow = this
				});
			}
		}
	}

	public void OnInvite()
	{
		GameData.instance.windowGenerator.NewBrawlRoomInviteWindow(_room, base.gameObject);
	}

	private void DoReady(bool ready)
	{
		GameData.instance.main.ShowLoading();
		BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(7), OnReady);
		BrawlDALC.instance.doReady(ready);
	}

	private void OnReady(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(7), OnReady);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	private void OnChange(BaseEvent e)
	{
		if (!(this == null))
		{
			BrawlRoom brawlRoom = BrawlRoom.fromSFSObject((e as DALCEvent).sfsob);
			_room.checkChanges(brawlRoom);
			CreateList();
			UpdateButtons();
			UpdateStats();
			UpdateCheckbox();
		}
	}

	private void OnMessage(BaseEvent e)
	{
		ChatData chatData = ChatData.fromSFSObject((e as DALCEvent).sfsob);
		_chatBox.ParseData(chatData);
	}

	private void OnRemoved(BaseEvent e)
	{
		ForceClose();
		GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_brawl_removed_confirm", new string[1] { Language.GetString("ui_brawl") }));
	}

	private void OnBegin(BaseEvent e)
	{
		GameData.instance.PROJECT.character.lastBrawlPrivateCheckbox = privateCheckBox.isOn;
		SFSObject sfsob = (e as DALCEvent).sfsob;
		GameData.instance.PROJECT.character.checkTimerChanges(sfsob);
	}

	private void DoStartConfirm()
	{
		if (!_room.isFull())
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_team_not_full_confirm"), null, null, delegate
			{
				DoStartBrawl();
			});
		}
		else
		{
			DoStartBrawl();
		}
	}

	public void DoStartBrawl()
	{
		GameData.instance.main.ShowLoading();
		BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(4), OnStart);
		BrawlDALC.instance.doStart();
	}

	private void OnStart(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(4), OnStart);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	private void DoRules(bool pub)
	{
		GameData.instance.main.ShowLoading();
		BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(13), OnRules);
		BrawlDALC.instance.doRules(new BrawlRules(pub));
	}

	private void OnRules(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(13), OnRules);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	public void OnKickConfirm(BrawlPlayer player)
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_kick"), Language.GetString("ui_brawl_kick_confirm", new string[2]
		{
			player.characterData.name,
			Language.GetString("ui_brawl")
		}), null, null, delegate
		{
			DoKick(player);
		});
	}

	private void DoKick(BrawlPlayer player)
	{
		GameData.instance.main.ShowLoading();
		BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(8), OnKick);
		BrawlDALC.instance.doKick(player.characterData.charID);
	}

	private void OnKick(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(8), OnKick);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	public void DoOrder(List<int> order)
	{
		GameData.instance.main.ShowLoading();
		BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(10), OnOrder);
		BrawlDALC.instance.doOrder(order);
	}

	private void OnOrder(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(10), OnOrder);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	public void DoLeave()
	{
		GameData.instance.main.ShowLoading();
		BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnLeave);
		BrawlDALC.instance.doLeave();
	}

	private void OnLeave(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnLeave);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0") && sfsob.GetInt("err0") != 130)
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			ForceClose();
		}
	}

	public void DoLeaveConfirm()
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_exit"), Language.GetString("battle_exit_confirm"), null, null, delegate
		{
			DoLeave();
		});
	}

	public override void OnClose()
	{
		DoLeaveConfirm();
	}

	public void ForceClose()
	{
		base.OnClose();
		GameData.instance.PROJECT.ShowBrawlWindow();
	}

	public override void DoDestroy()
	{
		base.DoDestroy();
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnChange);
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(6), OnMessage);
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(9), OnRemoved);
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(11), OnBegin);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		statsBG.interactable = true;
		energyBtn.interactable = true;
		startBtn.interactable = true;
		readyBtn.interactable = true;
		unreadyBtn.interactable = true;
		privateCheckBox.interactable = true;
		if (_chatBox != null)
		{
			_chatBox.DoEnable();
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		statsBG.interactable = false;
		energyBtn.interactable = false;
		startBtn.interactable = false;
		readyBtn.interactable = false;
		unreadyBtn.interactable = false;
		privateCheckBox.interactable = false;
		if (_chatBox != null)
		{
			_chatBox.DoDisable();
		}
	}
}
