using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.chat;

namespace com.ultrabit.bitheroes.ui.brawl;

public class BrawlRoomChatBox : ChatBox
{
	private BrawlRoom _room;

	public override void Create(BrawlRoom room)
	{
		base.LoadDetails(VariableBook.worldChatMessageLimit, VariableBook.worldChatInputLength, showInitials: false);
		_room = room;
	}

	public override void DoMessage(string text)
	{
		base.DoMessage(text);
		BrawlDALC.instance.doMessage(text);
	}

	public override void DoEnable()
	{
		base.DoEnable();
	}

	public override void DoDisable()
	{
		base.DoDisable();
	}
}
