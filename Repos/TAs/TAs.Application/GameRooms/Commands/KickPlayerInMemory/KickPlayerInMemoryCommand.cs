using MediatR;

namespace TAs.Application.GameRooms.Commands.KickPlayerInMemory
{
    public class KickPlayerInMemoryCommand : IRequest<KickPlayerInMemoryResult>
    {
        public Guid RoomId { get; set; }
        public Guid TargetUserId { get; set; } // The user to be kicked
    }

    public class KickPlayerInMemoryResult
    {
        public bool Success { get; set; }
        public string? Status { get; set; } // "Success", "RoomNotFound", "PlayerNotFound", "Unauthorized", "CannotKickHost"
        public string? Message { get; set; }
    }
} 