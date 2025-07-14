using MediatR;
using TAs.Application.Users;

namespace TAs.Application.GameRooms.Commands
{
    public class CreateGameRoomCommandHandler(InMemoryGameRoomService roomService, IUserContext userContext) : IRequestHandler<CreateGameRoomCommand, Guid>
    {
        private readonly InMemoryGameRoomService _roomService = roomService;
        private readonly IUserContext _userContext = userContext;

        public Task<Guid> Handle(CreateGameRoomCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
                throw new UnauthorizedAccessException("User not authenticated");

            var roomId = Guid.NewGuid();
            var room = new GameRoomState
            {
                RoomId = roomId,
                RoomName = request.RoomName,
                HostId = currentUser.Id,
                Settings = new RoomSettings
                {
                    MaxPlayers = request.MaxPlayers,
                    TimeLimit = 60,
                    MaxRetries = 2,
                    ShowRealTimeScore = true,
                    AllowHints = true,
                    LessonSelection = "host_choice"
                },
                Players = new List<PlayerState>
                {
                    new PlayerState
                    {
                        UserId = currentUser.Id,
                        UserName = currentUser.UserName, // hoặc UserName nếu có
                        Avatar = currentUser.Email[0].ToString(),
                        IsHost = true
                    }
                }
            };
            _roomService.AddRoom(room);
            return Task.FromResult(roomId);
        }
    }
} 