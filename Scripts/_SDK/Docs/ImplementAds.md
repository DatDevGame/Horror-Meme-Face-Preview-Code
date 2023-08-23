# Cách implement ads vào game

Nội dung chính

1. Giới thiệu
2. Cách hoạt động
3. Những lưu ý

## 1. Giới thiệu

Đây là phần implement vào Game sau khi đã Import các package từ [CreateProject](https://github.com/rocketsaigon/RocketSgSdk/blob/master/Assets/_SDK/Docs/CreateProject.md)

Để hiểu về quy trình config Ads hãy đọc phần : [Ads and Analytics ](https://github.com/rocketsaigon/RocketSgSdk/blob/master/Assets/_SDK/Docs/Ads.md)

## 2. Cách hoạt động

### Setup

AdsManager đã được tạo trong [SplashScreen](https://github.com/rocketsaigon/RocketSgSdk/blob/cff609ee733ba934efe0fbf1967f5670df1d4acd/Assets/_SDK/SplashScreen/LoadingScene.cs#L16)

- `AdsManager` là Class quản lý Ads bao gồm `AdsClient` implement interface `IAdsClient`.
- `IAdsClient` định khai báo các phương thức dùng để load và show Ads
- `MockAdsClient` và `MaxAdsClient` sẽ là hai class implement interface `IAdsClient`. 
  + `MockAdsClient` sử dụng để Debug, kiểm tra việc show ads đã được thiết lập chính xác chưa trước khi có file config từ Maketing. 
  + `MaxAdsClient` sẽ định nghĩa chính xác việc load, show ads

![image](https://user-images.githubusercontent.com/117144985/208034747-b0193da0-8cea-4574-97d1-53369b540174.png)

==> Chúng ta sẽ dùng class `AdsManager` để cài đặt các loại Ads trong game

### Các loại quảng cáo

Hiện tại chúng ta có 4 loại quảng cáo chính:

- Banner
- Intertial
- Reward
- App open ads (AOA)

### Chi tiết từng loại ads

#### Banner:

Luôn show ở bottom của game sau khi đã vào Game vì nó đã được settup và Init sẵn trong constructor khi `AdsClient` Init lúc khởi tạo Game.
Việc của dev chỉ là sử dụng hàm `ShowBanner` thông qua `AdsManager.Instance.AdsClient` khi vào đã vào Game.
Hàm `ShowBanner` sẽ check xem banner đã được init thành công hay chưa, nếu chưa sẽ chờ đến khi init thành công rồi toggle (active) nó lên

```dotnet
//EXAMPLE
AdsManager.Instance.AdsClient.ShowBanner(true);
```

#### Intertial:

Hiển thị khi người chơi chuyển tiếp Level (có thể là state), restart level, hoặc bấm vào các phím chức năng có chủ đích. `Intertial` trước khi hiển thị phải check `Capping Time` (đủ một thời gian nhất định mới được hiển thị và điều này đã có sẵn trong SDK) được setup trên FireBase (Bạn sẽ phải define biến Capping Time khi Release).

> Lưu ý: Intertial không được đột ngột xuất hiện

Sử dụng:
Gọi hàm `ShowInterstial()`, truyền vào hai params `[ levelIndex, placement ]` để tracking event analytics show Interstial này.

```dotnet
//EXAMPLE
AdsManager.Instance.AdsClient.ShowInterstitial(0, "TestExample");
```

#### Reward:

Hiển thị khi người chơi muốn nhận vật phẩm (nhân đôi coin, trial skin, ...)

> Lưu ý: RewardVideo cũng không được đột ngột xuất hiện.

Sử dụng:
Gọi hàm `ShowRewardedAd()`, truyền vào các params `[ levelIndex, placement, callbackEvent ]`

```dotnet
//EXAMPLE
AdsManager.Instance.AdsClient.ShowRewardedVideo(0, "TestExample", (result =>
		{
			if (result == ShowResult.Finished)  //TODO: Set Reward Items here.
      
		}));
```

#### AOA:

AOA Show khi mở Game, nếu mở lần đầu tiên phải đảm bảo sau 7 seconds ( Google Policy). Show AOA cả khi người chơi hot-start game (start game từ background).

> Lưu ý: Không show AOA khi người chơi mới quay lại game từ quảng cáo. (Và điều này đã được setup sẵn trong SDK)

Sử dụng:
Gọi hàm `AdsManager.Instance.AdsClient.ShowAOA();` để show AOA

```dotnet
//EXAMPLE
AdsManager.Instance.AdsClient.ShowAOA();
```

## 3. Lưu ý

- Tránh lỗi ANR:
  `AOA` phải được load trước khi load các loại ads khác
- Tránh vi phạm chính sách:
  Ở `sploadScreen`, cần đợi ít nhất 7s mới được show `AOA` trước khi vào `main scene`
- `Placement` truyền vào các hàm show ads: lấy từ các biến trong [AnalyticEvent](https://github.com/rocketsaigon/RocketSgSdk/blob/master/Assets/_SDK/Analytics/AnalyticsEvent.cs#L6)

### Google Policy

```
Từ 30/9, toàn bộ interstitals ads xuất hiện trong 1 trong 3 trường hợp sau đây sẽ bị cấm:
1. Xuất hiện đột ngột giữa màn chơi (ví dụ như các dòng Tycoon không chia level và chỉ có capping time hoặc show ở check point)
2. Xuất hiện ở đầu level (ví dụ như FNF show đầu level)
3. Xuất hiện đúng vị trí nhưng không thể tắt sau 15s
```

## Giải thích Sequence Diagram

https://sequencediagram.org/

```
	title Ads and Analytics

participant ApplicationStart
participant Game

participant MaxAdsClient

participant "AppLoving Max(Mediation-Platform)" as AppLoving MAX

participant Firebase

participant AppsFlyer

ApplicationStart -> ApplicationStart:Init Firebase, AppsFlyer Và AdsManager
ApplicationStart -> Firebase:Lấy Frequency CappingTime trong RemoteConfig

ApplicationStart -> MaxAdsClient:Setup Client với AdsConfig
MaxAdsClient -> AppLoving MAX: Init Banner, Inter, RV, AOA => Load AOA
AppLoving MAX -> AppLoving MAX: Load Inter, RV

ApplicationStart -> MaxAdsClient:Show AOA
MaxAdsClient -> AppLoving MAX: Show AOA

Game -> MaxAdsClient: Show Banner ở Bottom khi Game Start
MaxAdsClient -> AppLoving MAX: Show Banner

note over Game:Dựa theo Google Policy, Frequency Capping, Game Rules

Game -> MaxAdsClient:Show Inter khi có thể (check CappingTime)
MaxAdsClient -> AppLoving MAX: Show Inter
MaxAdsClient ->Firebase:Log Show Inter
MaxAdsClient -> AppsFlyer:Log Show Inter

Game -> MaxAdsClient: Show RV khi player muốn reward
MaxAdsClient -> AppLoving MAX: Show RV
MaxAdsClient ->Firebase:Log Show RV
MaxAdsClient -> AppsFlyer:Log Show RV
AppLoving MAX -> MaxAdsClient: OnRewarded
MaxAdsClient -> Game: OnRewarded
Game->Game: Reward if Result == finish



```
