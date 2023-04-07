using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.Util.ItemDragging;
using com.ultrabit.bitheroes.model.language;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.teamgrouplist;

public class MyViewsHolder : BaseItemViewsHolder
{
	public DraggableItem draggableComponent;

	public RectTransform scalableViews;

	public RectTransform teamObject;

	public RectTransform addBtn;

	public TextMeshProUGUI btnTxt;

	public TextMeshProUGUI numberTxt;

	public TextMeshProUGUI nameTxt;

	public TextMeshProUGUI powerTxt;

	public TextMeshProUGUI staminaTxt;

	public TextMeshProUGUI agilityTxt;

	public RectTransform upBtn;

	public RectTransform downBtn;

	public RectTransform removeBtn;

	public Image background;

	public Image avatar;

	public RectTransform mask;

	public RectTransform teamGroupTile;

	public override void CollectViews()
	{
		base.CollectViews();
		draggableComponent = root.GetComponent<DraggableItem>();
		root.GetComponentAtPath<RectTransform>("ScalableViews", out scalableViews);
		root.GetComponent<DraggableItem>().longClickTime = 1E+11f;
		scalableViews.GetComponentAtPath<Image>("Background", out background);
		scalableViews.GetComponentAtPath<RectTransform>("TeamGroupTile", out teamGroupTile);
		scalableViews.GetComponentAtPath<RectTransform>("TeamObject", out teamObject);
		scalableViews.GetComponentAtPath<RectTransform>("AddBtn", out addBtn);
		addBtn.GetComponentAtPath<TextMeshProUGUI>("BtnTxt", out btnTxt);
		btnTxt.text = Language.GetString("ui_add");
		scalableViews.GetComponentAtPath<TextMeshProUGUI>("NumberTxt", out numberTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out nameTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("PowerTxt", out powerTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("StaminaTxt", out staminaTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("AgilityTxt", out agilityTxt);
		teamObject.GetComponentAtPath<RectTransform>("UpBtn", out upBtn);
		teamObject.GetComponentAtPath<RectTransform>("DownBtn", out downBtn);
		teamObject.GetComponentAtPath<RectTransform>("RemoveBtn", out removeBtn);
		teamObject.GetComponentAtPath<RectTransform>("PlayerImageMask", out mask);
		mask.GetComponentAtPath<Image>("PlayerImage", out avatar);
	}
}
