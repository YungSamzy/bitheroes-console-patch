using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.payment;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.service;

public class ServiceOfferwallSelectWindow : WindowsMain
{
	public TextMeshProUGUI titleText;

	public TextMeshProUGUI descText;

	public Button ironSourcePaymentTile;

	public Image ironSourceTitleImage;

	public TextMeshProUGUI ironSourceDescText;

	public TextMeshProUGUI ironSourceCostText;

	public Button revUPaymentTile;

	public Image revUTitleImage;

	public TextMeshProUGUI revUDescText;

	public TextMeshProUGUI revUCostText;

	private PaymentData ironSourcePaymentData;

	private PaymentData revUPaymentData;

	public void LoadDetails(List<PaymentData> offerwallsData)
	{
		if (offerwallsData != null && offerwallsData.Count > 0)
		{
			titleText.text = Language.GetString("ui_offerwall_select_title");
			descText.text = Language.GetString("ui_offerwall_select_desc");
			foreach (PaymentData offerwallsDatum in offerwallsData)
			{
				if (offerwallsDatum.paymentRef.link != null && !(offerwallsDatum.paymentRef.link == ""))
				{
					if (offerwallsDatum.paymentRef.link == "offerwall_ironsource")
					{
						ironSourcePaymentData = offerwallsDatum;
						ironSourcePaymentTile.onClick.AddListener(OnIronSourceTileClick);
						ironSourceTitleImage.sprite = offerwallsDatum.paymentRef.GetOfferwallLogo();
						ironSourceDescText.text = offerwallsDatum.paymentRef.desc;
						ironSourceCostText.text = offerwallsDatum.paymentRef.cost;
					}
					if (offerwallsDatum.paymentRef.link == "offerwall_revu")
					{
						revUPaymentData = offerwallsDatum;
						revUPaymentTile.onClick.AddListener(OnRevUTileClick);
						revUTitleImage.sprite = offerwallsDatum.paymentRef.GetOfferwallLogo();
						revUDescText.text = offerwallsDatum.paymentRef.desc;
						revUCostText.text = offerwallsDatum.localizedPrice;
					}
				}
			}
		}
		else
		{
			DoDestroy();
		}
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnIronSourceTileClick()
	{
		ShowOfferwall(ironSourcePaymentData);
	}

	public void OnRevUTileClick()
	{
		ShowOfferwall(revUPaymentData);
	}

	private void ShowOfferwall(PaymentData paymentData)
	{
		CharacterDALC.instance.doExtraPlatformLink(AppInfo.userID, GameData.instance.PROJECT.character.name, AppInfo.platform, GameData.instance.PROJECT.character.id, GameData.instance.SAVE_STATE.adid);
		GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_offerwall"), Language.GetString("ui_offerwall_desc"), Language.GetString("ui_got_it"), delegate
		{
			onOfferwallDialog(paymentData.paymentRef.link);
		});
	}

	private void onOfferwallDialog(string link)
	{
		if (link != null)
		{
			AppInfo.ShowOfferwall(link);
		}
	}
}
