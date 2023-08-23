using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbtractTrapJumpScare : MonoBehaviour
{
	const string CAMERA_CHECK_JUMPSCARE = "CheckJumpScare";

	protected AudioSource _audioSource;
	protected const float TIME_DESTROY = 3F;

	private const float MIN_ANGLE = 140;
	private const float MAX_ANGLE = 220;

	protected void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		if (_audioSource == null)
			_audioSource = gameObject.AddComponent<AudioSource>();
	}
	protected virtual void ActiveJumpScare()
	{
		_audioSource.Play();
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == CAMERA_CHECK_JUMPSCARE)
		{
			if (other.transform.eulerAngles.y + transform.eulerAngles.y > transform.eulerAngles.y + MIN_ANGLE &&
				other.transform.eulerAngles.y + transform.eulerAngles.y < transform.eulerAngles.y + MAX_ANGLE)
			{
				ActiveJumpScare();
				this.GetComponent<Collider>().enabled = false;
			}
		}
	}
}
