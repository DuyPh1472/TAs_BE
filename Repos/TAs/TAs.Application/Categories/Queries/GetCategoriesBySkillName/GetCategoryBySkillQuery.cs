using MediatR;
using TAs.Application.Categories.DTOs.Retrieval;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Queries.GetCategoriesBySkillName
{
    public class GetCategoryBySkillQuery(string skillName)
     : IRequest<Result<List<GetAllCategoriesDTO>>>
    {
        public string SkillName = skillName;
    }
}