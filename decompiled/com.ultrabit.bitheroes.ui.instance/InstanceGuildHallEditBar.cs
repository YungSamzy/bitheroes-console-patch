using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.instance;

public class InstanceGuildHallEditBar : MonoBehaviour
{
	private const float SAVE_DELAY = 500f;

	public Button typeLeftBtn;

	public Button typeRightBtn;

	public Button cosmeticLeftBtn;

	public Button cosmeticRightBtn;

	public Image typeDropdown;

	public Image cosmeticIcon;

	private Instance _instance;

	private InstanceGuildHallInterface _hallInterface;

	private Coroutine _saveTimer;

	private GuildData _data;

	private List<InstanceObject> _glowingObjects = new List<InstanceObject>();

	private GuildHallCosmeticTypeRef _selectedCosmetic;

	private Transform dropdownWindow;

	private List<MyDropdownItemModel> _typeDropdownList = new List<MyDropdownItemModel>();

	public void LoadDetails(Instance instance, InstanceGuildHallInterface hallInterface)
	{
		_instance = instance;
		_hallInterface = hallInterface;
		CreateDropdowns();
		SetData();
	}

	public void SaveData()
	{
		if (_instance.data is GuildHallData)
		{
			GuildHallData guildHallData = _instance.data as GuildHallData;
			GuildDALC.instance.doUpdateHallCosmetics(guildHallData.cosmetics);
		}
		ClearSaveTimer();
	}

	public void SetData(GuildData data = null)
	{
		_data = data;
		if (_data != null)
		{
			DoEnable();
		}
		else
		{
			DoDisable();
		}
		UpdateSelectedType();
		UpdateSelectedCosmetic();
	}

	public void CreateDropdowns()
	{
		for (int i = 0; i < GuildHallBook.sizeCosmeticTypes; i++)
		{
			GuildHallCosmeticTypeRef guildHallCosmeticTypeRef = GuildHallBook.LookupCosmeticTypeID(i);
			if (guildHallCosmeticTypeRef != null && !guildHallCosmeticTypeRef.autoSelect)
			{
				if (_selectedCosmetic == null)
				{
					_selectedCosmetic = guildHallCosmeticTypeRef;
				}
				_typeDropdownList.Add(new MyDropdownItemModel
				{
					id = guildHallCosmeticTypeRef.id,
					title = guildHallCosmeticTypeRef.displayName,
					desc = guildHallCosmeticTypeRef.locationName,
					btnHelp = false,
					data = guildHallCosmeticTypeRef
				});
			}
		}
		typeDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _selectedCosmetic.name;
	}

	public GuildHallCosmeticTypeRef GetSelectedType()
	{
		return _selectedCosmetic;
	}

	public GuildHallCosmeticRef GetSelectedCosmetic()
	{
		GuildHallCosmeticTypeRef selectedType = GetSelectedType();
		if (_instance.data is GuildHallData)
		{
			return GuildHallBook.LookupCosmetic((_instance.data as GuildHallData).getCosmetic(selectedType.id), selectedType.id);
		}
		return null;
	}

	public void AdjustSelectedType(int change)
	{
		GuildHallCosmeticTypeRef selectedType = GetSelectedType();
		if (selectedType == null)
		{
			return;
		}
		int num = selectedType.id + change;
		GuildHallCosmeticTypeRef guildHallCosmeticTypeRef = _typeDropdownList[0].data as GuildHallCosmeticTypeRef;
		GuildHallCosmeticTypeRef guildHallCosmeticTypeRef2 = _typeDropdownList[_typeDropdownList.Count - 1].data as GuildHallCosmeticTypeRef;
		if (num < guildHallCosmeticTypeRef.id)
		{
			num = guildHallCosmeticTypeRef2.id;
		}
		if (num > guildHallCosmeticTypeRef2.id)
		{
			num = guildHallCosmeticTypeRef.id;
		}
		foreach (MyDropdownItemModel typeDropdown in _typeDropdownList)
		{
			if ((typeDropdown.data as GuildHallCosmeticTypeRef).id == num)
			{
				OnDropdownChange(typeDropdown);
				break;
			}
		}
	}

	public void AdjustSelectedCosmetic(int change)
	{
		ClearGlowingObjects();
		GuildHallCosmeticRef selectedCosmetic = GetSelectedCosmetic();
		List<GuildHallCosmeticRef> orderedCosmetics = GuildHallBook.GetOrderedCosmetics(selectedCosmetic.type, (_data == null) ? 1 : _data.level);
		int num = GetCosmeticIndex(selectedCosmetic, orderedCosmetics) + change;
		GuildHallCosmeticRef guildHallCosmeticRef = ((num >= 0 && num < orderedCosmetics.Count) ? orderedCosmetics[num] : null);
		if (guildHallCosmeticRef == null)
		{
			guildHallCosmeticRef = ((change > 0) ? orderedCosmetics[0] : orderedCosmetics[orderedCosmetics.Count - 1]);
		}
		if (guildHallCosmeticRef != null && _instance.data is GuildHallData)
		{
			_instance.UpdateObjectLinks(add: false);
			(_instance.data as GuildHallData).setCosmetic(guildHallCosmeticRef.type, guildHallCosmeticRef.id);
			UpdateSelectedCosmetic();
			_instance.UpdateData();
			StartSaveTimer();
		}
	}

	public void OnTypeDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		dropdownWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_area"));
		DropdownList componentInChildren = dropdownWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedCosmetic.id, OnDropdownChange);
		componentInChildren.Data.InsertItemsAtStart(_typeDropdownList);
	}

	private void OnDropdownChange(MyDropdownItemModel model)
	{
		_selectedCosmetic = model.data as GuildHallCosmeticTypeRef;
		typeDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _selectedCosmetic.name;
		UpdateSelectedType();
		UpdateSelectedCosmetic();
		if (dropdownWindow != null)
		{
			dropdownWindow.GetComponent<DropdownWindow>().OnClose();
			dropdownWindow = null;
		}
	}

	public void OnScroll(BaseEventData baseEventData)
	{
		PointerEventData pointerEventData = baseEventData as PointerEventData;
		AdjustSelectedType(Mathf.RoundToInt(0f - pointerEventData.scrollDelta.y));
	}

	public void UpdateSelectedType(bool glow = true)
	{
		GuildHallCosmeticTypeRef selectedType = GetSelectedType();
		List<InstanceObject> list = new List<InstanceObject>();
		foreach (InstanceObject @object in _instance.objects)
		{
			if (!(@object == null) && @object.data is InstanceObjectRef)
			{
				InstanceObjectRef instanceObjectRef = @object.data as InstanceObjectRef;
				if (instanceObjectRef.type == 16 && instanceObjectRef.value == selectedType.link)
				{
					list.Add(@object);
				}
			}
		}
		if (glow)
		{
			SetGlowingObjects(list);
		}
	}

	public void UpdateSelectedCosmetic()
	{
		GuildHallCosmeticRef selectedCosmetic = GetSelectedCosmetic();
		if (selectedCosmetic != null)
		{
			List<GuildHallCosmeticRef> orderedCosmetics = GuildHallBook.GetOrderedCosmetics(selectedCosmetic.type, (_data == null) ? 1 : _data.level);
			int cosmeticIndex = GetCosmeticIndex(selectedCosmetic, orderedCosmetics);
			if (cosmeticIndex <= 0)
			{
				Util.SetButton(cosmeticLeftBtn, enabled: false);
			}
			else
			{
				Util.SetButton(cosmeticLeftBtn);
			}
			if (cosmeticIndex >= orderedCosmetics.Count - 1)
			{
				Util.SetButton(cosmeticRightBtn, enabled: false);
			}
			else
			{
				Util.SetButton(cosmeticRightBtn);
			}
			SetAsset(selectedCosmetic.GetSpriteIcon());
		}
	}

	private int GetCosmeticIndex(GuildHallCosmeticRef cosmetic, List<GuildHallCosmeticRef> cosmetics)
	{
		for (int i = 0; i < cosmetics.Count; i++)
		{
			if (cosmetics[i].id == cosmetic.id && cosmetics[i].type == cosmetic.type)
			{
				return i;
			}
		}
		return 0;
	}

	private void SetGlowingObjects(List<InstanceObject> objects)
	{
		ClearGlowingObjects();
		_glowingObjects = objects;
		foreach (InstanceObject glowingObject in _glowingObjects)
		{
			if (glowingObject != null)
			{
				HoverAndGlowSprites hoverAndGlowSprites = glowingObject.gameObject.GetComponent<HoverAndGlowSprites>();
				if (hoverAndGlowSprites == null)
				{
					hoverAndGlowSprites = glowingObject.gameObject.AddComponent<HoverAndGlowSprites>();
					hoverAndGlowSprites.GetTargetAssets();
				}
				hoverAndGlowSprites.StartGlow();
			}
		}
	}

	public void ClearGlowingObjects()
	{
		foreach (InstanceObject glowingObject in _glowingObjects)
		{
			if (glowingObject != null)
			{
				HoverAndGlowSprites component = glowingObject.GetComponent<HoverAndGlowSprites>();
				if (component != null)
				{
					component.StopGlowing();
				}
			}
		}
		_glowingObjects.Clear();
	}

	private void SetAsset(Sprite sprite)
	{
		cosmeticIcon.overrideSprite = sprite;
	}

	private void StartSaveTimer()
	{
		if (_saveTimer != null)
		{
			GameData.instance.main.coroutineTimer.RestartTimer(_saveTimer);
		}
		else
		{
			_saveTimer = GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 500f, OnSaveTimer);
		}
		_hallInterface.SetButtons(enabled: false);
	}

	private void StopSaveTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _saveTimer);
	}

	private void ClearSaveTimer()
	{
		StopSaveTimer();
	}

	private void OnSaveTimer()
	{
		SaveData();
		_hallInterface.SetButtons(enabled: true);
	}

	public void OnTypeLeftBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		AdjustSelectedType(-1);
	}

	public void OnTypeRightBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		AdjustSelectedType(1);
	}

	public void OnCosmeticLeftBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		AdjustSelectedCosmetic(-1);
	}

	public void OnCosmeticRightBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		AdjustSelectedCosmetic(1);
	}

	public void DoEnable()
	{
		typeLeftBtn.interactable = true;
		typeRightBtn.interactable = true;
		cosmeticLeftBtn.interactable = true;
		cosmeticRightBtn.interactable = true;
		typeDropdown.GetComponent<EventTrigger>().enabled = true;
		GetComponent<EventTrigger>().enabled = true;
	}

	public void DoDisable()
	{
		typeLeftBtn.interactable = false;
		typeRightBtn.interactable = false;
		cosmeticLeftBtn.interactable = false;
		cosmeticRightBtn.interactable = false;
		typeDropdown.GetComponent<EventTrigger>().enabled = false;
		GetComponent<EventTrigger>().enabled = false;
	}
}
