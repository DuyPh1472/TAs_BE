using MediatR;
using TAs.Application.GameRooms.DTOs.Queries;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Queries.GetGameRoomsByRoomId
{
    public class GetGameRoomByRoomIdQuery : IRequest<Result<GetRoomDetailsDTO>>
    {
        public Guid RoomId { get; set; }
    }
}