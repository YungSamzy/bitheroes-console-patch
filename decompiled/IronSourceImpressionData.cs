using System;
using System.Collections.Generic;
using System.Globalization;
using IronSourceJSON;
using UnityEngine;

public class IronSourceImpressionData
{
	public readonly string auctionId;

	public readonly string adUnit;

	public readonly string country;

	public readonly string ab;

	public readonly string segmentName;

	public readonly string placement;

	public readonly string adNetwork;

	public readonly string instanceName;

	public readonly string instanceId;

	public readonly double? revenue;

	public readonly string precision;

	public readonly double? lifetimeRevenue;

	public readonly string encryptedCPM;

	public readonly int? conversionValue;

	public readonly string allData;

	public IronSourceImpressionData(string json)
	{
		if (json == null)
		{
			return;
		}
		try
		{
			allData = json;
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
			if (obj.TryGetValue("placement", out value) && value != null)
			{
				placement = value.ToString();
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
			if (obj.TryGetValue("conversionValue", out value) && value != null && int.TryParse(string.Format(invariantCulture, "{0}", value), NumberStyles.Any, invariantCulture, out var result2))
			{
				conversionValue = result2;
			}
		}
		catch (Exception ex)
		{
			Debug.Log("error parsing impression " + ex.ToString());
		}
	}

	public override string ToString()
	{
		string[] obj = new string[29]
		{
			"IronSourceImpressionData{auctionId='", auctionId, "', adUnit='", adUnit, "', country='", country, "', ab='", ab, "', segmentName='", segmentName,
			"', placement='", placement, "', adNetwork='", adNetwork, "', instanceName='", instanceName, "', instanceId='", instanceId, "', revenue=", null,
			null, null, null, null, null, null, null, null, null
		};
		double? num = revenue;
		obj[19] = num.ToString();
		obj[20] = ", precision='";
		obj[21] = precision;
		obj[22] = "', lifetimeRevenue=";
		num = lifetimeRevenue;
		obj[23] = num.ToString();
		obj[24] = ", encryptedCPM='";
		obj[25] = encryptedCPM;
		obj[26] = "', conversionValue=";
		int? num2 = conversionValue;
		obj[27] = num2.ToString();
		obj[28] = "}";
		return string.Concat(obj);
	}
}
