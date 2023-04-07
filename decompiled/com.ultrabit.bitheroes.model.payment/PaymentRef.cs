using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.payment;

[DebuggerDisplay("{paymentName} (PaymentRef)")]
public class PaymentRef : BaseRef, IEquatable<PaymentRef>, IComparable<PaymentRef>
{
	public const int PAYMENT_TYPE_NONE = 0;

	public const int PAYMENT_TYPE_CREDITS = 1;

	public const int PAYMENT_TYPE_ITEM = 2;

	public const int PAYMENT_TYPE_NBP = 3;

	public const int PAYMENT_TYPE_PREREG = 4;

	public const int PAYMENT_TYPE_OFFERWALL = 5;

	public const string OFFERWALL_IRONSOURCE = "offerwall_ironsource";

	public const string OFFERWALL_REVU = "offerwall_revu";

	private static Dictionary<string, int> PAYMENT_TYPES = new Dictionary<string, int>
	{
		["none"] = 0,
		["credits"] = 1,
		["item"] = 2,
		["nbp"] = 3,
		["prereg"] = 4,
		["offerwall"] = 5
	};

	private string _typeName;

	private int _type;

	private string _paymentID;

	private string _cost;

	private int _credits;

	private ItemData _itemData;

	public string paymentName => ((itemData != null) ? itemData.itemRef.name : base.name) + " (" + _typeName + ")";

	public string getPaymentStatType
	{
		get
		{
			string text = base.id + ((base.statName != null) ? (":" + base.statName) : "");
			if (type == 3)
			{
				string testingGroup = GameData.instance.PROJECT.character.extraInfo.testingGroup;
				if (!testingGroup.Equals(null) && !testingGroup.Equals(""))
				{
					text = text + "_" + testingGroup;
				}
			}
			return text;
		}
	}

	public int type => _type;

	public string paymentID
	{
		get
		{
			if (AppInfo.live)
			{
				return _paymentID;
			}
			return AppInfo.platform switch
			{
				2 => "com.odaclick.bitheroes.ios.5usd", 
				1 => "com.odaclick.bitheroes.usd5", 
				_ => _paymentID, 
			};
		}
	}

	public string cost => _cost;

	public int credits => _credits;

	public ItemData itemData => _itemData;

	public PaymentRef(int id, PaymentBookData.Payment paymentData)
		: base(id)
	{
		_typeName = paymentData.type;
		_type = (string.IsNullOrEmpty(_typeName) ? 1 : GetPaymentType(_typeName));
		_paymentID = paymentData.paymentID;
		_cost = Language.GetString(paymentData.cost);
		_credits = paymentData.credits;
		if (paymentData.item != null)
		{
			_itemData = new ItemData(ItemBook.Lookup(paymentData.item.id, ItemRef.getItemType(paymentData.item.type)), (paymentData.item.qty == null) ? 1 : int.Parse(paymentData.item.qty));
		}
		LoadDetails(paymentData);
	}

	public static int GetPaymentType(string type)
	{
		return PAYMENT_TYPES[type.ToLowerInvariant()];
	}

	public Sprite GetOfferwallLogo()
	{
		if (type != 5)
		{
			return null;
		}
		return GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.OFFERWALL_LOGO, base.thumbnail);
	}

	public override Sprite GetSpriteIcon()
	{
		return GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.SERVICE_ICON, icon);
	}

	public bool Equals(PaymentRef other)
	{
		if (other == null)
		{
			return false;
		}
		return paymentID.Equals(other.paymentID);
	}

	public int CompareTo(PaymentRef other)
	{
		if (other == null)
		{
			return -1;
		}
		int num = paymentID.CompareTo(other.paymentID);
		if (num == 0)
		{
			return base.id.CompareTo(other.id);
		}
		return num;
	}
}
