using System;
public class AdsConfig
{
	// AppOpenAdd
#if UNITY_ANDROID
	public const string ID_TIER_1 = "ca-app-pub-6336405384015455/8218328765";
	public const string ID_TIER_2 = "ca-app-pub-6336405384015455/5755816951";
	public const string ID_TIER_3 = "ca-app-pub-6336405384015455/6793566454";

#elif UNITY_IOS
        public const string ID_TIER_1 = "";
        public const string ID_TIER_2 = "";
        public const string ID_TIER_3 = "";
#else
        public const string ID_TIER_1 = "ca-app-pub-6336405384015455/4951987271";
        public const string ID_TIER_2 = "ca-app-pub-6336405384015455/6022931332";
        public const string ID_TIER_3 = "ca-app-pub-6336405384015455/4709849668";
#endif

	//AppFlyer
	//public const string AppFlyerDevKey = "Mza5CYwx7pzKhdhcFcTHdm";
	//public const string AppFlyerHost = "appsflyersdk.com";

	// public const string AppFlyerAppId = ""; // Only for iOS

	//Max
	public const string MaxSdkKey = "7PspscCcbGd6ohttmPcZTwGmZCihCW-Jwr7nSJN2a_9Mg0ERPs0tmGdKTK1gs__nr6XHQvK0vTNaTb1uR1mCIN";
	public const string InterstitialAdUnitId = "e968015ee6d60882";
	public const string RewardedAdUnitId = "995cbff75b996f00";
	public const string BannerAdUnitId = "19bcdd9e542f02c8";
	public const string AppOpenAdUnitId = "d3130dd26013ecf5";

	//public const string InterstitialAdUnitId = "d3ffc84f1fa3a03d";
	//public const string RewardedAdUnitId = "82926a5a0d827477";
	//public const string BannerAdUnitId = "efcca8e6f53d66d8";
	//public const string AppOpenAdUnitId = "";

	// Iron Source
	//public const string ironSourceAppKey = "19475de05";

    //public const string CappingTimeRemoteConfigKey = "config_hunt_and_seek_capping_time";
    //public const float CappingTimeDefaultValue = 25;
    public const string InterBackupRemoteConfigKey = "config_nextbot_chasing_show_inter_backup";
    public const bool InterBackupDefaultValue = false;
    // TODO: phai sua lại 5
    public const float CONST_TIME_WAIT_FOR_SHOW_FIRST_AOA = 5f;
    public const AdManagerEnum AdManagerType = AdManagerEnum.MaxAds;

	public static TypeAds EnabledTypeAds = TypeAds.AppOpen| TypeAds.Banner| TypeAds.RewardVideo| TypeAds.Interstitial;
}

public enum AdManagerEnum
{
    MockAds,
    IronSource,
	MaxAds,
}

[Flags]
public enum TypeAds
{
	None = 0,
	AppOpen = 1,
	Banner = 2,
	RewardVideo = 4,
	Interstitial = 8,
	Other = 16,
}