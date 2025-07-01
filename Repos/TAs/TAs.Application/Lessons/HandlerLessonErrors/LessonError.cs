using TAs.Domain.Errors;

namespace TAs.Application.Lessons.HandlerLessonErrors
{
    public static class LessonError
    {
        public static  Error IdNotFound(Guid lessonId)
        => new("IdNotFound", $"Lesson with Id: {lessonId} does not exist.");
    }
}