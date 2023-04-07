using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.lists.teamlistondungeon;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.dungeon;

public class DungeonUI : WindowsMain
{
	public TeamListOnDungeon dungeonEntitiesList;

	public LoadingOverlay _loadingOverlay;

	[SerializeField]
	private Image _exitBtnDungeonDoorClosed;

	[SerializeField]
	private Image _exitBtnDungeonDoorOpened;

	[SerializeField]
	private Sprite _exitRedButtonUp;

	[SerializeField]
	private Sprite _exitRedButtonDown;

	[SerializeField]
	private Sprite _exitGreenButtonUp;

	[SerializeField]
	private Sprite _exitGreenButtonDown;

	private Dungeon _dungeon;

	private bool lastFlag = true;

	private bool addedListeners;

	public override void Start()
	{
		base.Start();
	}

	public void DebugAuto()
	{
	}

	public void LoadDetails(Dungeon dungeon)
	{
		Disable();
		_dungeon = dungeon;
		_loadingOverlay.Hide();
		dungeonEntitiesList.InitList(this);
		CreateWindow(closeWord: false, "", scroll: false);
		Enable();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		foreach (MyListItemViewsHolder visibleItem in dungeonEntitiesList._VisibleItems)
		{
			dungeonEntitiesList.RefreshTile(visibleItem);
		}
	}

	public void SetObjects(bool flag)
	{
		lastFlag = flag;
		Util.SetButton(closeBtn, flag);
		Util.SetRenderers(closeBtn.gameObject, flag, checkDisabledChildren: true);
		if (flag)
		{
			_loadingOverlay.Hide();
		}
		else
		{
			_loadingOverlay.Show();
		}
		if (base.gameObject.activeSelf)
		{
			CreateEntityTiles();
		}
	}

	public void CreateEntityTiles()
	{
		dungeonEntitiesList.ClearList();
		List<DungeonEntityTileModel> list = new List<DungeonEntityTileModel>();
		if (_dungeon.focus != null && _dungeon.focus is DungeonPlayer)
		{
			DungeonPlayer dungeonPlayer = _dungeon.focus as DungeonPlayer;
			foreach (DungeonEntity entity in dungeonPlayer.entities)
			{
				if (!addedListeners)
				{
					entity.AddListener("ENTITY_HEALTH", delegate
					{
						UpdateEntityChanges();
					});
					entity.AddListener("ENTITY_SHIELD", delegate
					{
						UpdateEntityChanges();
					});
					entity.AddListener("ENTITY_UPDATE", delegate
					{
						UpdateEntityChanges();
					});
					entity.AddListener("ENTITY_METER", delegate
					{
						UpdateEntityChanges();
					});
					entity.AddListener("CONSUMABLE_CHANGE", delegate
					{
						UpdateEntityChanges();
					});
				}
				list.Add(new DungeonEntityTileModel
				{
					entity = entity,
					draggable = (dungeonPlayer.entities.Count > 1),
					enabled = lastFlag,
					dungeonUI = this
				});
			}
			addedListeners = true;
		}
		dungeonEntitiesList.Data.InsertItems(0, list);
	}

	public void UpdateEntityTiles()
	{
		CreateEntityTiles();
	}

	private void UpdateEntityChanges()
	{
		if (base.gameObject.activeSelf)
		{
			for (int i = 0; i < dungeonEntitiesList.Data.Count; i++)
			{
				dungeonEntitiesList.UpdateChanges(dungeonEntitiesList.GetItemViewsHolderIfVisible(i), dungeonEntitiesList.Data[i]);
			}
		}
	}

	public void UpdateEntitiesOrder(int[] order)
	{
		_dungeon.extension.DoEntityOrder(order);
	}

	public void UseConsumable(ItemRef selectedItem, DungeonEntity entity)
	{
		_dungeon.extension.DoUseConsumable(selectedItem.id, entity.index);
	}

	public void UpdateExitBtn()
	{
		if (_dungeon.extension.complete)
		{
			closeBtn.image.sprite = _exitGreenButtonUp;
			SpriteState spriteState = closeBtn.spriteState;
			spriteState.pressedSprite = _exitGreenButtonDown;
			closeBtn.spriteState = spriteState;
		}
		else
		{
			closeBtn.image.sprite = _exitRedButtonUp;
			SpriteState spriteState2 = closeBtn.spriteState;
			spriteState2.pressedSprite = _exitRedButtonDown;
			closeBtn.spriteState = spriteState2;
		}
	}

	public override void OnClose()
	{
		DoExitConfirm();
	}

	public override void CloseWithoutConfirmation(bool avoid)
	{
		base.CloseWithoutConfirmation(avoid: true);
	}

	public bool DoExitConfirm()
	{
		if (!closeBtn.interactable || !closeBtn.enabled)
		{
			return false;
		}
		GameData.instance.PROJECT.PauseDungeon();
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_exit"), Language.GetString("dungeon_exit_confirm"), null, null, delegate
		{
			CheckCompletion();
		}, delegate
		{
			GameData.instance.PROJECT.ResumeDungeon();
		});
		return true;
	}

	private void CheckCompletion()
	{
		if (_dungeon.extension.complete)
		{
			_dungeon.extension.ShowDungeonCompleteWindow();
			return;
		}
		Transform obj = GameData.instance.windowGenerator.NewDefeatableVictoryWindow();
		List<ItemData> items = ((GameData.instance.PROJECT.dungeon != null) ? GameData.instance.PROJECT.character.dungeonLootItems : null);
		obj.GetComponent<DefeatableVictoryWindow>().ON_CLOSE.AddListener(delegate
		{
			_dungeon.extension.DoPlayerExit();
		});
		obj.GetComponent<DefeatableVictoryWindow>().LoadDetails(0, isVictorious: false, 0L, 0, items);
	}

	public void ActiveUI(bool active)
	{
	}

	public MyListItemViewsHolder GetEntityTile(int index)
	{
		for (int i = 0; i < dungeonEntitiesList.Data.Count; i++)
		{
			if (dungeonEntitiesList.Data[i].entity.index == index)
			{
				return dungeonEntitiesList.GetItemViewsHolderIfVisible(i);
			}
		}
		return null;
	}

	public BattleTextUI AddTileText(MyListItemViewsHolder tile)
	{
		Transform obj = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/battle/BattleTextUIFix"));
		obj.SetParent(base.transform, worldPositionStays: false);
		obj.GetComponent<TextMeshProUGUI>().fontSize = 10f;
		return obj.GetComponent<BattleTextUI>();
	}

	public override void DoDestroy()
	{
		if (_dungeon.focus != null && _dungeon.focus is DungeonPlayer)
		{
			foreach (DungeonEntity entity in (_dungeon.focus as DungeonPlayer).entities)
			{
				entity.RemoveListener("ENTITY_HEALTH", delegate
				{
					UpdateEntityChanges();
				});
				entity.RemoveListener("ENTITY_SHIELD", delegate
				{
					UpdateEntityChanges();
				});
				entity.RemoveListener("ENTITY_UPDATE", delegate
				{
					UpdateEntityChanges();
				});
				entity.RemoveListener("ENTITY_METER", delegate
				{
					UpdateEntityChanges();
				});
				entity.RemoveListener("CONSUMABLE_CHANGE", delegate
				{
					UpdateEntityChanges();
				});
			}
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}

	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.KeypadMinus) || _dungeon.focus == null)
		{
			return;
		}
		foreach (DungeonEntity entity in (_dungeon.focus as DungeonPlayer).entities)
		{
			if (entity != null && entity.type == 3)
			{
				entity.SetHealth(entity.healthCurrent - 10, entity.healthTotal);
			}
		}
	}
}
