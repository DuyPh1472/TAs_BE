using MediatR;
using TAs.Application.Users;
using TAs.Application.GameRooms;

namespace TAs.Application.GameRooms.Commands.JoinRoomInMemory
{
    public class JoinGameRoomResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Guid? RoomId { get; set; }
    }

    public class JoinGameRoomCommandHandler(InMemoryGameRoomService roomService, IUserContext userContext) : IRequestHandler<JoinGameRoomCommand, JoinGameRoomResult>
    {
        private readonly InMemoryGameRoomService _roomService = roomService;
        private readonly IUserContext _userContext = userContext;

        public Task<JoinGameRoomResult> Handle(JoinGameRoomCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
                return Task.FromResult(new JoinGameRoomResult { Success = false, Message = "User not authenticated" });

            var player = new PlayerState
            {
                UserId = currentUser.Id,
                UserName = currentUser.UserName, // hoặc UserName nếu có
                Avatar = currentUser.Email[0].ToString(),
                IsHost = false
            };
            var result = _roomService.AddPlayer(request.RoomId, player);
            if (result.Status == "AlreadyInThisRoom")
            {
                return Task.FromResult(new JoinGameRoomResult { Success = false, Message = "already in this room", RoomId = request.RoomId });
            }
            if (result.Status == "AlreadyInAnotherRoom")
            {
                return Task.FromResult(new JoinGameRoomResult { Success = false, Message = "already in another room" });
            }
            if (result.Success)
            {
                return Task.FromResult(new JoinGameRoomResult { Success = true, RoomId = request.RoomId });
            }
            return Task.FromResult(new JoinGameRoomResult { Success = false, Message = result.Message });
        }
    }
} 