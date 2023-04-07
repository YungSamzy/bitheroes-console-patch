using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.familiar;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.augment;

public class AugmentTile : ItemIcon
{
	public const float ASSET_SCALE = 3f;

	public TextMeshProUGUI levelTxt;

	public Image bg;

	public Image color;

	public GameObject icon;

	public Image itemIconBg;

	public Image itemIconColor;

	private int _slot;

	private FamiliarRef _familiarRef;

	private bool _changeable;

	private bool _selectable;

	private bool _borders;

	private AugmentSlotSelectWindow _slotSelectWindow;

	private Augments _augments;

	private AugmentData _augmentData;

	[HideInInspector]
	public UnityCustomEvent SELECT = new UnityCustomEvent();

	public bool locked => !_augments.getSlotUnlocked(GameData.instance.PROJECT.character, _slot);

	public int slot => _slot;

	public bool changeable => _changeable;

	public bool selectable => _selectable;

	public FamiliarRef familiarRef => _familiarRef;

	public AugmentData augmentData => _augmentData;

	public void LoadDetails(int slot, Augments augments, AugmentData augmentData = null, FamiliarRef familiarRef = null, bool changeable = false, bool selectable = false, bool borders = false, AugmentSlotSelectWindow slotSelectWindow = null)
	{
		_slot = slot;
		_familiarRef = familiarRef;
		_changeable = changeable;
		_selectable = selectable;
		_borders = borders;
		_slotSelectWindow = slotSelectWindow;
		if (_borders)
		{
			bg.gameObject.SetActive(value: true);
			color.gameObject.SetActive(value: true);
			GetComponent<AugmentsHandler>().ReplaceAugmentBG(bg, slot);
			GetComponent<AugmentsHandler>().ReplaceAugmentColor(color, slot);
		}
		else
		{
			bg.gameObject.SetActive(value: false);
			color.gameObject.SetActive(value: false);
		}
		SetAugments(augments);
		SetAugment(augmentData);
		if ((changeable || selectable) && locked)
		{
			AugmentSlotRef augmentSlotRef = AugmentBook.LookupSlot(_slot);
			levelTxt.text = Util.NumberFormat(augmentSlotRef.levelReq);
		}
		else
		{
			levelTxt.gameObject.SetActive(value: false);
		}
	}

	public void OverrideAugmentBG(int slot)
	{
		GetComponent<AugmentsHandler>().ReplaceAugmentBG(bg, slot);
		GetComponent<AugmentsHandler>().ReplaceAugmentColor(color, slot);
	}

	public void SetAugments(Augments augments)
	{
		_augments = augments;
	}

	public void SetAugment(AugmentData augmentData)
	{
		_augmentData = augmentData;
		_borders = _augmentData != null && augmentData.equipped;
		SetItemData(_augmentData, locked: false, 1, tintRarity: true, _borders ? color : null);
		base.onClick.RemoveAllListeners();
		SetItemActionType(0, null);
		if (_augmentData != null)
		{
			SetItemActionType((!_changeable) ? (_selectable ? 10 : 0) : (_selectable ? 10 : 3), OnSelectedOrChanged);
		}
		else
		{
			base.onClick.RemoveAllListeners();
			base.onClick.AddListener(delegate
			{
				OnNullAugmentSelected();
			});
		}
		bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 1f);
		color.color = new Color(color.color.r, color.color.g, color.color.b, 1f);
		if (_borders)
		{
			itemIconBg.GetComponent<CanvasGroup>().alpha = 0f;
			itemIconColor.GetComponent<CanvasGroup>().alpha = 0f;
		}
		else
		{
			itemIconBg.GetComponent<CanvasGroup>().alpha = 1f;
			itemIconColor.GetComponent<CanvasGroup>().alpha = 1f;
		}
		if ((_changeable || _selectable) && locked)
		{
			bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0.15f);
			color.color = new Color(color.color.r, color.color.g, color.color.b, 0.15f);
		}
		else
		{
			bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 1f);
			color.color = new Color(color.color.r, color.color.g, color.color.b, 1f);
		}
		if (_augmentData == null)
		{
			icon.GetComponent<CanvasGroup>().alpha = 0f;
			if (!_borders)
			{
				itemIconBg.color = Color.white;
				itemIconColor.color = Color.white;
				if (locked)
				{
					itemIconBg.GetComponent<CanvasGroup>().alpha = 0.15f;
					itemIconColor.GetComponent<CanvasGroup>().alpha = 0.15f;
				}
				else
				{
					itemIconBg.GetComponent<CanvasGroup>().alpha = 1f;
					itemIconColor.GetComponent<CanvasGroup>().alpha = 1f;
				}
			}
		}
		else
		{
			icon.GetComponent<CanvasGroup>().alpha = 1f;
		}
		HoverImages hoverImages = GetComponent<HoverImages>();
		if (hoverImages == null)
		{
			hoverImages = base.gameObject.AddComponent<HoverImages>();
		}
		hoverImages.ForceStart();
		hoverImages.GetOwnTexts();
	}

	private void OnSelectedOrChanged(BaseModelData itemData)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (locked)
		{
			AugmentSlotRef augmentSlotRef = AugmentBook.LookupSlot(_slot);
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_requirement_level", new string[1] { Util.NumberFormat(augmentSlotRef.levelReq) }));
		}
		else
		{
			SELECT.Invoke(this);
		}
	}

	private void OnNullAugmentSelected()
	{
		if (!_changeable && !_selectable)
		{
			return;
		}
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (_slotSelectWindow == null)
		{
			if (locked)
			{
				AugmentSlotRef augmentSlotRef = AugmentBook.LookupSlot(_slot);
				GameData.instance.windowGenerator.ShowError(Language.GetString("error_requirement_level", new string[1] { Util.NumberFormat(augmentSlotRef.levelReq) }));
			}
			else
			{
				ShowChangeableAugments();
			}
		}
		else
		{
			OnSelectedOrChanged(_augmentData);
		}
	}

	public void ShowChangeableAugments(GameObject parent = null)
	{
		AugmentSlotRef augmentSlotRef = AugmentBook.LookupSlot(_slot);
		if (GameData.instance.PROJECT.character.augments.getAugmentsByType(augmentSlotRef.typeRef.id).Count <= 0)
		{
			DoCreate();
			return;
		}
		AugmentSelectWindow augmentSelectWindow = GameData.instance.windowGenerator.NewAugmentSelectWindow(GameData.instance.PROJECT.character.augments, changeable: true, _familiarRef, _slot, parent);
		augmentSelectWindow.SELECT.AddListener(delegate(object selected)
		{
			OnChangeAugmentSelected(selected);
		});
		augmentSelectWindow.SCROLL_OUT_START.AddListener(OnAugmentSelectionClosed);
	}

	private void DoCreate()
	{
		if (GameData.instance.PROJECT.character.augments.augments.Count >= VariableBook.augmentMax)
		{
			GameData.instance.windowGenerator.ShowErrorCode(119);
			return;
		}
		AugmentSlotRef augmentSlotRef = AugmentBook.LookupSlot(_slot);
		List<ItemData> itemsByType = GameData.instance.PROJECT.character.inventory.GetItemsByType(15, augmentSlotRef?.typeRef.id ?? (-1));
		if (itemsByType.Count <= 0)
		{
			GameData.instance.windowGenerator.showAugmentError(Language.GetString("error_augment_insert_unavailable"));
		}
		else
		{
			GameData.instance.windowGenerator.ShowItems(itemsByType, compare: false, added: true, Language.GetString("ui_identify"), large: true, forceNonEquipment: true, Util.parseMultiLine(Language.GetString("augment_help_desc")));
		}
	}

	private void OnAugmentSelectionClosed(object selectWindow)
	{
		(selectWindow as AugmentSelectWindow).SELECT.RemoveListener(delegate(object selected)
		{
			OnChangeAugmentSelected(selected);
		});
		(selectWindow as AugmentSelectWindow).SCROLL_OUT_START.RemoveListener(OnAugmentSelectionClosed);
	}

	private void OnChangeAugmentSelected(object selected)
	{
		DoAugmentEquip(selected as AugmentData);
	}

	public void DoAugmentEquip(AugmentData augmentData)
	{
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(45), OnAugmentEquip);
		CharacterDALC.instance.doAugmentEquip(_familiarRef, _slot, augmentData);
	}

	private void OnAugmentEquip(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(45), OnAugmentEquip);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GameData.instance.PROJECT.character.augments.updateAugmentList(AugmentData.listFromSFSObject(sfsob));
		if (GameData.instance.PROJECT.character.augments.getFamiliarAugmentSlot(_familiarRef, _slot) != null)
		{
			GameData.instance.audioManager.PlaySoundLink("equip");
		}
		else
		{
			GameData.instance.audioManager.PlaySoundLink("unequip");
		}
		AugmentSelectWindow augmentSelectWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AugmentSelectWindow)) as AugmentSelectWindow;
		if ((bool)augmentSelectWindow)
		{
			augmentSelectWindow.OnClose();
		}
		AugmentOptionsWindow augmentOptionsWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AugmentOptionsWindow)) as AugmentOptionsWindow;
		if ((bool)augmentOptionsWindow)
		{
			augmentOptionsWindow.OnClose();
		}
		FamiliarAugmentsPanel componentInParent = GetComponentInParent<FamiliarAugmentsPanel>();
		if (componentInParent != null)
		{
			componentInParent.DoUpdate();
		}
	}

	public void ShowAugment(object parent = null)
	{
		if (_augmentData != null)
		{
			GameData.instance.windowGenerator.NewAugmentWindow(_augmentData, parent as GameObject);
		}
	}

	protected override void OnDestroy()
	{
		base.onClick.RemoveListener(delegate
		{
			OnNullAugmentSelected();
		});
	}
}
