using MediatR;
using TAs.Application.ProgressApplication.DTOs;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Queries.GetProgressDetail
{
    public class GetProgressDetailsQuery : IRequest<Result<List<ProgressDetailDTO>>>
    {
        public Guid ProgressId { get; set; }
        public GetProgressDetailsQuery(Guid progressId)
        {
            ProgressId = progressId;
        }
    }
} 