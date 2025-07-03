namespace TAs.Application.ProgressApplication.DTOs.Queries.GetAll
{
    public class GetAllProgressDTO
    {
        public Guid ProgressId { get; set; }
        public Guid LessonId { get; set; }
        public Guid UserId { get; set; }
        public int CompletedSentences { get; set; }
        public int TotalSentences { get; set; }
        public double Score { get; set; }
        public string Status { get; set; } = string.Empty;
        public int CurrentSentence { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        public DateTimeOffset? LastUpdatedAt { get; set; }
    }
}