using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using TAs.Domain.Entities;
using TAs.Domain.Result;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TAs.Application.Users.HandlerErrors;

namespace TAs.Application.Identity.Commands.Login
{
    public class LoginCommandHandler(
        IConfiguration _config,
        SignInManager<User> _signInManager,
        UserManager<User> _userManager

    )
    : IRequestHandler<LoginCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result<string>.Failure(IdentityErrors.UserNotFound);
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                return Result<string>.Failure(IdentityErrors.LoginFailed);
            //Sign JWT
            var token = await GenerateJwtToken(user);
            return Result<string>.Success(token);
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? "")
            };

            // Add role claims
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}