using MediatR;
using TAs.Application.Interfaces;
using TAs.Application.ProgressApplication.DTOs.Queries.GetAll;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Queries.GetAllProgress
{
    public class GetAllProgressQueryHandler(IUnitOfWork unitOfWork)
     : IRequestHandler<GetAllProgressQuery, Result<List<GetAllProgressDTO>>>
    {
        public async Task<Result<List<GetAllProgressDTO>>>
         Handle(GetAllProgressQuery request, CancellationToken cancellationToken)
        {
            var queries = await unitOfWork
            .ProgressRepository
            .GetAllProgressAsync();
            var result = queries.Select(p => new GetAllProgressDTO
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
                LastUpdatedAt = p.UpdatedAt
            }).ToList();

            return Result<List<GetAllProgressDTO>>.Success(result);
        }
    }
}