using UnityEngine;

namespace com.ultrabit.bitheroes.ui.payment.nbp;

public abstract class PaymentNBPWindowHandler : MonoBehaviour
{
	protected PaymentNBPWindow _nbpWindow;

	public void SetPaymentNBPWindow(PaymentNBPWindow nbpWindow)
	{
		_nbpWindow = nbpWindow;
	}

	public abstract void doPayment();

	protected string GetPaymentID()
	{
		if (_nbpWindow != null && _nbpWindow.paymentRef != null)
		{
			return _nbpWindow.paymentRef.paymentID;
		}
		return "NOT_AVAILABLE";
	}
}
