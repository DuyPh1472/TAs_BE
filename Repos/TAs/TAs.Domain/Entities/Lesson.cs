using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class Lesson : BaseEntity
    {
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
        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
        public ICollection<DictationSentence> DictationSentences { get; set; } = [];
        public ICollection<Progress> Progresses = [];

    }
}