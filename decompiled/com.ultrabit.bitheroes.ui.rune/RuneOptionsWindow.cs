using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.rune;

public class RuneOptionsWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button useBtn;

	public Button changeBtn;

	public Button exchangeBtn;

	private int _slot;

	private RuneTile _tile;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(int slot, RuneTile tile)
	{
		_slot = slot;
		_tile = tile;
		topperTxt.text = Language.GetString("ui_options");
		useBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_insert");
		changeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_switch");
		exchangeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_exchange");
		if (tile.runeRef != null)
		{
			Debug.Log(_slot + tile.runeRef.name);
		}
		if (!GameData.instance.PROJECT.character.runes.canChangeSlot(_slot))
		{
			Util.SetButton(changeBtn, enabled: false);
			Util.SetButton(exchangeBtn, enabled: false);
		}
		GameData.instance.PROJECT.character.runes.getRuneSlot(_slot);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnUseBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_tile.ShowUsableRunes(this);
	}

	public void OnChangeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_tile.ShowChangeableRunes(this);
	}

	public void OnExchageBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_tile.ShowExchangeableRunes(this);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		useBtn.interactable = true;
		changeBtn.interactable = true;
		exchangeBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		useBtn.interactable = false;
		changeBtn.interactable = false;
		exchangeBtn.interactable = false;
	}
}
