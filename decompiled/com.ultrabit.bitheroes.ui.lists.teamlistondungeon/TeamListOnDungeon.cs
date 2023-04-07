using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.DataHelpers;
using Com.TheFallenGames.OSA.Util.ItemDragging;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.utility;
using frame8.Logic.Misc.Other.Extensions;
using frame8.Logic.Misc.Visual.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.lists.teamlistondungeon;

public class TeamListOnDungeon : OSA<MyParams, MyListItemViewsHolder>, DraggableItem.IDragDropListener, ICancelHandler, IEventSystemHandler
{
	private DragStateManager _DragManager = new DragStateManager();

	private Canvas _Canvas;

	private RectTransform _CanvasRT;

	private DungeonUI _dungeonUI;

	public SimpleDataHelper<DungeonEntityTileModel> Data { get; private set; }

	public void InitList(DungeonUI dungeonUI)
	{
		_dungeonUI = dungeonUI;
		if (Data == null)
		{
			Data = new SimpleDataHelper<DungeonEntityTileModel>(this);
			_Canvas = GetComponentInParent<Canvas>();
			_CanvasRT = _Canvas.transform as RectTransform;
			base.Start();
		}
	}

	public void ClearList()
	{
		if (Data != null && Data.Count > 0)
		{
			Data.RemoveItems(0, Data.Count);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!base.IsInitialized || _DragManager.State != DragState.DRAGGING)
		{
			return;
		}
		if (!GameData.instance.PROJECT.gameIsPaused)
		{
			GameData.instance.PROJECT.PauseDungeon();
		}
		if (!(this.GetContentSizeToViewportRatio() < 1.0) && GetLocalPointInViewportIfWithinBounds(_DragManager.Dragged.draggableComponent.CurrentOnDragEventWorldPosition, _DragManager.Dragged.draggableComponent.CurrentPressEventCamera, out var localPoint))
		{
			float value = ConvertViewportLocalPointToViewportLongitudinalPointStart0End1(localPoint);
			value = Mathf.Clamp01(value);
			float num = _Params.maxScrollSpeedOnBoundary * base.DeltaTime;
			float minDistFromEdgeToBeginScroll = _Params.minDistFromEdgeToBeginScroll01;
			float num2 = 1f - _Params.minDistFromEdgeToBeginScroll01;
			if (value < minDistFromEdgeToBeginScroll)
			{
				ScrollByAbstractDelta(num * (minDistFromEdgeToBeginScroll - value) / _Params.minDistFromEdgeToBeginScroll01);
			}
			else if (value > num2)
			{
				ScrollByAbstractDelta((0f - num) * (value - num2) / _Params.minDistFromEdgeToBeginScroll01);
			}
		}
	}

	protected override void CollectItemsSizes(ItemCountChangeMode changeMode, int count, int indexIfInsertingOrRemoving, ItemsDescriptor itemsDesc)
	{
		base.CollectItemsSizes(changeMode, count, indexIfInsertingOrRemoving, itemsDesc);
		if (changeMode == ItemCountChangeMode.RESET && count != 0)
		{
			int num = 0;
			int num2 = num + count;
			itemsDesc.BeginChangingItemsSizes(num);
			for (int i = num; i < num2; i++)
			{
				itemsDesc[i] = Random.Range(_Params.DefaultItemSize / 3f, _Params.DefaultItemSize * 3f);
			}
			itemsDesc.EndChangingItemsSizes();
		}
	}

	protected override MyListItemViewsHolder CreateViewsHolder(int itemIndex)
	{
		MyListItemViewsHolder myListItemViewsHolder = new MyListItemViewsHolder();
		myListItemViewsHolder.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
		myListItemViewsHolder.draggableComponent.dragDropListener = this;
		return myListItemViewsHolder;
	}

	protected override void UpdateViewsHolder(MyListItemViewsHolder newOrRecycled)
	{
		DungeonEntityTileModel dungeonEntityTileModel = Data[newOrRecycled.ItemIndex];
		bool flag = _DragManager.State != 0 && dungeonEntityTileModel == _DragManager.PlaceholderModel;
		newOrRecycled.scalableViews.gameObject.SetActive(!flag);
		if (dungeonEntityTileModel.entity != null)
		{
			UpdateChanges(newOrRecycled, dungeonEntityTileModel);
		}
	}

	public void CancelTeamListDrag()
	{
		if (_DragManager != null && _DragManager.State != 0 && !(_DragManager.Dragged.draggableComponent == null))
		{
			_DragManager.Dragged.draggableComponent.CancelDragSilently();
			Object.Destroy(_DragManager.Dragged.draggableComponent.gameObject);
			_DragManager.EnterState_None();
			GameData.instance.PROJECT.ResumeDungeon();
		}
	}

	public void RefreshTile(MyListItemViewsHolder newOrRecycled)
	{
		UpdateViewsHolder(newOrRecycled);
	}

	public void UpdateChanges(MyListItemViewsHolder newOrRecycled, DungeonEntityTileModel model)
	{
		if (model.entity == null)
		{
			return;
		}
		newOrRecycled.disableColor.gameObject.SetActive(value: false);
		Animator component = newOrRecycled.healthBar.GetComponent<Animator>();
		component.speed = 0f;
		float num = (float)model.entity.healthCurrent / (float)model.entity.healthTotal;
		if (num < 0f)
		{
			num = 0f;
		}
		if (num > 1f)
		{
			num = 1f;
		}
		newOrRecycled.healthBar.GetComponent<RegularBarFill>().UpdateBar(model.entity.healthCurrent, model.entity.healthTotal);
		if (newOrRecycled.healthBar.gameObject.activeInHierarchy)
		{
			component.Play("HealthBar", 0, Mathf.Round(100f - num * 100f) * 0.01f);
		}
		component.speed = 0f;
		bool flag = model.entity.type == 1 && model.entity.characterData.isIMXG0;
		if (model.entity.IsDead())
		{
			newOrRecycled.frame.color = new Color(1f, 0f, 0f, 0.75f);
			newOrRecycled.rarityColor.color = new Color(1f, 0f, 0f, 0.75f);
		}
		else
		{
			newOrRecycled.frame.color = Color.white;
			newOrRecycled.rarityColor.color = (flag ? model.entity.characterData.nftRarityColor : Color.white);
		}
		float num2 = (float)model.entity.shieldCurrent / (float)model.entity.shieldTotal;
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		RegularBarFill component2 = newOrRecycled.shieldBar.GetComponent<RegularBarFill>();
		float num3 = VariableBook.shieldMult + GameModifier.getTypeTotal(model.entity.GetModifiers(), 74);
		float percent = (float)model.entity.shieldCurrent / (float)model.entity.shieldTotal * num3;
		component2.UpdateBarByPerc(percent);
		newOrRecycled.shieldBar.gameObject.SetActive(num2 > 0f);
		newOrRecycled.meterBar.GetComponent<RegularBarFill>().UpdateBar(model.entity.meter, VariableBook.battleMeterMax);
		if (newOrRecycled.placeholderDisplay.transform.childCount > 0)
		{
			for (int i = 0; i < newOrRecycled.placeholderDisplay.transform.childCount; i++)
			{
				Object.Destroy(newOrRecycled.placeholderDisplay.transform.GetChild(i).gameObject);
			}
		}
		int num4 = _dungeonUI.sortingLayer + newOrRecycled.ItemIndex;
		if (newOrRecycled.assetMask0 != null)
		{
			newOrRecycled.assetMask0.frontSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask0.frontSortingOrder = num4 + 1;
			newOrRecycled.assetMask0.backSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask0.backSortingOrder = num4;
		}
		switch (model.entity.type)
		{
		case 1:
		{
			CharacterDisplay characterDisplay = model.entity.characterData.toCharacterDisplay(1f, displayMount: false, null, enableLoading: false);
			characterDisplay.characterPuppet.StopAllAnimations();
			characterDisplay.transform.SetParent(newOrRecycled.placeholderDisplay.transform, worldPositionStays: false);
			characterDisplay.transform.localPosition = Vector3.zero;
			characterDisplay.SetLocalPosition(new Vector3(-2f, -63f, 0f));
			characterDisplay.characterPuppet.HideMaskedElements();
			Util.ChangeLayer(characterDisplay.transform, "UI");
			SpriteRenderer[] componentsInChildren = characterDisplay.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
			}
			SortingGroup sortingGroup = characterDisplay.gameObject.AddComponent<SortingGroup>();
			if (sortingGroup != null && sortingGroup.enabled)
			{
				sortingGroup.sortingLayerName = "UI";
				sortingGroup.sortingOrder = num4 + 1;
			}
			break;
		}
		case 3:
		{
			FamiliarRef familiarRef = FamiliarBook.Lookup(model.entity.id);
			GameObject obj = new GameObject();
			Util.ChangeLayer(obj.transform, "UI");
			obj.transform.SetParent(newOrRecycled.placeholderDisplay, worldPositionStays: false);
			SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = familiarRef.GetSpriteIcon();
			spriteRenderer.sortingLayerID = SortingLayer.NameToID("UI");
			spriteRenderer.sortingOrder = num4 + 1;
			break;
		}
		}
		bool flag2 = model.entity.healthCurrent < model.entity.healthTotal;
		bool flag3 = model.entity.consumables > 0;
		newOrRecycled.consumableIcon.gameObject.SetActive(!flag3 || flag2);
		if (flag3)
		{
			newOrRecycled.root.GetComponent<DotsHandler>().ReplaceDot(newOrRecycled.consumableIcon, 1);
		}
		else
		{
			newOrRecycled.root.GetComponent<DotsHandler>().ReplaceDot(newOrRecycled.consumableIcon, 0);
		}
		newOrRecycled.root.GetComponent<HoverImages>().ForceStart();
		if (model.enabled)
		{
			newOrRecycled.root.GetComponent<HoverImages>().active = true;
			newOrRecycled.disableColor.gameObject.SetActive(value: false);
			newOrRecycled.root.GetComponent<DraggableItem>().enabled = true;
			newOrRecycled.root.GetComponent<DraggableItem>().clickCallBack = delegate
			{
				OnTileClick(model);
			};
		}
		else
		{
			newOrRecycled.root.GetComponent<HoverImages>().active = false;
			newOrRecycled.disableColor.gameObject.SetActive(value: true);
			newOrRecycled.root.GetComponent<DraggableItem>().enabled = false;
			newOrRecycled.root.GetComponent<DraggableItem>().clickCallBack = null;
		}
	}

	private void OnTileClick(DungeonEntityTileModel model = null)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (model.entity.consumables <= 0)
		{
			GameData.instance.windowGenerator.ShowErrorCode(31);
		}
		else if (model.entity.IsDead())
		{
			ItemSelectWindow component = GameData.instance.windowGenerator.NewItemSelectWindow().GetComponent<ItemSelectWindow>();
			component.SELECT.AddListener(delegate(object e)
			{
				OnItemSelected(e, model);
			});
			component.CLOSE.AddListener(delegate(object e)
			{
				OnItemSelectClosed(e, model);
			});
			component.LoadDetails(ConsumableBook.GetConsumablesByTypes(new int[2] { 8, 3 }), "Dungeon", model.entity);
		}
		else if (model.entity.healthCurrent < model.entity.healthTotal)
		{
			ItemSelectWindow component2 = GameData.instance.windowGenerator.NewItemSelectWindow().GetComponent<ItemSelectWindow>();
			component2.SELECT.AddListener(delegate(object e)
			{
				OnItemSelected(e, model);
			});
			component2.CLOSE.AddListener(delegate(object e)
			{
				OnItemSelectClosed(e, model);
			});
			component2.LoadDetails(ConsumableBook.GetConsumablesByTypes(new int[2] { 8, 2 }), "Dungeon", model.entity);
		}
		else if (model.entity.shieldCurrent < model.entity.shieldTotal && GameData.instance.PROJECT.character.inventory.getItemTypeQty(4, 8) > 0)
		{
			ItemSelectWindow component3 = GameData.instance.windowGenerator.NewItemSelectWindow().GetComponent<ItemSelectWindow>();
			component3.SELECT.AddListener(delegate(object e)
			{
				OnItemSelected(e, model);
			});
			component3.CLOSE.AddListener(delegate(object e)
			{
				OnItemSelectClosed(e, model);
			});
			component3.LoadDetails(ConsumableBook.GetConsumablesByTypes(new int[1] { 8 }), "Battle", model.entity);
		}
		else
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("battle_entity_no_potions_needed"));
		}
	}

	private void OnItemSelected(object e, DungeonEntityTileModel model)
	{
		ItemSelectWindow itemSelectWindow = e as ItemSelectWindow;
		itemSelectWindow.SELECT.AddListener(delegate(object f)
		{
			OnItemSelected(f, model);
		});
		itemSelectWindow.CLOSE.AddListener(delegate(object g)
		{
			OnItemSelectClosed(g, model);
		});
		model.dungeonUI.UseConsumable(itemSelectWindow.selectedItem, model.entity);
	}

	private void OnItemSelectClosed(object e, DungeonEntityTileModel model)
	{
		ItemSelectWindow obj = e as ItemSelectWindow;
		obj.SELECT.AddListener(delegate(object f)
		{
			OnItemSelected(f, model);
		});
		obj.CLOSE.AddListener(delegate(object g)
		{
			OnItemSelectClosed(g, model);
		});
	}

	bool DraggableItem.IDragDropListener.OnPrepareToDragItem(DraggableItem item)
	{
		MyListItemViewsHolder itemViewsHolderIfVisible = GetItemViewsHolderIfVisible(item.RT);
		if (itemViewsHolderIfVisible == null)
		{
			return false;
		}
		int itemIndex = itemViewsHolderIfVisible.ItemIndex;
		DungeonEntityTileModel modelOfDragged = Data[itemIndex];
		Data.List.RemoveAt(itemIndex);
		RemoveItemWithViewsHolder(itemViewsHolderIfVisible, stealViewsHolderInsteadOfRecycle: true, contentPanelEndEdgeStationary: false);
		UpdateScaleOfVisibleItems(itemViewsHolderIfVisible);
		EmptyModel placeholderModel = new EmptyModel();
		_DragManager.EnterState_PreparingForDrag(itemViewsHolderIfVisible, modelOfDragged, placeholderModel);
		InsertPlaceholderAtNewIndex(itemIndex);
		return true;
	}

	void DraggableItem.IDragDropListener.OnBeginDragItem(PointerEventData eventData)
	{
		_DragManager.EnterState_Dragging(eventData);
	}

	void DraggableItem.IDragDropListener.OnDraggedItem(PointerEventData eventData)
	{
		bool isPointInViewport;
		MyListItemViewsHolder closestVHAtScreenPoint = GetClosestVHAtScreenPoint(eventData, out isPointInViewport);
		UpdateScaleOfVisibleItems(closestVHAtScreenPoint);
		if (closestVHAtScreenPoint != null && closestVHAtScreenPoint.ItemIndex != _DragManager.PlaceholderModel.placeholderForIndex)
		{
			Data.RemoveOne(_DragManager.PlaceholderModel.placeholderForIndex);
			InsertPlaceholderAtNewIndex(closestVHAtScreenPoint.ItemIndex);
		}
	}

	DraggableItem.OrphanedItemBundle DraggableItem.IDragDropListener.OnDroppedItem(PointerEventData eventData)
	{
		return DropDraggedVHAndEnterNoneState(eventData);
	}

	bool DraggableItem.IDragDropListener.OnDroppedExternalItem(PointerEventData eventData, DraggableItem orphanedItemWithBundle)
	{
		return TryGrabOrphanedItemVH(eventData, orphanedItemWithBundle);
	}

	void ICancelHandler.OnCancel(BaseEventData eventData)
	{
		if (_DragManager.State != 0)
		{
			_DragManager.Dragged.draggableComponent.CancelDragSilently();
			DropDraggedVHAndEnterNoneState(null);
		}
	}

	private void InsertPlaceholderAtNewIndex(int index)
	{
		_DragManager.PlaceholderModel.placeholderForIndex = index;
		Data.InsertOne(index, _DragManager.PlaceholderModel, freezeEndEdge: true);
		RequestChangeItemSizeAndUpdateLayout(index, _DragManager.Dragged.root.rect.height, itemEndEdgeStationary: false, computeVisibility: true, correctItemPosition: true);
	}

	private void UpdateScaleOfVisibleItems(MyListItemViewsHolder vhToRotate)
	{
	}

	private bool TryGrabOrphanedItemVH(PointerEventData eventData, DraggableItem orphanedItemWithBundle)
	{
		bool isPointInViewport;
		MyListItemViewsHolder closestVHAtScreenPoint = GetClosestVHAtScreenPoint(eventData, out isPointInViewport);
		if (!isPointInViewport)
		{
			return false;
		}
		orphanedItemWithBundle.dragDropListener = this;
		int num = 0;
		num = closestVHAtScreenPoint?.ItemIndex ?? 0;
		MyListItemViewsHolder myListItemViewsHolder = orphanedItemWithBundle.OrphanedBundle.views as MyListItemViewsHolder;
		DungeonEntityTileModel item = orphanedItemWithBundle.OrphanedBundle.model as DungeonEntityTileModel;
		Data.List.Insert(num, item);
		float height = myListItemViewsHolder.root.rect.height;
		InsertItemWithViewsHolder(myListItemViewsHolder, num, contentPanelEndEdgeStationary: false);
		RequestChangeItemSizeAndUpdateLayout(num, height);
		return true;
	}

	private DraggableItem.OrphanedItemBundle DropDraggedVHAndEnterNoneState(PointerEventData eventData)
	{
		MyListItemViewsHolder dragged = _DragManager.Dragged;
		DungeonEntityTileModel modelOfDragged = _DragManager.ModelOfDragged;
		int placeholderForIndex = _DragManager.PlaceholderModel.placeholderForIndex;
		int num;
		if (eventData == null)
		{
			num = dragged.ItemIndex;
			Data.RemoveOne(placeholderForIndex);
			placeholderForIndex = -1;
		}
		else
		{
			GetClosestVHAtScreenPoint(eventData, out var _);
			num = placeholderForIndex;
			placeholderForIndex++;
		}
		Data.List.Insert(num, modelOfDragged);
		_DragManager.EnterState_None();
		float height = dragged.root.rect.height;
		InsertItemWithViewsHolder(dragged, num, contentPanelEndEdgeStationary: false);
		RequestChangeItemSizeAndUpdateLayout(num, height);
		UpdateScaleOfVisibleItems(null);
		if (placeholderForIndex != -1)
		{
			Data.RemoveOne(placeholderForIndex);
		}
		List<int> list = new List<int>();
		for (int i = 0; i < Data.Count; i++)
		{
			if (Data[i].entity != null && !Data[i].extraFix)
			{
				list.Add(Data[i].entity.index);
			}
		}
		if (GameData.instance.PROJECT.gameIsPaused)
		{
			GameData.instance.PROJECT.ResumeDungeon();
		}
		modelOfDragged.dungeonUI.UpdateEntitiesOrder(list.ToArray());
		return null;
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		D.Log("mari", "-----------------RECICLE START-----------------");
		if (inRecycleBinOrVisible.root.GetComponent<HoverImages>() != null)
		{
			inRecycleBinOrVisible.root.GetComponent<HoverImages>().OnExit();
		}
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<DungeonEntityTileModel> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<DungeonEntityTileModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		DungeonEntityTileModel[] array = new DungeonEntityTileModel[count];
		for (int i = 0; i < count; i++)
		{
			DungeonEntityTileModel dungeonEntityTileModel = new DungeonEntityTileModel
			{
				color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f)
			};
			array[i] = dungeonEntityTileModel;
		}
		OnDataRetrieved(array);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		DungeonEntityTileModel[] newItems = new DungeonEntityTileModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(DungeonEntityTileModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}

	private MyListItemViewsHolder GetClosestVHAtScreenPoint(PointerEventData eventData, out bool isPointInViewport)
	{
		isPointInViewport = false;
		if (!GetLocalPointInViewportIfWithinBounds(eventData.position, eventData.pressEventCamera, out var localPoint))
		{
			return null;
		}
		isPointInViewport = true;
		float distance;
		MyListItemViewsHolder viewsHolderClosestToViewportPoint = GetViewsHolderClosestToViewportPoint(_Canvas, _CanvasRT, localPoint, 0f, out distance);
		if (viewsHolderClosestToViewportPoint == null)
		{
			return null;
		}
		return viewsHolderClosestToViewportPoint;
	}

	private bool GetLocalPointInViewport(Vector2 screenPoint, Camera camera, out Vector2 localPoint)
	{
		return RectTransformUtility.ScreenPointToLocalPointInRectangle(_Params.Viewport, screenPoint, camera, out localPoint);
	}

	private bool GetLocalPointInViewportIfWithinBounds(Vector2 screenPoint, Camera camera, out Vector2 localPoint)
	{
		if (GetLocalPointInViewport(screenPoint, camera, out localPoint))
		{
			return _Params.Viewport.IsLocalPointInRect(localPoint);
		}
		return false;
	}
}
