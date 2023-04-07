using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.brawl;

public class BrawlCreateDifficultyWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI tierNameTxt;

	public TextMeshProUGUI difficultyNameTxt;

	public TextMeshProUGUI sealsTxt;

	public Button createBtn;

	public Button chestBtn;

	public Toggle privateCheckBox;

	public Image tierDropdown;

	public Image difficultyDropdown;

	private BrawlRef _brawlRef;

	private Transform tierDropWindow;

	private Transform difficultyDropWindow;

	private BrawlTierRef _selectedBrawlTierRef;

	private BrawlTierDifficultyRef _selectedBrawlTierDifficultyRef;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(BrawlRef brawlRef)
	{
		_brawlRef = brawlRef;
		topperTxt.text = Language.GetString("ui_summon");
		tierNameTxt.text = Language.GetString("ui_tier");
		difficultyNameTxt.text = Language.GetString("ui_difficulty");
		createBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_summon");
		privateCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_private");
		privateCheckBox.SetIsOnWithoutNotify(!GameData.instance.SAVE_STATE.GetBrawlPublicSelected(GameData.instance.PROJECT.character.id));
		foreach (BrawlTierRef tier in _brawlRef.tiers)
		{
			if (tier != null && tier.requirementsMet() && tier.id == GameData.instance.SAVE_STATE.GetBrawlTierSelected(GameData.instance.PROJECT.character.id, _brawlRef.id))
			{
				_selectedBrawlTierRef = tier;
			}
		}
		if (_selectedBrawlTierRef == null)
		{
			foreach (BrawlTierRef tier2 in _brawlRef.tiers)
			{
				if (tier2 != null && tier2.requirementsMet())
				{
					_selectedBrawlTierRef = tier2;
					GameData.instance.SAVE_STATE.SetBrawlTierSelected(GameData.instance.PROJECT.character.id, _brawlRef.id, _selectedBrawlTierRef.id);
					break;
				}
			}
		}
		tierDropdown.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_tier_count", new string[1] { _selectedBrawlTierRef.id.ToString() });
		int brawlDifficultySelected = GameData.instance.SAVE_STATE.GetBrawlDifficultySelected(GameData.instance.PROJECT.character.id, _brawlRef.id);
		foreach (BrawlTierDifficultyRef difficulty in _selectedBrawlTierRef.difficulties)
		{
			if (brawlDifficultySelected == difficulty.id)
			{
				_selectedBrawlTierDifficultyRef = difficulty;
			}
		}
		if (_selectedBrawlTierDifficultyRef == null)
		{
			foreach (BrawlTierDifficultyRef difficulty2 in _selectedBrawlTierRef.difficulties)
			{
				if (difficulty2 != null)
				{
					_selectedBrawlTierDifficultyRef = difficulty2;
					break;
				}
			}
		}
		difficultyDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _selectedBrawlTierDifficultyRef.difficultyRef.coloredName;
		sealsTxt.text = Util.NumberFormat(_selectedBrawlTierDifficultyRef.difficultyRef.seals);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnCreateBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoCreate();
	}

	public void OnChestBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		BrawlTierDifficultyRef selectedBrawlTierDifficultyRef = _selectedBrawlTierDifficultyRef;
		BrawlTierRef selectedBrawlTierRef = _selectedBrawlTierRef;
		GameData.instance.PROJECT.ShowPossibleDungeonLoot(9, _brawlRef.id, selectedBrawlTierRef.id, "", selectedBrawlTierDifficultyRef.id);
	}

	public void OnTierDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		tierDropWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_tier"));
		DropdownList componentInChildren = tierDropWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedBrawlTierRef.id, OnBrawlTierChange);
		foreach (BrawlTierRef tier in _brawlRef.tiers)
		{
			if (tier != null && tier.requirementsMet())
			{
				string @string = Language.GetString("ui_tier_count", new string[1] { tier.id.ToString() });
				componentInChildren.Data.InsertOneAtStart(new MyDropdownItemModel
				{
					id = tier.id,
					title = @string,
					btnHelp = false,
					data = tier
				});
			}
		}
	}

	public void OnBrawlTierChange(MyDropdownItemModel model)
	{
		BrawlTierRef brawlTierRef = ((model.data == null) ? null : (model.data as BrawlTierRef));
		if (brawlTierRef != null)
		{
			GameData.instance.SAVE_STATE.SetBrawlTierSelected(GameData.instance.PROJECT.character.id, _brawlRef.id, (model.data as BrawlTierRef).id);
			tierDropdown.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_tier_count", new string[1] { brawlTierRef.id.ToString() });
			_selectedBrawlTierRef = brawlTierRef;
			D.Log($"brawlTierRef != null {brawlTierRef.id}");
		}
		else
		{
			foreach (BrawlTierRef tier in _brawlRef.tiers)
			{
				if (tier != null && tier.requirementsMet())
				{
					tierDropdown.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_tier_count", new string[1] { tier.id.ToString() });
					_selectedBrawlTierRef = tier;
					D.Log($"brawlTierRef == null {_selectedBrawlTierRef.id}");
					break;
				}
			}
		}
		int brawlDifficultySelected = GameData.instance.SAVE_STATE.GetBrawlDifficultySelected(GameData.instance.PROJECT.character.id, _selectedBrawlTierRef.brawlRef.id);
		_selectedBrawlTierDifficultyRef = null;
		foreach (BrawlTierDifficultyRef difficulty in _selectedBrawlTierRef.difficulties)
		{
			if (brawlDifficultySelected == difficulty.id)
			{
				_selectedBrawlTierDifficultyRef = difficulty;
			}
		}
		if (_selectedBrawlTierDifficultyRef == null)
		{
			foreach (BrawlTierDifficultyRef difficulty2 in _selectedBrawlTierRef.difficulties)
			{
				if (difficulty2 != null)
				{
					_selectedBrawlTierDifficultyRef = difficulty2;
					GameData.instance.SAVE_STATE.SetBrawlDifficultySelected(GameData.instance.PROJECT.character.id, _selectedBrawlTierRef.brawlRef.id, _selectedBrawlTierDifficultyRef.id);
					break;
				}
			}
		}
		D.Log($"_selectedBrawlTierDifficultyRef -> {_selectedBrawlTierDifficultyRef.tierRef.id}");
		difficultyDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _selectedBrawlTierDifficultyRef.difficultyRef.coloredName;
		sealsTxt.text = Util.NumberFormat(_selectedBrawlTierDifficultyRef.difficultyRef.seals);
		if (tierDropWindow != null)
		{
			tierDropWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public void OnDifficultyDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		difficultyDropWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_difficulty"));
		DropdownList componentInChildren = difficultyDropWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedBrawlTierDifficultyRef.id, OnBrawlDifficultyChange);
		foreach (BrawlTierDifficultyRef difficulty in _selectedBrawlTierRef.difficulties)
		{
			componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
			{
				id = difficulty.id,
				title = difficulty.difficultyRef.coloredName,
				btnHelp = (difficulty.difficultyRef.modifiers != null && difficulty.difficultyRef.modifiers.Count > 0),
				data = difficulty
			});
		}
	}

	private void OnBrawlDifficultyChange(MyDropdownItemModel model)
	{
		BrawlTierDifficultyRef brawlTierDifficultyRef = ((model.data == null) ? null : (model.data as BrawlTierDifficultyRef));
		if (brawlTierDifficultyRef != null)
		{
			GameData.instance.SAVE_STATE.SetBrawlDifficultySelected(GameData.instance.PROJECT.character.id, _brawlRef.id, brawlTierDifficultyRef.id);
			difficultyDropdown.GetComponentInChildren<TextMeshProUGUI>().text = brawlTierDifficultyRef.difficultyRef.coloredName;
			_selectedBrawlTierDifficultyRef = brawlTierDifficultyRef;
		}
		else
		{
			foreach (BrawlTierDifficultyRef difficulty in _selectedBrawlTierRef.difficulties)
			{
				if (difficulty != null)
				{
					_selectedBrawlTierDifficultyRef = difficulty;
					break;
				}
			}
			difficultyDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _selectedBrawlTierDifficultyRef.difficultyRef.coloredName;
		}
		sealsTxt.text = Util.NumberFormat(_selectedBrawlTierDifficultyRef.difficultyRef.seals);
		if (difficultyDropWindow != null)
		{
			difficultyDropWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public void OnPublicCheckBox()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.SAVE_STATE.SetBrawlPublicSelected(GameData.instance.PROJECT.character.id, !privateCheckBox.isOn);
	}

	public BrawlRules GetRules()
	{
		return new BrawlRules(!privateCheckBox.isOn);
	}

	public void DoCreate()
	{
		GameData.instance.main.ShowLoading();
		BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(0), OnCreate);
		D.Log($"_selectedBrawlTierDifficultyRef -> {_selectedBrawlTierDifficultyRef.tierRef.id}");
		BrawlDALC.instance.doCreate(_selectedBrawlTierDifficultyRef, GetRules());
	}

	private void OnCreate(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(0), OnCreate);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		BrawlRoom brawlRoom = BrawlRoom.fromSFSObject(sfsob);
		GameData.instance.windowGenerator.ClearAllWindows(null, removeChat: false);
		GameData.instance.windowGenerator.NewBrawlRoomWindow(brawlRoom);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (createBtn != null)
		{
			createBtn.interactable = true;
		}
		if (privateCheckBox != null)
		{
			privateCheckBox.interactable = true;
		}
		if (chestBtn != null)
		{
			chestBtn.interactable = true;
		}
		if (tierDropdown != null && tierDropdown.gameObject != null)
		{
			tierDropdown.GetComponent<EventTrigger>().enabled = true;
		}
		if (difficultyDropdown != null && difficultyDropdown.gameObject != null)
		{
			difficultyDropdown.GetComponent<EventTrigger>().enabled = true;
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		createBtn.interactable = false;
		privateCheckBox.interactable = false;
		chestBtn.interactable = false;
		tierDropdown.GetComponent<EventTrigger>().enabled = false;
		difficultyDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
