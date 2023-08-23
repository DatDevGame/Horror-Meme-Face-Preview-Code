//using sirenix.odininspector;
//using system.collections;
//using unityeditor;
//using unityengine;
//using appsflyersdk;
//using firebase;
//using system.io;
//using nsubstitute;

//namespace assets._sdk.game
//{
//#if unity_editor
//	[editorwindowtitle(title = "game settings")]
//	public class gamesettingswindow : sirenix.odininspector.editor.odineditorwindow
//	{
//		// todo: hiện thị các settings khác liên quan đến ads như
//		// adsconfig, packagename, product name, ads network, firebase
//		[boxgroup("android settings")]
//		[infobox("cần kiểm tra kĩ các settings phải chính xác với tài liệu của marketing.", infomessagetype.warning)]
//		[readonly]
//		public string packagename = "";

//		[boxgroup("firebase settings")]
//		[readonly]
//		public string firebasesdkversion = "";

//		[boxgroup("max settings")]
//		[readonly]
//		public string maxsdkversion = "";
//		[readonly]
//		public string applovinkey = "";
//		[readonly]
//		public string mediatorgoogleadmobkey = "";
//		[readonly]
//		public string appopenaddid = "";
//		[readonly]
//		public string interstitialadunitid = "";
//		[readonly]
//		public string rewardedadunitid = "";
//		[readonly]
//		public string banneradunitid = "";

//		[boxgroup("appsflyer settings")]
//		[readonly]
//		public string appflyerversion = "";
//		//[readonly]
//		//public string appsflyerkey = "";
//		//[readonly]
//		//public string appsflyerhost = "";

//		[menuitem("game/settings")]
//		private static void openwindow()
//		{
//			getwindow<gamesettingswindow>().show();
//		}

//		protected override void onenable()
//		{
//			packagename = playersettings.getapplicationidentifier(buildtargetgroup.android);

//			//firebase setting
//			directoryinfo dir = new directoryinfo(application.datapath + "/firebase/m2repository/com/google/firebase/firebase-app-unity");
//			directoryinfo[] info = dir.getdirectories();
//			if (info.length > 0)
//				firebasesdkversion = info[0].name;

//			//max setting
//			maxsdkversion = maxsdk.version;
//			applovinkey = adsconfig.maxsdkkey;
//			mediatorgoogleadmobkey = applovinsettings.instance.admobandroidappid;
//			interstitialadunitid = adsconfig.interstitialadunitid;
//			rewardedadunitid = adsconfig.rewardedadunitid;
//			banneradunitid = adsconfig.banneradunitid;
//			appopenaddid = adsconfig.id_tier_1;
			
//			//appsflyer
//			appflyerversion = appsflyer.kappsflyerpluginversion;
//			//appsflyerkey = adsconfig.appflyerdevkey;
//			//appsflyerhost = adsconfig.appflyerhost;
//		}
//	}
//#endif

//}