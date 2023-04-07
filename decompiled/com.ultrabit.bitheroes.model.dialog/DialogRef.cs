using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.dialog;

[DebuggerDisplay("{link} (DialogRef)")]
public class DialogRef : IEquatable<DialogRef>, IComparable<DialogRef>
{
	private string _link;

	private List<DialogFrameRef> _frames;

	public bool seen => GameData.instance.SAVE_STATE.GetDialogSeen(GameData.instance.PROJECT.character.id, _link);

	public string link => _link;

	public List<DialogFrameRef> frames => _frames;

	public DialogRef(string link, DialogBookData.Dialog dialogData)
	{
		_link = link;
		if (dialogData.lstFrame == null)
		{
			return;
		}
		_frames = new List<DialogFrameRef>();
		foreach (DialogBookData.Frame item in dialogData.lstFrame)
		{
			DialogFrameRef dialogFrameRef = new DialogFrameRef(_frames.Count, item);
			dialogFrameRef.LoadDetails(item);
			_frames.Add(dialogFrameRef);
		}
	}

	public void loadAssets()
	{
		foreach (DialogFrameRef frame in _frames)
		{
			frame.loadAssets();
		}
	}

	public DialogFrameRef getFrame(int index)
	{
		if (index < 0 || index >= _frames.Count)
		{
			return null;
		}
		return _frames[index];
	}

	public void See()
	{
		GameData.instance.SAVE_STATE.SetDialogSeen(GameData.instance.PROJECT.character.id, _link);
	}

	public bool Equals(DialogRef other)
	{
		if (other == null)
		{
			return false;
		}
		return link.Equals(other.link);
	}

	public int CompareTo(DialogRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return link.CompareTo(other.link);
	}
}
