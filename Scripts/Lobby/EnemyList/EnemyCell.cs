using UnityEngine;
using TMPro;
using _GAME.Scripts.Inventory;
using UnityEngine.UI;
using _SDK.Entities;
using Assets._SDK.Analytics;
using Assets._SDK.Logger;
using Assets._SDK.Ads;

public class EnemyCell : MonoBehaviour
{
    [SerializeField] private Image _enemyAvatar;

    [SerializeField] private Button _playMissionButton;
    [SerializeField] private Image _adsIcon;
    [SerializeField] private Image _overlay;
    private Mission _mission;

    private void PlayEnemyMission()
    {
        AdsManager.Instance.ShowRewarded(isSuccess =>
		{
			if (isSuccess == AdsResult.Success)
			{
				GameManager.Instance.MissionInventory.SetPlayingMissionFromEnemyList(_mission);
				GameManager.Instance.Fire(Assets._SDK.Game.GameTrigger.Play);
			}
		}, 0, AnalyticParamKey.SPECIAL_NEXTBOT_MISSION);
    }

    public void SetData(Skin enemySkin)

    {
		_enemyAvatar.sprite = enemySkin.Image;
        _enemyAvatar.color = enemySkin.IsOwned ? new Color32(255, 255, 255, 255) : new Color32(0, 0, 0, 255);

        _mission = enemySkin.DefaultMissionSetting.Mission;
        //_adsIcon.gameObject.SetActive(!enemySkin.IsOwned);

        _playMissionButton.onClick.AddListener(PlayEnemyMission);
    }
}