using MediatR;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.JoinRoom
{
    public class JoinRoomCommand : IRequest<Result<Guid>>
    {
        public Guid RoomId { get; set; }
    }
}