# User Interface

## Architecture

- Khi GameManager load một Scene, ta sẽ tạo ra một SceneUI object để quản lý Scene đó. [Ví dụ](https://github.com/rocketsaigon/RocketSgSdk/blob/4dd0461ba036bf2fe31e723ae465bcf5685d0ccb/Assets/_GAME/Scripts/Game/GameManager.cs#L45)

```
        var loader = LoadSceneAsync("Game");

        if (loader != null)
        {
            yield return new WaitUntil(() => loader.isDone);
        }


        GameSceneUI = new GameSceneUI(SCENE_UI_NAME);
```
- Mỗi SceneUI sẽ bao gồm nhiều Panel, Mỗi Panel triển khai một AbstractPanel là một MonoBehavior để quản lý Panel đó

![image](https://user-images.githubusercontent.com/1218572/208564922-a37b992d-0333-4606-8004-f0068b97ceee.png)

```
@startuml
component [Scene] {
 [SceneUI] as sui
 [Panel] as panel
 [AbstractPanel] as apanel #Yellow
 sui --> panel: has many
 panel --> apanel  : is
}

sui --> [GameManager] : Subscribe State

@enduml
```
- SceneUI nên sử dụng UniRx để Subscribe GameState hoặc GamePlayState để hiện thị Panel tương ứng với State đó

```
            GameManager.Instance.CurrentState.Where((value) => value == GameState.LobbyHome)
                .Subscribe(_ => ShowPanel(nameof(LobbyHomePanel))).AddTo(GameObject);
            GameManager.Instance.CurrentState.Where((value) => value != GameState.LobbyHome)
                .Subscribe(_ => HidePanel(nameof(LobbyHomePanel))).AddTo(GameObject);

            GameManager.Instance.CurrentState.Where((value) => value == GameState.Shopping)
                .Subscribe(_ => ShowPanel(nameof(ShopPanel))).AddTo(GameObject);
            GameManager.Instance.CurrentState.Where((value) => value != GameState.Shopping)
                .Subscribe(_ => HidePanel(nameof(ShopPanel))).AddTo(GameObject);
```
