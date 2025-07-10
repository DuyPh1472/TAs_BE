using MediatR;
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
                return Result<Guid>.Failure(IdentityErrors.UserNotFound);
            }
            GameRoom room = new()
            {
                RoomName = request.RoomName,
                HostId = currentUser!.Id,
                IsActive = true,
                Status = GameStatus.Waiting,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = currentUser!.Id
            };
            unitOfWork.GameRoomRepository.Add(room);
            PlayerInRoom playerInRoom = new()
            {
                RoomId = room.Id,
                UserId = room.HostId,
                CreatedBy = room.HostId,
                CreatedAt = DateTimeOffset.UtcNow,
                IsHost = true
            };
            unitOfWork.PlayerInRoomRepository.Add(playerInRoom);
            await unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(room.Id);
        }
    }
}