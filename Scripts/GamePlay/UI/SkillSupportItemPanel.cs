using _SDK.UI;
using Assets._GAME.Scripts.GamePlay;
using Assets._SDK.GamePlay;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillSupportItemPanel : AbstractPanel
{
	[SerializeField] private Animator anim;

    [SerializeField] private Button _noThanksButton;
    [SerializeField] private Button _tryButton;
    [SerializeField] private Transform _content;
	[SerializeField] private CanvasGroup _delayShowFooter;

    [SerializeField] private List<ItemSupportSkillUpgrade> _defaultItemSupportList;


	[SerializeField] private List<ItemSupportSkillUpgrade> _HPSupportItems;
	[SerializeField] private List<ItemSupportSkillUpgrade> _staminaSupportItems;
	[SerializeField] private ItemSupportPanel _itemSupportPanel;

	private void Awake()
	{
		//_tryButton?.gameObject.SetActive(false);
		// Remove Button TryOneTime
		//_tryButton = null;

		_noThanksButton.onClick.AddListener(NotChosingSupportItem);
	}
	private void OnEnable()
	{
		anim.SetTrigger("OnAnim");
		StartCoroutine(EnableButtonsDelay(2.5f));
	}

	IEnumerator EnableButtonsDelay(float time)
	{
		_delayShowFooter.interactable = false;
		_delayShowFooter.alpha = 0f;
		yield return new WaitForSecondsRealtime(time);
		_delayShowFooter.alpha = 1f;
		_delayShowFooter.interactable = true;
	}

	private void NotChosingSupportItem()
    {
        GamePlayLoader.Instance.CurrentGamePlay.Fire(GamePlayTrigger.Play);
    }
    private void OnDisable()
    {
		//anim.enabled = false;
		Clear();
    }

    public void InitByGetItem(ItemSupportSkillUpgrade item)
    {
        if (item == null) return;

		var panel = Instantiate(_itemSupportPanel, _content);

		if (item.TypeItems == ItemSupportSkillUpgrade.TypeItem.AttackMelee 
			//&& SeekAndKillGamePlay.Instance.HasTriedOneTimeGetWeapon == false
		)
		{
			_tryButton?.gameObject.SetActive(true);
			_tryButton?.onClick.AddListener(() => StartCoroutine(UpgradeSkill(item)));
		}
		panel.SetData(item);
    }

    public void InitByMissionMaxLength()
    {
        InstantiateSupportItems(_defaultItemSupportList);
	}

	public void InitHPItemsByMissionMaxLength()
	{
		InstantiateSupportItemsUpgrade(_HPSupportItems);
	}

	public void InitStaminaItemsByMissionMaxLength()
	{
		InstantiateSupportItemsUpgrade(_staminaSupportItems);
	}

	private void InstantiateSupportItems(List<ItemSupportSkillUpgrade> SupportItems)
    {
		Clear();

		foreach (ItemSupportSkillUpgrade itemSupportSkill in SupportItems)
		{
			var panel = Instantiate(_itemSupportPanel, _content);
			panel.SetData(itemSupportSkill);
		}
	}
	private IEnumerator UpgradeSkill(ItemSupportSkillUpgrade itemSupportSkill)
	{
		yield return new WaitForEndOfFrame();
		GamePlayLoader.Instance.CurrentGamePlay.Fire(Assets._SDK.GamePlay.GamePlayTrigger.Play);
		itemSupportSkill.GetItemSupportTryFromPanel();
		SeekAndKillGamePlay.Instance.HasTriedOneTimeGetWeapon = true;
	}
	private void InstantiateSupportItemsUpgrade(List<ItemSupportSkillUpgrade> SupportItems)
	{
		Clear();
		for (int i = 0; i < SupportItems.Count; i++)
		{
			var panel = Instantiate(_itemSupportPanel, _content);
			if (i >= SupportItems.Count / 2)
				panel.SetDataHasAds(SupportItems[i]);
			else
				panel.SetDataNoAds(SupportItems[i]);
		}
	}


	private void Clear()
    {
		anim.SetTrigger("OffAnim");
		_tryButton?.gameObject.SetActive(false);
		_tryButton?.onClick.RemoveAllListeners();
		for (var i = 0; i < _content.transform.childCount; i++)
        {
            Destroy(_content.GetChild(i).gameObject);
        }
    }
}
