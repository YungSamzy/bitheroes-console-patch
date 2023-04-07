using System.Collections.Generic;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.bait;
using com.ultrabit.bitheroes.model.boober;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.game;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.instance;

public class InstanceFishingData : Messenger
{
	public const int STATE_IDLE = 1;

	public const int STATE_CASTING = 2;

	public const int STATE_CAST = 3;

	public const int STATE_CATCHING = 4;

	private EquipmentRef _rodRef;

	private BobberRef _bobberRef;

	private BaitRef _baitRef;

	private int _distance;

	private int _state = 1;

	private FishingItemRef _itemRef;

	public EquipmentRef rodRef => _rodRef;

	public BobberRef bobberRef => _bobberRef;

	public BaitRef baitRef => _baitRef;

	public int distance => _distance;

	public int state => _state;

	public FishingItemRef itemRef => _itemRef;

	public InstanceFishingData(EquipmentRef rodRef, BobberRef bobberRef, BaitRef baitRef, int distance)
	{
		_rodRef = rodRef;
		_bobberRef = bobberRef;
		_baitRef = baitRef;
		_distance = distance;
	}

	public void setDistance(int distance)
	{
		_distance = distance;
		Broadcast(CustomSFSXEvent.CHANGE);
	}

	public void setState(int state)
	{
		_state = state;
		Broadcast(CustomSFSXEvent.CHANGE);
	}

	public void setItemRef(FishingItemRef itemRef)
	{
		_itemRef = itemRef;
		Broadcast(CustomSFSXEvent.CHANGE);
	}

	public List<GameModifier> getModifiers()
	{
		List<GameModifier> list = new List<GameModifier>();
		if (_rodRef.modifiers != null)
		{
			foreach (GameModifier modifier in _rodRef.modifiers)
			{
				list.Add(modifier);
			}
		}
		if (_bobberRef.modifiers != null)
		{
			foreach (GameModifier modifier2 in _bobberRef.modifiers)
			{
				list.Add(modifier2);
			}
		}
		if (_baitRef.modifiers != null)
		{
			foreach (GameModifier modifier3 in _baitRef.modifiers)
			{
				list.Add(modifier3);
			}
			return list;
		}
		return list;
	}

	public static InstanceFishingData fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("fis0"))
		{
			return null;
		}
		int @int = sfsob.GetInt("fis0");
		int int2 = sfsob.GetInt("fis1");
		int int3 = sfsob.GetInt("fis2");
		int int4 = sfsob.GetInt("fis3");
		int int5 = sfsob.GetInt("fis4");
		FishingItemRef fishingItemRef = FishingBook.LookupItem(sfsob.GetInt("fis5"));
		InstanceFishingData instanceFishingData = new InstanceFishingData(EquipmentBook.Lookup(@int), BobberBook.Lookup(int2), BaitBook.Lookup(int3), int4);
		instanceFishingData.setState(int5);
		instanceFishingData.setItemRef(fishingItemRef);
		return instanceFishingData;
	}
}
