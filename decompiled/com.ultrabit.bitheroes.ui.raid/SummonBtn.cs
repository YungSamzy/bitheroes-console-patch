using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.raid;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.raid;

public class SummonBtn : MonoBehaviour
{
	public int id;

	public RaidWindow raidWindow;

	public void Click()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DialogRef dialogEnter = RaidBook.LookUp(id).getDialogEnter();
		if (dialogEnter != null && !dialogEnter.seen)
		{
			GameData.instance.windowGenerator.NewDialogPopup(dialogEnter).CLEAR.AddListener(OnRaidDialogClosed);
		}
		else
		{
			DoSummon();
		}
	}

	private void OnRaidDialogClosed(object e)
	{
		DoSummon();
	}

	private void DoSummon()
	{
		GameData.instance.SAVE_STATE.SetRaidSelected(id);
		GameData.instance.windowGenerator.NewRaidDifficultyWindow(id, raidWindow.gameObject);
	}
}
