using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.Enemies;
using Assets._GAME.Scripts.Game;
using Assets._SDK.Game;
using Assets._SDK.GamePlay;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Assets._GAME.Scripts.GamePlay
{
	public class TreasureGamePlay : AbstractNextBotGamePlay
	{
		public static TreasureGamePlay Instance => (TreasureGamePlay)GamePlayLoader.Instance.CurrentGamePlay;

		public TreasureObservable TreasureObservable;
		private CompositeDisposable _disposables;

        private int _qualityTreasure;
		public float TargetToWin;

		public List<GameObject> TreasuresSlots => _treasuresSlots;
        [ShowInInspector] private List<GameObject> _treasuresSlots;

		protected override void OnStart()
		{
            gamePlayingMainPanel?.StartUITreasure(TargetToWin);

            TreasureObservable = gameObject.GetComponent<TreasureObservable>();
            if (TreasureObservable == null)
                TreasureObservable = gameObject.AddComponent<TreasureObservable>();
        }
        private void Subcribe()
        {
            RunnerSkillSystem.RunnerObservable.SetTreasureSlot("");

            _disposables = new CompositeDisposable();

            TreasureObservable.OnRemoveTreasureInSlotNullStream
                .Subscribe(nameMonster =>
                {
                    ClearTreasureNullInSlot(nameMonster);
                }).AddTo(_disposables);

            TreasureObservable.OnCollectTreasureStream
                .Subscribe(nameMonster =>
                {
                    ClearTreasureNullInSlot(nameMonster);
                    RunnerSkillSystem.RunnerObservable.SetTreasureSlot(nameMonster);
                }).AddTo(_disposables);
        }
        private void ClearTreasureNullInSlot(string name)
        {
            _treasuresSlots = _treasuresSlots.Where(v => v.name != name).ToList();
        }
        protected override void InitRunner()
        {
            base.InitRunner();
		}

        protected override void InitEnemies()
        {
            base.InitEnemies();
			for (int i = 0; i < EnemyGroups.Count; i++)
			{
				LoadEnemyGroup(GlobalEnemySpawnPoints, RegionalEnemySpawnPoints, EnemyGroups[i]);
			}
            Subcribe();
        }

		void LoadEnemyGroup(List<GameObject> globalEnemyPositions, List<GameObject> regionalEnemyPositions, EnemyGroup enemyGroup)
		{
			if (enemyGroup.Type == EnemyType.Regional)
			{
				for (int i = 0; i < enemyGroup.Amount; i++)
				{
					if (IsEnemySpawnIgnored(i))
						continue;

					RegionalEnemySlot RESlot = new RegionalEnemySlot(MapHolder);
					RESlot.AddBehaviorIfNull(regionalEnemyPositions[amountRE++], enemyGroup.level);
					RESlot.AddSkinIfNull(enemyGroup.EnemySettings.skin);
					RegionalEnemySlots.Add(RESlot);
				}
			}

			if (enemyGroup.Type == EnemyType.Global)
			{
				for (int i = 0; i < enemyGroup.Amount; i++)
				{
					GlobalEnemySlot GESlot = new GlobalEnemySlot(MapHolder);
					GESlot.AddBehaviorIfNull(globalEnemyPositions[amountGE++], enemyGroup.level);
					GESlot.AddSkinIfNull(enemyGroup.EnemySettings.skin);
					GlobalEnemySlots.Add(GESlot);
				}
			}
		}
		public void ReceiveGetTreasure(int value)
		{
			if (CurrentState.Value != GamePlayState.Running)
				return;

			_qualityTreasure += value;
			gamePlayingMainPanel?.UpdateTargetTreasure(_qualityTreasure);

			if (_qualityTreasure >= TargetToWin)
			{
				gamePlayingMainPanel?.UpdateTargetTreasure(TargetToWin);
				RunnerSkillSystem.RemoveSkill();
				RunnerSkillSystem.RunnerObservable.ClearObservers();
				StartCoroutine(DelayToWin());
				Debug.Log("Win");
				return;
			}
		}

		IEnumerator DelayToWin()
		{
			yield return new WaitForSeconds(2f);
			if (CanFire(GamePlayTrigger.Win))
				Fire(GamePlayTrigger.Win);
			Debug.Log("Win");

		}

		public void ResetData()
		{
			TargetToWin = 0;
		}

		protected override void OnInitGamePlay()
		{
			InitTreasureItems();

			TargetToWin = playingMission.TargetWin;

			StateMachine.Activate();

			StateMachine.Fire(GamePlayTrigger.Play);
		}

		private void InitTreasureItems()
		{
			_treasuresSlots = new List<GameObject>();
			GameObject treasure = GameManager.Instance.Resources.Treasure;
			if(treasure == null) return;
			for (int i = 0; i < _playingMap.TreasurePositions.Count; i++)
			{
				GameObject treasureModel = Instantiate(treasure, _playingMap.TreasurePositions[i].transform.position, Quaternion.identity, MapHolder.transform);
				treasureModel.name = $"{treasureModel.name}{i}";
                _treasuresSlots.Add(treasureModel);
			}
        }
		protected override void End()
		{
			base.End();
			foreach (var treasure in _treasuresSlots)
			{
				Destroy(treasure);
			}
			_treasuresSlots.Clear();
		}

		protected override void Reload()
		{
			TargetToWin = 0;
			base.Reload();
		}
	}
}