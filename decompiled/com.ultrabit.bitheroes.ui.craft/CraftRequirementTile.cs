using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.craft;

public class CraftRequirementTile : MonoBehaviour
{
	public ItemIcon itemIcon;

	public TextMeshProUGUI txtRequiredQty;

	private bool isCraftable;

	public void Setup(ItemData itemData)
	{
		if (itemIcon == null)
		{
			itemIcon = base.gameObject.AddComponent<ItemIcon>();
		}
		int itemQty = GameData.instance.PROJECT.character.getItemQty(itemData.itemRef);
		isCraftable = itemQty > 0 && itemQty >= itemData.qty;
		itemIcon.SetItemData(itemData, isCraftable, itemQty);
		itemIcon.SetItemActionType(0);
		txtRequiredQty.text = Util.NumberFormat(itemData.qty);
	}

	public bool IsCraftable()
	{
		return isCraftable;
	}
}
