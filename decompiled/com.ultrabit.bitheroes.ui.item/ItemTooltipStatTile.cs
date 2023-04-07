using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemTooltipStatTile : MonoBehaviour
{
	public enum STAT_TYPE
	{
		POWER,
		STAMINA,
		AGILITY
	}

	public Image powerIcon;

	public Image staminaIcon;

	public Image agilityIcon;

	public ItemCompareWidget itemCompareWidget;

	public TextMeshProUGUI txtStat;

	public TextMeshProUGUI txtChange;

	public void LoadDetails(STAT_TYPE type, int stat, int equipped)
	{
		powerIcon.gameObject.SetActive(type.Equals(STAT_TYPE.POWER));
		staminaIcon.gameObject.SetActive(type.Equals(STAT_TYPE.STAMINA));
		agilityIcon.gameObject.SetActive(type.Equals(STAT_TYPE.AGILITY));
		txtStat.text = Util.NumberFormat(stat);
		if (equipped != stat)
		{
			int num = stat - equipped;
			Color colorFromHex = Util.GetColorFromHex(Util.GetNumberColor(stat, equipped, equipped));
			txtChange.color = colorFromHex;
			txtChange.text = ((num > 0) ? "+" : "") + Util.NumberFormat(num);
			itemCompareWidget.SetCompare(num);
			txtChange.gameObject.SetActive(value: true);
			itemCompareWidget.gameObject.SetActive(value: true);
		}
		else
		{
			txtChange.gameObject.SetActive(value: false);
			itemCompareWidget.gameObject.SetActive(value: false);
		}
	}

	public void LoadDetailsAsAs3(STAT_TYPE type, int stat, int change)
	{
		powerIcon.gameObject.SetActive(type.Equals(STAT_TYPE.POWER));
		staminaIcon.gameObject.SetActive(type.Equals(STAT_TYPE.STAMINA));
		agilityIcon.gameObject.SetActive(type.Equals(STAT_TYPE.AGILITY));
		txtStat.text = Util.NumberFormat(stat);
		itemCompareWidget.SetCompare("=");
		if (change == 0)
		{
			txtChange.gameObject.SetActive(value: false);
			itemCompareWidget.gameObject.SetActive(value: false);
			return;
		}
		if (change > 0)
		{
			itemCompareWidget.SetCompare("+");
		}
		else
		{
			itemCompareWidget.SetCompare("-");
		}
		Color colorFromHex = Util.GetColorFromHex(Util.GetNumberColor(stat, stat - change, stat - change));
		txtChange.color = colorFromHex;
		txtChange.text = ((change >= 0) ? ("+" + Util.NumberFormat(change)) : Util.NumberFormat(change));
	}
}
