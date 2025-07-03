using MediatR;
using TAs.Application.Lessons.DTOs;
using TAs.Domain.Result;

namespace TAs.Application.Lessons.Queries.GetAll
{
    public class GetAllLessonsQuery : IRequest<Result<List<GetAllLessonDTO>>>
    {
        
    }
}