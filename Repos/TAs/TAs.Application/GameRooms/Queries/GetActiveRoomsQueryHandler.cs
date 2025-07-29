using MediatR;
using TAs.Application.Interfaces;

namespace TAs.Application.GameRooms.Queries
{
    public class GetActiveRoomsQueryHandler : IRequestHandler<GetActiveRoomsQuery, IEnumerable<object>>
    {
        private readonly IInMemoryGameRoomService _roomService;
        private readonly IUnitOfWork _unitOfWork;
        public GetActiveRoomsQueryHandler(IInMemoryGameRoomService roomService, IUnitOfWork unitOfWork)
        {
            _roomService = roomService;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<object>> Handle(GetActiveRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = _roomService.GetActiveRoomsDTO();
            var result = new List<object>();
            foreach (dynamic room in rooms)
            {
                Guid? catId = null;
                string catTitle = string.Empty;
                Guid parsedId = Guid.Empty;
                if (room.CategoryId != null && Guid.TryParse(room.CategoryId.ToString(), out parsedId))
                {
                    var category = await _unitOfWork.CategoryRepository.GetByIdAsync(parsedId);
                    if (category != null)
                    {
                        catId = category.Id;
                        catTitle = category.Title ?? string.Empty;
                    }
                }
                result.Add(new
                {
                    room.id,
                    room.roomName,
                    CategoryId = catId ?? room.CategoryId,
                    categoryTitle = catTitle ?? room.categoryTitle,
                    room.hostId,
                    room.hostName,
                    room.hostAvatar,
                    room.playerCount,
                    room.maxPlayers,
                    room.status,
                    room.categoryDescription,
                    room.createdAt,
                    room.settings,
                    room.players
                });
            }
            return result;
        }
    }
}