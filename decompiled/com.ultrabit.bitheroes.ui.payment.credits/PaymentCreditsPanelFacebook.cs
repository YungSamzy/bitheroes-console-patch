using System;

namespace com.ultrabit.bitheroes.ui.payment.credits;

public class PaymentCreditsPanelFacebook : PaymentCreditsPanel
{
	public override void LoadDetails(bool refreshable = false)
	{
		base.LoadDetails(refreshable);
	}

	protected override void doPayment()
	{
		throw new NotImplementedException();
	}
}
