using Assets._GAME.Scripts.Skills;
using System.Collections;
using UnityEngine;
using UniRx;
using Assets._GAME.Scripts.GamePlay;
using _SDK.UI;
using System;

public class JumpScarePanel : AbstractPanel
{
    public LosePanel _losePanel;

    private AudioSource _audioSource;
    private AudioClip JumpScareSound;
    [SerializeField]
    private GameObject _jumpScareImg;

    GameObject modelJumpScare;
    float timeAnimationLength;
    string nameEnemy;

	public void Init()
    {
        AddSound();
        AdjustEnemyJumpScare();
		foreach (Transform child in this.transform)
		{
			child.gameObject.SetActive(false);
		}
		_jumpScareImg.gameObject.SetActive(false);
	}
    public void Reload()
    {
        Init();
    }

    private void AddSound()
    {
        if (GetComponent<AudioSource>() == null)
            this.gameObject.AddComponent<AudioSource>();

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();
    }
    private void PlaySoundJumpScare(string AudioName)
    {
        JumpScareSound = GameManager.Instance.SoundManager.GetSounds("JumpScare", AudioName);

        if (JumpScareSound == null) return;
        _audioSource.PlayOneShot(JumpScareSound);
    }
    private void AdjustEnemyJumpScare()
    {
        RunnerSkillSystem runnerSkillSystem = GamePlayLoader.Instance?.Runner.GetComponent<RunnerSkillSystem>();
        if (runnerSkillSystem == null) return;

        var listSkin = GameManager.Instance.Resources.AllEnemySettings;

        // Init Data for Jumscare 
        runnerSkillSystem.RunnerObservable.JumpScare.Subscribe((EnemyHitName) =>
        {
			_jumpScareImg.gameObject.SetActive(true);
			//_losePanel.gameObject.SetActive(true);
            nameEnemy = EnemyHitName.Replace("(Clone)", "");

			foreach (var child in listSkin)
            {
                if (child.Value.name.Contains(nameEnemy))
                {
                    if (child.Value.skin.JumpScareSkin.ModelJumpScare == null) return;

					modelJumpScare = Instantiate(child.Value.skin.JumpScareSkin.ModelJumpScare, Vector3.zero, Quaternion.Euler(0, 180, 0), transform);
                    modelJumpScare.transform.localPosition = Vector3.zero;
                    modelJumpScare.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("JumpScare");
                    var Animator = modelJumpScare.AddComponent<Animator>();
					modelJumpScare.SetActive(false);
					//Destroy(modelJumpScare, 5f);
					Animator.runtimeAnimatorController = child.Value.skin.JumpScareSkin.JumpScareAnimation;
					timeAnimationLength = Animator.runtimeAnimatorController.animationClips[0].length;
					break;
				}
			}
            //StartCoroutine(EndGame(timeAnimationRun));

        }).AddTo(this);
    }

	public void ShowJumpScare(Action completeJumpScare)
	{
        float timedelay = timeAnimationLength + 0.2f;

		if (modelJumpScare != null)
        {
			modelJumpScare?.SetActive(true);
			PlaySoundJumpScare(nameEnemy);
			Destroy(modelJumpScare, timeAnimationLength);
		}

		StartCoroutine(WaitforFinishJumpScare(timeAnimationLength, completeJumpScare));
    }

	IEnumerator WaitforFinishJumpScare(float timeDelay ,Action completeJumpScare)
	{
		yield return new WaitForSeconds(timeDelay);
		_losePanel?.gameObject.SetActive(true);
		completeJumpScare.Invoke();
	}
}
