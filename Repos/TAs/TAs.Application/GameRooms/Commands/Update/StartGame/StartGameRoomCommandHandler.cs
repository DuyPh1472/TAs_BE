using MediatR;
using TAs.Application.GameRooms.DTOs.Commands;
using TAs.Application.Interfaces;
using TAs.Application.Lessons.DTOs;
using TAs.Application.GameRooms.DTOs.Queries;
using TAs.Domain.Result;
using TAs.Application.GameRooms.HandlerExceptionGameRooms;
using TAs.Application.Users;
using TAs.Application.Users.HandlerErrors;
using TAs.Domain.Enums;

namespace TAs.Application.GameRooms.Commands.Update.StartGame
{
    public class StartGameRoomCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
        : IRequestHandler<StartGameRoomCommand, Result<StartGameRoomDTO>>
    {
        public async Task<Result<StartGameRoomDTO>> Handle(StartGameRoomCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            if (currentUser is null)
                return Result<StartGameRoomDTO>.Failure(IdentityErrors.UserNotFound);
            var room = await unitOfWork.GameRoomRepository.GetGameRoomByRoomId(request.RoomId);
            if (room is null)
                return Result<StartGameRoomDTO>.Failure(GameRoomErrors.NoRoomFound(request.RoomId));
            if (room.HostId != currentUser.Id)
                return Result<StartGameRoomDTO>.Failure(GameRoomErrors.OnlyHostCanStart);
            var players = await unitOfWork.PlayerInRoomRepository.GetPlayerInRoomsByRoomId(room.Id);
            if (players.Count == 0 || !players.All(p => p.IsReady))
                return Result<StartGameRoomDTO>.Failure(GameRoomErrors.NotAllPlayersReady);
            // Chọn lesson
            var lessonId = request.LessonId ?? room.SelectedLessonId;
            if (lessonId == null)
                return Result<StartGameRoomDTO>.Failure(GameRoomErrors.LessonNotSelected);
            var lesson = await unitOfWork.LessonRepository.GetLessonsById(lessonId.Value);
            if (lesson == null)
                return Result<StartGameRoomDTO>.Failure(GameRoomErrors.LessonNotFound);
            // Cập nhật trạng thái phòng
            room.Status = GameStatus.Playing;
            // Nếu GameRoom chưa có StartedAt, bỏ dòng này hoặc chỉ trả về DateTimeOffset.UtcNow trong DTO
            // room.StartedAt = DateTimeOffset.UtcNow;
            room.SelectedLessonId = lesson.Id;
            unitOfWork.GameRoomRepository.Update(room);
            await unitOfWork.SaveChangesAsync();
            // Chuẩn bị DTO trả về
            var dto = new StartGameRoomDTO
            {
                RoomId = room.Id,
                Status = room.Status,
                Lesson = new LessonDTO
                {
                    LessonId = lesson.Id,
                    Title = lesson.Title,
                    Description = lesson.Description,
                    Level = lesson.Level,
                    Sentences = lesson.Sentences,
                    Accent = lesson.Accent,
                    Duration = lesson.Duration,
                    Topics = lesson.Topics,
                    AudioUrl = lesson.AudioUrl,
                    YoutubeUrl = lesson.YoutubeUrl,
                    VideoId = lesson.VideoId,
                    // Nếu cần challenges, map từ DictationSentences
                    Challenges = lesson.DictationSentences?.Select(ds => new DictationSentenceDTO
                    {
                        Id = ds.Id,
                        Position = ds.Position,
                        Content = ds.Text,
                        AudioSrc = ds.AudioUrl,
                        TimeStart = ds.StartTime,
                        TimeEnd = ds.EndTime,
                        NbComments = ds.NbComments
                    }).ToList() ?? new List<DictationSentenceDTO>()
                },
                Players = players.Select(p => new PlayerInRoomDTO
                {
                    UserId = p.UserId.ToString(),
                    UserName = p.User?.UserName ?? string.Empty,
                    Avatar = p.User?.Avatar ?? string.Empty,
                    IsHost = p.IsHost,
                    IsReady = p.IsReady,
                    Score = p.Score,
                    CurrentProgress = p.CurrentProgress,
                    Status = p.Status.ToString(),
                    JoinedAt = p.JoinedAt
                }).ToList(),
                StartedAt = DateTimeOffset.UtcNow // hoặc bỏ nếu không muốn trả về
            };
            return Result<StartGameRoomDTO>.Success(dto);
        }
    }
}