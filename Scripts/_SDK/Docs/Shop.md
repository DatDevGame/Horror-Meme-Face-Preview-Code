# Shop
http://www.plantuml.com/plantuml/uml/

Shop là nơi quản lý việc mua bán các item trong game. Các IShopItem là một Interface thể hiện cách Shop abstract Data từ một Entity - ví dụ Skin, Weapon, etc
## 3 Layers
- UI: là các Panels - ShopPanel, ShopItemPanel
- Business Logic: Các Entity ( Skin ) và các Class quản lý các Entity (SkinShop)
- Data: PlayerPrefs và Resources
- Áp dụng CQRS Pattern - Command Query Responsibility Segregation - Trong đó Command là Select hoặc Buy ShopItem và Query là Load ShopItem
- Data luôn đi theo một chiều từ UI --> Business Logic --> Data
- Để đồng bộ UI sau mỗi Command(Update, Delete) có thể Observer Patterns hoặc Events Pattern

![image](https://user-images.githubusercontent.com/1218572/207754798-04b860c0-a1bc-4fc6-8c9f-a6c8828a3fe4.png)

```
@startuml

package UI {
 [ShopPanel] --> [ShopItemPanel] : has many
}

package BusinessLogic {
 [SkinShop] --> [Skin] : has many
}

package Data {
  [PlayerPrefs]
  [Resources] --> [SkinSettings]: has many 
}

@enduml
```
## Class Diagram

- Phải xác định Entity trong Game là gì và IShopItem sẽ là khía cạnh nào của Entity đó
- ShopPanel và ShopItemPanel là các UI Elements trong Shop và chỉ nên là nơi nhận Input của users và load data từ IShopItem - Presentation Layer
- Shop là nơi sử lý các logic liên quan đến load item, mua và select item
- Các request nên có argument duy nhất là IShopItem hoặc ItemId


![image](https://user-images.githubusercontent.com/1218572/207551348-227febf2-16f2-471b-ad39-ff5c51f60c99.png)

```
@startuml

abstract class AbstractEntity
{
 int Id
}

class Skin {
  int Price
  bool IsSelected {get;}
  Activate();
  DeActivate();
}

Skin --> PlayerPrefs: use

Skin --> AbstractEntity: is

interface IShopItem
{
  int Price {get; set;}
  bool IsSelected { get; } 
}

Skin --> IShopItem: implement

class Shop {
  Load()
  Buy(IShopItem shopItem)
}

Shop --> IShopItem: has many
Shop --> GameResources: use

class ShopItemPanel {
  DataChanged()
}

ShopItemPanel --> IShopItem: has

class ShopPanel {
  BuyByCoin()
  BuyByAds()
  Select()
  OnBuySuccess()
}
ShopPanel --> Shop: has
ShopPanel --> ShopItemPanel: has many
@enduml

```
## Data Flow

- Trong Code hiện tại, để đơn giản hóa mình dùng Polymophism và gọi DataChanged bình thường.

- Query: Load Shop Items 

![image](https://user-images.githubusercontent.com/1218572/207556688-76fc7e33-8b58-4524-8eb4-b3c13d2d0dbd.png)

```
@startuml
ShopPanel -> Shop : Load
Shop -> Resource: Get List ShopItem
ShopPanel -> ShopItemPanel: Instantiate
ShopItemPanel -> ShopItem: Load Data
ShopItem -> PlayerPrefs: Get IsSelected
ShopItem -> PlayerPrefs: Get IsBought
@enduml
```

- Command: Select Shop Item

![image](https://user-images.githubusercontent.com/1218572/207556607-0fcdc761-c97b-4972-af96-b6a04610d8ec.png)

```
@startuml
ShopItemPanel -> ShopPanel: OnItemSelected
ShopPanel-> Shop: Select
Shop -> SelectingItem : DeActivate
SelectingItem -> PlayerPrefs: Remove Key
Shop -> SelectedItem: Activate
SelectedItem -> PlayerPrefs: Set Key
@enduml
```
- Query: Báo cho ShopItemPanel biết Data của Entity đã changed để ShopItemPanel load lại data

![image](https://user-images.githubusercontent.com/1218572/207753746-e2ba0041-a353-49b9-bcf1-3ddada1a577f.png)

```
@startuml
title Báo DataChanged cho Selected Item ShopItemPanel
ShopPanel -> ShopItemPanel: DataChanged
ShopItemPanel -> ShopItem: Load
@enduml
```
