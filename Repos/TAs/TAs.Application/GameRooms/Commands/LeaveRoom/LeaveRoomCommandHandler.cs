using MediatR;
using TAs.Application.GameRooms.HandlerExceptionGameRooms;
using TAs.Application.Interfaces;
using TAs.Application.Users;
using TAs.Application.Users.HandlerErrors;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.LeaveRoom
{
    public class LeaveRoomCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext)
    : IRequestHandler<LeaveRoomCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(LeaveRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await unitOfWork.GameRoomRepository.GetGameRoomByRoomId(request.RoomId);
            if (room is null)
                return Result<Guid>.Failure(GameRoomErrors.RoomNotFound(request.RoomId));

            var currentUser = userContext.GetCurrentUser();
            if (currentUser is null)
                return Result<Guid>.Failure(IdentityErrors.UserNotFound);

            var currentPlayer = await unitOfWork.PlayerInRoomRepository
                .GetPlayerInRoomByUserAndRoom(currentUser.Id, room.Id);
            if (currentPlayer is null)
                return Result<Guid>.Failure(GameRoomErrors.UserNotInRoom(currentPlayer!.UserId, currentPlayer.RoomId));

            var playersInRoom = await unitOfWork.PlayerInRoomRepository
                .GetPlayerInRoomsByRoomId(request.RoomId);

            var isHost = await unitOfWork.PlayerInRoomRepository
                .CheckPlayerIsHost(currentUser.Id);

            if (isHost)
            {
                var otherPlayers = playersInRoom.Where(p => p.UserId != currentUser.Id).ToList();

                if (otherPlayers.Any())
                {
                    var newHost = otherPlayers.OrderBy(p => p.JoinedAt).First();
                    var getRoom = await unitOfWork.GameRoomRepository.GetByIdAsync(request.RoomId);

                    getRoom!.HostId = newHost.UserId;
                    unitOfWork.GameRoomRepository.Update(getRoom);

                    newHost.IsHost = true;
                    newHost.IsReady = true;
                    unitOfWork.PlayerInRoomRepository.Update(newHost);
                }
                else
                {
                    // Không còn ai khác → xóa phòng
                    var roomToDelete = await unitOfWork.GameRoomRepository.GetByIdAsync(request.RoomId);
                    unitOfWork.GameRoomRepository.Delete(roomToDelete!);
                }
            }

            unitOfWork.PlayerInRoomRepository.Delete(currentPlayer);

            await unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(room.Id);
        }

    }
}