using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.GamePlay;
using Assets._GAME.Scripts.Skills;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalTargetHandle : AbstractTargetHandle
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Transform _holderTranform;
    [SerializeField] private GameObject _exitDoorPrefab;

    private CompositeDisposable _disposables;
    private GameObject _exitDoorGameObject;
    private void Awake()
    {
        _disposables = new CompositeDisposable();
    }
    public override void TargetHandle(float currentTarget, Skin skin = null)
    {
        _timeText.SetText($"{currentTarget}");
        Subcribe();
    }
    private void Subcribe()
    {
        _disposables.Clear();

        if (_exitDoorGameObject != null)
            Destroy(_exitDoorGameObject);
        _exitDoorGameObject = Instantiate(_exitDoorPrefab, Vector3.zero, Quaternion.identity, _holderTranform.transform);

        var runnerObservable = GamePlayLoader.Instance.Runner.GetComponent<RunnerSkillSystem>().RunnerObservable;
        runnerObservable.OnExitDoorStream
            .Subscribe(_ =>
            {
                if (_exitDoorGameObject == null) return;
                _exitDoorGameObject.GetComponent<Image>().enabled = false;
                _exitDoorGameObject.transform.GetChild(0).gameObject.SetActive(true);
            }).AddTo(_disposables);
    }
    private void OnDisable()
    {
        _disposables.Clear();
    }
    private void OnDestroy()
    {
        _disposables.Clear();
    }
}
