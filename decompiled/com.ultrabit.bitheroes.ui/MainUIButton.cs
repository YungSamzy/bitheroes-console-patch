using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui;

public class MainUIButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public enum BUTTONTYPE
	{
		NONE,
		QUEST,
		PVP,
		RAID,
		FISHING,
		GAUNTLET,
		FAMILIARS,
		DAILY,
		DAILYBONUS,
		SERVICE,
		CRAFT,
		GVG,
		SHOP,
		FRIEND,
		GUILD,
		PLAYER,
		SETTINGS,
		INVASION,
		RIFT,
		SERVICE_CREDITS,
		SERVICE_GOLD,
		SERVICE_ENERGY,
		SERVICE_TICKETS,
		BRAWL,
		CHAT,
		BATTLE_CONSUMABLE
	}

	public BUTTONTYPE type;

	public TextMeshProUGUI buttonTxt;

	private Button _windowBtn;

	private HoverImages _hoverImages;

	private CurrencyBarFill _currencyBarFill;

	private GameRequirement _requirement;

	protected bool hasRequirementException;

	private bool _available = true;

	private bool _grayscale;

	public bool available => _available;

	public bool grayscale => _grayscale;

	public GameRequirement requirement => _requirement;

	private void Awake()
	{
		_windowBtn = base.gameObject.GetComponent<Button>();
		_windowBtn.onClick.AddListener(onClicked);
		_currencyBarFill = base.transform.GetComponentInChildren<CurrencyBarFill>();
		_hoverImages = GetComponent<HoverImages>();
	}

	public virtual void Create()
	{
	}

	public void LoadDetails(string text = "", GameRequirement requirement = null)
	{
		buttonTxt.text = text;
		SetRequirement(requirement);
		if (_hoverImages != null)
		{
			_hoverImages.ForceStart();
			_hoverImages.GetOwnTexts();
		}
		DoUpdate();
	}

	public virtual void DoUpdate()
	{
		if (_requirement == null || _requirement.RequirementsMet() || hasRequirementException)
		{
			EnableButton();
		}
		else
		{
			DisableButton();
		}
	}

	public void EnableButton()
	{
		if (!_available)
		{
			_available = true;
			base.gameObject.SetActive(value: true);
		}
	}

	public void DisableButton()
	{
		if (_available)
		{
			_available = false;
			base.gameObject.SetActive(value: false);
		}
	}

	public virtual void DoClick()
	{
	}

	public void onClicked()
	{
		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
		{
			return;
		}
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (MetGameRequirement() && available)
		{
			switch (type)
			{
			case BUTTONTYPE.SERVICE:
				GameData.instance.windowGenerator.NewServiceWindow();
				break;
			case BUTTONTYPE.SERVICE_CREDITS:
				GameData.instance.windowGenerator.ShowServices(0);
				break;
			case BUTTONTYPE.SERVICE_GOLD:
				GameData.instance.windowGenerator.ShowServices(1);
				break;
			case BUTTONTYPE.SERVICE_ENERGY:
				GameData.instance.windowGenerator.ShowServices(2);
				break;
			case BUTTONTYPE.SERVICE_TICKETS:
				GameData.instance.windowGenerator.ShowServices(3);
				break;
			}
			DoClick();
		}
	}

	public void OnPointerEnter(PointerEventData pointer)
	{
		if ((type == BUTTONTYPE.SERVICE || type == BUTTONTYPE.SERVICE_CREDITS || type == BUTTONTYPE.SERVICE_GOLD || type == BUTTONTYPE.SERVICE_ENERGY || type == BUTTONTYPE.SERVICE_TICKETS) && _currencyBarFill != null)
		{
			_currencyBarFill.OnFocus();
		}
	}

	public void OnPointerExit(PointerEventData pointer)
	{
		if ((type == BUTTONTYPE.SERVICE || type == BUTTONTYPE.SERVICE_CREDITS || type == BUTTONTYPE.SERVICE_GOLD || type == BUTTONTYPE.SERVICE_ENERGY || type == BUTTONTYPE.SERVICE_TICKETS) && _currencyBarFill != null)
		{
			_currencyBarFill.OnLostFocus();
		}
	}

	public void SetRequirement(GameRequirement requirement)
	{
		_requirement = requirement;
	}

	private bool MetGameRequirement()
	{
		if (_requirement != null && !hasRequirementException)
		{
			string requirementsText = _requirement.GetRequirementsText();
			if (requirementsText != null)
			{
				DialogRef dialogLocked = _requirement.GetDialogLocked();
				if (dialogLocked != null)
				{
					GameData.instance.windowGenerator.NewDialogPopup(dialogLocked);
				}
				else
				{
					GameData.instance.windowGenerator.ShowError(requirementsText);
				}
				return false;
			}
		}
		return true;
	}

	public void AddGrayscale()
	{
		_grayscale = true;
		_hoverImages.AddGrayscale();
	}

	public void ClearGrayscale()
	{
		_grayscale = false;
		_hoverImages.ClearGrayscale();
	}

	public void UpdateGrayscale()
	{
		if (_grayscale)
		{
			ClearGrayscale();
			AddGrayscale();
		}
	}

	public virtual void OnDestroy()
	{
		ClearGrayscale();
		_windowBtn.onClick.RemoveListener(onClicked);
	}
}
