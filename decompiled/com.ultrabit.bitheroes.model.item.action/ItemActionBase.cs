using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionBase
{
	public const int TYPE_NONE = 0;

	public const int TYPE_EQUIP = 1;

	public const int TYPE_UNEQUIP = 2;

	public const int TYPE_CHANGE = 3;

	public const int TYPE_BUY = 4;

	public const int TYPE_SELL = 5;

	public const int TYPE_USE = 6;

	public const int TYPE_VIEW = 7;

	public const int TYPE_EXCHANGE = 8;

	public const int TYPE_UPGRADE = 9;

	public const int TYPE_SELECT = 10;

	public const int TYPE_DESELECT = 11;

	public const int TYPE_REFORGE = 12;

	public const int TYPE_RELEASE = 13;

	public const int TYPE_IDENTIFY = 14;

	public const int TYPE_TRADE = 15;

	public const int TYPE_SUMMON = 16;

	public const int TYPE_RUNE_INSERT = 17;

	public const int TYPE_RUNE_EXCHANGE = 18;

	public const int TYPE_RUNE_SWITCH = 19;

	public const int TYPE_RUNE_ARMORY_SWITCH = 20;

	private static Dictionary<int, string> TYPE_NAMES = new Dictionary<int, string>
	{
		[0] = "ui_question_mark",
		[1] = "ui_equip",
		[2] = "ui_unequip",
		[3] = "ui_change",
		[4] = "ui_buy",
		[5] = "ui_sell",
		[6] = "ui_use",
		[7] = "ui_view",
		[8] = "ui_exchange",
		[9] = "ui_upgrade",
		[10] = "ui_select",
		[11] = "ui_deselect",
		[12] = "ui_reforge",
		[13] = "ui_release",
		[14] = "ui_identify",
		[15] = "ui_craft",
		[16] = "ui_summon",
		[17] = "ui_select",
		[18] = "ui_select",
		[19] = "ui_select",
		[20] = "ui_select"
	};

	protected int type;

	protected int forceType = -1;

	protected BaseModelData itemData;

	protected object _objParameter;

	protected string _altName;

	protected ItemIcon _itemIcon;

	public UnityAction<BaseModelData> onPostExecuteCallback;

	public int tooltipType
	{
		get
		{
			if (forceType == -1)
			{
				return type;
			}
			return forceType;
		}
	}

	protected ItemRef itemRef => itemData.itemRef;

	protected int itemType => itemData.itemRef.itemType;

	protected object objParameter => _objParameter;

	public ItemIcon itemIcon => _itemIcon;

	public void SetTooltipType(int pType)
	{
		forceType = pType;
	}

	public ItemActionBase(BaseModelData itemData, int type, object objParameter = null, string altName = null, ItemIcon itemIcon = null)
	{
		this.type = type;
		this.itemData = itemData;
		_objParameter = objParameter;
		_altName = altName;
		_itemIcon = itemIcon;
	}

	public string getTypeName()
	{
		if (_altName != null)
		{
			return _altName;
		}
		if (forceType == -1)
		{
			return Language.GetString(TYPE_NAMES[type]);
		}
		return Language.GetString(TYPE_NAMES[forceType]);
	}

	public int getType()
	{
		return type;
	}

	public string getTypeDesc()
	{
		if (tooltipType == 0)
		{
			return null;
		}
		return Language.GetString("ui_item_tooltip_desc", new string[1] { getTypeName() });
	}

	public virtual void Execute()
	{
	}

	protected void OnExecuteItemGenericAction()
	{
		switch (itemData.itemRef.itemType)
		{
		case 4:
			ConsumableManager.instance.SetupConsumable(itemData as ItemData);
			ConsumableManager.instance.DoUseConsumable();
			break;
		case 8:
		case 17:
			DoMountUse(itemData.itemRef as MountRef);
			break;
		case 11:
			DoEnchantUse(itemData.itemRef as EnchantRef);
			break;
		case 15:
			DoAugmentUse(itemData.itemRef as AugmentRef);
			break;
		}
	}

	public void DoMountUse(MountRef mountRef)
	{
		if (GameData.instance.PROJECT.character != null)
		{
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(37), OnMountUse);
			CharacterDALC.instance.doMountUse(mountRef);
		}
	}

	private void OnMountUse(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(37), OnMountUse);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			D.LogError(string.Format("{0}::OnMountUse::{1}", GetType(), "err0"));
			return;
		}
		MountData mountData = MountData.fromSFSObject(sfsob);
		ItemData itemData = ItemData.fromSFSObject(sfsob);
		GameData.instance.PROJECT.character.mounts.addMount(mountData);
		GameData.instance.PROJECT.character.removeItem(itemData);
		if (itemData.qty > 0)
		{
			ItemListWindow itemListWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(ItemListWindow)) as ItemListWindow;
			if ((bool)itemListWindow)
			{
				itemListWindow.RemoveItem(itemData);
			}
		}
		GameData.instance.audioManager.PlaySoundLink("exchange");
		GameData.instance.windowGenerator.NewMountWindow(mountData, GameData.instance.PROJECT.character.tier);
		if (onPostExecuteCallback != null)
		{
			onPostExecuteCallback(itemData);
		}
	}

	public void DoEnchantUse(EnchantRef enchantRef)
	{
		if (GameData.instance.PROJECT.character != null)
		{
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(29), OnEnchantUse);
			CharacterDALC.instance.doEnchantUse(enchantRef);
		}
	}

	private void OnEnchantUse(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(29), OnEnchantUse);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		EnchantData enchantData = EnchantData.fromSFSObject(sfsob);
		ItemData itemData = ItemData.fromSFSObject(sfsob);
		GameData.instance.PROJECT.character.enchants.addEnchant(enchantData);
		GameData.instance.PROJECT.character.removeItem(itemData);
		if (itemData.qty > 0)
		{
			ItemListWindow itemListWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(ItemListWindow)) as ItemListWindow;
			if ((bool)itemListWindow)
			{
				itemListWindow.RemoveItem(itemData);
			}
		}
		GameData.instance.audioManager.PlaySoundLink("exchange");
		GameData.instance.windowGenerator.NewEnchantWindow(enchantData);
	}

	public void DoAugmentUse(AugmentRef augmentRef)
	{
		if (GameData.instance.PROJECT.character != null)
		{
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(44), OnAugmentUse);
			CharacterDALC.instance.doAugmentUse(augmentRef);
		}
	}

	private void OnAugmentUse(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(44), OnAugmentUse);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		AugmentData augmentData = AugmentData.fromSFSObject(sfsob);
		ItemData itemData = ItemData.fromSFSObject(sfsob);
		GameData.instance.PROJECT.character.augments.addAugment(augmentData);
		GameData.instance.PROJECT.character.removeItem(itemData);
		if (itemData.qty > 0)
		{
			ItemListWindow itemListWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(ItemListWindow)) as ItemListWindow;
			if ((bool)itemListWindow)
			{
				itemListWindow.RemoveItem(itemData);
			}
		}
		GameData.instance.audioManager.PlaySoundLink("exchange");
		GameData.instance.windowGenerator.NewAugmentWindow(augmentData);
		if (onPostExecuteCallback != null)
		{
			onPostExecuteCallback(itemData);
		}
	}
}
