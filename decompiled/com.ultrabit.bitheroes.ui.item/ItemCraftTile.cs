using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemCraftTile : MonoBehaviour
{
	public TextMeshProUGUI qtyRequiredTxt;

	public TextMeshProUGUI qtyOwnedTxt;

	public Transform itemIcon;

	private ItemData _itemData;

	private bool _locked;

	private float _multiplier;

	private bool _loaded;

	private bool _requirementsMet;

	private ItemIcon _itemIcon;

	public bool requirementsMet => _requirementsMet;

	public void LoadDetails(ItemData itemData, bool locked = false, float multiplier = 1f)
	{
		_itemData = itemData;
		_locked = locked;
		_multiplier = multiplier;
		if (!itemIcon.gameObject.TryGetComponent<CanvasGroup>(out var _))
		{
			itemIcon.gameObject.AddComponent<CanvasGroup>();
		}
		if (_itemIcon == null)
		{
			_itemIcon = itemIcon.gameObject.AddComponent<ItemIcon>();
		}
		int itemQty = GameData.instance.PROJECT.character.getItemQty(_itemData.itemRef);
		int num = Mathf.RoundToInt((float)_itemData.qty * _multiplier);
		_itemIcon.SetItemData(new ItemData(_itemData.itemRef, itemQty), _locked, 0);
		_itemIcon.SetItemActionType((_itemData.itemRef.itemType == 6) ? 7 : 0);
		bool flag = _itemData.itemRef.isHidden();
		_itemIcon.setQty((!_locked) ? itemQty : 0, show: true);
		if (_locked || itemQty >= num)
		{
			_requirementsMet = true;
			_itemIcon.SetAlpha();
			qtyRequiredTxt.color = Color.white;
		}
		else
		{
			_requirementsMet = false;
			_itemIcon.SetAlpha(0.5f);
			qtyOwnedTxt.color = new Color(0.7f, 0.7f, 0.7f, 1f);
			qtyRequiredTxt.color = Color.red;
		}
		if (num < 0)
		{
			num = -num;
			qtyRequiredTxt.text = (_locked ? Language.GetString("ui_question_mark") : Util.NumberFormat(num, abbreviate: true, shortbool: true, 10f));
			qtyRequiredTxt.text = "+" + qtyRequiredTxt.text;
			qtyRequiredTxt.color = Color.green;
		}
		else
		{
			qtyRequiredTxt.text = (_locked ? Language.GetString("ui_question_mark") : Util.NumberFormat(num, abbreviate: true, shortbool: true, 10f));
			if (_locked)
			{
				qtyRequiredTxt.color = Color.white;
			}
		}
		_itemIcon.SetHidden(flag || _locked, _locked, _locked);
	}
}
