using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.tutorial;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceAutoPilotTile : MainUIButton
{
	public GameObject enabledMC;

	public GameObject disabledMC;

	public override void Create()
	{
		LoadDetails(Language.GetString("ui_auto_pilot"), VariableBook.GetGameRequirement(10));
		GameData.instance.PROJECT.character.AddListener("AUTO_PILOT_CHANGE", OnChange);
		DoUpdate();
	}

	private void OnChange()
	{
		DoUpdate();
	}

	public override void DoUpdate()
	{
		base.DoUpdate();
		enabledMC.SetActive(GameData.instance.PROJECT.character.autoPilot);
		disabledMC.SetActive(!enabledMC.activeSelf);
		if (enabledMC.activeSelf)
		{
			enabledMC.GetComponentInChildren<Animator>().Rebind();
		}
		UpdateGrayscale();
	}

	public void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && base.available && !GameData.instance.PROJECT.character.autoPilot && !GameData.instance.PROJECT.character.tutorial.GetState(23))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(23);
			GameData.instance.tutorialManager.ShowTutorialForButton(base.gameObject, new TutorialPopUpSettings(Tutorial.GetText(23), 2, base.gameObject), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: true, AfterTutorial);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	private void AfterTutorial(object e)
	{
		if (GameData.instance.PROJECT.dungeon == null)
		{
			GameData.instance.windowGenerator.HideBattleUI();
		}
	}

	public override void DoClick()
	{
		if (!GameData.instance.PROJECT.gameIsPaused)
		{
			base.DoClick();
			GameData.instance.PROJECT.character.autoPilot = !GameData.instance.PROJECT.character.autoPilot;
			CharacterDALC.instance.doSaveConfig(GameData.instance.PROJECT.character);
			if (GameData.instance.PROJECT.character.autoPilot)
			{
				GameData.instance.PROJECT.ValidateTutorialState(23);
			}
		}
	}

	public void ForceClick()
	{
		base.DoClick();
		GameData.instance.PROJECT.character.autoPilot = !GameData.instance.PROJECT.character.autoPilot;
		CharacterDALC.instance.doSaveConfig(GameData.instance.PROJECT.character);
	}

	private new void OnDestroy()
	{
		if (GameData.instance.PROJECT.character != null)
		{
			GameData.instance.PROJECT.character.RemoveListener("AUTO_PILOT_CHANGE", OnChange);
		}
	}
}
