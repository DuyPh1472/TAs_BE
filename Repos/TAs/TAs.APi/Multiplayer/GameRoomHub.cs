using Microsoft.AspNetCore.SignalR;
using TAs.Application.GameRooms;
using MediatR;
using TAs.Application.Users;
using Microsoft.AspNetCore.Authorization;
using TAs.Application.GameRooms.Queries;
using TAs.Application.GameRooms.Commands.CreateGameRoomInMemory;
using TAs.Application.GameRooms.Commands.Update.UpdateRoomSettings;

namespace TAs.APi.Multiplayer
{
    [Authorize]
    public class GameRoomHub(IMediator mediator, IUserContext userContext) : Hub
    {
        private readonly IMediator _mediator = mediator;
        private readonly IUserContext _userContext = userContext;

        public async Task JoinRoom(string roomId, string userId, string userName)
        {
            try
            {
                // Có thể xác thực userId nếu muốn, hoặc chỉ dùng userId, userName truyền từ FE
                Console.WriteLine($"[GameRoomHub] User {userId} ({userName}) joining room {roomId}");
                Console.WriteLine($"[GameRoomHub] Connection ID: {Context.ConnectionId}");
                
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                await Clients.Group(roomId).SendAsync("PlayerJoined", userId, userName);
                Console.WriteLine($"[GameRoomHub] User {userId} successfully joined room {roomId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameRoomHub] Error joining room: {ex.Message}");
                Console.WriteLine($"[GameRoomHub] Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task LeaveRoom(Guid roomId)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
            {
                await Clients.Caller.SendAsync("LeaveFailed", "User not authenticated");
                return;
            }
            // TODO: Implement RemovePlayer command/handler if needed
            // await _mediator.Send(new LeaveGameRoomCommand { RoomId = roomId });
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
            await Clients.Group(roomId.ToString()).SendAsync("PlayerLeft", currentUser.Id);
        }

        public async Task UpdateSettings(string roomId, UpdateSettingsDTO settings)
        {
            try
            {
                Console.WriteLine($"[GameRoomHub] UpdateSettings called with roomId: {roomId}");
                Console.WriteLine($"[GameRoomHub] Settings received: {System.Text.Json.JsonSerializer.Serialize(settings)}");
                Console.WriteLine($"[GameRoomHub] Context.User: {Context.User?.Identity?.Name}");
                Console.WriteLine($"[GameRoomHub] Context.User.IsAuthenticated: {Context.User?.Identity?.IsAuthenticated}");
                
                var currentUser = _userContext.GetCurrentUser();
                Console.WriteLine($"[GameRoomHub] CurrentUser from context: {currentUser?.Id}");
                
                if (currentUser == null)
                {
                    Console.WriteLine("[GameRoomHub] User not authenticated");
                    await Clients.Caller.SendAsync("UpdateSettingsFailed", "User not authenticated");
                    return;
                }
                
                Console.WriteLine($"[GameRoomHub] Current user: {currentUser.Id}");

                // Parse roomId từ string sang Guid
                if (!Guid.TryParse(roomId, out var roomGuid))
                {
                    Console.WriteLine($"[GameRoomHub] Invalid room ID format: {roomId}");
                    await Clients.Caller.SendAsync("UpdateSettingsFailed", "Invalid room ID format");
                    return;
                }

                // Gọi command để update settings trong database
                var command = new UpdateRoomSettingsCommand
                {
                    RoomId = roomGuid,
                    TimeLimit = settings.TimeLimit,
                    MaxRetries = settings.MaxRetries,
                    ShowRealTimeScore = settings.ShowRealTimeScore,
                    AllowHints = settings.AllowHints
                };

                Console.WriteLine($"[GameRoomHub] Sending command to mediator...");
                var result = await _mediator.Send(command);
                Console.WriteLine($"[GameRoomHub] Command result: {result.IsSuccess}");
                
                if (result.IsSuccess)
                {
                    // Broadcast updated settings to all clients in the room
                    Console.WriteLine($"[GameRoomHub] Broadcasting settings update to room {roomId}: {System.Text.Json.JsonSerializer.Serialize(settings)}");
                    await Clients.Group(roomId).SendAsync("SettingsUpdated", settings);
                    Console.WriteLine($"[GameRoomHub] Broadcast completed");
                }
                else
                {
                    Console.WriteLine($"[GameRoomHub] Update settings failed: {result.Error.Description}");
                    await Clients.Caller.SendAsync("UpdateSettingsFailed", result.Error.Description);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameRoomHub] Exception in UpdateSettings: {ex.Message}");
                Console.WriteLine($"[GameRoomHub] Stack trace: {ex.StackTrace}");
                await Clients.Caller.SendAsync("UpdateSettingsFailed", "Failed to update settings: " + ex.Message);
            }
        }

        // Game events methods
        public async Task SubmitAnswer(Guid roomId, object answerData)
        {
            // TODO: Implement SubmitAnswerCommand/Handler if needed
            await Clients.Group(roomId.ToString()).SendAsync("PlayerAnswered", answerData);
        }

        public async Task StartGame(Guid roomId, Guid lessonId)
        {
            // TODO: Implement StartGameCommand/Handler if needed
            // await _mediator.Send(new StartGameCommand { RoomId = roomId, LessonId = lessonId });
            await Clients.Group(roomId.ToString()).SendAsync("GameStarted", lessonId);
        }

        public async Task NextSentence(Guid roomId)
        {
            await Clients.Group(roomId.ToString()).SendAsync("NextSentence");
        }

        public async Task EndGame(Guid roomId)
        {
            // TODO: Implement EndGameCommand/Handler if needed
            // await _mediator.Send(new EndGameCommand { RoomId = roomId });
            await Clients.Group(roomId.ToString()).SendAsync("GameFinished");
        }

        public async Task LessonSelected(Guid roomId, string lessonId, string lessonTitle, object lessonData)
        {
            await Clients.Group(roomId.ToString()).SendAsync("LessonSelected", lessonId, lessonTitle, lessonData);
        }

        // Additional events for lobby
        public async Task CreateRoom(string roomName, int maxPlayers, Guid categoryId)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
            {
                await Clients.Caller.SendAsync("CreateRoomFailed", "User not authenticated");
                return;
            }
            var roomId = await _mediator.Send(new CreateGameRoomCommand
            {
                RoomName = roomName,
                MaxPlayers = maxPlayers,
                CategoryId = categoryId
            });
            // Lấy lại room details để broadcast
            var room = await _mediator.Send(new GetRoomDetailsQuery { RoomId = roomId });
            await Clients.All.SendAsync("RoomCreated", room);
        }

        public async Task CloseRoom(Guid roomId)
        {
            // TODO: Implement CloseRoomCommand/Handler if needed
            // await _mediator.Send(new CloseRoomCommand { RoomId = roomId });
            await Clients.All.SendAsync("RoomClosed", roomId);
        }

        public async Task UpdateRoom(Guid roomId)
        {
            var room = await _mediator.Send(new GetRoomDetailsQuery { RoomId = roomId });
            if (room != null)
            {
                await Clients.All.SendAsync("RoomUpdated", room);
            }
        }

        // Test method to check if SignalR is working
        public async Task TestConnection()
        {
            Console.WriteLine("[GameRoomHub] TestConnection called");
            await Clients.Caller.SendAsync("TestResponse", "SignalR is working!");
        }

        // Simple test method for UpdateSettings
        public async Task TestUpdateSettings(string roomId, string testData)
        {
            Console.WriteLine($"[GameRoomHub] TestUpdateSettings called with roomId: {roomId}, testData: {testData}");
            await Clients.Caller.SendAsync("TestUpdateSettingsResponse", "TestUpdateSettings is working!");
        }
    }
} 