using UnityEngine;
using UnityEngine.UI;
using Assets._SDK.Game;
using Assets._GAME.Scripts.GamePlay;
using Assets._SDK.GamePlay;
using _SDK.UI;
using TMPro;
using System.Collections;

public class LosePanel : AbstractPanel
{
    public Button PlayAgainButton;
    public Button BackToLobbyButton;
    public GameObject EndPanel;

    [SerializeField] private TextMeshProUGUI _coinValue;

    private AudioSource _audioSource;
    private AudioClip _loseSound;
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

	private bool _isDetectingLoad;
	bool isDetectingLoad
	{
		get { return _isDetectingLoad; }
		set
		{
			_isDetectingLoad = value;
			//AdsButton.interactable = !_isDetectingCoin && !AdsButton.interactable;
			PlayAgainButton.interactable = !_isDetectingLoad;
			BackToLobbyButton.interactable = !_isDetectingLoad;
		}
	}

	private void OnEnable()
    {
        PlaySound();
        UpdateUI();
		if (_delayEnableButtons != null)
			StopCoroutine(_delayEnableButtons);

		_delayEnableButtons = DelayToEnableButton(TimePlayAnimation);
		StartCoroutine(_delayEnableButtons);
	}

	IEnumerator DelayToEnableButton(float timeDelay = 0)
	{
		isDetectingLoad = true;
		yield return new WaitForSeconds(timeDelay+1f);
		var sysUnloader = GamePlayLoader.Instance.CurrentGamePlay.SysUnloader;
		if (sysUnloader != null)
			yield return new WaitUntil(() => GamePlayLoader.Instance.CurrentGamePlay.SysUnloader.isDone);

		isDetectingLoad = false;
	}

	private void Awake()
    {
        GetSound();
		isDetectingLoad = false;
	}

    private void Start()
    {
        PlayAgainButton.onClick.AddListener(PlayAgain);
        BackToLobbyButton.onClick.AddListener(Lobby);
    }
    private void GetSound()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();

        _loseSound = GameManager.Instance.SoundManager?.GetSoundLose();
        _audioSource.clip = _loseSound;
    }
    private void PlaySound()
    {
        if (_loseSound == null) return;
        _audioSource.Play();
    }

    private void UpdateUI()
    {
        _coinValue.SetText(GameManager.Instance.Wallet.GetAccountBy(_SDK.Money.Currency.Coin).Balance.Value.ToString());
    }

    private void Lobby()
    {
        GameManager.Instance.Fire(GameTrigger.End);
    }
    private void PlayAgain()
    {
		GamePlayLoader.Instance.CurrentGamePlay.Fire(GamePlayTrigger.Retry);
	}
}
