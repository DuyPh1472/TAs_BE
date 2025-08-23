using MediatR;
using TAs.Domain.Entities;

namespace TAs.Application.GameRooms.Commands.GameSession.SaveGameResult
{
    public class SaveGameResultCommand : IRequest<SaveGameResultResponse>
    {
        public Guid RoomId { get; set; }
        public Guid LessonId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
        public int TotalSentences { get; set; }
        public int TimeLimit { get; set; }
        public List<PlayerGameResult> PlayerResults { get; set; } = new();
    }

    public class PlayerGameResult
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int FinalScore { get; set; }
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }
        public int TotalTimeSpent { get; set; } // seconds
        public int TotalRetries { get; set; }
        public float AverageTimePerSentence { get; set; }
    }

    public class SaveGameResultResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Guid? GameSessionId { get; set; }
    }
} 