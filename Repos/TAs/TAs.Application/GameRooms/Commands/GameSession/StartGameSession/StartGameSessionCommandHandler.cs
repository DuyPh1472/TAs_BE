using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.Users;
using TAs.Application.Users.HandlerErrors;
using TAs.Application.GameRooms.DTOs.Queries;
using TAs.Domain.Enums;
using TAs.Domain.Result;
using TAs.Application.GameRooms.HandlerExceptionGameRooms;

namespace TAs.Application.GameRooms.Commands.GameSession.StartGameSession
{
    public class StartGameSessionCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
        : IRequestHandler<StartGameSessionCommand, Result<GameSessionDTO>>
    {
        public async Task<Result<GameSessionDTO>> Handle(StartGameSessionCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            if (currentUser is null)
            {
                return Result<GameSessionDTO>.Failure(IdentityErrors.UserNotFound);
            }

            var room = await unitOfWork.GameRoomRepository.GetByIdAsync(request.RoomId);
            if (room is null)
            {
                return Result<GameSessionDTO>.Failure(GameRoomErrors.RoomNotFound(request.RoomId));
            }

            // Chỉ host mới được start game
            if (room.HostId != currentUser.Id)
            {
                return Result<GameSessionDTO>.Failure(GameRoomErrors.NotRoomHost());
            }

            var lesson = await unitOfWork.LessonRepository.GetLessonsById(request.LessonId);
            if (lesson is null)
            {
                return Result<GameSessionDTO>.Failure(GameRoomErrors.LessonNotFound(request.LessonId));
            }

            // Kiểm tra xem đã có session active chưa
            var existingSession = await unitOfWork.GameSessionRepository.GetActiveSessionByRoomIdAsync(request.RoomId);
            if (existingSession != null)
            {
                return Result<GameSessionDTO>.Failure(GameRoomErrors.GameSessionAlreadyExists());
            }

            // Lấy settings từ room nếu có, nếu không thì dùng default
            var timeLimit = 60;
            var maxRetries = 2;
            var showRealTimeScore = true;
            var allowHints = true;

            if (!string.IsNullOrEmpty(room.Settings))
            {
                try
                {
                    var settings = System.Text.Json.JsonSerializer.Deserialize<dynamic>(room.Settings);
                    if (settings != null)
                    {
                        timeLimit = settings.GetProperty("TimeLimit").GetInt32();
                        maxRetries = settings.GetProperty("MaxRetries").GetInt32();
                        showRealTimeScore = settings.GetProperty("ShowRealTimeScore").GetBoolean();
                        allowHints = settings.GetProperty("AllowHints").GetBoolean();
                    }
                }
                catch
                {
                    // Nếu parse JSON fail, dùng default values
                }
            }

            // Tạo game session mới với settings từ room
            var gameSession = new Domain.Entities.GameSession
            {
                RoomId = request.RoomId,
                LessonId = request.LessonId,
                StartedAt = DateTime.UtcNow,
                Status = GameStatus.Waiting,
                CurrentSentence = 0,
                TotalSentences = lesson.DictationSentences.Count,
                TimeLimit = timeLimit,
                MaxRetries = maxRetries,
                ShowRealTimeScore = showRealTimeScore,
                AllowHints = allowHints,
                CreatedBy = currentUser.Id,
                CreatedAt = DateTimeOffset.UtcNow
            };

            unitOfWork.GameSessionRepository.Add(gameSession);

            // Cập nhật trạng thái room
            room.Status = GameStatus.Starting;
            room.SelectedLessonId = request.LessonId;
            room.UpdatedAt = DateTimeOffset.UtcNow;
            room.UpdatedBy = currentUser.Id;

            await unitOfWork.SaveChangesAsync();

            // Lấy danh sách players
            var players = await unitOfWork.PlayerInRoomRepository.GetPlayersByRoomIdAsync(request.RoomId);
            var playerDTOs = players.Select(p => new PlayerDTO
            {
                Id = p.UserId,
                Name = p.User.FullName,
                Avatar = p.User.Avatar,
                IsHost = p.IsHost,
                IsReady = p.IsReady,
                Score = p.Score,
                Status = p.Status.ToString(),
                JoinedAt = p.JoinedAt
            }).ToList();

            var gameSessionDTO = new GameSessionDTO
            {
                SessionId = gameSession.Id,
                RoomId = gameSession.RoomId,
                SelectedLessonId = gameSession.LessonId,
                TotalSentences = gameSession.TotalSentences,
                CurrentSentence = gameSession.CurrentSentence,
                StartedAt = gameSession.StartedAt,
                Status = gameSession.Status.ToString(),
                Players = playerDTOs,
                Settings = new GameSettingsDTO
                {
                    TimeLimit = gameSession.TimeLimit,
                    MaxRetries = gameSession.MaxRetries,
                    ShowRealTimeScore = gameSession.ShowRealTimeScore,
                    AllowHints = gameSession.AllowHints
                }
            };

            return Result<GameSessionDTO>.Success(gameSessionDTO);
        }
    }
}