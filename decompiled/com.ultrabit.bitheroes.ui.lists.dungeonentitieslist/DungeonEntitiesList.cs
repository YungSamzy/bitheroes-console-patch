using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.DataHelpers;
using Com.TheFallenGames.OSA.Util.ItemDragging;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.ui.team;
using frame8.Logic.Misc.Other.Extensions;
using frame8.Logic.Misc.Visual.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.ui.lists.dungeonentitieslist;

public class DungeonEntitiesList : OSA<MyParams, MyViewsHolder>, DraggableItem.IDragDropListener, ICancelHandler, IEventSystemHandler
{
	private DragStateManager _DragManager = new DragStateManager();

	private Canvas _Canvas;

	private RectTransform _CanvasRT;

	public int MaxPlayers = 5;

	private UnityAction<int> onAddButtonClicked;

	private UnityAction onRemoveButtonClicked;

	private TeamWindow teamWindow;

	private int highestLevel;

	private int lowestLevel;

	private int highestPower;

	private int lowestPower;

	private int highestStamina;

	private int lowestStamina;

	private int highestAgility;

	private int lowestAgility;

	public SimpleDataHelper<DungeonEntityItem> Data { get; private set; }

	public void SetReferenceStats(int highestLevel, int lowestLevel, int highestPower, int lowestPower, int highestStamina, int lowestStamina, int highestAgility, int lowestAgility)
	{
		this.highestLevel = highestLevel;
		this.lowestLevel = lowestLevel;
		this.highestPower = highestPower;
		this.lowestPower = lowestPower;
		this.highestStamina = highestStamina;
		this.highestStamina = highestStamina;
		this.highestAgility = highestAgility;
		this.lowestAgility = lowestAgility;
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<DungeonEntityItem>(this);
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

	protected override void Start()
	{
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	private void SwapUp(DraggableItem item)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		MyViewsHolder itemViewsHolderIfVisible = GetItemViewsHolderIfVisible(item.RT);
		if (itemViewsHolderIfVisible != null && itemViewsHolderIfVisible.ItemIndex > 0)
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex - 1);
			for (int i = 0; i < _VisibleItemsCount; i++)
			{
				UpdateViewsHolder(_VisibleItems[i]);
			}
		}
	}

	private void SwapDown(DraggableItem item)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		MyViewsHolder itemViewsHolderIfVisible = GetItemViewsHolderIfVisible(item.RT);
		if (itemViewsHolderIfVisible != null && itemViewsHolderIfVisible.ItemIndex < Data.Count - 1)
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex + 1);
			for (int i = 0; i < _VisibleItemsCount; i++)
			{
				UpdateViewsHolder(_VisibleItems[i]);
			}
		}
	}

	private void RetrieveDataAndUpdate(int count)
	{
	}

	private void OnDataRetrieved(DungeonEntityItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}

	protected override void Update()
	{
		base.Update();
		if (base.IsInitialized && _DragManager.State == DragState.DRAGGING && !(this.GetContentSizeToViewportRatio() < 1.0) && GetLocalPointInViewportIfWithinBounds(_DragManager.Dragged.draggableComponent.CurrentOnDragEventWorldPosition, _DragManager.Dragged.draggableComponent.CurrentPressEventCamera, out var localPoint))
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

	protected override MyViewsHolder CreateViewsHolder(int itemIndex)
	{
		MyViewsHolder myViewsHolder = new MyViewsHolder();
		myViewsHolder.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
		myViewsHolder.draggableComponent.dragDropListener = this;
		return myViewsHolder;
	}

	protected override void UpdateViewsHolder(MyViewsHolder newOrRecycled)
	{
		DungeonEntityItem dungeonEntityItem = Data[newOrRecycled.ItemIndex];
		bool flag = _DragManager.State != 0 && dungeonEntityItem == _DragManager.PlaceholderModel;
		newOrRecycled.scalableViews.gameObject.SetActive(!flag);
	}

	private void SetSlot(DungeonEntityItem model, MyViewsHolder newOrRecycled, bool empty)
	{
	}

	public void RemoveModel(int index)
	{
		Data.List[index] = null;
		for (int i = 0; i < _VisibleItemsCount; i++)
		{
			UpdateViewsHolder(_VisibleItems[i]);
		}
	}

	bool DraggableItem.IDragDropListener.OnPrepareToDragItem(DraggableItem item)
	{
		MyViewsHolder itemViewsHolderIfVisible = GetItemViewsHolderIfVisible(item.RT);
		if (itemViewsHolderIfVisible == null)
		{
			return false;
		}
		int itemIndex = itemViewsHolderIfVisible.ItemIndex;
		DungeonEntityItem modelOfDragged = Data[itemIndex];
		Data.List.RemoveAt(itemIndex);
		RemoveItemWithViewsHolder(itemViewsHolderIfVisible, stealViewsHolderInsteadOfRecycle: true, contentPanelEndEdgeStationary: false);
		UpdateScaleOfVisibleItems(itemViewsHolderIfVisible);
		TeamListItemDraggable placeholderModel = new TeamListItemDraggable();
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
		MyViewsHolder closestVHAtScreenPoint = GetClosestVHAtScreenPoint(eventData, out isPointInViewport);
		UpdateScaleOfVisibleItems(closestVHAtScreenPoint);
		if (closestVHAtScreenPoint != null && closestVHAtScreenPoint.ItemIndex != _DragManager.PlaceholderModel.index)
		{
			Data.RemoveOne(_DragManager.PlaceholderModel.index);
			InsertPlaceholderAtNewIndex(closestVHAtScreenPoint.ItemIndex);
		}
	}

	DraggableItem.OrphanedItemBundle DraggableItem.IDragDropListener.OnDroppedItem(PointerEventData eventData)
	{
		DraggableItem.OrphanedItemBundle result = DropDraggedVHAndEnterNoneState(eventData);
		for (int i = 0; i < _VisibleItemsCount; i++)
		{
			UpdateViewsHolder(_VisibleItems[i]);
		}
		return result;
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
		_DragManager.PlaceholderModel.index = index;
		Data.InsertOne(index, _DragManager.PlaceholderModel);
		RequestChangeItemSizeAndUpdateLayout(index, _DragManager.Dragged.root.rect.height, itemEndEdgeStationary: false, computeVisibility: true, correctItemPosition: true);
		Debug.Log("Finishing drag and droop");
	}

	private void UpdateScaleOfVisibleItems(MyViewsHolder vhToRotate)
	{
		foreach (MyViewsHolder visibleItem in _VisibleItems)
		{
			visibleItem.scalableViews.localScale = Vector3.one * ((visibleItem == vhToRotate) ? 0.98f : 1f);
		}
	}

	private bool TryGrabOrphanedItemVH(PointerEventData eventData, DraggableItem orphanedItemWithBundle)
	{
		bool isPointInViewport;
		MyViewsHolder closestVHAtScreenPoint = GetClosestVHAtScreenPoint(eventData, out isPointInViewport);
		if (!isPointInViewport)
		{
			return false;
		}
		orphanedItemWithBundle.dragDropListener = this;
		int num = 0;
		num = closestVHAtScreenPoint?.ItemIndex ?? 0;
		MyViewsHolder myViewsHolder = orphanedItemWithBundle.OrphanedBundle.views as MyViewsHolder;
		DungeonEntityItem item = orphanedItemWithBundle.OrphanedBundle.model as DungeonEntityItem;
		Data.List.Insert(num, item);
		float height = myViewsHolder.root.rect.height;
		InsertItemWithViewsHolder(myViewsHolder, num, contentPanelEndEdgeStationary: false);
		RequestChangeItemSizeAndUpdateLayout(num, height);
		return true;
	}

	private DraggableItem.OrphanedItemBundle DropDraggedVHAndEnterNoneState(PointerEventData eventData)
	{
		MyViewsHolder dragged = _DragManager.Dragged;
		DungeonEntityItem modelOfDragged = _DragManager.ModelOfDragged;
		int index = _DragManager.PlaceholderModel.index;
		int num;
		if (eventData == null)
		{
			num = dragged.ItemIndex;
			Data.RemoveOne(index);
			index = -1;
		}
		else
		{
			GetClosestVHAtScreenPoint(eventData, out var isPointInViewport);
			if (!isPointInViewport)
			{
				DraggableItem.OrphanedItemBundle result = new DraggableItem.OrphanedItemBundle
				{
					model = modelOfDragged,
					views = dragged,
					previousOwner = this
				};
				dragged.draggableComponent.dragDropListener = null;
				Data.RemoveOne(index);
				_DragManager.EnterState_None();
				UpdateScaleOfVisibleItems(null);
				return result;
			}
			num = index;
			index++;
		}
		Data.List.Insert(num, modelOfDragged);
		_DragManager.EnterState_None();
		float height = dragged.root.rect.height;
		InsertItemWithViewsHolder(dragged, num, contentPanelEndEdgeStationary: false);
		RequestChangeItemSizeAndUpdateLayout(num, height);
		UpdateScaleOfVisibleItems(null);
		if (index != -1)
		{
			Data.RemoveOne(index);
		}
		return null;
	}

	private MyViewsHolder GetClosestVHAtScreenPoint(PointerEventData eventData, out bool isPointInViewport)
	{
		isPointInViewport = false;
		if (!GetLocalPointInViewportIfWithinBounds(eventData.position, eventData.pressEventCamera, out var localPoint))
		{
			return null;
		}
		isPointInViewport = true;
		float distance;
		MyViewsHolder viewsHolderClosestToViewportPoint = GetViewsHolderClosestToViewportPoint(_Canvas, _CanvasRT, localPoint, 0f, out distance);
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
