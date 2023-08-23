using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.GamePlay;
using Assets._GAME.Scripts.Skills;
using Assets._GAME.Scripts.Skills.Live;
using Assets._GAME.Scripts.Skills.Move;
using Assets._SDK.Ads;
using Assets._SDK.Analytics;
using Assets._SDK.GamePlay;
using Assets._SDK.Input;
using Assets._SDK.Inventory.Interfaces;
using Assets._SDK.Logger;
using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSupportSkillUpgrade : MonoBehaviour
{
	private const float TIME_DELAY_TO_DESTROY = 5f;

	[SerializeField] private GameObject _adsPrefab;
	[SerializeField] private bool _isAds;

    public AbstractSkillSettings SkillUpGrade;
	public int IndexLevel = 1;
	public TypeItem TypeItems;
	public Sprite AvatarSkill;
	public GameObject modelItem;
	public GameObject EffectHit;

	IGameItemSuport ItemWeaponCurrent;
	IEnumerator _delayDestroy;

	private AudioSource _audioSource;
	private AudioClip _soundEffect;

	private string _placement;
	public string Placement { 
		get
        {
			_placement = "";

			if (SkillUpGrade is LiveRunnerSkillSettings)
			{
				_placement = AnalyticParamKey.REWARD_HP;
			}
			else if (SkillUpGrade is StaminaSkillSettings)
			{
				_placement = AnalyticParamKey.REWARD_STAMINA;
			}
			else if (SkillUpGrade is AttackMeleeSkillSettings)
			{
				_placement = AnalyticParamKey.REWARD_WEAPON_INGAME;
			}

			return _placement;
		}
	}

	public void InitData(IGameItemSuport ItemWeapon)
	{
		ItemWeaponCurrent = ItemWeapon;
		SkillUpGrade = ItemWeapon.SkillUpgrade;
		IndexLevel = ItemWeapon.LevelIndex;
		TypeItems = ItemWeapon.TypeItems;
		AvatarSkill = ItemWeapon.AvatarSkill;

		SetModelAds(_isAds, ItemWeapon.TypeItems);
    }

    private void Awake()
    {
		GetSound();
	}

	private void Start()
    {
        GetComponent<Collider>().enabled = true;
        EffectHit?.SetActive(false);
    }

	private void SetModelAds(bool isAds, TypeItem typeItem)
	{
		if (isAds && typeItem == TypeItem.AttackMelee)
		{
			GameObject adsPrefab = Instantiate(_adsPrefab, Vector3.zero, Quaternion.identity, transform);
			adsPrefab.transform.localPosition = Vector3.up * 1.5f;
			adsPrefab.transform.localScale = Vector3.one * 20;
			adsPrefab.transform.SetParent(modelItem.transform);
        }
	}
	private void GetSound()
    {
		_audioSource = GetComponent<AudioSource>();

		if (_audioSource == null)
			_audioSource = gameObject.AddComponent<AudioSource>();

		switch (TypeItems)
		{
			case TypeItem.Health:
				_soundEffect = GameManager.Instance.SoundManager.GetSoundHealItem();
				break;

			case TypeItem.Stamina:
				_soundEffect = GameManager.Instance.SoundManager.GetSoundStaminaItem();
				break;

			case TypeItem.AttackMelee:
				_soundEffect = GameManager.Instance.SoundManager.GetSoundStaminaItem();
				break;
			default :
				_soundEffect = GameManager.Instance.SoundManager.GetSoundStaminaItem(); 
				break;
		}

        _audioSource.clip = _soundEffect;
	}
	private void PlaySound()
	{
		if (_soundEffect == null) return;
		_audioSource.Play();
	}
	private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.tag == "Runner")
		{
			PlaySound();

			if (!_isAds)
				GetItemSupportTry();
			else
                ShowAdsGetItem();

            GetComponent<Collider>().enabled = false;
            if (EffectHit != null)
                EffectHit.SetActive(true);

            if (_delayDestroy != null)
                StopCoroutine(_delayDestroy);

            _delayDestroy = DestroyObject();
            StartCoroutine(_delayDestroy);
        }
    }

	IEnumerator ShowPopup()
    {
		yield return new WaitForEndOfFrame();
		GamePlayLoader.Instance.GameSceneUI.SkillSupportItemUI.InitByGetItem(this);
		GamePlayLoader.Instance.CurrentGamePlay.Fire(GamePlayTrigger.UpgradeSkill);
	}

	private void GetItemSupportTry()
	{
		RunnerSkillSystem runnerSkillSystem = GamePlayLoader.Instance?.Runner.GetComponent<RunnerSkillSystem>();
		runnerSkillSystem.RunnerObservable.AttachSkill.Invoke(new ItemUpgradeSkillSetting { SkillSeting = SkillUpGrade, Index = IndexLevel });
	}
	private void GetItemSupport()
	{
		RunnerSkillSystem runnerSkillSystem = GamePlayLoader.Instance?.Runner.GetComponent<RunnerSkillSystem>();
		runnerSkillSystem.RunnerObservable.AttachSkill.Invoke(new ItemUpgradeSkillSetting { SkillSeting = SkillUpGrade, Index = IndexLevel });
		
		if(TypeItems == TypeItem.AttackMelee)
			GameManager.Instance.WeaponInventory.SetOwnItemWeapon((AttackMeleeSkillSettings)SkillUpGrade);
	}

	private void GetItemSupportInvisible()
	{
		RunnerSkillSystem runnerSkillSystem = GamePlayLoader.Instance?.Runner.GetComponent<RunnerSkillSystem>();
		runnerSkillSystem.RunnerObservable.InvisibleRunner.Invoke(true);
	}

	private void GetItemSupportScan()
	{
		RunnerSkillSystem runnerSkillSystem = GamePlayLoader.Instance?.Runner.GetComponent<RunnerSkillSystem>();
		runnerSkillSystem.RunnerObservable.ScanAllEnemies.Invoke(true); 
		if (EffectHit != null)
			EffectHit.SetActive(true);
	}

	private void ShowAdsGetItem()
	{
        Debug.Log($"Show Rewarded Ads");
        AdsManager.Instance.ShowRewarded(isSuccess =>
        {
			if (isSuccess == AdsResult.Success)
                GetItem();
        }, GamePlayLoader.Instance.PlayingMission.Order, _placement);
    }
    private void GetItem()
    {
        PlaySound();

        switch (TypeItems)
        {
            case TypeItem.Invisible:
                GetItemSupportInvisible();
                break;
            case TypeItem.Scan:
                GetItemSupportScan();
                break;
            default:
                GetItemSupport();
                break;
        }
    }
    public void GetItemSupportFromPanel()
    {
		PlaySound();

		switch (TypeItems)
		{
			case TypeItem.Invisible:
				GetItemSupportInvisible();
				break;
			case TypeItem.Scan:
				GetItemSupportScan();
				break;
			default:
				GetItemSupport();
				break;
		}
    }
	public void GetItemSupportTryFromPanel()
	{
		PlaySound();
		GetItemSupportTry();
	}

	IEnumerator DestroyObject()
	{
		modelItem.SetActive(false);
		yield return new WaitForSeconds(TIME_DELAY_TO_DESTROY);
		Destroy(this.gameObject);
	}

	public string GetValueUpgrade()
	{
		switch(TypeItems)
		{
			case TypeItem.Health:
				LiveRunnerSkill liveSkill = (LiveRunnerSkill)SkillUpGrade.Skill;
				LiveRunnerSkillLevel liveSkillLevel = (LiveRunnerSkillLevel)liveSkill.GetSkillLevelBy(IndexLevel);
				return liveSkillLevel.HealthPoint.Point.ToString() + " Health";
			case TypeItem.Stamina:
				StaminaSkill staminaSkill = (StaminaSkill)SkillUpGrade.Skill;
				StaminaSkillLevel staminaSkillLevel = (StaminaSkillLevel)staminaSkill.GetSkillLevelBy(IndexLevel);
				return staminaSkillLevel.StaminaPoint.Point.ToString() + " Stamina";
			case TypeItem.AttackMelee:
				AttackMeleeSkill AttackMelleSkill = (AttackMeleeSkill)SkillUpGrade.Skill;
				AttackMeleeSkillLevel AttackSkillLevel = (AttackMeleeSkillLevel)AttackMelleSkill.GetSkillLevelBy(IndexLevel);
				return AttackSkillLevel.DamegePoint.Point.ToString() + " Attack ";
			case TypeItem.Invisible:
				return "Visible";
			case TypeItem.Scan:
				return "Scan All Enemies";
			default:
				return "Not Found Item";
		}
	}

	private void OnDestroy()
	{
		if (_delayDestroy != null)
			StopCoroutine(_delayDestroy);
	}
	public enum TypeItem
	{
		Health,
		Stamina,
		AttackMelee,
		Invisible,
		Scan
	}
}
