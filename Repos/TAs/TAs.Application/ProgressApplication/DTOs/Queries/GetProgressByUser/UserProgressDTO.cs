using TAs.Domain.Entities;

namespace TAs.Application.ProgressApplication.DTOs.Queries.GetProgressByUser
{
    public class UserProgressDTO : BaseEntity
    {
        public Guid ProgressId { get; set; }
        public Guid LessonId { get; set; }
        public Guid UserId { get; set; }
        public int CurrentSentence { get; set; }
        public int CompletedSentences { get; set; }
        public int TotalSentences { get; set; }
        public double Score { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        public DateTimeOffset? LastUpdatedAt { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public int TotalChallenge { get; set; }
        public int ProgressChallenge { get; set; }
        public bool ProgressStatus { get; set; }
    }
}