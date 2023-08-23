using System.Collections;
using _SDK.Inventory;
using _SDK.Money;
using Assets._SDK.Skills;
using Stateless;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets._SDK.Game
{
    public enum GameState
    {
        Init,
        Lobby,
        LobbyHome,
        Shopping,
        ReviewingEnemy,
        Playing
    }

    public enum GameTrigger
    {
        InitToLobby,
        GoShopping,
        BackToLobbyHome,
        GoEnemyList,
        Play,
        End
    }

    public abstract class AbstractGameManager : GameSingleton<AbstractGameManager>
    {
        protected StateMachine<GameState, GameTrigger> StateMachine { get; set; }

        public ReactiveProperty<GameState> PreviousState;
        public ReactiveProperty<GameState> CurrentState;

        public void Fire(GameTrigger trigger) => StateMachine.Fire(trigger);

        public bool CanFire(GameTrigger trigger) => StateMachine.CanFire(trigger);

        public Wallet Wallet { get; protected set; }

        protected override void OnAwake()
        {
            #region INIT
            CurrentState = new ReactiveProperty<GameState>(GameState.Init);
            PreviousState = new ReactiveProperty<GameState>(GameState.Init);

            StateMachine = new StateMachine<GameState, GameTrigger>(CurrentState.Value);

            StateMachine.OnTransitioned((transition) =>
                {
                    CurrentState.Value = transition.Destination;
                    PreviousState.Value = transition.Source;
                });

            StateMachine.Configure(GameState.Init)
                .Permit(GameTrigger.InitToLobby, GameState.Lobby);
            #endregion

            #region LOBBY

            StateMachine.Configure(GameState.LobbyHome)
               .SubstateOf(GameState.Lobby);

            StateMachine.Configure(GameState.Shopping)
                .SubstateOf(GameState.Lobby);

            StateMachine.Configure(GameState.ReviewingEnemy)
                .SubstateOf(GameState.Lobby);

            StateMachine.Configure(GameState.Lobby)
                .InitialTransition(GameState.LobbyHome);

            //StateMachine.Configure(GameState.LobbyHome)
            //    .Permit(GameTrigger.GoShopping, GameState.Shopping);

            StateMachine.Configure(GameState.LobbyHome)
                .Permit(GameTrigger.GoShopping, GameState.Shopping);

            StateMachine.Configure(GameState.LobbyHome)
                .Permit(GameTrigger.GoEnemyList, GameState.ReviewingEnemy);

            StateMachine.Configure(GameState.ReviewingEnemy)
                .Permit(GameTrigger.BackToLobbyHome, GameState.LobbyHome);

            StateMachine.Configure(GameState.Shopping)
                .Permit(GameTrigger.BackToLobbyHome, GameState.LobbyHome);

            #endregion

            #region PLAY
            StateMachine.Configure(GameState.Lobby)
               .Permit(GameTrigger.Play, GameState.Playing);

            StateMachine.Configure(GameState.Playing)
                .Permit(GameTrigger.End, GameState.Lobby);
            #endregion
        }


        public IEnumerator Restart()
        {
            if (StateMachine.CanFire(GameTrigger.End))
            {
                StateMachine.Fire(GameTrigger.End);
            }

            yield return null;
        }

        protected AsyncOperation LoadSceneAsync(string sceneName)
        {
            if (SceneManager.GetActiveScene().name != sceneName)
            {
                return SceneManager.LoadSceneAsync(sceneName);
            }
            else
            {
                return null;
            }
        }
    }
}