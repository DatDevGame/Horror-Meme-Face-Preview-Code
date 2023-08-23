using _SDK.UI;
using Assets._GAME.Scripts.GamePlay;
using Assets._SDK.Analytics;
using Assets._SDK.Game;
using Assets._SDK.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Assets._SDK.Ads;

public class PausePanel : AbstractPanel
{
    const string KEY_MUTE_SOUND = "SOUND_LISTIEN_GAME_PLAY";
    public Sprite SoundOnSprite;
    public Sprite SoundOffSprite;

    public Button SoundButton;
    public Button ResumeButton;
    public Button BackToLobbyButton;

    private bool isOnSound;
    private void Start()
    {
        SoundButton.onClick.AddListener(SwitchSound);
        ResumeButton.onClick.AddListener(Resume);
        BackToLobbyButton.onClick.AddListener(BackToLobby);
	}

    public void Init()
    {
        Debug.Log("Init Pause");
		isOnSound = GameManager.Instance.SoundManager.IsOnSound;
		SetUpSound(isOnSound, isOnSound ? SoundOnSprite : SoundOffSprite);
	}

    private void Resume()
    {
        GamePlayLoader.Instance.CurrentGamePlay.Fire(GamePlayTrigger.Play);
        this.gameObject.SetActive(false);
    }
    private void BackToLobby()
    {
        if (GamePlayLoader.Instance.CurrentGamePlay.IsMissionTutorial != true)
            AdsManager.Instance.ShowInterstitial(GamePlayLoader.Instance.PlayingMission.Order, AnalyticParamKey.MISSION_LEFT);

		GameManager.Instance.Fire(GameTrigger.End);
        GameManager.Instance.OnUnPause();
    }
    private void SwitchSound()
    {
        isOnSound = GameManager.Instance.SoundManager.IsOnSound;
		SetUpSound(isOnSound = !isOnSound, isOnSound? SoundOnSprite: SoundOffSprite);
	}
    private void SetUpSound(bool isActive, Sprite soundSprite)
    {
        GameManager.Instance.SoundManager.SetUpSound(isActive);
		SoundButton.image.sprite = soundSprite;
	}

    private void OnDestroy()
    {
		//AudioListener.volume = 1;
	}
}
