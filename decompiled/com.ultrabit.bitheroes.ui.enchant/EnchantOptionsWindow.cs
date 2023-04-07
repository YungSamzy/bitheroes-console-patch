using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.enchant;

public class EnchantOptionsWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button viewBtn;

	public Button changeBtn;

	public Button removeBtn;

	private int _slot;

	public override void Start()
	{
		Disable();
	}

	public void LoadDetails(int slot)
	{
		_slot = slot;
		topperTxt.text = Language.GetString("ui_options");
		viewBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_view");
		changeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_switch");
		removeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_remove");
		UpdateButtons();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnViewBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEnchantWindow(GameData.instance.PROJECT.character.enchants.getSlot(_slot));
	}

	public void OnChangeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.enchantsWindow.ShowEnchantSelectWindow(_slot);
		base.OnClose();
	}

	public void OnRemoveBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.enchantsWindow.DoEnchantEquip(_slot, null);
		base.OnClose();
	}

	private void UpdateButtons()
	{
		if (GameData.instance.PROJECT.character.enchants.getSlot(_slot) == null)
		{
			Util.SetButton(viewBtn, enabled: false);
			Util.SetButton(removeBtn, enabled: false);
		}
		else
		{
			Util.SetButton(viewBtn);
			Util.SetButton(removeBtn);
		}
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		viewBtn.interactable = true;
		changeBtn.interactable = true;
		removeBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		viewBtn.interactable = false;
		changeBtn.interactable = false;
		removeBtn.interactable = false;
	}
}
