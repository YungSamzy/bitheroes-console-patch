using System;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.ui.brawl;

namespace com.ultrabit.bitheroes.ui.lists.brawlteamlist;

[Serializable]
public class BrawlTeamListItemData
{
	public int slot;

	public BrawlPlayer player;

	public BrawlRoom room;

	public BrawlRoomWindow roomWindow;
}
