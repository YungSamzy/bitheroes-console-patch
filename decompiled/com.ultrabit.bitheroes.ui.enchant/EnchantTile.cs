using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.enchant;

public class EnchantTile : ItemIcon
{
	public const float ASSET_SCALE = 3f;

	public TextMeshProUGUI levelTxt;

	public Button tileButton;

	public Image EnchantBg;

	private int _slot;

	private bool _changeable;

	private bool _selectable;

	private bool _isArmory;

	private Enchants _enchants;

	private EnchantData _enchantData;

	private ItemIcon _itemIcon;

	private UnityAction<EnchantTile> overrideCallback;

	public bool locked => !_enchants.getSlotUnlocked(GameData.instance.PROJECT.character, _slot);

	public int slot => _slot;

	public void LoadDetails(int slot, Enchants enchants, EnchantData enchantData = null, bool changeable = false, bool selectable = false, bool isArmory = false)
	{
		_slot = slot;
		_changeable = changeable;
		_selectable = selectable;
		_isArmory = isArmory;
		_enchants = enchants;
		_enchantData = enchantData;
		if (_enchantData != null && _enchantData.enchantRef != null)
		{
			_enchantData.enchantRef.OverrideItemType(11);
		}
		HoverImages hoverImages = GetComponent<HoverImages>();
		if (hoverImages == null)
		{
			hoverImages = base.gameObject.AddComponent<HoverImages>();
		}
		if (levelTxt == null)
		{
			if (!(base.transform.Find("LevelTxt") != null))
			{
				return;
			}
			levelTxt = base.transform.Find("LevelTxt").GetComponent<TextMeshProUGUI>();
		}
		if (EnchantBg == null)
		{
			if (!(base.transform.Find("IconBtn/Views/BackgroundImage") != null))
			{
				return;
			}
			EnchantBg = base.transform.Find("IconBtn/Views/BackgroundImage").GetComponent<Image>();
		}
		if ((changeable || selectable) && locked)
		{
			EnchantSlotRef enchantSlotRef = EnchantBook.LookupSlot(_slot);
			levelTxt.text = Util.NumberFormat(enchantSlotRef.levelReq);
			Color color = EnchantBg.color;
			color.a = 0.4f;
			EnchantBg.color = color;
			SetItemActionType(0);
		}
		else
		{
			levelTxt.gameObject.SetActive(value: false);
		}
		if (enchantData != null && !_isArmory)
		{
			SetItemData(enchantData, locked: false, -1, tintRarity: true, base.transform.Find("ItemIcon/Views/ItemIconColor").GetComponent<Image>());
			if (changeable)
			{
				SetItemActionType(3);
			}
			else
			{
				SetItemActionType(0);
			}
		}
		else
		{
			if (enchantData != null)
			{
				SetItemData(enchantData, locked: false, -1, tintRarity: true, base.transform.Find("ItemIcon/Views/ItemIconColor").GetComponent<Image>());
				SetItemActionType(3);
			}
			else
			{
				SetItemData(null, locked: false, -1, tintRarity: false, null, showRanks: false, emptySlotFull: true);
			}
			if (locked)
			{
				if (changeable)
				{
					base.onClick.AddListener(delegate
					{
						GameData.instance.audioManager.PlaySoundLink("buttonclick");
						EnchantSlotRef enchantSlotRef2 = EnchantBook.LookupSlot(_slot);
						GameData.instance.windowGenerator.ShowError(Language.GetString("error_requirement_level", new string[1] { Util.NumberFormat(enchantSlotRef2.levelReq) }));
					});
				}
				else
				{
					SetItemActionType(0);
				}
				hoverImages.ForceStart();
				hoverImages.GetOwnTexts();
				return;
			}
			if (changeable)
			{
				base.onClick.AddListener(delegate
				{
					GameData.instance.audioManager.PlaySoundLink("buttonclick");
					GameData.instance.windowGenerator.enchantsWindow.ShowEnchantSelectWindow(_slot);
				});
			}
			else
			{
				SetItemActionType(0);
			}
		}
		hoverImages.ForceStart();
		hoverImages.GetOwnTexts();
	}

	public void OverrideClickBehavior(int type, UnityAction<EnchantTile> callback)
	{
		base.onClick.RemoveAllListeners();
		overrideCallback = callback;
		if (type == 10)
		{
			SetItemActionType(10, OnSelected);
		}
	}

	private void OnSelected(BaseModelData item)
	{
		if (overrideCallback != null)
		{
			overrideCallback(this);
		}
	}

	protected override void OnDestroy()
	{
		base.onClick.RemoveAllListeners();
	}
}
