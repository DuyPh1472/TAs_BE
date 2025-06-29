using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAs.APi.Response;
using TAs.Application.Categories.Commands;
using TAs.Application.Categories.DTOs;
using TAs.Application.Categories.Queries;
using TAs.Domain.Constants;

namespace TAs.APi.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoryController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] CreateCategoryCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Error.Description);
            return Created();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetAllCategoriesDTO>>>> GetAll()
        {
            var categories = await mediator.Send(new GetAllCategoriesQuery());
            return Ok(new ApiResponse<IEnumerable<GetAllCategoriesDTO>>
              (true, categories, 200, "Lấy danh sách thể loại thành công."));

        }
    }
}