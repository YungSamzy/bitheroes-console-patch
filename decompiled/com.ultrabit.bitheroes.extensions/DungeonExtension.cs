using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.ui;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.lists.teamlistondungeon;
using com.ultrabit.bitheroes.ui.utility;
using com.ultrabit.bitheroes.ui.victory;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.extensions;

public class DungeonExtension : Messenger
{
	public const int PLAYER_EXIT = 1;

	public const int PLAYER_DEFEAT = 2;

	public const int OBJECT_ADD = 3;

	public const int OBJECT_REMOVE = 4;

	public const int OBJECT_ACTIVATE = 5;

	public const int ENTITY_VALUES = 6;

	public const int ENTITY_UPDATE = 7;

	public const int ENTITY_ORDER = 8;

	public const int DUNGEON_COMPLETE = 9;

	public const int USE_CONSUMABLE = 10;

	public const int ITEMS_ADDED = 11;

	public const int ITEMS_REMOVED = 12;

	public const int OBJECT_DISABLE = 13;

	public const int ERROR = 14;

	public const int CURRENCY = 15;

	private Room _room;

	private Dungeon _dungeon;

	private bool _waiting;

	private bool _complete;

	private bool _ended;

	private bool _defeated;

	private bool _paused;

	private bool _resolvingAd;

	private List<SFSObject> _queue = new List<SFSObject>();

	private List<ItemData> _rewards;

	private DialogWindow _dialog;

	private bool _dialogBool;

	private bool _rerunDungeon;

	private DungeonVictoryWindow _dungeonVictoryWindow;

	public bool complete => _complete;

	public bool repeatable
	{
		get
		{
			bool flag = false;
			if (_dungeon.type == 1)
			{
				ZoneNodeDifficultyRef dungeonZoneNodeDifficultyRef = ZoneBook.GetDungeonZoneNodeDifficultyRef(_dungeon.dungeonRef);
				flag = ZoneBook.Lookup(dungeonZoneNodeDifficultyRef.zoneID).getNodeRef(dungeonZoneNodeDifficultyRef.nodeID).repeatable;
			}
			else if (_dungeon.type == 2)
			{
				flag = true;
			}
			if (flag && GameData.instance.PROJECT.character.selectedTeammates == null)
			{
				flag = false;
			}
			return flag;
		}
	}

	public Dungeon dungeon => _dungeon;

	public bool waiting => _waiting;

	public bool defeated => _defeated;

	public bool paused => _paused;

	public bool resolvingAd => _resolvingAd;

	public List<ItemData> rewards => _rewards;

	public bool reRunDungeon => _rerunDungeon;

	public DungeonExtension(int roomID)
	{
		_room = ServerExtension.instance.GetRoom(roomID);
		ServerExtension.instance.smartfox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
	}

	public void SetDungeon(Dungeon dungeon)
	{
		if (!(_dungeon != null))
		{
			_dungeon = dungeon;
		}
	}

	public void ShowDungeonCompleteWindow()
	{
		_dungeonVictoryWindow = GameData.instance.windowGenerator.NewDungeonVictoryWindow();
		if (_dungeonVictoryWindow != null)
		{
			GameData.instance.PROJECT.PauseDungeon();
			dungeon.CheckAutoPilot();
			SetPaused(pause: true);
			_dungeonVictoryWindow.LoadDetails(0, GameData.instance.PROJECT.character.dungeonLootItems, null, repeatable, OnExitYes);
			_dungeonVictoryWindow.ON_RERUN.AddListener(OnExitRerun);
		}
	}

	public void DoPlayerExit()
	{
		GameData.instance.PROJECT.ResumeDungeon();
		dungeon.CheckAutoPilot();
		SetPaused(pause: false);
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		Send(sFSObject);
		_dungeon.focus.ClearPath();
		_dungeon.TrackEnd(GetCompleteType());
		_ended = true;
		GameData.instance.main.ShowLoading();
	}

	public void DoObjectActivate(List<DungeonNode> path, bool wait = true, int currencyID = -1, int currencyCost = -1)
	{
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		foreach (DungeonNode item in path)
		{
			list.Add(item.row);
			list2.Add(item.column);
		}
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 5);
		sFSObject.PutIntArray("dun11", list.ToArray());
		sFSObject.PutIntArray("dun12", list2.ToArray());
		if (GameData.instance.PROJECT.character.toCharacterData().nftIsAdFree)
		{
			sFSObject.PutBool("cha130", val: true);
		}
		if (currencyID >= 0)
		{
			sFSObject.PutInt("curr0", currencyID);
		}
		if (currencyCost >= 0)
		{
			sFSObject.PutInt("curr2", currencyCost);
		}
		Send(sFSObject, wait);
	}

	public void DoEntityOrder(int[] order)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 8);
		sFSObject.PutIntArray("dun17", order);
		Send(sFSObject);
	}

	public void DoUseConsumable(int consumableID, int entityIndex)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 10);
		sFSObject.PutInt("ite0", consumableID);
		sFSObject.PutInt("dun18", entityIndex);
		Send(sFSObject);
	}

	public void RunQueue()
	{
		while (_queue.Count > 0)
		{
			SFSObject sfsob = _queue[0];
			_queue.RemoveAt(0);
			ParseSFSObject(sfsob);
		}
		ClearQueue();
	}

	public bool QueueContains(int action)
	{
		foreach (SFSObject item in _queue)
		{
			if (item.GetInt("act0") == action)
			{
				return true;
			}
		}
		return false;
	}

	private void AddToQueue(SFSObject sfsob)
	{
		_queue.Add(sfsob);
	}

	private void ClearQueue()
	{
		_queue.Clear();
	}

	public void SetWaiting(bool wait)
	{
		_waiting = wait;
		Broadcast(CustomSFSXEvent.CHANGE);
	}

	public void SetPaused(bool pause)
	{
		_paused = pause;
		Broadcast(CustomSFSXEvent.CHANGE);
		if (!_paused)
		{
			RunQueue();
		}
	}

	public void SetResolvingAd(bool resolvingAd)
	{
		_resolvingAd = resolvingAd;
	}

	private void OnExtensionResponse(BaseEvent e)
	{
		if (!((string)e.Params["cmd"] != "DungeonExtension") && !(_dungeon == null))
		{
			SFSObject sfsob = e.Params["params"] as SFSObject;
			ServerExtension.instance.stopTimeoutTimer();
			SetWaiting(wait: false);
			ParseSFSObject(sfsob);
		}
	}

	private void ParseSFSObject(SFSObject sfsob)
	{
		if (!sfsob.ContainsKey("act0"))
		{
			return;
		}
		int @int = sfsob.GetInt("act0");
		if (_paused)
		{
			AddToQueue(sfsob);
			return;
		}
		switch (@int)
		{
		case 1:
			OnPlayerExit(sfsob);
			break;
		case 2:
			OnPlayerDefeat(sfsob);
			break;
		case 3:
			OnObjectAdd(sfsob);
			break;
		case 4:
			OnObjectRemove(sfsob);
			break;
		case 6:
			OnEntityValues(sfsob);
			break;
		case 7:
			OnEntityUpdate(sfsob);
			break;
		case 8:
			OnEntityOrder(sfsob);
			break;
		case 9:
			OnDungeonComplete(sfsob);
			break;
		case 10:
			OnUseConsumable(sfsob);
			break;
		case 11:
			OnItemsAdded(sfsob);
			break;
		case 12:
			OnItemsRemoved(sfsob);
			break;
		case 13:
			OnObjectDisable(sfsob);
			break;
		case 14:
			OnError(sfsob);
			break;
		case 15:
			OnCurrency(sfsob);
			break;
		case 5:
			break;
		}
	}

	private void OnPlayerExit(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("dun8");
		if (@int == GameData.instance.PROJECT.character.id)
		{
			if (!_rerunDungeon)
			{
				GameData.instance.main.HideLoading();
			}
			_dungeon.COMPLETE.Invoke(this);
		}
		else
		{
			_dungeon.RemovePlayer(@int);
		}
		_dungeon.TrackEnd(GetCompleteType());
	}

	private void OnPlayerDefeat(SFSObject sfsob)
	{
		if (sfsob.GetInt("dun8") == GameData.instance.PROJECT.character.id)
		{
			_defeated = true;
			DoPlayerExit();
		}
	}

	private void OnObjectAdd(SFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("dun0");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			int @int = sFSObject.GetInt("dun1");
			int int2 = sFSObject.GetInt("dun2");
			DungeonObject obj = DungeonObject.FromSFSObject(sFSObject);
			_dungeon.GetNode(@int, int2).SetObject(obj);
		}
	}

	private void OnObjectRemove(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("dun1");
		int int2 = sfsob.GetInt("dun2");
		bool @bool = sfsob.GetBool("dun32");
		DungeonNode node = _dungeon.GetNode(@int, int2);
		DungeonObject obj = node.obj;
		if (obj != null && @bool)
		{
			switch (obj.type)
			{
			case 1:
				GameData.instance.audioManager.PlaySoundLink("treasure");
				break;
			case 3:
				GameData.instance.audioManager.PlaySoundLink("shrine");
				break;
			}
		}
		node.SetObject(null);
		if (!@bool)
		{
			_dungeon.CheckAutoPilot();
		}
	}

	private void OnEntityValues(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("dun8");
		bool @bool = sfsob.GetBool("dun27");
		DungeonPlayer player = _dungeon.GetPlayer(@int);
		if (player == null)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		ISFSArray sFSArray = sfsob.GetSFSArray("dun17");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			int int2 = sFSObject.GetInt("dun18");
			int int3 = sFSObject.GetInt("dun24");
			int int4 = sFSObject.GetInt("dun25");
			int int5 = sFSObject.GetInt("dun33");
			int int6 = sFSObject.GetInt("dun34");
			int int7 = sFSObject.GetInt("dun30");
			int int8 = sFSObject.GetInt("dun26");
			DungeonEntity entity = player.GetEntity(int2);
			if (entity == null)
			{
				continue;
			}
			if (@bool)
			{
				MyListItemViewsHolder entityTile = GameData.instance.PROJECT.dungeon.dungeonUI.GetEntityTile(entity.index);
				int num = int3 - entity.healthCurrent;
				if (num > 0)
				{
					GameData.instance.PROJECT.dungeon.dungeonUI.AddTileText(entityTile).LoadDetails("+" + Util.NumberFormat(num), BattleText.COLOR_GREEN, 3f, 0f, entityTile.root.transform.position.x, entityTile.root.transform.position.y);
					flag = true;
					player.AddShrinePrompts(entity.index, BattleText.COLOR_PINK, Language.GetString("ui_healed"));
				}
				if (int7 - entity.meter > 0 && int7 == VariableBook.battleMeterMax)
				{
					GameData.instance.PROJECT.dungeon.dungeonUI.AddTileText(entityTile).LoadDetails(Language.GetString("ui_max"), BattleText.COLOR_CYAN, 3f, 0f, entityTile.root.transform.position.x, entityTile.root.transform.position.y);
					player.AddShrinePrompts(entity.index, BattleText.COLOR_CYAN, Language.GetString("ui_max_sp"));
				}
			}
			entity.SetHealth(int3, int4);
			entity.SetShield(int5, int6);
			entity.SetMeter(int7);
			entity.SetConsumables(int8);
		}
		if (flag)
		{
			GameData.instance.audioManager.PlaySoundLink("heal");
		}
		if (flag2)
		{
			GameData.instance.audioManager.PlaySoundPoolLink("damage");
		}
	}

	private void OnEntityUpdate(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("dun8");
		DungeonPlayer player = _dungeon.GetPlayer(@int);
		if (!(player == null))
		{
			int int2 = sfsob.GetInt("dun18");
			DungeonEntity entity = player.GetEntity(int2);
			if (entity != null)
			{
				int int3 = sfsob.GetInt("dun21");
				int int4 = sfsob.GetInt("dun22");
				int int5 = sfsob.GetInt("dun23");
				int int6 = sfsob.GetInt("dun24");
				int int7 = sfsob.GetInt("dun25");
				int int8 = sfsob.GetInt("dun33");
				int int9 = sfsob.GetInt("dun34");
				int int10 = sfsob.GetInt("dun30");
				CharacterData characterData = CharacterData.fromSFSObject(sfsob);
				entity.SetPower(int3);
				entity.SetStamina(int4);
				entity.SetAgility(int5);
				entity.SetHealth(int6, int7);
				entity.SetShield(int8, int9);
				entity.SetMeter(int10);
				entity.SetCharacterData(characterData);
				player.UpdateFormation();
				entity.Broadcast("ENTITY_UPDATE");
			}
		}
	}

	private void OnEntityOrder(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("dun8");
		DungeonPlayer player = _dungeon.GetPlayer(@int);
		if (!(player == null))
		{
			int[] intArray = sfsob.GetIntArray("dun17");
			player.SetEntityOrder(intArray);
			_dungeon.dungeonUI.UpdateEntityTiles();
		}
	}

	private void OnDungeonComplete(SFSObject sfsob)
	{
		_complete = true;
		dungeon.dungeonUI.UpdateExitBtn();
		if (_dungeon.type == 1)
		{
			ZoneNodeDifficultyRef zoneNodeDifficultyRef = _dungeon.data as ZoneNodeDifficultyRef;
			ZoneData zoneData = GameData.instance.PROJECT.character.zones.getZoneData(zoneNodeDifficultyRef.zoneID, zoneNodeDifficultyRef.difficultyRef.id);
			if (zoneData == null)
			{
				zoneData = GameData.instance.PROJECT.character.zones.addZoneData(new ZoneData(zoneNodeDifficultyRef.getZoneRef(), zoneNodeDifficultyRef.difficultyRef, new List<int>()));
			}
			zoneData.setNodeCompleteCount(zoneNodeDifficultyRef.nodeID, zoneData.getNodeCompleteCount(zoneNodeDifficultyRef.nodeID) + 1);
		}
		if (sfsob.ContainsKey("ite3"))
		{
			_rewards = ItemData.listFromSFSObject(sfsob);
			GameData.instance.PROJECT.character.addItems(_rewards);
			KongregateAnalytics.checkEconomyTransaction("Zone Node Reward", null, _rewards, sfsob, "Zone", 2);
		}
		GameData.instance.PROJECT.character.analytics.incrementValue(BHAnalytics.DUNGEONS_WON);
		KongregateAnalytics.trackCrossPromotion();
	}

	public void ShowComplete()
	{
		if (!_dialogBool)
		{
			UnityAction onCancelCallback = delegate
			{
				ShowDungeonCompleteWindow();
			};
			_dialog = GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_complete"), Language.GetString("ui_complete_dungeon"), Language.GetString("ui_continue"), Language.GetString("ui_town"), null, onCancelCallback, null, -1, null, ButtonSquareColor.Default, ButtonSquareColor.Red);
			_dialog.DESTROYED.AddListener(OnCompleteClosed);
			_dialogBool = true;
		}
	}

	private void OnCompleteClosed(object e)
	{
		(e as DialogWindow).DESTROYED.RemoveListener(OnCompleteClosed);
		_dialog = null;
		_dialogBool = false;
	}

	public void ShowCleared()
	{
		if (!_dialogBool)
		{
			_dialogBool = true;
			ShowDungeonCompleteWindow();
		}
	}

	private void OnExitYes()
	{
		_dialog = null;
		_dialogBool = false;
		GameData.instance.audioManager.PlaySoundLink("dungeoncomplete");
		DoPlayerExit();
	}

	private void OnExitNo()
	{
		_dialog = null;
		_dialogBool = false;
	}

	public void OnExitRerun()
	{
		_dialog = null;
		_dialogBool = false;
		_rerunDungeon = true;
		DoPlayerExit();
	}

	private void OnUseConsumable(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("dun8");
		DungeonPlayer player = _dungeon.GetPlayer(@int);
		if (!(player == null))
		{
			int int2 = sfsob.GetInt("dun18");
			DungeonEntity entity = player.GetEntity(int2);
			if (entity != null)
			{
				int int3 = sfsob.GetInt("dun26");
				entity.SetConsumables(int3);
			}
		}
	}

	private void OnItemsAdded(SFSObject sfsob)
	{
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		GameData.instance.PROJECT.character.addItems(list, isDungeonLoot: true);
		_dungeon.AddCreditsGained(ItemData.getItemRefQuantity(list, CurrencyBook.Lookup(2)));
		_dungeon.AddGoldGained(ItemData.getItemRefQuantity(list, CurrencyBook.Lookup(1)));
		ItemListWindow itemWindow = GameData.instance.windowGenerator.NewItemListWindow(list, compare: true, added: true, null, large: false, forceNonEquipment: false, select: false, null, null, -1, Language.GetString("ui_collect"));
		itemWindow.DESTROYED.AddListener(OnItemWindowClosed);
		if (GameData.instance.PROJECT.character.autoPilot)
		{
			float num = 1.5f;
			if (AppInfo.TESTING)
			{
				num /= 3f;
			}
			GameData.instance.main.coroutineTimer.AddTimer(null, num, CoroutineTimer.TYPE.SECONDS, 1, delegate
			{
				DoItemWindowClosed(itemWindow);
			});
		}
		DungeonMerchantWindow dungeonMerchantWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(DungeonMerchantWindow)) as DungeonMerchantWindow;
		if (dungeonMerchantWindow != null)
		{
			GameData.instance.main.HideLoading();
			dungeonMerchantWindow.OnClose();
		}
		DungeonTreasureWindow dungeonTreasureWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(DungeonTreasureWindow)) as DungeonTreasureWindow;
		if ((bool)dungeonTreasureWindow)
		{
			GameData.instance.main.HideLoading();
			dungeonTreasureWindow.OnClose();
		}
		DungeonAdWindow dungeonAdWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(DungeonAdWindow)) as DungeonAdWindow;
		if ((bool)dungeonAdWindow)
		{
			GameData.instance.main.HideLoading();
			dungeonAdWindow.OnClose();
		}
	}

	private void DoItemWindowClosed(object e)
	{
		if ((!(GameData.instance.tutorialManager != null) || !GameData.instance.tutorialManager.hasPopup) && GameData.instance.PROJECT.character.autoPilot)
		{
			(e as ItemListWindow).OnClose();
		}
	}

	private void OnItemWindowClosed(object e)
	{
		if (e != null)
		{
			(e as ItemListWindow).DESTROYED.RemoveListener(OnItemWindowClosed);
		}
		if (_dungeon != null)
		{
			_dungeon.CheckAutoPilot();
		}
	}

	private void OnItemsRemoved(SFSObject sfsob)
	{
		List<ItemData> items = ItemData.listFromSFSObject(sfsob);
		GameData.instance.PROJECT.character.removeItems(items);
	}

	private void OnObjectDisable(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("dun1");
		int int2 = sfsob.GetInt("dun2");
		DungeonNode node = _dungeon.GetNode(@int, int2);
		DungeonObject obj = node.obj;
		if ((bool)obj)
		{
			switch (obj.type)
			{
			case 1:
				GameData.instance.audioManager.PlaySoundLink("treasure");
				break;
			case 3:
				GameData.instance.audioManager.PlaySoundLink("shrine");
				break;
			}
		}
		node.DisableObject();
		_dungeon.CheckAutoPilot();
	}

	private void OnError(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("err0");
		if (GameData.instance.windowGenerator.HasDialogByClass(typeof(DungeonMerchantWindow)))
		{
			GameData.instance.main.HideLoading();
		}
		if (GameData.instance.windowGenerator.HasDialogByClass(typeof(DungeonTreasureWindow)))
		{
			GameData.instance.main.HideLoading();
		}
		if (GameData.instance.windowGenerator.HasDialogByClass(typeof(DungeonAdWindow)))
		{
			GameData.instance.main.HideLoading();
		}
		GameData.instance.windowGenerator.ShowErrorCode(@int);
	}

	private void OnCurrency(SFSObject sfsob)
	{
		GameData.instance.PROJECT.character.checkCurrencyChanges(sfsob);
	}

	private string GetCompleteType()
	{
		if (_defeated)
		{
			return "Lose";
		}
		if (_complete)
		{
			return "Win";
		}
		return "Quit";
	}

	private void Send(SFSObject sfsob, bool wait = true)
	{
		if ((bool)_dungeon && !_ended)
		{
			ServerExtension.instance.Send(sfsob, _room, "DungeonExtension");
			if (wait)
			{
				ServerExtension.instance.startTimeoutTimer();
				SetWaiting(wait: true);
				Broadcast(CustomSFSXEvent.CHANGE);
			}
		}
	}

	public void Clear()
	{
		ClearQueue();
		_dungeon = null;
		if (ServerExtension.instance.smartfox != null)
		{
			ServerExtension.instance.smartfox.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
		}
	}

	public void DebugBools()
	{
	}
}
