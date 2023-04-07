using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.DataHelpers;
using Com.TheFallenGames.OSA.Util.ItemDragging;
using com.ultrabit.bitheroes.model.utility;
using frame8.Logic.Misc.Other.Extensions;
using frame8.Logic.Misc.Visual.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.teamgrouplist;

public class TeamGroupList : OSA<MyParams, MyViewsHolder>, DraggableItem.IDragDropListener, ICancelHandler, IEventSystemHandler
{
	private DragStateManager _DragManager = new DragStateManager();

	private Canvas _Canvas;

	private RectTransform _CanvasRT;

	private int playersByGroup = 3;

	private int GroupsCount = 3;

	public SimpleDataHelper<MyModel> Data { get; private set; }

	protected override void Start()
	{
		Data = new SimpleDataHelper<MyModel>(this);
		base.Start();
		_Canvas = GetComponentInParent<Canvas>();
		_CanvasRT = _Canvas.transform as RectTransform;
		RetrieveDataAndUpdate(playersByGroup * GroupsCount + GroupsCount);
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	private void SwapUp(DraggableItem item)
	{
		MyViewsHolder itemViewsHolderIfVisible = GetItemViewsHolderIfVisible(item.RT);
		if (itemViewsHolderIfVisible == null || itemViewsHolderIfVisible.ItemIndex <= 1)
		{
			return;
		}
		if (itemViewsHolderIfVisible.ItemIndex == GroupsCount + 2 || itemViewsHolderIfVisible.ItemIndex == 2 * GroupsCount + 2 || itemViewsHolderIfVisible.ItemIndex == 3 * GroupsCount || itemViewsHolderIfVisible.ItemIndex == 4 * GroupsCount + 2 || itemViewsHolderIfVisible.ItemIndex == 5 * GroupsCount + 2)
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex - 2);
			for (int i = 0; i < _VisibleItemsCount; i++)
			{
				UpdateViewsHolder(_VisibleItems[i]);
			}
		}
		else
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex - 1);
			for (int j = 0; j < _VisibleItemsCount; j++)
			{
				UpdateViewsHolder(_VisibleItems[j]);
			}
		}
	}

	private void SwapDown(DraggableItem item)
	{
		MyViewsHolder itemViewsHolderIfVisible = GetItemViewsHolderIfVisible(item.RT);
		if (itemViewsHolderIfVisible == null || itemViewsHolderIfVisible.ItemIndex >= Data.Count - 1)
		{
			return;
		}
		if (itemViewsHolderIfVisible.ItemIndex == GroupsCount || itemViewsHolderIfVisible.ItemIndex == 2 * GroupsCount + 1)
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex + 2);
			for (int i = 0; i < _VisibleItemsCount; i++)
			{
				UpdateViewsHolder(_VisibleItems[i]);
			}
		}
		else
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex + 1);
			for (int j = 0; j < _VisibleItemsCount; j++)
			{
				UpdateViewsHolder(_VisibleItems[j]);
			}
		}
	}

	private void RetrieveDataAndUpdate(int count)
	{
		MyModel[] array = new MyModel[count];
		for (int i = 0; i < count; i++)
		{
			if (i == 0 || i == 4 || i == 8 || i == 12 || i == 16)
			{
				MyModel myModel = new MyModel
				{
					divisor = true
				};
				array[i] = myModel;
			}
			else
			{
				MyModel myModel2 = new MyModel
				{
					color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f),
					idOrder = i
				};
				array[i] = myModel2;
			}
		}
		OnDataRetrieved(array);
	}

	private void OnDataRetrieved(MyModel[] newItems)
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
		MyModel myModel = Data[newOrRecycled.ItemIndex];
		bool flag = _DragManager.State != 0 && myModel == _DragManager.PlaceholderModel;
		if (!flag)
		{
			newOrRecycled.scalableViews.gameObject.SetActive(!flag);
			newOrRecycled.numberTxt.text = newOrRecycled.ItemIndex.ToString();
			Util.SetButton(newOrRecycled.upBtn.GetComponent<Button>(), newOrRecycled.numberTxt.text != "1");
			Util.SetButton(newOrRecycled.downBtn.GetComponent<Button>(), newOrRecycled.numberTxt.text != (playersByGroup * GroupsCount + GroupsCount - 1).ToString());
			newOrRecycled.removeBtn.GetComponent<Button>().onClick.AddListener(delegate
			{
				RemoveModel(newOrRecycled.ItemIndex);
			});
			if (myModel != null)
			{
				newOrRecycled.avatar.color = myModel.color;
			}
			newOrRecycled.background.gameObject.SetActive((myModel != null && !myModel.divisor) || myModel == null);
			newOrRecycled.teamObject.gameObject.SetActive(myModel != null && !myModel.divisor);
			newOrRecycled.addBtn.gameObject.SetActive(myModel == null);
			newOrRecycled.numberTxt.gameObject.SetActive(myModel == null);
			newOrRecycled.teamGroupTile.gameObject.SetActive(myModel?.divisor ?? false);
		}
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
		MyModel modelOfDragged = Data[itemIndex];
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
		MyViewsHolder closestVHAtScreenPoint = GetClosestVHAtScreenPoint(eventData, out isPointInViewport);
		UpdateScaleOfVisibleItems(closestVHAtScreenPoint);
		if (closestVHAtScreenPoint != null && closestVHAtScreenPoint.ItemIndex != _DragManager.PlaceholderModel.placeholderForIndex)
		{
			Data.RemoveOne(_DragManager.PlaceholderModel.placeholderForIndex);
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
		_DragManager.PlaceholderModel.placeholderForIndex = index;
		Data.InsertOne(index, _DragManager.PlaceholderModel);
		RequestChangeItemSizeAndUpdateLayout(index, _DragManager.Dragged.root.rect.height, itemEndEdgeStationary: false, computeVisibility: true, correctItemPosition: true);
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
		MyModel item = orphanedItemWithBundle.OrphanedBundle.model as MyModel;
		Data.List.Insert(num, item);
		float height = myViewsHolder.root.rect.height;
		InsertItemWithViewsHolder(myViewsHolder, num, contentPanelEndEdgeStationary: false);
		RequestChangeItemSizeAndUpdateLayout(num, height);
		return true;
	}

	private DraggableItem.OrphanedItemBundle DropDraggedVHAndEnterNoneState(PointerEventData eventData)
	{
		MyViewsHolder dragged = _DragManager.Dragged;
		MyModel modelOfDragged = _DragManager.ModelOfDragged;
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
				Data.RemoveOne(placeholderForIndex);
				_DragManager.EnterState_None();
				UpdateScaleOfVisibleItems(null);
				return result;
			}
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
