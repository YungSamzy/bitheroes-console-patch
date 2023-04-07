using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class UnitySingleton : BaseBehaviour
{
	private static readonly Dictionary<Type, UnitySingleton> instances = new Dictionary<Type, UnitySingleton>();

	private static readonly Dictionary<Type, Dictionary<MonoBehaviour, Action<UnitySingleton>>> instanceListeners = new Dictionary<Type, Dictionary<MonoBehaviour, Action<UnitySingleton>>>();

	protected bool IsInstanceReady { get; private set; }

	protected virtual bool DontDestroySingleton => false;

	private void RegisterAsSingleInstanceAndInit()
	{
		instances.Add(GetType(), this);
		InnerInit();
	}

	private void InnerInit()
	{
		InitAfterRegisteringAsSingleInstance();
		if (DontDestroySingleton)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	private static S GetOrCreateInstanceOnGameObject<S>(Type type) where S : CodeGeneratedSingleton
	{
		S val = null;
		GameObject gameObject = Resources.Load<GameObject>(type.Name);
		if ((bool)gameObject)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
			if (!gameObject2)
			{
				throw new Exception("Failed to instantiate prefab: " + type.Name);
			}
			val = gameObject2.GetComponent<S>();
			if (!(UnityEngine.Object)val)
			{
				val = gameObject2.AddComponent<S>();
			}
		}
		if (!(UnityEngine.Object)val)
		{
			val = new GameObject(type.Name).AddComponent<S>();
		}
		return val;
	}

	private void NotifyInstanceListeners()
	{
		Type type = GetType();
		if (!instanceListeners.ContainsKey(type))
		{
			return;
		}
		KeyValuePair<MonoBehaviour, Action<UnitySingleton>>[] array = instanceListeners[type].ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			KeyValuePair<MonoBehaviour, Action<UnitySingleton>> keyValuePair = array[i];
			if ((bool)keyValuePair.Key && keyValuePair.Value != null)
			{
				keyValuePair.Value(this);
			}
			instanceListeners[type].Remove(keyValuePair.Key);
		}
	}

	protected void DeclareAsReady()
	{
		IsInstanceReady = true;
		NotifyInstanceListeners();
	}

	protected static S GetSynchronousCodeGeneratedInstance<S>() where S : CodeGeneratedSingleton
	{
		Type typeFromHandle = typeof(S);
		S val;
		if (!instances.ContainsKey(typeFromHandle))
		{
			val = UnityEngine.Object.FindObjectOfType<S>();
			if (!(UnityEngine.Object)val)
			{
				val = GetOrCreateInstanceOnGameObject<S>(typeFromHandle);
			}
			val.RegisterAsSingleInstanceAndInit();
		}
		else
		{
			val = instances[typeFromHandle] as S;
		}
		if (!(UnityEngine.Object)val)
		{
			throw new Exception("No instance was created: " + typeFromHandle.Name);
		}
		val.IsInstanceReady = true;
		return val;
	}

	public static void DoWithCodeGeneratedInstance<C>(MonoBehaviour sender, Action<C> whatToDoWithInstanceWhenItsReady) where C : CodeGeneratedSingleton
	{
		GetSynchronousCodeGeneratedInstance<C>();
		DoWithExistingInstance(sender, whatToDoWithInstanceWhenItsReady);
	}

	public static void DoWithSceneInstance<S>(MonoBehaviour sender, Action<S> whatToDoWithInstanceWhenItsReady) where S : SceneSingleton
	{
		DoWithExistingInstance(sender, whatToDoWithInstanceWhenItsReady);
	}

	private static void DoWithExistingInstance<S>(MonoBehaviour sender, Action<S> whatToDoWithInstanceWhenItsReady) where S : UnitySingleton
	{
		Type typeFromHandle = typeof(S);
		bool flag = true;
		if (instances.ContainsKey(typeFromHandle))
		{
			S val = instances[typeFromHandle] as S;
			if ((bool)(UnityEngine.Object)val && val.IsInstanceReady)
			{
				flag = false;
				whatToDoWithInstanceWhenItsReady(val);
			}
		}
		if (flag)
		{
			if (!instanceListeners.ContainsKey(typeFromHandle))
			{
				instanceListeners.Add(typeFromHandle, new Dictionary<MonoBehaviour, Action<UnitySingleton>>());
			}
			if (!instanceListeners[typeFromHandle].ContainsKey(sender))
			{
				instanceListeners[typeFromHandle].Add(sender, null);
			}
			Dictionary<MonoBehaviour, Action<UnitySingleton>> dictionary = instanceListeners[typeFromHandle];
			dictionary[sender] = (Action<UnitySingleton>)Delegate.Combine(dictionary[sender], (Action<UnitySingleton>)delegate(UnitySingleton singleton)
			{
				whatToDoWithInstanceWhenItsReady(singleton as S);
			});
		}
	}

	protected sealed override void Start()
	{
		base.Start();
		Type type = GetType();
		bool flag = false;
		if (instances.ContainsKey(type))
		{
			if (instances[type] != this)
			{
				if (this is CodeGeneratedSingleton)
				{
					throw new Exception("There's already an instance for " + type.Name);
				}
				if (this is SceneSingleton && DontDestroySingleton)
				{
					flag = true;
				}
			}
		}
		else if (this is CodeGeneratedSingleton)
		{
			throw new NotSupportedException($"{type.Name} is a {typeof(CodeGeneratedSingleton).Name} and needs to be created via code, and not placed on a scene!");
		}
		if (flag)
		{
			Debug.LogWarning($"There's already a {type.Name} instance on the current scene, there's no point in staying.. goodbye.. I'm gonna go now :(");
			UnityEngine.Object.Destroy(this);
		}
		else if (this is SceneSingleton)
		{
			RegisterAsSingleInstanceAndInit();
			SetReadyAndNotifyAfterRegistering();
		}
	}

	protected virtual void SetReadyAndNotifyAfterRegistering()
	{
		DeclareAsReady();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Type type = GetType();
		if (instances.ContainsKey(type) && instances[type] == this)
		{
			instances.Remove(type);
		}
	}

	protected virtual void InitAfterRegisteringAsSingleInstance()
	{
	}
}
