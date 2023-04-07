using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.familiarstablelist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.familiar;

public class FamiliarStableWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public Button helpBtn;

	public Button addBtn;

	private List<FamiliarStableData> currentItems;

	public FamiliarStableList itemList;

	private FamiliarStable _familiarStable;

	private bool _changeable = true;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_stable");
		descTxt.text = Language.GetString("ui_board_familiar_stable_desc", new string[1] { ItemRef.GetItemNamePlural(6) });
		addBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_board");
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		itemList.StartList(OnSelectedFamiliar);
		if (_changeable)
		{
			GameData.instance.PROJECT.character.AddListener("FAMILIAR_STABLE_CHANGE", onFamiliarStableChange);
		}
		else
		{
			Util.SetButton(addBtn, enabled: false);
		}
		CreateTiles();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void onFamiliarStableChange()
	{
		CreateTiles();
	}

	private void CreateTiles()
	{
		GetItems();
		itemList.ClearList();
		List<FamiliarStableItemModel> list = new List<FamiliarStableItemModel>();
		foreach (FamiliarStableData currentItem in currentItems)
		{
			list.Add(new FamiliarStableItemModel
			{
				itemData = currentItem
			});
		}
		itemList.Data.InsertItems(0, list);
	}

	private void OnSelectedFamiliar(BaseModelData model)
	{
		FamiliarRef familiarRef = model.itemRef as FamiliarRef;
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_release_familiar_stable_confirm", new string[2]
		{
			familiarRef.coloredName,
			Language.GetString("ui_stable")
		}), null, null, delegate
		{
			DoFamiliarRelease(familiarRef);
		});
	}

	private void DoFamiliarRelease(FamiliarRef familiarRef)
	{
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(27), OnFamiliarRelease);
		CharacterDALC.instance.doFamiliarStableRelease(familiarRef);
	}

	private void OnFamiliarRelease(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(27), OnFamiliarRelease);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		FamiliarStable familiarStable = FamiliarStable.fromSFSObject(sfsob);
		List<ItemData> items = ItemData.listFromSFSObject(sfsob);
		_familiarStable = familiarStable;
		GameData.instance.PROJECT.character.familiarStable = familiarStable;
		GameData.instance.PROJECT.character.addItems(items);
		CreateTiles();
	}

	private void GetItems()
	{
		_familiarStable = GameData.instance.PROJECT.character.familiarStable;
		_changeable = true;
		List<FamiliarStableData> list = (from item in _familiarStable.familiars
			orderby item.rarity descending, item.id
			select item).ToList();
		int num = 0;
		currentItems = new List<FamiliarStableData>();
		foreach (FamiliarStableData item in list)
		{
			if (item.qty > 0)
			{
				currentItems.Add(item);
				num++;
			}
		}
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(Language.GetString("ui_stable"), Language.GetString("familiar_stable_help_desc"));
	}

	public void OnAddBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewFamiliarStableBoardWindow();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		addBtn.interactable = true;
		helpBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		addBtn.interactable = false;
		helpBtn.interactable = false;
	}

	public override void DoDestroy()
	{
		base.DoDestroy();
		if (_changeable)
		{
			GameData.instance.PROJECT.character.RemoveListener("FAMILIAR_STABLE_CHANGE", onFamiliarStableChange);
		}
	}
}
