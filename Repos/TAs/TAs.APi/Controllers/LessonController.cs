using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAs.Application.Lessons.DTOs;
using TAs.Application.Lessons.Queries.GetById;
using TAs.APi.Response;
using TAs.Application.Lessons.Queries.GetAll;
using TAs.Infrastructure.Seeder.Lessons.Services;
using TAs.Infrastructure.Seeder.Lessons.Request;
using TAs.Application.Lessons.Queries.GetLessonByCategoryTitle;

namespace TAs.APi.Controllers
{
    [ApiController]
    [Route("api/lessons")]
    public class LessonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILessonSeedService _lessonSeedService;
        public LessonController(IMediator mediator, ILessonSeedService lessonSeedService)
        {
            _mediator = mediator;
            _lessonSeedService = lessonSeedService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<LessonDTO>>> GetLessonById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetLessonByIdQuery(id));
            if (!result.IsSuccess)
            {
                var response =
                 new ApiResponse<LessonDTO>(false, null, 404, result.Error.Description);
                return NotFound(response);
            }
            return Ok(new ApiResponse<LessonDTO>(true, result.Data, 200, "Lesson retrieved successfully by ID."));
        }

        [HttpGet("get-all")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<GetAllLessonDTO>>>> GetAllLessons()
        {
            var result = await _mediator.Send(new GetAllLessonsQuery());
            return Ok(new ApiResponse<List<GetAllLessonDTO>>
            (true, result.Data, 200, "All lessons retrieved successfully."));
        }

        [HttpPost("{categoryTitle}/seed-json")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>>
        SeedLessonFromJson([FromBody] LessonSeedRequest json, [FromRoute] string categoryTitle)
        {
            var (success, message) =
             await _lessonSeedService.SeedLessonFromJsonAsync(categoryTitle, json);
            if (success)
                return Ok(new ApiResponse<string>(true, null, 200, message));
            return BadRequest(new ApiResponse<string>(false, null, 400, message));
        }

        [HttpGet("category-title/{title}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<GetAllLessonDTO>>>> GetLessonsByCategoryTitle([FromRoute] string title)
        {
            var result = await _mediator.Send(new GetLessonsByCategoryTitleQuery(title));
            if (!result.IsSuccess)
            {
                var response = new ApiResponse<List<GetAllLessonDTO>>(false, null, 404, result.Error.Description);
                return NotFound(response);
            }
            return Ok(new ApiResponse<List<GetAllLessonDTO>>(true, result.Data, 200, $"Lessons retrieved successfully for category '{title}'."));
        }
    }
}