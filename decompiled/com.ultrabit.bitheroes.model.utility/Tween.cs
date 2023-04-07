using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.utility;

public class Tween
{
	private static List<Dictionary<string, object>> DATA = new List<Dictionary<string, object>>();

	public static void ClearData()
	{
		DATA.Clear();
	}

	private static void SetParameters(object[] parameters)
	{
		GameObject obj = parameters[0] as GameObject;
		float value = (parameters[1] as float?).Value;
		float value2 = (parameters[2] as float?).Value;
		float value3 = (parameters[3] as float?).Value;
		float value4 = (parameters[4] as float?).Value;
		UnityAction<List<object>> onComplete = parameters[5] as UnityAction<List<object>>;
		List<object> onCompleteParams = parameters[6] as List<object>;
		string useLocalPosition = parameters[7] as string;
		StartMovement(obj, value, value2, value3, value4, onComplete, onCompleteParams, useLocalPosition);
	}

	public static void StartMovement(GameObject obj, float x, float y, float duration, float delay = 0f, UnityAction<List<object>> onComplete = null, List<object> onCompleteParams = null, string useLocalPosition = "false")
	{
		if (obj == null)
		{
			return;
		}
		Vector3 objectPosition = GetObjectPosition(obj, useLocalPosition);
		if (objectPosition.x == x && objectPosition.y == y)
		{
			return;
		}
		if (delay > 0f)
		{
			float newDelay = 0f;
			GameData.instance.main.coroutineTimer.AddTimer(null, delay, CoroutineTimer.TYPE.SECONDS, delegate
			{
				OnTimerCompleted(new object[8] { obj, x, y, duration, newDelay, onComplete, onCompleteParams, useLocalPosition });
			});
			return;
		}
		if (ObjectIsMoving(obj))
		{
			StopMovement(obj);
		}
		if (DATA.Count <= 0)
		{
			GameData.instance.main.DISPATCHER.FRAME_UPDATE.AddListener(OnUpdate);
		}
		float num = Mathf.Round(duration * (float)GameData.instance.main.DISPATCHER.currentFPS);
		if (num <= 0f)
		{
			num = 1f;
		}
		Vector2 vector = new Vector2(x - objectPosition.x, y - objectPosition.y);
		Vector2 vector2 = new Vector2(objectPosition.x, objectPosition.y);
		Vector2 vector3 = new Vector2(x, y);
		float num2 = 100f;
		Vector2 vector4 = new Vector2(vector.x / num, vector.y / num);
		if (vector4.x > 0f)
		{
			vector4.x = Mathf.Ceil(vector4.x * num2) / num2;
		}
		else if (vector4.x < 0f)
		{
			vector4.x = Mathf.Floor(vector4.x * num2) / num2;
		}
		if (vector4.y > 0f)
		{
			vector4.y = Mathf.Ceil(vector4.y * num2) / num2;
		}
		else if (vector4.y < 0f)
		{
			vector4.y = Mathf.Floor(vector4.y * num2) / num2;
		}
		Dictionary<string, object> item = new Dictionary<string, object>
		{
			["object"] = obj,
			["target"] = vector3,
			["position"] = vector2,
			["velocity"] = vector4,
			["duration"] = duration,
			["onComplete"] = onComplete,
			["onCompleteParams"] = onCompleteParams,
			["useLocalPosition"] = useLocalPosition
		};
		DATA.Add(item);
	}

	public static void StartLocalMovement(GameObject obj, float x, float y, float duration, float delay = 0f, UnityAction<List<object>> onComplete = null, List<object> onCompleteParams = null)
	{
		StartMovement(obj, x, y, duration, delay, onComplete, onCompleteParams, "true");
	}

	private static void OnTimerCompleted(object[] parameters)
	{
		SetParameters(parameters);
	}

	public static void StopMovement(GameObject obj)
	{
		if (obj == null)
		{
			return;
		}
		for (int i = 0; i < DATA.Count; i++)
		{
			if ((DATA[i]["object"] as GameObject).GetInstanceID() == obj.GetInstanceID())
			{
				DATA.RemoveAt(i);
				if (DATA.Count <= 0)
				{
					GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnUpdate);
				}
				break;
			}
		}
	}

	private static void OnUpdate(object e)
	{
		float num = (e as float[])[0];
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
		foreach (Dictionary<string, object> dATum in DATA)
		{
			GameObject gameObject = dATum["object"] as GameObject;
			if (gameObject == null)
			{
				list2.Add(dATum);
				continue;
			}
			Vector2 value = (dATum["target"] as Vector2?).Value;
			Vector2 value2 = (dATum["position"] as Vector2?).Value;
			Vector2 value3 = (dATum["velocity"] as Vector2?).Value;
			Vector2 vector = new Vector2(value3.x * num, value3.y * num);
			string useLocalPosition = dATum["useLocalPosition"] as string;
			if (value3.x != 0f)
			{
				value2.x += vector.x;
			}
			if (value3.y != 0f)
			{
				value2.y += vector.y;
			}
			if ((value3.x > 0f && value2.x >= value.x) || (value3.x < 0f && value2.x <= value.x))
			{
				value2.x = value.x;
				value3.x = 0f;
			}
			if ((value3.y > 0f && value2.y >= value.y) || (value3.y < 0f && value2.y <= value.y))
			{
				value2.y = value.y;
				value3.y = 0f;
			}
			dATum["position"] = value2;
			Vector3 objectPosition = GetObjectPosition(gameObject, useLocalPosition);
			SetObjectPosition(gameObject, new Vector3(value2.x, value2.y, objectPosition.z), useLocalPosition);
			if (value3.x == 0f && value3.y == 0f)
			{
				list.Add(dATum);
			}
		}
		if (list != null)
		{
			foreach (Dictionary<string, object> item in list)
			{
				StopMovement(item["object"] as GameObject);
				UnityAction<List<object>> unityAction = item["onComplete"] as UnityAction<List<object>>;
				List<object> arg = item["onCompleteParams"] as List<object>;
				unityAction?.Invoke(arg);
			}
		}
		if (list2 == null)
		{
			return;
		}
		foreach (Dictionary<string, object> item2 in list2)
		{
			DATA.Remove(item2);
		}
	}

	public static bool ObjectIsMoving(GameObject obj)
	{
		if (obj == null)
		{
			return false;
		}
		foreach (Dictionary<string, object> dATum in DATA)
		{
			if ((dATum["object"] as GameObject).GetInstanceID() == obj.GetInstanceID())
			{
				return true;
			}
		}
		return false;
	}

	private static Vector3 GetObjectPosition(GameObject obj, string useLocalPosition)
	{
		if (!useLocalPosition.Trim().ToLower().Equals("true"))
		{
			return obj.transform.position;
		}
		return obj.transform.localPosition;
	}

	private static void SetObjectPosition(GameObject obj, Vector3 pos, string useLocalPosition)
	{
		if (useLocalPosition.Trim().ToLower().Equals("true"))
		{
			obj.transform.localPosition = pos;
		}
		else
		{
			obj.transform.position = pos;
		}
	}
}
