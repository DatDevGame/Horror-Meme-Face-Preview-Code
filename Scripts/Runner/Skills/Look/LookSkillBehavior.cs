using Assets._SDK.Skills.Attributes;
using Assets._SDK.Input;
using System;
using UniRx;
using UnityEngine;
using System.Data.SqlTypes;
using System.Collections;
using Sirenix.OdinInspector;
using _GAME.Scripts.Inventory;
using UnityEngine.Rendering.Universal;
using Assets._GAME.Scripts.GamePlay;

namespace Assets._GAME.Scripts.Skills.Move
{
	public class LookSkillBehavior : MonoBehaviour, IObserver
	{
		private const float FIELD_OF_VIEW_RUN = 71f;
		private const float FAR_CLIP_PLANE = 200f;

		[SerializeField]
		private PercentAttribute _speed;
		[SerializeField,ReadOnly]
		private PercentAttribute _timeInertia;

		private Camera _camera;
		private Transform _cameraTransform;

		private Vector2 _lookInput;
		private float _cameraPitchX;
		private float _cameraPitchY;
		private Vector2 _cameraSpeedRotate;
		private int _leftFingerId, _rightFingerId;
		private float _halfScreenWidth;

		private float _fieldOfViewWalk = 0f;
		private CompositeDisposable _disposables;

		IEnumerator _rotationInertia;
		IEnumerator _fieldOfViewRun;
		IEnumerator _scanOfViewCameraUpdate;

		private const float MAX_ROTATEY_BEHIND = 175F;
		private const float SPEED_ROTATE_BEHIND = 500F;

		RunnerObservable runnerObservable;

		void Awake()
		{
			_speed = new PercentAttribute(0, 0);
			_timeInertia = new PercentAttribute(0, 0);

			_camera = gameObject.GetComponentInChildren<Camera>();
			_cameraTransform = _camera.transform;
			_fieldOfViewWalk = _camera.fieldOfView;

			//transform.localRotation = Quaternion.Euler(0, 0, 0);


			_disposables = new CompositeDisposable();

			// only calculate once
			_halfScreenWidth = Screen.width / 2;
			ResetTouchPlayer();

		}

		private void Start()
		{
			_cameraPitchX = -transform.eulerAngles.y;
			_cameraTransform.localRotation = Quaternion.Euler(0, 0, 0);

			ParticleSystem EffectScan = GetComponentInChildren<ParticleSystem>();
			foreach (var rendererFeature in GamePlayLoader.Instance.CurrentGamePlay.UniversalRendererData.rendererFeatures)
			{
				if (rendererFeature.name.CompareTo("Hidden") == 0)
				{
					rendererFeature.SetActive(false);
					runnerObservable.IsScanAllEnemies.Subscribe((isScane) =>
					{
						if (EffectScan != null)
						{
							EffectScan.startColor = isScane ? Color.red : Color.blue;
							EffectScan.Play();
						}

						
						if (_scanOfViewCameraUpdate != null)
							StopCoroutine(_scanOfViewCameraUpdate);

						_scanOfViewCameraUpdate = ScanOfViewCameraUpdate(rendererFeature, isScane);
						StartCoroutine(_scanOfViewCameraUpdate);
					}).AddTo(_disposables);
				}

				if (rendererFeature.name.CompareTo("Invisible") == 0)
				{
					rendererFeature.SetActive(false);
					runnerObservable.IsInvisible.Subscribe((isInvisible) =>
					{
						rendererFeature.SetActive(isInvisible);
					}).AddTo(_disposables);
				}
			}
		}
		IEnumerator ScanOfViewCameraUpdate(ScriptableRendererFeature rendererFeature, bool isScane)
		{
			yield return new WaitForSeconds(1f);
			rendererFeature.SetActive(isScane);

			//_camera.farClipPlane = 5f;
			//while (_camera.farClipPlane <= FAR_CLIP_PLANE)
			//{
			//	_camera.farClipPlane = Mathf.MoveTowards(_camera.farClipPlane, FAR_CLIP_PLANE, Time.deltaTime * 10f);

			//	yield return new WaitForEndOfFrame();
			//}
			//_camera.farClipPlane = FAR_CLIP_PLANE;
			//yield return null;
		}

		IEnumerator LookBehind()
		{
			float rotateY = 0;
			float lookValueX = _cameraPitchY / 18f;
			float lookValueY;
			Vector2 direction;
			WaitForFixedUpdate fixUpdate = new WaitForFixedUpdate();

			while (rotateY <= MAX_ROTATEY_BEHIND)
			{
				lookValueY = 1 + Time.deltaTime * SPEED_ROTATE_BEHIND;
				direction = new Vector2(lookValueY, lookValueX);
				rotateY += lookValueY;
				OnRotate(direction);
				yield return fixUpdate;
			}
		}

		private void ResetTouchPlayer()
		{
			_leftFingerId = -1;
			_rightFingerId = -1;

			_cameraSpeedRotate = Vector2.zero;

		}

		void LookAround()
		{
			if (_rightFingerId != -1)
			{
				_cameraSpeedRotate = _lookInput;
				OnRotate(_cameraSpeedRotate);
			}
			else
			{
				if (_timeInertia.Point == 0) return;

				if (_rotationInertia != null)
					StopCoroutine(_rotationInertia);

				_rotationInertia = OnRotateInertia();

				StartCoroutine(_rotationInertia);
			}

		}

		private void OnRotate(Vector2 moveVector)
		{
			// horizontal (yaw) rotation
			_cameraPitchX = _cameraPitchX - moveVector.x;
			transform.localRotation = Quaternion.Euler(0, -_cameraPitchX, 0);

			// vertical (pitch) rotation
			_cameraPitchY = Mathf.Clamp(_cameraPitchY - moveVector.y, - 90f, 90f);
			_cameraTransform.localRotation = Quaternion.Euler(_cameraPitchY, 0, 0);
		}

		IEnumerator OnRotateInertia()
		{
			while (_cameraSpeedRotate.magnitude > 0.01f)
			{
				_cameraSpeedRotate = Vector2.Lerp(_cameraSpeedRotate, Vector2.zero, Time.fixedDeltaTime * _speed.Point);
				OnRotate(_cameraSpeedRotate);

				yield return new WaitForEndOfFrame();
			}

			yield return null;
		}

		void GetTouchInput(Touch[] arrayTouch)
		{
			// Iterate through all the detected touches
			for (int i = 0; i < arrayTouch.Length; i++)
			{
				Touch t = arrayTouch[i];
				switch (t.phase)
				{
					case TouchPhase.Began:

						if (t.position.x < _halfScreenWidth && _leftFingerId == -1)
						{
							// Start tracking the left finger if it was not previously being tracked
							_leftFingerId = t.fingerId;
						}
						else if (t.position.x > _halfScreenWidth && _rightFingerId == -1)
						{
							// Start tracking the rightfinger if it was not previously being tracked
							_rightFingerId = t.fingerId;
							_lookInput = t.deltaPosition * _speed.Point * Time.fixedDeltaTime;
							LookAround();
						}

						break;
					case TouchPhase.Ended:
						if (t.fingerId == _leftFingerId)
						{
							// Stop tracking the left finger
							_leftFingerId = -1;
						}
						else if (t.fingerId == _rightFingerId)
						{
							// Stop tracking the right finger
							_rightFingerId = -1;
							_lookInput = t.deltaPosition * _speed.Point * Time.fixedDeltaTime;

							LookAround();
						}
						break;

					case TouchPhase.Canceled:

						if (t.fingerId == _leftFingerId)
						{
							// Stop tracking the left finger
							_leftFingerId = -1;
						}
						else if (t.fingerId == _rightFingerId)
						{
							// Stop tracking the right finger
							_rightFingerId = -1;
						}

						break;
					case TouchPhase.Moved:

						// Get input for looking around
						if (t.fingerId == _rightFingerId)
						{
							_lookInput = t.deltaPosition * _speed.Point * Time.fixedDeltaTime;
							LookAround();
						}
						else if (t.fingerId == _leftFingerId)
						{

							// calculating the position delta from the start position
							// moveInput = t.position - moveTouchStartPosition;
						}

						break;
					case TouchPhase.Stationary:
						// Set the look input to zero if the finger is still
						if (t.fingerId == _rightFingerId)
						{
							_lookInput = Vector2.zero;

							LookAround();
						}
						break;
				}
			}
		}


		public void Observe(IObservable observable)
		{
			runnerObservable = (RunnerObservable)observable;
			if (observable == null) return;
			runnerObservable.RotationStream.Subscribe((arrayTouch) =>
			{
				GetTouchInput(arrayTouch);
			}).AddTo(_disposables);

			runnerObservable.RunStream.Subscribe((moveVector) =>
			{
				if (_fieldOfViewRun != null)
					StopCoroutine(_fieldOfViewRun);

				_fieldOfViewRun = FieldOfViewRun(moveVector != Vector3.zero);
				StartCoroutine(_fieldOfViewRun);
			}).AddTo(_disposables);

			runnerObservable.WalkStream.Subscribe((moveVector) =>
			{
				if(_camera.fieldOfView == _fieldOfViewWalk) return;

					if (_fieldOfViewRun != null)
					StopCoroutine(_fieldOfViewRun);
				_fieldOfViewRun = FieldOfViewRun(false);
				StartCoroutine(_fieldOfViewRun);
			}).AddTo(_disposables);

			runnerObservable.StartPause.Subscribe((isPause) =>
			{
				ResetTouchPlayer();
			}).AddTo(_disposables);

			runnerObservable.LookBehindStream.Subscribe((_) =>
			{
				StartCoroutine(LookBehind());
			}).AddTo(_disposables);
		}

		IEnumerator FieldOfViewRun(bool fieldOfView)
		{
			float targetField = fieldOfView ? FIELD_OF_VIEW_RUN : _fieldOfViewWalk;
			while (_camera.fieldOfView != targetField)
			{
				_camera.fieldOfView = Mathf.MoveTowards(_camera.fieldOfView, targetField, Time.deltaTime * FIELD_OF_VIEW_RUN);

				yield return new WaitForEndOfFrame();
			}
			yield return null;
		}

		private void OnDestroy()
		{
			ResetTouchPlayer();
			_camera.fieldOfView = _fieldOfViewWalk;
			_disposables?.Clear();
		}

		public void LevelUp(LookSkillLevel rotationSkillLevel)
		{
			_speed = _speed.GetModifiedAttribute(rotationSkillLevel.modifierOperator, rotationSkillLevel.Speed);
			_timeInertia = _timeInertia.GetModifiedAttribute(rotationSkillLevel.modifierOperator, rotationSkillLevel.TimeInertia);
		}
	}
}