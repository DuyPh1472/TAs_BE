namespace TAs.Application.GameRooms
{
    public class PlayerState
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public bool IsHost { get; set; } = false;
        public bool IsReady { get; set; } = false;
        public int Score { get; set; } = 0;
    }
}