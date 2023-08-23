using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Assets._GAME.Scripts.GamePlay
{
    public class SeekAndSkillCollectObservable : MonoBehaviour, IObservable
    {
        private Subject<GameObject> _monsterHasKeyDead;
        private Subject<string> _clearMonsterHasKeyInSlotWhenDead;

        private void Awake()
        {
            _monsterHasKeyDead = new Subject<GameObject>();
            _clearMonsterHasKeyInSlotWhenDead = new Subject<string>();
        }

        public IObservable<GameObject> OnMonsterHasKeyDeadStream => _monsterHasKeyDead;
        public void SetHaveMonsterDead(GameObject monster)
        {
            _monsterHasKeyDead.OnNext(monster);
        }

        public IObservable<string> OnRemoveSlotStream => _clearMonsterHasKeyInSlotWhenDead;
        public void SetNameMonsterNeedRemoveSlot(string nameMonsterRemove)
        {
            _clearMonsterHasKeyInSlotWhenDead.OnNext(nameMonsterRemove);
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
