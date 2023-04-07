using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.dialog;

public class DialogPopup : WindowsMain
{
	private DialogRef _dialogRef;

	private object _data;

	private bool _showKongButton;

	private bool _closing;

	private DialogFrame _dialogFrame;

	public UnityCustomEvent CLEAR = new UnityCustomEvent();

	private bool _cleared;

	public DialogRef dialogRef => _dialogRef;

	public object data => _data;

	public bool cleared => _cleared;

	public override void Start()
	{
		base.Start();
	}

	public void LoadDetails(DialogRef dialogRef, object data = null, bool showKongButton = false, bool setSeen = true)
	{
		_dialogRef = dialogRef;
		if (setSeen)
		{
			_dialogRef.See();
		}
		Disable();
		_data = data;
		_showKongButton = showKongButton;
		ShowFrame(dialogRef.getFrame(0));
		ListenForBack(delegate
		{
			DoNext();
		});
		ListenForForward(delegate
		{
			DoNext();
		});
		if (showKongButton && GameData.instance.PROJECT != null && GameData.instance.PROJECT.instance != null && GameData.instance.PROJECT.instance.instanceInterface != null)
		{
			GameData.instance.PROJECT.instance.instanceInterface.ReassignKongregateTile(base.transform);
		}
		CreateWindow(closeWord: false, "", scroll: false);
		Enable();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		if (_dialogFrame != null)
		{
			_dialogFrame.UpdateLayers();
		}
	}

	public void OnDialogClick()
	{
		if (GameData.instance.PROJECT.character.autoPilot && AppInfo.TESTING)
		{
			CancelInvoke("Wait");
		}
		bool num = DoNext();
		Debug.Log(num);
		if (num)
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
		}
	}

	private bool DoNext()
	{
		if (_dialogFrame != null)
		{
			DialogFrameContentRef nextContent = _dialogFrame.GetNextContent();
			if (nextContent != null)
			{
				_dialogFrame.ShowContent(nextContent);
				return true;
			}
		}
		return ShowFrame(GetNextFrame());
	}

	private void DoSkip()
	{
		OnClose();
	}

	public override void OnClose()
	{
		if (!_closing)
		{
			_closing = true;
			if (_dialogFrame != null)
			{
				_dialogFrame.arrow.onClick.RemoveListener(OnDialogClick);
			}
			if (_showKongButton && GameData.instance.PROJECT != null && GameData.instance.PROJECT.instance != null && GameData.instance.PROJECT.instance.instanceInterface != null)
			{
				GameData.instance.PROJECT.instance.instanceInterface.ReassignKongregateTile();
			}
			CLEAR.Invoke(this);
			base.OnClose();
			base.DoDestroy();
		}
	}

	private bool ShowFrame(DialogFrameRef frameRef)
	{
		if (_dialogFrame != null)
		{
			_dialogFrame.arrow.onClick.RemoveListener(OnDialogClick);
			Object.Destroy(_dialogFrame.gameObject);
			_dialogFrame = null;
		}
		if (frameRef == null)
		{
			OnClose();
			return false;
		}
		Transform transform = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/dialog/" + typeof(DialogFrame).Name));
		transform.SetParent(panel.transform, worldPositionStays: false);
		_dialogFrame = transform.GetComponent<DialogFrame>();
		_dialogFrame.arrow.onClick.AddListener(OnDialogClick);
		_dialogFrame.LoadDetails(frameRef, this);
		if (GameData.instance.PROJECT.character.autoPilot && AppInfo.TESTING)
		{
			Invoke("Wait", 1f);
		}
		return true;
	}

	private void Wait()
	{
		Debug.LogFormat("Log: <color=orange>{0}</color>", "Continue...");
		OnDialogClick();
	}

	private DialogFrameRef GetNextFrame()
	{
		if (_dialogFrame == null || _dialogFrame.frameRef == null)
		{
			return null;
		}
		return _dialogRef.getFrame(_dialogFrame.frameRef.id + 1);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}
}
