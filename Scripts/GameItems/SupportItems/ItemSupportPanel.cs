using Assets._GAME.Scripts.GamePlay;
using Assets._SDK.Analytics;
using Assets._SDK.GamePlay;
using Assets._SDK.Logger;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets._GAME.Scripts.Skills.Live;
using Assets._GAME.Scripts.Skills.Move;
using System.Collections;
using Assets._SDK.Ads;

public class ItemSupportPanel : MonoBehaviour
{
    [SerializeField] private Button _takeItemButton;
    [SerializeField] private Button _tryItemButton;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _description;

    [SerializeField] private Sprite[] _itemIcon;

    private ItemSupportSkillUpgrade _itemSupportSkillUpgrade;

	bool NoAds;

	private void Start()
	{
		if (NoAds)
			StartCoroutine(EnableButtonsDelay(2.5f));
	}

	private IEnumerator UpgradeSkill()
	{
		Debug.Log("SHOW RV HERE");
		yield return new WaitForEndOfFrame();
		//TODO: SHOW RV Her
		AdsManager.Instance.ShowRewarded(isSuccess =>
		{
			if (isSuccess == AdsResult.Success)
			{
				GamePlayLoader.Instance.CurrentGamePlay.Fire(Assets._SDK.GamePlay.GamePlayTrigger.Play);
				_itemSupportSkillUpgrade.GetItemSupportFromPanel();
			}
		}, GamePlayLoader.Instance.PlayingMission.Order, _itemSupportSkillUpgrade.Placement);
		
    }

	private IEnumerator UpgradeSkillNoAds()
	{
		yield return new WaitForEndOfFrame();

		GamePlayLoader.Instance.CurrentGamePlay.Fire(Assets._SDK.GamePlay.GamePlayTrigger.Play);
		_itemSupportSkillUpgrade.GetItemSupportFromPanel();
	}
	public void SetData(ItemSupportSkillUpgrade item)
    {
        _itemSupportSkillUpgrade = item;
        if(item.AvatarSkill)
            _icon.sprite = item.AvatarSkill;
		_description.text = string.Format("{0} {1}", "+", item.GetValueUpgrade());
        _takeItemButton.onClick.AddListener(() => StartCoroutine(UpgradeSkill()));
		NoAds = false;
	}
	public void SetDataHasAds(ItemSupportSkillUpgrade item)
	{
		_itemSupportSkillUpgrade = item;
		if (item.AvatarSkill)
			_icon.sprite = item.AvatarSkill;
		_takeItemButton.GetComponent<Image>().sprite = _itemIcon[1];
		_description.text = string.Format("{0} {1}", "+", item.GetValueUpgrade());
		_takeItemButton.onClick.AddListener(() => StartCoroutine(UpgradeSkill()));
	}

	public void SetDataNoAds(ItemSupportSkillUpgrade item)
	{
		_itemSupportSkillUpgrade = item;
		if (item.AvatarSkill)
			_icon.sprite = item.AvatarSkill;
        _icon.transform.localScale = Vector3.one * 0.7f;
		_takeItemButton.GetComponent<Image>().sprite = _itemIcon[0];
		_takeItemButton.transform.localScale = Vector3.one * 0.7f;
		_description.text = string.Format("{0} {1}", "+", item.GetValueUpgrade());
		_takeItemButton.onClick.AddListener(() => StartCoroutine(UpgradeSkillNoAds()));
		NoAds = true;
	}
	IEnumerator EnableButtonsDelay(float time)
	{
		_takeItemButton.interactable = false;
		yield return new WaitForSecondsRealtime(time);
		_takeItemButton.interactable = true;
	}
}
