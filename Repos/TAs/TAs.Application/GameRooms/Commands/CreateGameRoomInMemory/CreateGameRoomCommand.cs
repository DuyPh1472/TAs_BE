using MediatR;

namespace TAs.Application.GameRooms.Commands.CreateGameRoomInMemory
{
    public class CreateGameRoomCommand : IRequest<Guid>
    {
        public string RoomName { get; set; } = string.Empty;
        public int MaxPlayers { get; set; } = 4;
        public Guid CategoryId { get; set; }
    }
}