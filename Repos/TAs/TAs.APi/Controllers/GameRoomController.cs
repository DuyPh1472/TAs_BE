using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAs.APi.Response;
using TAs.Application.GameRooms.Commands.Create;

namespace TAs.APi.Controllers
{
    [ApiController]
    [Route("api/GameRooms")]
    [Authorize]
    public class GameRoomController(IMediator mediator) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse<Guid>>> Create([FromBody] CreateRoomCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Error.Description);
            return Ok(new ApiResponse<Guid>(true, result.Data, 201, "Room created successfully."));
        }
    }
} 