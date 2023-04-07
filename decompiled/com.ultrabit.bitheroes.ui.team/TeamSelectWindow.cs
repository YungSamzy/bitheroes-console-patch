using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.teamselectlist;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.team;

public class TeamSelectWindow : WindowsMain
{
	private const string SORT_POWER = "powerCalculate";

	private const string SORT_AGILITY = "agilityCalculate";

	private const string SORT_STAMINA = "staminaCalculate";

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI nameListTxt;

	public TextMeshProUGUI sortTxt;

	public TextMeshProUGUI emptyTxt;

	public Button powerBtn;

	public Button staminaBtn;

	public Button agilityBtn;

	public TMP_InputField searchTxt;

	public Image filterDropdown;

	public TextMeshProUGUI dropdownTxt;

	public TMP_InputField searchText;

	public SpriteMask topMask;

	public SpriteMask bottomMask;

	private Button _sortButton;

	private List<TeammateData> _allowed;

	private List<TeammateData> _used;

	private TeamRules _rules;

	private TeamSelectList teamSelectList;

	private UnityAction<TeammateData> onItemSelected;

	private Transform window;

	private int _selectedCompanionType;

	private Dictionary<int, string> dropdownWording;

	private new int _sortingLayer;

	private const float SEARCH_OFFSET = 1f;

	public new int sortingLayer => _sortingLayer;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(List<TeammateData> allowed, List<TeammateData> used, TeamRules rules, UnityAction<TeammateData> onItemSelected)
	{
		_allowed = allowed;
		_used = used;
		_rules = rules;
		_selectedCompanionType = -1;
		this.onItemSelected = onItemSelected;
		topperTxt.text = Language.GetString("ui_select_teammate");
		nameListTxt.text = Language.GetString("ui_name");
		sortTxt.text = Language.GetString("ui_sort") + ":";
		searchTxt.text = "";
		CreateWindow();
		dropdownWording = new Dictionary<int, string>();
		dropdownWording.Add(_selectedCompanionType, Language.GetString("ui_all"));
		dropdownWording.Add(1, Language.GetString("ui_players"));
		dropdownWording.Add(2, ItemRef.GetItemNamePlural(6));
		dropdownTxt.text = Language.GetString("ui_all");
		emptyTxt.text = Language.GetString("ui_select_teammate_empty");
		teamSelectList = GetComponentInChildren<TeamSelectList>();
		teamSelectList.InitList(OnItemSelected, this);
		UpdateTiles();
		ListenForBack(OnClose);
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		_sortingLayer = layer;
		topMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		topMask.frontSortingOrder = 1 + _sortingLayer + teamSelectList.Content.childCount;
		topMask.backSortingLayerID = SortingLayer.NameToID("UI");
		topMask.backSortingOrder = _sortingLayer;
		bottomMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.frontSortingOrder = 1 + _sortingLayer + teamSelectList.Content.childCount;
		bottomMask.backSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.backSortingOrder = _sortingLayer;
		teamSelectList.Refresh();
	}

	private void UpdateTiles()
	{
		UpdateTilesCoroutine();
	}

	private void UpdateTilesCoroutine()
	{
		teamSelectList.ClearList();
		List<TeamSelectListItem> list = new List<TeamSelectListItem>();
		int num = int.MinValue;
		int num2 = int.MinValue;
		int num3 = int.MinValue;
		int num4 = int.MinValue;
		int num5 = int.MaxValue;
		int num6 = int.MaxValue;
		int num7 = int.MaxValue;
		int num8 = int.MaxValue;
		bool flag = _selectedCompanionType != -1;
		for (int i = 0; i < _allowed.Count; i++)
		{
			TeammateData teammateData = _allowed[i];
			int allowedCount = GetAllowedCount(teammateData);
			int usedCount = GetUsedCount(teammateData);
			int num9 = 0;
			for (int j = 0; j < list.Count; j++)
			{
				TeammateData teammateData2 = list[j].teammateData;
				if (teammateData2.id == teammateData.id && teammateData.type == teammateData2.type)
				{
					num9++;
				}
			}
			int num10 = num9;
			if ((!_rules.allowPlayerMultiHero && teammateData.type == 1 && teammateData.data is CharacterData characterData && IsPlayerIDUsed(characterData.playerID)) || allowedCount - num10 - usedCount <= 0 || num10 > 0 || (_selectedCompanionType != -1 && !teammateData.type.Equals(_selectedCompanionType)))
			{
				continue;
			}
			if (!searchText.text.Trim().Equals(""))
			{
				flag = true;
				if (!teammateData.nameColorless.ToLower().Contains(searchText.text.ToLower()))
				{
					continue;
				}
			}
			if (teammateData != null)
			{
				int level = teammateData.level;
				int power = GetPower(teammateData);
				int stamina = GetStamina(teammateData);
				int agility = GetAgility(teammateData);
				if (teammateData.powerCalculate != power)
				{
					teammateData.SetPowerToCalulate(power);
				}
				if (teammateData.staminaCalculate != stamina)
				{
					teammateData.SetStaminaToCalulate(stamina);
				}
				if (teammateData.agilityCalculate != agility)
				{
					teammateData.SetAgilityToCalulate(agility);
				}
				if (level > num)
				{
					num = level;
				}
				if (level < num5)
				{
					num5 = level;
				}
				if (power > num2)
				{
					num2 = power;
				}
				if (power < num6)
				{
					num6 = power;
				}
				if (stamina > num3)
				{
					num3 = stamina;
				}
				if (stamina < num7)
				{
					num7 = stamina;
				}
				if (agility > num4)
				{
					num4 = agility;
				}
				if (agility < num8)
				{
					num8 = agility;
				}
			}
			list.Add(new TeamSelectListItem
			{
				teammateData = teammateData
			});
		}
		teamSelectList.SetReferenceStats(num, num5, num2, num6, num3, num7, num4, num8);
		teamSelectList.Data.InsertItems(0, list);
		emptyTxt.gameObject.SetActive(teamSelectList.Data.Count == 0);
		emptyTxt.text = (flag ? Language.GetString("ui_select_teammate_empty_filter") : Language.GetString("ui_select_teammate_empty"));
	}

	private int GetCount(TeammateData teammateData, List<TeammateData> list)
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			_ = list[i];
			if (teammateData.id == teammateData.id && teammateData.type == teammateData.type)
			{
				num++;
			}
		}
		return num;
	}

	public int GetTileCount(TeammateData teammateData)
	{
		int num = 0;
		for (int i = 0; i < teamSelectList.Data.Count; i++)
		{
			TeammateData teammateData2 = teamSelectList.Data[i].teammateData;
			if (teammateData2.id == teammateData.id && teammateData.type == teammateData2.type)
			{
				num++;
			}
		}
		return num;
	}

	public int GetAllowedCount(TeammateData teammateData)
	{
		int num = 0;
		foreach (TeammateData item in _allowed)
		{
			if (item.id == teammateData.id && item.type == teammateData.type)
			{
				num++;
			}
		}
		return num;
	}

	public int GetUsedCount(TeammateData teammateData)
	{
		int num = 0;
		foreach (TeammateData item in _used)
		{
			if (item != null && teammateData != null && item.id == teammateData.id && item.type == teammateData.type)
			{
				num++;
			}
		}
		return num;
	}

	public bool IsPlayerIDUsed(int playerID)
	{
		foreach (TeammateData item in _used)
		{
			if (item != null && item.type == 1 && item.data is CharacterData characterData && characterData.playerID == playerID)
			{
				return true;
			}
		}
		return false;
	}

	public void OnItemSelected(TeammateData teammateData)
	{
		onItemSelected(teammateData);
		base.OnClose();
	}

	public void OnValueChanged()
	{
		CancelInvoke("DoSearch");
		Invoke("DoSearch", Util.SEARCHBOX_ACTION_DELAY);
	}

	private void DoSearch()
	{
		UpdateTiles();
	}

	public void OnPowerBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SortTiles(powerBtn, "powerCalculate");
	}

	public void OnStaminaBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SortTiles(staminaBtn, "staminaCalculate");
	}

	public void OnAgilityBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SortTiles(agilityBtn, "agilityCalculate");
	}

	public void OnFilterDropdown()
	{
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_filter"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedCompanionType, OnFilterDropdownClick);
		componentInChildren.ClearList();
		foreach (KeyValuePair<int, string> item in dropdownWording)
		{
			string @string = Language.GetString(item.Value);
			int key = item.Key;
			componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
			{
				id = key,
				title = @string
			});
		}
	}

	private void OnFilterDropdownClick(MyDropdownItemModel clickedOption)
	{
		_selectedCompanionType = clickedOption.id;
		dropdownTxt.text = clickedOption.title;
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
		UpdateTiles();
	}

	private void SortTiles(Button button, string orderBy)
	{
		if (_sortButton != null)
		{
			_sortButton.GetComponent<TeamSortBtn>().Unclicked();
		}
		_sortButton = button;
		if (_sortButton != null)
		{
			_sortButton.GetComponent<TeamSortBtn>().Clicked();
		}
		_allowed = Util.SortVector(_allowed, new string[1] { orderBy }, Util.ARRAY_DESCENDING);
		UpdateTiles();
	}

	public int GetPower(TeammateData data)
	{
		return GetStats(data).power;
	}

	public int GetStamina(TeammateData data)
	{
		return GetStats(data).stamina;
	}

	public int GetAgility(TeammateData data)
	{
		return GetStats(data).agility;
	}

	public int GetTotal(TeammateData data)
	{
		return GetPower(data) + GetStamina(data) + GetAgility(data);
	}

	private CharacterStats GetStats(TeammateData data)
	{
		switch (data.type)
		{
		case 1:
			if (_rules.statBalance)
			{
				return GameData.instance.PROJECT.character.getStatBalance(data.power, data.stamina, data.agility);
			}
			break;
		case 2:
		{
			FamiliarRef familiarRef = FamiliarBook.Lookup(data.id);
			float familiarMult = _rules.getFamiliarMult(familiarRef);
			float num = (float)data.power * familiarMult;
			float num2 = (float)data.stamina * familiarMult;
			float num3 = (float)data.agility * familiarMult;
			return new CharacterStats((int)num, (int)num2, (int)num3);
		}
		}
		return new CharacterStats(data.power, data.stamina, data.agility);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		powerBtn.interactable = true;
		staminaBtn.interactable = true;
		agilityBtn.interactable = true;
		searchTxt.interactable = true;
		filterDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		powerBtn.interactable = false;
		staminaBtn.interactable = false;
		agilityBtn.interactable = false;
		searchTxt.interactable = false;
		filterDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
