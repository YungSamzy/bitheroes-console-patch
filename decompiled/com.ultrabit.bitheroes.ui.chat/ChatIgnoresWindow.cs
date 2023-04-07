using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.lists.chatignoreslist;
using TMPro;

namespace com.ultrabit.bitheroes.ui.chat;

public class ChatIgnoresWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public ChatIgnoresList chatIgnoreList;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_ignores");
		GameData.instance.PROJECT.character.AddListener("CHAT_CHANGE", OnChatChange);
		chatIgnoreList.InitList();
		CreateList();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnChatChange()
	{
		CreateList();
	}

	private void CreateList()
	{
		chatIgnoreList.ClearList();
		for (int i = 0; i < GameData.instance.PROJECT.character.chatIgnores.Count; i++)
		{
			ChatPlayerData playerData = GameData.instance.PROJECT.character.chatIgnores[i];
			chatIgnoreList.Data.InsertOneAtEnd(new ChatIgnoreItem
			{
				playerData = playerData
			});
		}
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("CHAT_CHANGE", OnChatChange);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}
}
