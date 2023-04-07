using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.game;

public class GameModifierBtn : MonoBehaviour
{
	public void SetText(string text)
	{
		GetComponentInChildren<TextMeshProUGUI>().text = text;
	}

	public void OnClicked()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_bonus"), GetComponentInChildren<TextMeshProUGUI>().text);
	}
}
