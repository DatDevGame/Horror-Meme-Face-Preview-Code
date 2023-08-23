using System;
using Assets._SDK.Logger;

namespace Assets._SDK.Ads
{
    public class MockAdsManager : IAdsManager
    {
        public bool MustShowInterBackup { get; set; }
        public bool HasResumedFromAds => false;
		public void Init()
        {
			MustShowInterBackup = AdsConfig.InterBackupDefaultValue;
			DebugPro.Lime("INITIALIZED ADS MEDIATION");
        }

        public void ShowRewarded(Action<AdsResult> OnCalled, int levelIndex = 0, string placement = "")
        {
            DebugPro.Lime($"REWARDED {levelIndex}-{placement} SHOWN");
            OnCalled(AdsResult.Success);
        }

        public void ShowInterstitial(int levelIndex = 0, string placement = "")
        {
            DebugPro.Lime($"INTERSTITIAL {levelIndex}-{placement} SHOWN");
        }

        public void ShowAOA(int levelIndex = 0, string placement = "")
        {
			DebugPro.Lime($"APPOPENADS {levelIndex}-{placement} SHOWN");
		}
    }
}