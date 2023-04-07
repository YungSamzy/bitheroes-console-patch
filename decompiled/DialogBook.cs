using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.xml.common;
using UnityEngine.Events;

public class DialogBook
{
	public enum AdviceLink
	{
		none,
		advice_1,
		advice_2,
		advice_3,
		advice_4,
		advice_5,
		advice_6,
		advice_7,
		advice_8,
		advice_9,
		advice_10,
		advice_11,
		advice_12,
		advice_13,
		advice_14,
		advice_15,
		advice_16,
		advice_17
	}

	private static Dictionary<string, DialogRef> _dialogs;

	private static Dictionary<string, AssetDisplayRef> _assetsFromXML;

	public static List<DialogRef> dialog_list => new List<DialogRef>(_dialogs.Values);

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_dialogs = new Dictionary<string, DialogRef>();
		_assetsFromXML = new Dictionary<string, AssetDisplayRef>();
		foreach (AssetDisplayData item in XMLBook.instance.dialogBook.lstAsset)
		{
			_assetsFromXML.Add(item.link, new AssetDisplayRef(item));
		}
		foreach (DialogBookData.Dialog item2 in XMLBook.instance.dialogBook.lstDialog)
		{
			DialogRef value = new DialogRef(item2.link, item2);
			_dialogs.Add(item2.link, value);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static DialogRef Lookup(string link)
	{
		if (link != null && _dialogs.ContainsKey(link))
		{
			return _dialogs[link];
		}
		return null;
	}

	public static AssetDisplayRef GetAssetDisplayRefFromLink(string link)
	{
		if (_assetsFromXML.ContainsKey(link))
		{
			return _assetsFromXML[link];
		}
		return null;
	}
}
