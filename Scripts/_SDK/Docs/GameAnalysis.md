# Game Analysis
## Chơi game

- Chơi game được lựa chọn để cloned và các game tương tự
- Xác định những keys của game để biết game thu hút người chơi ở điểm nào
- Ví dụ Game Nextbot Chasing:

<img width="425" alt="image" src="https://user-images.githubusercontent.com/1218572/208005233-c4990f05-2c64-4c31-9c2a-af071d92db87.png">

## Xác định các Objects trong Game

- Xác định những Objects quan trọng nhất có liên quan đến Key phía trên để phân tích trước.
- Phân tích game ở góc độ quan sát cả game và người chơi game, tránh rơi vào mode người chơi game để phân tích game
- Tránh lầm lẫn giữa khái niệm Class và Object: Ví dụ Enemy là Category, còn Enemy ở vị trí (20,34,90), có model huggy, tốc độ chạy 20 là Object.

<img width="347" alt="image" src="https://user-images.githubusercontent.com/1218572/208005788-618979b3-e96f-4d00-84d3-4fda3d1df506.png">

## Xác định các Behaviors của các Objects

- Với mỗi Object đã lựa chọn phía trên, ta phân tích các Behaviors của Object đó
- Nếu một Object được tạo ra chỉ bằng cách bật tắt các Behaviors thì ta có thể nhóm các Objects đó lại thành Một Category. Ví dụ RegionalEnemy và GlobalEnemy trong game Nextbot sẽ nhóm lại thành Enemy
- Behavior luôn phải xác định Object là Actor của Behavior đó, tránh nhìn Behavior ở góc độ người chơi.
- Behavior luôn là một hành động.

<img width="497" alt="image" src="https://user-images.githubusercontent.com/1218572/208006655-e73c5b17-02ba-450a-99b8-a4586026ce82.png">

## Xác định Data của các Behavior

TO BE CONTINUED

## Xác định Entity trong Game

TO BE CONTINUED

## Xác định GamePlay Behaviors

TO BE CONTINUED
