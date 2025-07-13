namespace TAs.Application.GameRooms.DTOs.Commands
{
    public class ChatMessageDTO
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserAvatar { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
    }
} 