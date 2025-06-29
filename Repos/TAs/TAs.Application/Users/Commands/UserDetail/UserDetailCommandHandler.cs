using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TAs.Application.Users.HandlerErrors;
using TAs.Domain.Entities;
using TAs.Domain.Result;

namespace TAs.Application.Users.UserDetail
{
    public class UserDetailCommandHandler(
        ILogger<UserDetailCommandHandler> _logger,
        IUserContext _userContext,
        IUserStore<User> _userStore)
    : IRequestHandler<UserDetailCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(UserDetailCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            _logger.LogInformation("Updating user: {userId}, with @{Request}", user!.Id, request);
            var dbUser = await _userStore.FindByIdAsync(user.Id.ToString(), cancellationToken);
            if (dbUser is null)
                return Result<object>.Failure(IdentityErrors.IdNotFound(dbUser!.Id));
            dbUser.Avatar = request.Avatar;
            dbUser.DateOfBirth = request.DateOfBirth;
            dbUser.Region = request.Region;
            dbUser.UpdatedAt = DateTime.UtcNow;
            dbUser.UpdatedBy = user.Id;
            await _userStore.UpdateAsync(dbUser, cancellationToken);
            return Result<object>.Success(null!);
        }
    }
}