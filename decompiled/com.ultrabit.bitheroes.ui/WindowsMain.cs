using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.chat;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui;

public class WindowsMain : MonoBehaviour
{
	public const float SAFE_MARGIN = 200f;

	public Button closeBtn;

	public GameObject panel;

	public string nextScene;

	private float initialPosY;

	private bool _scroll;

	private Vector3 _posFinal;

	private float _speedIn;

	private float _speedOut;

	private bool _sound;

	[HideInInspector]
	public UnityCustomEvent CREATED = new UnityCustomEvent();

	[HideInInspector]
	public UnityCustomEvent SCROLL_IN_START = new UnityCustomEvent();

	[HideInInspector]
	public UnityCustomEvent SCROLL_IN_ANY_START = new UnityCustomEvent();

	[HideInInspector]
	public UnityCustomEvent SCROLL_IN_COMPLETE = new UnityCustomEvent();

	[HideInInspector]
	public UnityCustomEvent SCROLL_OUT_START = new UnityCustomEvent();

	[HideInInspector]
	public UnityCustomEvent DESTROYED = new UnityCustomEvent();

	private GameObject _dialogParent;

	private bool _disabled;

	private bool _scrollingOut;

	private bool _scrollingIn;

	private bool _isShown;

	private UnityAction _backFunc;

	private UnityAction _forwardFunc;

	private bool _stayUp;

	private int _layer = -1;

	protected int _sortingLayer;

	protected bool forceAnimation;

	public AsianLanguageFontManager asianLangManager;

	private bool animStarded;

	private bool endAnimStarted;

	public int layer
	{
		get
		{
			return _layer;
		}
		set
		{
			_layer = value;
		}
	}

	public int sortingLayer => _sortingLayer;

	public string SetNextScene
	{
		get
		{
			return nextScene;
		}
		set
		{
			nextScene = value;
		}
	}

	public bool disabled => _disabled;

	public bool scrollingOut
	{
		get
		{
			return _scrollingOut;
		}
		set
		{
			_scrollingOut = value;
		}
	}

	public bool scrollingIn
	{
		get
		{
			return _scrollingIn;
		}
		set
		{
			_scrollingIn = value;
		}
	}

	public bool isShown => _isShown;

	public GameObject dialogParent
	{
		get
		{
			return _dialogParent;
		}
		set
		{
			_dialogParent = value;
		}
	}

	public bool stayUp => _stayUp;

	private void Awake()
	{
		CheckAsianFont();
	}

	public virtual void Start()
	{
	}

	public void CreateWindow(bool closeWord = false, string buttonWord = "", bool scroll = true, bool stayUp = false, float speedIn = 1f, float speedOut = 1f, bool sound = true)
	{
		_scroll = scroll;
		_speedIn = speedIn;
		_speedOut = speedOut;
		_sound = sound;
		_stayUp = stayUp;
		CREATED.Invoke(this);
		if (closeBtn != null)
		{
			TextMeshProUGUI componentInChildren = closeBtn.GetComponentInChildren<TextMeshProUGUI>();
			if (componentInChildren != null)
			{
				if (!closeWord)
				{
					componentInChildren.text = Language.GetString("ui_x");
				}
				else if (buttonWord == "")
				{
					componentInChildren.text = Language.GetString("ui_close");
				}
				else
				{
					componentInChildren.text = buttonWord;
				}
			}
		}
		if (_scroll)
		{
			AnimateIn(isUp: false, _posFinal, _speedIn);
			SCROLL_IN_START.Invoke(this);
			scrollingIn = true;
			CheckSoundIn();
		}
		else if (stayUp)
		{
			_posFinal = panel.transform.localPosition;
			panel.transform.localPosition += new Vector3(0f, (float)Screen.height + 200f, 0f);
		}
		else
		{
			_isShown = true;
		}
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = base.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
	}

	public virtual void UpdateSortingLayers(int layer)
	{
		_sortingLayer = layer;
		Canvas[] componentsInChildren = GetComponentsInChildren<Canvas>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].sortingLayerName = "UI";
				componentsInChildren[i].sortingOrder = componentsInChildren[i].sortingOrder % 100 + layer;
				if (componentsInChildren[i].gameObject.GetComponent<GraphicRaycaster>() == null)
				{
					componentsInChildren[i].gameObject.AddComponent<GraphicRaycaster>();
				}
			}
		}
		Canvas canvas = base.gameObject.GetComponent<Canvas>();
		if (canvas == null)
		{
			canvas = base.gameObject.AddComponent<Canvas>();
			base.gameObject.AddComponent<GraphicRaycaster>();
		}
		canvas.overrideSorting = true;
		canvas.sortingLayerName = "UI";
		canvas.sortingOrder = layer;
	}

	public void ForceScrollDown(bool forceBg = false, GameObject bg = null)
	{
		_scroll = true;
		if (forceBg && bg != null)
		{
			bg.SetActive(value: true);
			GetComponent<Animator>().SetBool("onDown", value: true);
		}
		AnimateIn(isUp: true, _posFinal, _speedIn);
		SCROLL_IN_START.Invoke(this);
		scrollingIn = true;
		CheckSoundIn();
		if (GetComponent<ChatWindow>() != null)
		{
			GameData.instance.windowGenerator.chatVisible = true;
		}
	}

	public void DoShow()
	{
		if (_scroll)
		{
			AnimateIn(isUp: true, _posFinal, _speedIn);
			scrollingIn = true;
			GetComponent<Animator>().SetBool("onDown", value: true);
			SCROLL_IN_START.Invoke(this);
			CheckSoundIn();
		}
	}

	public void DoHide()
	{
		if (_scroll)
		{
			AnimateOut(_speedOut, destroyOnEnd: false);
			_scrollingOut = true;
			GetComponent<Animator>().SetBool("onDown", value: false);
			CheckSoundOut();
		}
	}

	public void ActivateCloseBtn()
	{
		if (closeBtn != null && closeBtn.gameObject != null)
		{
			closeBtn.onClick.AddListener(OnCloseBtn);
			if (!closeBtn.IsInteractable())
			{
				closeBtn.interactable = true;
			}
		}
	}

	public void DeactivateCloseBtn()
	{
		if (closeBtn != null)
		{
			closeBtn.onClick.RemoveListener(OnCloseBtn);
			closeBtn.interactable = false;
		}
	}

	private void OnCloseBtn()
	{
		EventSystem.current.SetSelectedGameObject(null);
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		OnClose();
	}

	public virtual void OnClose()
	{
		SCROLL_IN_COMPLETE.RemoveAllListeners();
		if (this == null)
		{
			return;
		}
		if (closeBtn != null && closeBtn.gameObject != null && closeBtn.onClick != null)
		{
			closeBtn.onClick.RemoveListener(OnCloseBtn);
		}
		if (_scroll)
		{
			AnimateOut(_speedOut);
			_scrollingOut = true;
			if (SCROLL_OUT_START != null)
			{
				SCROLL_OUT_START.Invoke(this);
			}
			CheckSoundOut();
		}
		if (base.gameObject != null && GetComponent<Animator>() != null)
		{
			GetComponent<Animator>().SetBool("onDown", value: false);
		}
		if (base.gameObject != null && GetComponent<ChatWindow>() != null)
		{
			GameData.instance.windowGenerator.chatVisible = false;
		}
	}

	public virtual void CloseWithoutConfirmation(bool avoid)
	{
		if (!avoid)
		{
			if (closeBtn != null)
			{
				closeBtn.onClick.RemoveListener(OnCloseBtn);
			}
			if (_scroll)
			{
				AnimateOut(_speedOut);
				_scrollingOut = true;
				SCROLL_OUT_START.Invoke(this);
				CheckSoundOut();
			}
			else
			{
				DoDestroy();
			}
			GetComponent<Animator>().SetBool("onDown", value: false);
			if (GetComponent<ChatWindow>() != null)
			{
				GameData.instance.windowGenerator.chatVisible = false;
			}
		}
	}

	private void SetSize(int width, int height)
	{
		panel.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
	}

	private void CheckSoundIn()
	{
		if (_sound && GameData.instance.windowGenerator.dialogCount <= 1)
		{
			GameData.instance.audioManager.PlaySoundLink("scrollin");
		}
	}

	private void CheckSoundOut()
	{
		if (_sound && GameData.instance.windowGenerator != null && GameData.instance.windowGenerator.dialogCount <= 1 && GameData.instance.audioManager != null)
		{
			GameData.instance.audioManager.PlaySoundLink("scrollout");
		}
	}

	public Image AddBG(float alpha = 0.75f)
	{
		GameObject obj = new GameObject("bg", typeof(RectTransform), typeof(Image));
		obj.transform.SetParent(base.transform, worldPositionStays: false);
		obj.transform.SetAsFirstSibling();
		RectTransform component = obj.GetComponent<RectTransform>();
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.localPosition = Vector2.zero;
		component.sizeDelta = Vector2.zero;
		Image component2 = obj.GetComponent<Image>();
		component2.color = new Color(0f, 0f, 0f, alpha);
		return component2;
	}

	public static void CheckAsianFont(GameObject gameObject)
	{
		AsianLanguageFontManager asianLanguageFontManager = gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLanguageFontManager == null)
		{
			asianLanguageFontManager = gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLanguageFontManager != null)
		{
			asianLanguageFontManager.SetAsianFontsIfNeeded();
		}
	}

	public void CheckAsianFont(bool overrideResized = false)
	{
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = base.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
	}

	private void OnDestroy()
	{
		if (closeBtn != null)
		{
			closeBtn.onClick.RemoveListener(OnCloseBtn);
		}
	}

	public virtual void DoDestroy()
	{
		DESTROYED.Invoke(this);
		if (base.isActiveAndEnabled && base.gameObject != null)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void InvokeDestroyedWithoutDestroying()
	{
		DESTROYED.Invoke(this);
	}

	public virtual void Disable()
	{
		_disabled = true;
	}

	public virtual void Enable()
	{
		_disabled = false;
	}

	public void ListenForBack(UnityAction func)
	{
		if (_backFunc == null)
		{
			_backFunc = func;
		}
	}

	public bool DoBack()
	{
		if (_backFunc == null || GameData.instance.tutorialManager.hasPopup || disabled || scrollingIn || scrollingOut)
		{
			return false;
		}
		_backFunc();
		return true;
	}

	public void ClearBackListener()
	{
		if (_backFunc != null)
		{
			_backFunc = null;
		}
	}

	public void ListenForForward(UnityAction func)
	{
		if (_forwardFunc == null)
		{
			_forwardFunc = func;
		}
	}

	public bool DoForward()
	{
		if (_forwardFunc == null || GameData.instance.tutorialManager.hasPopup || disabled || scrollingIn || scrollingOut)
		{
			return false;
		}
		_forwardFunc();
		return true;
	}

	public void ClearForwardListener()
	{
		if (_forwardFunc != null)
		{
			_forwardFunc = null;
		}
	}

	public virtual void DoStartWindow()
	{
	}

	public void AnimateIn(bool isUp, Vector3 posFinal, float speed = 1f)
	{
		animStarded = false;
		if (AppInfo.TESTING)
		{
			speed *= 3f;
		}
		if (panel == null || panel.transform == null)
		{
			scrollingIn = false;
			Enable();
			DoStartWindow();
			return;
		}
		if (!isUp)
		{
			posFinal = panel.transform.localPosition;
			panel.transform.localPosition += new Vector3(0f, (float)Screen.height + 200f, 0f);
		}
		Vector3 localPosition = panel.GetComponent<RectTransform>().localPosition;
		panel.transform.localPosition = localPosition;
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, panel.transform.DOLocalMove(posFinal -= new Vector3(0f, (float)Screen.height * 0.1f, 0f), 0.25f / speed));
		sequence.Insert(0.25f / speed, panel.transform.DOLocalMove(posFinal += new Vector3(0f, (float)Screen.height * 0.1f, 0f), 0.25f / speed));
		sequence.SetEase(Ease.OutQuad);
		sequence.OnComplete(OnCompleteAnimIn);
	}

	private void OnCompleteAnimIn()
	{
		scrollingIn = false;
		SCROLL_IN_COMPLETE.Invoke(null);
		_isShown = true;
		Enable();
		DoStartWindow();
	}

	private void CheckStartAnimation()
	{
		if (!animStarded)
		{
			AnimateIn(isUp: true, _posFinal, _speedIn);
		}
	}

	public void AnimateOut(float speed = 1f, bool destroyOnEnd = true)
	{
		_isShown = false;
		if (AppInfo.TESTING)
		{
			speed *= 3f;
		}
		Disable();
		if (panel == null || panel.transform == null)
		{
			return;
		}
		Vector3 localPosition = panel.transform.localPosition;
		panel.transform.localPosition += new Vector3(0f, (float)Screen.height + 200f, 0f);
		Vector3 localPosition2 = panel.GetComponent<RectTransform>().localPosition;
		if (panel != null && panel.transform != null)
		{
			panel.transform.localPosition = localPosition;
			Sequence sequence = DOTween.Sequence();
			sequence.Insert(0f, panel.transform.DOLocalMove(localPosition -= new Vector3(0f, (float)Screen.height * 0.1f, 0f), 0.25f / speed));
			sequence.Insert(0.25f / speed, panel.transform.DOLocalMove(localPosition2, 0.25f / speed));
			sequence.SetEase(Ease.OutQuad);
			sequence.OnComplete(delegate
			{
				Disable();
				if (!(panel == null) && !(panel.transform == null))
				{
					scrollingOut = false;
					if (panel != null && destroyOnEnd)
					{
						DoDestroy();
					}
				}
			});
		}
		else
		{
			Disable();
			panel.transform.localPosition = localPosition2;
			scrollingOut = false;
			if (panel != null && destroyOnEnd)
			{
				DoDestroy();
			}
		}
	}
}
