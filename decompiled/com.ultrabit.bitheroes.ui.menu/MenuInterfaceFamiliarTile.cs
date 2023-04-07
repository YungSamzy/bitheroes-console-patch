using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.variable;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceFamiliarTile : MainUIButton
{
	public Image badge_bg;

	public TextMeshProUGUI badgeText;

	public Image schematicsIcon;

	public override void Create()
	{
		LoadDetails(ItemRef.GetItemNamePlural(6), VariableBook.GetGameRequirement(13));
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		UpdateText();
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.windowGenerator.NewFamiliarsWindow();
	}

	private new void OnDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
	}

	private void OnInventoryChange()
	{
		UpdateText();
	}

	public void UpdateText()
	{
		int num = 0;
		int itemType = ItemRef.getItemType(GameRequirement.GetTypeName(19));
		if (itemType > 0)
		{
			num = GameData.instance.PROJECT.character.inventory.getItemTypeQty(itemType) - GameData.instance.SAVE_STATE.GetSeenSchematics(GameData.instance.PROJECT.character.id).Count;
		}
		schematicsIcon.gameObject.SetActive(num > 0 && GameData.instance.SAVE_STATE.notificationsFusions && !GameData.instance.SAVE_STATE.notificationsDisabled);
		int num2 = 0;
		foreach (FamiliarRef completeFamiliar in FamiliarBook.GetCompleteFamiliarList())
		{
			if (GameData.instance.PROJECT.character.inventory.hasOwnedItem(completeFamiliar))
			{
				num2++;
			}
		}
		int num3 = num2 - GameData.instance.SAVE_STATE.GetSeenFamiliars(GameData.instance.PROJECT.character.id).Count;
		bool active = num3 > 0 && GameData.instance.SAVE_STATE.notificationsFamiliars && !GameData.instance.SAVE_STATE.notificationsDisabled;
		badge_bg.gameObject.SetActive(active);
		badgeText.gameObject.SetActive(active);
		badgeText.text = num3.ToString();
	}
}
