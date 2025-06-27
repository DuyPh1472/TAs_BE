using MediatR;
using TAs.Application.Skills.DTOs;
using TAs.Domain.Result;

namespace TAs.Application.Skills.Queries.GetSkillById
{
    public class GetSkillByIdQuery(Guid id) : IRequest<Result<SkillDTO>>
    {
        public Guid Id = id;
    }
}