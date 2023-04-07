using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.familiar;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.augment;

public class AugmentOptionsWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button viewBtn;

	public Button changeBtn;

	public Button removeBtn;

	private FamiliarRef _familiarRef;

	private int _slot;

	private AugmentTile _tile;

	public FamiliarRef familiarRef => _familiarRef;

	public int slot => _slot;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(FamiliarRef familiarRef, int slot, AugmentData augmentData)
	{
		_familiarRef = familiarRef;
		_slot = slot;
		FamiliarWindow familiarWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(FamiliarWindow)) as FamiliarWindow;
		if (familiarWindow != null)
		{
			_tile = familiarWindow.augmentsPanel.GetAugmentSlot(slot);
		}
		topperTxt.text = Language.GetString("ui_options");
		viewBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_view");
		changeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_switch");
		removeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_remove");
		GameData.instance.PROJECT.character.augments.OnChange.AddListener(OnAugmentsChange);
		GameData.instance.PROJECT.character.AddListener("AUGMENTS_CHANGE", OnAugmentsChange);
		UpdateButtons();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnViewBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_tile.ShowAugment(this);
	}

	public void OnChangeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_tile.ShowChangeableAugments(base.gameObject);
	}

	public void OnRemoveBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_tile.DoAugmentEquip(null);
	}

	private void OnAugmentsChange()
	{
		UpdateButtons();
	}

	private void UpdateButtons()
	{
		if (GameData.instance.PROJECT.character.augments.getFamiliarAugmentSlot(_familiarRef, _slot) == null)
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

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.augments.OnChange.RemoveListener(OnAugmentsChange);
		GameData.instance.PROJECT.character.RemoveListener("AUGMENTS_CHANGE", OnAugmentsChange);
		base.DoDestroy();
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
