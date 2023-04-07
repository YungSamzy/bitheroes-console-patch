using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.invasion;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.gamemodifierlist;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.ui.invasion;

public class InvasionGameModifierListWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public TextMeshProUGUI dropTxt;

	public TextMeshProUGUI bonusesLabel;

	public GameModifierList modifiers;

	private Transform levelWindow;

	private List<InvasionEventLevelRef> _levels;

	private long _points;

	private List<MyDropdownItemModel> _levelsObjs = new List<MyDropdownItemModel>();

	private MyDropdownItemModel _selectedLevel;

	public override void Start()
	{
		base.Start();
	}

	public void LoadDetails(List<InvasionEventLevelRef> levels, long points)
	{
		Disable();
		_levels = levels;
		_points = points;
		topperTxt.text = Language.GetString("ui_bonuses");
		bonusesLabel.text = Language.GetString("ui_bonuses") + ":";
		descTxt.text = Language.GetString("invasion_segmented_community_points");
		ListenForBack(OnClose);
		CreateDropdown();
		UpdateTiers();
		CreateWindow();
	}

	private void UpdateTiers()
	{
		if (!modifiers.IsInitialized)
		{
			modifiers.InitList();
		}
		modifiers.ClearList();
		dropTxt.text = _selectedLevel.title;
		foreach (GameModifier modifier in (_selectedLevel.data as InvasionEventLevelRef).modifiers)
		{
			modifiers.Data.InsertOneAtEnd(new GameModifierItem
			{
				description = modifier.GetTileDesc()
			});
		}
	}

	private void CreateDropdown()
	{
		if (_levels.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < _levels.Count; i++)
		{
			if (_levels[i] != null)
			{
				MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel();
				myDropdownItemModel.id = _levels[i].id;
				myDropdownItemModel.title = Language.GetString("ui_level_count", new string[1] { (_levels[i].id - 1).ToString() });
				myDropdownItemModel.data = _levels[i];
				MyDropdownItemModel myDropdownItemModel2 = myDropdownItemModel;
				if (_points >= _levels[i].points)
				{
					_selectedLevel = myDropdownItemModel2;
				}
				_levelsObjs.Add(myDropdownItemModel2);
			}
		}
		if (_points == _levels[0].points)
		{
			_selectedLevel = _levelsObjs[0];
		}
	}

	public void OnLevelDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		levelWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_level"));
		DropdownList componentInChildren = levelWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedLevel.id, OnLevelDropdownClick);
		componentInChildren.ClearList();
		componentInChildren.Data.InsertItemsAtEnd(_levelsObjs);
	}

	public void OnLevelDropdownClick(MyDropdownItemModel model)
	{
		_selectedLevel = model;
		UpdateTiers();
		if (levelWindow != null)
		{
			levelWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		dropTxt.GetComponentInParent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		dropTxt.GetComponentInParent<EventTrigger>().enabled = false;
	}
}
