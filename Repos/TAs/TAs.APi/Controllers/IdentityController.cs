using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAs.APi.Response;
using TAs.Application.Identity.Commands.Login;
using TAs.Application.Identity.Commands.Register;
using TAs.Application.Identity.DTOs;
using TAs.Application.Users.Commands.AssignUserRoles;
using TAs.Application.Users.UserDetail;
using TAs.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using TAs.Application.Interfaces;
using TAs.Domain.Entities;

namespace TAs.APi.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResultDTO>>> Register([FromBody] RegisterCommand dto)
        {
            var result = await mediator.Send(dto);
            if (!result.IsSuccess)
                return BadRequest(
                    new ApiResponse<AuthResultDTO>(
                        false, null, 400, result.Error.Description));
            return Ok(
                new ApiResponse<AuthResultDTO>(
                    true, result.Data, 200, "Đăng ký thành công"));
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResultDTO>>> Login([FromBody] LoginCommand dto)
        {
            var result = await mediator.Send(dto);
            if (!result.IsSuccess)
                return Unauthorized(
                    new ApiResponse<AuthResultDTO>(
                        false, null, 401, result.Error.Description));
            return Ok(
                new ApiResponse<AuthResultDTO>(
                    true, result.Data, 200, "Đăng nhập thành công"));
        }

        [HttpPatch("user")]
        [Authorize(Roles = UserRoles.User)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> UpdateUserDetail([FromBody] UserDetailCommand request)
        {
            var command = await mediator.Send(request);
            if (!command.IsSuccess)
                return NotFound(
                    new ApiResponse<object>(
                        false, null, 404, command.Error.Description));
            return Ok(
                new ApiResponse<object>(
                    true, null, 200, "Cập nhật thành công"));
        }

        [HttpPost("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<ApiResponse<object>>> AssignUserRole(AssignUserRoleCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return NotFound(
                    new ApiResponse<object>(
                        false, null, 404, result.Error.Description));
            return Ok(
                new ApiResponse<object>(
                    true, null, 200, "Cập nhật thành công"));

        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AuthResultDTO>>> RefreshToken([FromBody] RefreshTokenRequest request, [FromServices] IJwtService jwtService, [FromServices] IRefreshTokenService refreshTokenService, [FromServices] UserManager<User> userManager)
        {
            // 1. Tìm refresh token trong DB
            var refreshTokenEntity = await refreshTokenService.GetRefreshTokenAsync(request.RefreshToken);
            if (refreshTokenEntity == null || refreshTokenEntity.IsUsed || refreshTokenEntity.IsRevoked || refreshTokenEntity.Expires < DateTime.UtcNow)
            {
                return Unauthorized(new ApiResponse<AuthResultDTO>(false, null, 401, "Refresh token is invalid or expired"));
            }
            // 2. Lấy user
            var user = await userManager.FindByIdAsync(refreshTokenEntity.UserId.ToString());
            if (user == null)
            {
                return Unauthorized(new ApiResponse<AuthResultDTO>(false, null, 401, "User not found"));
            }
            // 3. Đánh dấu refresh token cũ đã dùng
            await refreshTokenService.MarkRefreshTokenAsUsed(refreshTokenEntity);
            // 4. Sinh access token mới và refresh token mới
            var accessToken = jwtService.GenerateJwtToken(user);
            var newRefreshToken = jwtService.GenerateRefreshToken();
            await refreshTokenService.SaveRefreshToken(user, newRefreshToken);
            // 5. Trả về
            var result = new AuthResultDTO
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
            return Ok(new ApiResponse<AuthResultDTO>(true, result, 200, "Token refreshed successfully"));
        }
    }
}