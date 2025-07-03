using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAs.APi.Response;
using TAs.Application.ProgressApplication.Commands;
using TAs.Application.ProgressApplication.Commands.Create;
using TAs.Application.ProgressApplication.Commands.Update;
using TAs.Application.ProgressApplication.DTOs;
using TAs.Application.ProgressApplication.DTOs.Queries.GetByIdAndSentence;
using TAs.Application.ProgressApplication.DTOs.Queries.GetProgressByUser;
using TAs.Application.ProgressApplication.Queries;
using TAs.Application.ProgressApplication.Queries.GetByIdAndSentenceIndex;
using TAs.Application.ProgressApplication.Queries.GetProgressByUser;
using TAs.Application.ProgressApplication.Queries.GetProgressDetail;

namespace TAs.APi.Controllers
{
    [ApiController]
    [Route("api/progress")]
    [Authorize]
    public class ProgressController(IMediator mediator) : ControllerBase
    {
        [HttpPost("update")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateProgress([FromBody] UpdateProgressCommand request)
        {
            var command = await mediator.Send(request);
            if (!command.IsSuccess)
            {
                return NotFound(new ApiResponse<object>
                (false, null, 404, command.Error.Description));
            }
            return Created();
        }
        // [HttpGet("get-all")]
        // [AllowAnonymous]
        // public async Task<ActionResult<ApiResponse<List<GetAllProgressDTO>>>> GetAll()
        // {
        //     var result =
        //      await mediator.Send(new GetAllProgressQuery());
        //     return Ok(new ApiResponse<List<GetAllProgressDTO>>
        //     (true, result.Data, 200, "Progress retrieved successfully"));
        // }
        [HttpGet]
        [Route("{progressId:guid}/sentence/{sentenceIndex:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<GetProgressByIdAndSentenceIndexDTO>>> GetByIdAndSentence
        (Guid progressId, int sentenceIndex)
        {
            var result =
             await mediator.Send(new GetProgressByIdAndSentenceQuery(progressId, sentenceIndex));
            if (!result.IsSuccess)
                return NotFound(new ApiResponse<GetProgressByIdAndSentenceIndexDTO>
                (false, null, 404, result.Error.Description));
            return Ok(new ApiResponse<GetProgressByIdAndSentenceIndexDTO>
                (true, result.Data, 200, "Progress retrieved successfully"));
        }
        [HttpGet]
        [Route("{userId:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<UserProgressDTO>>>> GetProgressesByUSer
        (Guid userId)
        {
            var result =
             await mediator.Send(new GetProgressByUserQuery(userId));
            if (!result.IsSuccess)
                return NotFound(new ApiResponse<UserProgressDTO>
                (false, null, 404, result.Error.Description));
            return Ok(new ApiResponse<List<UserProgressDTO>>
                (true, result.Data, 200, "Progress retrieved successfully"));
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Guid>>> CreateProgress([FromBody] CreateProgressCommand request)
        {
            var result = await mediator.Send(request);
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<Guid>(false, Guid.Empty, 400, result.Error.Description));
            return Ok(new ApiResponse<Guid>(true, result.Data, 200, "Progress created successfully"));
        }
        [HttpGet("{progressId:guid}/sentences")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<ProgressDetailDTO>>>> GetProgressDetails(Guid progressId)
        {
            var result = await mediator.Send(new GetProgressDetailsQuery(progressId));
            if (!result.IsSuccess)
                return NotFound(new ApiResponse<List<ProgressDetailDTO>>(false, null, 404, result.Error.Description));
            return Ok(new ApiResponse<List<ProgressDetailDTO>>(true, result.Data, 200, "Progress details retrieved successfully"));
        }
        [HttpGet("{progressId:guid}/completed-count")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<CompletedCountResponse>>> GetCompletedCount(Guid progressId)
        {
            var result = await mediator.Send(new GetCompletedCountQuery(progressId));
            if (!result.IsSuccess)
                return NotFound(new ApiResponse<CompletedCountResponse>(false, null, 404, result.Error.Description));
            return Ok(new ApiResponse<CompletedCountResponse>(true, result.Data, 200, "Completed count retrieved successfully"));
        }
        [HttpDelete("{progressId:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteProgress(Guid progressId)
        {
            var result = await mediator.Send(new DeleteProgressCommand(progressId));
            if (!result.IsSuccess)
                return NotFound(new ApiResponse<object>(false, null, 404, result.Error.Description));
            return Ok(new ApiResponse<object>(true, null, 200, "Progress deleted successfully"));
        }
    }
}