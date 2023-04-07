using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildMessageBox : MonoBehaviour
{
	public TextMeshProUGUI nameTxt;

	public TextContainer txtcont;

	public TMP_InputField contentTxt;

	public Button saveBtn;

	public Button clearBtn;

	public Button cancelBtn;

	public Image loadingIcon;

	public TextMeshProUGUI emptyTxt;

	private string _message = "";

	private bool hasPermission = true;

	public Scrollbar scrollbar;

	public void LoadDetails()
	{
		nameTxt.text = Language.GetString("ui_message") + ":";
		contentTxt.characterLimit = VariableBook.guildMessageLimit;
		saveBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_save");
		clearBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_clear");
		cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_cancel");
		contentTxt.text = "";
		loadingIcon.gameObject.SetActive(value: false);
		DoUpdate();
	}

	public void OnValueChanged()
	{
		CheckButtons();
		UpdateEmptyText();
	}

	private void UpdateEmptyText()
	{
		emptyTxt.gameObject.SetActive(contentTxt.text == string.Empty);
	}

	public void OnInputSelected()
	{
		if (GameData.instance.PROJECT.character.guildData.hasPermission(8))
		{
			contentTxt.readOnly = false;
			emptyTxt.gameObject.SetActive(value: false);
			CheckButtons();
		}
		else
		{
			contentTxt.readOnly = true;
		}
	}

	public void OnInputDeselected()
	{
		if (GameData.instance.PROJECT.character.guildData.hasPermission(8))
		{
			UpdateEmptyText();
		}
	}

	public void OnUpBtn()
	{
		Debug.Log(contentTxt.textComponent.isTextOverflowing);
		if (contentTxt.textComponent.isTextOverflowing)
		{
			float num = (scrollbar.value -= 1 / contentTxt.text.Split('\n').Length);
			if (num < 0f)
			{
				num = 0f;
			}
			scrollbar.value = num;
			Debug.Log(num);
		}
	}

	public void OnDownBtn()
	{
		if (contentTxt.textComponent.isTextOverflowing)
		{
			float num = (scrollbar.value += 1 / contentTxt.text.Split('\n').Length);
			if (num > 1f)
			{
				num = 1f;
			}
			scrollbar.value = num;
			Debug.Log(num);
		}
	}

	public void OnScrollBarChanged()
	{
	}

	public void OnSaveBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoSaveMessage(contentTxt.text);
	}

	public void OnClearBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		contentTxt.text = "";
		CheckButtons();
	}

	public void OnCancelBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetMessage(_message);
		ClearFocus();
	}

	private void CheckButtons()
	{
		if (!GameData.instance.PROJECT.character.guildData.hasPermission(8))
		{
			SetButtons(v: false);
		}
		else if (_message == contentTxt.text)
		{
			SetButtons(v: false);
		}
		else
		{
			SetButtons(v: true);
		}
	}

	private void SetButtons(bool v)
	{
		if (saveBtn != null)
		{
			saveBtn.gameObject.SetActive(v);
		}
		if (clearBtn != null)
		{
			clearBtn.gameObject.SetActive(v);
		}
		if (cancelBtn != null)
		{
			cancelBtn.gameObject.SetActive(v);
		}
		if (nameTxt != null)
		{
			nameTxt.gameObject.SetActive(!v);
		}
	}

	public void SetMessage(string message)
	{
		_message = message;
		string text = message.Replace("\r", "\r\n");
		contentTxt.text = text;
		UpdateEmptyText();
		CheckButtons();
	}

	public void DoUpdate()
	{
		if (GameData.instance.PROJECT.character.guildData.hasPermission(8))
		{
			contentTxt.interactable = true;
			emptyTxt.text = Language.GetString("ui_guild_messagebox_empty", new string[1] { Tutorial.GetCursorText() });
		}
		else
		{
			contentTxt.interactable = false;
			emptyTxt.text = Language.GetString("ui_guild_messagebox_unavailable");
		}
		CheckButtons();
	}

	private void ClearFocus()
	{
	}

	private void DoSaveMessage(string message)
	{
		ClearFocus();
		DoDisable();
		GameData.instance.main.ShowLoading();
		string message2 = message.Replace("\r\n", "\r");
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(38), OnSaveMessage);
		GuildDALC.instance.doSaveMessage(message2);
	}

	private void OnSaveMessage(BaseEvent e)
	{
		DoEnable();
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(38), OnSaveMessage);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		string utfString = sfsob.GetUtfString("gui19");
		SetMessage(utfString);
	}

	public void DoEnable()
	{
		saveBtn.interactable = true;
		clearBtn.interactable = true;
		cancelBtn.interactable = true;
		contentTxt.interactable = true;
	}

	public void DoDisable()
	{
		saveBtn.interactable = false;
		clearBtn.interactable = false;
		cancelBtn.interactable = false;
		contentTxt.interactable = false;
	}
}
