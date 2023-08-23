using _GAME.Scripts.Shop;
using _SDK.UI;
using Assets._SDK.Game;
using Stateless;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace _GAME.Scripts.Lobby
{
    public class LobbySceneUI : AbstractSceneUI
    {
        private string LobbyHomePanel;
        private string ShopPanel;
        private string EnemyListPanel;
		private string LoadingPanel;

		private LoadingPanel _loadingPanelUI;
		public LoadingPanel LoadingPanelUI
		{
			get
			{
				if (_loadingPanelUI == null) _loadingPanelUI = (LoadingPanel)GetOrAddPanel(nameof(LoadingPanel));
				return _loadingPanelUI;
			}
		}
        private WeaponShopPanel _weaponPanelUI;
        public WeaponShopPanel WeaponPanelUI
        {
            get
            {
                if (_weaponPanelUI == null) _weaponPanelUI = (WeaponShopPanel)GetOrAddPanel(nameof(WeaponPanelUI));
                return _weaponPanelUI;
            }
        }

        protected override void OnStart()
        {
            #region Subscribe Game State

            GameManager.Instance.CurrentState.Where((value) => value == GameState.LobbyHome)
                .Subscribe(_ => ShowPanel(nameof(LobbyHomePanel))).AddTo(gameObject);
            GameManager.Instance.PreviousState.Where((value) => value == GameState.LobbyHome)
                .Subscribe(_ => HidePanel(nameof(LobbyHomePanel))).AddTo(gameObject);

            GameManager.Instance.CurrentState.Where((value) => value == GameState.Shopping)
                .Subscribe(_ => ShowPanel(nameof(ShopPanel))).AddTo(gameObject);
            GameManager.Instance.PreviousState.Where((value) => value == GameState.Shopping)
                .Subscribe(_ => HidePanel(nameof(ShopPanel))).AddTo(gameObject);

            // GameManager.Instance.CurrentState.Where((value) => value == GameState.Shopping)
            //     .Subscribe(_ => ShowPanel(nameof(ShopPanel))).AddTo(GameObject);
            // GameManager.Instance.PreviousState.Where((value) => value == GameState.Shopping)
            //     .Subscribe(_ => HidePanel(nameof(ShopPanel))).AddTo(GameObject);

            //GameManager.Instance.CurrentState.Where((value) => value == GameState.Shopping)
            //    .Subscribe(_ => {
            //        var panel = (LobbyHomePanel) GetOrAddPanel(nameof(LobbyHomePanel));
            //        panel.EnableOrDisableUnlockMission(true);
            //    }).AddTo(gameObject);
            //GameManager.Instance.PreviousState.Where((value) => value == GameState.Shopping)
            //    .Subscribe(_ => {
            //        var panel = (LobbyHomePanel) GetOrAddPanel(nameof(LobbyHomePanel));
            //        panel.EnableOrDisableUnlockMission(false);
            //    }).AddTo(gameObject);

            GameManager.Instance.CurrentState.Where((value) => value == GameState.ReviewingEnemy)
                .Subscribe(_ =>  ShowPanel(nameof(EnemyListPanel))).AddTo(gameObject);
            GameManager.Instance.PreviousState.Where((value) => value == GameState.ReviewingEnemy)
                .Subscribe(_ => HidePanel(nameof(EnemyListPanel))).AddTo(gameObject);

			GameManager.Instance.CurrentState.Where((value) => value == GameState.Playing)
				.Subscribe(_ => ShowPanel(nameof(LoadingPanel))).AddTo(gameObject);
			GameManager.Instance.PreviousState.Where((value) => value == GameState.Playing)
				.Subscribe(_ => HidePanel(nameof(LoadingPanel))).AddTo(gameObject);
            #endregion

            //TODO: It should not call here
            GameManager.Instance.LoadingPanel = LoadingPanelUI;
		}


    }
}