namespace TAs.Application.ProgressApplication.DTOs.Commands
{
    public class UpdateProgressDTO
    {
        public Guid LessonId { get; set; }
        public int SentenceIndex { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCorrect { get; set; }
        public string UserAnswer { get; set; } = string.Empty;
    }
}