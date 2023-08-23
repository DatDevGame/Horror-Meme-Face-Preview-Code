using System;
using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts.Inventory;
using _GAME.Scripts.Lobby;
using _SDK.Money;
using _SDK.UI;
using Assets._GAME.Scripts.Game;
using Assets._GAME.Scripts.Shop;
using Assets._SDK.Ads;
using Assets._SDK.Game;
using Assets._SDK.Skills;
using Castle.Components.DictionaryAdapter.Xml;
using Stateless;
using Stateless.Graph;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class GameManager : AbstractGameManager
{
    public new static GameManager Instance => (GameManager)AbstractGameManager.Instance;

    public const string SCENE_UI_NAME = "SceneUI";

	public MissionInventory MissionInventory { get; private set; }
	public WeaponInventory WeaponInventory { get; private set; }

	public SkinInventory SkinInventory { get; private set; }

	public GameResources Resources { get; private set; }

    public WeaponShop WeaponShop { get; private set; }
    public MissionShop MissionShop { get; private set; }
	public SoundManager SoundManager { get; private set; }

    public MissionSettingDriverPanel DriverToolTest;

    public LoadingPanel LoadingPanel;

    private AsyncOperation _asyncOperationGame;
	protected override void OnAwake()
    {
        base.OnAwake();
        Resources = gameObject.GetComponent<GameResources>();

        StateMachine.Configure(GameState.LobbyHome)
            .OnEntry(() => StartCoroutine(LoadSceneLobby()));

        StateMachine.Configure(GameState.Playing)
            .OnEntry(() => StartCoroutine(LoadGameScene()));

        StartCoroutine(LoadSceneLobby());

        StateMachine.Activate();
        SoundManager = gameObject.AddComponent<SoundManager>();
	}

    private IEnumerator LoadGameScene()
    {
        WeaponInventory ??= new WeaponInventory();
        WeaponInventory.Load();
        Wallet ??= new Wallet();

        var loader = LoadSceneAsync("Game");
        _asyncOperationGame = loader;
        loader.allowSceneActivation = false;
		float progress = 0;
		while (!loader.isDone)
        {
            progress = Mathf.MoveTowards(progress, loader.progress, Time.deltaTime);
			LoadingPanel?.SetProgressUI(progress);

			if (progress >= 0.9f)
			{
		        LoadingPanel?.SetProgressUI(1f);
			}
			yield return null;
		}
	}
    public void NextGame() => _asyncOperationGame.allowSceneActivation = true;

    IEnumerator LoadSceneLobby()
    {
        MissionInventory ??= new MissionInventory();
		Wallet ??= new Wallet();
        WeaponShop ??= new WeaponShop();
        MissionInventory.LoadAllMissions();

        MissionShop ??= new MissionShop();
        MissionShop.Load();

        var loader = LoadSceneAsync("Lobby");

        if (loader != null)
        {
            yield return new WaitUntil(() => loader.isDone);
        }

		AudioListener.volume = 1;
	}

    public void OnPause() => Time.timeScale = 0f;

    public void OnUnPause() => Time.timeScale = 1f;

    void Start()
    {
        StateMachine.Fire(GameTrigger.InitToLobby);
	}
}