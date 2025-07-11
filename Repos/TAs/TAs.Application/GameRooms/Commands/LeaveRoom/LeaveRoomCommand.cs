using MediatR;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.LeaveRoom
{
    public class LeaveRoomCommand : IRequest<Result<Guid>>
    {
        public Guid RoomId { get; set; }
    }
}