using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.Skills;
using Assets._SDK.Input;
using Assets._SDK.Skills;
using System;
using System.Collections;
using UnityEngine;
using Assets._GAME.Scripts.GamePlay;
using UniRx;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Assets._GAME.GameItems.DoorExit;

public class DoorExitBehavior : MonoBehaviour
{
	[SerializeField]
	private GameObject EffectHit;

	private AudioSource _audioSource;
	private AudioClip _exitSound;
	private Transform _exitTransform;
	private Collider _colliderDoor;
	private Animator _anim;
	private void Awake()
	{
		_anim = GetComponent<Animator>();

        GetSound();
	}

	public void InitAttribute (Transform exitPosition)
	{
		_exitTransform = exitPosition;
		_colliderDoor = GetComponent<Collider>();
		SetDetectOpenDoor(false);
	}
	public void SetDetectOpenDoor(bool isOpen)
	{
		_colliderDoor.enabled = isOpen;
        _anim.SetBool("IsOpenDoor", false);
	}

	private void GetSound()
	{
		_audioSource = GetComponent<AudioSource>();
		if (_audioSource == null)
			_audioSource = gameObject.AddComponent<AudioSource>();

		_exitSound = GameManager.Instance.SoundManager?.GetSoundTreasureItem();
		_audioSource.clip = _exitSound;
	}
	private void OnTriggerEnter(Collider other)
	{
		var runnerObservable = other.GetComponent<RunnerSkillSystem>().RunnerObservable;
		if (runnerObservable == null) return;
        _anim.SetBool("IsOpenDoor", true);
        PlaySound();
		runnerObservable.TryExitDoor(_exitTransform);
		runnerObservable.OnCompeleteMissionStream.Subscribe(isOpen => _anim.SetBool("IsOpenDoor", !isOpen));
	}

	private void PlaySound()
	{
		if (_exitSound == null) return;
		_audioSource.Play();
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
}
