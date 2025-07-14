using MediatR;
using TAs.Application.Users;

namespace TAs.Application.GameRooms.Commands
{
    public class JoinGameRoomCommandHandler(InMemoryGameRoomService roomService, IUserContext userContext) : IRequestHandler<JoinGameRoomCommand, bool>
    {
        private readonly InMemoryGameRoomService _roomService = roomService;
        private readonly IUserContext _userContext = userContext;

        public Task<bool> Handle(JoinGameRoomCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
                throw new UnauthorizedAccessException("User not authenticated");

            var player = new PlayerState
            {
                UserId = currentUser.Id,
                UserName = currentUser.UserName, // hoặc UserName nếu có
                Avatar = currentUser.Email[0].ToString(),
                IsHost = false
            };
            var result = _roomService.AddPlayer(request.RoomId, player);
            if (!result)
            {
                Console.WriteLine($"Join failed for user {currentUser.Id} to room {request.RoomId}");
            }
            return Task.FromResult(result);
        }
    }
} 