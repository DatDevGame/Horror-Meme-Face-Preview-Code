using System;

public enum AdsResult
{
    Success = 2,
	Skipped = 1,
	Failed = 0
}

public interface IAdsManager
{
    public void Init();
    public void ShowRewarded(Action<AdsResult> OnCalled, int levelIndex = 0, string placement = "");
    public void ShowInterstitial(int levelIndex = 0, string placement = "");
    public bool MustShowInterBackup { get; set; }
	public void ShowAOA(int levelIndex = 0, string placement = "");
    public bool HasResumedFromAds { get; }
}