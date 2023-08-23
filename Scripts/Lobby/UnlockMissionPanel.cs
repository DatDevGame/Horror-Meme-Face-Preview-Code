using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets._SDK.Game;
using _GAME.Scripts.Inventory;
using _SDK.Shop;
using Assets._SDK.Logger;
using FancyScrollView.LobbyMissionList;
using _SDK.UI;
using Assets._GAME.Scripts.GamePlay;
using Assets._SDK.Analytics;
using Assets._SDK.Ads;

public class UnlockMissionPanel : AbstractShoppingPanel<MissionShop>
{
	#region UI
	[SerializeField] private Button _buyWithCoin;
	[SerializeField] private Button _buyWithAds;
	[SerializeField] private Button _overlay;
	[SerializeField] private TextMeshProUGUI _coinValue;
	#endregion

	#region Parameter
	private int _missionMoneyToUnlock;
	private Mission _missionCurrent;
	#endregion

	void Start()
	{
		_buyWithCoin.onClick.AddListener(BuyWithCoin);
		_buyWithAds.onClick.AddListener(BuyWithAds);
		_overlay.onClick.AddListener(ClosePopupUnlockMission);
	}
	private void BuyWithAds()
	{
		AdsManager.Instance.ShowRewarded(isSuccess =>
		{
			if (isSuccess == AdsResult.Success)
			{
				_missionCurrent.Bought();
				OnBuying(true);
			}
			else OnBuying(false);
		}, 0, AnalyticParamKey.REWARD_MISSION);
	}

	private void BuyWithCoin()
	{
		bool isSuccess = GameManager.Instance.MissionShop.Buy(_missionCurrent);

		OnBuying(isSuccess);
	}

	private void OnBuying(bool isSuccess)
	{
		if (isSuccess)
		{
			OnBuySuccess(_missionCurrent.Id);
		}
		else
		{
			OnBuyFailed(_missionCurrent.Id);
		}
	}

	private void ClosePopupUnlockMission()
	{
		gameObject.SetActive(false);
	}

	public void SetData(Mission mission)
	{
		_missionCurrent = mission;
		_missionMoneyToUnlock = _missionCurrent.MoneyToUnlock;

		UpdateUI();
	}

	private void UpdateUI()
	{
		_coinValue.SetText(_missionMoneyToUnlock.ToString());
	}

	protected override void OnBuyFailed(int itemId)
	{
		DebugPro.BrownBold("Buy failed. You may not have enough coin");
	}

	protected override void OnBuySuccess(int itemId)
	{
		Mission selectedMission = (Mission)GameManager.Instance.MissionInventory.PlayingMission;
		if (selectedMission.PhotoTypeMission != PhotoTypeMission.None)
		{
			//Set Data OnSelect
			var contents = transform.parent.GetComponent<LobbyHomePanel>().ContentListMission;
			MissionCell cell = contents.GetChild(contents.childCount - 1).GetComponent<MissionCell>();
			cell.SetDataItem(selectedMission);
			cell.UpdateContent();

			//Show Take Photo When Mission Have Type Photo
			var galleryPicturePanel = GetComponentInParent<LobbyHomePanel>().GalleryPicturePanel;

			//Show thang vao chup hinh bo qua buoc chon
			//galleryPicturePanel.gameObject.SetActive(true);

			galleryPicturePanel.HandleGalleryPictureStart();
			galleryPicturePanel.TakePictureButton.onClick.Invoke();

			this.gameObject.SetActive(false);
			return;
		}

		GameManager.Instance.Fire(GameTrigger.Play);
	}

	protected override void OnItemSelected(int itemId)
	{

	}

	protected override void OnPageNavigated(int itemId)
	{

	}
}