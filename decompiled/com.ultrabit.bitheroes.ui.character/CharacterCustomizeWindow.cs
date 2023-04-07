using System;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.filter;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterCustomizeWindow : WindowsMain
{
	private const float PUPPET_SCALE = 1.3333334f;

	private const string LEFT_ARROW = "<";

	private const string RIGHT_ARROW = ">";

	[HideInInspector]
	public UnityEvent SELECT = new UnityEvent();

	public Action<string, bool, int, int, int> CREATED_CHARACTER;

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI nameLabelTxt;

	public TextMeshProUGUI genderLabelTxt;

	public TextMeshProUGUI hairIDLabelTxt;

	public TextMeshProUGUI hairColorIDLabelTxt;

	public TextMeshProUGUI skinColorIDLabelTxt;

	public TextMeshProUGUI genderTxt;

	public TextMeshProUGUI hairIDTxt;

	public Image hairColorIDSprite;

	public Image skinColorIDSprite;

	public TMP_InputField nameTxt;

	public Button genderMaleBtn;

	public Button genderFemaleBtn;

	public Button hairIDLeftBtn;

	public Button hairIDRightBtn;

	public Button hairColorIDLeftBtn;

	public Button hairColorIDRightBtn;

	public Button skinColorIDLeftBtn;

	public Button skinColorIDRightBtn;

	public Button randomBtn;

	public Button saveBtn;

	public RectTransform placeholderDisplay;

	private bool _nameChange;

	private string _gender = "";

	private int _hairID;

	private int _hairStyles = 17;

	private int _hairColors = 11;

	private int _hairColorID;

	private int _skinColors = 6;

	private int _skinColorID;

	private string _customconsum = "";

	private CharacterDisplay _display;

	public override void Start()
	{
		string text = "";
		base.Start();
		Disable();
		if (GameData.instance.PROJECT.character != null && GameData.instance.PROJECT.character.imxG0Data == null)
		{
			_gender = GameData.instance.PROJECT.character.gender;
			_hairID = GameData.instance.PROJECT.character.hairID;
			_hairColorID = GameData.instance.PROJECT.character.hairColorID;
			_skinColorID = GameData.instance.PROJECT.character.skinColorID;
			_customconsum = GameData.instance.PROJECT.character.customconsum;
			text = GameData.instance.PROJECT.character.name;
			base.name = ((text.Length > 0) ? text : AppInfo.GetPossibleCharacterName());
		}
		else
		{
			Randomize();
			base.name = "";
		}
		topperTxt.text = Language.GetString("ui_customize");
		nameLabelTxt.text = Language.GetString("ui_name");
		genderLabelTxt.text = Language.GetString("ui_gender");
		hairIDLabelTxt.text = Language.GetString("ui_hair_style");
		hairColorIDLabelTxt.text = Language.GetString("ui_hair_color");
		skinColorIDLabelTxt.text = Language.GetString("ui_skin");
		nameTxt.characterLimit = VariableBook.characterNameLength;
		nameTxt.text = base.name;
		_nameChange = text.Length <= 0;
		if (!_nameChange)
		{
			nameTxt.enabled = false;
		}
		genderMaleBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_male_identifier");
		genderFemaleBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_female_identifier");
		TextMeshProUGUI componentInChildren = hairIDLeftBtn.GetComponentInChildren<TextMeshProUGUI>();
		TextMeshProUGUI componentInChildren2 = hairColorIDLeftBtn.GetComponentInChildren<TextMeshProUGUI>();
		string text3 = (skinColorIDLeftBtn.GetComponentInChildren<TextMeshProUGUI>().text = "<");
		string text5 = (componentInChildren2.text = text3);
		componentInChildren.text = text5;
		TextMeshProUGUI componentInChildren3 = hairIDRightBtn.GetComponentInChildren<TextMeshProUGUI>();
		TextMeshProUGUI componentInChildren4 = hairColorIDRightBtn.GetComponentInChildren<TextMeshProUGUI>();
		text3 = (skinColorIDRightBtn.GetComponentInChildren<TextMeshProUGUI>().text = ">");
		text5 = (componentInChildren4.text = text3);
		componentInChildren3.text = text5;
		randomBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_random");
		saveBtn.GetComponentInChildren<TextMeshProUGUI>().text = (_nameChange ? Language.GetString("ui_create") : Language.GetString("ui_save"));
		if (_nameChange)
		{
			Randomize();
			UpdateDisplay();
		}
		else
		{
			UpdateDisplay();
		}
		CreateWindow();
	}

	public void OnValueChanged()
	{
		for (int i = 0; i < nameTxt.text.Length; i++)
		{
			if (!Util.CharacterNameAllowed(nameTxt.text[i]))
			{
				nameTxt.text = nameTxt.text.Remove(i, 1);
				nameTxt.caretPosition = i;
			}
		}
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		if (_display != null && _display.GetComponent<SortingGroup>() != null)
		{
			_display.GetComponent<SortingGroup>().sortingOrder = 1 + base.sortingLayer;
		}
	}

	private void Randomize()
	{
		_gender = ((Util.randomInt(0, 1) == 1) ? "M" : "F");
		_hairID = Util.randomInt(1, _hairStyles);
		_hairColorID = Util.randomInt(0, _hairColors - 1);
		_skinColorID = Util.randomInt(0, _skinColors - 1);
	}

	private void UpdateDisplay()
	{
		if (_display != null)
		{
			UnityEngine.Object.Destroy(_display.gameObject);
			_display = null;
		}
		CharacterPuppetInfoDefault characterPuppetInfoDefault = new CharacterPuppetInfoDefault(CharacterPuppet.ParseGenderFromString(_gender), _hairID, _hairColorID, _skinColorID, 1.3333334f);
		_display = GameData.instance.windowGenerator.GetCharacterDisplay(characterPuppetInfoDefault);
		_display.SetCharacterDisplay(characterPuppetInfoDefault);
		_display.transform.SetParent(placeholderDisplay, worldPositionStays: false);
		_display.SetLocalPosition(new Vector3(0f, -30f, 0f));
		Util.ChangeLayer(_display.transform, "UI");
		SortingGroup sortingGroup = _display.gameObject.AddComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = 1 + base.sortingLayer;
		genderTxt.text = _gender;
		hairIDTxt.text = _hairID.ToString();
		UpdateHairColorIDSprite();
		UpdateSkinColorIDSprite();
	}

	private void UpdateHairColorIDSprite()
	{
		ColorUtility.TryParseHtmlString(CharacterPuppetDefault.GetHairColor(_hairColorID), out var color);
		hairColorIDSprite.color = color;
	}

	private void UpdateSkinColorIDSprite()
	{
		ColorUtility.TryParseHtmlString(CharacterPuppetDefault.GetSkinColor(_skinColorID), out var color);
		skinColorIDSprite.color = color;
	}

	public void OnGenderMaleBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_gender = "M";
		UpdateDisplay();
	}

	public void OnGenderFemaleBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_gender = "F";
		UpdateDisplay();
	}

	public void OnHairIDLeftBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_hairID = ((_hairID <= 1) ? _hairStyles : (--_hairID));
		UpdateDisplay();
	}

	public void OnHairIDRightBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_hairID = ((_hairID >= _hairStyles) ? 1 : (++_hairID));
		UpdateDisplay();
	}

	public void OnHairColorIDLeftBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_hairColorID = ((_hairColorID <= 0) ? (--_hairColors) : (--_hairColorID));
		UpdateDisplay();
	}

	public void OnHairColorIDRightBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_hairColorID = ((_hairColorID < _hairColors - 1) ? (++_hairColorID) : 0);
		UpdateDisplay();
	}

	public void OnSkinColorIDLeftBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_skinColorID = ((_skinColorID <= 0) ? (--_skinColors) : (--_skinColorID));
		UpdateDisplay();
	}

	public void OnSkinColorIDRightBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_skinColorID = ((_skinColorID < _skinColors - 1) ? (++_skinColorID) : 0);
		UpdateDisplay();
	}

	public void OnRandomizeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		Randomize();
		UpdateDisplay();
	}

	public void OnSaveBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (nameTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_blank_name"));
			return;
		}
		if (!Filter.allow(nameTxt.text))
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_invalid_name"));
			return;
		}
		string text = (_nameChange ? Language.GetString("ui_customize_name_confirm", new string[1] { nameTxt.text }, color: true) : Language.GetString("ui_customize_appearance_confirm"));
		GameData.instance.windowGenerator.NewMessageWindow(DialogWindow.TYPE.TYPE_YES_NO, Language.GetString("ui_confirm"), Util.ParseString(text), base.gameObject, "Character");
	}

	private void OnCustomize(BaseEvent e)
	{
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnCustomize);
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		string utfString = sfsob.GetUtfString("cha2");
		string utfString2 = sfsob.GetUtfString("cha12");
		int @int = sfsob.GetInt("cha20");
		int int2 = sfsob.GetInt("cha21");
		int int3 = sfsob.GetInt("cha22");
		string utfString3 = sfsob.GetUtfString("char107");
		GameData.instance.PROJECT.character.name = utfString;
		GameData.instance.PROJECT.character.gender = utfString2;
		GameData.instance.PROJECT.character.hairID = @int;
		GameData.instance.PROJECT.character.hairColorID = int2;
		GameData.instance.PROJECT.character.skinColorID = int3;
		GameData.instance.PROJECT.character.customconsum = utfString3;
		ItemData itemData = ItemData.fromSFSObject(sfsob);
		if (itemData != null)
		{
			GameData.instance.PROJECT.character.removeItem(itemData);
		}
		SELECT.Invoke();
		SELECT.RemoveAllListeners();
		base.OnClose();
	}

	private void DialogYesRecieverCharacter()
	{
		if (CREATED_CHARACTER != null)
		{
			CREATED_CHARACTER(nameTxt.text, _gender == "M", _hairID, _hairColorID, _skinColorID);
			OnClose();
		}
		else
		{
			GameData.instance.main.ShowLoading(Language.GetString("ui_saving"));
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnCustomize);
			CharacterDALC.instance.doCustomize(nameTxt.text, _gender == "M", _hairID, _hairColorID, _skinColorID, _customconsum);
		}
	}

	private void DialogNoRecieverCharacter()
	{
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		SetButtonsState(state: true);
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		SetButtonsState(state: false);
	}

	private void SetButtonsState(bool state)
	{
		if (genderMaleBtn != null)
		{
			genderMaleBtn.interactable = state;
		}
		if (genderFemaleBtn != null)
		{
			genderFemaleBtn.interactable = state;
		}
		if (hairIDLeftBtn != null)
		{
			hairIDLeftBtn.interactable = state;
		}
		if (hairIDRightBtn != null)
		{
			hairIDRightBtn.interactable = state;
		}
		if (hairColorIDLeftBtn != null)
		{
			hairColorIDLeftBtn.interactable = state;
		}
		if (hairColorIDRightBtn != null)
		{
			hairColorIDRightBtn.interactable = state;
		}
		if (skinColorIDLeftBtn != null)
		{
			skinColorIDLeftBtn.interactable = state;
		}
		if (skinColorIDRightBtn != null)
		{
			skinColorIDRightBtn.interactable = state;
		}
		if (randomBtn != null)
		{
			randomBtn.interactable = state;
		}
		if (saveBtn != null)
		{
			saveBtn.interactable = state;
		}
		if (nameTxt != null)
		{
			nameTxt.interactable = state;
		}
	}
}
