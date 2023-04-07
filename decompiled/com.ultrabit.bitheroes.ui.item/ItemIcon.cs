using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.item.action;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.battle;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemIcon : Button
{
	private BaseModelData _item;

	private ItemData _cosmetic;

	private int _qty;

	private bool _qtyShow;

	private bool _qtyForceStats;

	private Transform tooltip;

	private ItemActionBase _itemIconAction;

	private ItemIconComparision itemIconComparision;

	public Transform notifyWidgetPrefab;

	public Transform selectWidgetPrefab;

	public Transform lockWidgetPrefab;

	private GameObject _selectedWidget;

	private GameObject _lockedWidget;

	private GameObject _notifyWidget;

	private GameObject _unknownIcon;

	private Image _asset;

	private bool _isThumbnail;

	private bool _compare;

	private bool _selected;

	private bool _locked;

	private bool _notify;

	private bool _hidden;

	private bool _unknown;

	private bool _clickable;

	private bool _alphaLocked;

	private bool _forceConsume;

	private Transform _swfAsset;

	private CharacterData _characterData;

	private ArmoryEquipment _armoryEquipment;

	[HideInInspector]
	public RectTransform rectTransform;

	public CharacterData characterData => _characterData;

	public bool alphaLocked => _alphaLocked;

	public ItemRef itemRef
	{
		get
		{
			if (item == null)
			{
				return null;
			}
			return item.itemRef;
		}
	}

	public BaseModelData item => _item;

	public ItemData cosmetic => _cosmetic;

	public ItemActionBase itemActionBase => _itemIconAction;

	public bool isThumbnail
	{
		get
		{
			return _isThumbnail;
		}
		set
		{
			_isThumbnail = value;
		}
	}

	public bool compare => _compare;

	public int qty => _qty;

	public bool forceConsume => _forceConsume;

	public void SetMountData(MountData mountData)
	{
		base.transform.Find("Views/NFTDots")?.gameObject.SetActive(value: false);
		base.transform.Find("Views/NFTShine")?.gameObject.SetActive(value: false);
		if (SetItemData(mountData))
		{
			SetupItemComparision(showCosmetic: false, showComparision: true);
		}
	}

	public void SetMountData(MountData mountData, MountRef cosmetic)
	{
		base.transform.Find("Views/NFTDots")?.gameObject.SetActive(value: false);
		base.transform.Find("Views/NFTShine")?.gameObject.SetActive(value: false);
		if (SetItemData(mountData))
		{
			if (cosmetic == mountData.mountRef)
			{
				cosmetic = null;
			}
			SetupItemComparision(cosmetic != null, showComparision: false);
			_cosmetic = null;
			if (cosmetic != null)
			{
				_cosmetic = new ItemData(cosmetic, 1);
			}
		}
	}

	public void SetRuneData(RuneRef runeRef)
	{
		base.transform.Find("Views/NFTDots")?.gameObject.SetActive(value: false);
		base.transform.Find("Views/NFTShine")?.gameObject.SetActive(value: false);
		if (SetItemData(new ItemData(runeRef, 0), locked: false, -1, tintRarity: false))
		{
			SetupItemComparision(showCosmetic: false, showComparision: false);
		}
	}

	public void SetEquipmentData(ArmoryRef itemRef, ArmoryRef cosmetic, bool showComparision = true)
	{
		base.transform.Find("Views/NFTDots")?.gameObject.SetActive(value: false);
		base.transform.Find("Views/NFTShine")?.gameObject.SetActive(value: false);
		if (SetItemData(new ItemData(itemRef, 0)))
		{
			SetupItemComparision(cosmetic != null, showComparision);
		}
		_cosmetic = null;
		if (cosmetic != null)
		{
			_cosmetic = new ItemData(cosmetic, 1);
		}
	}

	public void SetEquipmentData(EquipmentRef itemRef, EquipmentRef cosmetic, bool showComparision = true)
	{
		base.transform.Find("Views/NFTDots")?.gameObject.SetActive(value: false);
		base.transform.Find("Views/NFTShine")?.gameObject.SetActive(value: false);
		if (SetItemData(new ItemData(itemRef, 0)))
		{
			SetupItemComparision(cosmetic != null, showComparision);
		}
		_cosmetic = null;
		if (cosmetic != null)
		{
			_cosmetic = new ItemData(cosmetic, 1);
		}
	}

	public void SetEquipmentData(EquipmentRef itemRef, EquipmentRef cosmetic, bool showComparision = true, int actualQty = 0)
	{
		base.transform.Find("Views/NFTDots")?.gameObject.SetActive(value: false);
		base.transform.Find("Views/NFTShine")?.gameObject.SetActive(value: false);
		if (SetItemData(new ItemData(itemRef, actualQty)))
		{
			SetupItemComparision(cosmetic != null, showComparision);
		}
		_cosmetic = null;
		if (cosmetic != null)
		{
			_cosmetic = new ItemData(cosmetic, 1);
		}
	}

	public void SetEquipmentData(ItemData itemData, EquipmentRef cosmetic, bool showComparision = true, int displayQty = -1)
	{
		base.transform.Find("Views/NFTDots")?.gameObject.SetActive(value: false);
		base.transform.Find("Views/NFTShine")?.gameObject.SetActive(value: false);
		if (SetItemData(itemData, locked: false, displayQty))
		{
			SetupItemComparision(cosmetic != null, showComparision);
		}
		_cosmetic = null;
		if (cosmetic != null)
		{
			_cosmetic = new ItemData(cosmetic, 1);
		}
	}

	public void SetEnchantData(EnchantData enchantData, bool locked = false)
	{
		base.transform.Find("Views/NFTDots")?.gameObject.SetActive(value: false);
		base.transform.Find("Views/NFTShine")?.gameObject.SetActive(value: false);
		if (SetItemData(enchantData, locked))
		{
			SetupItemComparision(showCosmetic: false, showComparision: true);
		}
	}

	public void SetFamiliarData(FamiliarRef itemRef, FamiliarRef cosmetic, bool showComparision = true)
	{
		base.transform.Find("Views/NFTDots")?.gameObject.SetActive(value: false);
		base.transform.Find("Views/NFTShine")?.gameObject.SetActive(value: false);
		if (SetItemData(new ItemData(itemRef, 0)))
		{
			SetupItemComparision(cosmetic != null, showComparision);
		}
		if (cosmetic != null)
		{
			_cosmetic = new ItemData(cosmetic, 1);
		}
	}

	public void SetAugmentData(AugmentData augmentData)
	{
		base.transform.Find("Views/NFTDots")?.gameObject.SetActive(value: false);
		base.transform.Find("Views/NFTShine")?.gameObject.SetActive(value: false);
		if (SetItemData(augmentData))
		{
			SetupItemComparision(showCosmetic: false, showComparision: true);
		}
	}

	public void SetItemActionType(int type, UnityAction<BaseModelData> callback)
	{
		SetItemActionType(type);
		AddOnItemIconActionPostExecuteCallback(callback);
	}

	public void SetItemActionType(int type)
	{
		_itemIconAction = ItemActionFactory.Create(_item, type, null, this);
	}

	public void SetItemActionTypeWithName(int type, string altName)
	{
		_itemIconAction = ItemActionFactory.Create(_item, type, altName, this);
	}

	public void SetItemActionType(int type, bool forceConsume)
	{
		_forceConsume = forceConsume;
		_itemIconAction = ItemActionFactory.Create(_item, type, null, this, forceConsume);
	}

	public void SetCharacterData(CharacterData characterData)
	{
		_characterData = characterData;
	}

	public void SetArmoryEquipment(ArmoryEquipment armoryEquipment)
	{
		_armoryEquipment = armoryEquipment;
	}

	public void AddOnItemIconActionPostExecuteCallback(UnityAction<BaseModelData> callback)
	{
		_itemIconAction.onPostExecuteCallback = callback;
	}

	public void DisableItem(bool disable, bool changeAction = true, bool changeClickable = true, bool isAnimated = false)
	{
		float value = (disable ? 0.5f : 1f);
		if (disable && changeAction)
		{
			SetItemActionType(0);
		}
		SetAlpha(value, isAnimated);
		if (changeClickable)
		{
			_clickable = !disable;
		}
	}

	public void EmptySlot(bool emptySlotFull)
	{
		_item = null;
		TextMeshProUGUI componentInChildren = base.transform.GetComponentInChildren<TextMeshProUGUI>();
		if (componentInChildren != null)
		{
			componentInChildren.text = "";
		}
		Transform transform = base.transform.Find("Views/ItemIconColor");
		if (transform == null)
		{
			transform = base.transform.Find("ItemIcon/Views/ItemIconColor");
		}
		transform.GetComponent<Image>().color = Color.white;
		if (!emptySlotFull)
		{
			Image componentInChildren2 = base.transform.GetComponentInChildren<Image>();
			componentInChildren2.color = Color.white;
			Color white = Color.white;
			white.a = 0.5f;
			componentInChildren2.color = white;
		}
		_itemIconAction = null;
		SetupItemComparision(showCosmetic: false, showComparision: false);
		HideComparison();
		GameObject spriteLoaderPlaceholder = GameData.instance.main.assetLoader.GetSpriteLoaderPlaceholder(base.gameObject);
		if (spriteLoaderPlaceholder != null)
		{
			_asset = spriteLoaderPlaceholder.GetComponent<Image>();
			_asset.overrideSprite = null;
			_asset.color = new Color(1f, 1f, 1f, 0f);
		}
		ItemIconRanks componentInChildren3 = GetComponentInChildren<ItemIconRanks>();
		if (componentInChildren3 != null)
		{
			componentInChildren3.SetRank(null, showRanks: false, _isThumbnail);
		}
	}

	public bool SetItemData(BaseModelData data, bool locked = false, int qtyDisplay = -1, bool tintRarity = true, Image bg = null, bool showRanks = true, bool emptySlotFull = false, bool isCosmetic = false, RarityRef forcedRarity = null)
	{
		if (_unknownIcon != null)
		{
			_unknownIcon.gameObject.SetActive(value: false);
		}
		if (_swfAsset != null)
		{
			Object.Destroy(_swfAsset.gameObject);
		}
		_clickable = true;
		_qtyShow = false;
		rectTransform = GetComponent<RectTransform>();
		if (data == null || data.itemRef == null)
		{
			EmptySlot(emptySlotFull);
			return false;
		}
		_item = data;
		if (isCosmetic)
		{
			(_item as ItemData).SetCosmeticStats();
		}
		GameObject spriteLoaderPlaceholder = GameData.instance.main.assetLoader.GetSpriteLoaderPlaceholder(base.gameObject);
		if (spriteLoaderPlaceholder != null)
		{
			_asset = spriteLoaderPlaceholder.GetComponent<Image>();
			_asset.overrideSprite = null;
			_asset.color = new Color(1f, 1f, 1f, 0f);
		}
		if (!AssetLoader.UrlIsSWF(data.itemRef.icon))
		{
			Sprite spriteIcon = data.itemRef.GetSpriteIcon();
			if (spriteIcon != null && _asset != null)
			{
				_asset.overrideSprite = spriteIcon;
				_asset.color = Color.white;
			}
		}
		else if (_asset != null)
		{
			_swfAsset = GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.GetAssetType(data.itemRef, icon: true), data.itemRef.icon);
			if (_swfAsset != null)
			{
				_swfAsset.SetParent(_asset.transform, worldPositionStays: false);
				_swfAsset.localPosition = Vector3.zero;
				_swfAsset.localScale = Vector3.one;
			}
		}
		_qty = ((qtyDisplay >= 0) ? qtyDisplay : data.qty);
		updateQty();
		_alphaLocked = locked;
		AlphaBackground(bg, locked);
		if (data.itemRef != null && data.itemRef.rarityRef != null && data.itemRef.rarityRef.objectColor != null && tintRarity)
		{
			Image image = bg;
			Transform transform = base.transform.Find("Views/ItemIconColor");
			if (transform != null)
			{
				image = transform.GetComponent<Image>();
			}
			if (image != null)
			{
				ColorUtility.TryParseHtmlString("#" + ((forcedRarity == null) ? data.itemRef.rarityRef.objectColor : forcedRarity.objectColor), out var color);
				image.color = color;
			}
		}
		ItemIconRanks componentInChildren = GetComponentInChildren<ItemIconRanks>();
		if (componentInChildren != null)
		{
			componentInChildren.SetRank(_item, showRanks, _isThumbnail);
		}
		bool active = data.itemRef != null && data.itemRef.isNFT;
		base.transform.Find("Views/NFTDots")?.gameObject.SetActive(active);
		base.transform.Find("Views/NFTShine")?.gameObject.SetActive(active);
		_itemIconAction = ItemActionFactory.Create(data, -1, null, this);
		SetupItemComparision(showCosmetic: false, showComparision: false);
		return true;
	}

	public void AlphaBackground(Image bg, bool locked)
	{
		Image image = ((!(bg == null)) ? bg : base.transform.GetComponentInChildren<Image>());
		Color white = Color.white;
		white.a = (locked ? 0.5f : 1f);
		image.color = white;
	}

	public void setQty(int qty, bool show = false, bool forceStats = false)
	{
		if (_qty != qty || show)
		{
			_qty = qty;
			_qtyShow = show;
			_qtyForceStats = forceStats;
			updateQty();
		}
	}

	public void SetClickable(bool clickable)
	{
		_clickable = clickable;
	}

	public void SetItemDataNoDisplay(BaseModelData data)
	{
		_clickable = true;
		_item = data;
		_itemIconAction = ItemActionFactory.Create(data, -1, null, this);
	}

	public virtual void OnAssetRefreshed()
	{
	}

	public virtual void SetSelected(bool selected)
	{
		_selected = selected;
		if (selected)
		{
			if (_selectedWidget == null)
			{
				Transform transform = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/item/ItemSelectWidget"));
				transform.SetParent(base.transform.GetChild(0), worldPositionStays: false);
				_ = itemIconComparision != null;
				_selectedWidget = transform.gameObject;
				DisableItem(disable: true, changeAction: false, changeClickable: false);
			}
			else
			{
				_selectedWidget.gameObject.SetActive(value: true);
				DisableItem(disable: true, changeAction: false, changeClickable: false);
			}
		}
		else if (_selectedWidget != null)
		{
			_selectedWidget.gameObject.SetActive(value: false);
			DisableItem(disable: false, changeAction: false, changeClickable: false);
		}
	}

	public void SetLocked(bool locked)
	{
		_locked = locked;
		if (locked)
		{
			if (_lockedWidget == null)
			{
				Transform transform = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/item/ItemLockWidget"));
				transform.SetParent(base.transform.GetChild(0), worldPositionStays: false);
				_ = itemIconComparision != null;
				_lockedWidget = transform.gameObject;
				DisableItem(disable: true, changeAction: false, changeClickable: false);
			}
			else
			{
				_lockedWidget.gameObject.SetActive(value: true);
				DisableItem(disable: true, changeAction: false, changeClickable: false);
			}
		}
		else if (_lockedWidget != null)
		{
			_lockedWidget.gameObject.SetActive(value: false);
			DisableItem(disable: false, changeAction: false, changeClickable: false);
		}
	}

	public void SetNotify(bool notify)
	{
		_notify = notify;
		if (notify)
		{
			if (_notifyWidget == null)
			{
				Transform transform = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/item/ItemNotifyWidget"));
				transform.SetParent(base.transform.GetChild(0), worldPositionStays: false);
				if (_isThumbnail)
				{
					transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-11.2f, 13f);
				}
				_notifyWidget = transform.gameObject;
			}
			else
			{
				_notifyWidget.gameObject.SetActive(value: true);
			}
		}
		else if (_notifyWidget != null)
		{
			_notifyWidget.gameObject.SetActive(value: false);
		}
	}

	public void SetLongNotify(bool notify)
	{
		_notify = notify;
		if (notify)
		{
			if (_notifyWidget == null)
			{
				Transform transform = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/item/ItemNotifyLongWidget"));
				transform.SetParent(base.transform.GetChild(0), worldPositionStays: false);
				if (_isThumbnail)
				{
					transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-11.2f, 13f);
				}
				_notifyWidget = transform.gameObject;
			}
			else
			{
				_notifyWidget.gameObject.SetActive(value: true);
			}
		}
		else if (_notifyWidget != null)
		{
			_notifyWidget.gameObject.SetActive(value: false);
		}
	}

	public void ResetHidden()
	{
		_asset.color = Color.white;
		if (_swfAsset != null)
		{
			Image[] componentsInChildren = _asset.GetComponentsInChildren<Image>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = Color.white;
			}
		}
		DisableItem(disable: false, changeAction: false, changeClickable: true, AssetLoader.UrlIsSWF(_item.itemRef.icon));
		if (_unknownIcon != null)
		{
			_unknownIcon.gameObject.SetActive(value: false);
		}
		_asset.gameObject.SetActive(value: true);
	}

	public void SetHidden(bool v, bool unknown = false, bool changeClick = true)
	{
		_hidden = v;
		_unknown = unknown;
		if (!v)
		{
			return;
		}
		_asset.color = Color.black;
		if (_swfAsset != null)
		{
			Image[] componentsInChildren = _asset.GetComponentsInChildren<Image>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = Color.black;
			}
		}
		DisableItem(disable: true, unknown, unknown);
		if (unknown)
		{
			SetItemActionType(0);
			_clickable = false;
			if (_unknownIcon == null)
			{
				Transform transform = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/item/UnknownIcon"));
				transform.SetParent(base.transform.GetChild(0), worldPositionStays: false);
				_unknownIcon = transform.gameObject;
			}
			else
			{
				_unknownIcon.gameObject.SetActive(value: true);
			}
			_asset.gameObject.SetActive(value: false);
		}
	}

	public void SetupItemComparision(bool showCosmetic, bool showComparision)
	{
		if (itemIconComparision == null)
		{
			itemIconComparision = Util.GetOrAddComponent<ItemIconComparision>(base.gameObject);
		}
		_compare = showComparision;
		itemIconComparision.Setup(_item, showCosmetic, showComparision);
	}

	public void SetCompare(bool compare)
	{
		_compare = compare;
	}

	public void PlayComparison(string label)
	{
		if (itemIconComparision != null && label != "=")
		{
			itemIconComparision.PlayComparison(label);
		}
	}

	public void HideComparison()
	{
		if (itemIconComparision != null)
		{
			itemIconComparision.PlayComparison("=");
		}
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		if (!_clickable)
		{
			return;
		}
		if (_itemIconAction == null || AppInfo.IsMobile() || _itemIconAction.getType() != 0)
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
		}
		if (AppInfo.IsMobile())
		{
			CreateTooltip();
		}
		else if (_itemIconAction != null)
		{
			if (_itemIconAction.getType() == 1)
			{
				ShowEquippedText();
			}
			_itemIconAction.Execute();
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		if (_clickable && !AppInfo.IsMobile() && eventData.button == PointerEventData.InputButton.Left)
		{
			CreateTooltip();
		}
	}

	private void CreateTooltip()
	{
		if (_item != null && _item.itemRef != null)
		{
			tooltip = GameData.instance.windowGenerator.NewItemToolTipContainer(this, _characterData, _armoryEquipment, 11);
		}
	}

	private void DestroyTooltip()
	{
		tooltip.gameObject.SetActive(value: false);
		tooltip.GetComponent<ItemTooltipContainer>().HideComparisson();
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		if (!AppInfo.IsMobile() && tooltip != null)
		{
			DestroyTooltip();
		}
	}

	public void SetAlpha(float value = 1f, bool isAnimated = false)
	{
		if (GetComponent<CanvasGroup>() == null)
		{
			base.gameObject.AddComponent<CanvasGroup>();
		}
		if (isAnimated && _asset != null)
		{
			Color color = _asset.color;
			color.a = 0f;
			_asset.color = color;
		}
		GetComponent<CanvasGroup>().alpha = value;
	}

	public void ShowEquippedText()
	{
		Transform obj = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/battle/BattleTextUIFix"));
		obj.SetParent(Main.CONTAINER.GetLayerGO(13).transform, worldPositionStays: false);
		obj.GetComponent<TextMeshProUGUI>().fontSize = 10f;
		BattleTextUI component = obj.GetComponent<BattleTextUI>();
		component.LoadDetails(Language.GetString("ui_equipped"), BattleText.COLOR_YELLOW, 0.5f, 0f, base.transform.position.x, base.transform.position.y, 1f, -20f, 0f, doFlash: false);
		Canvas canvas = component.gameObject.AddComponent<Canvas>();
		canvas.overrideSorting = true;
		canvas.sortingLayerName = "UI";
		canvas.sortingOrder = 14000;
	}

	public bool GetNotify()
	{
		if (_notifyWidget != null)
		{
			return _notifyWidget.activeSelf;
		}
		return false;
	}

	public string GetComparison()
	{
		if (itemIconComparision == null || !itemIconComparision.gameObject.activeSelf)
		{
			return "";
		}
		return itemIconComparision.currentLabel;
	}

	private void updateQty()
	{
		if (_qty > 1 || _qtyShow)
		{
			setQtyText(Util.NumberFormat(_qty, abbreviate: true, shortbool: true, 10f));
		}
		else
		{
			setQtyText("");
		}
	}

	public void setQtyText(string text)
	{
		TextMeshProUGUI componentInChildren = base.transform.GetComponentInChildren<TextMeshProUGUI>();
		if (componentInChildren != null)
		{
			componentInChildren.text = text;
		}
	}

	public static ItemIcon GetEquipmentTile(bool upgrade, List<ItemIcon> tiles)
	{
		foreach (ItemIcon tile in tiles)
		{
			if (tile.qty <= 0)
			{
				continue;
			}
			switch (tile.itemRef.itemType)
			{
			case 1:
				if (!upgrade)
				{
					return tile;
				}
				if (tile.GetComparison() == "+")
				{
					return tile;
				}
				break;
			}
		}
		return null;
	}

	public static int GetEquipmentTileIndex(bool upgrade, List<ItemIcon> tiles)
	{
		for (int i = 0; i < tiles.Count; i++)
		{
			if (tiles[i].qty <= 0)
			{
				continue;
			}
			switch (tiles[i].itemRef.itemType)
			{
			case 1:
				if (!upgrade)
				{
					return i;
				}
				if (tiles[i].GetComparison() == "+")
				{
					return i;
				}
				break;
			case 16:
				D.LogError("nacho", "ADD ARMORY FEATURE");
				break;
			}
		}
		return -1;
	}

	public static List<ItemIcon> GetLowestRarityItems(List<ItemIcon> tiles)
	{
		List<ItemIcon> list = new List<ItemIcon>();
		int num = 9;
		foreach (ItemIcon tile in tiles)
		{
			num = Mathf.Min(tile.itemRef.rarity, num);
			if (num == 0)
			{
				break;
			}
		}
		foreach (ItemIcon tile2 in tiles)
		{
			if (tile2.qty > 0 && tile2.itemRef.rarity == num)
			{
				list.Add(tile2);
			}
		}
		return list;
	}

	public static ItemIcon GetAbilityChangeTile(List<ItemIcon> tiles)
	{
		EquipmentRef equipmentSlot = GameData.instance.PROJECT.character.equipment.getEquipmentSlot(0);
		List<AbilityRef> list = ((equipmentSlot != null) ? equipmentSlot.abilities : null);
		int num = list?.Count ?? 0;
		foreach (ItemIcon tile in tiles)
		{
			if (tile.qty <= 0)
			{
				continue;
			}
			switch (tile.itemRef.itemType)
			{
			case 1:
			{
				EquipmentRef equipmentRef = tile.itemRef as EquipmentRef;
				if (equipmentSlot != null && equipmentRef != null && equipmentRef.equipmentType == equipmentSlot.equipmentType && equipmentRef.abilities != null && list != null)
				{
					if (equipmentRef.abilities.Count > num)
					{
						return tile;
					}
					if (equipmentRef.abilities[0].id != list[0].id)
					{
						return tile;
					}
				}
				break;
			}
			}
		}
		return null;
	}

	public static ItemIcon GetItemTypeSubtypeTile(List<ItemIcon> tiles, int type, int subtype = -1)
	{
		foreach (ItemIcon tile in tiles)
		{
			if (tile.itemRef.itemType == type && tile.itemRef.MatchesSubtype(subtype))
			{
				return tile;
			}
		}
		return null;
	}

	public static ItemIcon GetItemTypeTile(List<ItemIcon> tiles, int type)
	{
		foreach (ItemIcon tile in tiles)
		{
			if (tile.itemRef.itemType == type)
			{
				return tile;
			}
		}
		return null;
	}

	protected override void OnDestroy()
	{
		if (tooltip != null)
		{
			tooltip.gameObject.SetActive(value: false);
			tooltip.GetComponent<ItemTooltipContainer>().HideComparisson();
		}
		base.OnDestroy();
	}
}
