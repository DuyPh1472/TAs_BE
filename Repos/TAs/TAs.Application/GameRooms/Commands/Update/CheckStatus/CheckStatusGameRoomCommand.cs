using MediatR;
using TAs.Application.GameRooms.DTOs.Commands;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.Update.CheckStatus
{
    public class CheckStatusGameRoomCommand(Guid roomId)
    : IRequest<Result<UpdateStatusRoomDTO>>
    {
        public Guid RoomId = roomId;
    }
}