using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.language;

namespace com.ultrabit.bitheroes.model.payment;

[DebuggerDisplay("{name} (PaymentData)")]
public class PaymentData : IEquatable<PaymentData>, IComparable<PaymentData>
{
	private PaymentRef _paymentRef;

	private string _price;

	private string name => paymentRef.paymentName;

	public PaymentRef paymentRef => _paymentRef;

	public string price => _price;

	public string localizedPrice => Language.GetString(_price);

	public PaymentData(PaymentRef paymentRef, string price)
	{
		_paymentRef = paymentRef;
		_price = price;
	}

	public bool Equals(PaymentData other)
	{
		if (other == null)
		{
			return false;
		}
		if (paymentRef.Equals(other.paymentRef))
		{
			return price.Equals(other.price);
		}
		return false;
	}

	public int CompareTo(PaymentData other)
	{
		if (other == null)
		{
			return -1;
		}
		int num = paymentRef.CompareTo(other.paymentRef);
		if (num == 0)
		{
			return paymentRef.CompareTo(other.paymentRef);
		}
		return num;
	}
}
