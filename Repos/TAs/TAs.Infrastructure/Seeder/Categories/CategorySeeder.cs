using Microsoft.EntityFrameworkCore;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Seeder.Skills;

namespace TAs.Infrastructure.Seeder.Categories
{
    public class CategorySeeder(TAsDbContext dbContext) : ISeeder
    {
        public async Task Seed()
        {
            if (!await dbContext.Categories.AnyAsync())
            {
                var adminId = Guid.Parse("ba02df20-a2ca-4f10-be79-8f5fc5bca1da");

                var categories = new List<Category>
                {
                    new Category { Id = Guid.Parse("1b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e"), SkillId = Guid.Parse("11111111-1111-1111-1111-111111111111"), CategoryType = "IELTS", Title = "IELTS Listening", Description = "IELTS Listening practice", Difficult = "Medium", Accent = "American", Duration = 251.66f, CreatedBy =adminId},
                    new Category { Id = Guid.Parse("2b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e"), SkillId = Guid.Parse("22222222-2222-2222-2222-222222222222"), CategoryType = "IELTS", Title = "IELTS Reading", Description = "IELTS Reading practice", Difficult = "Medium", Accent = "British", Duration = 200.0f,CreatedBy =adminId }
                };
                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.SaveChangesAsync();
            }
        }
        private IEnumerable<Category> GetCategories()
        {
            var today = DateTime.UtcNow;
            var adminId = Guid.Parse("ba02df20-a2ca-4f10-be79-8f5fc5bca1da");
            return new List<Category>
        {
        new Category
        {
            Id = Guid.Parse("e3e6dffc-7754-457f-b23a-85d76c9faa91"),
            Title = "Short Stories",
            Description = "A collection of audio articles introducing culture, people, places, historical events and daily life in English-speaking countries, especially Canada and America.",
            Difficult = "Intermediate",
            Accent = "North American",
            Duration = 7.5f, // Average of 5-10 min
            CreatedAt = today,
            CreatedBy = adminId
        },
        new Category
        {
            Id = Guid.Parse("71a9568d-f5d9-4fc8-bac9-283e34b0aa7b"),
            Title = "Daily Conversations",
            Description = "Short and fun English conversations in common situations you may experience in daily life.",
            Difficult = "Beginner",
            Accent = "Mixed",
            Duration = 4.0f, // Average of 3-5 min
            CreatedAt = today,
            CreatedBy = adminId
        },
        new Category
        {
            Id = Guid.Parse("6fe3caca-8225-49a3-93d6-79e864789986"),
            Title = "TOEIC Listening",
            Description = "In this section, there are a lot of conversations and short talks in everyday life and at work. Let's practice and improve your English communication skills!",
            Difficult = "Intermediate",
            Accent = "American",
            Duration = 6.0f, // Average of 4-8 min
            CreatedAt = today,
            CreatedBy = adminId
        },
        new Category
        {
            Id = Guid.Parse("8a4b5c6d-9e0f-1a2b-3c4d-5e6f7a8b9c0d"),
            Title = "YouTube",
            Description = "Are you bored with English lessons for students? Let's learn real English from YouTube videos that native speakers watch and enjoy!",
            Difficult = "Advanced",
            Accent = "Mixed",
            Duration = 11.5f, // Average of 8-15 min
            CreatedAt = today,
            CreatedBy = adminId
        },
        new Category
        {
            Id = Guid.Parse("1b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e"),
            Title = "IELTS Listening",
            Description = "Listening to IELTS recordings will help you learn a lot of vocabulary and expressions about everyday conversations & academic talks. These recordings are mainly in British and Australian accents.",
            Difficult = "Intermediate",
            Accent = "British/Australian",
            Duration = 9.0f, // Average of 6-12 min
            CreatedAt = today,
            CreatedBy = adminId
        },
        new Category
        {
            Id = Guid.Parse("2c3d4e5f-6a7b-8c9d-0e1f-2a3b4c5d6e7f"),
            Title = "TOEFL Listening",
            Description = "TOEFL listening recordings are academic conversations & lectures, mainly in American English. These recordings will help you to get better preparation if you are planning to study in an English-speaking country, especially the US and Canada.",
            Difficult = "Advanced",
            Accent = "American",
            Duration = 15.0f, // Average of 10-20 min
            CreatedAt = today,
            CreatedBy = adminId
        },
        new Category
        {
            Id = Guid.Parse("3d4e5f6a-7b8c-9d0e-1f2a-3b4c5d6e7f8a"),
            Title = "Spelling Names",
            Description = "Let's learn and practice the English alphabet by spelling some common names and improve your ability to understand English numbers when they are spoken quickly.",
            Difficult = "Beginner",
            Accent = "American",
            Duration = 3.0f, // Average of 2-4 min
            CreatedAt = today,
            CreatedBy = adminId
        },
        new Category
        {
            Id = Guid.Parse("4e5f6a7b-8c9d-0e1f-2a3b-4c5d6e7f8a9b"),
            Title = "World News",
            Description = "Stay updated with current events while improving your listening skills through authentic news broadcasts and reports.",
            Difficult = "Advanced",
            Accent = "International",
            Duration = 8.5f, // Average of 5-12 min
            CreatedAt = today,
            CreatedBy = adminId
        }
    };
        }
    }
}