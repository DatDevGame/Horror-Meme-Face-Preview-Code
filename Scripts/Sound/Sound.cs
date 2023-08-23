using System;
using System.Collections.Generic;
using Assets._GAME.Scripts.Skills;
using Assets._GAME.Scripts.Skills.Live;
using Assets._GAME.Scripts.Skills.Move;
using Assets._GAME.Scripts.Skills.TurnLightOnOff;
using Assets._SDK.Entities;
using Assets._SDK.Inventory.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    [Serializable]
    public class Sound : AbstractEntity
    {
        public override int Id => (nameof(Sound) + Name).GetHashCode();

        [BoxGroup("Runner")] public AudioClip RunnerWalk;
        [BoxGroup("Runner")] public AudioClip RunnerRun;
        [BoxGroup("Runner")] public AudioClip RunnerAttack;
		[BoxGroup("Runner")] public AudioClip RunnerTired;
		[BoxGroup("Runner")] public AudioClip RunnerLowHP;
		[BoxGroup("Runner")] public AudioClip MaxStamina;

        [BoxGroup("Enemy")] public AudioClip EnemyReceiveDame;
        [BoxGroup("Enemy")] public AudioClip EnemyAttack;
        [BoxGroup("Enemy")] public List<AudioClip> ListJumpScare;
        [BoxGroup("Enemy")] public List<AudioClip> ListChasing;

        [BoxGroup("Other")] public AudioClip SoundBackGroundLobby;
        [BoxGroup("Other")] public AudioClip SoundBackGroundInGame;
        [BoxGroup("Other")] public AudioClip Win;
        [BoxGroup("Other")] public AudioClip Lose;
        [BoxGroup("Other")] public AudioClip HealItem;
        [BoxGroup("Other")] public AudioClip StaminaItem;
        [BoxGroup("Other")] public AudioClip TreasureItem;
        [BoxGroup("Other")] public AudioClip BreakTimeInGame;

    }
}