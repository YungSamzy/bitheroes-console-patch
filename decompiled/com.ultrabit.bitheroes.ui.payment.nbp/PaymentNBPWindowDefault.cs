using System.Collections.Generic;
using com.ultrabit.bitheroes.model.booster;
using com.ultrabit.bitheroes.model.payment;

namespace com.ultrabit.bitheroes.ui.payment.nbp;

public class PaymentNBPWindowDefault : PaymentNBPWindow
{
	public void LoadPlatformDetails(PaymentRef paymentRef, List<PaymentRef> paymentsNBPZ = null, int itemIndex = 0)
	{
		LoadDetails(paymentRef, paymentsNBPZ, itemIndex);
	}

	public void LoadPlatformDetails(List<BoosterRef> boosters, int itemIndex = 0)
	{
		LoadDetailsBoosters(boosters, itemIndex);
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
	}

	protected override void Init()
	{
		base.Init();
		SetCost(base.paymentRef.cost);
	}

	public override void DoDestroy()
	{
		base.DoDestroy();
	}
}
