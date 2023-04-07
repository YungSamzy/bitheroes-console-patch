using com.ultrabit.bitheroes.model.eventsales;
using com.ultrabit.bitheroes.model.language;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.eventsales;

public class EventSalesShopTab : MonoBehaviour
{
	public Button tabBtn;

	private int _index;

	private EventSalesShopTabRef _tabRef;

	public int index => _index;

	public EventSalesShopTabRef tabRef => _tabRef;

	public void LoadDetails(int index, string name, EventSalesShopTabRef tabRef = null)
	{
		_index = index;
		_tabRef = tabRef;
		tabBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString(name);
	}
}
