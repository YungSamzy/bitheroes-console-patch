using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.guildinviteslist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI UIName;

	public TextMeshProUGUI UIMembers;

	public TextMeshProUGUI UILevel;

	public Button UIAccept;

	public Button UIDecline;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		root.GetComponentAtPath<TextMeshProUGUI>("MembersTxt", out UIMembers);
		root.GetComponentAtPath<TextMeshProUGUI>("LevelTxt", out UILevel);
		root.GetComponentAtPath<Button>("AcceptBtn", out UIAccept);
		root.GetComponentAtPath<Button>("DeclineBtn", out UIDecline);
	}
}
