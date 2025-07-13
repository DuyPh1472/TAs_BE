using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TAs.Application.GameRooms;
using MediatR;
using TAs.Application.Categories.Queries.CheckCategoryExists;
using TAs.APi.Multiplayer;

namespace TAs.APi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameRoomInMemoryController(InMemoryGameRoomService roomService, IMediator mediator, IHubContext<GameRoomHub> hubContext) : ControllerBase
    {
        private readonly InMemoryGameRoomService _roomService = roomService;
        private readonly IMediator _mediator = mediator;
        private readonly IHubContext<GameRoomHub> _hubContext = hubContext;

        [HttpGet("active")]
        public IActionResult GetActiveRooms()
        {
            try
            {
                var rooms = _roomService.GetActiveRoomsDTO();
                return Ok(new
                {
                    success = true,
                    data = rooms,
                    message = "Active rooms retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("{roomId}")]
        public IActionResult GetRoomDetails(string roomId)
        {
            try
            {
                if (!Guid.TryParse(roomId, out var guid))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid room ID format"
                    });
                }

                var roomDetails = _roomService.GetRoomDetailsDTO(guid);
                if (roomDetails == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Room not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = roomDetails,
                    message = "Room details retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request)
        {
            try
            {
                if (!Guid.TryParse(request.CategoryId, out var categoryGuid))
                {
                    return BadRequest(new { success = false, message = "CategoryId must be a valid Guid" });
                }

                var categoryExists = await _mediator.Send(new CheckCategoryExistsQuery(categoryGuid));
                if (!categoryExists)
                {
                    return BadRequest(new { success = false, message = "CategoryId not found" });
                }

                // Tạo room mới trong in-memory storage
                var roomId = Guid.NewGuid();
                var room = _roomService.CreateRoom(roomId, request.RoomName, request.MaxPlayers, categoryGuid);
                
                // Broadcast room created event to all clients
                Console.WriteLine($"[GameRoomInMemoryController] Broadcasting RoomCreated event for room: {roomId}");
                await _hubContext.Clients.All.SendAsync("RoomCreated", room);
                Console.WriteLine($"[GameRoomInMemoryController] RoomCreated event broadcasted successfully");
                
                return Ok(new
                {
                    success = true,
                    data = roomId.ToString(),
                    message = "Room created successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost("{roomId}/join")]
        public IActionResult JoinRoom(string roomId, [FromBody] JoinRoomRequest request)
        {
            try
            {
                if (!Guid.TryParse(roomId, out var guid))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid room ID format"
                    });
                }

                var room = _roomService.GetRoom(guid);
                if (room == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Room not found"
                    });
                }

                // Join logic sẽ được xử lý qua SignalR
                return Ok(new
                {
                    success = true,
                    message = "Join request sent successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
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