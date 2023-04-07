using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.ui.team;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.MultiplePrefabsTeamList;

public class TeamListItemModel : BaseModel
{
	public int slot;

	public TeammateData teammateData;

	public UnityAction<int> onAddButtonClicked;

	public UnityAction onRemoveButtonClicked;

	public TeamWindow teamWindow;
}
