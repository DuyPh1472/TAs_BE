using TAs.Domain.Errors;

namespace TAs.Application.Users.HandlerErrors
{
    public static class IdentityErrors
    {
        public static readonly Error PassWordNotMatch = new("PasswordMismatch", "Passwords do not match.");
        public static Error IdNotFound(Guid Id)
         => new("Error not found", $"Skill with Id: {Id} does not exist.");
        public static readonly Error UserNotFound = new("UserNotFound", "User not found. ");
        public static readonly Error LoginFailed = new("LoginFailed", "Invalid credentials");
        public static Error EmailNotExist(string Email) => new("EmailNotExist", $"Email:{Email} not found.");
        public static Error RoleNotExist(string Role) => new("RoleNotExist", $"Role:{Role} does not exist.");

    }
}