using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.language;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.fishing;

public class FishingShopTab : MonoBehaviour
{
	public Button tabBtn;

	private int _index;

	private FishingShopTabRef _tabRef;

	public int index => _index;

	public FishingShopTabRef tabRef => _tabRef;

	public void LoadDetails(int index, string name, FishingShopTabRef tabRef = null)
	{
		_index = index;
		_tabRef = tabRef;
		tabBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString(name);
	}
}
