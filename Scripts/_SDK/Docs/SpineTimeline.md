# Giới thiệu
Spine là một công cụ hoạt hình tập trung đặc biệt vào hoạt hình 2D cho trò chơi. Spine nhằm mục đích có một quy trình làm việc hiệu quả, được sắp xếp hợp lý, cả để tạo hoạt ảnh bằng trình chỉnh sửa và sử dụng các hoạt ảnh đó trong các trò chơi bằng Spine Runtimes.
Các engine sử dụng được spine:

<img width="413" alt="image" src="https://user-images.githubusercontent.com/1218572/196612759-4717e3b1-b48d-4ccd-95b6-e44cf9ed25fe.png">

# Từ ngữ chuyên ngành
- SkeletonDataAsset : loại dữ liệu đã được tổng hợp bởi Spine package từ file export phần mềm Spine.
- Animation Asset Reference : những hành động (animation) của đối tượng Spine (SkeletonDataAsset).
- Track : những hành động trong thời gian, ta có những loại track như : âm thanh. animation.
- Track Group : quản lý những track.
- Playable Director : nơi chạy Timeline
- Bindings : liên kết giữa Timeline và SkeletonDataAsset thông qua Playable Director.
- Timeline : những sự kiện trong một khoảng thời gian đã sắp xếp.

# Cài đặt
Vào trang chủ của Spine để tải xuống spine-unity package hoặc là add bằng link gihub : 
https://github.com/EsotericSoftware/spine-runtimes.git?path=spine-unity/Modules/com.esotericsoftware.spine.timeline#3.8. 

**Lưu ý:**
- Phiên bản sẽ phụ thuộc vào phần mềm Spine chính bên artist làm việc.
- Các phiên bản extension phải theo phiên bản tương ứng của package spine đã tải xuống.
**Ví dụ:** Artist dùng Spine 3.8 nên ta tải package : 
- Spine package : spine-unity 3.8 for Unity 2017.1-2020.3.
- Spine timeline package : spine.timeline 3.8 2021-03-19.

Sau khi tải xuống ta sẽ có : 
- File unity package cho Spine.
- File nén là extension như Timeline, Shader.
**Cài đặt: **
Đối với package Unity thì ta cài đặt như mọi package khác bằng cách open file trong khi mở Unity.
Đổi với file nén : ta vào Unity -> Package Manager -> chọn file file json.

Kết quả :

<img width="452" alt="image" src="https://user-images.githubusercontent.com/1218572/196613186-29d9d41a-5a08-424b-a649-6277c28d9298.png">

<img width="282" alt="image" src="https://user-images.githubusercontent.com/1218572/196613238-4730b854-bbc8-40d4-b3b7-ead59083ac3b.png">

# Tổng quan
## Cơ chế của Spine

Một spine cơ bản nhất sẽ được xuất ra từ phần mềm Spine sẽ bao gồm : 
File text Atlas, chứa các material theo dạng Spine/Skeleton, mỗi material. File Atlas sẽ chứa thông tin các vị trí của từng mảnh vẽ trong một tấm ảnh không có trật tự 

<img width="328" alt="image" src="https://user-images.githubusercontent.com/1218572/196613358-53c527a5-f56d-436b-846f-f0bce0f2025a.png">

File Json : là nơi apply Atlas vào và nó chứa các thông tin về animation sẽ hoạt động ra sao. Kế tiếp, khi ta để file json vào Unity thì nó sẽ tự sinh ra một file Skeleton Data và nó sẽ mapping tương ứng các giá trị trong đó. Hơn nữa, chúng ta cũng có thể tùy chỉnh Scale cho Spine trong chỗ này được.

<img width="340" alt="image" src="https://user-images.githubusercontent.com/1218572/196613431-8f601200-6522-4adf-b427-a66e56520b95.png">

## Spine Reference Assets

Để có thể sử dụng được các đoạn animation nhỏ trong Unity (Animation Reference Asset) ta sẽ chọn vào SkeletonData, kéo xuống gần cuối và chọn vào “Create Animation Reference Assets” như ảnh sau : 

<img width="327" alt="image" src="https://user-images.githubusercontent.com/1218572/196613524-01a22c7b-eca3-4d5f-9aa9-44cd901b3044.png">

Sau đó ta sẽ được một folder Reference Assets cùng cấp với SkeletonData.

## Animation Reference Asset

Là những đoạn animation của mỗi hoạt ảnh trong SkeletonDataAsset như : idle, run ,etc . Ta có thể thay đổi các giá trị trong đó mỗi khi update SkeletonDataAsset mà không phải “Create Animation Reference Assets” ra nữa.

<img width="317" alt="image" src="https://user-images.githubusercontent.com/1218572/196613625-3e3f670b-820a-45ef-859e-7869643a85c1.png">

# Sử dụng

## Spine
Ta chỉ cần kéo SkeletonDataAsset vào Scene thì nó sẽ cho chúng ta ba tùy chọn 

<img width="249" alt="image" src="https://user-images.githubusercontent.com/1218572/196613729-a7319c63-758c-431a-ac9a-b70b6e995193.png">

- Skeleton Animation : là một GameObject dạng Skeleton.
- Skeleton Graphic : dùng cho UI.
- Skeleton Mecanim : thay vì sử dụng animator của Unity.

Với mỗi Skeleton Animation ta có thể tùy chỉnh Skin, Animation name và những thứ khác.

<img width="296" alt="image" src="https://user-images.githubusercontent.com/1218572/196613822-4113a3c9-82d8-467b-baab-df3f788b72a7.png">

## Timeline (Skeleton Animation - Graphic)

Mở Timeline trong unity bằng cách : vào Window (trong Unity) -> Sequencing -> Timeline hoặc double click vào timeline đó ở trong project.

Mỗi Timeline sẽ chứa các đoạn thông tin của animation để gửi tới đối tượng (Binding) đó và play animation của nó.

<img width="475" alt="image" src="https://user-images.githubusercontent.com/1218572/196614072-fd070dbc-7c8e-432b-8ef1-e820316cb20e.png">

- Lấy những Reference asset bỏ vào trong từng track và add liên kết - binding vào track đó để có thể play animation bằng 2 cách sau : 

<img width="533" alt="image" src="https://user-images.githubusercontent.com/1218572/196614136-00924cac-21a1-438a-87e6-53c5dc206826.png">

Add animations on overlay tracks : là việc ta thêm một hành động, animation khác vào trong lúc một animation đang chạy.

**Lưu ý : phải thay đổi các track index để tránh bị ghi đè animation.**

<img width="608" alt="image" src="https://user-images.githubusercontent.com/1218572/196614257-05ba2a0b-46ae-4c4d-be87-f5278d63c569.png">

Từ đó ta có thể tùy biến về track index để xảy ra nhiều trường hợp bất ngờ thông qua hiệu ứng ghi đè của animation.

## Các công cụ của Timeline : 

### Mix Mode

Để thêm, định vị và cắt các clip mà không cần di chuyển hoặc thay thế các clip liền kề. Chế độ trộn tạo ra sự pha trộn giữa các clip giao nhau.
Dùng khi trạng thái hành động Idle tới trạng thái hành động chạy ma ta muốn nó chuyển trạng thái mượt mà thì ta sẽ dùng cái mode này.

<img width="468" alt="image" src="https://user-images.githubusercontent.com/1218572/196614435-ac1e1eac-5770-4565-b5d2-51d74ddcdfcd.png">

Đoạn hình chữ nhật có đường xéo là mix animation.

### Ripple Mode

Để thêm, định vị và cắt một clip trong khi ảnh hưởng đến các clip tiếp theo trên cùng một bản nhạc. Định vị hoặc cắt các clip ở chế độ Ripple giữ nguyên khoảng cách giữa các clip tiếp theo.
Dùng khi ta muốn di chuyển những clip phía sau nó mà không muốn thay đổi vị trí của mix clip .

### Replace Mode

Chế độ thay thế để thêm, định vị và cắt một clip trong khi cắt hoặc thay thế các clip giao nhau.
Dùng khi ta muốn sử dụng một đoạn nhỏ của clip nào đó hay muốn chèn giữa và không dùng cái clip đó.

<img width="329" alt="image" src="https://user-images.githubusercontent.com/1218572/196614668-31747ab5-a7b2-411b-8980-7151376f1957.png">

## Animator ( Skeleton Mecanim )

Bằng cách ta chọn Skeleton Mecanim lúc khởi tạo đối tượng thì ta sẽ có một GameObject mang trong mình Skeleton Mecanim và Animator của Unity

<img width="488" alt="image" src="https://user-images.githubusercontent.com/1218572/196614799-fb847792-26fd-4bfd-b0ad-d3a3398182c9.png">

Với cách này chúng ta có thể tạo ra các liên kết animation trong animator.

# Demo

## Object Oriented Spine Sample

Tham khảo trong Getting Started của Spine Examples.

**SpineboyBeginnerInput.cs**

> Input ( keyboard hay joystick).
>
> Gọi SpineboyBeginnerModel.cs để chuyển đổi trạng thái : move, shoot, jump.

```
public string attackButton = "Fire1";
public SpineboyBeginnerModel model;

void Update () {
    if (model == null) return;

    float currentHorizontal = Input.GetAxisRaw(horizontalAxis);
    model.TryMove(currentHorizontal);

    if (Input.GetButton(attackButton))
        model.TryShoot();
    if (Input.GetButtonDown(jumpButton))
        model.TryJump();
}

```
**SpineboyBeginnerModel.cs**

> Di chuyển hoạt ảnh của nhận vật : quay trái, phải; tốc độ; nhảy lên (di chuyển game object lên).

```
public float currentSpeed;
public bool facingLeft;
public event System.Action ShootEvent; 

public void TryShoot () {
    float currentTime = Time.time;

    if (currentTime - lastShootTime > shootInterval) {
        lastShootTime = currentTime;
        if (ShootEvent != null) ShootEvent();  
    }
}

```
**SpineboyBeginnerView.cs**

> Thay đổi các animation giữa trạng thái.
> Thêm các hiệu ứng, âm thanh vào lúc chạy, bắn.

**SpineboyTargetController.cs**

> Thay đổi vị trí súng của Spine.


## Thay đổi skin

- Đối với GameObject : 
```
public static void SetSkin(this SkeletonAnimation skeletonAnimation,string skin)
{
     skeletonAnimation.initialSkinName = skin;
     skeletonAnimation.Initialize(true);
}

```

- Đối với UI, Graphic : 

```
public static void SetSkin(this SkeletonGraphic skeletonAnimation,string skin)
{
     skeletonAnimation.initialSkinName = skin;
     skeletonAnimation.Initialize(true);
}

```

# Kết luận

- Spine là gì?
- Tại sao chúng ta lại dùng Spine? Nhẹ hơn nhiều loại animation 2D Sprite của Unity và nó mang tính chất đa dạng hơn.
- Ta nên dùng Spine khi nào? Game 2D có những mối liên hệ hoạt ảnh liên quan tới nhau.
- Dùng Spine bằng cách nào?
- Ai sẽ là người tạo Spine? Bên Artist sẽ tạo ra các hình ảnh và 

# Tài liệu tham khảo

Trang chủ Spine, Esoteric Software LLC,  : http://en.esotericsoftware.com/ , 
