using Assets._SDK.Game;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using _GAME.Scripts.Inventory;
using FancyScrollView.LobbyMissionList;
using Assets._SDK.Ads;
using Assets._SDK.Analytics;

namespace _SDK.UI
{
    public class LobbyHomePanel : AbstractPanel
	{
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _enemyButton;
        [SerializeField] private Button _shopButton; 
		[SerializeField] private Button _specialMissionAdsButton;
		[SerializeField] private Button _specialGrimaceMissionAdsButton;
		[SerializeField] private TextMeshProUGUI _playerCoinValue;
		[SerializeField] private MissionListUI _missionListUI;
        [SerializeField] private UnlockMissionPanel UnlockMissionPanel;
        [SerializeField] private GalleryPicturePanel _galleryPicturePanel;

        public GalleryPicturePanel GalleryPicturePanel => _galleryPicturePanel;
        public Transform ContentListMission => _missionListUI.content;
        private void Start()
        {
            _playButton.onClick.AddListener(() => 
            {
                Playing();
            });
            _enemyButton.onClick.AddListener(GoEnemyList);

            _shopButton.onClick.AddListener(() =>
            {
                GameManager.Instance.WeaponShop.Load();
                GameManager.Instance.Fire(GameTrigger.GoShopping);
            });

			_specialMissionAdsButton.onClick.AddListener(() =>
			{
				AdsManager.Instance.ShowRewarded(_ =>
				{
					GameManager.Instance.MissionInventory.SetPlayingSpecialMissionAds();
					_missionListUI?.RefeshUI();

					var selectedMission = (Mission)GameManager.Instance.MissionInventory.PlayingMission;
				    if (selectedMission.PhotoTypeMission != PhotoTypeMission.None && selectedMission.IsOwned)
                    {
					    //Show thang vao chup hinh bo qua buoc chon
						//GalleryPicturePanel.gameObject.SetActive(true);HandleGalleryPictureStart
					    GalleryPicturePanel.HandleGalleryPictureStart();
						GalleryPicturePanel.TakePictureButton.onClick.Invoke();
				    }

				}, GameManager.Instance.MissionInventory.PlayingMission.Order, AnalyticParamKey.SPECIAL_MISSION);
		    });

			_specialGrimaceMissionAdsButton.onClick.AddListener(() =>
			{
				AdsManager.Instance.ShowRewarded(_ =>
				{
					GameManager.Instance.MissionInventory.SetGrimacePlayingSpecialMissionAds();
					_missionListUI?.RefeshUI();

					AbstractGameManager.Instance.Fire(GameTrigger.Play);

				}, GameManager.Instance.MissionInventory.PlayingMission.Order, AnalyticParamKey.SPECIAL_GRIMACE_MISSION);
			});

			GameManager.Instance.Wallet.DefaultAccount.Balance.Subscribe((value) =>
            {
                _playerCoinValue.SetText(value.ToString());
            }).AddTo(this);
        }

        private void Playing()
        {
            Mission selectedMission = (Mission) GameManager.Instance.MissionInventory.PlayingMission;
            if (selectedMission.PhotoTypeMission != PhotoTypeMission.None && selectedMission.IsOwned)
            {
				//Show thang vao chup hinh bo qua buoc chon
				AdsManager.Instance.ShowRewarded(_ =>
				{
					//GalleryPicturePanel.gameObject.SetActive(true);
					GalleryPicturePanel.HandleGalleryPictureStart();
					GalleryPicturePanel.TakePictureButton.onClick.Invoke();

				}, selectedMission.Order, AnalyticParamKey.SPECIAL_MISSION);

				return;
			}

            if (selectedMission.IsOwned)
                AbstractGameManager.Instance.Fire(GameTrigger.Play);
            else
            {
                UnlockMissionPanel.SetData(selectedMission);
                UnlockMissionPanel.gameObject.SetActive(true);
            }
                
        }

        private void GoEnemyList()
        {
            GameManager.Instance.Fire(GameTrigger.GoEnemyList);
        }

    }
}