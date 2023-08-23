# Skill System
## Terms
- **Skill**: Data của một skill: name, id, prefab, etc - là một Entity, Skill chứa các SkillLevel
- **SkillLevel**: Data của một skill level: index, attributes và modifyOperator

- **SkillBehavior**: Behavior của game object để thực hiện skill
- **SkillSlot**: Logic để gắn Skill Behavior nhất định vào game object, thay đổi giá trị của Skill Behavior dựa theo Skill Level - Strategy Pattern
- **SkillSystem**: Behavior quản lý các skills của một game object bảo gồm: Load Default Skills, thiết lập Input cho Skill Behavior
- **GameResources**: Là một thư viện resources các Skills. Danh sách Skills là các ScriptableObject - AbstractSkillSettings và được truy cập qua GameManager.

- **Attribute**: Thuộc tính của các Skill Level và Skill Behavior - damage, speed
- **ModifierOperator**: phép toán để modify giá trị của Attribute: Add, Multiply, Override
- 
## Sequence Diagram

http://www.plantuml.com/plantuml/uml/SoWkIImgAStDuNBAJrBGjLDmpCbCJbMmKiX8pSd9vt98pKi1IW80
### Init Skill cho Hero

```
@startuml
title Load Default Skills cho Hero

HeroSkillSystem->GameResources: Load Hero Default Skills
HeroSkillSystem->SkillSlotFactory: Get SkillSlot với Skill và Level Index
HeroSkillSystem-> HeroGameObject: Add SkillBehavior của Skill
HeroSkillSystem-> HeroGameObject: LevelUp SkillBehavior với data của SkillLevel
HeroSkillSystem-> HeroGameObject: SetInput cho SkillBehavior nếu cần
@enduml
```
![image](https://user-images.githubusercontent.com/1218572/205869271-a8f5d145-2d7b-4200-8d2d-bb79727d2a15.png)

### Control SkillBehavior thông qua Input

```
title Control Behavior qua Input Stream

GameInput -> GameInput: Define MoveStream
MoveSkillBehavior->GameInput: Subscribe MoveStream
note over MoveSkillBehavior: Stream chỉ thực sự chạy khi  có Subscriber.
MoveSkillBehavior->MoveSkillBehavior: Unsubscribe khi Destroy

```
![image](https://user-images.githubusercontent.com/1218572/196839195-dbc7f2f4-048e-4867-998e-fcd4e1c981aa.png)

## NOTES:

### Performance
- Không nên dùng Abstract Class cho Skill Behavior - Các SkillBehavior nên luôn là sealed class
- Không nên tạo nhiều SkillBehavior giống nhau cho nhiều gameobject. Ví dụ MoveSkillBehavior cho 1 Enemy nên chuyển thành MoveSkillBehavior cho GroupEnemy nếu số lượng Enemy nhiều. Reference: https://blog.unity.com/technology/1k-update-calls

### Using POCO - KISS ( Keep It Simple Stupid)
- Skill, SkillLevel không nên là các ScriptableObject mà chỉ nên là các POCO (Plain Old C Object) - tức là các Data Object dùng để thay đổi các thuộc tính của Behavior.
- Nếu cần configure Skill, SkillLevel qua Inspector thì có thể bọc bằng các ScriptableObject - SkillSettings. Ngoài ra có thể đưa các POCO này vào các prefabs.
- Nên sử dụng GameResources để quản lý Skill.
