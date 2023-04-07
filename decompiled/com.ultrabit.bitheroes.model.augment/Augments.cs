using System.Collections.Generic;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.familiar;
using Sfs2X.Entities.Data;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.augment;

public class Augments
{
	public const int SLOT_A = 0;

	public const int SLOT_B = 1;

	public const int SLOT_C = 2;

	public const int SLOT_D = 3;

	public const int SLOT_E = 4;

	public const int SLOT_F = 5;

	public const int SLOTS = 6;

	public UnityEvent OnChange = new UnityEvent();

	private List<AugmentData> _augments;

	public List<AugmentData> augments => _augments;

	public Augments(List<AugmentData> augments)
	{
		_augments = ((augments != null) ? augments : new List<AugmentData>());
	}

	public void Broadcast()
	{
		OnChange?.Invoke();
	}

	public void addAugment(AugmentData augmentData)
	{
		if (getAugment(augmentData.uid) == null)
		{
			_augments.Add(augmentData);
			Broadcast();
		}
	}

	public void removeAugment(long uid)
	{
		for (int i = 0; i < _augments.Count; i++)
		{
			if (_augments[i].uid == uid)
			{
				_augments.RemoveAt(i);
				Broadcast();
				break;
			}
		}
	}

	public void updateAugmentData(AugmentData augmentData, bool dispatch = true)
	{
		AugmentData augment = getAugment(augmentData.uid);
		if (augment != null)
		{
			augment.copyData(augmentData);
			if (dispatch)
			{
				Broadcast();
			}
		}
	}

	public AugmentData getAugment(long uid)
	{
		foreach (AugmentData augment in _augments)
		{
			if (augment.uid == uid)
			{
				return augment;
			}
		}
		return null;
	}

	public List<AugmentData> getFamiliarAugments(FamiliarRef familiarRef)
	{
		List<AugmentData> list = new List<AugmentData>();
		if (familiarRef == null)
		{
			return list;
		}
		foreach (AugmentData augment in _augments)
		{
			if (augment.familiarID == familiarRef.id && augment.slot >= 0)
			{
				list.Add(augment);
			}
		}
		return list;
	}

	public AugmentData getFamiliarAugmentSlot(FamiliarRef familiarRef, int slot)
	{
		if (familiarRef == null)
		{
			return null;
		}
		foreach (AugmentData augment in _augments)
		{
			if (augment.familiarID == familiarRef.id && augment.slot == slot)
			{
				return augment;
			}
		}
		return null;
	}

	public List<AugmentData> getFamiliarAugmentSlots(FamiliarRef familiarRef)
	{
		List<AugmentData> list = new List<AugmentData>();
		if (familiarRef == null)
		{
			return list;
		}
		foreach (AugmentData augment in _augments)
		{
			if (augment.familiarID == familiarRef.id && augment.slot >= 0)
			{
				while (list.Count <= augment.slot)
				{
					list.Add(null);
				}
				list[augment.slot] = augment;
			}
		}
		return list;
	}

	public AugmentData getFamiliarAugmentData(FamiliarRef familiarRef, int slot)
	{
		if (familiarRef == null)
		{
			return null;
		}
		foreach (AugmentData augment in _augments)
		{
			if (augment.familiarID == familiarRef.id && augment.slot == slot)
			{
				return augment;
			}
		}
		return null;
	}

	public void clearFamiliarAugments(FamiliarRef familiarRef)
	{
		if (familiarRef == null)
		{
			return;
		}
		foreach (AugmentData augment in _augments)
		{
			if (augment.familiarID == familiarRef.id)
			{
				augment.clearData();
			}
		}
	}

	public bool getSlotUnlocked(Character character, int slot)
	{
		AugmentSlotRef augmentSlotRef = AugmentBook.LookupSlot(slot);
		if (augmentSlotRef == null)
		{
			return false;
		}
		return character.level >= augmentSlotRef.levelReq;
	}

	public List<AugmentData> getAugmentsByType(int type)
	{
		List<AugmentData> list = new List<AugmentData>();
		foreach (AugmentData augment in _augments)
		{
			if (augment != null && augment.augmentRef.typeRef.id == type)
			{
				list.Add(augment);
			}
		}
		return list;
	}

	public void updateAugmentList(List<AugmentData> augments)
	{
		if (augments == null || augments.Count <= 0)
		{
			return;
		}
		foreach (AugmentData augment in augments)
		{
			updateAugmentData(augment, dispatch: false);
		}
		Broadcast();
	}

	public static Augments fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("aug0"))
		{
			return null;
		}
		return new Augments(AugmentData.listFromSFSObject(sfsob.GetSFSObject("aug0")));
	}

	public static bool slotListHasAugment(List<float> slots, AugmentData augmentData)
	{
		if (slots == null || augmentData == null || slots.Count <= 0)
		{
			return false;
		}
		foreach (float slot in slots)
		{
			if ((float)augmentData.uid == slot)
			{
				return true;
			}
		}
		return false;
	}
}
