using com.ultrabit.bitheroes.core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class CheckBoxTile : MonoBehaviour
{
	public class CheckBoxObject
	{
		public int id;

		public string text;

		public object objectRef;

		public CheckBoxObject(string text, object objectRef = null)
		{
			this.text = text;
			this.objectRef = objectRef;
		}

		public CheckBoxObject(string text, int id)
		{
			this.text = text;
			this.id = id;
		}
	}

	public TextMeshProUGUI labelTxt;

	public Toggle toggle;

	private bool _enabled = true;

	private bool _changable = true;

	private string _callerLink;

	private CheckBoxObject _data;

	private GameObject _broadcastReciever;

	private UnityAction<CheckBoxObject> onClickedCallback;

	private bool started;

	private int veces;

	public bool isChecked
	{
		get
		{
			return toggle.isOn;
		}
		set
		{
			toggle.SetIsOnWithoutNotify(value);
		}
	}

	public string text
	{
		get
		{
			return labelTxt.text;
		}
		set
		{
			labelTxt.text = value;
		}
	}

	public CheckBoxObject data => _data;

	private void Start()
	{
		started = true;
	}

	public void Create(CheckBoxObject data, bool isOn = false, bool changable = true, float width = 0f, string callerLink = "", GameObject broadcastReciever = null)
	{
		_changable = changable;
		_data = data;
		SetText(data.text);
		_callerLink = callerLink;
		_broadcastReciever = broadcastReciever;
		isChecked = isOn;
		base.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, base.gameObject.GetComponent<RectTransform>().sizeDelta.y);
	}

	public void AddOnClickedCallback(UnityAction<CheckBoxObject> onClickedCallback)
	{
		this.onClickedCallback = onClickedCallback;
	}

	public void SetText(string text)
	{
		labelTxt.text = text;
	}

	public void SetChecked(bool flag, bool dispatch = true)
	{
		toggle.SetIsOnWithoutNotify(flag);
		if (dispatch)
		{
			Broadcast();
		}
	}

	public void OnValueChanged()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (started)
		{
			Broadcast();
		}
	}

	private void Broadcast()
	{
		if (_broadcastReciever != null)
		{
			_broadcastReciever.BroadcastMessage("OnToggleChange" + _callerLink);
		}
		if (onClickedCallback != null)
		{
			onClickedCallback(data);
		}
	}
}
