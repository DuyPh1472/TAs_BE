using System.Security.Cryptography.X509Certificates;
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
            .GetProgressByUserAndLesson(request.LessonId, currentUser!.Id);
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
            var detail = await unitOfWork.ProGressDetailRepository
            .GetByProgressAndSentence(progress.ProgressId, request.SentenceIndex);
            if (detail == null)
            {
                detail = new ProgressDetail
                {
                    ProgressId = progress.ProgressId,
                    SentenceIndex = request.SentenceIndex,
                    IsCompleted = request.IsCompleted,
                    IsCorrect = request.IsCorrect,
                    UserAnswer = request.UserAnswer,
                    CompletedAt = DateTime.UtcNow
                };
                unitOfWork.ProGressDetailRepository.Add(detail);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                detail.IsCompleted = request.IsCompleted;
                detail.IsCorrect = request.IsCorrect;
                detail.UserAnswer = request.UserAnswer;
                detail.CompletedAt = DateTime.UtcNow;
                unitOfWork.ProGressDetailRepository.Update(detail);
            }
            // 4. Cập nhật lại ProgressChallenge và trạng thái hoàn thành
            progress.ProgressChallenge = await unitOfWork.ProGressDetailRepository
            .CountCompleted(progress.ProgressId);
            progress.ProgressStatus = (progress.ProgressChallenge == progress.TotalChallenge);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}