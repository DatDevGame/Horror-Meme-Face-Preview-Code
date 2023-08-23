using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.GamePlay;
using Assets._GAME.Scripts.Skills;
using Assets._SDK.Inventory.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureGameItem : MonoBehaviour
{
	[SerializeField]
	private GameObject EffectHit;

	private const float TIME_DESTROY = 3F;
	private AudioSource _audioSource;
	private AudioClip _treasureSound;

	IEnumerator _delayDestroy;
	private void Awake()
    {
		GetSound();
	}
	private void GetSound()
	{
		_audioSource = GetComponent<AudioSource>();
		if (_audioSource == null)
			_audioSource = gameObject.AddComponent<AudioSource>();

		_treasureSound = GameManager.Instance.SoundManager?.GetSoundTreasureItem();
		_audioSource.clip = _treasureSound;
	}
	private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.tag == "Runner")
		{
			this.GetComponent<Collider>().enabled = false;
			PlaySound();
			GetTreasure();
		}
	}

	private void CallCollected()
	{
        var treasureObservable = TreasureGamePlay.Instance.TreasureObservable;
        if (treasureObservable != null)
        {
            var indicatorTarget = gameObject.transform.parent.GetComponent<OffScreenTargetIndicator>();
            if (indicatorTarget != null)
            {
                Destroy(indicatorTarget);
                treasureObservable.SetCollectedTreasure(gameObject.transform.parent.name);
            }
            treasureObservable.SetNameTreasureNeedRemoveSlot(gameObject.transform.parent.name);
        }
    }
	private void GetTreasure()
	{
		CallCollected();

        EffectHit.SetActive(true);
		this.GetComponentInChildren<MeshRenderer>().gameObject.SetActive(false);
		TreasureGamePlay.Instance.ReceiveGetTreasure(1);

		if (_delayDestroy != null)
			StopCoroutine(_delayDestroy);

		_delayDestroy = DestroyObject();
		StartCoroutine(_delayDestroy);
	}

	private void PlaySound()
	{
		if (_treasureSound == null) return;
		_audioSource.Play();
	}

	private void OnDestroy()
	{
		if (_delayDestroy != null)
			StopCoroutine(_delayDestroy);
	}
	IEnumerator DestroyObject()
	{
		yield return new WaitForSeconds(TIME_DESTROY);
		Destroy(this.gameObject);
	}
}
