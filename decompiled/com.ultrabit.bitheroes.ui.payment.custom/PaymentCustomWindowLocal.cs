using com.ultrabit.bitheroes.model.utility;

namespace com.ultrabit.bitheroes.ui.payment.custom;

public class PaymentCustomWindowLocal : PaymentCustomWindowHandler
{
	private bool _purchasing;

	public override void doPayment()
	{
		if (!_purchasing)
		{
			_purchasing = true;
			D.Log("Insert into the database, the consumable associated to this payment id: " + _customWindow.paymentRef.paymentID + " to the purchase");
			Invoke("SimulatePayment", 2f);
		}
	}

	private void SimulatePayment()
	{
		_customWindow.DoInventoryCheck();
		_purchasing = false;
	}
}
