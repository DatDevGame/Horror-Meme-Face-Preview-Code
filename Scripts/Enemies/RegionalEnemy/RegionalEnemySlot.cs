using _SDK.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Assets._SDK.Inventory.Interfaces;
using Assets._SDK.Entities;
using System;
using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.Enemies;
using DG.Tweening;

public class RegionalEnemySlot
{
	public GameObject Enemy;
	GameObject EnemyPosition;
	GameObject UITutiroal;
	SkinSlot skinSlot;
	public SkinSlot SkinSlot => skinSlot; 

	RegionalEnemySkillSystem RESkillSystem;
	public RegionalEnemyObservable REObservable => RESkillSystem.RegionalEnemyObservable;
	public RegionalEnemySlot(GameObject item)
    {
		EnemyPosition = item;
	}

    public void AddSkinIfNull(Skin skin)
    {
		if(skinSlot == null)
			skinSlot = new SkinSlot(skin);
		skinSlot.AttachTo(Enemy);

		RESkillSystem.RegionalEnemyObservable.Skin = skin;

		Enemy.name = $"{skin.Model.name}-Regional-{Enemy.name}";
	}
	public void AddDirectionTutorial(Mission mission, GameObject runner)
	{
		//GameObject gameObject = new GameObject();
		AddBehaviorIfNull(mission.TutorialGameObject, EnemyLevel.Noob, RegionalEnemyType.RE0);
		AddSkinIfNull(mission.EnemyGroups[0].EnemySettings.skin);
		//AddTutorialUI(mission.TutorialUIEnemy);
        Enemy.gameObject.transform.position = runner.transform.position + runner.transform.forward * 6;
		Enemy.gameObject.transform.position += new Vector3(0, -0.5f, 0);
		Enemy.gameObject.transform.LookAt(runner.transform);

		SkinSlot.Model?.GetComponentInChildren<Animator>()?.SetBool("isSleep", true);
	}

	public void DisableLineDirectionTutorial()
	{
		LineRenderer lineTutorial = Enemy.GetComponent<LineRenderer>();
		if (lineTutorial)
			lineTutorial.enabled = false;
		UITutiroal?.SetActive(false);
	}

	public void AddBehaviorIfNull(GameObject toGameObject, EnemyLevel enemyLevel, RegionalEnemyType REtype = RegionalEnemyType.RE2)
    {
		Enemy = UnityEngine.Object.Instantiate(toGameObject, toGameObject.transform.position, Quaternion.identity, EnemyPosition.transform);
        Enemy.transform.localScale = new Vector3(1,1,1) *1.5f;

		RESkillSystem = Enemy.GetComponent<RegionalEnemySkillSystem>();
		if (RESkillSystem == null)
        {
			RESkillSystem = Enemy.AddComponent<RegionalEnemySkillSystem>();
		}

		if (Enemy.GetComponent<Rigidbody>() == null)
		{
			var rb = Enemy.AddComponent<Rigidbody>();
			rb.useGravity = false;
		}

		if (Enemy.GetComponent<AudioSource>() == null)
			Enemy.AddComponent<AudioSource>();


		if (Enemy.GetComponent<NavMeshAgent>() == null)
		{
			Enemy.AddComponent<NavMeshAgent>();
			Enemy.GetComponent<NavMeshAgent>().baseOffset = 1;
		}
		RESkillSystem.Init(enemyLevel, REtype);
	}

	public void AddTutorialUI(GameObject TutorialUI)
	{
		UITutiroal = UnityEngine.Object.Instantiate(TutorialUI, Vector3.zero, Quaternion.identity, Enemy.transform);
		UITutiroal.transform.localPosition = Vector3.zero;
	}
}
