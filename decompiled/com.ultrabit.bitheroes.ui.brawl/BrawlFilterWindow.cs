using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.brawl;

public class BrawlFilterWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI brawlNameTxt;

	public TextMeshProUGUI tierNameTxt;

	public TextMeshProUGUI difficultyNameTxt;

	public Image brawlDropdown;

	public Image tierDropdown;

	public Image difficultyDropdown;

	private bool _changed;

	private BrawlRef _firstBrawlRef;

	private Transform brawlDropWindow;

	private Transform tierDropWindow;

	private Transform difficultyDropWindow;

	private List<MyDropdownItemModel> brawlObjects = new List<MyDropdownItemModel>();

	private MyDropdownItemModel _selectedBrawl;

	private List<MyDropdownItemModel> tierObjects = new List<MyDropdownItemModel>();

	private MyDropdownItemModel _selectedTier;

	private List<MyDropdownItemModel> difficultyObjects = new List<MyDropdownItemModel>();

	private MyDropdownItemModel _selectedDifficulty;

	public override void Start()
	{
		base.Start();
		Disable();
		_firstBrawlRef = BrawlBook.GetFirstBrawl();
		topperTxt.text = Language.GetString("ui_filter");
		brawlNameTxt.text = Language.GetString("ui_brawl_short");
		tierNameTxt.text = Language.GetString("ui_tier");
		difficultyNameTxt.text = Language.GetString("ui_difficulty");
		CreateBrawlDropdown();
		CreateTierDropdown();
		CreateDifficultyDropdown();
		ListenForBack(OnClose);
		forceAnimation = true;
		CreateWindow();
	}

	private void CreateBrawlDropdown()
	{
		brawlObjects.Clear();
		int brawlFilter = GameData.instance.SAVE_STATE.GetBrawlFilter(GameData.instance.PROJECT.character.id);
		_selectedBrawl = new MyDropdownItemModel
		{
			id = -1,
			title = Language.GetString("ui_all")
		};
		MyDropdownItemModel selectedBrawl = _selectedBrawl;
		for (int i = 0; i <= BrawlBook.size; i++)
		{
			BrawlRef brawlRef = BrawlBook.Lookup(i);
			if (brawlRef != null && brawlRef.requirementsMet())
			{
				MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel
				{
					id = brawlRef.id,
					title = brawlRef.name,
					data = brawlRef
				};
				brawlObjects.Insert(0, myDropdownItemModel);
				if (brawlFilter == brawlRef.id)
				{
					_selectedBrawl = myDropdownItemModel;
				}
			}
		}
		brawlObjects.Insert(0, selectedBrawl);
		brawlDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _selectedBrawl.title;
	}

	public void OnBrawlDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		brawlDropWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_brawl_short"));
		DropdownList componentInChildren = brawlDropWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, GameData.instance.SAVE_STATE.GetBrawlFilter(GameData.instance.PROJECT.character.id), OnBrawlChange);
		componentInChildren.Data.InsertItemsAtEnd(brawlObjects);
	}

	private void OnBrawlChange(MyDropdownItemModel model)
	{
		_selectedBrawl = model;
		brawlDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _selectedBrawl.title;
		CreateTierDropdown();
		CreateDifficultyDropdown();
		OnDropdownChange();
		if (brawlDropWindow != null)
		{
			brawlDropWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private void CreateTierDropdown()
	{
		tierObjects.Clear();
		int brawlTierFilter = GameData.instance.SAVE_STATE.GetBrawlTierFilter(GameData.instance.PROJECT.character.id);
		List<BrawlTierRef> list = new List<BrawlTierRef>();
		BrawlRef brawlRef = GetBrawlRef();
		for (int i = 0; i <= BrawlBook.size; i++)
		{
			BrawlRef brawlRef2 = BrawlBook.Lookup(i);
			if (brawlRef2 == null || !brawlRef2.requirementsMet() || (brawlRef != null && brawlRef2 != brawlRef))
			{
				continue;
			}
			foreach (BrawlTierRef tier in brawlRef2.tiers)
			{
				if (tier == null || !tier.requirementsMet())
				{
					continue;
				}
				bool flag = true;
				foreach (BrawlTierRef item in list)
				{
					if (item.id == tier.id)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(tier);
				}
			}
		}
		_selectedTier = new MyDropdownItemModel
		{
			id = -1,
			title = Language.GetString("ui_all")
		};
		tierObjects.Add(_selectedTier);
		foreach (BrawlTierRef item2 in Util.SortVector(list, new string[1] { "id" }, Util.ARRAY_DESCENDING))
		{
			string @string = Language.GetString("ui_tier_count", new string[1] { item2.id.ToString() });
			MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel
			{
				id = item2.id,
				title = @string,
				data = item2
			};
			tierObjects.Add(myDropdownItemModel);
			if (brawlTierFilter == item2.id)
			{
				_selectedTier = myDropdownItemModel;
			}
		}
		tierDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _selectedTier.title;
	}

	public void OnTierDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		tierDropWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_tier"));
		DropdownList componentInChildren = tierDropWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, GameData.instance.SAVE_STATE.GetBrawlTierFilter(GameData.instance.PROJECT.character.id), OnBrawlTierChange);
		componentInChildren.Data.InsertItemsAtEnd(tierObjects);
	}

	private void OnBrawlTierChange(MyDropdownItemModel model)
	{
		_selectedTier = model;
		tierDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _selectedTier.title;
		CreateDifficultyDropdown();
		OnDropdownChange();
		if (tierDropWindow != null)
		{
			tierDropWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private void CreateDifficultyDropdown()
	{
		difficultyObjects.Clear();
		int brawlDifficultyFilter = GameData.instance.SAVE_STATE.GetBrawlDifficultyFilter(GameData.instance.PROJECT.character.id);
		BrawlTierRef firstTier = _firstBrawlRef.getFirstTier();
		_selectedDifficulty = new MyDropdownItemModel
		{
			id = -1,
			title = Language.GetString("ui_all")
		};
		difficultyObjects.Add(_selectedDifficulty);
		if (firstTier != null)
		{
			foreach (BrawlTierDifficultyRef difficulty in firstTier.difficulties)
			{
				MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel
				{
					id = difficulty.id,
					title = difficulty.difficultyRef.coloredName,
					data = difficulty,
					btnHelp = true
				};
				difficultyObjects.Add(myDropdownItemModel);
				if (brawlDifficultyFilter == difficulty.id)
				{
					_selectedDifficulty = myDropdownItemModel;
				}
			}
		}
		difficultyDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _selectedDifficulty.title;
	}

	public void OnDifficultyDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		difficultyDropWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_difficulty"));
		DropdownList componentInChildren = difficultyDropWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, GameData.instance.SAVE_STATE.GetBrawlDifficultyFilter(GameData.instance.PROJECT.character.id), OnBrawlDifficultyChange);
		componentInChildren.Data.InsertItemsAtEnd(difficultyObjects);
	}

	private void OnBrawlDifficultyChange(MyDropdownItemModel model)
	{
		_selectedDifficulty = model;
		difficultyDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _selectedDifficulty.title;
		OnDropdownChange();
		if (difficultyDropWindow != null)
		{
			difficultyDropWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public void OnDropdownChange()
	{
		_changed = true;
		UpdateBrawlSaved();
		UpdateTierSaved();
		UpdateDifficultySaved();
	}

	private void UpdateBrawlSaved()
	{
		int filter = GetBrawlRef()?.id ?? (-1);
		GameData.instance.SAVE_STATE.SetBrawlFilter(GameData.instance.PROJECT.character.id, filter);
	}

	private void UpdateTierSaved()
	{
		int tierFilter = GetTierRef()?.id ?? (-1);
		GameData.instance.SAVE_STATE.SetBrawlTierFilter(GameData.instance.PROJECT.character.id, tierFilter);
	}

	private void UpdateDifficultySaved()
	{
		int difficultyFilter = GetDifficultyRef()?.id ?? (-1);
		GameData.instance.SAVE_STATE.SetBrawlDifficultyFilter(GameData.instance.PROJECT.character.id, difficultyFilter);
	}

	private BrawlRef GetBrawlRef()
	{
		return _selectedBrawl.data as BrawlRef;
	}

	private BrawlTierRef GetTierRef()
	{
		return _selectedTier.data as BrawlTierRef;
	}

	private BrawlTierDifficultyRef GetDifficultyRef()
	{
		return _selectedDifficulty.data as BrawlTierDifficultyRef;
	}

	public override void OnClose()
	{
		if (_changed)
		{
			BrawlWindow brawlWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(BrawlWindow)) as BrawlWindow;
			if (brawlWindow != null)
			{
				brawlWindow.DoSearch();
			}
		}
		base.OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		brawlDropdown.GetComponent<EventTrigger>().enabled = true;
		tierDropdown.GetComponent<EventTrigger>().enabled = true;
		difficultyDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		brawlDropdown.GetComponent<EventTrigger>().enabled = false;
		tierDropdown.GetComponent<EventTrigger>().enabled = false;
		difficultyDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
