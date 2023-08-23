# References
1. [Unity Scripting Reference](https://docs.unity3d.com/ScriptReference)
2. [Microsoft’s C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
3. [This Page](https://myunity.dev/coding-guideline-for-unity-c)

Cấu trúc Folder
----------------

Tìm hiểu cấu trúc của SDK Folder trước khi lập trình _GAME Folder:

```
Assets
  _GAME            Game 
    Scripts
    Scenes
  _SDK             SDK 
```

Tên File
-----------

Luôn dùng Pascal Case để đặt tên file ngoại trừ images:

-   Folders --- PascalCase\
    `FolderName`/
-   Images --- hyphen-\
    `image-name-64x64.jpg`
-   The rest --- PascalCase\
    `ScriptName.cs`, `PrefabName.prefab`, `SceneName.unity`

Tên Script
-------------

Tên của Script giống tên class:

-   `XxxPanel`, `XxxSlot`, `XxxButton`, dùng Postfix cho UI Elements
    `MenuPanel`, `AchievementSlot`, `CoinShopButton`
-   `XxxManager`, Script kiểm soát riêng một Workflow nào đó của 1 scene - thường là Singleton
    `GameManager`, `DailyMissionManager`, `AchievementManager`
-   `XxxController`, Script để điều khiển một game object nào đó, nên đổi tên nếu có >=2 scripts
    `PlayerController`, `BossController`, `BackgroundControler`
-   `XxxDatabase`, Cho một file DB (e.g CSV) thường bao gồm 1 list các row
    `WeaponDatabase`, `CardDatabase`
-   `XxxData`, Để đọc 1 Row trong DB
    `WeaponData`, `CardData`
-   `XxxItem`, Cho in-game item
    `CardItem`, `CharacterItem`
-   `XxxGenerator`, Scripts để tạo 1 GameObjects
    `ObjectGenerator`, `LandGenerator`
-   `XxxSettings`, Settings scripts được tạo từ `[ScriptableObject](https://docs.unity3d.com/Manual/class-ScriptableObject.html)` class\
    `AchievementSettings`, `DailyLoginSettings`
-   `XxxEditor`, Cho editor-only scripts inherent Unity's `[Editor](https://docs.unity3d.com/ScriptReference/Editor.html)` class\
    `TutorialTaskEditor`, `AchievementSettingsEditor`

> *Sự khác nhau giữa *`*Manager*`* và *`*Controller*`* là *`*Manager*`* nên là singleton, và nó control một game logic nhất định mà có thể bao gồm nhiều objects hoặc assets, trong khi *`*Controller*`* điều khiển 1 object và có thể có nhiều instances. Ví dụ, có nhiều EnemyController trong 1 scence, mỗi script điều khiển 1 enemy.*
>
> *Sự khác nhau của *`*Data*`*và*`*Item*`* là *`*Item*`* là một object trong game, và trong hầu hết các trường hợp *`*Item*`* chứa *`*Data*`*. Ví dụ *`*CardData*`* bao tất cả các thuộc tính của 1 card, trong khi *`*CardItem*`* bao gồm *`*CardData*`* và các thuộc tính thêm vào cho các player khác nhau ví dụ như card level. *`*Data*`* là Value Object và *`*Item*`* là Entity*

Tên Biến
---------------

Giống như tên file, tên biến cho người đọc biết biến đó dùng làm gì mà không cần phải đọc code:
Không viết tắt tên biến

-   Dùng camcelCase, luôn là danh từ hoặc (is/has) + Adjective\
    `hasRewarded`, `currentPosition`, `isInitialized`
-   Prefix với 1 dấu gạch chân (underscore) cho biến private và protected\
    `_itemCount`, `_controller`, `_titleText`
-   Const: UPPER CASE\
    `SHOW_SUB_MENU_LEVEL`, `MAX_HP_COUNT`, `BASE_DAMAGE`
-   Dùng UPPER CASE (PREFS_XXXX) cho PlayerPref keys\
    `private const string PREFS_LAST_FREE_DRAW = "LastFreeDraw";`\
    `PlayerPrefs.GetInt(PREFS_LAST_FREE_DRAW)`
-   Dùng`xxxGameObject`cho scene biến GameObject\
    `optionButtonGameObject`, `backgroundMaskGameObject`
-   Dùng `xxxPrefab`cho biến scene GameObject\
    `weaponSlotPrefab`, `explosionPrefab`
-   Dùng `xxxTransform` cho biến Transform\
    `weaponTransform`, `armTransform`
-   Dùng `xxxComponent` cho tất cả các component khác\
    `eyesSpriteRenderer`, `runAnimation`, `attckAnimationClip``, `victoryAudioClip`
-   Dùng xxxxs cho arrays\
    `slotPrefabs = new Prefabs[0]`\
    `achievementIds = new int [0]`
-   Dùng xxxxList cho List and xxxxDictionary cho Dictionary\
    `weaponTransformList = new List()`\
    `achievementProgressDictionary = new Dictionary()`
-   Dùng nounVerbed cho callback event\
    `public static event UnityAction gameStarted`\
    `public static UnityAction characterDied`
-   Chỉ dùng khai báo var trong function

>
> *Callback event nên theo Unity's event naming convention, e.g. *`[*SceneManager.activeSceneChanged*](https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-activeSceneChanged.html)`*.*

Tên Function
---------------

-   Dùng PascalCase, bắt đầu với động từ và tiếp theo là danh từ\
    `Reward()`, `StartGame()`, `MoveToCenter()`
-   Dùng `OnXxxxClick`, cho UI button clicks\
    `OnStartClick()`, `OnCancelClick()`
-   Dùng `OnNounVerbed`, for callbacks\
    `OnChestOpened()`, `OnBuyWeaponConfirmed()`

Tên Properties
-----------------

Nếu function không có Input thì nên dùng get/set property:\
`bool IsRewarded() { }`\
nên là\
`bool IsRewarded {get { return ...} }`\
Hoặc thậm chí ngắn hơn\
`bool IsRewarded => ...;`

Có thể sử dụng private phía trong để kiểm soát sự truy cập:\
`public int Id { get; private set; }`\
`public Sprite IconSprite { get; private set; }`

Thứ tự của Biến / Functions
---------------------------

Có một thứ tự nhất định sẽ tiết kiệm thời gian tìm kiểm một biến hay functions nào đó:

```
MyClass: MonoBehavior {
  // Constant variables
  public const int CONST_1;
  private const string CONST_2;

  // Static variables
  public static int static1;
  private static string _static2;

  // Editor-assigned variables
  [SerializeField] public Image maskImage;
  [SerializeField] private MyClass _anotherComponent;

  // Other varaibles
  public int primitiveVariables1;
  private string _primitiveVariable2;

  // Properties
  public int PropertyName
  {
    get;
    private set;
  }

  // Unity functions
  Awake()
  OnEnable()
  Start()
  Update()
  FixedUpdate()

  // Other custom functions
  Init()
    ...
  Reset()

  // Unity functions
  OnDisable()
  OnDestroy()

  #region UNITY_EDITOR
    // Debug functions that only runs in Editor
    [ContextMenu("...")]
  private void DebugFunction1()[MenuItem("Debug/...")]
  private static void DebugFunction2()
  #endregion
}
```

Có thể nhóm một số biến hoặc functions có cùng một mục đích vào một region nhưng nên nghĩ đến việc tạo 1 class khác nếu có thể:

```
#region Timer
private const int RESET_SECONDS = 180;
private float _secondsLeft;
public float canResetTime => _secondsLeft < 0;
public void ResetTime() {
  _secondsLeft = RESET_SECONDS;
}
#endregion Timer
  ... // Other regions
```

Single-line conditional statements
-------------------------------

```
if (1 == 1)
{
  DoSomethingFancy();
}
if (1 == 1) 
{
    return true;
}

if (1 == 1) 
{
  FirstThing();
  LastThing();
}
```

Comments
--------

Có một space sau dấu // khi comment:\
`// Notice the space after the two slashes`\

Comment code thông cần space:\
`//noSpace.Code();`\

`TODO` comments\
`// TODO: Notice the space after the slashes and the 2 spaces after the colon of todo`
