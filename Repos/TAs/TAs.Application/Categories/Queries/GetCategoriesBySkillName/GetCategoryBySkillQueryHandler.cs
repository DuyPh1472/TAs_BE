using MediatR;
using TAs.Application.Categories.DTOs.Retrieval;
using TAs.Application.Categories.HandleErrors;
using TAs.Application.Interfaces;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Queries.GetCategoriesBySkillName
{
    public class GetCategoryBySkillQueryHandler(IUnitOfWork unitOfWork)
     : IRequestHandler<GetCategoryBySkillQuery, Result<List<GetAllCategoriesDTO>>>
    {
        public async Task<Result<List<GetAllCategoriesDTO>>>
        Handle(GetCategoryBySkillQuery request, CancellationToken cancellationToken)
        {
            var categories = await unitOfWork
                               .CategoryRepository
                               .GetCategoriesBySkillName(request.SkillName);
            var skill = await unitOfWork
            .SkillRepository
            .GetSkillByNameAsync(request.SkillName);                  
            if (request.SkillName != skill?.Name)
                return Result<List<GetAllCategoriesDTO>>
                .Failure(CategoryErrors
                .NoSkillFound(request.SkillName));
            return Result<List<GetAllCategoriesDTO>>
                 .Success(categories.Select(GetAllCategoriesDTO.FromEntity).ToList(  ));
        }
    }
}