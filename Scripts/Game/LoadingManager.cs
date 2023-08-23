#define MAX_ADS
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if GOOGLE_AOA_ADS
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
#endif
using Assets._SDK.Ads;
using UnityEngine.SceneManagement;

public class LoadingManager : GameSingleton<LoadingManager>
{
	[SerializeField] private Image imgLoading;
	[SerializeField] private float timeLoading = 5;
	public PopupGDPR popupGDPR;
#if GOOGLE_AOA_ADS
	private AppOpenAdManager appOpenAdManager;
#endif
	private string firstScene = "Lobby";

	public Slider loadingBar;

	private void Awake()
	{
#if GOOGLE_AOA_ADS
		appOpenAdManager = AppOpenAdManager.Instance;
#endif
		_ = FirebaseService.Instance;
		_ = AdsManager.Instance;
	}

	private void SetProgressUI(float progress) // progress is from 0 to 1
	{
		loadingBar.value = progress;
	}

	private void Start()
	{
		//DontDestroyOnLoad(gameObject);
		if (PlayerPrefs.GetInt("showGDPR", 0) == 1)
		{
			Init();
		}
		else
		{
			PlayerPrefs.SetInt("showGDPR", 1);
			popupGDPR.gameObject.SetActive(true);
			popupGDPR.SetUp();
		}
	}
	public void Init()
	{
		Debug.Log("LOADING-----");
		//RunLoadingBar();
		LoadAppOpen();
		AdsManager.Instance.Init();
		StartCoroutine(Load());
	}
	private void LoadAppOpen()
	{
#if GOOGLE_AOA_ADS
		MobileAds.Initialize(initStatus => { appOpenAdManager.LoadAd(); AppStateEventNotifier.AppStateChanged += OnAppStateChanged; });
#endif
	}
	//private void RunLoadingBar()
	//{
	//    imgLoading.DOFillAmount(1, timeLoading)
	//        .SetEase(Ease.Linear)
	//        .OnComplete(() => {
	//            appOpenAdManager.ShowAdIfAvailable();
	//            SceneManager.LoadScene(firstScene); 
	//        });
	//}

	IEnumerator Load()
	{
		// Note: Doi 0.5sec truoc khi load thi se nhanh hon. 
		yield return new WaitForSeconds(0.5f);

		loadingBar.value = 0;
		float progress = 0;
		var loadGameScene = SceneManager.LoadSceneAsync(firstScene);

		loadGameScene.allowSceneActivation = false;

		// TODO: SplashScreen se show qua timelimit
		//if (AdsConfig.EnabledTypeAds.HasFlag(TypeAds.AppOpen))
		//{
		float _timeToRun = AdsConfig.CONST_TIME_WAIT_FOR_SHOW_FIRST_AOA;
		while ((_timeToRun -= Time.deltaTime) >= 0)
		{
			progress = Mathf.Clamp01((1 - _timeToRun / AdsConfig.CONST_TIME_WAIT_FOR_SHOW_FIRST_AOA) * .7f);

			SetProgressUI(progress);
			yield return null;
		}

		AdsManager.Instance.ShowAOA();
		//appOpenAdManager.ShowAdIfAvailable();
		//}


		while (!loadGameScene.isDone)
		{
			progress = Mathf.MoveTowards(progress, loadGameScene.progress, Time.deltaTime);

			//if ( !appOpenAdManager.IsShowed )
			//   appOpenAdManager.ShowAdIfAvailable();

			if (progress >= 0.9f)
			{
				progress = 1f;
				loadGameScene.allowSceneActivation = true;
			}

			SetProgressUI(progress);

			yield return null;
		}

		//AdManager.Instance.SetRemoteConfigValue();
	}
#if GOOGLE_AOA_ADS
	private void OnAppStateChanged(AppState state)
	{
		// Display the app open ad when the app is foregrounded.
		Debug.Log("App State is " + state);
		if (state == AppState.Foreground)
		{
			appOpenAdManager.ShowAdIfAvailable();
		}
	}
#endif
}
