using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.ui.heroselector;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.menu;

public class HeroSelectButton : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OpenHeroSelectWindow);
	}

	private void OpenHeroSelectWindow()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		HeroSelectWindow.OpenWindow();
	}
}
