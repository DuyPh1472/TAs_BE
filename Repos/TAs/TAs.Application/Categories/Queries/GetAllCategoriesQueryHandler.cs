using MediatR;
using TAs.Application.Categories.DTOs;
using TAs.Application.Interfaces;

namespace TAs.Application.Categories.Queries
{
    public class GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllCategoriesQuery, IEnumerable<GetAllCategoriesDTO>>
    {
        public async Task<IEnumerable<GetAllCategoriesDTO>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var queries = await unitOfWork.CategoryRepository.GetAllAsync();
            return queries.Select(GetAllCategoriesDTO.FromEntity);
        }
    }
}