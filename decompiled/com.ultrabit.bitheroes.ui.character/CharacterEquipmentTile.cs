using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterEquipmentTile : MonoBehaviour
{
	private int _slot;

	private bool _editable;

	private ItemIcon _itemIcon;

	private Button button;

	private CharacterWindow _characterWindow;

	public int slot => _slot;

	public ItemRef itemRef => _itemIcon.itemRef;

	public ItemIcon itemIcon => _itemIcon;

	public CharacterWindow characterWindow
	{
		get
		{
			return _characterWindow;
		}
		set
		{
			_characterWindow = value;
		}
	}

	public void Create(int slot)
	{
		_slot = slot;
		button = GetComponent<Button>();
		button.onClick.AddListener(OnButtonClick);
	}

	public void Create(int slot, EquipmentRef equipmentRef, EquipmentRef cosmeticRef, Equipment equipment, bool editable = false)
	{
		_slot = slot;
		_editable = editable;
		button = GetComponent<Button>();
		button.onClick.AddListener(OnButtonClick);
		if (_editable)
		{
			SetEquipmentItems(equipmentRef, cosmeticRef, equipment);
		}
	}

	public void OnButtonClick()
	{
		GameData.instance.windowGenerator.NewEquipmentWindow(_slot, _characterWindow.gameObject);
	}

	public void SetCharacterData(CharacterData characterData)
	{
	}

	public void SetEquipmentItems(EquipmentRef equipmentRef, EquipmentRef cosmeticRef, Equipment equipment)
	{
		if (equipmentRef == null)
		{
			int slotEquipmentType = Equipment.getSlotEquipmentType(_slot);
			foreach (ItemData item in GameData.instance.PROJECT.character.inventory.GetItemsByType(1, slotEquipmentType))
			{
				_ = item;
				if (_editable)
				{
					break;
				}
			}
		}
		if (equipmentRef == null || equipmentRef == cosmeticRef)
		{
			cosmeticRef = null;
		}
		if (equipmentRef != null && cosmeticRef != null)
		{
			if (!equipmentRef.subtypesMatch(cosmeticRef))
			{
				cosmeticRef = null;
			}
			else if (equipmentRef.isRelated(cosmeticRef))
			{
				cosmeticRef = null;
			}
		}
		_ = cosmeticRef != null;
	}

	private void OnDestroy()
	{
		if (button != null)
		{
			button.onClick.RemoveListener(OnButtonClick);
		}
	}
}
