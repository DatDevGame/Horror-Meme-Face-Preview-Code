using _SDK.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets._SDK.Inventory.Interfaces;
using Assets._SDK.Entities;
using System;
using _GAME.Scripts.Inventory;
using Assets._GAME.Scripts.Enemies;

public class GlobalEnemySlot
{
    GameObject EnemyPosition;
	public GameObject Enemy;
	SkinSlot skinSlot;
	GlobalEnemySkillSystem GESkillSystem;

	public GlobalEnemySlot(GameObject item)
    {
		EnemyPosition = item;
	}

    public void AddSkinIfNull(Skin skin)
    {
		if(skinSlot == null)
			skinSlot = new SkinSlot(skin);
		skinSlot.AttachTo(Enemy);

		GESkillSystem.GEObservable.Skin = skin;

		Enemy.name = $"{skin.Model.name}-Global-{Enemy.name}";

		OutLineRender(Enemy);
	}

    public void AddBehaviorIfNull(GameObject toGameObject, EnemyLevel enemyLevel)
    {
		Enemy = UnityEngine.Object.Instantiate(toGameObject, toGameObject.transform.position, Quaternion.identity, EnemyPosition.transform);
	
		Enemy.transform.localScale = new Vector3(1, 1, 1) * 2f;

		GESkillSystem = Enemy.GetComponent<GlobalEnemySkillSystem>();
		if (GESkillSystem == null)
			GESkillSystem = Enemy.AddComponent<GlobalEnemySkillSystem>();

		if (Enemy.GetComponent<Rigidbody>() == null)
		{
			var rb = Enemy.AddComponent<Rigidbody>();
			rb.useGravity = false;
		}

		GESkillSystem.Init(enemyLevel);
	}

	private void OutLineRender(GameObject toGameObject)
	{
		var outLine = toGameObject.GetComponent<Outline>();
		if (outLine == null)
		{
			outLine = toGameObject.AddComponent<Outline>();
		}
		outLine.OutlineMode = Outline.Mode.OutlineVisible;
		outLine.OutlineColor = Color.red;
		outLine.OutlineWidth = 5f;
	}
}