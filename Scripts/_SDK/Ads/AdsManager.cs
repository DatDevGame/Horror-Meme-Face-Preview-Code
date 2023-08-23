#define MAX_ADS
using RocketSg.Sdk.AdsClient;
using System;
using System.Net.Sockets;
using UnityEngine;

namespace Assets._SDK.Ads
{
    public class AdsManager : GameSingleton<AdsManager>, IAdsManager
    {
        private IAdsManager _adsManager;
        private FirebaseService firebaseService;

		public bool HasResumedFromAds { get; }

		public bool MustShowInterBackup
		{
			get => _adsManager.MustShowInterBackup; 
            set => _adsManager.MustShowInterBackup = value;
		}

		public void Init()
        {
            if (AdsConfig.AdManagerType == AdManagerEnum.MockAds)
                _adsManager = new MockAdsManager();
            else
#if MAX_ADS
				_adsManager = MaxAdsClient.Instance;
#elif IRONSOURCE_ADS
				_adsManager = IronSourceAd.AdManager.Instance;
#endif
			_adsManager.Init();
        }

        public void ShowRewarded(Action<AdsResult> OnCalled, int levelIndex = 0, string placement = "")
        {
            _adsManager.ShowRewarded(OnCalled, levelIndex, placement);
        }

        public void ShowInterstitial(int levelIndex = 0, string placement = "")
        {
            _adsManager.ShowInterstitial(levelIndex, placement);
        }

        public void ShowAOA(int levelIndex = 0, string placement = "")
        {
			_adsManager.ShowAOA(levelIndex,placement);
		}

		public void SetShowInterBackupFromFirebase()
		{
			MustShowInterBackup = firebaseService.GetRemoteConfigByKey(AdsConfig.InterBackupRemoteConfigKey).BooleanValue;
		}

		public void OnApplicationPause(bool isPaused)
		{
			bool isUnPaused = !isPaused;

			if (isUnPaused && _adsManager != null && !_adsManager.HasResumedFromAds)
			{
				_adsManager.ShowAOA();
			}
		}
	}
}