using System;
using System.Reflection;
using UnityEngine;

public static class ComponentUtil
{
	public static T GetComponent<T>(GameObject go, bool suppressDebugLog = false) where T : Component
	{
		T result = null;
		if (go != null)
		{
			T component = go.GetComponent<T>();
			if ((UnityEngine.Object)component != (UnityEngine.Object)null)
			{
				return component;
			}
			if (!suppressDebugLog)
			{
				Debug.LogWarning(go.name + " does not have a " + typeof(T).Name + " component.", go);
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot get component " + typeof(T).Name + "; go is null");
		}
		return result;
	}

	public static Component GetOrCreateComponent(GameObject go, Type componentType, bool suppressDebugLog = false)
	{
		Component component = null;
		if (componentType != null)
		{
			if (go != null)
			{
				component = go.GetComponent(componentType);
				if (component == null)
				{
					component = go.AddComponent(componentType);
				}
			}
			else if (!suppressDebugLog)
			{
				Debug.LogWarning("Cannot get or create component " + componentType.Name + "; go is null");
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot get or create component; componentType is null");
		}
		return component;
	}

	public static T GetOrCreateComponent<T>(GameObject go, bool suppressDebugLog = false) where T : Component
	{
		T val = null;
		if (go != null)
		{
			val = go.GetComponent<T>();
			if ((UnityEngine.Object)val == (UnityEngine.Object)null)
			{
				val = go.AddComponent<T>();
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot get or create component " + typeof(T).Name + "; go is null");
		}
		return val;
	}

	public static T GetComponentInAncestors<T>(GameObject go, bool suppressDebugLog = false) where T : Component
	{
		T val = null;
		if (go != null)
		{
			Transform parent = go.transform.parent;
			while ((bool)parent)
			{
				val = GetComponent<T>(parent.gameObject, suppressDebugLog: true);
				if ((bool)(UnityEngine.Object)val)
				{
					break;
				}
				parent = parent.parent;
			}
			if ((UnityEngine.Object)val == (UnityEngine.Object)null && !suppressDebugLog)
			{
				Debug.LogWarning("Could not get component " + typeof(T).Name + " in ancestors of " + go.name + ".");
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot get component " + typeof(T).Name + " in ancestors; go is null.");
		}
		return val;
	}

	public static void DestroyComponent<T>(GameObject go, bool immediate = true, bool suppressDebugLog = false) where T : Component
	{
		T component = GetComponent<T>(go, suppressDebugLog);
		if ((UnityEngine.Object)component != (UnityEngine.Object)null)
		{
			if (immediate)
			{
				UnityEngine.Object.DestroyImmediate(component);
			}
			else
			{
				UnityEngine.Object.Destroy(component);
			}
		}
	}

	public static void DestroyComponentsInChildren<T>(GameObject go, bool immediate = true, bool suppressDebugLog = false) where T : Component
	{
		if (go != null)
		{
			T[] componentsInChildren = go.GetComponentsInChildren<T>();
			foreach (T obj in componentsInChildren)
			{
				if (immediate)
				{
					UnityEngine.Object.DestroyImmediate(obj);
				}
				else
				{
					UnityEngine.Object.Destroy(obj);
				}
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot destroy components " + typeof(T).Name + " in children; go is null");
		}
	}

	public static T AddComponentCopy<T>(GameObject go, T component, bool suppressDebugLog = false) where T : Component
	{
		T val = null;
		if (go != null)
		{
			if ((UnityEngine.Object)component != (UnityEngine.Object)null)
			{
				val = GetOrCreateComponent<T>(go);
				if ((UnityEngine.Object)val == (UnityEngine.Object)null)
				{
					Type type = component.GetType();
					BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
					PropertyInfo[] properties = type.GetProperties(bindingAttr);
					foreach (PropertyInfo propertyInfo in properties)
					{
						if (propertyInfo.CanWrite)
						{
							propertyInfo.SetValue(val, propertyInfo.GetValue(component));
						}
					}
					FieldInfo[] fields = type.GetFields(bindingAttr);
					foreach (FieldInfo fieldInfo in fields)
					{
						fieldInfo.SetValue(val, fieldInfo.GetValue(component));
					}
				}
				else if (!suppressDebugLog)
				{
					Debug.LogWarning("Cannot copy component " + typeof(T).Name + "; failed to add component to game object");
				}
			}
			else if (!suppressDebugLog)
			{
				Debug.LogWarning("Cannot copy component " + typeof(T).Name + "; " + typeof(T).Name + " component to copy from is not set");
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot copy component " + typeof(T).Name + "; " + typeof(GameObject).Name + " to copy into is not set");
		}
		return val;
	}
}
