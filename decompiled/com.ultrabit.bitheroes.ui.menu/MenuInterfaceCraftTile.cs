using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.craft;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceCraftTile : MainUIButton
{
	public Image craftNotification;

	public override void Create()
	{
		LoadDetails(Language.GetString("ui_smelter"), VariableBook.GetGameRequirement(2));
		UpdateText();
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.windowGenerator.NewCraftWindow(this);
	}

	public void UpdateText()
	{
		int num = CraftTradePanel.availableCraftRecipes.Count - GameData.instance.SAVE_STATE.GetSeenRecipes(GameData.instance.PROJECT.character.id).Count;
		bool active = num > 0 && GameData.instance.SAVE_STATE.notificationsCraft && !GameData.instance.SAVE_STATE.notificationsDisabled;
		craftNotification.gameObject.SetActive(active);
		craftNotification.GetComponentInChildren<TextMeshProUGUI>().text = num.ToString();
	}
}
