using MediatR;
using TAs.Application.Users;

namespace TAs.Application.GameRooms.Commands.KickPlayerInMemory
{
    public class KickPlayerInMemoryCommandHandler : IRequestHandler<KickPlayerInMemoryCommand, KickPlayerInMemoryResult>
    {
        private readonly IInMemoryGameRoomService _gameRoomService;
        private readonly IUserContext _userContext;

        public KickPlayerInMemoryCommandHandler(IInMemoryGameRoomService gameRoomService, IUserContext userContext)
        {
            _gameRoomService = gameRoomService;
            _userContext = userContext;
        }

        public async Task<KickPlayerInMemoryResult> Handle(KickPlayerInMemoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
            {
                return await Task.FromResult(new KickPlayerInMemoryResult 
                { 
                    Success = false, 
                    Status = "Unauthorized", 
                    Message = "User not authenticated" 
                });
            }

            var result = _gameRoomService.KickPlayerInMemory(request.RoomId, currentUser.Id, request.TargetUserId);

            // Handle possible null result from service
            if (result == null)
            {
                return await Task.FromResult(new KickPlayerInMemoryResult
                {
                    Success = false,
                    Status = "Error",
                    Message = "Service returned null result"
                });
            }
            
            return await Task.FromResult(new KickPlayerInMemoryResult
            {
                Success = result.Success,
                Status = result.Status,
                Message = result.Message
            });
        }
    }
} 