using MediatR;
using TAs.Application.Skills.DTOs;
namespace TAs.Application.Skills.Queries.GetAllSkills
{
    public class GetAllSkillsQuery : IRequest<IEnumerable<SkillDTO>>;
}