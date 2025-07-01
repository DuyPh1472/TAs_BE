using MediatR;
using TAs.Application.ProgressApplication.DTOs.Queries.GetProgressByUser;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Queries.GetProgressByUser
{
    public class GetProgressByUserQuery(Guid userId)
     : IRequest<Result<List<UserProgressDTO>>>
    {
        public Guid UserId = userId;
    }
}