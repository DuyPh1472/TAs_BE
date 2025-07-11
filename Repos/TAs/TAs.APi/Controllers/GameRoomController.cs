using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAs.APi.Response;
using TAs.Application.GameRooms.Commands.Create;
using TAs.Application.GameRooms.Commands.JoinRoom;
using TAs.Application.GameRooms.Commands.LeaveRoom;
using TAs.Application.GameRooms.Queries.GetGameRoomsByRoomId;
using TAs.Application.GameRooms.DTOs.Queries;

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
                return BadRequest(new ApiResponse<Guid>(false, default, 400, result.Error.Description));
            return Ok(new ApiResponse<Guid>(true, result.Data, 201, "Room created successfully."));
        }

        [HttpPost("join")]
        public async Task<ActionResult<ApiResponse<Guid>>> Join([FromBody] JoinRoomCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<Guid>(false, default, 400, result.Error.Description));
            return Ok(new ApiResponse<Guid>(true, result.Data, 200, "Joined room successfully."));
        }

        [HttpPost("leave")]
        public async Task<ActionResult<ApiResponse<Guid>>> Leave([FromBody] LeaveRoomCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<Guid>(false, default, 400, result.Error.Description));
            return Ok(new ApiResponse<Guid>(true, result.Data, 200, "Left room successfully."));
        }

        [HttpGet("{roomId}")]
        public async Task<ActionResult<ApiResponse<GetRoomDetailsDTO>>> GetRoomDetails(Guid roomId)
        {
            var query = new GetGameRoomByRoomIdQuery { RoomId = roomId };
            var result = await mediator.Send(query);
            
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<GetRoomDetailsDTO>(false, default, 400, result.Error.Description));
            
            return Ok(new ApiResponse<GetRoomDetailsDTO>(true, result.Data, 200, "Room retrieved successfully."));
        }
    }
}