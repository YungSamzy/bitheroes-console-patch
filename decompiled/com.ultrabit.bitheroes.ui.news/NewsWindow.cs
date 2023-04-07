using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.news;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.news;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.news;

public class NewsWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Toggle showCheckBox;

	public Button notesBtn;

	public Button confirmBtn;

	public NewsList newsList;

	public Button leftBtn;

	public Button rightBtn;

	public SpriteMask[] masks;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_news");
		notesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_notes");
		confirmBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_close");
		showCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ad_disable_desc");
		showCheckBox.isOn = GameData.instance.SAVE_STATE.newsVersion == NewsBook.VERSION;
		CreateList();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		if (newsList.Data != null)
		{
			newsList.Refresh();
		}
		SpriteMask[] array = masks;
		foreach (SpriteMask obj in array)
		{
			obj.frontSortingLayerID = SortingLayer.NameToID("UI");
			obj.frontSortingOrder = base.sortingLayer + 99;
			obj.backSortingLayerID = SortingLayer.NameToID("UI");
			obj.backSortingOrder = base.sortingLayer - 1;
		}
	}

	public void OnNotesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(Language.GetString("ui_notes"), Util.ParseString(NewsBook.NOTES), base.gameObject);
	}

	public void OnConfirmBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		base.OnClose();
	}

	private void CreateList()
	{
		newsList.InitList(this);
		if (newsList.Data.Count > 0)
		{
			newsList.Data.RemoveItems(0, newsList.Data.Count);
		}
		List<NewsItemModel> list = new List<NewsItemModel>();
		for (int i = 0; i < NewsBook.size; i++)
		{
			NewsRef newsRef = NewsBook.Lookup(i);
			if (newsRef != null && newsRef.getActive())
			{
				list.Add(new NewsItemModel
				{
					newsRef = newsRef
				});
			}
		}
		newsList.AddItemsAt(0, list);
		newsList.Refresh();
	}

	public override void DoDestroy()
	{
		if (showCheckBox.isOn)
		{
			GameData.instance.SAVE_STATE.newsVersion = NewsBook.VERSION;
		}
		else
		{
			GameData.instance.SAVE_STATE.newsVersion = "";
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (showCheckBox != null)
		{
			showCheckBox.interactable = true;
		}
		if (notesBtn != null)
		{
			notesBtn.interactable = true;
		}
		if (confirmBtn != null)
		{
			confirmBtn.interactable = true;
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		if (showCheckBox != null)
		{
			showCheckBox.interactable = false;
		}
		if (notesBtn != null)
		{
			notesBtn.interactable = false;
		}
		if (confirmBtn != null)
		{
			confirmBtn.interactable = false;
		}
	}
}
