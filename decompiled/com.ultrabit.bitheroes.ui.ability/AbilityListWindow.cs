using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.lists.abilitylist;
using TMPro;

namespace com.ultrabit.bitheroes.ui.ability;

public class AbilityListWindow : WindowsMain
{
	private const int OFFSET = 69;

	public TextMeshProUGUI topperTxt;

	private List<AbilityRef> _abilities;

	private int _power;

	private float _bonus;

	public AbilityList abilityList;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(List<AbilityRef> abilities, int power, float bonus)
	{
		_abilities = (from item in abilities
			orderby item.meterCost, item.id
			select item).ToList();
		_power = power;
		_bonus = bonus;
		topperTxt.text = Language.GetString("ability_plural_name");
		CreateWindow();
		CreateAbilityList();
		ListenForBack(OnClose);
	}

	private void CreateAbilityList()
	{
		abilityList.InitList();
		abilityList.ClearList();
		foreach (AbilityRef ability in _abilities)
		{
			abilityList.Data.InsertOneAtEnd(new AbilityListModel
			{
				abilityRef = ability,
				power = _power,
				bonus = _bonus,
				clickable = false
			});
		}
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}
}
