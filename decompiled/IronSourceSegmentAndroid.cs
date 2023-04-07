using System;
using UnityEngine;

public class IronSourceSegmentAndroid : AndroidJavaProxy, IUnitySegment
{
	public event Action<string> OnSegmentRecieved = delegate
	{
	};

	public IronSourceSegmentAndroid()
		: base("com.ironsource.unity.androidbridge.UnitySegmentListener")
	{
		try
		{
			using AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.ironsource.unity.androidbridge.AndroidBridge");
			androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", Array.Empty<object>()).Call("setUnitySegmentListener", this);
		}
		catch (Exception ex)
		{
			Debug.LogError("setUnitySegmentListener method doesn't exist, error: " + ex.Message);
		}
	}

	public void onSegmentRecieved(string segmentName)
	{
		if (this.OnSegmentRecieved != null)
		{
			this.OnSegmentRecieved(segmentName);
		}
	}
}
