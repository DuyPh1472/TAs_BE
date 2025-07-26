using MediatR;
using Microsoft.AspNetCore.Identity;
using TAs.Domain.Entities;
using TAs.Domain.Result;
using TAs.Application.Users.HandlerErrors;
using TAs.Application.Identity.DTOs;
using TAs.Application.Interfaces;

namespace TAs.Application.Identity.Commands.Login
{
    public class LoginCommandHandler(
        SignInManager<User> _signInManager,
        UserManager<User> _userManager,
        IJwtService _jwtService,
        IRefreshTokenService _refreshTokenService
    )
    : IRequestHandler<LoginCommand, Result<AuthResultDTO>>
    {
        public async Task<Result<AuthResultDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result<AuthResultDTO>.Failure(IdentityErrors.UserNotFound);
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                return Result<AuthResultDTO>.Failure(IdentityErrors.LoginFailed);
            // Generate tokens
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