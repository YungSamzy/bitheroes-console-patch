using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterPlatformLinkTile : MonoBehaviour
{
	public TextMeshProUGUI nameTxt;

	public TextMeshProUGUI levelTxt;

	public Image placeholderDisplay;

	public Button selectBtn;

	private CharacterData _characterData;

	private CharacterPlatformLinkWindow _parent;

	[HideInInspector]
	public UnityCustomEvent SELECT = new UnityCustomEvent();

	public CharacterData characterData => _characterData;

	public void LoadDetails(CharacterData characterData, CharacterPlatformLinkWindow parent)
	{
		_characterData = characterData;
		_parent = parent;
		nameTxt.text = characterData.parsedName;
		levelTxt.text = Language.GetString("ui_current_level", new string[1] { Util.NumberFormat(characterData.level) });
		selectBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_select");
		Transform obj = characterData.toCharacterDisplay().transform;
		obj.SetParent(placeholderDisplay.transform);
		obj.localPosition = Vector3.zero;
		obj.localScale *= 0.5f;
		Util.ChangeLayer(obj, "UI");
		SortingGroup sortingGroup = obj.gameObject.AddComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = 5000 + (GameData.instance.windowGenerator.dialogCount - 1) * 10 + 400;
	}

	public void OnSelectBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SELECT.Invoke(this);
	}

	public void OnAssetClick()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowPlayer(_characterData.charID);
	}

	public void DoEnable()
	{
		selectBtn.interactable = true;
	}

	public void DoDisable()
	{
		selectBtn.interactable = false;
	}
}
