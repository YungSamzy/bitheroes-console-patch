using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.chat;

public class ChatTab : MonoBehaviour
{
	public Button tabBtn;

	public TextMeshProUGUI btnTxt;

	public TextMeshProUGUI countTxt;

	public Image countBG;

	public int tabIndex;

	private int _tab;

	public bool interactable
	{
		get
		{
			return tabBtn.interactable;
		}
		set
		{
			tabBtn.interactable = value;
		}
	}

	public void LoadDetails(int tab)
	{
		_tab = tab;
		btnTxt.text = ChatWindow.GetTabName(tab);
		SetCount();
	}

	public void OnTabClick()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		base.transform.parent.parent.BroadcastMessage("OnTabButtonClicked", this);
	}

	public void SetCount(int count = 0)
	{
		if (count > 0)
		{
			countTxt.text = Util.colorString(Util.NumberFormat(count), ChatWindow.GetTabColor(_tab));
			countTxt.gameObject.SetActive(value: true);
			countBG.gameObject.SetActive(value: true);
		}
		else
		{
			countTxt.gameObject.SetActive(value: false);
			countBG.gameObject.SetActive(value: false);
		}
	}
}
