using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Assets._GAME.Scripts.GamePlay
{
    public class SeekAndKillObservable : MonoBehaviour, IObservable
    {
        private Subject<string> _haveMonsterDead;
        private Subject<string> _clearMonsterInSlotWhenDead;

        private void Awake()
        {
            _haveMonsterDead = new Subject<string>();
            _clearMonsterInSlotWhenDead = new Subject<string>();
        }

        public IObservable<string> OnMonsterDeadStream => _haveMonsterDead;
        public void SetHaveMonsterDead(string monster)
        {
            _haveMonsterDead.OnNext(monster);
        }

        public IObservable<string> OnRemoveSlotStream => _clearMonsterInSlotWhenDead;
        public void SetNameMonsterNeedRemoveSlot(string nameMonsterRemove)
        {
            _clearMonsterInSlotWhenDead.OnNext(nameMonsterRemove);
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
