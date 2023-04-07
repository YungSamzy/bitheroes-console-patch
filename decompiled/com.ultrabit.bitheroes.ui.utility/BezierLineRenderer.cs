using System.Collections.Generic;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

public class BezierLineRenderer : MonoBehaviour
{
	public LineRenderer lineRenderer;

	private static int curveCount = 0;

	private int layerOrder;

	private static int SEGMENT_COUNT = 100;

	public void LoadDetails(Color color)
	{
		if (!lineRenderer)
		{
			lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
			lineRenderer.startColor = color;
			lineRenderer.endColor = color;
		}
		curveCount = 1;
	}

	public void LoadDetails(string htmlColor)
	{
		if (!lineRenderer)
		{
			lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
			ColorUtility.TryParseHtmlString(htmlColor, out var color);
			lineRenderer.startColor = color;
			lineRenderer.endColor = color;
		}
		curveCount = 1;
	}

	public void DrawCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		for (int i = 0; i < curveCount; i++)
		{
			for (int j = 1; j < SEGMENT_COUNT; j++)
			{
				Vector3 position = CalculateCubicBezierPoint((float)j / (float)SEGMENT_COUNT, p0, p1, p2, p3);
				lineRenderer.positionCount = i * SEGMENT_COUNT + j;
				lineRenderer.SetPosition(i * SEGMENT_COUNT + (j - 1), position);
			}
		}
	}

	public void DrawCurve(List<Vector3> positions)
	{
		lineRenderer.positionCount = positions.Count;
		for (int i = 0; i < positions.Count; i++)
		{
			lineRenderer.SetPosition(i, positions[i]);
		}
	}

	public static List<Vector3> CalculateCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < 1; i++)
		{
			for (int j = 0; j <= SEGMENT_COUNT; j++)
			{
				Vector3 item = CalculateCubicBezierPoint((float)j / (float)SEGMENT_COUNT, p0, p1, p2, p3);
				list.Add(item);
			}
		}
		Debug.DrawLine(list[0], list[list.Count - 1]);
		return list;
	}

	private static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float num = 1f - t;
		float num2 = t * t;
		float num3 = num * num;
		float num4 = num3 * num;
		float num5 = num2 * t;
		return num4 * p0 + 3f * num3 * t * p1 + 3f * num * num2 * p2 + num5 * p3;
	}
}
