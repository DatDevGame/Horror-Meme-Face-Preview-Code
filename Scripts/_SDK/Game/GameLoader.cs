using Assets._SDK.Ads;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets._SDK.Analytics;

[DefaultExecutionOrder(-1)]
public class GameLoader : MonoBehaviour
{
    public TMP_Text loadingProcessText;
    public Slider loadingBar;
	public string firstScene = "Lobby";

    private void Awake()
	{
		_ = FirebaseService.Instance;
		_ = AdsManager.Instance;
    }
    void Start()
    {
        StartCoroutine(Load());
    }

	private void SetProgressUI(float progress) // progress is from 0 to 1
	{
        loadingProcessText.SetText(string.Format("{0:0}", (progress * 100)) + "%");
        loadingBar.value = progress;
    }
    IEnumerator Load()
    {
        // Note: Doi 0.5sec truoc khi load thi se nhanh hon. 
        yield return new WaitForSeconds(0.5f);

		loadingBar.value = 0;
		float progress = 0;
		var loadGameScene = SceneManager.LoadSceneAsync(firstScene);

		loadGameScene.allowSceneActivation = false;

		// TODO: SplashScreen se show qua timelimit
		//if (AdsConfig.TypeAdsUse.HasFlag(TypeAdsMax.AOA))
		//{
		//	float _timeToRun = AdsConfig.CONST_TIME_WAIT_FOR_SHOW_FIRST_AOA;
		//	while ((_timeToRun -= Time.deltaTime) >= 0)
		//	{
		//		progress = Mathf.Clamp01((1 - _timeToRun / AdsConfig.CONST_TIME_WAIT_FOR_SHOW_FIRST_AOA) * .7f);
				
		//		SetProgressUI(progress);
		//		yield return null;
		//	}

		//	//AdsManager.Instance.AdsClient.ShowAOA();
		//}


		while (!loadGameScene.isDone)
		{
			progress = Mathf.MoveTowards(progress, loadGameScene.progress, Time.deltaTime);
			SetProgressUI(progress);

			if (progress >= 0.9f)
			{
				//if (AdsConfig.TypeAdsUse.HasFlag(TypeAdsMax.AOA) 
				//	&& AdsManager.Instance.AdsClient.IsShowAOAFirst)
				//{
				//	AdsManager.Instance.AdsClient.ShowAOA();
				//}

                SetProgressUI(1f);
                loadGameScene.allowSceneActivation = true;
			}
			yield return null;
		}
    }
}
