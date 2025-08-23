using MediatR;
using TAs.Domain.Entities;
using TAs.Domain.Enums;
using Microsoft.Extensions.Logging;
using TAs.Application.Interfaces;

namespace TAs.Application.GameRooms.Commands.GameSession.SaveGameResult
{
    public class SaveCompleteGameResultCommand : IRequest<SaveGameResultResponse>
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public Guid HostId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid LessonId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
        public int TotalSentences { get; set; }
        public int TimeLimit { get; set; }
        public int MaxRetries { get; set; }
        public bool ShowRealTimeScore { get; set; }
        public bool AllowHints { get; set; }
        public string LessonSelection { get; set; } = "host_choice";
        public List<PlayerGameResult> PlayerResults { get; set; } = new();
    }

    public class SaveCompleteGameResultCommandHandler : IRequestHandler<SaveCompleteGameResultCommand, SaveGameResultResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SaveCompleteGameResultCommandHandler> _logger;

        public SaveCompleteGameResultCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<SaveCompleteGameResultCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<SaveGameResultResponse> Handle(SaveCompleteGameResultCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Saving complete game result for room {RoomId} (Background Service)", request.RoomId);

                // 1. Tạo GameRoom trước (nếu chưa tồn tại)
                var existingRoom = await _unitOfWork.GameRoomRepository.GetGameRoomByRoomId(request.RoomId);
                if (existingRoom == null)
                {
                    var gameRoom = new Domain.Entities.GameRoom
                    {
                        Id = request.RoomId,
                        HostId = request.HostId,
                        RoomName = request.RoomName,
                        IsActive = false, // Game đã kết thúc
                        Status = GameStatus.Finished,
                        MaxPlayers = 4, // Default value
                        SelectedLessonId = request.LessonId,
                        CurrentSentence = request.TotalSentences,
                        CategoryId = request.CategoryId,
                        TimeLimit = request.TimeLimit,
                        MaxRetries = request.MaxRetries,
                        ShowRealTimeScore = request.ShowRealTimeScore,
                        AllowHints = request.AllowHints,
                        LessonSelection = request.LessonSelection,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = Guid.Empty // Background service
                    };

                    _unitOfWork.GameRoomRepository.Add(gameRoom);
                    _logger.LogInformation("Created new GameRoom record for room {RoomId}", request.RoomId);
                }
                else
                {
                    // Cập nhật trạng thái phòng nếu đã tồn tại
                    existingRoom.Status = GameStatus.Finished;
                    existingRoom.IsActive = false;
                    _unitOfWork.GameRoomRepository.Update(existingRoom);
                    _logger.LogInformation("Updated existing GameRoom status for room {RoomId}", request.RoomId);
                }

                // 2. Tạo GameSession
                var gameSession = new Domain.Entities.GameSession
                {
                    Id = Guid.NewGuid(),
                    RoomId = request.RoomId,
                    LessonId = request.LessonId,
                    StartedAt = request.StartedAt,
                    EndedAt = request.EndedAt,
                    TotalSentences = request.TotalSentences,
                    CurrentSentence = request.TotalSentences, // Game đã kết thúc
                    Status = GameStatus.Finished,
                    TimeLimit = request.TimeLimit,
                    MaxRetries = request.MaxRetries,
                    ShowRealTimeScore = request.ShowRealTimeScore,
                    AllowHints = request.AllowHints,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Guid.Empty // Background service
                };

                _unitOfWork.GameSessionRepository.Add(gameSession);

                // 3. Lưu PlayerScore cho từng player
                var playerScores = new List<PlayerScore>();
                foreach (var playerResult in request.PlayerResults)
                {
                    var playerScore = new Domain.Entities.PlayerScore
                    {
                        Id = Guid.NewGuid(),
                        RoomId = request.RoomId,
                        UserId = playerResult.UserId,
                        Score = playerResult.FinalScore,
                        TotalSentences = request.TotalSentences,
                        CorrectAnswers = playerResult.CorrectAnswers,
                        IncorrectAnswers = playerResult.IncorrectAnswers,
                        AverageTimePerSentence = playerResult.AverageTimePerSentence,
                        TotalTimeSpent = playerResult.TotalTimeSpent,
                        TotalRetries = playerResult.TotalRetries,
                        CompletedAt = request.EndedAt,
                        GameSessionId = gameSession.Id.ToString(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = Guid.Empty // Background service
                    };

                    playerScores.Add(playerScore);
                }

                foreach (var playerScore in playerScores)
                {
                    _unitOfWork.PlayerScoreRepository.Add(playerScore);
                }

                // Lưu tất cả thay đổi
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully saved complete game result (Background Service). GameSessionId: {GameSessionId}, Players: {PlayerCount}",
                    gameSession.Id, playerScores.Count);

                return new SaveGameResultResponse
                {
                    Success = true,
                    Message = "Complete game result saved successfully by background service",
                    GameSessionId = gameSession.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving complete game result for room {RoomId} (Background Service)", request.RoomId);
                return new SaveGameResultResponse
                {
                    Success = false,
                    Message = $"Error saving complete game result: {ex.Message}"
                };
            }
        }
    }
} 