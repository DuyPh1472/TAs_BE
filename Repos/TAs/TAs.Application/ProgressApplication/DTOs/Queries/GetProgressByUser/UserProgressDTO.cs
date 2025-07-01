using TAs.Domain.Entities;

namespace TAs.Application.ProgressApplication.DTOs.Queries.GetProgressByUser
{
    public class UserProgressDTO : BaseEntity
    {
        public Guid ProgressId { get; set; }
        public Guid LessonId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public int TotalChallenge { get; set; }
        public int ProgressChallenge { get; set; }
        public float Score { get; set; }
        public bool ProgressStatus { get; set; }
    }
}