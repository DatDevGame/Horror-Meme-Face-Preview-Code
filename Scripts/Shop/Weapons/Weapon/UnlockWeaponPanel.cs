using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets._SDK.Game;
using _GAME.Scripts.Inventory;
using _SDK.Shop;
using Assets._SDK.Logger;
using FancyScrollView.LobbyMissionList;
using _SDK.UI;
using Assets._GAME.Scripts.Shop;
using FancyScrollView.WeaponShopList;

public class UnlockWeaponPanel : AbstractShoppingPanel<WeaponShop>
{
    #region UI
    [SerializeField] private Button _buyWithCoin;
    [SerializeField] private Button _buyWithAds;
    [SerializeField] private Button _overlay;
    [SerializeField] private TextMeshProUGUI _coinValue;
    #endregion

    #region Parameter
    private int _missionMoneyToUnlock;
    private Weapon _weaponCurrent;
    #endregion

    void Start()
    {
        _buyWithCoin.onClick.AddListener(BuyWithCoin);
        _buyWithAds.onClick.AddListener(BuyWithAds);
        _overlay.onClick.AddListener(ClosePopupUnlockMission);
    }
    private void BuyWithAds()
    {
		//TODO
		bool isSuccess = GameManager.Instance.MissionShop.Buy(_weaponCurrent);
	}

    private void BuyWithCoin()
    {
        bool isSuccess = GameManager.Instance.MissionShop.Buy(_weaponCurrent);

        if (isSuccess)
        {
            OnBuySuccess(_weaponCurrent.Id);
        }
        else
        {
            OnBuyFailed(_weaponCurrent.Id);
        }
    }
    private void ClosePopupUnlockMission()
    {
        gameObject.SetActive(false);
    }

    public void SetData(Weapon weapon)
    {
        _weaponCurrent = weapon;
        _missionMoneyToUnlock = (int)_weaponCurrent.Price.Value;

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
        //Weapon selectedWeapon = (Weapon)GameManager.Instance.WeaponInventory.PlayingWeapon;
        //if (selectedWeapon.PhotoTypeMission != PhotoTypeMission.None)
        //{
        //    //Set Data OnSelect
        //    var contents = transform.parent.GetComponent<WeaponListUI>().content;
        //    ItemCell cell = contents.GetChild(contents.childCount - 1).GetComponent<ItemCell>();
        //    cell.SetDataItem(selectedWeapon);
        //    cell.UpdateContent();

        //    //Show Take Photo When Mission Have Type Photo
        //    var galleryPicturePanel = GetComponentInParent<LobbyHomePanel>().GalleryPicturePanel;
        //    galleryPicturePanel.gameObject.SetActive(true);
        //    this.gameObject.SetActive(false);
        //    return;
        //}

        //GameManager.Instance.Fire(GameTrigger.Play);
    }

    protected override void OnItemSelected(int itemId)
    {

    }

    protected override void OnPageNavigated(int itemId)
    {

    }
}