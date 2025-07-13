using MediatR;
using TAs.Application.Interfaces;

namespace TAs.Application.Categories.Queries.CheckCategoryExists
{
    public class CheckCategoryExistsQueryHandler : IRequestHandler<CheckCategoryExistsQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CheckCategoryExistsQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(CheckCategoryExistsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.CategoryRepository.ExistsAsync(request.CategoryId);
        }
    }
} 