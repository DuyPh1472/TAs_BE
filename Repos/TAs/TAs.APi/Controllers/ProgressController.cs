using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAs.APi.Response;
using TAs.Application.ProgressApplication.Commands.Update;

namespace TAs.APi.Controllers
{
    [ApiController]
    [Route("api/progress")]
    [Authorize]
    public class ProgressController(IMediator mediator) : ControllerBase
    {
        [HttpPost("update")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateProgress([FromBody] UpdateProgressCommand request)
        {
            var command = await mediator.Send( request);
            if (!command.IsSuccess)
            {
                return NotFound(new ApiResponse<object>
                (false, null, 404, command.Error.Description));
            }
            return Created();
        }
    }
}