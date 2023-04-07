using System.Collections.Generic;
using com.ultrabit.bitheroes.model.language;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

public abstract class ItemFilterWindow : WindowsMain
{
	private float noTogglesSize;

	protected int[] availableFilters = new int[0];

	protected int typeForSubtypes = -1;

	protected int[] availableSubtypeFilters = new int[0];

	protected int columns;

	protected UnityAction onAdvancedFilterBtn;

	public TextMeshProUGUI topperTxt;

	public ToggleGroup filtersContainer;

	public Button advancedFilterBtn;

	[SerializeField]
	private ItemTypeToggle _togglePrefab;

	[SerializeField]
	private RectTransform _rowPrefab;

	[SerializeField]
	private bool centerOddRows;

	private List<ItemTypeToggle> toggles = new List<ItemTypeToggle>();

	protected int filter;

	protected AdvancedFilterSettings advancedFilterSettings;

	public UnityEvent OnEventClose;

	protected virtual void Awake()
	{
		noTogglesSize = (panel.transform as RectTransform).rect.height - _rowPrefab.rect.height;
	}

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails()
	{
		topperTxt.text = Language.GetString("ui_filter");
		advancedFilterBtn.GetComponentInChildren<TextMeshProUGUI>().SetText(Language.GetString("ui_advanced"));
		LoadFilter();
		CreateToggles();
		SetToggles();
		advancedFilterBtn.onClick.AddListener(onAdvancedFilterBtn);
		ListenForBack(OnClose);
		CreateWindow();
	}

	protected virtual void LoadFilter()
	{
	}

	protected virtual void SaveFilter()
	{
	}

	private void CreateToggles()
	{
		if (advancedFilterSettings.TryConvertToBaseFilter(out var filterType, out var filterSurtype, availableFilters, availableSubtypeFilters))
		{
			filter = ((filterType != -1) ? filterType : filter);
			typeForSubtypes = ((filterSurtype != -1) ? filterSurtype : typeForSubtypes);
		}
		int num = Mathf.CeilToInt((float)(availableFilters.Length + availableSubtypeFilters.Length) / 2f);
		List<RectTransform> list = new List<RectTransform> { _rowPrefab };
		for (int i = 1; i < num; i++)
		{
			RectTransform rectTransform = Object.Instantiate(_rowPrefab, filtersContainer.transform);
			list.Add(rectTransform);
			rectTransform.anchoredPosition += new Vector2(0f, (0f - _rowPrefab.sizeDelta.y) * (float)i);
		}
		for (int j = 0; j < num; j++)
		{
			for (int k = 0; k < columns; k++)
			{
				if (j * columns + k < availableFilters.Length + availableSubtypeFilters.Length)
				{
					ItemTypeToggle itemTypeToggle = Object.Instantiate(_togglePrefab, list[j]);
					RectTransform obj = itemTypeToggle.transform as RectTransform;
					int num2 = j * columns + k;
					float num3 = (centerOddRows ? (_rowPrefab.rect.width / (float)Mathf.Min(columns, availableFilters.Length + availableSubtypeFilters.Length - j * columns)) : (_rowPrefab.rect.width / (float)columns));
					float num4 = num3 / 2f - _rowPrefab.rect.width / 2f;
					obj.anchoredPosition = new Vector2(num4 + num3 * (float)k, 0f);
					if (num2 < availableFilters.Length)
					{
						itemTypeToggle.itemType = availableFilters[num2];
					}
					else
					{
						itemTypeToggle.itemSurtype = typeForSubtypes;
						itemTypeToggle.itemType = availableSubtypeFilters[num2 - availableFilters.Length];
					}
					toggles.Add(itemTypeToggle);
					itemTypeToggle.GetComponent<Toggle>().group = filtersContainer;
					filtersContainer.RegisterToggle(itemTypeToggle.GetComponent<Toggle>());
				}
			}
		}
		RectTransform obj2 = panel.transform as RectTransform;
		obj2.sizeDelta = new Vector2(obj2.sizeDelta.x, _rowPrefab.sizeDelta.y * (float)num + noTogglesSize);
	}

	private void SetToggles()
	{
		foreach (ItemTypeToggle toggle in toggles)
		{
			Toggle component = toggle.GetComponent<Toggle>();
			toggle.onToggleOn.AddListener(OnToggleChange);
			component.SetIsOnWithoutNotify((toggle.itemSurtype == -1) ? (toggle.itemType == filter) : ((toggle.itemSurtype == 1) ? (toggle.itemSurtype == typeForSubtypes && toggle.itemType == filter) : (toggle.itemSurtype == typeForSubtypes && toggle.itemType == filter - 1)));
		}
		if (filter < 0)
		{
			filtersContainer.SetAllTogglesOff();
		}
	}

	private void OnToggleChange(int type, int surtype)
	{
		filter = ((surtype != 15) ? type : (type + 1));
		advancedFilterSettings.enabled = false;
		SaveFilter();
		OnClose();
	}

	protected void OnAdvancedFilterWindowClose(AdvancedFilterSettings settings, bool isApplied)
	{
		advancedFilterSettings = settings;
		if (isApplied)
		{
			filter = -1;
			SaveFilter();
			closeBtn.onClick.RemoveAllListeners();
			OnEventClose?.Invoke();
			DoDestroy();
		}
	}

	public override void OnClose()
	{
		SaveFilter();
		OnEventClose?.Invoke();
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
		if (filtersContainer != null)
		{
			Toggle[] componentsInChildren = filtersContainer.GetComponentsInChildren<Toggle>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = enabled;
			}
		}
		if (advancedFilterBtn != null)
		{
			advancedFilterBtn.interactable = enabled;
		}
	}
}
