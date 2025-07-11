using TAs.Domain.Enums;

namespace TAs.Application.GameRooms.DTOs.Commands
{
    public class StartGameRoomDTO
    {
        public Guid RoomId { get; set; }
        public TAs.Domain.Enums.GameStatus Status { get; set; }
        public Lessons.DTOs.LessonDTO Lesson { get; set; } = new();
        public List<DTOs.Queries.PlayerInRoomDTO> Players { get; set; } = new();
        public DateTimeOffset StartedAt { get; set; }
    }
} 