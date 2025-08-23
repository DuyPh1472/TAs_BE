using MediatR;
using TAs.Domain.Entities;
using TAs.Domain.Enums;
using Microsoft.Extensions.Logging;
using TAs.Application.Interfaces;
using TAs.Application.Users;

namespace TAs.Application.GameRooms.Commands.GameSession.SaveGameResult
{
    public class SaveGameResultCommandHandler : IRequestHandler<SaveGameResultCommand, SaveGameResultResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SaveGameResultCommandHandler> _logger;
        private readonly IUserContext _userContext;

        public SaveGameResultCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<SaveGameResultCommandHandler> logger,
            IUserContext userContext)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userContext = userContext;
        }

        public async Task<SaveGameResultResponse> Handle(SaveGameResultCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            try
            {
                _logger.LogInformation("Saving game result for room {RoomId}", request.RoomId);

                // 1. Tạo GameSession
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
                    CreatedBy = currentUser!.Id
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
                        CreatedBy = currentUser!.Id

                    };

                    playerScores.Add(playerScore);
                }

                foreach (var playerScore in playerScores)
                {
                    _unitOfWork.PlayerScoreRepository.Add(playerScore);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully saved game result. GameSessionId: {GameSessionId}, Players: {PlayerCount}",
                    gameSession.Id, playerScores.Count);

                return new SaveGameResultResponse
                {
                    Success = true,
                    Message = "Game result saved successfully",
                    GameSessionId = gameSession.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving game result for room {RoomId}", request.RoomId);
                return new SaveGameResultResponse
                {
                    Success = false,
                    Message = $"Error saving game result: {ex.Message}"
                };
            }
        }
    }
}