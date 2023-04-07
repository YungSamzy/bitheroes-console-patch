using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.ui.lists.eventrewardsranklist;

public class EventRewardItemRank
{
	public EventRef eventRef;

	public EventRewardRef rewardRef;

	public int points = -1;

	public int rank = -1;

	public bool displayRank = true;

	public bool currentEvent;

	public bool currentZone;
}
