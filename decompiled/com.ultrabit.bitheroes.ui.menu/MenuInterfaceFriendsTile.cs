using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceFriendsTile : MainUIButton
{
	public TextMeshProUGUI onlineTxt;

	public Image requestsIcon;

	public override void Create()
	{
		LoadDetails(Language.GetString("ui_friends"), VariableBook.GetGameRequirement(5));
		GameData.instance.PROJECT.character.AddListener("FRIEND_CHANGE", OnFriendChange);
		GameData.instance.PROJECT.character.AddListener("REQUEST_CHANGE", OnRequestChange);
		DoUpdate();
	}

	private void OnFriendChange()
	{
		DoUpdate();
	}

	private void OnRequestChange()
	{
		DoUpdate();
	}

	public override void DoUpdate()
	{
		base.DoUpdate();
		int count = GameData.instance.PROJECT.character.getFriendsOnline().Count;
		int count2 = GameData.instance.PROJECT.character.requests.Count;
		if (count > 0)
		{
			onlineTxt.text = Util.NumberFormat(count);
		}
		else
		{
			onlineTxt.text = "";
		}
		if (count2 > 0)
		{
			requestsIcon.gameObject.SetActive(value: true);
		}
		else
		{
			requestsIcon.gameObject.SetActive(value: false);
		}
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.windowGenerator.NewFriendWindow();
	}

	private new void OnDestroy()
	{
		if (GameData.instance.PROJECT.character != null)
		{
			GameData.instance.PROJECT.character.RemoveListener("FRIEND_CHANGE", OnFriendChange);
			GameData.instance.PROJECT.character.RemoveListener("REQUEST_CHANGE", OnRequestChange);
		}
	}
}
