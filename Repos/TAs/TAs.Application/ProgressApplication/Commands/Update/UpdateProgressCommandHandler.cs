using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.Skills.HandleError;
using TAs.Application.Users;
using TAs.Domain.Entities;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Commands.Update
{
    public class UpdateProgressCommandHandler(IUserContext userContext,
    IUnitOfWork unitOfWork)
     : IRequestHandler<UpdateProgressCommand, Result>
    {
        public async Task<Result> Handle(UpdateProgressCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            var progress = await unitOfWork
            .ProgressRepository
            .GetProgressByUserAndLesson(currentUser!.Id, request.LessonId);
            if (progress == null)
            {
                var lesson = await unitOfWork.LessonRepository.GetByIdAsync(request.LessonId);
                int TotalChallenge;
                if (lesson != null)
                {
                    TotalChallenge = lesson.Sentences.Split("|").Length;
                }
                else
                {
                    return Result.Failure(SkillErrors.IdNotFound(request.LessonId));
                }
                progress = new Progress
                {
                    LessonId = request.LessonId,
                    UserId = currentUser.Id,
                    ProgressChallenge = 0,
                    TotalChallenge = TotalChallenge,
                    ProgressStatus = false,
                    Score = 0,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUser.Id
                };
                unitOfWork.ProgressRepository.Add(progress);
                await unitOfWork.SaveChangesAsync();
            }
            
            progress.ProgressStatus = (progress.ProgressChallenge == progress.TotalChallenge);
            progress.LastUpdatedAt = DateTimeOffset.UtcNow;
            if (progress.ProgressStatus && progress.CompletedAt == null)
                progress.CompletedAt = DateTimeOffset.UtcNow;
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}