using UnityEngine;

public class Point : ScriptableObject
{
	public float x;

	public float y;

	public static Point create(float x = 0f, float y = 0f)
	{
		Point obj = ScriptableObject.CreateInstance(typeof(Point).ToString()) as Point;
		obj.init(x, y);
		return obj;
	}

	public static Point fromVector2(Vector2 v)
	{
		return create(v.x, v.y);
	}

	public void init(float _x = 0f, float _y = 0f)
	{
		x = _x;
		y = _y;
	}

	public Vector2 toVector2()
	{
		return new Vector2(x, y);
	}

	public Vector3 toVector3()
	{
		return new Vector3(x, y, 0f);
	}
}
