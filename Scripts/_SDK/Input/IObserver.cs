using Assets._GAME.Scripts.Skills.TurnLightOnOff;
using System.Collections;
using UnityEngine;

namespace Assets._SDK.Input
{
    public interface IObserver
    {
        void Observe(IObservable observable);
    }
}