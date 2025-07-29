using MediatR;
using TAs.Application.Users;

namespace TAs.Application.GameRooms.Commands.ReadyInMemory
{
    public class ReadyInMemoryCommandHandler : IRequestHandler<ReadyInMemoryCommand, ReadyInMemoryResult>
    {
        private readonly IInMemoryGameRoomService _gameRoomService;
        private readonly IUserContext _userContext;

        public ReadyInMemoryCommandHandler(IInMemoryGameRoomService gameRoomService, IUserContext userContext)
        {
            _gameRoomService = gameRoomService;
            _userContext = userContext;
        }

        public async Task<ReadyInMemoryResult> Handle(ReadyInMemoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
            {
                return await Task.FromResult(new ReadyInMemoryResult 
                { 
                    Success = false, 
                    Status = "Unauthorized", 
                    Message = "User not authenticated" 
                });
            }

            // Pass IsReady to the service
            var result = _gameRoomService.ReadyInMemory(request.RoomId, currentUser.Id, request.IsReady);

            // Handle possible null result from service
            if (result == null)
            {
                return await Task.FromResult(new ReadyInMemoryResult
                {
                    Success = false,
                    Status = "Error",
                    Message = "Service returned null result"
                });
            }
            
            return await Task.FromResult(new ReadyInMemoryResult
            {
                Success = result.Success,
                Status = result.Status,
                Message = result.Message
            });
        }
    }
} 