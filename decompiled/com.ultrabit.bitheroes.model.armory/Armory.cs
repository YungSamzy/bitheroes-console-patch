using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.armory.enchant;
using com.ultrabit.bitheroes.model.armory.rune;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rune;
using Sfs2X.Core;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.armory;

public class Armory
{
	private List<ArmoryEquipment> _armoryEquipmentSlots;

	private ArmoryEquipment _currentArmoryEquipmentSlot;

	private List<ArmoryBattleType> _battleTypes;

	private uint currentEquipIndex;

	private bool _battleArmorySelected;

	public List<ArmoryEquipment> armoryEquipmentSlots => _armoryEquipmentSlots;

	public List<ArmoryBattleType> battleTypes
	{
		get
		{
			return _battleTypes;
		}
		set
		{
			_battleTypes = value;
		}
	}

	public ArmoryEquipment currentArmoryEquipmentSlot
	{
		get
		{
			if (_currentArmoryEquipmentSlot == null)
			{
				SetCurrentArmoryEquipmentSlotByIdx(0u);
			}
			return _currentArmoryEquipmentSlot;
		}
	}

	public bool battleArmorySelected
	{
		get
		{
			return _battleArmorySelected;
		}
		set
		{
			_battleArmorySelected = value;
		}
	}

	public Armory(List<ArmoryEquipment> pArmoryEquipmentSlots)
	{
		_armoryEquipmentSlots = pArmoryEquipmentSlots;
	}

	public void SetArmoryEquipmentByIndex(uint idx, ArmoryEquipment aequip)
	{
		_armoryEquipmentSlots[(int)idx] = aequip;
	}

	public void CloneEquipmentForIndex(uint idx, bool doCreate = false)
	{
		currentEquipIndex = idx;
		if (doCreate)
		{
			string name = Language.GetString("ui_armory_generic_name") + " " + (idx + 1);
			CharacterDALC.instance.doCreateArmorySlot(idx + 1, name);
			GameData.instance.main.ShowLoading();
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(54), OnArmoryCreated);
		}
		else
		{
			UpdateArmoryEquipmentWithCharacterEquipment(_armoryEquipmentSlots[(int)currentEquipIndex]);
		}
	}

	public void UpdateArmorySlot(List<ItemData> itemsAdded, EquipmentRef prevEquipRef)
	{
		foreach (ItemData item in itemsAdded)
		{
			for (int i = 0; i < armoryEquipmentSlots.Count; i++)
			{
				ArmoryEquipment armoryEquipment = armoryEquipmentSlots[i];
				if (armoryEquipment != null)
				{
					ArmoryRef armoryEquipmentSlotByArmoryType = armoryEquipment.GetArmoryEquipmentSlotByArmoryType(prevEquipRef.equipmentType);
					if (armoryEquipmentSlotByArmoryType != null && armoryEquipmentSlotByArmoryType.id == prevEquipRef.id && prevEquipRef != null)
					{
						int availableSlot = ArmoryEquipment.GetAvailableSlot(prevEquipRef.equipmentType);
						armoryEquipment.SetArmoryEquipmentSlot(item.itemRef as ArmoryRef, availableSlot);
						CharacterDALC.instance.doSaveArmory(armoryEquipment);
					}
				}
			}
		}
	}

	public void UpdateArmorySlotWithItemRef(List<ItemData> itemsAdded, ItemRef prevItemRef)
	{
		foreach (ItemData item in itemsAdded)
		{
			for (int i = 0; i < armoryEquipmentSlots.Count; i++)
			{
				ArmoryEquipment armoryEquipment = armoryEquipmentSlots[i];
				int num = 0;
				foreach (ArmoryRef item2 in new List<ArmoryRef>(armoryEquipment.armorySlots.Values))
				{
					if (item2 != null && item2.id == prevItemRef.id)
					{
						if (prevItemRef.subtype == item.itemRef.subtype)
						{
							ArmoryRef armoryRefFromEquipmentRef = (item.itemRef as EquipmentRef).GetArmoryRefFromEquipmentRef();
							armoryEquipment.SetArmoryEquipmentSlot(armoryRefFromEquipmentRef, num);
						}
						else
						{
							armoryEquipment.SetArmoryEquipmentSlot(null, num);
						}
						CharacterDALC.instance.doSaveArmory(armoryEquipment);
					}
					num++;
				}
			}
		}
	}

	public void updateArmorySlot(List<ItemData> itemsAdded, EquipmentRef prevEquipRef)
	{
		foreach (ItemData item in itemsAdded)
		{
			for (int i = 0; i < armoryEquipmentSlots.Count; i++)
			{
				ArmoryEquipment armoryEquipment = armoryEquipmentSlots[i];
				ArmoryRef armoryEquipmentSlotByArmoryType = armoryEquipment.GetArmoryEquipmentSlotByArmoryType(prevEquipRef.equipmentType);
				if (armoryEquipmentSlotByArmoryType != null && armoryEquipmentSlotByArmoryType.id == prevEquipRef.id)
				{
					int availableSlot = ArmoryEquipment.GetAvailableSlot(prevEquipRef.equipmentType);
					armoryEquipment.SetArmoryEquipmentSlot(ArmoryRef.EquipmentRefToArmoryRef((EquipmentRef)item.itemRef), availableSlot);
					CharacterDALC.instance.doSaveArmory(armoryEquipment);
				}
			}
		}
	}

	public bool IsEquipped(EquipmentRef equipRef)
	{
		for (int i = 0; i < armoryEquipmentSlots.Count; i++)
		{
			foreach (KeyValuePair<int, ArmoryRef> armorySlot in armoryEquipmentSlots[i].armorySlots)
			{
				if (armorySlot.Value != null && armorySlot.Value.id == equipRef.id)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void OnArmoryCreated(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		ArmoryEquipment aequip = ArmoryEquipment.FromSFSObject(obj.sfsob);
		SetArmoryEquipmentByIndex(currentEquipIndex, aequip);
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(54), OnArmoryCreated);
		SetCurrentArmoryEquipmentSlotByIdx(currentEquipIndex);
		UpdateArmoryEquipmentWithCharacterEquipment(aequip);
	}

	private void UpdateArmoryEquipmentWithCharacterEquipment(ArmoryEquipment aequip)
	{
		Equipment equipment = GameData.instance.PROJECT.character.equipment;
		for (uint num = 0u; num < 8; num++)
		{
			ArmoryRef equipRef = equipment.getEquipmentSlot((int)num) as ArmoryRef;
			aequip.SetArmoryEquipmentSlot(equipRef, (int)num);
		}
		if (GameData.instance.PROJECT.character.mounts.getMountEquipped() != null)
		{
			aequip.mount = GameData.instance.PROJECT.character.mounts.getMountEquipped().uid;
		}
		else
		{
			aequip.mount = 0L;
		}
		ArmoryRunes runes = new ArmoryRunes(GameData.instance.PROJECT.character.runes.runeSlots, GameData.instance.PROJECT.character.runes.runeSlotsMemory);
		aequip.SetRunes(runes);
		ArmoryEnchants enchants = new ArmoryEnchants(GameData.instance.PROJECT.character.enchants.slots, GameData.instance.PROJECT.character.enchants.enchants);
		aequip.SetEnchants(enchants);
		CharacterDALC.instance.updateArmoryRunes(aequip);
		CharacterDALC.instance.updateArmoryEnchants(aequip);
		CharacterDALC.instance.doSaveArmory(aequip);
	}

	public static Armory FromSFSObject(ISFSObject sfsob)
	{
		ISFSObject sFSObject = sfsob.GetSFSObject("cha102");
		ISFSObject sFSObject2 = sfsob.GetSFSObject("cha106");
		return new Armory(ArmoryEquipment.ListFromSFSObject(sFSObject))
		{
			battleTypes = ArmoryBattleType.ListFromSFSObject(sFSObject2)
		};
	}

	public void SetCurrentArmoryEquipmentSlot(ArmoryEquipment aequip)
	{
		_currentArmoryEquipmentSlot = aequip;
	}

	public void SetCurrentArmoryEquipmentSlotByIdx(uint idx)
	{
		if (_armoryEquipmentSlots.Count > idx)
		{
			_currentArmoryEquipmentSlot = _armoryEquipmentSlots[(int)idx];
		}
	}

	public void SetCurrentArmoryEquipmentSlotByID(long id)
	{
		for (int i = 0; i < _armoryEquipmentSlots.Count; i++)
		{
			if (_armoryEquipmentSlots[i].id == id)
			{
				_currentArmoryEquipmentSlot = _armoryEquipmentSlots[i];
				break;
			}
		}
	}

	public void CheckArmoryEquipmentSlotState(Inventory inventory)
	{
		bool flag = false;
		for (int i = 0; i < _armoryEquipmentSlots.Count; i++)
		{
			if (_armoryEquipmentSlots[i].CheckArmoryEquipmentState(inventory))
			{
				flag = true;
			}
		}
		if (flag && (GameData.instance.PROJECT.dungeon == null || GameData.instance.PROJECT.battle == null))
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("ui_armory_change_items_removed"));
		}
	}

	public void RemoveRuneFromArmoryIfEquipped(RuneRef runeRef)
	{
		for (int i = 0; i < _armoryEquipmentSlots.Count; i++)
		{
			if (_armoryEquipmentSlots[i].RemoveRuneIfEquipped(runeRef))
			{
				CharacterDALC.instance.updateArmoryRunes(_armoryEquipmentSlots[i]);
			}
		}
	}

	public void RemoveEnchantFromArmoryIfEquipped(EnchantData enchantData)
	{
		for (int i = 0; i < _armoryEquipmentSlots.Count; i++)
		{
			_armoryEquipmentSlots[i].RemoveEnchantIfEquipped(enchantData);
		}
	}

	public ArmoryEquipment GetArmoryEquipmentByID(long id)
	{
		foreach (ArmoryEquipment armoryEquipmentSlot in _armoryEquipmentSlots)
		{
			if (armoryEquipmentSlot.id == id)
			{
				return armoryEquipmentSlot;
			}
		}
		return null;
	}

	public int GetEquipmentCount(ItemRef itemRef)
	{
		int num = 0;
		foreach (ArmoryEquipment armoryEquipmentSlot in _armoryEquipmentSlots)
		{
			foreach (KeyValuePair<int, ArmoryRef> armorySlot in armoryEquipmentSlot.armorySlots)
			{
				if (!(armorySlot.Value == null) && armorySlot.Value.id == itemRef.id)
				{
					num++;
				}
			}
		}
		return num;
	}
}
