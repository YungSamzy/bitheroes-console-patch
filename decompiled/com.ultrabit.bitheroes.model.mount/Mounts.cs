using System.Collections.Generic;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.mount;

public class Mounts
{
	private long _equipped;

	private MountRef _cosmetic;

	private List<MountData> _mounts;

	public UnityEvent OnChange = new UnityEvent();

	public UnityEvent OnCosmeticChange = new UnityEvent();

	public MountRef cosmetic => _cosmetic;

	public List<MountData> mounts => _mounts;

	public Mounts(long equipped, MountRef cosmetic, List<MountData> mounts)
	{
		_equipped = equipped;
		if (mounts != null)
		{
			_mounts = mounts;
		}
		else
		{
			D.LogError("No Mount Data");
		}
		setCosmetic(cosmetic);
	}

	public void setCosmetic(MountRef mountRef)
	{
		_cosmetic = mountRef;
		BroadcastCosmetic();
	}

	public void setEquipped(MountData mountData, bool doDispatch = true)
	{
		_equipped = mountData?.uid ?? 0;
		if (doDispatch)
		{
			Broadcast();
		}
	}

	public void addMount(MountData mountData)
	{
		if (getMount(mountData.uid) == null)
		{
			_mounts.Add(mountData);
			Broadcast();
		}
	}

	public void removeMount(long uid)
	{
		for (int i = 0; i < _mounts.Count; i++)
		{
			MountData mountData = _mounts[i];
			if (mountData.uid == uid)
			{
				_mounts.Remove(mountData);
				Broadcast();
				break;
			}
		}
	}

	public void updateMount(MountData mountData)
	{
		if (mountData != null)
		{
			MountData mount = getMount(mountData.uid);
			if (mount != null)
			{
				mount.copyData(mountData);
				Broadcast();
			}
		}
	}

	public void Broadcast()
	{
		OnChange?.Invoke();
	}

	public void BroadcastCosmetic()
	{
		OnCosmeticChange?.Invoke();
	}

	public MountData getMount(long uid)
	{
		foreach (MountData mount in _mounts)
		{
			if (mount.uid == uid)
			{
				return mount;
			}
		}
		return null;
	}

	public MountRef getCosmeticMount(int id)
	{
		foreach (MountData mount in _mounts)
		{
			if (mount.mountRef.id == id)
			{
				return mount.mountRef;
			}
		}
		foreach (MountRef fullMount in MountBook.GetFullMountList())
		{
			if (!(fullMount == null) && fullMount.cosmetic && fullMount.id == id)
			{
				return fullMount;
			}
		}
		return null;
	}

	public List<GameModifier> getGameModifiers()
	{
		List<GameModifier> list = new List<GameModifier>();
		MountData mountEquipped = getMountEquipped();
		if (mountEquipped != null)
		{
			foreach (GameModifier gameModifier in mountEquipped.getGameModifiers())
			{
				list.Add(gameModifier);
			}
			return list;
		}
		return list;
	}

	public MountData getMountEquipped()
	{
		return getMount(_equipped);
	}

	public long getMountEquippedUID()
	{
		return getMountEquipped()?.uid ?? 0;
	}

	public static Mounts fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("moun0"))
		{
			return null;
		}
		ISFSObject sFSObject = sfsob.GetSFSObject("moun0");
		if (sFSObject == null)
		{
			return null;
		}
		long @long = sFSObject.GetLong("moun6");
		MountRef mountRef = MountBook.Lookup(sFSObject.GetInt("moun11"));
		List<MountData> list = MountData.listFromSFSObject(sFSObject);
		return new Mounts(@long, mountRef, list);
	}
}
