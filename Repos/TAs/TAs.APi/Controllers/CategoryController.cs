using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAs.APi.Response;
using TAs.Application.Categories.Commands.Create;
using TAs.Application.Categories.Commands.Update;
using TAs.Application.Categories.DTOs.Requests;
using TAs.Application.Categories.DTOs.Retrieval;
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
        public async Task<ActionResult<ApiResponse<GetAllCategoriesDTO>>> Create([FromBody] CreateCategoryCommand command)
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
        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("{categoryId:guid}")]
        public async Task<ActionResult<ApiResponse<GetAllCategoriesDTO>>> Update(Guid categoryId, CategoryRequest request)
        {
            var command = await mediator.Send(new UpdateCategoryCommand(categoryId, request));
            if (!command.IsSuccess)
            {
                var response = new
                ApiResponse<GetAllCategoriesDTO>(false, null, 404, command.Error.Description);
                if (command.Error.Code == "NoCategoryFound")
                    return NotFound(response);
            }
            return Ok(new ApiResponse<GetAllCategoriesDTO>(true, null, 204, string.Empty));

        }
    }
}