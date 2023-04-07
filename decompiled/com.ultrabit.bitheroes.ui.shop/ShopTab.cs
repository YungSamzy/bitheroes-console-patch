using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.shop;

public class ShopTab : MonoBehaviour
{
	public Button tabBtn;

	private int _index;

	private ShopTabRef _tabRef;

	public int index => _index;

	public ShopTabRef tabRef => _tabRef;

	public void LoadDetails(int index, string name, ShopTabRef tabRef = null)
	{
		_index = index;
		_tabRef = tabRef;
		tabBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString(name);
	}
}
