using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.lists.abilitylist;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.familiar;

public class FamiliarSkillsPanel : FamiliarPanel
{
	public AbilityList abilityList;

	public GameObject abilityListView;

	public GameObject abilityListScroll;

	private FamiliarWindow _familiarWindow;

	private FamiliarRef _familiarRef;

	private bool _skillsEnabled = true;

	public bool skillsEnabled => _skillsEnabled;

	public void LoadDetails(FamiliarWindow familiarWindow, FamiliarRef familiarRef)
	{
		_familiarWindow = familiarWindow;
		_familiarRef = familiarRef;
		abilityList.InitList(this, OnTileSelected);
		CreateAbilityList();
	}

	public override void DoUpdate()
	{
		if (_familiarWindow.currentTab == 0)
		{
			CreateAbilityList();
		}
	}

	private void CreateAbilityList()
	{
		abilityList.ClearList();
		if (_familiarRef.abilities == null)
		{
			return;
		}
		foreach (AbilityRef ability in _familiarRef.abilities)
		{
			abilityList.Data.InsertOneAtEnd(new AbilityListModel
			{
				abilityRef = ability,
				power = _familiarRef.getPower(GameData.instance.PROJECT.character.getTotalStats()),
				bonus = 0f,
				clickable = _skillsEnabled,
				enabled = _skillsEnabled
			});
		}
	}

	private void OnTileSelected(AbilityRef abilityRef)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SWFAsset asset = _familiarWindow.asset;
		if (!(asset == null))
		{
			asset.StopAnimation();
			if (asset.PlayAnimation(abilityRef.animation))
			{
				ToggleTiles(enabled: false);
				asset.ANIMATION_END.AddListener(OnAnimationEnd);
			}
			else
			{
				asset.PlayAnimation("idle");
			}
		}
	}

	private void OnAnimationEnd(SWFAsset swfAsset)
	{
		swfAsset.ANIMATION_END.RemoveListener(OnAnimationEnd);
		ToggleTiles(enabled: true);
	}

	public void ToggleTiles(bool enabled)
	{
		_skillsEnabled = enabled;
		abilityList.Refresh();
	}

	public override void DoShow()
	{
		base.DoShow();
		abilityListView.SetActive(value: true);
		abilityListScroll.SetActive(value: true);
	}

	public override void DoHide()
	{
		base.DoHide();
		abilityListView.SetActive(value: false);
		abilityListScroll.SetActive(value: false);
	}

	public void Remove()
	{
		if (_familiarWindow.familiarAsset != null)
		{
			if (_familiarWindow.asset != null)
			{
				_familiarWindow.asset.ANIMATION_END.RemoveListener(OnAnimationEnd);
			}
			Object.Destroy(_familiarWindow.familiarAsset.gameObject);
		}
	}
}
