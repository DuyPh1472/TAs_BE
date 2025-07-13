using MediatR;
using TAs.Application.GameRooms.DTOs.Queries;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Queries.GetRoomPlayers
{
    public class GetRoomPlayersQuery : IRequest<Result<List<PlayerDTO>>>
    {
        public Guid RoomId { get; set; }
    }
} 