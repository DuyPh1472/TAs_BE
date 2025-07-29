using MediatR;

namespace TAs.Application.GameRooms.Queries
{
    public class GetRoomDetailsQueryHandler(IInMemoryGameRoomService roomService) : IRequestHandler<GetRoomDetailsQuery, object?>
    {
        private readonly IInMemoryGameRoomService _roomService = roomService;

        public Task<object?> Handle(GetRoomDetailsQuery request, CancellationToken cancellationToken)
        {
            var room = _roomService.GetRoomDetailsDTO(request.RoomId);
            return Task.FromResult(room);
        }
    }
} 