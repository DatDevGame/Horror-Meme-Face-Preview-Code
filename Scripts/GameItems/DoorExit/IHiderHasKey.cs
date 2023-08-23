using _SDK.Entities;
using System;
using UniRx;
using UnityEngine;

namespace Assets._GAME.GameItems.DoorExit
{
	public interface IFoundKeyObservable : IObservable
	{
		public ReactiveProperty<int> TotalKeysFound { get; }
		public IObservable<int> OnFoundKeyStream => TotalKeysFound;
		public void SetTotalKeysFound(int totalKeysFound);

		public Subject<Transform> ExitDoor { get; }
		public IObservable<Transform> OnExitDoorStream => ExitDoor;
		public void TryExitDoor(Transform positionTagert);
	}
}
