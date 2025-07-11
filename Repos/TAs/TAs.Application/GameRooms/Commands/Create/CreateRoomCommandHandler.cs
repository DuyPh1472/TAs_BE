using MediatR;
using TAs.Application.Categories.HandleErrors;
using TAs.Application.Interfaces;
using TAs.Application.Users;
using TAs.Application.Users.HandlerErrors;
using TAs.Domain.Entities;
using TAs.Domain.Enums;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.Create
{
    public class CreateRoomCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext) :
    IRequestHandler<CreateRoomCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            if (currentUser is null)
            {
                return Result<Guid>
                .Failure(IdentityErrors.UserNotFound);
            }
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(request.CategoryId);
            if (category is null)
            {
                return Result<Guid>
                .Failure(CategoryErrors.NoCategoryFound(request.CategoryId));
            }
            GameRoom room = new()
            {
                RoomName = request.RoomName,
                HostId = currentUser!.Id,
                IsActive = true,
                Status = GameStatus.Waiting,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = currentUser!.Id,
                CategoryId = category.Id,
                MaxPlayers = request.MaxPlayers
            };
            unitOfWork.GameRoomRepository.Add(room);
            PlayerInRoom playerInRoom = new()
            {
                RoomId = room.Id,
                UserId = room.HostId,
                CreatedBy = room.HostId,
                CreatedAt = DateTimeOffset.UtcNow,
                IsHost = true,
                IsReady = true,
                Status = PlayerStatus.Connected,
                JoinedAt = DateTime.UtcNow
            };
            unitOfWork.PlayerInRoomRepository.Add(playerInRoom);
            await unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(room.Id);
        }
    }
}