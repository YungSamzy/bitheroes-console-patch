using System;
using System.Collections.Generic;
using System.Globalization;
using IronSourceJSON;
using UnityEngine;

public class IronSourceAdInfo
{
	public readonly string auctionId;

	public readonly string adUnit;

	public readonly string country;

	public readonly string ab;

	public readonly string segmentName;

	public readonly string adNetwork;

	public readonly string instanceName;

	public readonly string instanceId;

	public readonly double? revenue;

	public readonly string precision;

	public readonly double? lifetimeRevenue;

	public readonly string encryptedCPM;

	public IronSourceAdInfo(string json)
	{
		if (json == null || !(json != ""))
		{
			return;
		}
		try
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			Dictionary<string, object> obj = Json.Deserialize(json) as Dictionary<string, object>;
			if (obj.TryGetValue("auctionId", out var value) && value != null)
			{
				auctionId = value.ToString();
			}
			if (obj.TryGetValue("adUnit", out value) && value != null)
			{
				adUnit = value.ToString();
			}
			if (obj.TryGetValue("country", out value) && value != null)
			{
				country = value.ToString();
			}
			if (obj.TryGetValue("ab", out value) && value != null)
			{
				ab = value.ToString();
			}
			if (obj.TryGetValue("segmentName", out value) && value != null)
			{
				segmentName = value.ToString();
			}
			if (obj.TryGetValue("adNetwork", out value) && value != null)
			{
				adNetwork = value.ToString();
			}
			if (obj.TryGetValue("instanceName", out value) && value != null)
			{
				instanceName = value.ToString();
			}
			if (obj.TryGetValue("instanceId", out value) && value != null)
			{
				instanceId = value.ToString();
			}
			if (obj.TryGetValue("precision", out value) && value != null)
			{
				precision = value.ToString();
			}
			if (obj.TryGetValue("encryptedCPM", out value) && value != null)
			{
				encryptedCPM = value.ToString();
			}
			if (obj.TryGetValue("revenue", out value) && value != null && double.TryParse(string.Format(invariantCulture, "{0}", value), NumberStyles.Any, invariantCulture, out var result))
			{
				revenue = result;
			}
			if (obj.TryGetValue("lifetimeRevenue", out value) && value != null && double.TryParse(string.Format(invariantCulture, "{0}", value), NumberStyles.Any, invariantCulture, out result))
			{
				lifetimeRevenue = result;
			}
		}
		catch (Exception ex)
		{
			Debug.Log("error parsing ad info " + ex.ToString());
		}
	}

	public override string ToString()
	{
		string[] obj = new string[25]
		{
			"IronSourceAdInfo {auctionId='", auctionId, "', adUnit='", adUnit, "', country='", country, "', ab='", ab, "', segmentName='", segmentName,
			"', adNetwork='", adNetwork, "', instanceName='", instanceName, "', instanceId='", instanceId, "', revenue=", null, null, null,
			null, null, null, null, null
		};
		double? num = revenue;
		obj[17] = num.ToString();
		obj[18] = ", precision='";
		obj[19] = precision;
		obj[20] = "', lifetimeRevenue=";
		num = lifetimeRevenue;
		obj[21] = num.ToString();
		obj[22] = ", encryptedCPM='";
		obj[23] = encryptedCPM;
		obj[24] = "'}";
		return string.Concat(obj);
	}
}
