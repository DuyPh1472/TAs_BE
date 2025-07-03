using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class Progress : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid LessonId { get; set; }
        // Tổng số challenge/câu hỏi của lesson (lấy từ Lesson.TotalChallenge)
        public int TotalChallenge { get; set; }
        public int ProgressChallenge { get; set; }
        public float Score { get; set; }
        public bool ProgressStatus { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; set; } = null!;
        public ICollection<ProgressDetail> ProgressDetails { get; set; } = [];
    }
}