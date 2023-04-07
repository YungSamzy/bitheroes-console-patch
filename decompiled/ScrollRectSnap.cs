using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour
{
	private float[] points;

	[Tooltip("how many screens or pages are there within the content (steps)")]
	public int screens = 1;

	[Tooltip("How quickly the GUI snaps to each panel")]
	public float snapSpeed;

	public float inertiaCutoffMagnitude;

	private float stepSize;

	private ScrollRect scroll;

	private bool LerpH;

	private float targetH;

	[Tooltip("Snap horizontally")]
	public bool snapInH = true;

	private bool LerpV;

	private float targetV;

	[Tooltip("Snap vertically")]
	public bool snapInV = true;

	public string controllTag;

	private bool dragInit = true;

	private int dragStartNearest;

	private float horizontalNormalizedPosition;

	private float verticalNormalizedPosition;

	public static event Action<int, int> OnEndReached;

	public static event Action<int, int, string> OnEndReachedWithTag;

	private void Start()
	{
		Init();
	}

	private void Init()
	{
		scroll = base.transform.parent.GetComponent<ScrollRect>();
		scroll.inertia = true;
		if (screens > 0)
		{
			points = new float[screens];
			stepSize = (float)Math.Round(1f / (float)(screens - 1), 2);
			for (int i = 0; i < screens; i++)
			{
				points[i] = (float)i * stepSize;
			}
		}
		else
		{
			points[0] = 0f;
		}
	}

	private void Update()
	{
		horizontalNormalizedPosition = scroll.horizontalNormalizedPosition;
		verticalNormalizedPosition = scroll.verticalNormalizedPosition;
		if (LerpH)
		{
			scroll.horizontalNormalizedPosition = Mathf.Lerp(scroll.horizontalNormalizedPosition, targetH, snapSpeed * Time.deltaTime);
			if (Mathf.Approximately((float)Math.Round(scroll.horizontalNormalizedPosition, 2), targetH))
			{
				LerpH = false;
				int num = FindNearest(scroll.horizontalNormalizedPosition, points);
				if (num == points.Length - 1)
				{
					if (ScrollRectSnap.OnEndReached != null)
					{
						ScrollRectSnap.OnEndReached(1, num);
					}
					if (ScrollRectSnap.OnEndReachedWithTag != null)
					{
						ScrollRectSnap.OnEndReachedWithTag(1, num, controllTag);
					}
				}
				else if (num == 0)
				{
					if (ScrollRectSnap.OnEndReached != null)
					{
						ScrollRectSnap.OnEndReached(-1, num);
					}
					if (ScrollRectSnap.OnEndReachedWithTag != null)
					{
						ScrollRectSnap.OnEndReachedWithTag(-1, num, controllTag);
					}
				}
				else
				{
					if (ScrollRectSnap.OnEndReached != null)
					{
						ScrollRectSnap.OnEndReached(0, num);
					}
					if (ScrollRectSnap.OnEndReachedWithTag != null)
					{
						ScrollRectSnap.OnEndReachedWithTag(0, num, controllTag);
					}
				}
			}
		}
		if (LerpV)
		{
			scroll.verticalNormalizedPosition = Mathf.Lerp(scroll.verticalNormalizedPosition, targetV, snapSpeed * Time.deltaTime);
			if (Mathf.Approximately(scroll.verticalNormalizedPosition, targetV))
			{
				LerpV = false;
			}
		}
	}

	public void DragEnd()
	{
		int num = FindNearest(scroll.horizontalNormalizedPosition, points);
		if (num == dragStartNearest && scroll.velocity.sqrMagnitude > inertiaCutoffMagnitude * inertiaCutoffMagnitude)
		{
			if (scroll.velocity.x < 0f)
			{
				num = dragStartNearest + 1;
			}
			else if (scroll.velocity.x > 1f)
			{
				num = dragStartNearest - 1;
			}
			num = Mathf.Clamp(num, 0, points.Length - 1);
		}
		if (scroll.horizontal && snapInH)
		{
			targetH = points[num];
			LerpH = true;
		}
		if (scroll.vertical && snapInV && scroll.verticalNormalizedPosition > 0f && scroll.verticalNormalizedPosition < 1f)
		{
			targetH = points[num];
			LerpH = true;
		}
		dragInit = true;
	}

	public void OnDrag()
	{
		if (dragInit)
		{
			if (scroll == null)
			{
				scroll = base.transform.parent.GetComponent<ScrollRect>();
			}
			dragStartNearest = FindNearest(scroll.horizontalNormalizedPosition, points);
			dragInit = false;
		}
		LerpH = false;
		LerpV = false;
	}

	private int FindNearest(float f, float[] array)
	{
		float num = float.PositiveInfinity;
		int result = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (Mathf.Abs(array[i] - f) < num)
			{
				num = Mathf.Abs(array[i] - f);
				result = i;
			}
		}
		return result;
	}

	public void DraggedOnLeftOrUp()
	{
		OnDrag();
		if (scroll.horizontal && snapInH && scroll.horizontalNormalizedPosition > -0.001f && scroll.horizontalNormalizedPosition < 1.001f)
		{
			Debug.Log("Before Press, LerpH : " + LerpH);
			if (dragStartNearest < points.Length - 1)
			{
				targetH = points[dragStartNearest + 1];
				LerpH = true;
			}
			else
			{
				targetH = points[dragStartNearest];
				LerpH = true;
			}
			Debug.Log("After Press, LerpH : " + LerpH);
		}
		if (scroll.vertical && snapInV && scroll.verticalNormalizedPosition > 0f && scroll.verticalNormalizedPosition < 1f)
		{
			if (dragStartNearest < points.Length - 1)
			{
				targetV = points[dragStartNearest + 1];
				LerpV = true;
			}
			else
			{
				targetV = points[dragStartNearest];
				LerpV = true;
			}
		}
		dragInit = true;
	}

	public void DraggedOnRightOrDown()
	{
		OnDrag();
		if (scroll.horizontal && snapInH && scroll.horizontalNormalizedPosition > -0.001f && scroll.horizontalNormalizedPosition < 1.001f)
		{
			if (dragStartNearest > 0)
			{
				targetH = points[dragStartNearest - 1];
				LerpH = true;
			}
			else
			{
				targetH = points[dragStartNearest];
				LerpH = true;
			}
		}
		if (scroll.vertical && snapInV && scroll.verticalNormalizedPosition > 0f && scroll.verticalNormalizedPosition < 1f)
		{
			if (dragStartNearest > 0)
			{
				targetV = points[dragStartNearest - 1];
				LerpV = true;
			}
			else
			{
				targetV = points[dragStartNearest];
				LerpV = true;
			}
		}
		dragInit = true;
	}

	public void SnapToSelectedIndex(int index)
	{
		if (points == null)
		{
			Init();
		}
		dragInit = false;
		LerpH = false;
		LerpV = false;
		targetH = points[index];
		LerpH = true;
		dragInit = true;
	}
}
