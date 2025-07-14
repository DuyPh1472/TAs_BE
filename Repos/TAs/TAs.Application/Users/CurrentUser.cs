namespace TAs.Application.Users
{
    public record CurrentUser(Guid Id, string Email, string UserName,IEnumerable<string> Roles)
    {
        public bool IsRole(string role) => Roles.Contains(role);
    }
}