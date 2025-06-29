using MediatR;
using TAs.Application.Categories.DTOs.Requests;
using TAs.Domain.Result;

namespace TAs.Application.Categories.Commands.Create
{
    public class CreateCategoryCommand : IRequest<Result>
    {
        public CategoryRequest? categoryRequest { get; set; } 
    }
}