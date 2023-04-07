using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.ui.brawl;
using com.ultrabit.bitheroes.ui.dialog;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.brawl;

public class SummonBtn : MonoBehaviour
{
	public BrawlRef brawlRef;

	public BrawlCreateWindow brawlCreateWindow;

	public void Click()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DialogRef dialogEnter = brawlRef.getDialogEnter();
		if (dialogEnter != null && !dialogEnter.seen)
		{
			DialogPopup dialog = GameData.instance.windowGenerator.NewDialogPopup(dialogEnter);
			dialog.CLEAR.AddListener(delegate
			{
				dialog.CLEAR.RemoveAllListeners();
				DoSummon();
			});
		}
		else
		{
			DoSummon();
		}
	}

	private void DoSummon()
	{
		GameData.instance.SAVE_STATE.SetBrawlSelected(brawlRef.id);
		GameData.instance.windowGenerator.NewBrawlCreateDifficultyWindow(brawlRef, brawlCreateWindow.gameObject);
	}
}
