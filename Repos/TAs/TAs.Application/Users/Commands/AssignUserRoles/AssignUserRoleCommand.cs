using MediatR;
using TAs.Domain.Result;

namespace TAs.Application.Users.Commands.AssignUserRoles
{
    public class AssignUserRoleCommand : IRequest<Result>
    {
        public string Email { get; set; } = default!;
        public string RoleName { get; set; } = default!;
    }
}