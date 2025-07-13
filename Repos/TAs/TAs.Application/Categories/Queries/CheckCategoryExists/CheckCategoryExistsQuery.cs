using MediatR;
using System;

namespace TAs.Application.Categories.Queries.CheckCategoryExists
{
    public class CheckCategoryExistsQuery : IRequest<bool>
    {
        public Guid CategoryId { get; set; }
        public CheckCategoryExistsQuery(Guid categoryId) => CategoryId = categoryId;
    }
} 