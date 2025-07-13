using MediatR;
using TAs.Application.GameRooms.DTOs.Commands;
using TAs.Application.GameRooms.HandlerExceptionGameRooms;
using TAs.Application.Interfaces;
using TAs.Application.Users;
using TAs.Application.Users.HandlerErrors;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.Update.CheckStatus
{
    public class CheckStatusGameRoomCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext) :
     IRequestHandler<CheckStatusGameRoomCommand, Result<UpdateStatusRoomDTO>>
    {
        public async Task<Result<UpdateStatusRoomDTO>> Handle(CheckStatusGameRoomCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            if (currentUser is null)
                return Result<UpdateStatusRoomDTO>.Failure(IdentityErrors.UserNotFound);
            var room = await unitOfWork.GameRoomRepository.GetGameRoomByRoomId(request.RoomId);
            if (room is null)
                return Result<UpdateStatusRoomDTO>.Failure(GameRoomErrors.RoomNotFound(request.RoomId));
            var player = await unitOfWork.PlayerInRoomRepository
           .GetPlayerInRoomByUserAndRoom(currentUser.Id, room.Id);

            if (player is null)
                return Result<UpdateStatusRoomDTO>.Failure(GameRoomErrors.UserNotInRoom(player!.UserId, player.RoomId));
            player.IsReady = !player.IsReady;
            await unitOfWork.SaveChangesAsync();
            var allReady = await unitOfWork.PlayerInRoomRepository.AllPlayerReady(room.Id);

            var dto = new UpdateStatusRoomDTO
            {
                AllReady = false,
                IsReady = player.IsReady,
                RoomId = room.Id,
                UserId = currentUser.Id
            };
            if (allReady)
            {
                dto = new UpdateStatusRoomDTO
                {
                    AllReady = true,
                    IsReady = player.IsReady,
                    RoomId = room.Id,
                    UserId = currentUser.Id
                };
            }
            return Result<UpdateStatusRoomDTO>.Success(dto);
        }
    }
}