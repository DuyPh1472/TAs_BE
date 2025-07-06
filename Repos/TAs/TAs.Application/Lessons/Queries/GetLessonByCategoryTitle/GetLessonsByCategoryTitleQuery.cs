using MediatR;
using TAs.Application.Lessons.DTOs;
using TAs.Domain.Result;

namespace TAs.Application.Lessons.Queries.GetLessonByCategoryTitle
{
    public class GetLessonsByCategoryTitleQuery(string title)
     : IRequest<Result<List<GetAllLessonDTO>>>
    {
        public string Title = title;    
    }
}