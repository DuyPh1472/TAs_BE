using MediatR;
using Microsoft.Extensions.Logging;
using TAs.Application.Skills.DTOs;
using TAs.Application.Skills.HandleError;
using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Domain.Result;

namespace TAs.Application.Skills.Queries.GetSkillById
{
    public class GetSkillByIdQueryHandler(ISkillRepository skillRepository,
    ILogger<GetSkillByIdQueryHandler> logger)
     : IRequestHandler<GetSkillByIdQuery, Result<SkillDTO>>
    {
        public async Task<Result<SkillDTO>> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Get Skill by Id");
              Skill? skill = await skillRepository.GetSkillByIdAsync(request.Id);
            if (skill is null) return Result<SkillDTO>.Failure(SkillErrors.IdNotFound(request.Id));
            return Result<SkillDTO>.Success(SkillDTO.FromEntity(skill));

        }
    }
}