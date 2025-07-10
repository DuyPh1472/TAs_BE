using MediatR;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.Create
{
    public class CreateRoomCommand : IRequest<Result<Guid>>
    {
        public required string RoomName { get; set; } 
    }
}