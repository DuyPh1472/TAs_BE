using MediatR;
using TAs.Application.GameRooms.DTOs.Queries;
using TAs.Application.GameRooms.HandlerExceptionGameRooms;
using TAs.Application.Interfaces;
using TAs.Application.Users;
using TAs.Application.Users.HandlerErrors;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Queries.GetGameRoomsByRoomId
{
    public class GetGameRoomByRoomIdQueryHandler(IUnitOfWork unitOfWork,
    IUserContext userContext)
    : IRequestHandler<GetGameRoomByRoomIdQuery, Result<GetRoomDetailsDTO>>
    {
        public async Task<Result<GetRoomDetailsDTO>> Handle(GetGameRoomByRoomIdQuery request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            if (currentUser is null)
                return Result<GetRoomDetailsDTO>.Failure(IdentityErrors.UserNotFound);

            var room = await unitOfWork.GameRoomRepository.GetGameRoomByRoomId(request.RoomId);
            if (room is null)
                return Result<GetRoomDetailsDTO>.Failure(GameRoomErrors.NoRoomFound(request.RoomId));

            var response = new GetRoomDetailsDTO
            {
                Id = room.Id.ToString(),
                CreatedAt = room.CreatedAt,
                CreatedBy = room.CreatedBy,
                CategoryDescription = room.Category?.Description ?? string.Empty,
                CategoryDifficult = room.Category?.Difficult ?? string.Empty,
                CategoryId = room.CategoryId.ToString(),
                CategoryTitle = room.Category?.Title ?? string.Empty,
                HostAvatar = room.Host?.Avatar ?? string.Empty,
                HostId = room.HostId.ToString(),
                HostName = room.Host?.UserName ?? string.Empty,
                RoomName = room.RoomName,
                MaxPlayers = room.MaxPlayers,
                Status = room.Status.ToString(),
                CurrentPlayers = room.PlayerInRooms?.Count ?? 0,
                SelectedLessonId = room.SelectedLessonId?.ToString(),
                SelectedLessonTitle = room.SelectedLesson?.Title,
                Players = room.PlayerInRooms?.Select(pl => new PlayerInRoomDTO
                {
                    UserId = pl.User?.Id.ToString() ?? string.Empty,
                    Avatar = pl.User?.Avatar ?? string.Empty,
                    UserName = pl.User?.UserName ?? string.Empty,
                    IsHost = pl.IsHost,
                    IsReady = pl.IsReady,
                    Score = pl.Score,
                    CurrentProgress = pl.CurrentProgress,
                    Status = pl.Status.ToString(),
                    JoinedAt = pl.JoinedAt
                }).ToList() ?? []
            };

            return Result<GetRoomDetailsDTO>.Success(response);
        }
    }
}