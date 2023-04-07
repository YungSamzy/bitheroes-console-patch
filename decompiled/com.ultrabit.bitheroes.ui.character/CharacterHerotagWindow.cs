using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterHerotagWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI nameTxt;

	public Button changeBtn;

	public Button helpBtn;

	private string _name;

	private string _herotag;

	private bool _changeable;

	private CharacterHerotagChangeWindow _characterHerotagChange;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(string name, string herotag, bool changeable = false)
	{
		_name = name;
		_herotag = herotag;
		_changeable = changeable;
		topperTxt.text = Language.GetString("ui_herotag_title");
		nameTxt.text = _name + "<color=#9FA9B5>#" + _herotag + "</color>";
		if (_changeable)
		{
			changeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_change");
			helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		}
		else
		{
			changeBtn.gameObject.SetActive(value: false);
			helpBtn.gameObject.SetActive(value: false);
		}
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnCharacterHerotagChange(object e)
	{
		_characterHerotagChange.NAME_CHANGE.RemoveListener(OnCharacterHerotagChange);
		_name = _characterHerotagChange.currentName;
		nameTxt.text = _name + "<color=#9FA9B5>#" + _herotag + "</color>";
	}

	private void OnCharacterHerotagChangeClosed(object e)
	{
		_characterHerotagChange.DESTROYED.RemoveListener(OnCharacterHerotagChangeClosed);
		_characterHerotagChange.NAME_CHANGE.RemoveListener(OnCharacterHerotagChange);
		_characterHerotagChange = null;
	}

	public void OnChangeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_characterHerotagChange = GameData.instance.windowGenerator.NewCharacterHerotagChangeWindow();
		_characterHerotagChange.NAME_CHANGE.AddListener(OnCharacterHerotagChange);
		_characterHerotagChange.DESTROYED.AddListener(OnCharacterHerotagChangeClosed);
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(Language.GetString("herotag_help_title"), Util.parseMultiLine(Language.GetString("herotag_help_desc")), base.gameObject);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		changeBtn.interactable = true;
		helpBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		changeBtn.interactable = false;
		helpBtn.interactable = false;
	}
}
