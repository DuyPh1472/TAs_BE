using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MediatR;
using TAs.APi.Multiplayer;
using Microsoft.AspNetCore.Authorization;
using TAs.Application.GameRooms.Queries;
using TAs.Application.GameRooms.Commands;

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
            if (!result)
            {
                return BadRequest(new { success = false, message = "Join room failed" });
            }
            return Ok(new { success = true, message = "Join request sent successfully" });
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