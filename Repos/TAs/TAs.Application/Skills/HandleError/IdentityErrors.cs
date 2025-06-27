using TAs.Domain.Errors;

namespace TAs.Application.Skills.HandleError
{
    public static class IdentityErrors
    {
        public static readonly Error PassWordNotMatch = new("PasswordMismatch", "Passwords do not match.");
        public static Error IdNotFound(Guid Id)
         => new("Lỗi không tìm thấy", $"Skill với Id: {Id} không tồn tại.");
        public static readonly Error UserNotFound = new("UserNotFound", "User not found. ");
        public static readonly Error LoginFailed = new("LoginFailed", "Invalid credentials");
    }
}