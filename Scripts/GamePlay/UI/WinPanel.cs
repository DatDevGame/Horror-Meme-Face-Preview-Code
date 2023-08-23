using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets._SDK.Game;
using Assets._GAME.Scripts.GamePlay;
using Assets._SDK.GamePlay;
using _SDK.UI;
using _SDK.Money;
using _GAME.Scripts.Inventory;
using UniRx;
using System;
using Assets._SDK.Analytics;
using Assets._SDK.Ads;
using DG.Tweening;
using Unity.VisualScripting;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;
using static UnityEngine.GraphicsBuffer;

public class WinPanel : AbstractPanel
{
	#region Button
	public Button AdsButton;
    public Button PlayAgainButton;
    public Button NextMissionButton;
    public Button BackToLobbyButton;
	public Button ClaimAdsButton;

	[SerializeField] private Button _nextMissionWithCoin;
    [SerializeField] private Button _nextMissionWithAds;
	[SerializeField] private TMP_Text rouletteRewardText;
	[SerializeField] private RectTransform RouletteArrow;
	#endregion

	#region RouletteArrowData
	private const float x5AbsRange = 81f;
	private const float x3AbsRange = 235f;
	private const float x2AbsRange = 444f;

	private float minX = -400f;
	private float maxX = 400f;
	#endregion
	//private int _totalCoinReceive;
	private int currentRouletteReward;

	public TMP_Text CoinReceiveText;

    [SerializeField] private TextMeshProUGUI _coinValue;

    [SerializeField] private GameObject _notification;
    [SerializeField] private CoinCollectEffect CoinCollectEffect;

    [SerializeField] private RectTransform _coinReceiveIcon;
    [SerializeField] private RectTransform _coinReleasePos;
    [SerializeField] private RectTransform _coinCurrencyIcon;

    private AudioSource _audioSource;
    private AudioClip _winSound;
	IEnumerator _delayEnableButtons;
	float timePlayAnimation = 0f;
	public float TimePlayAnimation
	{
		get
		{
			if (timePlayAnimation <= 0)
				timePlayAnimation = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].weight;
			return timePlayAnimation;
		}
	}

	private bool _isDetectingCoin;
	bool isDetectingCoin
	{
		get { return _isDetectingCoin; }
		set
        {
			_isDetectingCoin = value;
            //AdsButton.interactable = !_isDetectingCoin && !AdsButton.interactable;
            _nextMissionWithAds.interactable = !_isDetectingCoin;
			_nextMissionWithCoin.interactable = !_isDetectingCoin;
			NextMissionButton.interactable = !_isDetectingCoin;
			PlayAgainButton.interactable = !_isDetectingCoin;
	        BackToLobbyButton.interactable = !_isDetectingCoin;
            ClaimAdsButton.interactable = !_isDetectingCoin;

		}
    }

	#region Parameter
	private bool _canBuyWithCoin;
        private Mission _currentMission;
        private Mission _nextMission;

        private int _totalCoinReceive;
    #endregion

    private void Awake()
    {
        GetSound();
		isDetectingCoin = false;
	}

    private void Start()
    {
        AdsButton.onClick.AddListener(AdsDouble);
        PlayAgainButton.onClick.AddListener(PlayAgain);
        NextMissionButton.onClick.AddListener(NextMission);
        _nextMissionWithCoin.onClick.AddListener(NextMissionWithCoin);
        _nextMissionWithAds.onClick.AddListener(NextMissionWithAds);
        BackToLobbyButton.onClick.AddListener(Lobby);
        ClaimAdsButton.onClick.AddListener(ClaimSpecialAds);

        GameManager.Instance.Wallet.GetAccountBy(Currency.Coin).Balance.Subscribe(value =>
        {
            _coinValue.SetText(value.ToString());
        });

		UpdateUI(_totalCoinReceive);
    }

    private void OnEnable()
    {
		CoinReceiveText.SetText("+ 0");
		SetWinNextMission();

        PlaySound();
        LoadData();

        UpdateUI(_totalCoinReceive);
        if (_delayEnableButtons != null)
            StopCoroutine(_delayEnableButtons);

        _delayEnableButtons = DelayToEnableButton(TimePlayAnimation);
		StartCoroutine(_delayEnableButtons);

		StartRoulette();
	}

    private void OnDisable()
	{
		RouletteArrow.DOKill();
		StopAllCoroutines();
	}

	private void OnDestroy()
	{
		RouletteArrow.DOKill();
	}

	IEnumerator DelayToEnableButton(float timeDelay = 0)
	{
		isDetectingCoin = true;
        AdsButton.interactable = false;
		yield return new WaitForSeconds(timeDelay);
		AniamtionCount(0, _totalCoinReceive);
		ReceiveCoin(GamePlayLoader.Instance.CurrentGamePlay.PrizeMission);
		AdsButton.interactable = true;
	}

    IEnumerator EnableButton()
    {
		var sysUnloader = GamePlayLoader.Instance.CurrentGamePlay.SysUnloader;
		if (sysUnloader != null)
			yield return new WaitUntil(() => GamePlayLoader.Instance.CurrentGamePlay.SysUnloader.isDone);

		isDetectingCoin = false;
	}

	private void GetSound()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();

        _winSound = GameManager.Instance.SoundManager?.GetSoundWin();
        _audioSource.clip = _winSound;
    }
    private void PlaySound()
    {
        if (_winSound == null) return;
        _audioSource.Play();
    }

    private void LoadData()
    {

		isDetectingCoin = false;
		AdsButton.interactable = true;

		_currentMission = GamePlayLoader.Instance.PlayingMission;
		_nextMission = GameManager.Instance.MissionInventory.GetNextMission();

        _totalCoinReceive = _currentMission.Prize;
    }

    private void UpdateUI(int amout)
    {
		//CoinReceiveText.SetText($"+ {amout}");

		_canBuyWithCoin = CanBuyWithCoin();

        _notification.SetActive(!_canBuyWithCoin);
        _coinValue.SetText(GameManager.Instance.Wallet.GetAccountBy(Currency.Coin).Balance.Value.ToString());

        NextMissionButton.gameObject.SetActive(_nextMission.IsOwned);
		ClaimAdsButton.gameObject.SetActive(_nextMission.IsOwned);
		_nextMissionWithAds.gameObject.SetActive(!_canBuyWithCoin && !_nextMission.IsOwned);
        _nextMissionWithCoin.gameObject.SetActive(_canBuyWithCoin && !_nextMission.IsOwned); 
    }

    private bool CanBuyWithCoin()
    {
        float nextMissionPrice = _nextMission.Price.Value;
        float playerCoin = GameManager.Instance.Wallet.GetAccountBy(Currency.Coin).Balance.Value;

        return playerCoin >= nextMissionPrice;
    }
    private void StartRoulette()
    {
		RouletteArrow.DOAnchorPosX(maxX, 2f).SetEase(Ease.InOutFlash) //.From(-302.6f)
			.From(new Vector2(minX, RouletteArrow.anchoredPosition.y))
			.SetUpdate(true).SetLoops(-1, LoopType.Yoyo)
			.OnUpdate((TweenCallback)(() =>
			{
				int cached = currentRouletteReward;
				currentRouletteReward = Mathf.Abs(RouletteArrow.anchoredPosition.x) switch
				{
					< x5AbsRange => _totalCoinReceive * 5,
					< x3AbsRange => _totalCoinReceive * 3,
					< x2AbsRange => _totalCoinReceive * 2,
					_ => currentRouletteReward
				};

				if (cached != currentRouletteReward)
					rouletteRewardText.SetText(currentRouletteReward.ToString());
			}));
	}

	private void ClaimSpecialAds()
	{
		if (isDetectingCoin == true) return;

		isDetectingCoin = true;
		RouletteArrow.DOKill();

		AdsManager.Instance.ShowRewarded(result =>
		{
			if (result == AdsResult.Success)
			{
                ReceiveCoin(currentRouletteReward);
                AniamtionCount(_totalCoinReceive, currentRouletteReward);

				StartCoroutine(DelayToNextMission(1.5f));
			}
		}, _currentMission.Order, AnalyticParamKey.REWARD_DOUBLE_PRIZE);

		StartCoroutine(DelayToEnableButtons(4.5f));
	}

	IEnumerator DelayToEnableButtons(float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);

		isDetectingCoin = false;
	}

	IEnumerator DelayToNextMission(float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);

		NextMission();
	}

	public void ReceiveCoin(int amout)
    {
        isDetectingCoin = true;
		StartCoroutine(CoinCollectEffect.StartCollectionEffect(_coinReceiveIcon, _coinCurrencyIcon, (Action)(() =>
        {
            //GameManager.Instance.Wallet.GetAccountBy(Currency.Coin).Deposit(amout);
			var coinAccount = GameManager.Instance.Wallet.GetAccountBy(Currency.Coin);

			coinAccount.Deposit(amout);
			int newCointAmount = (int)coinAccount.Balance.Value;
			_coinValue.SetText(newCointAmount.ToString());
            StartCoroutine(EnableButton());
			UpdateUI(amout);
        })));
    }

    void AniamtionCount(int currentValue, int TargetValue)
    {
		int v = currentValue;
		TweenerCore<int, int, NoOptions> t = DOTween.To(() => v, x => {
			v = x;
			CoinReceiveText.SetText($"+ {v}");
		}, TargetValue, 1f);
	}

    private void AdsDouble()
	{
		if (isDetectingCoin) return;
		AdsManager.Instance.ShowRewarded((Action<AdsResult>)(isSuccess =>
		{
            if (isSuccess == AdsResult.Success)
            {
				ReceiveCoin(GamePlayLoader.Instance.CurrentGamePlay.PrizeMission);
				_totalCoinReceive *= 2;
				AdsButton.interactable = false;
            }
		}), _currentMission.Order, AnalyticParamKey.REWARD_DOUBLE_PRIZE);
    }
    private void PlayAgain()
    {
        GamePlayLoader.Instance.CurrentGamePlay.Fire(GamePlayTrigger.Retry);
    }
    private void Lobby()
    {
        SetCurrentActiveMissionPlayed();
        GameManager.Instance.Fire(GameTrigger.End);
    }
    private void NextMission()
    {
        var missionOrder = GameManager.Instance.MissionInventory;
        missionOrder.SetActiveMission(missionOrder.GetIndexActiveMisionSetting());
        LoadNextMission();
    }

    private void ClosePopup() => gameObject.SetActive(false);

    private void LoadNextMission()
    {
		Mission mission = (Mission)GameManager.Instance.MissionInventory.PlayingMission;
		if (mission.PhotoTypeMission != PhotoTypeMission.None)
		{
			AbstractGameManager.Instance.Fire(GameTrigger.End);
			return;
		}

		GamePlayLoader.Instance.CurrentGamePlay.Fire(GamePlayTrigger.Retry);
    }    

    private void NextMissionWithCoin()
	{
        if (isDetectingCoin == true) return;
		int prizeMission = (int) _nextMission.Price.Value;
        isDetectingCoin = true;
		StartCoroutine(CoinCollectEffect.StartCollectionEffect(null, null, (Action)(() => 
        {
            var coinAccount = GameManager.Instance.Wallet.GetAccountBy(Currency.Coin);
		    
            coinAccount.Credit(prizeMission);
            int newCointAmount = (int)coinAccount.Balance.Value;
			_coinValue.SetText(newCointAmount.ToString());
			isDetectingCoin = false;
			ClosePopup();
			NextMission();
        })));
    }

    private void MoneyOnChangeAnimation(bool isIncreasing, int changeValue)
    {
        int moneyValue = Int32.Parse(_coinValue.text);

        int value = isIncreasing ? moneyValue + changeValue : moneyValue - changeValue;

        _coinValue.SetText(value.ToString());
    }

    private void NextMissionWithAds()
    {
        Debug.Log("Buy By Ads");
        NextMission();
	}

    private void SetWinNextMission()
    {
        var missionOrder = GameManager.Instance.MissionInventory;
        if (missionOrder.GetIndexActiveMisionSetting() < GameManager.Instance.Resources.AllMissionSettings.Count)
            GameManager.Instance.MissionInventory.SetWinMission(missionOrder.GetIndexActiveMisionSetting());
    }

    private void SetCurrentActiveMissionPlayed()
    {
        var missionOrder = GameManager.Instance.MissionInventory.GetIndexActiveMisionSetting();
        if (missionOrder < GameManager.Instance.Resources.AllMissionSettings.Count)
        {
            GameManager.Instance.MissionInventory.SetActiveMission(missionOrder);
        }
    }

}
