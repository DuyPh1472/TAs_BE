using MediatR;
using TAs.Application.Categories.DTOs.Retrieval;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Queries.GetById
{
    public class GetCategoryByIdQuery(Guid categoryId) : IRequest<Result<GetCategoryIdDTO>>
    {
        public Guid Id = categoryId;
    }

}