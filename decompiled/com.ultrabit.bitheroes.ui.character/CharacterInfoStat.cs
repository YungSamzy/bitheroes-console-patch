using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterInfoStat : MonoBehaviour
{
	public TextMeshProUGUI statNameTxt;

	public TextMeshProUGUI statValueTxt;

	public void LoadDetails(CharacterInfoData statData, CharacterInfoValue statInfo)
	{
		GetComponent<RectTransform>();
		if (statData.offset != 0)
		{
			float x = statData.offset / 3 * 2;
			statNameTxt.rectTransform.sizeDelta -= new Vector2(x, 0f);
			statValueTxt.rectTransform.sizeDelta += new Vector2(x, 0f);
			statValueTxt.rectTransform.anchoredPosition -= new Vector2(x, 0f);
		}
		if (statData.colorName != null)
		{
			ColorUtility.TryParseHtmlString(statData.colorName, out var color);
			statNameTxt.color = color;
		}
		if (statData.colorValue != null)
		{
			ColorUtility.TryParseHtmlString(statData.colorValue, out var color2);
			statValueTxt.color = color2;
		}
		statNameTxt.text = "";
		statValueTxt.text = "";
		string text = statInfo.name + ":";
		if (statInfo.color != null)
		{
			text = Util.colorString(text, statInfo.color);
		}
		statNameTxt.text += text;
		statValueTxt.text += ((statInfo.value.Length <= 0) ? " " : statInfo.value);
	}
}
