using System;
using UniRx;
using UnityEngine;

namespace Assets._GAME.Scripts.GamePlay
{
    public class TreasureObservable : MonoBehaviour, IObservable
    {
        private Subject<string> _haveCollectTreasure;
        private Subject<string> _clearTreasureInListToNull;

        private void Awake()
        {
            _haveCollectTreasure = new Subject<string>();
            _clearTreasureInListToNull = new Subject<string>();
        }

        public IObservable<string> OnCollectTreasureStream => _haveCollectTreasure;
        public void SetCollectedTreasure(string monster)
        {
            _haveCollectTreasure.OnNext(monster);
        }
        public IObservable<string> OnRemoveTreasureInSlotNullStream => _clearTreasureInListToNull;
        public void SetNameTreasureNeedRemoveSlot(string nameMonsterRemove)
        {
            _clearTreasureInListToNull.OnNext(nameMonsterRemove);
        }

        public void AddDisposable(IDisposable disposable)
        {
            throw new NotImplementedException();
        }

        public void ClearObservers()
        {
            throw new NotImplementedException();
        }
    }
}

