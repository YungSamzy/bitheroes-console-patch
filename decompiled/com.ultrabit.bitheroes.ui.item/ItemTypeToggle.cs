using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

[RequireComponent(typeof(Toggle))]
public class ItemTypeToggle : MonoBehaviour
{
	public class FilterEvent : UnityEvent<int, int>
	{
	}

	[SerializeField]
	private int _itemType;

	private int _itemSurtype = -1;

	private TextMeshProUGUI _textBox;

	private Toggle _toggle;

	public FilterEvent onToggleOn = new FilterEvent();

	public int itemType
	{
		get
		{
			return _itemType;
		}
		set
		{
			_itemType = value;
		}
	}

	public int itemSurtype
	{
		get
		{
			return _itemSurtype;
		}
		set
		{
			_itemSurtype = value;
		}
	}

	private string languageText
	{
		get
		{
			int type = 0;
			if (itemSurtype == -1)
			{
				switch (_itemType)
				{
				case 0:
					return Language.GetString("ui_all");
				case 12:
					return Language.GetString("ui_fishing");
				case 1:
				case 2:
				case 3:
				case 4:
				case 9:
				case 11:
				case 15:
					type = _itemType;
					break;
				}
			}
			else
			{
				switch (_itemSurtype)
				{
				case 1:
					return EquipmentRef.GetEquipmentTypeNamePlural(_itemType);
				case 15:
					switch (_itemType)
					{
					case 0:
						return Language.GetString("ui_augment_a");
					case 1:
						return Language.GetString("ui_augment_b");
					case 2:
						return Language.GetString("ui_augment_c");
					case 3:
						return Language.GetString("ui_augment_d");
					}
					break;
				default:
					type = 0;
					break;
				}
			}
			return ItemRef.GetItemNamePlural(type);
		}
	}

	private void Awake()
	{
		_textBox = GetComponentInChildren<TextMeshProUGUI>();
		_toggle = GetComponent<Toggle>();
		_toggle.onValueChanged.AddListener(OnValueChange);
	}

	private void Start()
	{
		_textBox.SetText(languageText);
	}

	private void OnValueChange(bool isOn)
	{
		if (isOn)
		{
			onToggleOn.Invoke(_itemType, _itemSurtype);
		}
	}
}
