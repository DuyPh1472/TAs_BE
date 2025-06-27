using MediatR;
using Microsoft.Extensions.Logging;
using TAs.Application.Skills.DTOs;
using TAs.Domain.Repositories;

namespace TAs.Application.Skills.Queries.GetAllSkills
{
    public class GetAllSkillsQueryHandler : IRequestHandler<GetAllSkillsQuery, IEnumerable<SkillDTO>>
    {
        private readonly ISkillRepository skillRepository;
        private readonly ILogger<GetAllSkillsQueryHandler> logger;

        public GetAllSkillsQueryHandler(ISkillRepository skillRepository, ILogger<GetAllSkillsQueryHandler> logger)
        {
            this.skillRepository = skillRepository;
            this.logger = logger;
        }
           public async Task<IEnumerable<SkillDTO>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all Skills.");
            var queries = await skillRepository.GetAllSkillsAsync();
            return queries.Select(SkillDTO.FromEntity);
        }
    }
}