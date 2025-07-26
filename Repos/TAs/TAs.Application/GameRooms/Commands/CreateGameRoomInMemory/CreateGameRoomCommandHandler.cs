using MediatR;
using TAs.Application.Users;
using TAs.Application.Interfaces;

namespace TAs.Application.GameRooms.Commands.CreateGameRoomInMemory
{
    public class CreateGameRoomCommandHandler(InMemoryGameRoomService roomService, IUnitOfWork unitOfWork,
    IUserContext userContext) : IRequestHandler<CreateGameRoomCommand, Guid>
    {
        private readonly InMemoryGameRoomService _roomService = roomService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IUserContext _userContext = userContext;

        public async Task<Guid> Handle(CreateGameRoomCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
                throw new UnauthorizedAccessException("User not authenticated");

            // Fetch category from DB
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
                throw new Exception($"Category not found for id {request.CategoryId}");
            // Use real category info
            var roomId = _roomService.CreateRoom(
                request.RoomName,
                request.MaxPlayers,
                category.Id,
                category.Title,
                currentUser.Id,
                currentUser.UserName,
                category.Difficult
            );
            return roomId;
        }
    }
}