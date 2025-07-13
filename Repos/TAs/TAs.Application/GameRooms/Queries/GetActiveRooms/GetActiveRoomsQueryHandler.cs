using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.GameRooms.DTOs.Queries;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Queries.GetActiveRooms
{
    public class GetActiveRoomsQueryHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetActiveRoomsQuery, Result<List<GameRoomListDTO>>>
    {
        public async Task<Result<List<GameRoomListDTO>>> Handle(GetActiveRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = await unitOfWork.GameRoomRepository.GetActiveRoomsAsync(
                request.SearchTerm,
                request.CategoryId,
                request.Page,
                request.PageSize);

            var roomDTOs = rooms.Select(room => new GameRoomListDTO
            {
                Id = room.Id,
                RoomName = room.RoomName,
                HostName = room.Host.UserName ?? string.Empty,
                HostAvatar = room.Host.Avatar ?? room.Host.FullName.Substring(0, 2).ToUpper(),
                PlayerCount = room.PlayerInRooms.Count,
                MaxPlayers = room.MaxPlayers,
                Status = room.Status.ToString(),
                CategoryTitle = room.Category.Title,
                CategoryDescription = room.Category.Description,
                CreatedAt = room.CreatedAt.DateTime,
                Settings = new GameRoomSettingsDTO
                {
                    TimeLimit = room.TimeLimit,
                    MaxRetries = room.MaxRetries,
                    ShowRealTimeScore = room.ShowRealTimeScore,
                    AllowHints = room.AllowHints,
                    LessonSelection = room.LessonSelection
                },
                Players = room.PlayerInRooms.Select(pir => new PlayerInRoomDTO
                {
                    UserId = pir.UserId.ToString(),
                    UserName = pir.User?.UserName ?? string.Empty,
                    Avatar = pir.User?.Avatar ?? (pir.User?.FullName != null ? pir.User.FullName.Substring(0, 2).ToUpper() : "??"),
                    IsHost = pir.UserId == room.HostId,
                    IsReady = pir.IsReady,
                    Score = pir.Score,
                    CurrentProgress = pir.CurrentProgress,
                    Status = pir.Status.ToString(),
                    JoinedAt = pir.JoinedAt.Date
                }).ToList()

            }).ToList();

            return Result<List<GameRoomListDTO>>.Success(roomDTOs);
        }
    }
}