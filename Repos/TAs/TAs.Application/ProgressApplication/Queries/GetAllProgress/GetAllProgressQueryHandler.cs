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
                ProgressId = p.ProgressId,
                CompletedAt = p.ProgressDetails
                       .Where(pd => pd.CompletedAt.HasValue)
                       .Select(pd => pd.CompletedAt!.Value)
                       .ToList(),
                IsCompleted = p.ProgressDetails
                       .Select(pd => pd.IsCompleted)
                       .ToList(),
                SentenceIndex = p.ProgressDetails
                       .Select(pd => pd.SentenceIndex)
                       .ToList()
            }).ToList();

            return Result<List<GetAllProgressDTO>>.Success(result);
        }
    }
}