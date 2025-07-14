using MediatR;

namespace TAs.Application.GameRooms.Queries
{
    public class GetActiveRoomsQuery : IRequest<IEnumerable<object>>
    {
    }
} 