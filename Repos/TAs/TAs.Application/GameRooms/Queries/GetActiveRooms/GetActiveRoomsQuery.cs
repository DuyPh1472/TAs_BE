using MediatR;
using TAs.Application.GameRooms.DTOs.Queries;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Queries.GetActiveRooms
{
    public class GetActiveRoomsQuery : IRequest<Result<List<GameRoomListDTO>>>
    {
        public string? SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
} 