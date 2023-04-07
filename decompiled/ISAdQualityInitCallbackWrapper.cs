using System;
using UnityEngine;

public class ISAdQualityInitCallbackWrapper : MonoBehaviour
{
	private ISAdQualityInitCallback mCallback;

	public ISAdQualityInitCallback AdQualityInitCallback
	{
		get
		{
			return mCallback;
		}
		set
		{
			mCallback = value;
		}
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void adQualitySdkInitSuccess(string unityMsg)
	{
		if (mCallback != null)
		{
			mCallback.adQualitySdkInitSuccess();
		}
	}

	public void adQualitySdkInitFailed(string unityMsg)
	{
		ISAdQualityInitError adQualityInitError = ISAdQualityInitError.EXCEPTION_ON_INIT;
		string errorMessage = string.Empty;
		try
		{
			if (!string.IsNullOrEmpty(unityMsg))
			{
				string[] separator = new string[1] { "Unity:" };
				string[] array = unityMsg.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length > 1)
				{
					adQualityInitError = (ISAdQualityInitError)Enum.Parse(typeof(ISAdQualityInitError), array[0]);
					errorMessage = array[1];
				}
			}
		}
		catch (Exception ex)
		{
			errorMessage = ex.Message;
		}
		if (mCallback != null)
		{
			mCallback.adQualitySdkInitFailed(adQualityInitError, errorMessage);
		}
	}
}
