namespace TAs.APi.Controllers
{
    public class UpdateRoomSettingsRequest
    {
        public int TimeLimit { get; set; }
        public int MaxRetries { get; set; }
        public bool ShowRealTimeScore { get; set; }
 
    }
} 