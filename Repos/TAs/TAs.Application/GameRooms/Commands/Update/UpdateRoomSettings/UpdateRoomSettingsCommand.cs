using MediatR;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.Update.UpdateRoomSettings
{
    public class UpdateRoomSettingsCommand : IRequest<Result<bool>>
    {
        public Guid RoomId { get; set; }
        public int TimeLimit { get; set; }
        public int MaxRetries { get; set; }
        public bool ShowRealTimeScore { get; set; }
        public bool AllowHints { get; set; }
    }
}