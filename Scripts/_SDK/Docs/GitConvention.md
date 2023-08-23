# Git Convention
## Branching
### Naming
- camel-case: add-player; set-game-rule
- có ý nghĩa
### Team 2 người
1. Tạo 2 branches có dùng tên người: dat-branch; thuc-branch
2. Thường xuyên merge với nhau
3. Gắn commit với trello ticket
4. Không quan trọng nhánh default
### Team 3 người
1. Tạo branch gắn với trello ticket
2. Thường xuyên merge from default để update code mới nhất.
3. Tạo Pull Request để merge với nhánh default
4. Phải có approve trước khi merge, người review và approve được chỉ định trong team

## Tips
### Sử dụng prefrab để tránh conflict scene files.
- Tạo Prefab cho 1 group các items trong Hierarchy. Ví dụ UI
- Lưu Prefab đó trong Scene/Groups Folder
