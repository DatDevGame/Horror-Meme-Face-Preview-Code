using Assets._GAME.Scripts.Game;
using Assets._GAME.Scripts.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _GAME.Scripts.Inventory;
using UniRx;
using Assets._SDK.GamePlay;
using Assets._SDK.Game;
using System;

public class SoundManager : MonoBehaviour
{
	const string KEY_MUTE_SOUND = "SOUND_LISTIEN_GAME_PLAY";
	private Sound sound;

    private AudioSource _audioSource;
    private AudioClip audioClip;
    public AudioListener Listener { get; set; }

	//private bool isOnSound;
	public bool IsOnSound => IsOnSoundCheck();

	private void Start()
    {
        sound = GameManager.Instance.Resources.SoundSettings.Sound;
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();

        SoundBackGround(sound.SoundBackGroundLobby);
		//isOnSound = !PlayerPrefs.HasKey(KEY_MUTE_SOUND);
		//SetUpSound(isOnSound);

		GameManager.Instance.CurrentState.Where((value) => value == GameState.Lobby)
                .Subscribe(_ => SoundBackGround(sound.SoundBackGroundLobby));
        GameManager.Instance.CurrentState.Where((value) => value == GameState.Playing)
                .Subscribe(_ => SoundBackGround(sound.SoundBackGroundInGame));
    }

    bool IsOnSoundCheck()
    {
        return !PlayerPrefs.HasKey(KEY_MUTE_SOUND);
	}

	public void SetUpSound(bool isActive)
	{
		if (isActive == false) PlayerPrefs.SetInt(KEY_MUTE_SOUND, 1);
		else PlayerPrefs.DeleteKey(KEY_MUTE_SOUND);

		AudioListener.volume = isActive ? 1 : 0;
		//isOnSound = !PlayerPrefs.HasKey(KEY_MUTE_SOUND);
	}

	public void PauseSound(bool isActive)
	{
		AudioListener.volume = isActive ? 1 : 0;
	}

	public void SoundBackGround(AudioClip soundBackGround)
    {
        _audioSource.clip = soundBackGround;
        _audioSource.Play();
        _audioSource.loop = true;
    }
    public void SoundSystem(bool isActive)
    {
        Listener.enabled = isActive;
    }


    private List<AudioClip> HandleListSound(string soundName)
    {
        switch (soundName)
        {
            case "Chasing":
                return sound?.ListChasing;
            case "JumpScare":
                return sound?.ListJumpScare;
            default:
                return null;
        }
    }

    public AudioClip GetSoundWin()
    {
        return sound.Win;
    }
    public AudioClip GetSoundLose()
    {
        return sound.Lose;
    }
    public AudioClip GetSoundHealItem()
    {
        return sound.HealItem;
    }
    public AudioClip GetSoundStaminaItem()
    {
        return sound.StaminaItem;
    }
    public AudioClip GetSoundTreasureItem()
    {
        return sound.TreasureItem;
    }
    public AudioClip GetSoundBreakTime()
    {
        return sound.BreakTimeInGame;
    }


    public AudioClip GetSounds(string soundName, string ObjectName)
    {
        List<AudioClip> listSounds = new List<AudioClip>(HandleListSound(soundName));
        audioClip = listSounds[0];

		foreach (AudioClip soundChild in listSounds)
        {
            if (soundChild == null) continue;
            if (soundChild.name.Contains(ObjectName))
                audioClip = soundChild;
        }
        return audioClip;
    }
    public AudioClip GetSoundRunner(RunnerSound runnerSound)
    {
        switch (runnerSound)
        {
            case RunnerSound.Walk:
                return sound?.RunnerWalk;
            case RunnerSound.Run:
                return sound?.RunnerRun;
            case RunnerSound.Attack:
                return sound?.RunnerAttack;
			case RunnerSound.Tired:
				return sound?.RunnerTired;
			case RunnerSound.LowHP:
				return sound?.RunnerLowHP;

			default:
                return null;
        }
    }
    public AudioClip GetSoundEnemy(EnemySound enemySound)
    {
        switch (enemySound)
        {
            case EnemySound.Attack:
                return sound?.EnemyAttack;
            case EnemySound.ReceiveDame:
                return sound?.EnemyReceiveDame; 
            default:
                return null;
        }
    }

    public enum RunnerSound
    {
        Walk,
        Run,
        Attack,
		Tired,
        LowHP
	}
    public enum EnemySound
    {
        Attack,
        ReceiveDame,
    }
}
