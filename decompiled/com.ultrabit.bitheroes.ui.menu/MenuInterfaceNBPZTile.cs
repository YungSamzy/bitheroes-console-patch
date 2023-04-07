using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.variable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceNBPZTile : MainUIButton
{
	public TextMeshProUGUI badge;

	public Image badge_bg;

	private bool updatedBadge;

	private int noView;

	public override void Create()
	{
		LoadDetails(Language.GetString("ui_btn_nbpz"), VariableBook.GetGameRequirement(6));
		UpdateBadge();
	}

	public void HideBagde()
	{
		badge.gameObject.SetActive(value: false);
		badge_bg.gameObject.SetActive(value: false);
	}

	public override void DoUpdate()
	{
		if (updatedBadge)
		{
			if (base.requirement != null && !base.requirement.RequirementsMet())
			{
				HideBagde();
				DisableButton();
			}
			else
			{
				UpdateNum();
				EnableButton();
			}
		}
	}

	private void UpdateNum()
	{
		List<string> boostersIdInZone = PaymentBook.GetBoostersIdInZone();
		List<string> boosterBadge = GameData.instance.SAVE_STATE.GetBoosterBadge(GameData.instance.PROJECT.character.id);
		noView = 0;
		foreach (string item in boostersIdInZone)
		{
			if (boosterBadge.IndexOf(item) == -1)
			{
				noView++;
			}
		}
		if (noView > 0)
		{
			badge.gameObject.SetActive(value: true);
			badge_bg.gameObject.SetActive(value: true);
			badge.text = noView.ToString();
			if (GameData.instance.SAVE_STATE.boosterBadgeNotification < noView && GameData.instance.PROJECT.character.tutorial.GetState(62))
			{
				StartCoroutine(WaitForBooterNotification(noView));
			}
		}
		else
		{
			GameData.instance.SAVE_STATE.boosterBadgeNotification = 0;
			HideBagde();
		}
	}

	private IEnumerator WaitForBooterNotification(int pNoview)
	{
		yield return new WaitForSeconds(1f);
		GameData.instance.SAVE_STATE.boosterBadgeNotification = noView;
		GameData.instance.PROJECT.NotificationNewBoosters(pNoview - GameData.instance.SAVE_STATE.boosterBadgeNotification);
	}

	public void UpdateBadge()
	{
		UpdateNum();
		updatedBadge = true;
		DoUpdate();
	}

	public override void DoClick()
	{
		base.DoClick();
		if (noView > 0)
		{
			List<string> boostersIdInZone = PaymentBook.GetBoostersIdInZone();
			List<string> boosterBadge = GameData.instance.SAVE_STATE.GetBoosterBadge(GameData.instance.PROJECT.character.id);
			foreach (string item in boostersIdInZone)
			{
				if (boosterBadge.IndexOf(item) == -1)
				{
					boosterBadge.Add(item);
				}
			}
			GameData.instance.SAVE_STATE.SetBoosterBadge(GameData.instance.PROJECT.character.id, boosterBadge);
			noView = 0;
			HideBagde();
		}
		GameData.instance.windowGenerator.ShowNBPZ();
	}
}
