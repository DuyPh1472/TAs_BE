using MediatR;
using Microsoft.AspNetCore.Mvc;
using TAs.APi.ApiResponse;
using TAs.Application.Identity.Commands;
using TAs.Application.Identity.Commands.Login;

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
                return BadRequest(new ApiResponse<object>(false, null, 400, result.Error.Description));
            return Ok(new ApiResponse<object>(true, null, 200, "Đăng ký thành công"));
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] LoginCommand dto)
        {
            var result = await mediator.Send(dto);
            if (!result.IsSuccess)
                return Unauthorized
                (new ApiResponse<string>(false, null, 401, result.Error.Description));
            return Ok(new ApiResponse<string>(true, result.Data, 200, "Đăng nhập thành công"));
        }
    }
}