using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.multiplePrefabsAchievementsCompletedList;

public class AchievementDificultyViewTileModel : BaseModel
{
	public string TrophyRarity;

	public bool showClaimButton;

	public UnityAction claimBtnAction;
}
