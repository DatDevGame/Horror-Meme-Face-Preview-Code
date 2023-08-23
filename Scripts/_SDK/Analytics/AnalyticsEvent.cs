using System.Collections;
using UnityEngine;

namespace Assets._SDK.Analytics
{
    public class AnalyticsEvent
    {
        public static string LEVEL_START = "level_start";
        public static string LEVEL_WIN = "level_win";
        public static string LEVEL_LOSE = "level_lose";
        public static string REWARDED_VIDEO_SHOW = "rewarded_video_show";
        public static string INTERSTITIAL_SHOW = "interstitial_show";
		public static string APPOPENADS_SHOW = "app_open_ads_show";
	}
    public static class UserProperties
    {
        public static string LEVEL_REACH = "level_reach";
        public static string DAYS_PLAYING = "days_playing";
        public static string TOTAL_SPENT = "total_spent";
        public static string TOTAL_EARN = "total_earn";
    }
    public static class AnalyticParamKey
    {
        public static string TIME = "time";
        public static string LEVEL = "level";
        public static string PLACEMENT = "placement";


        #region REWARD
        public static string REWARD_MISSION = "reward_mission";
        public static string REWARD_DOUBLE_PRIZE = "reward_double_prize";
        public static string REWARD_HP = "reward_hp";
        public static string REWARD_STAMINA = "reward_stamina";
        public static string REWARD_WEAPON_INGAME = "reward_weapon_ingame";
		public static string REWARD_WEAPON_SHOP = "reward_weapon_shop";
		public static string SPECIAL_MISSION = "special_mission";
		public static string SPECIAL_NEXTBOT_MISSION = "gallery_mission";
		public static string SPECIAL_GRIMACE_MISSION = "special_grimace_mission";
		public static string REWARD_CHOOSE_GUN = "reward_choose_gun";
		#endregion

		#region INTERSTITIAL
		public static string MISSION_WIN = "mission_win";
        public static string MISSION_LOSE = "mission_lose";
        public static string MISSION_MAX_LENGTH = "mission_max_length";
		public static string MISSION_LEFT = "mission_left";
		#endregion
	}

}