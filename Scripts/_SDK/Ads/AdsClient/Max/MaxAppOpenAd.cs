using Assets._SDK.Analytics;
using System;
using UnityEngine;

namespace RocketSg.Sdk.AdsClient
{
	public class MaxAppOpenAd
	{
		private readonly MaxAdsClient _adsClient;
		private readonly string _AOAUnitId;
		private bool _hasAppOpenAdShown = false;
		private bool _hasResumed = false;

		public bool HasResumed => _hasResumed;

		public bool HasAppOpenAdShown => _hasAppOpenAdShown;
		public MaxAppOpenAd(string AOAUnitId, MaxAdsClient adsClient)
		{
			_AOAUnitId = AOAUnitId;
			_adsClient = adsClient;

			MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
			MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAOALoadedFailedEvent;
			MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnAdFailedToDisplayEvent;
			MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAOARevenuePaidEvent;
			MaxSdk.LoadAppOpenAd(_AOAUnitId);
		}

		public void ShowAdIfReady(int levelIndex = 0, string placement = "")
		{
			if (MaxSdk.IsAppOpenAdReady(_AOAUnitId))
			{
				MaxSdk.ShowAppOpenAd(_AOAUnitId);
				_hasResumed = true;
				_hasAppOpenAdShown = true;

				AnalyticsService.LogEventAppOpenAdsShow(levelIndex, placement);
			}
		}

		/// <summary>
		/// Load AOA First
		/// </summary>
		private void OnAOALoadedFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
		{
			MaxSdk.LoadAppOpenAd(_AOAUnitId);
		}

		/// <summary>
		/// Set RusumedAd = False after close Ads
		/// </summary>
		private void OnAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
					MaxSdkBase.AdInfo adInfo)
		{
			_hasResumed = false;
		}

		public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
		{
			MaxSdk.LoadAppOpenAd(_AOAUnitId);
			_hasResumed = false;
		}

		private void OnAOARevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
		{
			// Interstitial ad revenue paid. Use this callback to track user revenue.
			Debug.Log("appopenads revenue paid");

			// Ad revenue
			double revenue = adInfo.Revenue;

			// Miscellaneous data
			string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
			string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
			string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
			string placement = adInfo.Placement; // The placement this ad's postbacks are tied to

			var data = new ImpressionData();
			data.AdFormat = "appopenads";
			data.AdUnitIdentifier = adUnitIdentifier;
			data.CountryCode = countryCode;
			data.NetworkName = networkName;
			data.Placement = placement;
			data.Revenue = revenue;

			AnalyticsRevenueAds.SendEvent(data, adInfo.AdFormat);
		}
	}
}