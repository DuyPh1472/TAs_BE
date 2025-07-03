using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.ProgressApplication.DTOs.Queries.GetProgressByUser;
using TAs.Application.ProgressApplication.HandleProgressErrors;
using TAs.Application.Users;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Queries.GetProgressByUser
{
    public class GetProgressByUserQueryHandler(IUnitOfWork unitOfWork,
    IUserContext userContext)
    : IRequestHandler<GetProgressByUserQuery, Result<List<UserProgressDTO>>>
    {
        public async Task<Result<List<UserProgressDTO>>>
        Handle(GetProgressByUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            var progress = await unitOfWork.ProgressRepository.GetProgressesByUser(request.UserId);
            if (progress == null)
                return Result<List<UserProgressDTO>>.Failure(ProgressError.UserNotFound(request.UserId));
            var result = progress.Select(p => new UserProgressDTO
            {
                ProgressId = p.Id,
                LessonId = p.LessonId,
                UserId = p.UserId,
                CurrentSentence = p.CurrentSentence,
                CompletedSentences = p.ProgressChallenge,
                TotalSentences = p.TotalChallenge,
                Score = p.Score,
                Status = p.ProgressStatus ? "completed" : "in_progress",
                StartedAt = p.StartedAt,
                CompletedAt = p.CompletedAt,
                LastUpdatedAt = p.UpdatedAt,
                LessonTitle = p.Lesson?.Title ?? string.Empty,
                TotalChallenge = p.TotalChallenge,
                ProgressChallenge = p.ProgressChallenge,
                ProgressStatus = p.ProgressStatus,
                CreatedBy = currentUser!.Id
            }).ToList();
            return Result<List<UserProgressDTO>>.Success(result);
        }
    }
}