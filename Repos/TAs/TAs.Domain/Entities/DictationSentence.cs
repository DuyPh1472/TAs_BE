using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class DictationSentence : BaseEntity
    {
        public Guid LessonId { get; set; }
        public string Text { get; set; } = string.Empty;
        public float StartTime { get; set; } 
        public float EndTime { get; set; }   
        public string? AudioUrl { get; set; } 
        public int Position { get; set; }

        public string? Hint { get; set; }
        public string? Explanation { get; set; }
        public bool AlwaysShowExplanation { get; set; } = true;
        public int NbComments { get; set; } = 0;
        public string? JsonContent { get; set; } 
        public string? Solution { get; set; } 

        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; set; } = null!;
    }
}