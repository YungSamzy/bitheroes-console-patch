using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.ui.character;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemListWindowEquipment : ItemListWindow
{
	public CharacterEquipmentPanel _equipmentPanel;

	private int _prevPower;

	private int _prevStamina;

	private int _prevAgility;

	public override void LoadDetails(List<ItemData> items, bool compare = true, bool added = false, string name = null, bool select = false, string helpText = null, string closeWord = null, bool forceItemEnabled = false)
	{
		base.LoadDetails(items, compare, added, name, select, helpText, closeWord, forceItemEnabled);
		_equipmentPanel.LoadDetails(GameData.instance.PROJECT.character.toCharacterData(), editable: true);
		GameData.instance.PROJECT.character.AddListener("STATS_CHANGE", OnStatChange);
		GameData.instance.PROJECT.character.equipment.OnChange.AddListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.equipment.BeforeChange.AddListener(OnEquipmentBeforeChange);
		UpdateStats();
	}

	private void OnEquipmentBeforeChange()
	{
		_prevPower = GameData.instance.PROJECT.character.getTotalPower();
		_prevStamina = GameData.instance.PROJECT.character.getTotalStamina();
		_prevAgility = GameData.instance.PROJECT.character.getTotalAgility();
	}

	private void OnEquipmentChange()
	{
		_equipmentPanel.SetCharacterData(GameData.instance.PROJECT.character.toCharacterData());
		_equipmentPanel.SetNewStats(0, _prevPower);
		_equipmentPanel.SetNewStats(1, _prevStamina);
		_equipmentPanel.SetNewStats(2, _prevAgility);
	}

	public void UpdateStats()
	{
		_equipmentPanel.SetStats(GameData.instance.PROJECT.character.getTotalPower(), GameData.instance.PROJECT.character.getTotalStamina(), GameData.instance.PROJECT.character.getTotalAgility());
	}

	private void OnStatChange()
	{
		UpdateStats();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		_equipmentPanel.UpdateLayer();
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("STATS_CHANGE", OnStatChange);
		GameData.instance.PROJECT.character.equipment.OnChange.RemoveListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.equipment.BeforeChange.RemoveListener(OnEquipmentBeforeChange);
		base.DoDestroy();
	}
}
