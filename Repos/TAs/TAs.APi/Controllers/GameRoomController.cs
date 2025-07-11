using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAs.APi.Response;
using TAs.Application.GameRooms.Commands.Create;
using TAs.Application.GameRooms.Commands.JoinRoom;
using TAs.Application.GameRooms.Commands.LeaveRoom;
using TAs.Application.GameRooms.Queries.GetGameRoomsByRoomId;
using TAs.Application.GameRooms.DTOs.Queries;
using TAs.Application.GameRooms.DTOs.Commands;
using TAs.Application.GameRooms.Commands.Update.SelectLesson;
using TAs.Application.GameRooms.Commands.Update.CheckStatus;
using TAs.Application.GameRooms.Commands.Update.StartGame;

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

        [HttpPatch("{roomId}/ready")]
        public async Task<ActionResult<ApiResponse<UpdateStatusRoomDTO>>> ToggleReady(Guid roomId)
        {
            var result = await mediator.Send(new CheckStatusGameRoomCommand(roomId));
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<UpdateStatusRoomDTO>(false, default, 400, result.Error.Description));
            return Ok(new ApiResponse<UpdateStatusRoomDTO>(true, result.Data, 200, "Toggled ready status successfully."));
        }

        [HttpPost("{roomId}/start")]
        public async Task<ActionResult<ApiResponse<StartGameRoomDTO>>> StartGame(Guid roomId)
        {
            var result = await mediator.Send(new StartGameRoomCommand(roomId));
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<StartGameRoomDTO>(false, default, 400, result.Error.Description));
            return Ok(new ApiResponse<StartGameRoomDTO>(true, result.Data, 200, "Game started successfully."));
        }

        [HttpPatch("{roomId}/select-lesson")]
        public async Task<ActionResult<ApiResponse<bool>>> SelectLesson(Guid roomId, [FromBody] SelectLessonRequest request)
        {
            var result = await mediator.Send(new SelectLessonCommand(roomId, request.LessonId));
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<bool>(false, false, 400, result.Error.Description));
            return Ok(new ApiResponse<bool>(true, true, 200, "Lesson selected successfully."));
        }
    }
}