using Assets._SDK.Input;
using Assets._SDK.Skills.Attributes;
using Assets._GAME.Scripts.Skills.Live;
using UniRx;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Assets._GAME.Scripts.GamePlay;
using Assets._GAME.Scripts.Enemies.Skills;
using System.Collections;
using Assets._GAME.Scripts.Skills.Move;

namespace Assets._GAME.Scripts.Skills
{
	public class MaxstaminaBehavior : MonoBehaviour
	{
		//public Animator AnimatorControl;

		private const string RUNNER_TAG = "Runner";

		private AudioSource _audioSource;
		private AudioClip _attackSound;

		void Awake()
		{
		}
		private void Start()
		{
			_attackSound = GameManager.Instance.Resources.SoundSettings.Sound?.RunnerAttack;
			_audioSource = GetComponent<AudioSource>();
		}


		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == RUNNER_TAG)
			{
				var runnerSkillSystem = other.transform.GetComponent<RunnerSkillSystem>();
				if (runnerSkillSystem == null) return;

				//runnerSkillSystem.GamePlayingMainPanel.UpdateMaxStamina(true);
				runnerSkillSystem.RunnerObservable.MaxStamina.Invoke(true);

				_audioSource?.PlayOneShot(_attackSound);
				this.GetComponent<BoxCollider>().enabled = false;
				this.gameObject.SetActive(false);
				Destroy(this.transform.gameObject,1f);
			}
		}
	}
}