using MediatR;
using TAs.Application.Categories.DTOs;

namespace TAs.Application.Categories.Queries
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<GetAllCategoriesDTO>>
    {
        
    }
}