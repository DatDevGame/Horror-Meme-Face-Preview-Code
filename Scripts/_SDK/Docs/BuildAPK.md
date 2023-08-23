# Thiết lập môi trường để Build APK file

## Setup Build Environment

### Downloads

- Android Studio

Link: https://developer.android.com/studio
Mở Android Studio lên vào File >> Setting >> System Setting >> Android SDK
SDK Platforms chọn tải Android 9, 10, 11, 12
SDK Tool chọn tải 30.0.2,   30.0.3,   31.0.0

<img width="300" alt="image" src="https://user-images.githubusercontent.com/1218572/203484803-7ff6817e-aeba-4662-b263-fea4774d83b2.png">

- NDK

Link: https://drive.google.com/drive/folders/15-VG28C9Tn5mpL4PTF-X_4w8AVOd8tTa
Chỉ download file RAR extract ra một folder bất kì.

### Sử dụng JDK, SDK, NDK cho Unity Build

vào Unity >> Edit >>  Preferences >> External Tools

<img width="300" alt="image" src="https://user-images.githubusercontent.com/1218572/203484919-8f891a84-c4a3-444f-b6bf-5287548ffdb3.png">

- Uncheck như trong hình
- Riêng gradle sẽ sử dụng của Unity không cần Uncheck
- JDK >> Browse vào đường dẫn hồi lúc tải Java
- SDK >> Browse >> Mở Android Studio vào File >> Setting >> System Setting >> Android SDK >> Copy cái đường dẫn bỏ vào SDK
- NDK >> Browse >> trỏ vào đường dẫn vào NDK đã tải

### Custom Gradle

Mở Unity >> Edit >> Project Setting >> Player >> Publishing Setting

<img width="300" alt="image" src="https://user-images.githubusercontent.com/1218572/203485358-d3c50ddb-bb7a-475c-a22f-1c4f9eded2f5.png">

### Thiết lập API Level

<img width="300" alt="image" src="https://user-images.githubusercontent.com/1218572/203485633-f1bbb254-d033-4bd5-ac68-ef0df929ac84.png">

### Run Android Resolver : Assets ->External Dependency Manager -> Android Resolver -> Force Resolver. 
<img width="413" alt="image" src="https://user-images.githubusercontent.com/117144985/200237572-22b2c62c-b1c2-4fb1-ac59-82267a045c83.png">

  + NOTE: Đợi đến khi chạy xong, không sử dụng đến Gradle trong quá trình này VD: build project khác , Android Resolver project khác....Lý do vì Java Env share chung một gradle cache.
  
## Check Project Versions

NOTE: Rất quan trọng Chỉ được dùng version trong được cung cấp trong link download phía dưới
- [Max SDK](https://nextcloud.rocketstudio.com.vn/s/mWtGftaDDBKgxXC) : 5.5.7
- Firebase : 9.1.0
    [FirebaseAnalytics](https://nextcloud.rocketstudio.com.vn/s/APsdCLd5ZWtNMTQ),
    [FirebaseCrashlytics](https://nextcloud.rocketstudio.com.vn/s/Dff8pCfKrRRzagz),
    [FirebaseRemoteConfig](https://nextcloud.rocketstudio.com.vn/s/EFpZRzyLXs462pX).
- [AppsFlyer](https://nextcloud.rocketstudio.com.vn/s/QTA9PdDoNCWczpC) : 6.8.1


### Reset Unity:

Khi hoàn thành xong hết save xong tắt unity và mở lại tiến hành build.

Note: Có thể phải mở Unity bằng quyền Admin nếu các file SDK, JDK, NDK để ở ổ C ( mặc định)


## Cách fix các lỗi có thể có khi build.
- Lỗi Gradle của Applopvin :
  + Setup version các [Mediation](https://nextcloud.rocketstudio.com.vn/s/pmGJQLHjfcyHJAn) của Applopvin tương ứng bản cũ (Vì version của các Mediation có thể đã upgrade lên gây ra lỗi).Cụ thể GoogleAdManager dễ bị lỗi để check các bạn có thể thử remove -> build hoặc download [GoogleAdManager](https://nextcloud.rocketstudio.com.vn/s/jp8ejgnPt7oDYYm) version cũ (21.1.0.3)-> build.
 
