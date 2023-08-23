using _SDK.UI;
using Assets._SDK.Game;
using Stateless;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Assets._SDK.GamePlay;
using Assets._GAME.Scripts.GamePlay;

namespace Assets._SDK.Game
{
	public class GameSceneUI : AbstractSceneUI
	{
		private string WinPanel;
		private string LosePanel;
		private string PausePanel;
		private string SkillSupportItemPanel;
		private string JumpScarePanel;
		private string GamePlayingPanel;
		private string CollectEnemyPanel;

		private CompositeDisposable _disposables;

		private PausePanel _pausePanel;
		public PausePanel PausePanelUI
		{
			get
			{
				if (_pausePanel == null) _pausePanel = (PausePanel)GetOrAddPanel(nameof(PausePanel));
				return _pausePanel;
			}
		}
		[SerializeField]
		private JumpScarePanel _jumpScarePanel;
		public JumpScarePanel JumpScarePanelUI
		{
			get
			{
				if (_jumpScarePanel == null) _jumpScarePanel = (JumpScarePanel)GetOrAddPanel(nameof(JumpScarePanel));
				return _jumpScarePanel;
			}
		}
		[SerializeField]
		private GamePlayingMainPanel _gamePlayingMainPanel;
		public GamePlayingMainPanel GamePlayingMainPanelUI
		{
			get
			{
				if (_gamePlayingMainPanel == null) _gamePlayingMainPanel = (GamePlayingMainPanel)GetOrAddPanel(nameof(GamePlayingPanel));
				return _gamePlayingMainPanel;
			}
		}

		private CollectEnemyPanel _collectEnemyPanel;
		public CollectEnemyPanel CollectEnemyUI
		{
			get
			{
				if (_collectEnemyPanel == null) _collectEnemyPanel = (CollectEnemyPanel)GetOrAddPanel(nameof(CollectEnemyPanel));
				return _collectEnemyPanel;
			}
		}

		private SkillSupportItemPanel _skillSupportItemPanel;
		public SkillSupportItemPanel SkillSupportItemUI
		{
			get
			{
				if (_skillSupportItemPanel == null) _skillSupportItemPanel = (SkillSupportItemPanel)GetOrAddPanel(nameof(SkillSupportItemPanel));
				return _skillSupportItemPanel;
			}
		}
		protected override void OnStart()
		{
			PausePanelUI.Init();
		}
		public void Reload(AbstractGamePlay GamePlay)
		{
			Subcribe(GamePlay);
			JumpScarePanelUI?.Reload();
			GamePlayingMainPanelUI?.ReloadUI();
		}
		public void Subcribe(AbstractGamePlay GamePlay)
		{
			if (_disposables == null)
				_disposables = new CompositeDisposable();

			#region Subscribe Game State
			GamePlay?.CurrentState.Where((value) => value == GamePlayState.PauseSettings || value == GamePlayState.Pausing)
				.Subscribe(_ => ShowPanel(nameof(PausePanel))).AddTo(_disposables);
			GamePlay?.CurrentState.Where((value) => value != GamePlayState.PauseSettings && value != GamePlayState.Pausing)
				.Subscribe(_ => HidePanel(nameof(PausePanel))).AddTo(_disposables);

			GamePlay?.CurrentState.Where((value) => value == GamePlayState.PauseUpgradingSkill)
				.Subscribe(_ => ShowPanel(nameof(SkillSupportItemPanel))).AddTo(_disposables);
			GamePlay?.CurrentState.Where((value) => value != GamePlayState.PauseUpgradingSkill)
				.Subscribe(_ => HidePanel(nameof(SkillSupportItemPanel))).AddTo(_disposables);

            GamePlay?.CurrentState.Where((value) => value == GamePlayState.PauseCollectingSkin)
                .Subscribe(_ => 
				{
					CollectEnemyUI.SetData(SeekAndKillGamePlay.Instance.CollectingSkin);
					ShowPanel(nameof(CollectEnemyPanel));
				}).AddTo(_disposables);

            GamePlay?.CurrentState.Where((value) => value != GamePlayState.PauseCollectingSkin)
                .Subscribe(_ => HidePanel(nameof(CollectEnemyPanel))).AddTo(_disposables);

            GamePlay?.CurrentState.Where((value) => value == GamePlayState.Lost)
				.Subscribe(_ => ShowPanel(nameof(JumpScarePanel))).AddTo(_disposables);
			GamePlay?.CurrentState.Where((value) => value != GamePlayState.Lost)
				.Subscribe(_ => HidePanel(nameof(JumpScarePanel))).AddTo(_disposables);

			GamePlay?.CurrentState.Where((value) => value == GamePlayState.Won)
				.Subscribe(_ => ShowPanel(nameof(WinPanel))).AddTo(_disposables);
			GamePlay?.CurrentState.Where((value) => value != GamePlayState.Won)
				.Subscribe(_ => HidePanel(nameof(WinPanel))).AddTo(_disposables);

			//GamePlay?.CurrentState.Where((value) => value == GamePlayState.Lost)
			//	.Subscribe(_ => ShowPanel(nameof(LosePanel))).AddTo(_disposables);
			GamePlay?.CurrentState.Where((value) => value != GamePlayState.Lost)
				.Subscribe(_ => HidePanel(nameof(LosePanel))).AddTo(_disposables);
			#endregion
		}

		public void UnSubcribe()
		{
			_disposables?.Clear();
		}
	}
}