using TAs.Infrastructure.Seeder.Lessons.Request;

namespace TAs.Infrastructure.Seeder.Lessons.Services
{
    public interface ILessonSeedService
    {
        Task<(bool Success, string Message)> SeedLessonFromJsonAsync(LessonSeedRequest json);
    }
}