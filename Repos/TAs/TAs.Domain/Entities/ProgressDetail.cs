using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class ProgressDetail : BaseEntity
    {
        public Guid ProgressId { get; set; }
        [ForeignKey(nameof(ProgressId))]
        public Progress Progress { get; set; } = null!;
        public int SentenceIndex { get; set; } // Vị trí câu trong lesson (bắt đầu từ 0 hoặc 1)
        public bool IsCompleted { get; set; } // Đã hoàn thành chưa
        public bool IsCorrect { get; set; }   // Đúng hay sai (nếu cần)
        public string? UserAnswer { get; set; } // Đáp án user nhập (nếu cần)
        public float? Score { get; set; } // Điểm từng câu (nếu cần)
        public DateTime? CompletedAt { get; set; } 
    }
}