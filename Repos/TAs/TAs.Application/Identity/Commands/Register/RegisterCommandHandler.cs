using MediatR;
using Microsoft.AspNetCore.Identity;
using TAs.Application.Users.HandlerErrors;
using TAs.Domain.Constants;
using TAs.Domain.Entities;
using TAs.Domain.Errors;
using TAs.Domain.Result;
namespace TAs.Application.Identity.Commands
{
    public class RegisterCommandHandler(
        UserManager<User> _userManager
    )
    : IRequestHandler<RegisterCommand, Result>
    {
        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
                return Result.Failure(IdentityErrors.PassWordNotMatch);

            var user = new User { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return Result.Failure(new Error("RegisterFailed", string.Join("; ", result.Errors.Select(e => e.Description))));

            // Gán role mặc định là "User"
            var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.User);
            if (!roleResult.Succeeded)
                return Result.Failure(new Error("AddRoleFailed", string.Join("; ", roleResult.Errors.Select(e => e.Description))));

            return Result.Success();
        }
    }
}