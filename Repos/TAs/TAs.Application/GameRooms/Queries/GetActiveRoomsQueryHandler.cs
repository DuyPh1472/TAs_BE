using MediatR;

namespace TAs.Application.GameRooms.Queries
{
    public class GetActiveRoomsQueryHandler : IRequestHandler<GetActiveRoomsQuery, IEnumerable<object>>
    {
        private readonly InMemoryGameRoomService _roomService;
        public GetActiveRoomsQueryHandler(InMemoryGameRoomService roomService)
        {
            _roomService = roomService;
        }
        public Task<IEnumerable<object>> Handle(GetActiveRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = _roomService.GetActiveRoomsDTO();
            return Task.FromResult(rooms);
        }
    }
} 