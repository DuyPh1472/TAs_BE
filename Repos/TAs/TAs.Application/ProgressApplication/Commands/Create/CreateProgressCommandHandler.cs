using MediatR;
using TAs.Domain.Result;
using TAs.Domain.Entities;
using TAs.Application.Interfaces;
using TAs.Application.Lessons.HandlerLessonErrors;
using TAs.Application.Users;

namespace TAs.Application.ProgressApplication.Commands.Create
{
    public class CreateProgressCommandHandler(IUnitOfWork _unitOfWork,
    IUserContext userContext)
     : IRequestHandler<CreateProgressCommand, Result<Guid>>
    {

        public async Task<Result<Guid>> Handle(CreateProgressCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            var lesson = await _unitOfWork.LessonRepository.GetLessonsById(request.LessonId);
            if (lesson == null)
                return Result<Guid>.Failure(LessonError.IdNotFound(request.LessonId));
            var totalChallenge = lesson.Sentences.Split('|', StringSplitOptions.RemoveEmptyEntries).Length;
            var progress = new Progress
            {
                UserId = currentUser!.Id,
                LessonId = request.LessonId,
                TotalChallenge = totalChallenge,
                ProgressChallenge = 0,
                Score = 0,
                ProgressStatus = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = lesson.CreatedBy,
                StartedAt = DateTime.UtcNow,
                CurrentSentence = 0,
                LastUpdatedAt = DateTime.UtcNow
            };
             _unitOfWork.ProgressRepository.Add(progress);
            await _unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(progress.Id);
        }
    }
}