namespace TAs.Application.ProgressApplication.DTOs.Queries.GetByIdAndSentence
{
    public class GetProgressByIdAndSentenceIndexDTO
    {
        public Guid ProgressId { get; set; }
        public int SentenceIndex { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}