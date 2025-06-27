using TAs.Domain.Errors;

namespace TAs.Application.Skills.HandleError
{
    public static class SkillErrors
    {
        public static readonly Error SameSkill = new("Lỗi trùng lặp", "Tên Skill hay Path đã tồn tại.");
        public static Error IdNotFound(Guid Id)
         => new("Lỗi không tìm thấy", $"Skill với Id: {Id} không tồn tại.");
    }
}