using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.chatmuteloglist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI dateTxt;

	public TextMeshProUGUI characterNameTxt;

	public TextMeshProUGUI moderatorNameTxt;

	public TextMeshProUGUI durationTxt;

	public TextMeshProUGUI reasonTxt;

	public Button button;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("DateTxt", out dateTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("CharacterNameTxt", out characterNameTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("ModeratorNameTxt", out moderatorNameTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("DurationTxt", out durationTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("ReasonTxt", out reasonTxt);
		button = root.GetComponent<Button>();
	}
}
