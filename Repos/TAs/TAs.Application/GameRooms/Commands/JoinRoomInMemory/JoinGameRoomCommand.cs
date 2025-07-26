using MediatR;
using TAs.Application.GameRooms.Commands.JoinRoomInMemory;
namespace TAs.Application.GameRooms.Commands.JoinRoomInMemory
{
    public class JoinGameRoomCommand : IRequest<JoinGameRoomResult>
    {
        public Guid RoomId { get; set; }
    }
} 