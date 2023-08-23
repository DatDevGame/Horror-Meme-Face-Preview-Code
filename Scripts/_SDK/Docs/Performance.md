# Performance
## Must Read
- https://docs.microsoft.com/en-us/windows/mixed-reality/develop/unity/performance-recommendations-for-unity
- https://learn.unity.com/tutorial/physics-best-practices


## Size Files
Tổng file build phải nhỏ hơn 150Mb

## Framerate
- Set Framerate = 60

## Textures:
- Texture Max Size = 2048 --> 512 Hero; Max Size 256 - Khac
- Format ASTC = 8x8; 
- Sprite Atlas - https://docs.unity3d.com/Manual/class-SpriteAtlas.html


## Kiểm soát level of details:
- https://docs.unity3d.com/Manual/class-LODGroup.html. Note: Thường apply cho những game có tầm nhìn rộng, nhiều item chi tiết.


## Xử lý ánh sáng:
- Game HyperCasual thường ít xử lý ánh sáng
- Fake shadow - vẽ sprite 2D làm bóng

## Configure Layer Collision Matrix:
- Thiết lập tương tác cho các item trong layer tương ứng.

## Cache References
- Cache lại các giá trị trong Start hoặc Awake để giảm tính toán trong Update:
  - Không sử dụng GetComponent trong Update

```cs
public class ExampleClass : MonoBehaviour
{
    private Camera cam;
    private CustomComponent comp;

    void Start() 
    {
        cam = Camera.main;
        comp = GetComponent<CustomComponent>();
    }

    void Update()
    {
        // Good
        this.transform.position = cam.transform.position + cam.transform.forward * 10.0f;

        // Bad
        this.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 10.0f;

        // Good
        comp.DoSomethingAwesome();

        // Bad
        GetComponent<CustomComponent>().DoSomethingAwesome();
    }
}
```

- Không khai báo mới Quaternion rotation hoặc Vector3 direction trong Update. Init 1 lần và cache lại, lần sau gán value mới.
- Cache lại các graphich items đã render và sử dụng lại nhiều lần https://docs.unity3d.com/Manual/GPUInstancing.html


## Tránh dùng các phép toán tốn performance:
- Không dùng System.LINQ
- Không dùng các Unity API phổ biến:
```cs
    GameObject.SendMessage()
    GameObject.BroadcastMessage()
    UnityEngine.Object.Find()
    UnityEngine.Object.FindWithTag()
    UnityEngine.Object.FindObjectOfType()
    UnityEngine.Object.FindObjectsOfType()
    UnityEngine.Object.FindGameObjectsWithTag()
    UnityEngine.Object.FindGameObjectsWithTag()
```

## Chú ý tối ưu những phần code lặp
Những hàm Unity thường xuyên gọi lại cần phải tối ưu rất cẩn thận vì những đoạn code đó sẽ được gọi theo mỗi giây hoặc mỗi frame.

### Không dùng các empty callbacks. 

Unity phải xử lý mỗi lần chuyển đi chuyển lại giữa unmanaged (code script) và managed code ( UnityEngine code), vì vậy những đoạn code như thế này sẽ giảm performance

```cs
void Update()
{
}
```
### Giảm số Behaviors trong Scene

- Lí do: https://blog.unity.com/technology/1k-update-calls
- Giải pháp: Sử dụng MicroCoroutine cho một nhóm các nhân vật https://github.com/neuecc/UniRx#microcoroutine


### Dùng lại kết quả của các phép tính toán chạy một lần một frame

Những Unity API này khá phổ biến nhưng nếu có thể thì tránh dùng hoặc dùng lại các kết quả cho toàn app  
- Sử dụng 1 Singleton để Raycast cho scene và sau đó sử dụng lại kết quả trong tất cả các component. Sử dụng Raycase cho các LayerMasks khác nhau

```cs
    UnityEngine.Physics.Raycast()
    UnityEngine.Physics.RaycastAll()
```
- Tránh dùng GetComponent trong Update mà nên cache reference trong Start hoặc Awake

- Sử dụng ObjectPooling, khởi tạo toàn bộ các object khi game start sau đó reuse trong quá trình chạy
    UniRx đã cung cấp - [https://github.com/neuecc/UniRx#microcoroutine](https://github.com/neuecc/UniRx#unirxtoolkit)

- Tránh dùng Interface và Virtual Constructs. Nếu ko cần thiết thì ko nên dùng Interface, có trade-off với code cleaning

## Khác

### Physics
- Cách dễ nhất là để improve là giới hạn số lần tính toán / second --> Coroutine
- Các loại colliders trong Unity có performance khác nhau: Sphere < Capsule < Box <<< Mesh (Convex) < Mesh (non-Convex)  [Unity Physics Best Practices](https://learn.unity.com/tutorial/physics-best-practices?uv=4.x)
### Animations
- Disable idle animations bằng cách disabling Animator component. 
- Không set lại animation nếu animation name ko đổi
