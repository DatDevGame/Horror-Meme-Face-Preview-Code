#define MAX_ADS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using System;
using AppsFlyerSDK;

public class ImpressionData
{
	public string CountryCode;
	public string NetworkName;
	public string AdUnitIdentifier;
	public string Placement;
	public double Revenue;
	public string AdFormat;
}

public class AnalyticsRevenueAds
{
	//public static string AppsflyerID;
	//public static string FirebaseID;
#if MAX_ADS
	public static void SendEvent(ImpressionData data, string AdFormat)
	{
		SendEventFB(data);
		SendEventAF(data);
	}

	#region FB
	public static void SendEventFB(ImpressionData data)
	{
		Firebase.Analytics.Parameter[] AdParameters = {
			 new Firebase.Analytics.Parameter("ad_platform", "applovin"),
			 new Firebase.Analytics.Parameter("ad_source", data.NetworkName),
			 new Firebase.Analytics.Parameter("ad_unit_name", data.AdUnitIdentifier),
			 new Firebase.Analytics.Parameter("currency","USD"),
			 new Firebase.Analytics.Parameter("value",data.Revenue),
			 new Firebase.Analytics.Parameter("placement",data.Placement),
			 new Firebase.Analytics.Parameter("country_code",data.CountryCode),
			 new Firebase.Analytics.Parameter("ad_format",data.AdFormat),
		};
		Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression_leo", AdParameters);
	}
	#endregion

	#region AF
	internal static void SendEventAF(ImpressionData data)
	{
		Dictionary<string, string> dic = new Dictionary<string, string>();
		dic.Add(AFAdRevenueEvent.COUNTRY, data.CountryCode);
		dic.Add(AFAdRevenueEvent.AD_UNIT, data.AdUnitIdentifier);
		dic.Add(AFAdRevenueEvent.AD_TYPE, data.AdFormat);
		dic.Add(AFAdRevenueEvent.PLACEMENT, data.Placement);
		//dic.Add(AFAdRevenueEvent.ECPM_PAYLOAD, data.encryptedCPM);
		//dic.Add("custom", "foo");
		//dic.Add("custom_2", "bar");
		//dic.Add("af_quantity", "1");

		AppsFlyerAdRevenue.logAdRevenue(data.NetworkName, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax, data.Revenue, "USD", dic);
	}
	#endregion
#endif

#if IRONSOURCE_ADS
    public static void SendEventFB(IronSourceImpressionData data)
    {
        SendEventRealtime(data);
    }
    private static void SendEventRealtime(IronSourceImpressionData data)
    {
        //string revenue = data.revenue.Value.ToString("N12").TrimEnd('0');

        Firebase.Analytics.Parameter[] AdParameters = {
             new Firebase.Analytics.Parameter("ad_platform", "iron_source"),
             new Firebase.Analytics.Parameter("ad_source", data.adNetwork),
             new Firebase.Analytics.Parameter("ad_unit_name",data.adUnit),
             new Firebase.Analytics.Parameter("currency","USD"),
             new Firebase.Analytics.Parameter("value",data.revenue.Value),
             new Firebase.Analytics.Parameter("placement",data.placement),
             new Firebase.Analytics.Parameter("country_code",data.country),
             new Firebase.Analytics.Parameter("ad_format",data.instanceName),
        };

        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_Impression_ironsource", AdParameters);


    }
	#region AF
	internal static void SendEventAF(IronSourceImpressionData data)
	{
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add(AFAdRevenueEvent.COUNTRY, data.country);
        dic.Add(AFAdRevenueEvent.AD_UNIT, data.adUnit);
        dic.Add(AFAdRevenueEvent.AD_TYPE, data.instanceName);
        dic.Add(AFAdRevenueEvent.PLACEMENT, data.placement);
        dic.Add(AFAdRevenueEvent.ECPM_PAYLOAD, data.encryptedCPM);
        //dic.Add("custom", "foo");
        //dic.Add("custom_2", "bar");
        //dic.Add("af_quantity", "1");
        AppsFlyerAdRevenue.logAdRevenue(data.adNetwork, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeIronSource, data.revenue.Value, "USD", dic);

    }
	#endregion
#endif
}


