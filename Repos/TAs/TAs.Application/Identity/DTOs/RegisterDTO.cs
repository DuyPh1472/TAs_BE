using TAs.Domain.Enums;

namespace TAs.Application.Identity.DTOs
{
    public class RegisterDTO
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public UserLevelEnum CurrentLevel { get; set; }
        public float TargetScore { get; set; }
    }
}