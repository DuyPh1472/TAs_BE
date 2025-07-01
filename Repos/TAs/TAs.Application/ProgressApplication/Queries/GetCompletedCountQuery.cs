using MediatR;
using TAs.Application.ProgressApplication.DTOs;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Queries
{
    public class GetCompletedCountQuery : IRequest<Result<CompletedCountResponse>>
    {
        public Guid ProgressId { get; set; }
        public GetCompletedCountQuery(Guid progressId)
        {
            ProgressId = progressId;
        }
    }
} 