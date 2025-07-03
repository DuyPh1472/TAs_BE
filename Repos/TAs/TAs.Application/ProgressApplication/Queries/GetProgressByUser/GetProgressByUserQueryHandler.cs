using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.ProgressApplication.DTOs.Queries.GetProgressByUser;
using TAs.Application.ProgressApplication.HandleProgressErrors;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Queries.GetProgressByUser
{
    public class GetProgressByUserQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProgressByUserQuery, Result<List<UserProgressDTO>>>
    {
        public async Task<Result<List<UserProgressDTO>>>
        Handle(GetProgressByUserQuery request, CancellationToken cancellationToken)
        {
            var progress = await unitOfWork.ProgressRepository.GetProgressesByUser(request.UserId);
            if (progress == null)
                return Result<List<UserProgressDTO>>.Failure(ProgressError.UserNotFound(request.UserId));
            var response = progress.Select(p => new UserProgressDTO
            {
                CreatedAt = p.CreatedAt,
                LessonId = p.LessonId,
                LessonTitle = p.Lesson.Title,
                ProgressId = p.Id,
                ProgressChallenge = p.ProgressChallenge,
                Score = p.Score,
                TotalChallenge = p.TotalChallenge,
                ProgressStatus = p.ProgressStatus,
                UpdatedAt = p.UpdatedAt,
                CreatedBy = p.CreatedBy
            }).ToList();
            return Result<List<UserProgressDTO>>.Success(response);
        }
    }
}