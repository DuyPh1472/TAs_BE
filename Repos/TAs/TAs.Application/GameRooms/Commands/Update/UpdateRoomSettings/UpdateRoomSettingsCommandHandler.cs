using MediatR;
using TAs.Application.GameRooms.HandlerExceptionGameRooms;
using TAs.Application.Interfaces;
using TAs.Application.Users;
using TAs.Application.Users.HandlerErrors;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.Update.UpdateRoomSettings
{
    public class UpdateRoomSettingsCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext) 
        : IRequestHandler<UpdateRoomSettingsCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(UpdateRoomSettingsCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            if (currentUser is null)
            {
                return Result<bool>.Failure(IdentityErrors.UserNotFound);
            }

            var room = await unitOfWork.GameRoomRepository.GetByIdAsync(request.RoomId);
            if (room is null)
            {
                return Result<bool>.Failure(GameRoomErrors.RoomNotFound(request.RoomId));
            }

            // Chỉ host mới được cập nhật settings
            if (room.HostId != currentUser.Id)
            {
                return Result<bool>.Failure(GameRoomErrors.NotRoomHost());
            }

            // Cập nhật settings vào GameSession nếu đang có session active
            var activeSession = await unitOfWork.GameSessionRepository.GetActiveSessionByRoomIdAsync(request.RoomId);
            if (activeSession != null)
            {
                // Nếu có session active, cập nhật trực tiếp vào session
                activeSession.TimeLimit = request.TimeLimit;
                activeSession.MaxRetries = request.MaxRetries;
                activeSession.ShowRealTimeScore = request.ShowRealTimeScore;
                activeSession.UpdatedAt = DateTimeOffset.UtcNow;
                activeSession.UpdatedBy = currentUser.Id;
            }
            else
            {
                // Nếu chưa có session, lưu settings vào room để sử dụng khi tạo session
                var settings = new
                {
                    TimeLimit = request.TimeLimit,
                    MaxRetries = request.MaxRetries,
                    ShowRealTimeScore = request.ShowRealTimeScore
                };
                room.Settings = System.Text.Json.JsonSerializer.Serialize(settings);
                room.UpdatedAt = DateTimeOffset.UtcNow;
                room.UpdatedBy = currentUser.Id;
            }

            await unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
} 