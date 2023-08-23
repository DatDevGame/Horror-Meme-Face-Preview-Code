using System;
using System.Collections;
using Assets._SDK.Ads;
using Assets._SDK.Analytics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace RocketSg.Sdk.AdsClient
{
	public class MaxAdsClient : GameSingleton<MaxAdsClient>, IAdsManager
	{
        private TypeAds _enabledTypeAds = AdsConfig.EnabledTypeAds;

        private string _maxSdkKey = AdsConfig.MaxSdkKey;
		private string _interstitialAdUnitId = AdsConfig.InterstitialAdUnitId;
		private string _rewardedAdUnitId = AdsConfig.RewardedAdUnitId;
		private string _bannerAdUnitId = AdsConfig.BannerAdUnitId;
		private string _aOAUnitId = AdsConfig.AppOpenAdUnitId;
		
		private MaxInterstitial _interstitial;
		private MaxRewarded _rewarded;
		private MaxBanner _banner;
		private MaxAppOpenAd _appOA;
		private CappingTimer _cappingTimer;

        private MaxSdkBase.BannerPosition _bannerPosition = MaxSdkBase.BannerPosition.BottomCenter;

        protected override void OnAwake()
		{
			MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
			{
				// AppLovin SDK is initialized, configure and start loading ads.
				Debug.Log("MAX SDK Initialized");

				bool isEnabledMaxAppOpen = _enabledTypeAds.HasFlag(TypeAds.AppOpen)
				&& !String.IsNullOrEmpty(_aOAUnitId);

                if (isEnabledMaxAppOpen)
				{
					_appOA = new MaxAppOpenAd(_aOAUnitId, this);
                    // Tao 1 Method Load Inter, Reward
                    MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += LoadAdsAfterAOA; 
				}

				if (_enabledTypeAds.HasFlag(TypeAds.Interstitial))
				{
					_interstitial = new MaxInterstitial(_interstitialAdUnitId, this);
				}

				if (_enabledTypeAds.HasFlag(TypeAds.RewardVideo))
				{
					_rewarded = new MaxRewarded(_rewardedAdUnitId, this);
				}

				if (_enabledTypeAds.HasFlag(TypeAds.Banner))
				{
					_banner = new MaxBanner(_bannerAdUnitId, _bannerPosition);
					ShowBanner(true);
				}

				if (!isEnabledMaxAppOpen)
				{
					LoadAdsAfterAOA("", null);
                }
			};

			MaxSdk.SetHasUserConsent(PlayerPrefs.GetInt("Max_Consent") == 1);
			MaxSdk.SetDoNotSell(PlayerPrefs.GetInt("Max_Consent") == 1);

			MaxSdk.SetSdkKey(_maxSdkKey);

			// Devices TEST CTY
			MaxSdk.SetTestDeviceAdvertisingIdentifiers(new string[] { "1ef54a65-15dd-4740-95fc-1b34a740c544", "d6590ef7-0c38-4537-b916-6212c4088b28" });

			MaxSdk.InitializeSdk();
		}

		public override void Init()
		{
			MustShowInterBackup = AdsConfig.InterBackupDefaultValue;
		}



		/// <summary>
		/// Set Capping Timer
		/// </summary>
		public void SetCappingTimer(CappingTimer cappingTimer)
		{
			_cappingTimer = cappingTimer;
		}

		#region Max

		public bool IsRewardedVideoReady => MaxSdk.IsRewardedAdReady(_rewardedAdUnitId);

		public bool IsInterstitialLoaded => MaxSdk.IsInterstitialReady(_interstitialAdUnitId);

		public bool IsAOALoaded => MaxSdk.IsAppOpenAdReady(_aOAUnitId);

		public bool HasAOAShown => _appOA.HasAppOpenAdShown;

		public bool MustShowInterBackup { get; set; }

		//public void OnApplicationPause(bool isPause)
		//{
		//	Debug.Log("<color=green>OnApplicationPause is :</color>" + isPause);

		//	if (isPause)
		//	{
		//		_cappingTimer?.Stop();
		//	}
		//	else
		//	{
		//		_cappingTimer?.Start();
		//	}
		//}

		//CheckResumedAds
		

		public bool HasResumedFromAds => (_appOA != null && _appOA.HasResumed)
				|| (_rewarded != null && _rewarded.HasResumed)  
				|| (_interstitial != null && _interstitial.HasResumed);


		/// <summary>
		/// Load Other Ads sau khi load AOA hoac ko co AOA
		/// </summary>
		private void LoadAdsAfterAOA(string adUnitId, MaxSdkBase.AdInfo adInfo)
		{
			_rewarded?.LoadRewardedAd();
			_interstitial?.LoadInterstitial();
		}


		/// <summary>
		/// Long ads (it is often 30sec)
		/// </summary>
		/// <param name="placementName"></param>
		/// <param name="onRewarded"></param>
		public void ShowRewarded(Action<AdsResult> OnCalled, int levelIndex = 0, string placement = "")
		{
			Debug.Log("<color=green>Show Video Reward</color>");
			_rewarded?.ShowRewardedAd(levelIndex, placement, (result) =>
			{
				AnalyticsService.LogEventRewardedVideoShow(levelIndex, placement);
				OnCalled.Invoke(result);
			});
		}


		/// <summary>
		/// Show short ads 
		/// </summary>
		/// <param name="placementName"></param>
		/// <param name="onRewarded"></param>
		public void ShowInterstitial(int levelIndex, string placementName)
		{

			if (
				//!MustShowInterBackup &&
				IsInterstitialLoaded)
			{
				Debug.Log("<color=green>Show Interstitial</color>");
				_interstitial?.ShowInterstitial(levelIndex, placementName);
				//_cappingTimer?.Reset();
			}
			else
			{
				Debug.Log("<color=green>Not able to show Interstitial</color>");
			}
		}

		/// <summary>
		/// Show banner
		/// </summary>
		public void ShowBanner(bool isShow)
		{
			StartCoroutine(WaitToShowBanner(isShow));

			//_banner.ToggleBannerVisibility(isShow);

		}
		IEnumerator WaitToShowBanner(bool isActive)
		{

			while (_banner == null)
			{
				yield return new WaitForSecondsRealtime(1f);
			}

			Debug.Log("<color=green>Show Banner</color>");
			_banner.ToggleBannerVisibility(isActive);
		}

		/// <summary>
		/// Show ShowAOA
		/// </summary>
		public void ShowAOA(int levelIndex = 0, string placement = "")
		{
			Debug.Log("<color=green>Show AOA</color>");
			_appOA?.ShowAdIfReady();
		}
		#endregion
	}
}