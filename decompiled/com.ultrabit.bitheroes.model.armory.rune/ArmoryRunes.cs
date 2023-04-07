using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.rune;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.armory.rune;

public class ArmoryRunes : Runes
{
	public const string MAJORA = "MajorA";

	public const string MAJORB = "MajorB";

	public const string MAJORC = "MajorC";

	public const string MAJORD = "MajorD";

	public const string MINORA = "MinorA";

	public const string MINORB = "MinorB";

	public const string META = "Meta";

	public const string RELIC = "Relic";

	public const string ARTIFACT = "Artifact";

	private int _id;

	public int id => _id;

	public ArmoryRunes(Dictionary<int, RuneRef> runeSlots = null, Dictionary<int, List<RuneRef>> runeSlotsMemory = null)
		: base(runeSlots, runeSlotsMemory)
	{
	}

	public void setID(int id)
	{
		_id = id;
	}

	public override List<RuneRef> getRuneSlotMemory(int slot)
	{
		return GameData.instance.PROJECT.character.runes.getRuneSlotMemory(slot);
	}

	public bool isRuneEquipped(RuneRef runeRef)
	{
		foreach (KeyValuePair<int, RuneRef> runeSlot in base.runeSlots)
		{
			if (runeSlot.Value != null && runeSlot.Value.id == runeRef.id)
			{
				return true;
			}
		}
		return false;
	}

	public new static ArmoryRunes fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("run0"))
		{
			if (GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot != null)
			{
				return GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.runes;
			}
			return null;
		}
		ArmoryRunes armoryRunes = new ArmoryRunes();
		armoryRunes.setID(sfsob.GetInt("id"));
		int[] intArray = sfsob.GetIntArray("run0");
		for (int i = 0; i < intArray.Length; i++)
		{
			RuneRef runeRef = RuneBook.Lookup(intArray[i]);
			if (runeRef != null)
			{
				armoryRunes.setRuneSlot(runeRef, i);
			}
		}
		if (sfsob.ContainsKey("run1"))
		{
			ISFSArray sFSArray = sfsob.GetSFSArray("run1");
			for (int j = 0; j < sFSArray.Size(); j++)
			{
				ISFSObject sFSObject = sFSArray.GetSFSObject(j);
				int[] intArray2 = sFSObject.GetIntArray("run3");
				int @int = sFSObject.GetInt("run2");
				armoryRunes.setRuneSlotMemory(RuneBook.LookupList(intArray2), @int);
			}
		}
		return armoryRunes;
	}
}
