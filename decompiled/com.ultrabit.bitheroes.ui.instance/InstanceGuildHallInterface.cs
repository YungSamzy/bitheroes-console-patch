using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.instance;

public class InstanceGuildHallInterface : WindowsMain
{
	public TextMeshProUGUI nameTxt;

	public Button inventoryBtn;

	public Image border;

	public Image nameplate;

	public Transform guildHallEditBarPrefab;

	private Instance _instance;

	private InstanceGuildHallEditBar _editBar;

	private GuildData _data;

	public InstanceGuildHallEditBar editBar => _editBar;

	public override void Start()
	{
		base.Start();
	}

	public void LoadDetails(Instance instance)
	{
		Disable();
		_instance = instance;
		nameTxt.text = Language.GetString("ui_edit_mode");
		nameTxt.ForceMeshUpdate();
		StartCoroutine(WaitToFixText());
		Transform transform = Object.Instantiate(guildHallEditBarPrefab);
		transform.SetParent(base.transform, worldPositionStays: false);
		_editBar = transform.GetComponent<InstanceGuildHallEditBar>();
		_editBar.LoadDetails(_instance, this);
		Vector2 sizeDelta = GameData.instance.windowGenerator.canvas.GetComponent<RectTransform>().sizeDelta;
		border.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x / 3f, sizeDelta.y / 3f);
		DoLoadData();
		CreateWindow(closeWord: false, "", scroll: false);
		Enable();
	}

	private IEnumerator WaitToFixText()
	{
		yield return new WaitForEndOfFrame();
		if (nameplate.GetComponent<RectTransform>().sizeDelta.x > 93f)
		{
			nameplate.GetComponent<HorizontalLayoutGroup>().childControlWidth = false;
			nameTxt.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60f, nameTxt.gameObject.GetComponent<RectTransform>().sizeDelta.y);
		}
	}

	public void OnInventoryBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewGuildInventoryWindow(_data);
	}

	private void DoLoadData()
	{
		SetData();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnLoadData);
		GuildDALC.instance.doLoadData();
	}

	public void OnLoadData(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnLoadData);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GuildData data = GuildData.fromSFSObject(sfsob);
		SetData(data);
	}

	private void SetData(GuildData data = null)
	{
		_data = data;
		if (_editBar != null)
		{
			_editBar.SetData(data);
		}
		if (_data != null)
		{
			SetButtons(enabled: true);
			Enable(addCloseListener: false);
		}
		else
		{
			SetButtons(enabled: false);
			Disable(removeCloseListener: false);
		}
	}

	public void SetButtons(bool enabled)
	{
		Util.SetButton(inventoryBtn, enabled);
		Util.SetButton(closeBtn, enabled);
	}

	public override void OnClose()
	{
		GameData.instance.PROJECT.ToggleGuildHallEditMode();
	}

	private void Enable(bool addCloseListener = true)
	{
		if (addCloseListener)
		{
			ActivateCloseBtn();
		}
		inventoryBtn.interactable = true;
		if (_editBar != null)
		{
			_editBar.DoEnable();
		}
	}

	private void Disable(bool removeCloseListener = true)
	{
		if (removeCloseListener)
		{
			DeactivateCloseBtn();
		}
		inventoryBtn.interactable = false;
		if (_editBar != null)
		{
			_editBar.DoDisable();
		}
	}
}
