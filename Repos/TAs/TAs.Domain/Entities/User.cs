using Microsoft.AspNetCore.Identity;
namespace TAs.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public DateOnly DateOfBirth { get; set; }
        public string Avatar { get; set; } = default!;
        public string Region { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}