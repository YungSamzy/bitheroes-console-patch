using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.battle;

public class BattleTransitionScreen : TransitionScreen
{
	public override void LoadDetails(string sceneName, string currentSceneName, UnityAction completeAction = null, UnityAction toggleAction = null, bool unloadFirst = true)
	{
		base.LoadDetails(sceneName, currentSceneName, completeAction, toggleAction, unloadFirst);
		if (!AppInfo.TESTING)
		{
			GameData.instance.audioManager.PlaySoundLink("battletransition");
		}
	}
}
