using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.character;

public class CharacterPaymentData
{
	private int _id;

	private DateTime _createDate;

	private string _userID;

	private int _platform;

	private int _credits;

	private int _dollars;

	private string _paymentID;

	private string _orderID;

	public int id => _id;

	public DateTime createDate => _createDate;

	public string userID => _userID;

	public int platform => _platform;

	public int credits => _credits;

	public int dollars => _dollars;

	public string paymentID => _paymentID;

	public string orderID => _orderID;

	public CharacterPaymentData(int id, DateTime createDate, string userID, int platform, int credits, int dollars, string paymentID, string orderID)
	{
		_id = id;
		_createDate = createDate;
		_userID = userID;
		_platform = platform;
		_credits = credits;
		_dollars = dollars;
		_paymentID = paymentID;
		_orderID = orderID;
	}

	public static CharacterPaymentData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("pay0");
		DateTime dateFromString = Util.GetDateFromString(sfsob.GetUtfString("pay1"));
		string utfString = sfsob.GetUtfString("pay2");
		int int2 = sfsob.GetInt("pay3");
		int int3 = sfsob.GetInt("pay4");
		int int4 = sfsob.GetInt("pay5");
		string utfString2 = sfsob.GetUtfString("pay6");
		string utfString3 = sfsob.GetUtfString("pay7");
		return new CharacterPaymentData(@int, dateFromString, utfString, int2, int3, int4, utfString2, utfString3);
	}

	public static List<CharacterPaymentData> listFromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("pay8");
		List<CharacterPaymentData> list = new List<CharacterPaymentData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(fromSFSObject(sFSObject));
		}
		return list;
	}
}
