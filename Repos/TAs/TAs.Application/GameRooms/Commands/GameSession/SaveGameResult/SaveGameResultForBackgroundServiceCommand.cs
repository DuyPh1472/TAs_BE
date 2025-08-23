using MediatR;
using TAs.Domain.Entities;
using TAs.Domain.Enums;
using Microsoft.Extensions.Logging;
using TAs.Application.Interfaces;

namespace TAs.Application.GameRooms.Commands.GameSession.SaveGameResult
{
    public class SaveGameResultForBackgroundServiceCommand : IRequest<SaveGameResultResponse>
    {
        public Guid RoomId { get; set; }
        public Guid LessonId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
        public int TotalSentences { get; set; }
        public int TimeLimit { get; set; }
        public List<PlayerGameResult> PlayerResults { get; set; } = new();
    }

    public class SaveGameResultForBackgroundServiceCommandHandler : IRequestHandler<SaveGameResultForBackgroundServiceCommand, SaveGameResultResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SaveGameResultForBackgroundServiceCommandHandler> _logger;

        public SaveGameResultForBackgroundServiceCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<SaveGameResultForBackgroundServiceCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<SaveGameResultResponse> Handle(SaveGameResultForBackgroundServiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Saving game result for room {RoomId} (Background Service)", request.RoomId);

                // 1. Tạo GameSession - không cần CreatedBy vì đây là auto-save
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
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Guid.Empty // Không có user context trong background service
                };

                _unitOfWork.GameSessionRepository.Add(gameSession);

                // 2. Lưu PlayerScore cho từng player
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
                        CreatedBy = Guid.Empty // Không có user context trong background service
                    };

                    playerScores.Add(playerScore);
                }

                foreach (var playerScore in playerScores)
                {
                    _unitOfWork.PlayerScoreRepository.Add(playerScore);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully saved game result (Background Service). GameSessionId: {GameSessionId}, Players: {PlayerCount}",
                    gameSession.Id, playerScores.Count);

                return new SaveGameResultResponse
                {
                    Success = true,
                    Message = "Game result saved successfully by background service",
                    GameSessionId = gameSession.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving game result for room {RoomId} (Background Service)", request.RoomId);
                return new SaveGameResultResponse
                {
                    Success = false,
                    Message = $"Error saving game result: {ex.Message}"
                };
            }
        }
    }
} 