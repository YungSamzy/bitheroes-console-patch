using System;
using System.Collections;
using System.Collections.Generic;
using AOT;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WebGLSupport;

public class WebGLInputMobile : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	private static Dictionary<int, WebGLInputMobile> instances = new Dictionary<int, WebGLInputMobile>();

	private int id = -1;

	private void Awake()
	{
		base.enabled = false;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (id == -1)
		{
			id = WebGLInputMobilePlugin.WebGLInputMobileRegister(OnTouchEnd);
			instances[id] = this;
		}
	}

	[MonoPInvokeCallback(typeof(Action<int>))]
	private static void OnTouchEnd(int id)
	{
		WebGLInputMobile webGLInputMobile = instances[id];
		webGLInputMobile.GetComponent<WebGLInput>().OnSelect();
		webGLInputMobile.StartCoroutine(RegisterOnFocusOut(id));
	}

	private static IEnumerator RegisterOnFocusOut(int id)
	{
		yield return null;
		WebGLInputMobilePlugin.WebGLInputMobileOnFocusOut(id, OnFocusOut);
	}

	[MonoPInvokeCallback(typeof(Action<int>))]
	private static void OnFocusOut(int id)
	{
		Debug.Log($"OnFocusOut:{id}");
		WebGLInputMobile webGLInputMobile = instances[id];
		webGLInputMobile.GetComponent<WebGLInput>().DeactivateInputField();
		webGLInputMobile.id = -1;
		instances.Remove(id);
	}
}
