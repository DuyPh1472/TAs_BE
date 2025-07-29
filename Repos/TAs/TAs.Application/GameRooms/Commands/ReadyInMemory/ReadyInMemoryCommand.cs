using MediatR;

namespace TAs.Application.GameRooms.Commands.ReadyInMemory
{
    public class ReadyInMemoryCommand : IRequest<ReadyInMemoryResult>
    {
        public Guid RoomId { get; set; }
        public bool IsReady { get; set; } // New property to allow toggling ready/unready
    }
} 