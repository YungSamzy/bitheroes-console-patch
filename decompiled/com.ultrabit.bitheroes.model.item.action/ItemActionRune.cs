using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.armory.rune;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.rune;
using Sfs2X.Core;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionRune : ItemActionBase
{
	private RuneRef selectedRuneRef;

	private RuneRef exchangeRuneRef;

	public ItemActionRune(BaseModelData itemData, int type, object objParameter = null)
		: base(itemData, type, objParameter)
	{
	}

	public override void Execute()
	{
		base.Execute();
		switch (base.tooltipType)
		{
		case 18:
			if (GameData.instance.windowGenerator.GetDialogByClass(typeof(ArmoryRunesWindow)) == null)
			{
				GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_exchange_confirm_item", new string[1] { base.itemRef.coloredName }), null, null, OnRuneExchange);
			}
			break;
		case 17:
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_rune_insert_confirm", new string[1] { base.itemRef.coloredName }), null, null, OnRuneInsert);
			break;
		case 19:
			OnRuneSwitch();
			break;
		case 20:
			OnArmoryRuneSwitch();
			break;
		}
	}

	public void OnRuneInsert()
	{
		if (onPostExecuteCallback != null)
		{
			onPostExecuteCallback(itemData);
		}
		DoRuneUse(itemData.itemRef as RuneRef);
	}

	private void DoRuneUse(RuneRef runeRef)
	{
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(17), OnRuneUse);
		CharacterDALC.instance.doRuneUse(runeRef, (base.objParameter as RuneTile).slot);
	}

	private void OnRuneUse(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(17), OnRuneUse);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		ItemListWindow itemListWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(ItemListWindow)) as ItemListWindow;
		if ((bool)itemListWindow)
		{
			itemListWindow.OnClose();
		}
		RuneOptionsWindow runeOptionsWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(RuneOptionsWindow)) as RuneOptionsWindow;
		if ((bool)runeOptionsWindow)
		{
			runeOptionsWindow.OnClose();
		}
		Runes runes = Runes.fromSFSObject(sfsob);
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		List<RuneTile> list2 = null;
		if ((base.objParameter as RuneTile).runesWindow != null)
		{
			list2 = (base.objParameter as RuneTile).runesWindow.GetRuneDifferenceTiles(runes);
		}
		GameData.instance.PROJECT.character.runes = runes;
		GameData.instance.PROJECT.character.removeItems(list);
		KongregateAnalytics.checkEconomyTransaction("Rune", list, null, sfsob, "Game", 2);
		GameData.instance.audioManager.PlaySoundLink("upgradeitem");
		if ((base.objParameter as RuneTile).runesWindow != null && list2 != null)
		{
			(base.objParameter as RuneTile).runesWindow.AnimateTiles(list2);
		}
		GameData.instance.PROJECT.character.tutorial.SetState(92);
		GameData.instance.PROJECT.CheckTutorialChanges();
	}

	public void OnArmoryRuneSwitch()
	{
		if (onPostExecuteCallback != null)
		{
			onPostExecuteCallback(itemData);
		}
		DoArmoryRuneChange(base.itemRef as RuneRef);
	}

	private void DoArmoryRuneChange(RuneRef runeRef)
	{
		selectedRuneRef = runeRef;
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(49), OnArmoryRuneChange);
		CharacterDALC.instance.doArmoryRuneChange(runeRef, (base.objParameter as ArmoryRuneTile).slot);
	}

	private void OnArmoryRuneChange(BaseEvent e)
	{
		GameData.instance.main.HideLoading();
		DALCEvent obj = e as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(49), OnArmoryRuneChange);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		ItemListWindow itemListWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(ItemListWindow)) as ItemListWindow;
		if ((bool)itemListWindow)
		{
			itemListWindow.OnClose();
		}
		RuneOptionsWindow runeOptionsWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(RuneOptionsWindow)) as RuneOptionsWindow;
		if ((bool)runeOptionsWindow)
		{
			runeOptionsWindow.OnClose();
		}
		if (selectedRuneRef != null)
		{
			int slot = (base.objParameter as ArmoryRuneTile).slot;
			GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.runes.setRuneSlot(selectedRuneRef, slot);
		}
		ArmoryRunes runes = ArmoryRunes.fromSFSObject(sfsob);
		if ((base.objParameter as ArmoryRuneTile).runesWindow != null)
		{
			(base.objParameter as ArmoryRuneTile).runesWindow.GetRuneDifferenceTiles(runes);
		}
		GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.SetRunes(runes);
		GameData.instance.audioManager.PlaySoundLink("equip");
	}

	public void OnRuneSwitch()
	{
		if (onPostExecuteCallback != null)
		{
			onPostExecuteCallback(itemData);
		}
		DoRuneChange(base.itemRef as RuneRef);
	}

	private void DoRuneChange(RuneRef runeRef)
	{
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(18), OnRuneChange);
		CharacterDALC.instance.doRuneChange(runeRef, (base.objParameter as RuneTile).slot);
	}

	private void OnRuneChange(BaseEvent e)
	{
		GameData.instance.main.HideLoading();
		DALCEvent obj = e as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(18), OnRuneChange);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		ItemListWindow itemListWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(ItemListWindow)) as ItemListWindow;
		if ((bool)itemListWindow)
		{
			itemListWindow.OnClose();
		}
		RuneOptionsWindow runeOptionsWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(RuneOptionsWindow)) as RuneOptionsWindow;
		if ((bool)runeOptionsWindow)
		{
			runeOptionsWindow.OnClose();
		}
		Runes runes = Runes.fromSFSObject(sfsob);
		if ((base.objParameter as RuneTile).runesWindow != null)
		{
			(base.objParameter as RuneTile).runesWindow.GetRuneDifferenceTiles(runes);
		}
		GameData.instance.PROJECT.character.runes = runes;
		GameData.instance.audioManager.PlaySoundLink("equip");
	}

	public void OnRuneExchange()
	{
		if (onPostExecuteCallback != null)
		{
			onPostExecuteCallback(itemData);
		}
		DoRuneExchange(base.itemRef as RuneRef);
	}

	private void DoRuneExchange(RuneRef runeRef)
	{
		exchangeRuneRef = runeRef;
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(22), OnRuneExchange);
		CharacterDALC.instance.doRuneExchange(runeRef, (base.objParameter as RuneTile).slot);
	}

	private void OnRuneExchange(BaseEvent e)
	{
		GameData.instance.main.HideLoading();
		DALCEvent obj = e as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(22), OnRuneExchange);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		if (exchangeRuneRef != null)
		{
			GameData.instance.PROJECT.character.armory.RemoveRuneFromArmoryIfEquipped(exchangeRuneRef);
			exchangeRuneRef = null;
		}
		ItemListWindow itemListWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(ItemListWindow)) as ItemListWindow;
		if ((bool)itemListWindow)
		{
			itemListWindow.OnClose();
		}
		RuneOptionsWindow runeOptionsWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(RuneOptionsWindow)) as RuneOptionsWindow;
		if ((bool)runeOptionsWindow)
		{
			runeOptionsWindow.OnClose();
		}
		Runes runes = Runes.fromSFSObject(sfsob);
		List<ItemData> items = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite4"));
		GameData.instance.PROJECT.character.runes = runes;
		GameData.instance.PROJECT.character.addItems(items);
		GameData.instance.windowGenerator.ShowItems(items, compare: true, added: true);
		GameData.instance.audioManager.PlaySoundLink("exchange");
	}
}
