using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.game;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.ui.battle;

public class BattleDamageGainTile : GameTextSprite, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public TextMeshProUGUI gainTxt;

	public UnityEvent SELECT = new UnityEvent();

	private void Awake()
	{
		LoadGameTextSprite(GetTooltipText(), click: false, 4, base.gameObject, 0f, 70f);
	}

	public void SetDamageGain(int gain)
	{
		gainTxt.text = Util.NumberFormat(gain);
		SetTooltipText(GetTooltipText(gain));
	}

	public string GetTooltipText(int gain = 0)
	{
		return Util.colorString(Language.GetString("battle_enrage") + ":", "#FFFF00") + "<br>" + Util.ParseString(Language.GetString("battle_enrage_desc", new string[1] { Util.NumberFormat(gain) }, color: true));
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
	}

	public new void OnPointerClick(PointerEventData eventData)
	{
		if (!AppInfo.IsMobile() || !base.active)
		{
			SetActive(active: false);
			SELECT.Invoke();
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	private void OnDisable()
	{
		SetActive(active: false);
	}
}
