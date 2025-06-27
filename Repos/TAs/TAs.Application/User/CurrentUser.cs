namespace TAs.Application.User
{
    public record CurrentUser(Guid Id, string Email, IEnumerable<string> Roles)
    {
        public bool IsRole(string role) => Roles.Contains(role);
    }
}