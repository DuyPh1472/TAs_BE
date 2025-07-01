using MediatR;
using TAs.Application.ProgressApplication.DTOs.Queries.GetAll;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Queries.GetAllProgress
{
    public class GetAllProgressQuery : IRequest<Result<List<GetAllProgressDTO>>>;

}