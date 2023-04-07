using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.npc;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.encounter;

[DebuggerDisplay("{link} (EncounterRef)")]
public class EncounterRef : BaseRef, IEquatable<EncounterRef>, IComparable<EncounterRef>
{
	private string _asset;

	private string _definition;

	private string _dialogStart;

	private string _dialogVictory;

	private string _dialogDefeat;

	private List<NPCRef> _npcs;

	public new string link => _link;

	public string assetURL => _asset;

	public List<NPCRef> npcs => _npcs;

	public EncounterRef(int id, EncounterBookData.Encounter encounterData)
		: base(id)
	{
		_asset = ((encounterData.asset != null) ? encounterData.asset : null);
		_definition = ((encounterData.definition != null) ? encounterData.definition : null);
		_dialogStart = ((encounterData.dialogStart != null) ? encounterData.dialogStart : null);
		_dialogVictory = ((encounterData.dialogVictory != null) ? encounterData.dialogVictory : null);
		_dialogDefeat = ((encounterData.dialogDefeat != null) ? encounterData.dialogDefeat : null);
		if (encounterData.npc != null)
		{
			_npcs = new List<NPCRef>();
			foreach (NPCBookData.NPC item in encounterData.npc)
			{
				_npcs.Add(NPCBook.LookupLink(item.link));
			}
		}
		base.LoadDetails(encounterData);
	}

	public void loadAssets()
	{
		foreach (NPCRef npc in _npcs)
		{
			npc.displayRef.loadAssets();
		}
		_ = _asset;
	}

	public DialogRef getDialogStart()
	{
		if (_dialogStart == null)
		{
			return null;
		}
		return DialogBook.Lookup(_dialogStart);
	}

	public DialogRef getDialogVictory()
	{
		if (_dialogVictory == null)
		{
			return null;
		}
		return DialogBook.Lookup(_dialogVictory);
	}

	public DialogRef getDialogDefeat()
	{
		if (_dialogDefeat == null)
		{
			return null;
		}
		return DialogBook.Lookup(_dialogDefeat);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(EncounterRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(EncounterRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
