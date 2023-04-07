using System.Collections.Generic;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.events;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.brawl;

public class BrawlRoom : Messenger
{
	private int _index;

	private BrawlTierDifficultyRef _difficultyRef;

	private long _createMilliseconds;

	private int _leader;

	private List<BrawlPlayer> _slots;

	private BrawlRules _rules;

	public int index => _index;

	public BrawlTierDifficultyRef difficultyRef => _difficultyRef;

	public int leader => _leader;

	public List<BrawlPlayer> slots => _slots;

	public BrawlRules rules => _rules;

	public int brawlID => _difficultyRef.brawlRef.id;

	public int tierID => _difficultyRef.tierRef.id;

	public int difficultyID => _difficultyRef.id;

	public BrawlRoom(int index, BrawlTierDifficultyRef difficultyRef, long createMilliseconds)
	{
		_index = index;
		_difficultyRef = difficultyRef;
		_createMilliseconds = createMilliseconds;
	}

	public void checkChanges(BrawlRoom room)
	{
		if (room.index == _index)
		{
			_leader = room.leader;
			setSlots(room.slots);
			setRules(room.rules);
			Broadcast(CustomSFSXEvent.CHANGE);
		}
	}

	public void setSlots(List<BrawlPlayer> slots)
	{
		_slots = slots;
	}

	public void setLeader(int leader)
	{
		_leader = leader;
	}

	public void setRules(BrawlRules rules)
	{
		_rules = rules;
	}

	public bool hasPlayer(int charID)
	{
		return getPlayer(charID) != null;
	}

	public BrawlPlayer getPlayer(int charID)
	{
		foreach (BrawlPlayer slot in _slots)
		{
			if (slot != null && slot.characterData.charID == charID)
			{
				return slot;
			}
		}
		return null;
	}

	public void addPlayer(BrawlPlayer player, int slot)
	{
		_slots[slot] = player;
	}

	public void removePlayer(int charID)
	{
		for (int i = 0; i < _slots.Count; i++)
		{
			BrawlPlayer brawlPlayer = _slots[i];
			if (brawlPlayer != null && brawlPlayer.characterData.charID == charID)
			{
				_slots[i] = null;
				break;
			}
		}
	}

	public List<BrawlPlayer> getPlayers()
	{
		List<BrawlPlayer> list = new List<BrawlPlayer>();
		foreach (BrawlPlayer slot in _slots)
		{
			if (slot != null)
			{
				list.Add(slot);
			}
		}
		return list;
	}

	public bool getReady()
	{
		foreach (BrawlPlayer slot in _slots)
		{
			if (slot != null && !slot.ready && slot.characterData.charID != _leader)
			{
				return false;
			}
		}
		return true;
	}

	public bool isFull()
	{
		foreach (BrawlPlayer slot in _slots)
		{
			if (slot == null)
			{
				return false;
			}
		}
		return true;
	}

	public bool isEmpty()
	{
		foreach (BrawlPlayer slot in _slots)
		{
			if (slot != null)
			{
				return false;
			}
		}
		return true;
	}

	public List<int> getPlayerOrder()
	{
		List<int> list = new List<int>();
		foreach (BrawlPlayer slot in _slots)
		{
			if (slot == null)
			{
				list.Add(0);
			}
			else
			{
				list.Add(slot.characterData.charID);
			}
		}
		return list;
	}

	public static BrawlRoom fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("bra1");
		int int2 = sfsob.GetInt("bra2");
		int int3 = sfsob.GetInt("bra3");
		int int4 = sfsob.GetInt("bra4");
		int int5 = sfsob.GetInt("bra5");
		long @long = sfsob.GetLong("bra8");
		List<BrawlPlayer> list = BrawlPlayer.listFromSFSObject(sfsob);
		BrawlRules brawlRules = BrawlRules.fromSFSObject(sfsob);
		BrawlTierDifficultyRef difficulty = BrawlBook.Lookup(int2).getTier(int3).getDifficulty(int4);
		BrawlRoom brawlRoom = new BrawlRoom(@int, difficulty, @long);
		brawlRoom.setLeader(int5);
		brawlRoom.setSlots(list);
		brawlRoom.setRules(brawlRules);
		return brawlRoom;
	}

	public static List<BrawlRoom> listFromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("bra12"))
		{
			return null;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("bra12");
		List<BrawlRoom> list = new List<BrawlRoom>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			BrawlRoom item = fromSFSObject(sFSArray.GetSFSObject(i));
			list.Add(item);
		}
		return list;
	}
}
