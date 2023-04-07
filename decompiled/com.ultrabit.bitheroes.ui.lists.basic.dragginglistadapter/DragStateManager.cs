using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.ui.lists.basic.dragginglistadapter;

internal class DragStateManager
{
	public MyViewsHolder Dragged { get; private set; }

	public MyModel ModelOfDragged { get; private set; }

	public DragState State { get; private set; }

	public EmptyModel PlaceholderModel { get; private set; }

	public void EnterState_None()
	{
		Dragged = null;
		ModelOfDragged = null;
		PlaceholderModel = null;
		State = DragState.NONE;
	}

	public void EnterState_PreparingForDrag(MyViewsHolder dragged, MyModel modelOfDragged, EmptyModel placeholderModel)
	{
		Dragged = dragged;
		ModelOfDragged = modelOfDragged;
		PlaceholderModel = placeholderModel;
		State = DragState.PREPARING_FOR_DRAG;
	}

	public void EnterState_Dragging(PointerEventData eventData)
	{
		State = DragState.DRAGGING;
	}
}
