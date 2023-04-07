using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.daily;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.daily;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceDailyQuestTile : MainUIButton
{
	public GameObject notify;

	public GameObject shimmer;

	public TextMeshProUGUI questsTxt;

	public override void Create()
	{
		LoadDetails(Language.GetString("daily_quest_plural_name"), VariableBook.GetGameRequirement(17));
		buttonTxt.gameObject.SetActive(value: false);
		GameData.instance.PROJECT.character.AddListener("DAILY_QUEST_CHANGE", OnDailyQuestChange);
		foreach (DailyQuestData quest in GameData.instance.PROJECT.character.dailyQuests.quests)
		{
			if (quest.completed)
			{
				GameData.instance.PROJECT.character.dailyQuests.setUpdated(v: true);
				break;
			}
		}
		DoUpdate();
	}

	private void OnDailyQuestChange()
	{
		DoUpdate();
	}

	public override void DoClick()
	{
		base.DoClick();
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(DailyQuestsWindow)) == null)
		{
			GameData.instance.windowGenerator.NewDailyQuestsWindow();
		}
	}

	public override void DoUpdate()
	{
		base.DoUpdate();
		if (GameData.instance.PROJECT.character.dailyQuests.updated)
		{
			questsTxt.text = Util.ParseString("^" + Util.NumberFormat(GameData.instance.PROJECT.character.dailyQuests.quests.Count) + "^");
			shimmer.SetActive(value: true);
			notify.SetActive(value: true);
		}
		else
		{
			questsTxt.text = Util.NumberFormat(GameData.instance.PROJECT.character.dailyQuests.quests.Count);
			shimmer.SetActive(value: false);
			notify.SetActive(value: false);
		}
	}

	private new void OnDestroy()
	{
		if (GameData.instance.PROJECT.character != null)
		{
			GameData.instance.PROJECT.character.RemoveListener("DAILY_QUEST_CHANGE", OnDailyQuestChange);
		}
	}
}
