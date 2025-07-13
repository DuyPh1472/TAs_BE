using MediatR;
using Microsoft.AspNetCore.Identity;
using TAs.Application.Users.HandlerErrors;
using TAs.Domain.Constants;
using TAs.Domain.Entities;
using TAs.Domain.Errors;
using TAs.Domain.Result;
using TAs.Application.Interfaces;
using TAs.Application.Identity.DTOs;

namespace TAs.Application.Identity.Commands
{
    public class RegisterCommandHandler(
        UserManager<User> _userManager,
        IJwtService _jwtService,
        IRefreshTokenService _refreshTokenService
    )
    : IRequestHandler<RegisterCommand, Result<AuthResultDTO>>
    {
        public async Task<Result<AuthResultDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
                return Result<AuthResultDTO>.Failure(IdentityErrors.PassWordNotMatch);
            if (9 < request.TargetScore || request.TargetScore < 4)
                return Result<AuthResultDTO>.Failure(IdentityErrors.InvalidScore);
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                Level = request.CurrentLevel,
                TargetScore = request.TargetScore
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return Result<AuthResultDTO>
                .Failure(new Error("RegisterFailed", string
                .Join("; ", result.Errors
                .Select(e => e.Description))));

            // Gán role mặc định là "User"
            var roleResult = await _userManager
            .AddToRoleAsync(user, UserRoles.User);
            if (!roleResult.Succeeded)
                return Result<AuthResultDTO>
                .Failure(new Error("AddRoleFailed", string
                .Join("; ", roleResult.Errors
                .Select(e => e.Description))));

            // Sinh access token và refresh token
            var accessToken = _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            await _refreshTokenService.SaveRefreshToken(user, refreshToken);

            var authResult = new AuthResultDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return Result<AuthResultDTO>.Success(authResult);
        }
    }
}