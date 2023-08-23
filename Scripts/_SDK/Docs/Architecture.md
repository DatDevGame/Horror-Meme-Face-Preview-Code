# SDK Architecture

SDK Architecture dựa trên các knowledge sau:
- Trong OOP một Object sẽ bao gồm data và behavior. UnityEngine đã triển khai MonoBehavior mặc định cho các script nên mục đích chính của Architecture này là tách Data ra khỏi Behavior.
- Strategy Pattern được sử dụng để load data vào các behavior.
- Các data được triển khai theo hướng Entity first
- Architecture có ảnh hưởng nhiều bới Domain Driven Development và Clean Architecture.
 
 ## Layers
 
https://www.plantuml.com/

![image](https://user-images.githubusercontent.com/1218572/206115026-e6202337-b7eb-4292-9566-91a4731dd956.png)


```
@startuml
component "UnityEngine" as engine {
 [MonoBehavior] as behavior  #Yellow
 [GameObject] as go #Yellow
 go ..* behavior: has
}



component "SDK" as core {
 [Entity] as entity
 component [Domain] as domain {
  [Slot] as slot
  [DomainEntity] as domainEntity
  [EntityBehavior] as entityBehavior #Yellow
  domainEntity ..> entity: is
  slot ..> domainEntity: use
  slot ..> entityBehavior: use
 }

 note top of domainEntity
   Data của
   Skill, ShopItem,
   GameItem, etc
 end note

 note bottom of slot
   Modify EntityBehavior 
   với settings từ DomainEntity
   - Strategy Pattern
 end note

 component "Utilties" {
   [UI] #Yellow
   [Input] #Yellow
   [GameManager] #Yellow
   [GameDriver] #Yellow
   [Ads] #Yellow
 }
}

component "Data" as data {
 [PlayerPrefs] as playerPrefs
 [Resources] as resources
 entity ..> playerPrefs: save, get
 entity ..> resources: load
}

@enduml
```

## Components

![image](https://user-images.githubusercontent.com/1218572/206114596-91ec0cde-8c70-420c-b1ef-cbaded0724b1.png)


https://www.plantuml.com/


```
@startuml
package "Game" {
 [GameManager] as gm #Yellow
 [GameDriver] as gd #Yellow

}

package "Entity" {

[IEntity] as entity

} 

package "Shop" {
  [Shop] as sp
  [ShopItem] as sip
  [Price] as pri
  sp ..> sip: has many
  sip ..> entity: is
  sip ..> pri: has
  [ShopPanel] as spp #Yellow
  spp ..> sp: has

}

package "Inventory" {
  [InventorySystem] as invs #Yellow
  [Inventory] as inv
  invs ..> inv: has
  [Item] as ite
  inv ..> ite: has many
  [ItemSlot] as its
  ite ..> entity: is
  its ..> ite: has
}

package "Money" {
 [Wallet] as wal
 [Account] as acc
 acc ..> entity: is
 wal ..> acc: has many
 [Currency] as cur
 acc ..> cur: has
}

package "Skills" {
[Skill] as skill
[SkillLevel] as sle
skill ..> entity: is
skill ..> sle: has many
[SkillSlot] as ssl
[SkillSystem] as sss #Yellow
[SkillBehavior] as sbe #Yellow
 sss ..> ssl: has many
 ssl ..> skill: has
 ssl ..> sbe: has
}

package "Maps" {
[Map] as map
map ..> entity: is
[MapSystem] as mss #Yellow
mss ..> map: has many
}

package "Input" {
 [InputDevice] as iss #Yellow 
 [IHasInput] as hai
 [IInput] as inp
 inp ..> iss: use
}

database "Data" {
  [PlayerPrefs] as pp #Yellow
  entity ..> pp: use
  [GameResources] as gr #Yellow
  entity ..> gr: load
}

@enduml
```
