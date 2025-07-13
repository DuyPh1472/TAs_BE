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
using TAs.Application.GameRooms.Commands.Update.UpdateRoomSettings;
using TAs.Application.GameRooms.Commands.GameSession.StartGameSession;
using TAs.Application.GameRooms.Queries.GetActiveRooms;
using TAs.Application.GameRooms.Queries.GetRoomPlayers;

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

        [HttpPatch("{roomId}/select-lesson")]
        public async Task<ActionResult<ApiResponse<bool>>> SelectLesson(Guid roomId, [FromBody] SelectLessonRequest request)
        {
            var result = await mediator.Send(new SelectLessonCommand(roomId, request.LessonId));
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<bool>(false, false, 400, result.Error.Description));
            return Ok(new ApiResponse<bool>(true, true, 200, "Lesson selected successfully."));
        }

        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<List<GameRoomListDTO>>>> GetActiveRooms([FromQuery] string? searchTerm, [FromQuery] Guid? categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var query = new GetActiveRoomsQuery { SearchTerm = searchTerm, CategoryId = categoryId, Page = page, PageSize = pageSize };
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<List<GameRoomListDTO>>(false, default, 400, result.Error.Description));
            return Ok(new ApiResponse<List<GameRoomListDTO>>(true, result.Data, 200, "Active rooms retrieved successfully."));
        }

        [HttpGet("{roomId}/players")]
        public async Task<ActionResult<ApiResponse<List<PlayerDTO>>>> GetRoomPlayers(Guid roomId)
        {
            var query = new GetRoomPlayersQuery { RoomId = roomId };
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<List<PlayerDTO>>(false, default, 400, result.Error.Description));
            return Ok(new ApiResponse<List<PlayerDTO>>(true, result.Data, 200, "Room players retrieved successfully."));
        }

        [HttpPut("{roomId}/settings")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateRoomSettings(Guid roomId, [FromBody] UpdateRoomSettingsRequest request)
        {
            var command = new UpdateRoomSettingsCommand 
            { 
                RoomId = roomId,
                TimeLimit = request.TimeLimit,
                MaxRetries = request.MaxRetries,
                ShowRealTimeScore = request.ShowRealTimeScore,
   
            };
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<bool>(false, false, 400, result.Error.Description));
            return Ok(new ApiResponse<bool>(true, true, 200, "Room settings updated successfully."));
        }

        [HttpPost("{roomId}/game/start")]
        public async Task<ActionResult<ApiResponse<GameSessionDTO>>> StartGameSession(Guid roomId, [FromBody] StartGameSessionRequest request)
        {
            var command = new StartGameSessionCommand { RoomId = roomId, LessonId = request.LessonId };
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<GameSessionDTO>(false, default, 400, result.Error.Description));
            return Ok(new ApiResponse<GameSessionDTO>(true, result.Data, 200, "Game session started successfully."));
        }
    }
}