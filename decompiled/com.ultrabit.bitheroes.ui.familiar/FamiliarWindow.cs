using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.gve;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.invasion;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.raid;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.game;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.familiar;

public class FamiliarWindow : WindowsMain
{
	public const int TAB_SKILLS = 0;

	public const int TAB_AUGMENTS = 1;

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public TextMeshProUGUI powerTxt;

	public TextMeshProUGUI staminaTxt;

	public TextMeshProUGUI agilityTxt;

	public Button helpBtn;

	public Button searchBtn;

	public Button skillsBtn;

	public Button augmentsBtn;

	public GameObject placeholderModifiers;

	public GameObject BonusBtn;

	public RectTransform placeholderAsset;

	public Image loadingTile;

	public Transform familiarSkillsPanelPrefab;

	public Transform familiarAugmentsPanelPrefab;

	public GameModifierBtn gameModifierBtnPrefab;

	public GameObject UIFusionBackground;

	public GameObject UIFamiliarBackground;

	public Button walkBtn;

	public Button hitBtn;

	public Button idleBtn;

	private Transform _familiarAsset;

	private FamiliarRef _familiarRef;

	private bool _owned;

	private bool _mine;

	private CharacterData _sourceCharacterData;

	private SWFAsset _asset;

	private FamiliarSkillsPanel _skillsPanel;

	private FamiliarAugmentsPanel _augmentsPanel;

	private List<GameModifierBtn> _modifiers;

	private List<Button> _tabs;

	private List<FamiliarPanel> _panels = new List<FamiliarPanel>();

	private int _currentTab = -1;

	private Transform window;

	public Transform familiarAsset => _familiarAsset;

	public int currentTab => _currentTab;

	public FamiliarAugmentsPanel augmentsPanel => _augmentsPanel;

	public SWFAsset asset => _asset;

	public bool showAugmentTab
	{
		get
		{
			if (!_mine || !_owned)
			{
				return _sourceCharacterData != null;
			}
			return true;
		}
	}

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(FamiliarRef familiarRef, bool mine = false, CharacterData sourceCharacterData = null)
	{
		_familiarRef = familiarRef;
		_owned = GameData.instance.PROJECT.character.inventory.hasOwnedItem(_familiarRef);
		_mine = mine;
		_sourceCharacterData = sourceCharacterData;
		UIFusionBackground.SetActive(_familiarRef.isFusion());
		UIFamiliarBackground.SetActive(!_familiarRef.isFusion());
		int totalStats = GameData.instance.PROJECT.character.getTotalStats();
		topperTxt.text = familiarRef.coloredName;
		descTxt.text = Util.ParseItemString(familiarRef.desc, familiarRef, _familiarRef.getPower(totalStats), GameModifier.getTypeTotal(_familiarRef.modifiers, 17));
		powerTxt.text = Util.NumberFormat(_familiarRef.getPower(totalStats));
		staminaTxt.text = Util.NumberFormat(_familiarRef.getStamina(totalStats));
		agilityTxt.text = Util.NumberFormat(_familiarRef.getAgility(totalStats));
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		SetTabButtons();
		if (GameData.instance.PROJECT.character.hasAugmentOrHad())
		{
			SetTab(showAugmentTab ? 1 : 0);
		}
		else
		{
			SetTab();
			skillsBtn.transform.position = augmentsBtn.transform.position;
		}
		CreateAsset();
		CreateModifiers();
		if (mine)
		{
			GameData.instance.PROJECT.character.AddListener("AUGMENTS_CHANGE", OnAugmentsChange);
			GameData.instance.PROJECT.character.augments.OnChange.AddListener(OnAugmentsChange);
		}
		walkBtn.gameObject.SetActive(GameData.instance.PROJECT.character.admin);
		hitBtn.gameObject.SetActive(GameData.instance.PROJECT.character.admin);
		idleBtn.gameObject.SetActive(GameData.instance.PROJECT.character.admin);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		if (_familiarAsset != null)
		{
			_familiarAsset.GetComponent<SortingGroup>().sortingOrder = base.sortingLayer + 1;
		}
	}

	public void DoUpdate()
	{
		foreach (FamiliarPanel panel in _panels)
		{
			if ((bool)panel)
			{
				panel.DoUpdate();
			}
		}
	}

	private void SetTabButtons()
	{
		if (_tabs == null)
		{
			_tabs = new List<Button>();
			SetTabButton(skillsBtn, 0);
			SetTabButton(augmentsBtn, 1, showAugmentTab);
		}
	}

	private void SetTabButton(Button button, int tab, bool enabled = true)
	{
		button.GetComponentInChildren<TextMeshProUGUI>().text = GetTabName(tab);
		if (enabled)
		{
			while (_tabs.Count <= tab)
			{
				_tabs.Add(null);
			}
			_tabs[tab] = button;
			button.onClick.AddListener(delegate
			{
				OnTabButtonClicked(button);
			});
		}
		else
		{
			Util.SetButton(button, enabled: false);
		}
	}

	private void OnTabButtonClicked(Button button)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		for (int i = 0; i < _tabs.Count; i++)
		{
			if (button.GetInstanceID() == _tabs[i].GetInstanceID())
			{
				SetTab(i);
				break;
			}
		}
	}

	public FamiliarPanel GetPanel(int id)
	{
		if (id < 0 || id >= _panels.Count)
		{
			return null;
		}
		return _panels[id];
	}

	private void SetTab(int id = 0)
	{
		for (int i = 0; i < _tabs.Count; i++)
		{
			Util.SetTab(_tabs[i], i == id);
		}
		FamiliarPanel familiarPanel = GetPanel(_currentTab);
		if ((bool)familiarPanel)
		{
			familiarPanel.DoHide();
		}
		_currentTab = id;
		FamiliarPanel skillsPanel = GetPanel(_currentTab);
		if (!skillsPanel)
		{
			switch (_currentTab)
			{
			case 0:
			{
				Transform transform2 = Object.Instantiate(familiarSkillsPanelPrefab);
				transform2.SetParent(panel.transform, worldPositionStays: false);
				transform2.GetComponent<FamiliarSkillsPanel>().DoShow();
				transform2.GetComponent<FamiliarSkillsPanel>().LoadDetails(this, _familiarRef);
				_skillsPanel = transform2.GetComponent<FamiliarSkillsPanel>();
				skillsPanel = _skillsPanel;
				break;
			}
			case 1:
			{
				Transform transform = Object.Instantiate(familiarAugmentsPanelPrefab);
				transform.SetParent(panel.transform, worldPositionStays: false);
				transform.GetComponent<FamiliarAugmentsPanel>().DoShow();
				Augments augments = GameData.instance.PROJECT.character.augments;
				if (!_mine && _sourceCharacterData != null && _sourceCharacterData.augments != null)
				{
					augments = _sourceCharacterData.augments;
				}
				transform.GetComponent<FamiliarAugmentsPanel>().LoadDetails(this, _familiarRef, augments);
				_augmentsPanel = transform.GetComponent<FamiliarAugmentsPanel>();
				skillsPanel = _augmentsPanel;
				break;
			}
			}
			while (_panels.Count <= _currentTab)
			{
				_panels.Add(null);
			}
			_panels[_currentTab] = skillsPanel;
		}
		if ((bool)skillsPanel)
		{
			skillsPanel.DoShow();
		}
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(ItemRef.GetItemNamePlural(15), Util.parseMultiLine(Language.GetString("augment_help_desc")));
	}

	public void OnSearchBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ShowSearchDropdown();
	}

	private void CreateAsset()
	{
		if (!(_familiarAsset != null))
		{
			_familiarAsset = _familiarRef.displayRef.getAsset(center: true, 2f / 3f, placeholderAsset.transform);
			if (_familiarAsset != null)
			{
				OnAssetLoaded();
			}
		}
	}

	private void OnAssetLoaded()
	{
		loadingTile.gameObject.SetActive(value: false);
		_asset = _familiarAsset.gameObject.AddComponent<SWFAsset>();
		if (_asset != null)
		{
			_asset.PlayAnimation("idle");
		}
		Util.ChangeLayer(_familiarAsset.transform, "UI");
		SortingGroup sortingGroup = _familiarAsset.gameObject.AddComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = base.sortingLayer + 1;
		if (_familiarRef.obtainable && !_owned && !AppInfo.TESTING)
		{
			TintAsset(Color.black);
		}
		CharacterDisplay componentInChildren = _asset.GetComponentInChildren<CharacterDisplay>();
		if (componentInChildren != null)
		{
			componentInChildren.SetLocalPosition(new Vector3(0f, -45f, 0f));
		}
	}

	private void TintAsset(Color color)
	{
		Material material = Resources.Load<Material>("Shader/SpriteTintBlack");
		SpriteRenderer[] componentsInChildren = _familiarAsset.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		if (componentsInChildren != null && componentsInChildren.Length != 0)
		{
			SpriteRenderer[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].material = material;
			}
		}
		ParticleSystem[] componentsInChildren2 = _familiarAsset.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
		if (componentsInChildren2 != null && componentsInChildren2.Length != 0)
		{
			ParticleSystem[] array2 = componentsInChildren2;
			for (int i = 0; i < array2.Length; i++)
			{
				ParticleSystem.MainModule main = array2[i].main;
				main.startColor = new ParticleSystem.MinMaxGradient(color);
			}
		}
	}

	private void CreateModifiers()
	{
		if (_modifiers != null)
		{
			return;
		}
		_modifiers = new List<GameModifierBtn>();
		List<GameModifier> modifiers = _familiarRef.modifiers;
		if (modifiers == null || modifiers.Count <= 0)
		{
			return;
		}
		foreach (GameModifier item in modifiers)
		{
			if (item.tooltip && (item.type != 0 || (item.desc != null && item.desc.Length > 0)))
			{
				GameModifierBtn gameModifierBtn = Object.Instantiate(gameModifierBtnPrefab);
				gameModifierBtn.transform.SetParent(placeholderModifiers.transform, worldPositionStays: false);
				gameModifierBtn.SetText(item.GetTileDesc());
				_modifiers.Add(gameModifierBtn);
			}
		}
	}

	private void OnAugmentsChange()
	{
		DoUpdate();
	}

	public static string GetTabName(int tab)
	{
		return tab switch
		{
			0 => Language.GetString("ability_plural_name"), 
			1 => ItemRef.GetItemNamePlural(15), 
			_ => "?", 
		};
	}

	private void ShowSearchDropdown()
	{
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(64), OnGetFamiliarEncounterInfo);
		CharacterDALC.instance.doGetFamiliarEncounterInfo(_familiarRef.id);
	}

	private void OnGetFamiliarEncounterInfo(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(64), OnGetFamiliarEncounterInfo);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			D.LogError(string.Format("FamiliarWindow::onGetFamiliarEncounterInfo::{0}", sfsob.GetInt("err0")));
		}
		else
		{
			if (!sfsob.ContainsKey("fam3"))
			{
				return;
			}
			List<MyDropdownItemModel> list = new List<MyDropdownItemModel>();
			ISFSArray sFSArray = sfsob.GetSFSArray("fam3");
			for (int i = 0; i < sFSArray.Size(); i++)
			{
				FamiliarEncounterItem familiarEncounterItem = FamiliarEncounterItem.fromSFSObject(sFSArray.GetSFSObject(i));
				switch (familiarEncounterItem.type)
				{
				case 1:
				{
					ZoneNodeRef nodeRef = ZoneBook.Lookup(familiarEncounterItem.zoneID).getNodeRef(familiarEncounterItem.nodeID);
					MyDropdownItemModel item = new MyDropdownItemModel
					{
						id = i,
						title = nodeRef.name,
						data = nodeRef
					};
					list.Add(item);
					break;
				}
				case 4:
				{
					RaidRef raidRef = RaidBook.LookUp(familiarEncounterItem.zoneID);
					MyDropdownItemModel item = new MyDropdownItemModel
					{
						id = i,
						title = raidRef.name,
						data = raidRef
					};
					list.Add(item);
					break;
				}
				case 9:
				{
					BrawlRef brawlRef = BrawlBook.Lookup(familiarEncounterItem.zoneID);
					MyDropdownItemModel item = new MyDropdownItemModel
					{
						id = i,
						title = brawlRef.name,
						data = brawlRef
					};
					list.Add(item);
					break;
				}
				case 11:
				{
					GvEZoneRef gvEZoneRef = GvEEventBook.LookupZone(familiarEncounterItem.zoneID);
					GvEEventRef gvEEventRef = GvEEventBook.GetCurrentEventRef();
					if (!GameData.instance.PROJECT.CheckGameRequirement(35) || gvEEventRef == null || !(gvEEventRef.name == gvEZoneRef.name))
					{
						gvEEventRef = null;
					}
					MyDropdownItemModel item = new MyDropdownItemModel
					{
						id = i,
						title = gvEZoneRef.getNodeRef(familiarEncounterItem.nodeID).name,
						data = gvEEventRef
					};
					list.Add(item);
					break;
				}
				case 8:
				{
					InvasionEventRef invasionEventRef = InvasionEventBook.GetCurrentEventRef();
					if (!GameData.instance.PROJECT.CheckGameRequirement(33) || invasionEventRef == null || !(invasionEventRef.name == familiarEncounterItem.link))
					{
						invasionEventRef = null;
					}
					MyDropdownItemModel item = new MyDropdownItemModel
					{
						id = i,
						title = Language.GetString(familiarEncounterItem.link),
						data = invasionEventRef
					};
					list.Add(item);
					break;
				}
				}
			}
			window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_view_familiar_find_me"));
			DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
			componentInChildren.StartList(base.gameObject, -1, OnSelectZone);
			componentInChildren.Data.InsertItemsAtEnd(list);
		}
	}

	private void OnSelectZone(MyDropdownItemModel model)
	{
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
		if (GameData.instance.PROJECT.battle != null || GameData.instance.PROJECT.dungeon != null)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("ui_view_familiar_must_finish_battle"));
		}
		else if (model.data is ZoneNodeRef)
		{
			ZoneNodeRef zoneNodeRef = model.data as ZoneNodeRef;
			if (GameData.instance.PROJECT.character.zones.nodeIsUnlocked(zoneNodeRef))
			{
				if (zoneNodeRef.difficulties.Count <= 0 || !zoneNodeRef.getActive())
				{
					GameData.instance.windowGenerator.ShowError(Language.GetString("ui_coming_soon"));
				}
				else if (zoneNodeRef.difficulties.Count == 1)
				{
					GameData.instance.windowGenerator.NewZoneNodeSingleWindow(zoneNodeRef, null, null);
				}
				else
				{
					GameData.instance.windowGenerator.NewZoneNodeDifficultyWindow(zoneNodeRef, null, null);
				}
			}
			else
			{
				GameData.instance.windowGenerator.ShowError(Language.GetString("ui_view_familiar_must_unlock_zone_dungeon", new string[2]
				{
					zoneNodeRef.getZoneRef().name,
					zoneNodeRef.name
				}, color: true));
			}
		}
		else if (model.data is RaidRef)
		{
			RaidRef raidRef = model.data as RaidRef;
			if (GameData.instance.PROJECT.CheckGameRequirement(3) && raidRef.getUnlocked())
			{
				GameData.instance.windowGenerator.NewRaidDifficultyWindow(raidRef.id, null);
				return;
			}
			ZoneRef zoneRef = ZoneBook.Lookup(raidRef.requiredZone);
			if (zoneRef != null)
			{
				GameData.instance.windowGenerator.ShowError(Language.GetString("ui_view_familiar_must_complete_zone", new string[1] { zoneRef.name }, color: true));
			}
		}
		else if (model.data is BrawlRef)
		{
			BrawlRef brawlRef = model.data as BrawlRef;
			if (GameData.instance.PROJECT.CheckGameRequirement(34) && brawlRef.requirementsMet())
			{
				GameData.instance.windowGenerator.NewBrawlCreateDifficultyWindow(brawlRef);
				return;
			}
			GameRequirement gameRequirement = VariableBook.GetGameRequirement(34);
			if (gameRequirement != null)
			{
				GameData.instance.windowGenerator.ShowError(gameRequirement.GetRequirementsText());
			}
		}
		else if (model.data is GvEEventRef)
		{
			GameData.instance.PROJECT.ShowGvEWindow();
		}
		else if (model.data is InvasionEventRef)
		{
			GameData.instance.PROJECT.ShowInvasionWindow();
		}
		else
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("ui_view_familiar_event_inactive"));
		}
	}

	public void OnWalkBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		OnWalkOrHit("walk");
	}

	public void OnHitBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		OnWalkOrHit("hit");
	}

	public void OnIdleBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		OnWalkOrHit("idle");
	}

	public void OnWalkOrHit(string animation)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (!(asset == null))
		{
			asset.StopAnimation();
			asset.PlayAnimation(animation);
		}
	}

	public override void DoDestroy()
	{
		if (_skillsPanel != null)
		{
			_skillsPanel.Remove();
		}
		if (_mine)
		{
			GameData.instance.PROJECT.character.augments.OnChange.RemoveListener(OnAugmentsChange);
			GameData.instance.PROJECT.character.RemoveListener("AUGMENTS_CHANGE", OnAugmentsChange);
		}
		if (_tabs != null)
		{
			foreach (Button tab in _tabs)
			{
				if (tab != null)
				{
					tab.onClick.RemoveListener(delegate
					{
						OnTabButtonClicked(tab);
					});
				}
			}
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		helpBtn.interactable = true;
		searchBtn.interactable = true;
		if (_tabs == null)
		{
			return;
		}
		foreach (Button tab in _tabs)
		{
			if (tab != null)
			{
				tab.interactable = true;
			}
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		helpBtn.interactable = false;
		searchBtn.interactable = false;
		if (_tabs == null)
		{
			return;
		}
		foreach (Button tab in _tabs)
		{
			if (tab != null)
			{
				tab.interactable = false;
			}
		}
	}
}
