namespace TAs.Application.Lessons.DTOs
{
    public class GetAllLessonDTO
    {
        public Guid LessonId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Sentences { get; set; } = string.Empty;
        public string Accent { get; set; } = string.Empty;
        public float Duration { get; set; }
        public string Topics { get; set; } = string.Empty;
        public string? AudioUrl { get; set; } // audio tổng (nếu có)
        public string? YoutubeUrl { get; set; } // link youtube (nếu có)
        public string? VideoId { get; set; } // id youtube (nếu có)
    }
}