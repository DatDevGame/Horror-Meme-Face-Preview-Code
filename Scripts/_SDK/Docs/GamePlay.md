# GamePlay

- Quản lý một Round game bao gồm Init Phase: Map, Skin, GamePlay Settings
- Show Tutorial nếu cần
- Thay đổi nhịp độ game để phù hợp với Interest Curve


```
@startuml
state Init {
 [*] --> [*]:  InitMap
 [*] --> [*]:  InitSkin
 [*] --> [*]:  SetupGamePlay
}



Init --> Running: Play
Running --> Pausing: Pause
Pausing --> Running: UnPause
Running --> Won: Win
Running --> Lost: Lose
state Won {
  [*] --> [*]: NextMap
}
Won --> Init: Retry
Lose --> Init: Retry
@enduml
```
