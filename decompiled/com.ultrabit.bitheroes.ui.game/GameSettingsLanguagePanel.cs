using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.game;

public class GameSettingsLanguagePanel : MonoBehaviour
{
	public GameObject gameSettingsLanguageContent;

	public Transform checkBoxPrefab;

	private float listWidth;

	private List<CheckBoxTile> checkBoxes = new List<CheckBoxTile>();

	private LanguageRef requestedLanguage;

	private AsianLanguageFontManager asianLangManager;

	private void Start()
	{
		listWidth = GetComponent<RectTransform>().sizeDelta.x;
		CreateCheckboxes();
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

	private void CreateCheckboxes()
	{
		LanguageRef currentLanguageRef = LanguageBook.GetCurrentLanguageRef();
		for (int i = 0; i < LanguageBook.size; i++)
		{
			LanguageRef languageRef = LanguageBook.Lookup(i);
			if (languageRef != null)
			{
				Transform transform = Object.Instantiate(checkBoxPrefab);
				transform.SetParent(gameSettingsLanguageContent.transform, worldPositionStays: false);
				transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(languageRef.name, languageRef), languageRef.name == currentLanguageRef.name, changable: true, listWidth);
				transform.GetComponent<CheckBoxTile>().AddOnClickedCallback(OnCheckBox);
				checkBoxes.Add(transform.GetComponent<CheckBoxTile>());
			}
		}
		UpdateCheckboxes(new CheckBoxTile.CheckBoxObject(currentLanguageRef.name, currentLanguageRef));
	}

	private void OnCheckBox(CheckBoxTile.CheckBoxObject checkBoxData)
	{
		if (LanguageBook.GetCurrentLanguageRef() != checkBoxData.objectRef as LanguageRef)
		{
			UpdateCheckboxes(checkBoxData);
			requestedLanguage = checkBoxData.objectRef as LanguageRef;
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_language_change_confirm", new string[1] { requestedLanguage.name }), null, null, delegate
			{
				GameData.instance.SAVE_STATE.language = requestedLanguage.lang;
				GameData.instance.main.Logout(relog: true, reloadXMLfiles: true);
			});
		}
	}

	private void UpdateCheckboxes(CheckBoxTile.CheckBoxObject selectedRef)
	{
		LanguageRef languageRef = selectedRef.objectRef as LanguageRef;
		foreach (CheckBoxTile checkBox in checkBoxes)
		{
			LanguageRef languageRef2 = checkBox.data.objectRef as LanguageRef;
			checkBox.SetChecked(languageRef2.name == languageRef.name, dispatch: false);
		}
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void DoEnable()
	{
		for (int i = 0; i < checkBoxes.Count; i++)
		{
			checkBoxes[i].toggle.interactable = true;
		}
	}

	public void DoDisable()
	{
		for (int i = 0; i < checkBoxes.Count; i++)
		{
			checkBoxes[i].toggle.interactable = false;
		}
	}
}
