using Assets._SDK.Analytics;
using Assets._SDK.Logger;
using System;
using System.Collections;
using UnityEngine;
using static AnalyticsRevenueAds;

namespace RocketSg.Sdk.AdsClient
{
    public class MaxRewarded
    {
        private readonly string _rewardedAdUnitId;
        private readonly MaxAdsClient _adsClient;
        private int _retryAttempt;
        private Action<AdsResult> _onRewarded;
        private AdsResult _showResult;
		private bool _hasResumed = false;

		public bool HasResumed => _hasResumed;

		public MaxRewarded(string rewardedAdUnitId, MaxAdsClient adsClient)
        {
            _rewardedAdUnitId = rewardedAdUnitId;
            _adsClient = adsClient;
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
			MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
			//LoadRewardedAd();
		}

		public void LoadRewardedAd()
        {
            MaxSdk.LoadRewardedAd(_rewardedAdUnitId);
        }

        public void ShowRewardedAd(int levelIndex, string placementName, Action<AdsResult> onRewarded = null)
        {
            _onRewarded = onRewarded;
            _showResult = AdsResult.Skipped;
            if (MaxSdk.IsRewardedAdReady(_rewardedAdUnitId))
            {
                MaxSdk.ShowRewardedAd(_rewardedAdUnitId);
                AnalyticsService.LogEventRewardedVideoShow(levelIndex, placementName);
                _hasResumed = true;

			}
            else
            {
                _onRewarded?.Invoke((int)AdsResult.Failed);
            }
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
            Debug.Log("Rewarded ad loaded");

            // Reset retry attempt
            _retryAttempt = 0;
        }

        private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            _retryAttempt++;
            float retryDelay = (float)Math.Pow(2, Math.Min(4, _retryAttempt));

            Debug.Log("Rewarded ad failed to load with error code: " + errorInfo.Code);

            _adsClient.StartCoroutine(LoadRoutine(retryDelay));
        }

        private IEnumerator LoadRoutine(float retryDelay)
        {
            yield return new WaitForSeconds(retryDelay);
            LoadRewardedAd();
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad failed to display. We recommend loading the next ad
            Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
            _showResult = AdsResult.Failed;
            _onRewarded?.Invoke(_showResult);
            LoadRewardedAd();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Rewarded ad displayed");
            _showResult = AdsResult.Skipped;
            _hasResumed = false;

		}

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Rewarded ad clicked");
        }

        private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is hidden. Pre-load the next ad
            Debug.Log("Rewarded ad dismissed");
            _onRewarded?.Invoke(_showResult);
            LoadRewardedAd();
            _hasResumed = false;

		}

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad was displayed and user should receive the reward
            Debug.Log("Rewarded ad received reward");
            _showResult = AdsResult.Success;
        }

		private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
		{
			// Interstitial ad revenue paid. Use this callback to track user revenue.
			Debug.Log("rewarded revenue paid");

			// Ad revenue
			double revenue = adInfo.Revenue;

			// Miscellaneous data
			string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
			string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
			string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
			string placement = adInfo.Placement; // The placement this ad's postbacks are tied to

			var data = new ImpressionData();
			data.AdFormat = "rewarded";
			data.AdUnitIdentifier = adUnitIdentifier;
			data.CountryCode = countryCode;
			data.NetworkName = networkName;
			data.Placement = placement;
			data.Revenue = revenue;

			AnalyticsRevenueAds.SendEvent(data, adInfo.AdFormat);
		}

	}
}