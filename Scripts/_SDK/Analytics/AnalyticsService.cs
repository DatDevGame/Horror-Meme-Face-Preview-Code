using AppsFlyerSDK;
using Assets._SDK.Logger;
using Firebase.Analytics;
using System.Collections.Generic;

namespace Assets._SDK.Analytics
{
    public class AnalyticsService
    {

        public static void SetUserProperty(string name, string val)
        {
            if (FirebaseService.Instance.IsConnected.Value)
            {
                return;
            }

            FirebaseAnalytics.SetUserProperty(name, val);
        }

        public static void LogEventLevelStart(int levelIndex)
        {
            UserLogServer.LogServer.LogEvent(AnalyticsEvent.LEVEL_START,
                AnalyticParamKey.LEVEL, levelIndex);
        }

        public static void LogEventLevelWin(int levelIndex)
        {
            UserLogServer.LogServer.LogEvent(AnalyticsEvent.LEVEL_WIN,
                AnalyticParamKey.LEVEL, levelIndex);
        }

        public static void LogEventLevelLose(int levelIndex)
        {
            UserLogServer.LogServer.LogEvent(AnalyticsEvent.LEVEL_LOSE,
                AnalyticParamKey.LEVEL, levelIndex);
        }

        public static void LogEventRewardedVideoShow(int levelIndex, string placement)
        {
            AppsFlyer.sendEvent(AnalyticsEvent.REWARDED_VIDEO_SHOW, 
                new Dictionary<string, string>() {
                    { AnalyticParamKey.LEVEL, levelIndex.ToString() }, 
                    { AnalyticParamKey.PLACEMENT, placement } 
                });

            UserLogServer.LogServer.LogEvent(AnalyticsEvent.REWARDED_VIDEO_SHOW, 
                new LogParameter[] {
                    new LogParameter(AnalyticParamKey.LEVEL,levelIndex),
                    new LogParameter(AnalyticParamKey.PLACEMENT,placement)
                });
        }

        public static void LogEventInterstitialShow(int levelIndex, string placement)
        {
            AppsFlyer.sendEvent(AnalyticsEvent.INTERSTITIAL_SHOW, 
                new Dictionary<string, string>() { 
                { AnalyticParamKey.LEVEL, levelIndex.ToString() },
                { AnalyticParamKey.PLACEMENT,placement}
            });

            UserLogServer.LogServer.LogEvent(AnalyticsEvent.INTERSTITIAL_SHOW, 
                new LogParameter[] {
                        new LogParameter(AnalyticParamKey.LEVEL,levelIndex),
                        new LogParameter(AnalyticParamKey.PLACEMENT,placement)
                });
        }

		public static void LogEventAppOpenAdsShow(int levelIndex, string placement)
		{
			AppsFlyer.sendEvent(AnalyticsEvent.APPOPENADS_SHOW,
				new Dictionary<string, string>() {
				{ AnalyticParamKey.LEVEL, levelIndex.ToString() },
				{ AnalyticParamKey.PLACEMENT,placement}
			});

			UserLogServer.LogServer.LogEvent(AnalyticsEvent.APPOPENADS_SHOW,
				new LogParameter[] {
						new LogParameter(AnalyticParamKey.LEVEL,levelIndex),
						new LogParameter(AnalyticParamKey.PLACEMENT,placement)
				});
		}

		public static void SetPropertiesTotalSpentAndEarn(int totalSpent, int value)
        {
            if (value < 0)
            {
                int total = totalSpent - value;
                SetUserProperty(UserProperties.TOTAL_SPENT, total.ToString());
            }
            else
            {
                int total = totalSpent + value;
                SetUserProperty(UserProperties.TOTAL_EARN, total.ToString());
            }
        }

        public static void SetPropertiesDayPlaying(int param)
        {
            SetUserProperty(UserProperties.DAYS_PLAYING, param.ToString());
        }

        public static void SetPropertiesLevelReach(int param)
        {
            SetUserProperty(UserProperties.LEVEL_REACH, param.ToString());
        }
    }

}
