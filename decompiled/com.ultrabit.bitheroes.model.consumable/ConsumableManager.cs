using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.item;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.consumable;

public class ConsumableManager
{
	private static ConsumableManager _instance;

	private ItemData itemData;

	private ConsumableRef consumableRef;

	private UnityAction onUseConsumableCallback;

	private ItemListWindow listWindow;

	private bool _forceConsume;

	public UnityEvent OnAdgorUpdate = new UnityEvent();

	public static ConsumableManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ConsumableManager();
			}
			return _instance;
		}
	}

	public void SetupConsumable(ConsumableRef consumableRef, int qty, bool forceConsume = false)
	{
		this.consumableRef = consumableRef;
		_forceConsume = forceConsume;
		SetupConsumable(new ItemData(consumableRef, qty));
	}

	public void SetupConsumable(ItemData itemData)
	{
		this.itemData = itemData;
		consumableRef = itemData.itemRef as ConsumableRef;
	}

	public void ProcessUse()
	{
		List<string> list = new List<string>();
		if (consumableRef.consumableType == 4 && itemData.qty > 1 && !new List<int> { 109, 110, 111, 112, 273 }.Contains(itemData.itemRef.id))
		{
			DoUseConsumableSelectQty();
			return;
		}
		foreach (ConsumableModifierData consumableModifiersWithMatchingModifier in GameData.instance.PROJECT.character.getConsumableModifiersWithMatchingModifiers(consumableRef.modifiers))
		{
			if (consumableModifiersWithMatchingModifier.isActive())
			{
				list.Add(consumableModifiersWithMatchingModifier.consumableRef.coloredName);
			}
		}
		string text = ((list.Count > 0) ? Language.GetString("ui_use_consumable_replace_confirm", new string[2]
		{
			itemData.itemRef.coloredName,
			Util.FormatStrings(list)
		}) : Language.GetString("ui_use_consumable_confirm", new string[1] { itemData.itemRef.coloredName }));
		DoUseConsumableConfirm(text, 1);
	}

	public void DoUseConsumableSelectQty()
	{
		GameData.instance.windowGenerator.NewItemUseWindow(itemData);
	}

	public void DoUseConsumableConfirm(string text = null, int qty = -1)
	{
		if (!_forceConsume)
		{
			if (qty < 0)
			{
				qty = itemData.qty;
			}
			if (text == null)
			{
				text = ((itemData.qty <= 1) ? Language.GetString("ui_use_consumable_confirm", new string[1] { itemData.itemRef.coloredName }) : Language.GetString("ui_use_consumable_multiple_confirm", new string[2]
				{
					Language.GetString("ui_number_multiplier", new string[1] { Util.NumberFormat(itemData.qty) }),
					itemData.itemRef.coloredName
				}));
			}
			GameData.instance.windowGenerator.NewPromptMessageWindow(ItemRef.GetItemName(itemData.itemRef.itemType), text, null, null, delegate
			{
				DoUseConsumable(qty);
			});
		}
		else
		{
			DoUseConsumable(qty);
		}
	}

	public void DoUseConsumable(int qty = 0, UnityAction onUseConsumableCallback = null)
	{
		this.onUseConsumableCallback = onUseConsumableCallback;
		if (qty == 0)
		{
			qty = itemData.qty;
		}
		ConsumableRef consumableRef = this.consumableRef;
		if (GameData.instance.PROJECT.character != null)
		{
			if (consumableRef.consumableType == 10)
			{
				GameData.instance.windowGenerator.NewCharacterHerotagChangeWindow();
				return;
			}
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(7), OnUseConsumable);
			CharacterDALC.instance.doUseConsumable(consumableRef, qty);
		}
	}

	private void OnUseConsumable(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(7), OnUseConsumable);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			ItemData itemData = ItemData.fromSFSObject(sfsob);
			GameData.instance.PROJECT.character.removeItem(itemData);
			if (itemData.qty > 0)
			{
				ItemListWindow itemListWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(ItemListWindow)) as ItemListWindow;
				if (itemListWindow != null)
				{
					itemListWindow.RemoveItem(itemData);
				}
			}
			ItemUseWindow itemUseWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(ItemUseWindow)) as ItemUseWindow;
			if (itemUseWindow != null)
			{
				itemUseWindow.OnClose();
			}
			ConsumableRef consumableRef = ConsumableBook.Lookup(itemData.itemRef.id);
			switch (consumableRef.consumableType)
			{
			case 4:
			{
				List<ItemData> list = ItemData.listFromSFSObject(sfsob);
				GameData.instance.PROJECT.character.addItems(list);
				KongregateAnalytics.checkEconomyTransaction("Use Consumable", null, list, sfsob, "Game", 2);
				listWindow = GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true);
				listWindow.onCloseEvent.AddListener(onUseConsumableLootClosed);
				break;
			}
			case 5:
			{
				List<ConsumableModifierData> consumableModifiers = ConsumableModifierData.listFromSFSObject(sfsob);
				GameData.instance.PROJECT.character.consumableModifiers = consumableModifiers;
				bool flag = false;
				if (VariableBook.excludedBoostWindow != null)
				{
					foreach (ConsumableRef item in VariableBook.excludedBoostWindow)
					{
						if (item.id == consumableRef.id)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					GameData.instance.windowGenerator.NewItemUseSuccessWindow(consumableRef);
				}
				break;
			}
			case 6:
			{
				int @int = sfsob.GetInt("cha22");
				GameData.instance.PROJECT.character.skinColorID = @int;
				break;
			}
			case 7:
				GameData.instance.PROJECT.character.points = sfsob.GetInt("cha19");
				GameData.instance.PROJECT.character.power = sfsob.GetInt("cha6");
				GameData.instance.PROJECT.character.stamina = sfsob.GetInt("cha7");
				GameData.instance.PROJECT.character.agility = sfsob.GetInt("cha8");
				GameData.instance.windowGenerator.NewCharacterStatWindow();
				break;
			case 9:
			{
				List<ItemData> items = ItemData.listFromSFSObject(sfsob);
				GameData.instance.PROJECT.character.addItems(items);
				GameData.instance.windowGenerator.NewCharacterCustomizeWindow();
				break;
			}
			case 15:
			{
				List<ConsumableModifierData> consumableModifiers = ConsumableModifierData.listFromSFSObject(sfsob);
				GameData.instance.PROJECT.character.consumableModifiers = consumableModifiers;
				OnAdgorUpdate?.Invoke();
				break;
			}
			}
		}
		if (onUseConsumableCallback != null)
		{
			onUseConsumableCallback();
		}
	}

	private void onUseConsumableLootClosed()
	{
		if (listWindow != null)
		{
			listWindow.onCloseEvent.RemoveListener(onUseConsumableLootClosed);
			listWindow = null;
			onUseConsumableCallback?.Invoke();
		}
	}
}
