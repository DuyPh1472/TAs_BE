using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAs.APi.Response;
using TAs.Application.Categories.Commands;
using TAs.Application.Categories.DTOs;
using TAs.Application.Categories.Queries;
using TAs.Application.Categories.Queries.GetById;
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
              (true, categories, 200, "Categories retrieved successfully."));

        }
        [HttpGet]
        [AllowAnonymous]
        [Route("{categoryId:guid}")]
        public async Task<ActionResult<ApiResponse<GetCategoryIdDTO>>> GetById(Guid categoryId)
        {
            var category = await mediator
                                  .Send(new GetCategoryByIdQuery(categoryId));
            if (!category.IsSuccess)
            {
                var response = new
                ApiResponse<GetCategoryIdDTO>(false, null, 404, category.Error.Description);
                if (category.Error.Code == "NoCategoryFound")
                    return NotFound(response);
            }
            return Ok(
                new ApiResponse<GetCategoryIdDTO>(
                    true, category.Data, 200, "Category retrieved successfully by ID."));
        }
    }
}