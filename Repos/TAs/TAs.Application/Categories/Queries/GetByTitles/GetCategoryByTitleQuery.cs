using MediatR;
using TAs.Application.Categories.DTOs.Retrieval;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Queries.GetByTitles
{
    public class GetCategoryByTitleQuery(string Title)
     : IRequest<Result<GetCategoryByTitleDTO>>
    {
        public string title = Title;
    }
}