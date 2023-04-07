using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.lists.paymentlist;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.payment.credits;

public abstract class PaymentCreditsPanel : MonoBehaviour
{
	public GameObject paymentListView;

	public GameObject paymentListScroll;

	public PaymentList paymentList;

	public Button refreshBtn;

	private List<PaymentData> _payments;

	private bool _refreshable;

	protected PaymentData selectedPaymentData;

	public virtual void LoadDetails(bool refreshable = false)
	{
		_refreshable = refreshable;
		if (refreshBtn != null)
		{
			refreshBtn.gameObject.SetActive(_refreshable);
		}
		paymentList.InitList(OnPaymentTileClick);
		CreateTiles();
	}

	private void CreateTiles()
	{
		List<PaymentData> paymentData = PaymentBook.GetPaymentData(1);
		paymentList.ClearList();
		if (AppInfo.offerwallAvailable)
		{
			List<PaymentData> paymentData2 = PaymentBook.GetPaymentData(5);
			if (paymentData2 != null && paymentData2.Count > 0)
			{
				paymentList.Data.InsertOneAtEnd(new PaymentItem(paymentData2[0]));
			}
		}
		foreach (PaymentData item in paymentData)
		{
			paymentList.Data.InsertOneAtEnd(new PaymentItem(item));
		}
	}

	private void OnPaymentTileClick(PaymentData payment)
	{
		switch (payment.paymentRef.type)
		{
		case 5:
		{
			List<PaymentData> paymentData = PaymentBook.GetPaymentData(5);
			if (paymentData != null && paymentData.Count > 0)
			{
				if (paymentData.Count == 1)
				{
					AppInfo.ShowOfferwall(paymentData[0].paymentRef.link);
				}
				else
				{
					GameData.instance.windowGenerator.NewServiceOfferwallSelectWindow(paymentData);
				}
			}
			else
			{
				D.LogError("all", "OfferWall Error: No Offerwalls available");
			}
			break;
		}
		case 1:
			selectedPaymentData = payment;
			KongregateAnalytics.trackPaymentStart(selectedPaymentData.paymentRef.paymentID, KongregateAnalytics.getPaymentGameFields(selectedPaymentData.paymentRef.getPaymentStatType));
			doPayment();
			break;
		}
	}

	private void onOfferwallDialog(string link)
	{
		if (link != null)
		{
			AppInfo.ShowOfferwall(link);
		}
	}

	protected abstract void doPayment();

	protected void CleanUpProcess()
	{
		selectedPaymentData = null;
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
	}

	internal void CreateList(List<PaymentData> payments)
	{
		paymentList.ClearList();
		_payments = payments;
		if (AppInfo.offerwallAvailable)
		{
			List<PaymentData> paymentData = PaymentBook.GetPaymentData(5);
			if (paymentData != null && paymentData.Count > 0)
			{
				foreach (PaymentData item in paymentData)
				{
					if (!(item.paymentRef.link == "offerwall_revu") || ((VariableBook.revUEnabled || AppInfo.TESTING) && AppInfo.GetForRevUOfferwallURL() != null))
					{
						paymentList.Data.InsertOneAtEnd(new PaymentItem(item));
					}
				}
			}
		}
		foreach (PaymentData payment in payments)
		{
			paymentList.Data.InsertOneAtEnd(new PaymentItem(payment));
		}
	}

	public void ShowServicePanel()
	{
		paymentList.gameObject.SetActive(value: true);
		paymentListView.SetActive(value: true);
		paymentListScroll.SetActive(value: true);
		refreshBtn.gameObject.SetActive(_refreshable);
	}

	public void HideServicePanel()
	{
		paymentListView.SetActive(value: false);
		paymentListScroll.SetActive(value: false);
		refreshBtn.gameObject.SetActive(value: false);
		paymentList.gameObject.SetActive(value: false);
	}

	public void DoEnable()
	{
		refreshBtn.interactable = true;
	}

	public void DoDisable()
	{
		refreshBtn.interactable = false;
	}

	public void ShowSuccess(int gems)
	{
		GameData.instance.PROJECT.character.credits += gems;
		GameData.instance.windowGenerator.ShowCreditsPurchased(gems, "IAP");
	}

	protected string GetPaymentID()
	{
		if (selectedPaymentData != null && selectedPaymentData.paymentRef != null)
		{
			return selectedPaymentData.paymentRef.paymentID;
		}
		return "NOT_AVAILABLE";
	}
}
