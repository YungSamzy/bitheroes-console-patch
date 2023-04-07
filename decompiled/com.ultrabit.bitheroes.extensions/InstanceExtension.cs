using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.bait;
using com.ultrabit.bitheroes.model.boober;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.ui.grid;
using com.ultrabit.bitheroes.ui.instance;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.extensions;

public class InstanceExtension : Messenger
{
	public const int PLAYER_ENTER = 0;

	public const int PLAYER_EXIT = 1;

	public const int PLAYER_MOVEMENT = 2;

	public const int PLAYER_UPDATE = 3;

	public const int PLAYER_LIST = 4;

	public const int DATA_UPDATE = 5;

	public const int FISHING_START = 6;

	public const int FISHING_CASTING = 7;

	public const int FISHING_CAST = 8;

	public const int FISHING_CATCHING = 9;

	public const int FISHING_CATCH = 10;

	public const int FISHING_END = 11;

	private Room _room;

	private int _roomID;

	private int _zoneID;

	private Instance _instance;

	private bool _waiting;

	public Instance instance => _instance;

	public bool waiting => _waiting;

	public InstanceExtension(int roomID, int zoneID)
	{
		_room = ServerExtension.instance.GetRoom(roomID);
		_roomID = roomID;
		_zoneID = zoneID;
		ServerExtension.instance.smartfox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
	}

	public void SetInstance(Instance instance)
	{
		if (!(_instance != null))
		{
			_instance = instance;
		}
	}

	public void DoPlayerExit()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		Send(sFSObject);
	}

	public void DoPlayerMovement(Tile tile, float speedMult)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		sFSObject.PutInt("ins2", tile.id);
		sFSObject.PutFloat("ins3", speedMult);
		Send(sFSObject);
	}

	public void DoFishingStart(int tileID, EquipmentRef rodRef, BobberRef bobberRef, BaitRef baitRef, bool flipped)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 6);
		sFSObject.PutInt("ins2", tileID);
		sFSObject.PutInt("fis0", (rodRef != null) ? rodRef.id : (-1));
		sFSObject.PutInt("fis1", (bobberRef != null) ? bobberRef.id : (-1));
		sFSObject.PutInt("fis2", (baitRef != null) ? baitRef.id : (-1));
		sFSObject.PutBool("ins5", flipped);
		Send(sFSObject);
	}

	public void DoFishingCasting()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 7);
		Send(sFSObject);
	}

	public void DoFishingCast(int distance)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 8);
		sFSObject.PutInt("fis3", distance);
		Send(sFSObject);
	}

	public void DoFishingCatching()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 9);
		Send(sFSObject);
	}

	public void DoFishingCatch(FishingBarChanceRef chanceRef = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 10);
		sFSObject.PutInt("fis6", chanceRef?.id ?? (-1));
		Send(sFSObject);
	}

	public void DoFishingEnd()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 11);
		Send(sFSObject);
	}

	private void OnExtensionResponse(BaseEvent e)
	{
		if (!((string)e.Params["cmd"] != "InstanceExtension") && !(_instance == null))
		{
			SFSObject sFSObject = e.Params["params"] as SFSObject;
			int @int = sFSObject.GetInt("roo1");
			int int2 = sFSObject.GetInt("roo3");
			if (@int == _roomID && int2 == _zoneID)
			{
				_waiting = false;
				ServerExtension.instance.stopTimeoutTimer();
				Broadcast(CustomSFSXEvent.CHANGE);
				ParseSFSObject(sFSObject);
			}
		}
	}

	private void ParseSFSObject(SFSObject sfsob)
	{
		if (sfsob.ContainsKey("act0"))
		{
			switch (sfsob.GetInt("act0"))
			{
			case 0:
				OnPlayerEnter(sfsob);
				break;
			case 1:
				OnPlayerExit(sfsob);
				break;
			case 2:
				OnPlayerMovement(sfsob);
				break;
			case 3:
				OnPlayerUpdate(sfsob);
				break;
			case 4:
				OnPlayerList(sfsob);
				break;
			case 5:
				OnDataUpdate(sfsob);
				break;
			case 6:
				OnFishingStart(sfsob);
				break;
			case 7:
				OnFishingCasting(sfsob);
				break;
			case 8:
				OnFishingCast(sfsob);
				break;
			case 9:
				OnFishingCatching(sfsob);
				break;
			case 10:
				OnFishingCatch(sfsob);
				break;
			case 11:
				OnFishingEnd(sfsob);
				break;
			}
		}
	}

	private void OnPlayerEnter(SFSObject sfsob)
	{
		_instance.AddPlayer(sfsob);
	}

	private void OnPlayerExit(SFSObject sfsob)
	{
		if (sfsob == null || _instance == null)
		{
			return;
		}
		int charID = sfsob.GetInt("cha1");
		if (_instance.GetPlayer(charID) == null)
		{
			return;
		}
		if (charID == GameData.instance.PROJECT.character.id)
		{
			GameData.instance.main.coroutineTimer.AddTimer(null, 500f, delegate
			{
				if (_instance != null)
				{
					_instance.RemovePlayer(charID);
				}
			});
		}
		else if (_instance != null)
		{
			_instance.RemovePlayer(charID);
		}
	}

	private void OnPlayerMovement(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("cha1");
		InstancePlayer player = _instance.GetPlayer(@int);
		if (!(player == null) && !player.isMe)
		{
			int int2 = sfsob.GetInt("ins2");
			Tile tileByID = _instance.getTileByID(int2);
			if (tileByID != null)
			{
				float @float = sfsob.GetFloat("ins3");
				player.SetSpeedMult(@float);
				List<Tile> path = _instance.GeneratePath(player, tileByID);
				player.SetPath(path);
			}
		}
	}

	private void OnPlayerUpdate(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("cha1");
		InstancePlayer player = _instance.GetPlayer(@int);
		if (!(player == null))
		{
			player.SetCharacterData(CharacterData.fromSFSObject(sfsob));
		}
	}

	private void OnPlayerList(SFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("ins1");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			_instance.AddPlayer(sFSObject);
		}
	}

	private void OnDataUpdate(SFSObject sfsob)
	{
		if (sfsob.GetInt("cha1") != GameData.instance.PROJECT.character.id)
		{
			object data = Instance.DataFromSFSObject(sfsob, _instance.instanceRef);
			_instance.SetData(data);
		}
	}

	private void OnFishingStart(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("cha1");
		InstancePlayer player = _instance.GetPlayer(@int);
		if (!(player == null) && !player.isMe)
		{
			int int2 = sfsob.GetInt("ins2");
			bool @bool = sfsob.GetBool("ins5");
			InstanceFishingData fishingData = InstanceFishingData.fromSFSObject(sfsob);
			Tile tileByID = _instance.getTileByID(int2);
			if (tileByID != null)
			{
				player.SetTile(tileByID, tween: false);
			}
			player.SetFlipped(@bool);
			player.SetFishingData(fishingData);
		}
	}

	private void OnFishingCasting(SFSObject sfsob)
	{
		if (_instance == null)
		{
			return;
		}
		int @int = sfsob.GetInt("cha1");
		InstancePlayer player = _instance.GetPlayer(@int);
		if (!(player == null) && !player.isMe)
		{
			if (player.fishingData != null)
			{
				player.fishingData.setState(2);
			}
			player.UpdateAnimation();
		}
	}

	private void OnFishingCast(SFSObject sfsob)
	{
		if (_instance == null)
		{
			return;
		}
		int @int = sfsob.GetInt("cha1");
		InstancePlayer player = _instance.GetPlayer(@int);
		if (player == null)
		{
			return;
		}
		int int2 = sfsob.GetInt("fis3");
		if (player.fishingData != null)
		{
			player.fishingData.setDistance(int2);
			player.fishingData.setItemRef(FishingBook.LookupItem(sfsob.GetInt("fis5")));
			FishingBook.LookupItem(sfsob.GetInt("fis5"));
			player.fishingData.setState(3);
			if (player.isMe)
			{
				_instance.instanceFishingInterface.DoFishingCatching();
			}
			player.UpdateAnimation();
		}
	}

	private void OnFishingCatching(SFSObject sfsob)
	{
		if (_instance == null)
		{
			return;
		}
		int @int = sfsob.GetInt("cha1");
		InstancePlayer player = _instance.GetPlayer(@int);
		if (!(player == null))
		{
			if (player.fishingData != null)
			{
				player.fishingData.setState(4);
			}
			player.UpdateAnimation();
			if (player.isMe)
			{
				_instance.instanceFishingInterface.DoFishingCatchStart();
			}
		}
	}

	private void OnFishingCatch(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("cha1");
		InstancePlayer player = _instance.GetPlayer(@int);
		if (!(player == null))
		{
			int int2 = sfsob.GetInt("fis5");
			int int3 = sfsob.GetInt("fis8");
			bool @bool = sfsob.GetBool("fis7");
			List<ItemData> items = ItemData.listFromSFSObject(sfsob);
			if (player.fishingData != null)
			{
				player.fishingData.setState(1);
			}
			player.UpdateAnimation();
			player.SetFishingData(null);
			FishingItemRef fishingItemRef = FishingBook.LookupItem(int2);
			if (player.isMe && _instance.instanceFishingInterface != null)
			{
				_instance.instanceFishingInterface.DoFishingCatchComplete(fishingItemRef, items, int3, @bool);
			}
			else if (@bool)
			{
				player.ShowItemObtained(fishingItemRef.itemRef);
			}
		}
	}

	private void OnFishingEnd(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("cha1");
		InstancePlayer player = _instance.GetPlayer(@int);
		if (!(player == null) && !player.isMe)
		{
			player.SetFishingData(null);
			player.UpdateAnimation();
		}
	}

	private void Send(SFSObject sfsob)
	{
		if (!(_instance == null))
		{
			ServerExtension.instance.Send(sfsob, _room, "InstanceExtension");
			_waiting = true;
			ServerExtension.instance.startTimeoutTimer();
			Broadcast(CustomSFSXEvent.CHANGE);
		}
	}

	public void Clear()
	{
		if (ServerExtension.instance.smartfox != null)
		{
			ServerExtension.instance.smartfox.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
		}
		_instance = null;
	}
}
