using com.ultrabit.bitheroes.ui.lists.shoppromolist;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.promo;

public class ShopPromoDotClick : MonoBehaviour
{
	public ShopPromoList shopPromoList;

	public int id;

	public void SetID(int _id)
	{
		id = _id;
	}

	public void Click()
	{
		shopPromoList.ScrollToPage(id, direct: true);
	}
}
