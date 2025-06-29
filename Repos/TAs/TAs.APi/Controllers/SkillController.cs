using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAs.APi.Response;
using TAs.Application.Skills.Commands.CreateSkill;
using TAs.Application.Skills.Commands.DTOs;
using TAs.Application.Skills.Commands.RemoveSkill;
using TAs.Application.Skills.Commands.UpdateSkill;
using TAs.Application.Skills.DTOs;
using TAs.Application.Skills.Queries.GetAllSkills;
using TAs.Application.Skills.Queries.GetSkillById;
using TAs.Domain.Constants;
namespace TAs.APi.Controllers
{
    [ApiController]
    [Route("api/skills")]
    [Authorize]
    public class SkillController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<SkillDTO>>>> GetSkills()
        {
            var skills = await mediator.Send(new GetAllSkillsQuery());
            return Ok(new ApiResponse<IEnumerable<SkillDTO>>
            (true, skills, 200, "Skill list retrieved successfully."));
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("{id:guid}")]
        public async Task<ActionResult<ApiResponse<SkillDTO>>> GetSkillById([FromRoute] Guid id)
        {
            var result = await mediator.Send(new GetSkillByIdQuery(id));
            if (!result.IsSuccess)
            {
                var response =
                new ApiResponse<SkillDTO>(false, null, 404, result.Error.Description);
                if (result.Error.Code == "Error not found")
                    return NotFound(response);
            }
            var isSuccess =
             new ApiResponse<SkillDTO>(true, result.Data, 200, "Skill retrieved successfully by ID.");
            return Ok(isSuccess);
        }
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<ApiResponse<SkillDTO>>> Create([FromBody] CreateSkillCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                var response =
                new ApiResponse<SkillDTO>
                (false, null, 409, result.Error.Description);
                if (result.Error.Code == "Duplicate Error")
                    return Conflict(response);

                return BadRequest(response);
            }

            return Created(string.Empty, new ApiResponse<SkillDTO>(true, null, 201, "Skill created successfully."));
        }
        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("{id:guid}")]
        public async Task<ActionResult<ApiResponse<SkillDTO>>> Update([FromRoute] Guid id, CreateAndUpdateSkill request)
        {
            var result = await
            mediator.Send(new UpdateSkillCommand(id, request));
            if (!result.IsSuccess)
            {
                var response =
                new ApiResponse<SkillDTO>
                (false, null, 404, result.Error.Description);
                if (result.Error.Code == "Error not found")
                    return NotFound(response);
            }
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("{id:guid}")]
        public async Task<ActionResult<ApiResponse<SkillDTO>>> Remove([FromRoute] Guid id)
        {
            var command = await mediator.Send(new RemoveSkillCommand(id));
            if (!command.IsSuccess)
            {
                var response =
                new ApiResponse<SkillDTO>
                (false, null, 404, command.Error.Description);
                if (command.Error.Code == "Error not found")
                    return NotFound(response);
            }
            return NoContent();
        }
    }
}