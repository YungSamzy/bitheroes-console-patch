using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtil
{
	public delegate void GameObjectDelegate(GameObject go);

	public static void AttachChild(GameObject parent, GameObject child, bool worldPositionStays = false, bool suppressDebugLog = false)
	{
		if (parent != null)
		{
			if (child != null)
			{
				child.transform.SetParent(parent.transform, worldPositionStays);
			}
			else if (!suppressDebugLog)
			{
				Debug.LogWarning("Cannot attach child; child is null.");
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot attach child; parent is null.");
		}
	}

	public static GameObject AddChild(GameObject parent, GameObject childPrefab, bool worldPositionStays = false, bool suppressDebugLog = false)
	{
		GameObject gameObject = null;
		if (childPrefab != null)
		{
			gameObject = Object.Instantiate(childPrefab, Vector3.zero, Quaternion.identity);
			if (gameObject != null)
			{
				AttachChild(parent, gameObject, worldPositionStays, suppressDebugLog);
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot add child; childPrefab is null.");
		}
		return gameObject;
	}

	public static T AddChildComponent<T>(GameObject parent, T childComponent, bool worldPositionStays = false, bool suppressDebugLog = false) where T : Component
	{
		T result = null;
		if ((bool)(Object)childComponent)
		{
			GameObject gameObject = AddChild(parent, childComponent.gameObject, worldPositionStays, suppressDebugLog);
			if (gameObject != null)
			{
				return ComponentUtil.GetComponent<T>(gameObject, suppressDebugLog);
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot add child component " + typeof(T).Name + "; childComponent is null.");
		}
		return result;
	}

	public static T FindChildComponent<T>(GameObject parent, string childName, bool suppressDebugLog = false) where T : Component
	{
		T val = null;
		GameObject gameObject = FindChild(parent, childName, suppressDebugLog);
		if (gameObject != null)
		{
			val = gameObject.GetComponent<T>();
			if ((Object)val == (Object)null && !suppressDebugLog)
			{
				Debug.LogWarning("Cannot find child component " + typeof(T).Name + "; childComponent is null.");
			}
		}
		return val;
	}

	public static void DestroyChildren(GameObject go, bool immediate = false, bool suppressDebugLog = false)
	{
		if (go != null)
		{
			for (int num = go.transform.childCount - 1; num >= 0; num--)
			{
				Transform child = go.transform.GetChild(num);
				if ((bool)child)
				{
					if (immediate)
					{
						Object.DestroyImmediate(child.gameObject);
					}
					else
					{
						Object.Destroy(child.gameObject);
					}
				}
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot destroy children; go is null.");
		}
	}

	public static void DestroyChildrenWithComponent<T>(GameObject go, bool immediate = false, bool suppressDebugLog = false) where T : MonoBehaviour
	{
		if (go != null)
		{
			for (int num = go.transform.childCount - 1; num >= 0; num--)
			{
				T component = go.transform.GetChild(num).GetComponent<T>();
				if ((bool)(Object)component)
				{
					if (immediate)
					{
						Object.DestroyImmediate(component.gameObject);
					}
					else
					{
						Object.Destroy(component.gameObject);
					}
				}
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot destroy children with component " + typeof(T).Name + "; go is null.");
		}
	}

	public static void IterateChildren(GameObject go, GameObjectDelegate iterator, bool recursive = false, bool suppressDebugLog = false)
	{
		if (go != null)
		{
			foreach (Transform item in go.transform)
			{
				iterator(item.gameObject);
				if (recursive)
				{
					IterateChildren(item.gameObject, iterator, recursive: true);
				}
			}
			return;
		}
		if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot iterate children; go is null.");
		}
	}

	public static GameObject FindChild(GameObject go, string childName, bool suppressDebugLog = false)
	{
		GameObject result = null;
		if (go != null)
		{
			Transform transform = go.transform.Find(childName);
			if (transform != null)
			{
				result = transform.gameObject;
			}
			else if (!suppressDebugLog)
			{
				Debug.LogWarning("Could not find child " + childName + " of " + go.name, go);
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot find child; go is null.");
		}
		return result;
	}

	public static GameObject FindAncestor(GameObject go, string ancestorName, bool suppressDebugLog = false)
	{
		GameObject gameObject = null;
		if (go != null)
		{
			Transform parent = go.transform.parent;
			while ((bool)parent)
			{
				if (parent.name == ancestorName)
				{
					gameObject = parent.gameObject;
					break;
				}
				parent = parent.parent;
			}
			if (gameObject == null && !suppressDebugLog)
			{
				Debug.LogWarning("Could not find ancestor " + ancestorName + " of " + go.name + ".", go);
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot find ancestor; go is null.");
		}
		return gameObject;
	}

	public static bool HasActiveChildren(GameObject go, bool suppressDebugLog = false)
	{
		bool result = false;
		if (go != null)
		{
			int childCount = go.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				GameObject gameObject = go.transform.GetChild(i).gameObject;
				if ((bool)gameObject && gameObject.activeSelf)
				{
					result = true;
					break;
				}
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot check has active children; go is null.");
		}
		return result;
	}

	public static void SetChildrenActive(GameObject go, bool activeValue, bool suppressDebugLog = false)
	{
		if (go != null)
		{
			int childCount = go.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				GameObject gameObject = go.transform.GetChild(i).gameObject;
				if ((bool)gameObject)
				{
					gameObject.SetActive(activeValue);
				}
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot set children active; go is null.");
		}
	}

	private static List<Transform> GetChildrenTransforms(Transform parent, bool includeInactive)
	{
		List<Transform> list = null;
		if (parent != null)
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform child = parent.GetChild(i);
				if (child.gameObject.activeSelf || includeInactive)
				{
					if (list == null)
					{
						list = new List<Transform>();
					}
					list.Add(child);
				}
			}
		}
		return list;
	}

	public static T GetChildComponentInChildren<T>(Transform parent, string childName, bool includeInactive, bool inclideParent = false, int depth = int.MaxValue) where T : Component
	{
		T result = null;
		List<T> componentsInChildren = GetComponentsInChildren<T>(parent, includeInactive, inclideParent, depth);
		if (componentsInChildren != null)
		{
			foreach (T item in componentsInChildren)
			{
				if ((Object)item != (Object)null && item.gameObject != null && item.gameObject.name == childName)
				{
					return item;
				}
			}
			return result;
		}
		return result;
	}

	public static List<T> GetComponentsInChildren<T>(Transform parent, bool includeInactive, bool inclideParent = false, int depth = int.MaxValue) where T : Component
	{
		List<T> list = null;
		if (parent != null)
		{
			List<Transform> list2 = GetChildrenTransforms(parent, includeInactive);
			if (inclideParent)
			{
				if (list2 == null)
				{
					list2 = new List<Transform>();
				}
				list2.Insert(0, parent);
			}
			if (list2 != null)
			{
				int i = 0;
				while (depth > 0)
				{
					List<Transform> list3 = new List<Transform>();
					for (; i < list2.Count; i++)
					{
						List<Transform> childrenTransforms = GetChildrenTransforms(list2[i], includeInactive);
						if (childrenTransforms != null)
						{
							list3.AddRange(childrenTransforms);
						}
					}
					if (list3.Count == 0)
					{
						break;
					}
					list2.AddRange(list3);
					depth--;
				}
			}
			if (list2 != null)
			{
				foreach (Transform item in list2)
				{
					T component = item.GetComponent<T>();
					if ((Object)component != (Object)null)
					{
						if (list == null)
						{
							list = new List<T>();
						}
						list.Add(component);
					}
				}
				return list;
			}
		}
		return list;
	}
}
