using MediatR;
using TAs.Application.GameRooms.HandlerExceptionGameRooms;
using TAs.Application.Interfaces;
using TAs.Application.Users;
using TAs.Domain.Entities;
using TAs.Domain.Enums;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.JoinRoom
{
    public class JoinRoomCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext)
     : IRequestHandler<JoinRoomCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(JoinRoomCommand request, CancellationToken cancellationToken)
        {
            CurrentUser? currentUser = userContext.GetCurrentUser();
            var room = await unitOfWork.GameRoomRepository.GetByIdAsync(request.RoomId);
            if (room is null)
                return Result<Guid>.Failure(GameRoomErrors.RoomNotFound(request.RoomId));
            // Kiểm tra user đã ở trong phòng nào chưa (bất kỳ phòng nào)
            var existingPlayer = await unitOfWork.PlayerInRoomRepository.GetPlayerInRoomByUser(currentUser!.Id);
            if (existingPlayer is not null)
                return Result<Guid>.Failure(GameRoomErrors.UserAlreadyInRoom(existingPlayer.UserId, existingPlayer.RoomId));
            PlayerInRoom playerInRoom = new()
            {
                RoomId = room.Id,
                CreatedBy = currentUser!.Id,
                CreatedAt = DateTimeOffset.UtcNow,
                IsHost = false,
                IsReady = false,
                Status = PlayerStatus.Connected,
                UserId = currentUser!.Id
            };
            unitOfWork.PlayerInRoomRepository.Add(playerInRoom);
            await unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(playerInRoom.Id);
        }
    }
}