using _GAME.Scripts.Inventory;
using _SDK.UI;
using Assets._GAME.Scripts.GamePlay;
using Assets._SDK.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using Unity.VisualScripting;

public class CollectEnemyPanel : AbstractPanel
{
    [SerializeField] private Button _collectButton;

    [SerializeField] private Image _enemyAvatar;
    [SerializeField] private TextMeshProUGUI _enemyName;

    private Skin _enemySkin;

    private void Start()
    {
        _collectButton.onClick.AddListener(() => StartCoroutine(Collect()));
    }

    private IEnumerator Collect()
    {
		_enemySkin.Own();

		yield return new WaitForEndOfFrame();

		SeekAndKillGamePlay.Instance.CollectingSkin = null;
		GamePlayLoader.Instance.CurrentGamePlay.Fire(GamePlayTrigger.Play);
		SeekAndKillGamePlay.Instance.ReceiveKilledEnemy(1, _enemySkin);
	}

    public void SetData(Skin skin)
    {
        _enemySkin = skin;
        _enemyAvatar.sprite = skin.Image;
        _enemyName.text = skin.Name;
    }
}
