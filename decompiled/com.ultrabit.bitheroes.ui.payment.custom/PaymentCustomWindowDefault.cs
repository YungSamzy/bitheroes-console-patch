using com.ultrabit.bitheroes.model.payment;

namespace com.ultrabit.bitheroes.ui.payment.custom;

public class PaymentCustomWindowDefault : PaymentCustomWindow
{
	public void LoadPlatformDetails(PaymentRef paymentRef)
	{
		LoadDetails(paymentRef);
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
	}

	internal override void Init()
	{
		base.Init();
		SetCost(base.paymentRef.cost);
	}

	public override void DoDestroy()
	{
		base.DoDestroy();
	}
}
