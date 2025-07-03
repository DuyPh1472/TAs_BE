namespace TAs.Application.Lessons.DTOs
{
    public class LessonDTO
    {
        public Guid LessonId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Sentences { get; set; } = string.Empty;
        public string Accent { get; set; } = string.Empty;
        public float Duration { get; set; }
        public string Topics { get; set; } = string.Empty;
        public string? AudioUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public string? VideoId { get; set; }
        public string? VideoTitle { get; set; }
        public string? VocabLevel { get; set; }
        public string? SpeechToTextLangCode { get; set; }
        public List<DictationSentenceDTO> Challenges { get; set; } = new();
    }

    public class DictationSentenceDTO
    {
        public Guid Id { get; set; }
        public int Position { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? AudioSrc { get; set; }
        public float TimeStart { get; set; }
        public float TimeEnd { get; set; }
        public List<List<string>> Solution { get; set; } = new();
        public string? Hint { get; set; }
        public List<string> Hints { get; set; } = new();
        public string? Explanation { get; set; }
        public bool AlwaysShowExplanation { get; set; }
        public int NbComments { get; set; }
    }
}