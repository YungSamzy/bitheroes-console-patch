using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.item;

public abstract class ItemAdvancedFilterWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public RectTransform fillLight;

	private const float VERTICAL_MARGIN = 30f;

	public RectTransform placeholderRarities;

	public RectTransform placeholderEquipmentCheckBox;

	public RectTransform placeholderEquipment;

	public RectTransform placeholderAugments;

	public RectTransform placeholderTypes;

	private RectTransform[] placeholders;

	public CheckBoxTile checkBoxResizablePrefab;

	private AdvancedFilterSettings _settings;

	private AdvancedFilterSettings _initialSettings;

	protected int[] availableRarityFilters = new int[0];

	protected int[] availableEquipmentFilters = new int[0];

	protected int[] availableAugmentFilters = new int[0];

	protected int[] availableTypeFilters = new int[0];

	private int typeException = -1;

	private List<CheckBoxTile> _checkboxesRarity;

	private CheckBoxTile _checkBoxEquipmentType;

	private List<CheckBoxTile> _checkboxesEquipment;

	private List<CheckBoxTile> _checkboxesAugment;

	private List<CheckBoxTile> _checkboxesTypes;

	private CheckBoxTile _checkBoxMountType;

	private bool _isApplied;

	private bool _hasChanges;

	public AdvancedFilterEvent OnEventClose = new AdvancedFilterEvent();

	private AdvancedFilterSettings settings
	{
		get
		{
			Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
			foreach (CheckBoxTile item in _checkboxesRarity)
			{
				RarityRef rarityRef = item.data.objectRef as RarityRef;
				dictionary.Add(rarityRef.id, item.isChecked);
			}
			Dictionary<int, bool> dictionary2 = new Dictionary<int, bool>();
			foreach (CheckBoxTile item2 in _checkboxesEquipment)
			{
				int id = item2.data.id;
				dictionary2[id] = item2.toggle.isOn;
			}
			Dictionary<int, bool> dictionary3 = new Dictionary<int, bool>();
			foreach (CheckBoxTile item3 in _checkboxesAugment)
			{
				int id2 = item3.data.id;
				dictionary3[id2] = item3.toggle.isOn;
			}
			Dictionary<int, bool> dictionary4 = new Dictionary<int, bool>();
			foreach (CheckBoxTile checkboxesType in _checkboxesTypes)
			{
				int id3 = checkboxesType.data.id;
				dictionary4.Add(id3, checkboxesType.isChecked);
			}
			if (typeException != -1 && !dictionary4.ContainsKey(typeException))
			{
				dictionary4.Add(typeException, value: true);
			}
			for (int i = 0; i < 22; i++)
			{
				if (!dictionary4.ContainsKey(i) || (i == typeException && !dictionary4.ContainsKey(typeException)))
				{
					dictionary4.Add(i, value: false);
				}
			}
			_settings = new AdvancedFilterSettings(_settings.enabled, dictionary, dictionary2, dictionary3, dictionary4);
			return _settings;
		}
		set
		{
			_settings = value;
		}
	}

	public override void Start()
	{
		base.Start();
		Disable();
		Invoke("ResizeWindow", 0.05f);
	}

	public void LoadDetails(int selectedBaseFilter, AdvancedFilterSettings settings, UnityAction<AdvancedFilterSettings, bool> onClose, int typeException = -1)
	{
		this.settings = settings;
		this.typeException = typeException;
		OnEventClose.AddListener(onClose);
		topperTxt.text = Language.GetString("ui_filter");
		CreateCheckboxes(selectedBaseFilter);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void ResizeWindow()
	{
		placeholders = new RectTransform[5] { placeholderRarities, placeholderEquipmentCheckBox, placeholderEquipment, placeholderTypes, placeholderAugments };
		float[] array = new float[placeholders.Length];
		for (int i = 0; i < placeholders.Length; i++)
		{
			if (placeholders[i] != null)
			{
				array[i] = Mathf.Abs(placeholders[i].anchoredPosition.y) + placeholders[i].rect.height * placeholders[i].localScale.y + 5f;
			}
		}
		float num = Mathf.Max(array);
		RectTransform obj = panel.transform as RectTransform;
		obj.sizeDelta = obj.sizeDelta * Vector2.right + new Vector2(0f, num + 30f);
	}

	private void CreateCheckboxes(int baseFilter)
	{
		if (_checkboxesRarity == null)
		{
			_checkboxesRarity = new List<CheckBoxTile>();
			int[] array = availableRarityFilters;
			for (int i = 0; i < array.Length; i++)
			{
				RarityRef rarityRef = RarityBook.LookupID(array[i]);
				if (rarityRef != null)
				{
					CheckBoxTile checkBoxTile = Object.Instantiate(checkBoxResizablePrefab, placeholderRarities);
					bool isOn = baseFilter > -1 || _settings.IsRarityFilterOn(rarityRef.id);
					checkBoxTile.Create(new CheckBoxTile.CheckBoxObject(rarityRef.coloredName, rarityRef), isOn, changable: true, placeholderRarities.sizeDelta.x, null, base.gameObject);
					_checkboxesRarity.Add(checkBoxTile);
				}
			}
		}
		if (_checkboxesEquipment == null)
		{
			_checkboxesEquipment = new List<CheckBoxTile>();
			int[] array = availableEquipmentFilters;
			foreach (int num in array)
			{
				if (num >= 0 && num <= 8)
				{
					CheckBoxTile checkBoxTile2 = Object.Instantiate(checkBoxResizablePrefab, placeholderEquipment);
					bool isOn2 = ((baseFilter <= -1) ? _settings.IsEquipmentFilterOn(num) : (baseFilter == 0 || ((typeException == 1) ? (baseFilter == num) : (baseFilter == 1))));
					checkBoxTile2.Create(new CheckBoxTile.CheckBoxObject(EquipmentRef.getEquipmentTypeName(num), num), isOn2, changable: true, placeholderEquipment.sizeDelta.x, "Equipment", base.gameObject);
					_checkboxesEquipment.Add(checkBoxTile2);
				}
			}
		}
		if (_checkboxesAugment == null)
		{
			_checkboxesAugment = new List<CheckBoxTile>();
			int[] array = availableAugmentFilters;
			foreach (int num2 in array)
			{
				if (num2 >= 0 && num2 <= 4)
				{
					CheckBoxTile checkBoxTile3 = Object.Instantiate(checkBoxResizablePrefab, placeholderAugments);
					bool isOn3 = ((baseFilter <= -1) ? _settings.IsAugmentFilterOn(num2) : (baseFilter == 0 || num2 + 1 == baseFilter));
					checkBoxTile3.Create(new CheckBoxTile.CheckBoxObject(AugmentTypeRef.GetName(num2), num2), isOn3, changable: true, placeholderAugments.sizeDelta.x, null, base.gameObject);
					_checkboxesAugment.Add(checkBoxTile3);
				}
			}
		}
		if (_checkboxesTypes == null)
		{
			bool flag = false;
			_checkboxesTypes = new List<CheckBoxTile>();
			int[] array = availableTypeFilters;
			foreach (int num3 in array)
			{
				if (num3 == 0)
				{
					continue;
				}
				string text = ItemRef.GetItemName(num3);
				CheckBoxTile checkBoxTile4 = Object.Instantiate(checkBoxResizablePrefab);
				bool isOn4 = ((baseFilter <= -1) ? _settings.IsTypeFilterOn(num3) : (baseFilter == 0 || baseFilter == num3));
				if (num3 == 1)
				{
					flag = true;
					checkBoxTile4.transform.SetParent(placeholderEquipmentCheckBox, worldPositionStays: false);
					_checkBoxEquipmentType = checkBoxTile4;
					checkBoxTile4.AddOnClickedCallback(OnToggleChangeEquipmentType);
				}
				else
				{
					if (num3 == 12)
					{
						text = Language.GetString("ui_fishing");
					}
					checkBoxTile4.transform.SetParent(placeholderTypes, worldPositionStays: false);
				}
				checkBoxTile4.Create(new CheckBoxTile.CheckBoxObject(text, num3), isOn4, changable: true, placeholderTypes.sizeDelta.x, null, base.gameObject);
				_checkboxesTypes.Add(checkBoxTile4);
			}
			if (flag)
			{
				CheckBoxTile checkBoxTile5 = Object.Instantiate(checkBoxResizablePrefab, placeholderEquipment);
				bool isOn4 = ((baseFilter <= -1) ? _settings.IsTypeFilterOn(8) : (baseFilter == 0 || baseFilter == 1));
				_checkBoxMountType = checkBoxTile5;
				checkBoxTile5.Create(new CheckBoxTile.CheckBoxObject(ItemRef.GetItemName(8), 8), isOn4, changable: true, placeholderTypes.sizeDelta.x, "Equipment", base.gameObject);
				_checkboxesTypes.Add(checkBoxTile5);
			}
		}
		_initialSettings = settings;
	}

	public void OnToggleChange()
	{
	}

	public void OnToggleChangeEquipment()
	{
		bool isChecked = false;
		foreach (CheckBoxTile item in _checkboxesEquipment)
		{
			if (item.isChecked)
			{
				isChecked = true;
				break;
			}
		}
		if (_checkBoxMountType != null && _checkBoxMountType.isChecked)
		{
			isChecked = true;
		}
		if (_checkBoxEquipmentType != null)
		{
			_checkBoxEquipmentType.isChecked = isChecked;
		}
	}

	public void OnToggleChangeEquipmentType(CheckBoxTile.CheckBoxObject checkBox)
	{
		foreach (CheckBoxTile item in _checkboxesEquipment)
		{
			item.isChecked = _checkBoxEquipmentType.isChecked;
		}
		_checkBoxMountType.isChecked = _checkBoxEquipmentType.isChecked;
	}

	private void ApplyAdvancedFilters()
	{
		_settings.enabled = true;
	}

	private void OnApplyBtn()
	{
		ApplyAdvancedFilters();
		_isApplied = true;
		CustomClose();
	}

	public override void OnClose()
	{
		if (!settings.Equals(_initialSettings))
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_apply_changes"), null, null, delegate
			{
				OnApplyBtn();
			}, delegate
			{
				CustomClose();
			});
		}
		else
		{
			CustomClose();
		}
	}

	private void CustomClose()
	{
		OnEventClose?.Invoke(_settings, _isApplied);
		OnEventClose.RemoveAllListeners();
		base.OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		EnableUI(enabled: true);
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		EnableUI(enabled: false);
	}

	private void EnableUI(bool enabled)
	{
		if (_checkboxesRarity != null)
		{
			foreach (CheckBoxTile item in _checkboxesRarity)
			{
				item.toggle.enabled = enabled;
			}
		}
		if (_checkboxesEquipment != null)
		{
			foreach (CheckBoxTile item2 in _checkboxesEquipment)
			{
				item2.toggle.enabled = enabled;
			}
		}
		if (_checkboxesTypes == null)
		{
			return;
		}
		foreach (CheckBoxTile checkboxesType in _checkboxesTypes)
		{
			checkboxesType.toggle.enabled = enabled;
		}
	}
}
