using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.zone;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.payment;

public class PaymentBook
{
	private static List<PaymentRef> _payments;

	private static List<PaymentData> _payments_data;

	public static int size => _payments.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_payments = new List<PaymentRef>();
		_payments_data = new List<PaymentData>();
		foreach (PaymentBookData.Platform item in XMLBook.instance.paymentBook.lstPlatform)
		{
			string[] array = item.types.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				if (AppInfo.GetPlatformType(array[i]) != AppInfo.platform)
				{
					continue;
				}
				int num = 0;
				foreach (PaymentBookData.Payment item2 in item.lstPayment)
				{
					_payments.Add(new PaymentRef(num, item2));
					num++;
				}
			}
		}
		_payments_data = GetDefaultPaymentData();
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static PaymentRef GetFirstPaymentByType(int type)
	{
		foreach (PaymentRef payment in _payments)
		{
			if (payment != null && payment.type == type)
			{
				return payment;
			}
		}
		return null;
	}

	public static PaymentRef GetFirstPaymentByItem(ItemRef itemRef)
	{
		foreach (PaymentRef payment in _payments)
		{
			if (payment != null && payment.itemData != null && payment.type == 2 && payment.itemData.itemRef == itemRef)
			{
				return payment;
			}
		}
		return null;
	}

	public static PaymentRef GetFirstPaymentByItemForBooster(ItemRef itemRef)
	{
		foreach (PaymentRef payment in _payments)
		{
			if (payment != null && payment.itemData != null && payment.itemData.itemRef == itemRef)
			{
				return payment;
			}
		}
		return null;
	}

	public static PaymentData GetPaymentRefData(PaymentRef paymentRef)
	{
		if (paymentRef == null)
		{
			return null;
		}
		foreach (PaymentData payments_datum in _payments_data)
		{
			if (payments_datum != null && payments_datum.paymentRef.id == paymentRef.id)
			{
				return payments_datum;
			}
		}
		return null;
	}

	private static List<PaymentData> GetDefaultPaymentData(int type = -1)
	{
		List<PaymentData> list = new List<PaymentData>();
		foreach (PaymentRef payment in _payments)
		{
			if (payment != null && (type < 0 || payment.type == type))
			{
				PaymentData item = new PaymentData(payment, payment.cost);
				list.Add(item);
			}
		}
		return list;
	}

	public static PaymentRef Lookup(int index)
	{
		if (index < 0 || index >= _payments.Count)
		{
			return null;
		}
		return _payments[index];
	}

	public static List<PaymentData> GetPaymentData(int type = -1)
	{
		List<PaymentData> list = new List<PaymentData>();
		foreach (PaymentData payments_datum in _payments_data)
		{
			if (payments_datum != null && (payments_datum.paymentRef.type == type || type < 0))
			{
				list.Add(payments_datum);
			}
		}
		if (list.Count > 0)
		{
			return list;
		}
		return GetDefaultPaymentData(type);
	}

	public static List<PaymentRef> GetAllPaymentsByTypeAndZone(int type, int zone)
	{
		List<PaymentRef> list = new List<PaymentRef>();
		ZoneRef zoneRef = ZoneBook.Lookup(zone);
		List<string> list2 = ((zoneRef != null && zoneRef.paymentsZoneID != null) ? zoneRef.paymentsZoneID : new List<string>());
		if (GameData.instance.PROJECT.character.zoneCompleted >= 1)
		{
			list2.AddRange(VariableBook.paymentsIDNBPZone);
		}
		foreach (PaymentRef payment in _payments)
		{
			if (payment != null && payment.type == type && list2.Contains(payment.paymentID))
			{
				list.Add(payment);
			}
		}
		return list;
	}

	public static List<string> GetBoostersIdInZone()
	{
		List<string> list = new List<string>();
		ZoneRef zoneRef = ZoneBook.Lookup(GameData.instance.PROJECT.character.zoneCompleted);
		List<string> list2 = ((zoneRef != null && zoneRef.paymentsZoneID != null) ? zoneRef.paymentsZoneID : new List<string>());
		if (GameData.instance.PROJECT.character.zoneCompleted >= 2)
		{
			list2.AddRange(VariableBook.paymentsIDNBPZone);
		}
		foreach (PaymentRef payment in _payments)
		{
			if (payment != null && payment.type == 3 && list2.Contains(payment.paymentID))
			{
				list.Add(payment.paymentID);
			}
		}
		return list;
	}
}
