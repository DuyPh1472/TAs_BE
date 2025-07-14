using Microsoft.AspNetCore.SignalR;
using TAs.Application.GameRooms;
using MediatR;
using TAs.Application.GameRooms.Commands;
using TAs.Application.Users;
using Microsoft.AspNetCore.Authorization;
using TAs.Application.GameRooms.Queries;

namespace TAs.APi.Multiplayer
{
    [Authorize]
    public class GameRoomHub(IMediator mediator, IUserContext userContext) : Hub
    {
        private readonly IMediator _mediator = mediator;
        private readonly IUserContext _userContext = userContext;

        public async Task JoinRoom(Guid roomId)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
            {
                await Clients.Caller.SendAsync("JoinFailed", "User not authenticated");
                return;
            }
            var result = await _mediator.Send(new JoinGameRoomCommand { RoomId = roomId });
            if (result)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
                await Clients.Group(roomId.ToString()).SendAsync("PlayerJoined", currentUser.Id, currentUser.Email); // hoặc UserName nếu có
            }
            else
            {
                await Clients.Caller.SendAsync("JoinFailed", "Room full or already joined");
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

        public async Task UpdateSettings(Guid roomId, RoomSettings settings)
        {
            // TODO: Implement UpdateSettingsCommand/Handler if needed
            // await _mediator.Send(new UpdateRoomSettingsCommand { RoomId = roomId, Settings = settings });
            // Broadcast event if needed
            await Clients.Group(roomId.ToString()).SendAsync("SettingsUpdated", settings);
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
    }
} 