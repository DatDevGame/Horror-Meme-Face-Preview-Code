
using Stateless;
﻿using _GAME.Scripts.Inventory;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Assets._SDK.GamePlay
{
    public enum GamePlayState
    {
        Init,
        Running,
        Pausing,
        PauseSettings,
        PauseUpgradingSkill,
        PauseCollectingSkin,
        Won,
        Lost,
		Reload
	}

    public enum GamePlayTrigger
    {
        InitMap,
        InitEnemies,
		InitRunner,
		SetupGamePlay,
        Play,
        ShowTutorial,
        UpgradeSkill,
        ShowPausePanel,
        CollectNewSkin,
        SetupEasyMode,
        Pause,
        UnPause,
        Win,
        Lose,
        Retry
    }
    public class AbstractGamePlay : MonoBehaviour
    {
        protected StateMachine<GamePlayState, GamePlayTrigger> StateMachine { get; set; }

        public ReactiveProperty<GamePlayState> PreviousState { get; private set; }
        public ReactiveProperty<GamePlayState> CurrentState { get; private set; }

        public void Fire(GamePlayTrigger trigger) => StateMachine.Fire(trigger);

        public bool CanFire(GamePlayTrigger trigger) => StateMachine.CanFire(trigger);

        private void Awake()
        {
            OnAwake();
		}

        public void OnAwake()
        {
            CurrentState = new ReactiveProperty<GamePlayState>(GamePlayState.Init);
            PreviousState = new ReactiveProperty<GamePlayState>(GamePlayState.Init);

            StateMachine = new StateMachine<GamePlayState, GamePlayTrigger>(CurrentState.Value);

            StateMachine.OnTransitioned((transition) =>
            {
                CurrentState.Value = transition.Destination;
                PreviousState.Value = transition.Source;
            });

            StateMachine.Configure(GamePlayState.Init)
				.InternalTransition(GamePlayTrigger.SetupGamePlay, () => SetupGamePlay())
                .Permit(GamePlayTrigger.Play, GamePlayState.Running);

            StateMachine.Configure(GamePlayState.Running)
                .InternalTransition(GamePlayTrigger.ShowTutorial, () => ShowTutorial())
                .InternalTransition(GamePlayTrigger.SetupEasyMode, () => SetupEasyMode())
                .Permit(GamePlayTrigger.Pause, GamePlayState.Pausing)
                .Permit(GamePlayTrigger.ShowPausePanel, GamePlayState.PauseSettings)
                .Permit(GamePlayTrigger.UpgradeSkill, GamePlayState.PauseUpgradingSkill)
                .Permit(GamePlayTrigger.CollectNewSkin, GamePlayState.PauseCollectingSkin)
                .Permit(GamePlayTrigger.Win, GamePlayState.Won)
                .Permit(GamePlayTrigger.Lose, GamePlayState.Lost);

            StateMachine.Configure(GamePlayState.PauseSettings)
                .SubstateOf(GamePlayState.Pausing);

            StateMachine.Configure(GamePlayState.PauseUpgradingSkill)
                .SubstateOf(GamePlayState.Pausing);

            StateMachine.Configure(GamePlayState.PauseCollectingSkin)
                .SubstateOf(GamePlayState.Pausing);

            StateMachine.Configure(GamePlayState.Pausing)
                .Permit(GamePlayTrigger.Play, GamePlayState.Running)
                .OnEntry(Pause)
                .OnExit(UnPause);

            StateMachine.Configure(GamePlayState.Won)
				.Permit(GamePlayTrigger.Retry, GamePlayState.Reload)
				.OnEntry(End);

			StateMachine.Configure(GamePlayState.Lost)
                .Permit(GamePlayTrigger.Retry, GamePlayState.Reload)
				.OnEntry(JumpScare);

			StateMachine.Configure(GamePlayState.Reload)
                .OnEntry(Reload);

			StateMachine.Configure(GamePlayState.Reload)
				.Permit(GamePlayTrigger.Play, GamePlayState.Running);
		}

        protected virtual void ShowTutorial()
        {
            
        }

        protected virtual void SetupEasyMode()
        {

        }
        protected virtual void SetupGamePlay()
        {

        }

		protected virtual void Reload()
		{

		}

		protected virtual void End()
		{

		}

		protected virtual void JumpScare()
		{
            End();
		}

		protected virtual void Pause()
        {
            
        }

        protected virtual void UnPause()
        {

        }
    }
}