using TAs.Domain.Result;
using TAs.Application.ProgressApplication.DTOs;
using TAs.Application.Interfaces;
using MediatR;
using Namespace;

namespace TAs.Application.ProgressApplication.Queries.GetProgressDetail
{
    public class GetProgressDetailsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProgressDetailsQuery, Result<List<ProgressDetailDTO>>>
    {
        public async Task<Result<List<ProgressDetailDTO>>> Handle(GetProgressDetailsQuery request, CancellationToken cancellationToken)
        {
            var details = await unitOfWork.ProGressDetailRepository.GetByProgressIdAsync(request.ProgressId);
            if (details == null)
                return Result<List<ProgressDetailDTO>>
                .Failure(ProgressDetailError
                .ProgressDetailNotFound(request.ProgressId));
            var dto = details.Select(d => new ProgressDetailDTO
            {
                SentenceIndex = d.SentenceIndex,
                IsCompleted = d.IsCompleted,
                CompletedAt = d.CompletedAt
            }).ToList();
            return Result<List<ProgressDetailDTO>>.Success(dto);
        }
    }
}