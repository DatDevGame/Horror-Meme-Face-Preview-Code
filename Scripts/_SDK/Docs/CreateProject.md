## Environments
- Development Environment: Unity Editor Version 2021.3.6f1.
- **Chú ý các versions** vì nếu sai sẽ tốn rất nhiều thời gian cho build
- **Uncheck folder ExternalDependencyManager** khi import các package vì trong SDK đã giữ phiên bản cao nhất.
![image](https://user-images.githubusercontent.com/117144985/200244712-02ccf339-8c06-4d2e-87ff-8cac30b771ae.png)

## STEP 1 - Tạo Dự Án Mới - Chuyển qua Step 2 nếu Dự án đã có sắn

- Fork RocketSgSdk Repository cho Game mới trên GitHub
- Clone về máy local.
- Cài các thư viện sau:
  + **Max (Applopvin)**: Đã có trong SDK, có thể bỏ qua
    Import Package [Max package](https://nextcloud.rocketstudio.com.vn/s/mWtGftaDDBKgxXC) 
    
    Disable Max: Khi start một game chúng ta chưa có key từ Marketing vì vậy phải disable Max cho đến khi cài đặt Ads.
    
    Các Ads Network (Facebook, Unit Ads, etc) cũng chỉ được install khi có thông tin từ bên MKT, đọc file [Ads.md](https://github.com/rocketsaigon/RocketSgSdk/blob/master/Assets/_SDK/Docs/Ads.md) để biết thêm chi tiết. 
    
    ![image](https://user-images.githubusercontent.com/117144985/200223627-a2c3b63b-d12e-4a7a-ac11-16b72bbede9f.png)
    
  + **AppsFlyer**: Đã có trong SDK có thể bỏ qua
    Import Package [AppsFlyer package](https://nextcloud.rocketstudio.com.vn/s/QTA9PdDoNCWczpC).
    NOTE: Hiện tại đang Disable Appflyer, cần cập nhật ID,Key prefab này trong LoadingScene khi bên Marketing đưa.
    
    <img width="413" alt="image" src="https://user-images.githubusercontent.com/117144985/200230500-864dbcf9-18aa-493d-88d0-517ff0e58f4c.png">
    
## STEP 2 - Tiếp tục một dự án đã có hoặc đã hoàn thành STEP 1

   + **FireBase** :
    Chỉ Import các Packages này
    
 [FirebaseAnalytics](https://nextcloud.rocketstudio.com.vn/s/APsdCLd5ZWtNMTQ),

 [FirebaseCrashlytics](https://nextcloud.rocketstudio.com.vn/s/Dff8pCfKrRRzagz),

 [FirebaseRemoteConfig](https://nextcloud.rocketstudio.com.vn/s/EFpZRzyLXs462pX). 
     
NOTE: Hiện tại đang dùng file google-services của SDK, cần cập nhật file này khi bên Marketing đưa. Contact Thức, Hiếu để lấy access vào Firebase Dashboard Test

## STEP 3: Version Checks

NOTE: Chỉ được dùng version trong được cung cấp trong link download phía dưới

- [Max SDK](https://nextcloud.rocketstudio.com.vn/s/mWtGftaDDBKgxXC) : 5.5.7
- Firebase : 9.1.0
    [FirebaseAnalytics](https://nextcloud.rocketstudio.com.vn/s/APsdCLd5ZWtNMTQ),
    [FirebaseCrashlytics](https://nextcloud.rocketstudio.com.vn/s/Dff8pCfKrRRzagz),
    [FirebaseRemoteConfig](https://nextcloud.rocketstudio.com.vn/s/EFpZRzyLXs462pX).
- [AppsFlyer](https://nextcloud.rocketstudio.com.vn/s/QTA9PdDoNCWczpC) : 6.8.1

## Các Packages khác dùng trong SDK

#### Code Pattern Package
- [UniRx](https://assetstore.unity.com/packages/tools/integration/unirx-reactive-extensions-for-unity-17276#description)
- [State Machine](https://github.com/dotnet-state-machine/stateless)


#### Dev Utilities Packages
- [NugetForUnity](https://github.com/GlitchEnzo/NuGetForUnity/releases)
- [Joystick Pack](https://assetstore.unity.com/packages/tools/input-management/joystick-pack-107631/reviews)
- [Find Reference 2](https://nextcloud.rocketstudio.com.vn/s/sAk6XP38CZzPAf7?path=%2FVietlabs%2FEditor%20ExtensionsUtilities)
- [Odin](https://nextcloud.rocketstudio.com.vn/s/sAk6XP38CZzPAf7?path=%2FSirenix%2FEditor%20ExtensionsSystem)
