# Tính năng Tự động Kết thúc Game khi Hết Thời gian

## Tổng quan

Tính năng này đảm bảo rằng game sẽ tự động kết thúc và lưu kết quả vào database khi hết thời gian, mà không cần người dùng phải nhấn nút "Kết thúc game".

## Cách hoạt động

### 1. BackgroundService
- `GameRoomTimeoutService` chạy ngầm và kiểm tra các phòng đang chơi mỗi 2 giây
- Khi phát hiện phòng nào đã hết thời gian (dựa vào `GameStartedAt` + `TimeLimit`), service sẽ tự động:
  - Lưu kết quả game vào database
  - Cập nhật trạng thái phòng thành `Finished`
  - Broadcast `GameFinished` event đến tất cả clients trong phòng

### 2. Tracking thời gian
- Khi game bắt đầu (`StartGame`), `GameStartedAt` được set thành thời điểm hiện tại
- BackgroundService tính toán thời gian hết hạn: `GameStartedAt + TimeLimit`
- Khi `DateTime.UtcNow >= timeoutTime`, game sẽ tự động kết thúc

### 3. Lưu kết quả
- Kết quả được lưu thông qua `SaveGameResultCommand`
- Bao gồm điểm số, thời gian chơi, số câu đúng/sai của từng người chơi

### 4. Event Broadcasting
- Sử dụng `IGameRoomEventService` interface để decouple Application layer khỏi SignalR
- `GameRoomEventService` implementation trong API project xử lý việc broadcast SignalR events

## Các thay đổi đã thực hiện

### Backend

1. **GameRoomState.cs**
   - Thêm field `GameStartedAt` để track thời điểm bắt đầu game

2. **InMemoryGameRoomService.cs**
   - Thêm method `StartGame()`: Set `GameStartedAt` và cập nhật status thành `Playing`
   - Thêm method `EndGame()`: Cập nhật status thành `Finished`

3. **GameRoomTimeoutService.cs** (Mới)
   - BackgroundService chạy ngầm kiểm tra timeout
   - Tự động lưu kết quả và broadcast event

4. **IGameRoomEventService.cs** (Mới)
   - Interface để decouple việc broadcast events

5. **GameRoomEventService.cs** (Mới)
   - Implementation trong API project để broadcast SignalR events

6. **GameRoomHub.cs**
   - Cập nhật `StartGame()`: Gọi `inMemoryService.StartGame()`
   - Cập nhật `EndGame()`: Gọi `inMemoryService.EndGame()`

7. **ServiceCollectionExtensions.cs**
   - Đăng ký `GameRoomTimeoutService` vào DI container

8. **Program.cs**
   - Đăng ký `IGameRoomEventService` và `GameRoomEventService` vào DI container

### Frontend

1. **DictationLessonForPK.tsx**
   - Loại bỏ button "Kết thúc game"
   - Thay thế bằng thông báo "Game sẽ tự động kết thúc khi hết thời gian"
   - Loại bỏ prop `endGame` và logic gọi `endGame()`

2. **MultiplayerRoom.tsx**
   - Loại bỏ việc truyền `endGame` prop

3. **useGameRoomSignalR.ts**
   - Loại bỏ method `endGame()`

## Lợi ích

1. **Đảm bảo kết quả luôn được lưu**: Không phụ thuộc vào thao tác của user
2. **Trải nghiệm người dùng tốt hơn**: Không cần nhấn nút kết thúc
3. **Tính nhất quán**: Tất cả game đều kết thúc đúng thời gian
4. **Dễ maintain**: Logic tập trung ở backend, không phân tán ở client
5. **Clean Architecture**: Sử dụng interface để decouple các layer

## Cấu hình

- **Check interval**: 2 giây (có thể điều chỉnh trong `GameRoomTimeoutService`)
- **Time limit**: Lấy từ settings của phòng (mặc định 60 giây)
- **Logging**: Đầy đủ log để debug và monitor

## Monitoring

Service sẽ log các thông tin sau:
- Khi phát hiện timeout: `"Game timeout detected for room {RoomId}"`
- Khi lưu kết quả thành công: `"Game result saved successfully"`
- Khi broadcast event: `"GameFinished event broadcasted"`

## Troubleshooting

1. **Game không tự động kết thúc**: Kiểm tra log của `GameRoomTimeoutService`
2. **Kết quả không được lưu**: Kiểm tra `SaveGameResultCommand` và database connection
3. **Client không nhận được event**: Kiểm tra SignalR connection và `GameRoomEventService`
4. **Dependency injection error**: Kiểm tra đăng ký services trong `Program.cs` và `ServiceCollectionExtensions.cs` 