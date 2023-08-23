using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.GamePlay;
using Assets._SDK.Input;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._GAME.Scripts.Skills.DetectNearestTarget
{
    public class DetectNearestTargetRunnerSkillBehavior : MonoBehaviour, IObserver
    {
        private const float MIN_DISTANCE_HIDE_SCREEN_INDICATOR = 10f;
        private const float MIN_DISTANCE_TRANFORM_Y = 2F;

        private CompositeDisposable _disposables;
        private RunnerObservable _runnerObservable;
        private GameObject _screenIndicator;
        private OffScreenTargetIndicator _offScreenTargetIndicator;
        private IEnumerator _raycastCheckCoroutine;
        private Color _seekAndKillColor;
        private Color _seekAndKillCollectColor;
        private Color _treasureColor;
        private Sprite _survivalSprite;
        private Sprite _treasureSprite;
        private Sprite _seekAndKillCollectSprite;
        private Sprite _spriteCurrentIndicator;
        private Image _targetImageIndicator;
        void Awake()
        {
			_disposables = new CompositeDisposable();
		}
        private void Start()
        {
            Subscribe();
            StartCoroutine(AfterStart());
        }
        private IEnumerator AfterStart()
        {
            float timeDelayAfterStart = 0.5f;
            yield return new WaitForSecondsRealtime(timeDelayAfterStart);
            SetSpriteIndicatorSpriteTarget();
        }
        private IEnumerator CheckDistanceTarget(Transform target)
        {
            WaitForSeconds timeDelay = new WaitForSeconds(0.5f);
            while (target != null)
            {
                float distanceTarget = Vector3.Distance(transform.position, target.position);
                _screenIndicator.SetActive(distanceTarget > MIN_DISTANCE_HIDE_SCREEN_INDICATOR);
                yield return timeDelay;
            }
            _screenIndicator.SetActive(true);
        }
        private void CheckMonsterHasKeyNearby()
        {
            var seekAndKillCollectGameplay = SeekAndKillCollectGamePlay.Instance;

            List<RegionalEnemySlot> RegionalEnemyHasKeySlots = seekAndKillCollectGameplay.ListRegionalEnemyHasKeySlots;
            List<Transform> listRegionalHasKey = new List<Transform>();
            RegionalEnemyHasKeySlots.ForEach(v => listRegionalHasKey.Add(v.Enemy.transform));

            if(_runnerObservable.GetTotalKey() < seekAndKillCollectGameplay.TargetKeyToWin)
                FindKeyMode(listRegionalHasKey);

            _offScreenTargetIndicator.SetColorIndicator(_seekAndKillCollectColor);
        }
        private void CheckMonsterNearby()
        {
            List<RegionalEnemySlot> RegionalEnemySlots = SeekAndKillGamePlay.Instance.ListRegionalEnemySlots;
            List<Transform> listRegional = new List<Transform>();
            RegionalEnemySlots.ForEach(v => listRegional.Add(v.Enemy.transform));
            FindObjectNearest(listRegional);
            _offScreenTargetIndicator.SetColorIndicator(_seekAndKillColor);
        }
        private void CheckTreasureNearby()
        {
            List<GameObject> treasureSlots = TreasureGamePlay.Instance.TreasuresSlots;
            List<Transform> listTreasure = new List<Transform>();
            treasureSlots.ForEach(v => listTreasure.Add(v.transform));
            FindObjectNearest(listTreasure);
            _offScreenTargetIndicator.SetColorIndicator(_treasureColor);
        }
        private void FindKeyMode(List<Transform> objects)
        {
            Transform objectNearest = FilterObject(objects);
            OnOffTargetScreen(objectNearest);
            _offScreenTargetIndicator = objectNearest.AddComponent<OffScreenTargetIndicator>();
        }
        private void FindObjectNearest(List<Transform> objects)
        {
            Transform objectNearest = FilterObject(objects);

            if(_offScreenTargetIndicator != null)
                Destroy(_offScreenTargetIndicator);
            _offScreenTargetIndicator = objectNearest.AddComponent<OffScreenTargetIndicator>();
        }
        private Transform FilterObject(List<Transform> objects)
        {
            Transform objectNearest = null;
            List<Transform> TranformYDifferents = new List<Transform>();
            float minDistance = float.MaxValue;
            foreach (var child in objects)
            {
                if (child == null) continue;
                CheckObjectNearestHandle(ref objectNearest, ref TranformYDifferents, ref minDistance, child);
            }
            if (objectNearest == null)
                objectNearest = TranformYDifferents[Random.Range(0, TranformYDifferents.Count)];

            return objectNearest;
        }
        private void CheckObjectNearestHandle(ref Transform objectNearest,
            ref List<Transform> TranformYDifferents,
            ref float minDistance,
            Transform child)
        {
            Vector3 offset = child.position - transform.position;
            float sqrLen = offset.sqrMagnitude;
            float offsetY = child.position.y - transform.position.y;

            if (offsetY <= MIN_DISTANCE_TRANFORM_Y)
            {
                if (sqrLen < minDistance)
                {
                    minDistance = sqrLen;
                    objectNearest = child;
                }
            }
            else
                TranformYDifferents.Add(child);
        }
        private void OnOffTargetScreen(Transform target)
        {
            if (_raycastCheckCoroutine != null)
                StopCoroutine(_raycastCheckCoroutine);
            _raycastCheckCoroutine = CheckDistanceTarget(target);
            StartCoroutine(_raycastCheckCoroutine);
        }
        public void LevelUp(GameObject screenIndicatorPrefab,
            Color seekAndKillColor,
            Color seekAndKillCollectColor,
            Color treasureColor,
            Sprite survivalSprite,
            Sprite treasureSprite,
            Sprite seekAndKillCollectSprite)
        {
            _screenIndicator = screenIndicatorPrefab;
            _seekAndKillColor = seekAndKillColor;
            _seekAndKillCollectColor = seekAndKillCollectColor;
            _treasureColor = treasureColor;
            _survivalSprite = survivalSprite;
            _treasureSprite = treasureSprite;
            _seekAndKillCollectSprite = seekAndKillCollectSprite;
        }
        public void Observe(IObservable input)
        {
            if (input == null) return;
            _runnerObservable = (RunnerObservable)input;
        }
        private void SetSpriteIndicatorSpriteTarget()
        {
            if (_spriteCurrentIndicator != null) return;

            var targetImage = MyUtils.FindInChildrenIncludingInactive(_screenIndicator, "TargetImage").GetComponent<Image>();
            targetImage.color = Color.white;
            var modeMission = GamePlayLoader.Instance.CurrentGamePlay.ModeGameMission;
            switch (modeMission)
            {
                case ModeGameMission.Survival:
                    targetImage.sprite = _survivalSprite;
                    break;

                case ModeGameMission.Treasure:
                    targetImage.sprite = _treasureSprite;
                    break;

                case ModeGameMission.SeekAndKill:
                    targetImage.color = new Color(1, 1, 1, 0);
                    break;

                case ModeGameMission.SeekAndKillCollect:
                    targetImage.sprite = _seekAndKillCollectSprite;
                    break;
            }
            _targetImageIndicator = targetImage;
            _spriteCurrentIndicator = targetImage.sprite;

        }
        private void Subscribe()
        {
            if (GamePlayLoader.Instance.CurrentGamePlay.ModeGameMission == ModeGameMission.Survival)
            {
                GameObject exitDoor = SurvivalGamePlay.Instance.ExitDoor;
                var indicatorExitdoor = exitDoor.AddComponent<OffScreenTargetIndicator>();
                indicatorExitdoor.SetColorIndicator(Color.green);
                OnOffTargetScreen(exitDoor.transform);
            }
            _runnerObservable.OnRegionalStream
                .Where(_
                => GamePlayLoader.Instance.CurrentGamePlay.ModeGameMission == ModeGameMission.SeekAndKill)
                .Subscribe(_ => CheckMonsterNearby())
                .AddTo(_disposables);

            _runnerObservable.OnChangeNearTarget
                .Where(_
                => GamePlayLoader.Instance.CurrentGamePlay.ModeGameMission == ModeGameMission.SeekAndKillCollect)
                .Subscribe(_ => CheckMonsterHasKeyNearby())
                .AddTo(_disposables);

            _runnerObservable.OnFindTreasurelStream
                .Where(_
                => GamePlayLoader.Instance.CurrentGamePlay.ModeGameMission == ModeGameMission.Treasure)
                .Subscribe(_ => CheckTreasureNearby())
                .AddTo(_disposables);

            _runnerObservable.OnExitDoorOpened.Where(_
                => GamePlayLoader.Instance.CurrentGamePlay.ModeGameMission == ModeGameMission.SeekAndKillCollect)
                .Subscribe(_ => {
                    Destroy(_offScreenTargetIndicator);
                    SeekAndKillCollectGamePlay.Instance.ExitDoor.AddComponent<OffScreenTargetIndicator>().SetColorIndicator(Color.green);
					OnOffTargetScreen(SeekAndKillCollectGamePlay.Instance.ExitDoor);
                    _targetImageIndicator.sprite = _survivalSprite;
                })
				.AddTo(_disposables);
        }
        public void UnSubscribe()
        {
            _disposables?.Clear();
        }
        private void OnDisable()
        {
            _disposables?.Clear();
        }
        private void OnDestroy()
        {
            _disposables?.Clear();

            if (_screenIndicator != null)
                Destroy(_screenIndicator);
        }
    }
}