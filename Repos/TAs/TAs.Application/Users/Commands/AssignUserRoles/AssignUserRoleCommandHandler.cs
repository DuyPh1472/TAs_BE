using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TAs.Application.Skills.HandleError;
using TAs.Domain.Entities;
using TAs.Domain.Result;

namespace TAs.Application.Users.Commands.AssignUserRoles
{
    public class AssignUserRoleCommandHandler(
        ILogger<AssignUserRoleCommand> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager
    ) : IRequestHandler<AssignUserRoleCommand, Result>
    {
        public async Task<Result> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Assigning user role: {@request}", request);
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Failure(IdentityErrors.EmailNotExist(request.Email));
            var role = await roleManager.FindByNameAsync(request.RoleName);
            if (role is null)
                return Result.Failure(IdentityErrors.RoleNotExist(request.RoleName));
            var currentRoles = await userManager.GetRolesAsync(user);
            if (currentRoles.Any())
                await userManager.RemoveFromRolesAsync(user, currentRoles);
            await userManager.AddToRoleAsync(user, role.Name!);
            return Result.Success();

        }
    }
}