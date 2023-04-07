using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.admin;

public class AdminRenameWindow : WindowsMain
{
	public const int TYPE_CHARACTER_NAME = 0;

	public const int TYPE_GUILD_NAME = 1;

	public const int TYPE_GUILD_INITIALS = 2;

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI currentNameTxt;

	public TextMeshProUGUI renamingTxt;

	public Button renameBtn;

	public TMP_InputField renameTxt;

	private int _type;

	private string _herotag;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(string currentName, int type, string herotag = "")
	{
		_type = type;
		_herotag = ((herotag != "") ? ("#" + herotag) : "");
		currentNameTxt.text = currentName;
		renameTxt.text = "";
		switch (_type)
		{
		case 0:
			renameTxt.characterLimit = VariableBook.characterNameLength;
			break;
		case 1:
			renameTxt.characterLimit = VariableBook.guildNameLength;
			break;
		case 2:
			renameTxt.characterLimit = VariableBook.guildInitialsLength;
			break;
		}
		topperTxt.text = Language.GetString("ui_rename");
		renamingTxt.text = Language.GetString("ui_renaming") + ":";
		renameBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_save");
		Debug.LogWarning("Check InputText Submit on mobile");
		renameTxt.onSubmit.AddListener(DoRenameConfirm);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnRenameBtn(string args)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoRenameConfirm();
	}

	public void OnValueChanged()
	{
		switch (_type)
		{
		case 0:
		{
			for (int j = 0; j < renameTxt.text.Length; j++)
			{
				if (!Util.CharacterNameAllowed(renameTxt.text[j]))
				{
					renameTxt.text = renameTxt.text.Remove(j, 1);
					renameTxt.caretPosition = j;
				}
			}
			break;
		}
		case 1:
		{
			for (int k = 0; k < renameTxt.text.Length; k++)
			{
				if (!Util.GuildNameAllowed(renameTxt.text[k]))
				{
					renameTxt.text = renameTxt.text.Remove(k, 1);
					renameTxt.caretPosition = k;
				}
			}
			break;
		}
		case 2:
		{
			for (int i = 0; i < renameTxt.text.Length; i++)
			{
				if (!Util.GuildInitialsAllowed(renameTxt.text[i]))
				{
					renameTxt.text = renameTxt.text.Remove(i, 1);
					renameTxt.caretPosition = i;
				}
			}
			break;
		}
		}
	}

	private void DoRenameConfirm(string args = null)
	{
		int type = _type;
		if ((uint)(type - 1) <= 1u)
		{
			DoRename();
			return;
		}
		if (renameTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_name"));
			return;
		}
		if (renameTxt.text == currentNameTxt.text)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_same_name"));
			return;
		}
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_rename"), Language.GetString("ui_rename_confirm", new string[2] { currentNameTxt.text, renameTxt.text }, color: true), null, null, delegate
		{
			OnRenameConfirm();
		});
	}

	private void OnRenameConfirm()
	{
		DoRename();
	}

	private void DoRename()
	{
		switch (_type)
		{
		case 0:
			DoCharacterRename();
			break;
		case 1:
			DoGuildRename();
			break;
		case 2:
			DoGuildReinitials();
			break;
		}
	}

	private void DoCharacterRename()
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(14), OnCharacterRename);
		AdminDALC.instance.doCharacterRename(currentNameTxt.text + _herotag, renameTxt.text);
	}

	private void OnCharacterRename(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(14), OnCharacterRename);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_success"), Language.GetString("ui_rename_success", new string[2] { currentNameTxt.text, renameTxt.text }, color: true));
		AdminCharacterWindow adminCharacterWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AdminCharacterWindow)) as AdminCharacterWindow;
		if (adminCharacterWindow != null)
		{
			adminCharacterWindow.DoRefresh();
		}
		OnClose();
	}

	private void DoGuildRename()
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(25), OnGuildRename);
		AdminDALC.instance.doGuildRename(currentNameTxt.text, renameTxt.text);
	}

	private void OnGuildRename(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(25), OnGuildRename);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		AdminGuildWindow adminGuildWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AdminGuildWindow)) as AdminGuildWindow;
		if (adminGuildWindow != null)
		{
			adminGuildWindow.DoRefresh();
		}
		OnClose();
	}

	private void DoGuildReinitials()
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(26), OnGuildReinitials);
		AdminDALC.instance.doGuildInitials(currentNameTxt.text, renameTxt.text);
	}

	private void OnGuildReinitials(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(26), OnGuildReinitials);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		AdminGuildWindow adminGuildWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AdminGuildWindow)) as AdminGuildWindow;
		if (adminGuildWindow != null)
		{
			adminGuildWindow.DoRefresh();
		}
		OnClose();
	}

	public override void DoDestroy()
	{
		renameTxt.onSubmit.RemoveListener(DoRenameConfirm);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		renameBtn.interactable = true;
		renameTxt.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		renameBtn.interactable = false;
		renameTxt.interactable = false;
	}
}
