using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.battle;

public class BattleTestWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI difficultyTitleTxt;

	public TextMeshProUGUI battlesTitleTxt;

	public Button gauntletBtn;

	public TMP_InputField difficultyTxt;

	public TMP_InputField battlesTxt;

	public override void Start()
	{
		base.Start();
		Disable();
		difficultyTxt.contentType = TMP_InputField.ContentType.IntegerNumber;
		battlesTxt.contentType = TMP_InputField.ContentType.IntegerNumber;
		difficultyTxt.characterLimit = 4;
		battlesTxt.characterLimit = 2;
		difficultyTxt.text = Util.roundToNearest(5f, (float)GameData.instance.PROJECT.character.getTotalStats() * 0.05f).ToString();
		battlesTxt.text = "10";
		gauntletBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Gauntlet";
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnGauntletBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		gauntletBtn.interactable = true;
		difficultyTxt.interactable = true;
		battlesTxt.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		gauntletBtn.interactable = false;
		difficultyTxt.interactable = false;
		battlesTxt.interactable = false;
	}
}
