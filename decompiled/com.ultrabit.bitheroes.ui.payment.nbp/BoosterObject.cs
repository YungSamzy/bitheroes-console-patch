using com.ultrabit.bitheroes.model.booster;
using com.ultrabit.bitheroes.model.payment;

namespace com.ultrabit.bitheroes.ui.payment.nbp;

public class BoosterObject
{
	private BoosterRef _boosterRef;

	private PaymentRef _paymentRef;

	public BoosterRef boosterRef => _boosterRef;

	public PaymentRef paymentRef => _paymentRef;

	public BoosterObject(BoosterRef boosterRef, PaymentRef paymentRef)
	{
		_boosterRef = boosterRef;
		_paymentRef = paymentRef;
	}
}
