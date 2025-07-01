namespace TAs.Application.ProgressApplication.DTOs.Queries.GetAll
{
    public class GetAllProgressDTO
    {
        public Guid ProgressId { get; set; }
        public List<int> SentenceIndex { get; set; } = [];
        public List<bool> IsCompleted { get; set; } = [];
        public List<DateTime>? CompletedAt { get; set; }
    }
}