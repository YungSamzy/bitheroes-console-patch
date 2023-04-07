using UnityEngine;

namespace com.ultrabit.bitheroes.ui.payment.custom;

[DisallowMultipleComponent]
public abstract class PaymentCustomWindowHandler : MonoBehaviour
{
	protected PaymentCustomWindow _customWindow;

	public void SetPaymentCustomWindow(PaymentCustomWindow customWindow)
	{
		_customWindow = customWindow;
	}

	public abstract void doPayment();

	protected string GetPaymentID()
	{
		if (_customWindow != null && _customWindow.paymentRef != null)
		{
			return _customWindow.paymentRef.paymentID;
		}
		return "NOT_AVAILABLE";
	}
}
