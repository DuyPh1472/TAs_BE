using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAs.APi.ApiResponse;
using TAs.Application.Identity.Commands;
using TAs.Application.Identity.Commands.Login;
using TAs.Application.Users.Commands.AssignUserRoles;
using TAs.Application.Users.UserDetail;
using TAs.Domain.Constants;

namespace TAs.APi.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<object>>> Register([FromBody] RegisterCommand dto)
        {
            var result = await mediator.Send(dto);
            if (!result.IsSuccess)
                return BadRequest(
                    new ApiResponse<object>(
                        false, null, 400, result.Error.Description));
            return Ok(
                new ApiResponse<object>(
                    true, null, 200, "Đăng ký thành công"));
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] LoginCommand dto)
        {
            var result = await mediator.Send(dto);
            if (!result.IsSuccess)
                return Unauthorized(
                    new ApiResponse<string>(
                        false, null, 401, result.Error.Description));
            return Ok(
                new ApiResponse<string>(
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
    }
}