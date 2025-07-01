namespace TAs.Application.ProgressApplication.DTOs
{
    public class ProgressDetailDTO
    {
        public int SentenceIndex { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
} 