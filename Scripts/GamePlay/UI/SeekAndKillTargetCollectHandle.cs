using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.GamePlay;
using Assets._GAME.Scripts.Skills;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SeekAndKillTargetCollectHandle : AbstractTargetHandle
{
    [SerializeField] private GameObject _exitDoorPrefab;

    private GameObject _exitDoorGameObject;
    private CompositeDisposable _disposables;
    private void Awake()
    {
        _disposables = new CompositeDisposable();
    }
    public override void AddTarget(int targetWin)
    {
        base.AddTarget(targetWin);
        Subcribe();
    }
	public override void TargetHandle(float currentTarget, Skin skin = null)
	{
		var item = this.transform.GetChild((int)currentTarget - 1);
		if (item == null) return;

		item.GetComponent<Image>().enabled = false;
		item.transform.GetChild(0).gameObject.SetActive(true);
	}
	private void Subcribe()
	{
        _disposables.Clear();
        if (_exitDoorGameObject != null)
            Destroy(_exitDoorGameObject);
        _exitDoorGameObject = Instantiate(_exitDoorPrefab, Vector3.zero, Quaternion.identity, transform);

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
