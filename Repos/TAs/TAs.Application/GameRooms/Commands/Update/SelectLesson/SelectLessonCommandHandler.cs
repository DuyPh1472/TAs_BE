using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.Users;
using TAs.Application.Users.HandlerErrors;
using TAs.Application.GameRooms.HandlerExceptionGameRooms;
using TAs.Domain.Result;

namespace TAs.Application.GameRooms.Commands.Update.SelectLesson
{
    public class SelectLessonCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
        : IRequestHandler<SelectLessonCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(SelectLessonCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            if (currentUser is null)
                return Result<bool>.Failure(IdentityErrors.UserNotFound);
            var room = await unitOfWork.GameRoomRepository.GetGameRoomByRoomId(request.RoomId);
            if (room is null)
                return Result<bool>.Failure(GameRoomErrors.NoRoomFound(request.RoomId));
            if (room.HostId != currentUser.Id)
                return Result<bool>.Failure(GameRoomErrors.OnlyHostCanStart);
            var lesson = await unitOfWork.LessonRepository.GetLessonsById(request.LessonId);
            if (lesson == null)
                return Result<bool>.Failure(GameRoomErrors.LessonNotFound);
            room.SelectedLessonId = lesson.Id;
            await unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}