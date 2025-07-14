using MediatR;

namespace TAs.Application.GameRooms.Queries
{
    public class GetRoomDetailsQuery : IRequest<object?>
    {
        public Guid RoomId { get; set; }
    }
} 