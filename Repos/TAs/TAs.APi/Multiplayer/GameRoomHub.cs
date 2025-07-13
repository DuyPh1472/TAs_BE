using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TAs.Application.GameRooms;

namespace TAs.APi.Multiplayer
{
    public class GameRoomHub : Hub
    {
        private readonly InMemoryGameRoomService _roomService;
        public GameRoomHub(InMemoryGameRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task JoinRoom(Guid roomId, Guid userId, string userName)
        {
            var success = _roomService.JoinRoom(roomId, userId, userName);
            if (success)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
                await Clients.Group(roomId.ToString()).SendAsync("PlayerJoined", userId, userName);
            }
            else
            {
                await Clients.Caller.SendAsync("JoinFailed", "Room full or already joined");
            }
        }

        public async Task LeaveRoom(Guid roomId, Guid userId)
        {
            _roomService.LeaveRoom(roomId, userId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
            await Clients.Group(roomId.ToString()).SendAsync("PlayerLeft", userId);
        }

        public async Task UpdateSettings(Guid roomId, RoomSettings settings)
        {
            var room = _roomService.GetRoom(roomId);
            if (room != null)
            {
                room.Settings = settings;
                await Clients.Group(roomId.ToString()).SendAsync("SettingsUpdated", settings);
            }
        }

        // Game events methods
        public async Task SubmitAnswer(Guid roomId, object answerData)
        {
            // Broadcast answer submission to all players in room
            await Clients.Group(roomId.ToString()).SendAsync("PlayerAnswered", answerData);
        }

        public async Task StartGame(Guid roomId, Guid lessonId)
        {
            var room = _roomService.GetRoom(roomId);
            if (room != null)
            {
                room.Settings.LessonId = lessonId;
                room.GameStatus = "Playing";
                await Clients.Group(roomId.ToString()).SendAsync("GameStarted", lessonId);
            }
        }

        public async Task NextSentence(Guid roomId)
        {
            // Broadcast next sentence event to all players
            await Clients.Group(roomId.ToString()).SendAsync("NextSentence");
        }

        public async Task EndGame(Guid roomId)
        {
            var room = _roomService.GetRoom(roomId);
            if (room != null)
            {
                room.GameStatus = "Finished";
                await Clients.Group(roomId.ToString()).SendAsync("GameFinished");
            }
        }

        // Additional events for lobby
        public async Task CreateRoom(Guid hostId, string hostName, RoomSettings settings)
        {
            var room = _roomService.CreateRoom(hostId, hostName, settings);
            await Clients.All.SendAsync("RoomCreated", room);
        }

        public async Task CloseRoom(Guid roomId)
        {
            _roomService.LeaveRoom(roomId, Guid.Empty); // Remove all players
            await Clients.All.SendAsync("RoomClosed", roomId);
        }

        public async Task UpdateRoom(Guid roomId)
        {
            var room = _roomService.GetRoom(roomId);
            if (room != null)
            {
                await Clients.All.SendAsync("RoomUpdated", room);
            }
        }
    }
} 