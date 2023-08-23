using Assets._SDK.Analytics;
using System;
using System.Collections;
using UnityEngine;
using static AnalyticsRevenueAds;

namespace RocketSg.Sdk.AdsClient
{
    public class MaxInterstitial
    {
        private int _retryAttempt;
        private readonly string _interstitialAdUnitId;
        private readonly MaxAdsClient _adsClient;
        private bool _isLoading;
		private bool _hasResumed = false;

		public bool HasResumed => _hasResumed;

		public MaxInterstitial(string interstitialAdUnitId, MaxAdsClient adsClient)
        {
            _interstitialAdUnitId = interstitialAdUnitId;
            _adsClient = adsClient;
            // Attach callbacks
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;
			// Load the first interstitial
			//LoadInterstitial();
		}

        public bool LoadInterstitial()
        {
            if (!_isLoading)
            {
                MaxSdk.LoadInterstitial(_interstitialAdUnitId);
                _isLoading = true;
                return true;
            }

            return false;
        }

        public void ShowInterstitial(int levelIndex, string placementName)
        {
            if (MaxSdk.IsInterstitialReady(_interstitialAdUnitId))
            {
                MaxSdk.ShowInterstitial(_interstitialAdUnitId);
                AnalyticsService.LogEventInterstitialShow(levelIndex, placementName);
                _hasResumed = true;
			}
        }

        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
            Debug.Log("Interstitial loaded");

            // Reset retry attempt
            _retryAttempt = 0;
        }

        private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 16 seconds).
            _retryAttempt++;
            float retryDelay = (float) Math.Pow(2, Math.Min(4, _retryAttempt));

            Debug.Log("Interstitial failed to load with error code: " + errorInfo.Code);

            _adsClient.StartCoroutine(LoadRoutine(retryDelay));
        }

        private IEnumerator LoadRoutine(float retryDelay)
        {
            yield return new WaitForSeconds(retryDelay);
            _isLoading = false;
            LoadInterstitial();
        }

        private void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad failed to display. We recommend loading the next ad
            Debug.Log("Interstitial failed to display with error code: " + errorInfo.Code);
            _isLoading = false;
            LoadInterstitial();
            _hasResumed = false;

		}

        private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is hidden. Pre-load the next ad
            Debug.Log("Interstitial dismissed");
            _isLoading = false;
            LoadInterstitial();
            _hasResumed = false;
		}


		private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
		{
			// Interstitial ad revenue paid. Use this callback to track user revenue.
			Debug.Log("Interstitial revenue paid");

			// Ad revenue
			double revenue = adInfo.Revenue;

			// Miscellaneous data
			string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
			string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
			string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
			string placement = adInfo.Placement; // The placement this ad's postbacks are tied to

			var data = new ImpressionData();
			data.AdFormat = "interstitial";
			data.AdUnitIdentifier = adUnitIdentifier;
			data.CountryCode = countryCode;
			data.NetworkName = networkName;
			data.Placement = placement;
			data.Revenue = revenue;

			AnalyticsRevenueAds.SendEvent(data, adInfo.AdFormat);
		}

	}
}