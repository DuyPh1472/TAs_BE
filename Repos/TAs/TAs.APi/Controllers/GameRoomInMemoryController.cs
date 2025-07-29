using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MediatR;
using TAs.APi.Multiplayer;
using Microsoft.AspNetCore.Authorization;
using TAs.Application.GameRooms.Queries;
using TAs.Application.GameRooms.Commands.CreateGameRoomInMemory;
using TAs.Application.GameRooms.Commands.JoinRoomInMemory;
using TAs.Application.GameRooms.Commands.SelectLessonInMemory;
using TAs.Application.GameRooms.Commands.ReadyInMemory;
using TAs.Application.GameRooms.Commands.KickPlayerInMemory;

namespace TAs.APi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameRoomInMemoryController(IMediator mediator, IHubContext<GameRoomHub> hubContext) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IHubContext<GameRoomHub> _hubContext = hubContext;

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveRooms()
        {
            var rooms = await _mediator.Send(new GetActiveRoomsQuery());
            return Ok(new
            {
                success = true,
                data = rooms,
                message = "Active rooms retrieved successfully"
            });
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetRoomDetails(string roomId)
        {
            if (!Guid.TryParse(roomId, out var guid))
            {
                return BadRequest(new { success = false, message = "Invalid room ID format" });
            }
            var roomDetails = await _mediator.Send(new GetRoomDetailsQuery { RoomId = guid });
            if (roomDetails == null)
            {
                return NotFound(new { success = false, message = "Room not found" });
            }
            return Ok(new
            {
                success = true,
                data = roomDetails,
                message = "Room details retrieved successfully"
            });
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateRoom([FromBody] CreateGameRoomCommand command)
        {
            // command.CategoryId is now a Guid string from client
            var roomId = await _mediator.Send(command);
            // Lấy thông tin room vừa tạo
            var roomDetails = await _mediator.Send(new GetRoomDetailsQuery { RoomId = roomId });
            // Broadcast SignalR cho tất cả client
            await _hubContext.Clients.All.SendAsync("RoomCreated", roomDetails);
            return Ok(new
            {
                success = true,
                data = roomId.ToString(),
                message = "Room created successfully"
            });
        }

        [HttpPost("{roomId}/join")]
        [Authorize]
        public async Task<IActionResult> JoinRoom(string roomId)
        {
            if (!Guid.TryParse(roomId, out var guid))
            {
                return BadRequest(new { success = false, message = "Invalid room ID format" });
            }
            var result = await _mediator.Send(new JoinGameRoomCommand { RoomId = guid });
            if (result.Success)
            {
                // Thêm user vào group SignalR
                var userId = User?.Identity?.Name ?? result.RoomId.ToString();
                var connectionId = HttpContext.Request.Headers["X-SignalR-ConnectionId"].FirstOrDefault();
                if (!string.IsNullOrEmpty(connectionId))
                {
                    await _hubContext.Groups.AddToGroupAsync(connectionId, roomId);
                }
                return Ok(new { success = true, roomId = result.RoomId, message = "Join request sent successfully" });
            }
            if (result.Message == "already in this room")
            {
                return Ok(new { success = false, message = result.Message, roomId = result.RoomId });
            }
            return BadRequest(new { success = false, message = result.Message ?? "Join room failed" });
        }

        [HttpPost("{roomId}/select-lesson")]
        [Authorize]
        public async Task<IActionResult> SelectLesson(string roomId, [FromBody] SelectLessonRequest request)
        {
            if (!Guid.TryParse(roomId, out var guid))
            {
                return BadRequest(new { success = false, message = "Invalid room ID format" });
            }
            var result = await _mediator.Send(new SelectLessonInMemoryCommand
            {
                RoomId = guid,
                LessonId = request.LessonId
            });
            if (result.Success && result.Lesson != null)
            {
                // Gửi SignalR ở đây
                await _hubContext.Clients.Group(roomId).SendAsync(
                    "LessonSelected",
                    result.Lesson.LessonId, 
                    result.Lesson.Title,
                    new {
                        result.Lesson.LessonId,
                        result.Lesson.Title,
                        result.Lesson.Description,
                        result.Lesson.Level,
                        result.Lesson.Accent,
                        result.Lesson.Duration
                    }
                );
                return Ok(new { success = true, message = "Lesson selected successfully" });
            }
            return BadRequest(new { success = false, message = result.Message });
        }

        [HttpPost("{roomId}/ready")]
        [Authorize]
        public async Task<IActionResult> ReadyPlayer(string roomId, [FromBody] ReadyRequest body)
        {
            if (!Guid.TryParse(roomId, out var guid))
            {
                return BadRequest(new { success = false, message = "Invalid room ID format" });
            }
            
            var result = await _mediator.Send(new ReadyInMemoryCommand { RoomId = guid, IsReady = body.IsReady });
            
            if (result.Success)
            {
                // Broadcast to all clients in the room that a player is ready
                await _hubContext.Clients.Group(roomId).SendAsync("PlayerReady", roomId);
                return Ok(new { success = true, message = "Player ready state updated" });
            }
            
            return BadRequest(new { success = false, message = result.Message });
        }

        [HttpPost("{roomId}/kick")]
        [Authorize]
        public async Task<IActionResult> KickPlayer(string roomId, [FromBody] KickPlayerRequest body)
        {
            if (!Guid.TryParse(roomId, out var guid))
            {
                return BadRequest(new { success = false, message = "Invalid room ID format" });
            }

            if (!Guid.TryParse(body.TargetUserId, out var targetUserId))
            {
                return BadRequest(new { success = false, message = "Invalid target user ID format" });
            }
            
            var result = await _mediator.Send(new KickPlayerInMemoryCommand { RoomId = guid, TargetUserId = targetUserId });
            
            if (result.Success)
            {
                // Broadcast to all clients in the room that a player has been kicked
                await _hubContext.Clients.Group(roomId).SendAsync("PlayerKicked", roomId, body.TargetUserId);
                return Ok(new { success = true, message = "Player kicked successfully" });
            }
            
            return BadRequest(new { success = false, message = result.Message });
        }

        public class SelectLessonInMemoryRequest
        {
            public Guid LessonId { get; set; }
        }

        public class ReadyRequest
        {
            public bool IsReady { get; set; }
        }

        public class KickPlayerRequest
        {
            public string TargetUserId { get; set; } = string.Empty;
        }
    }

    public class CreateRoomRequest
    {
        public string RoomName { get; set; } = string.Empty;
        public int MaxPlayers { get; set; } = 4;
        public string CategoryId { get; set; } = string.Empty;
    }

    public class JoinRoomRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}