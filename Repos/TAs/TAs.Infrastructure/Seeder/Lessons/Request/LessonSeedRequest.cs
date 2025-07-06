using System.Text.Json; // Cần thiết cho JsonElement
using System.Text.Json.Serialization;

namespace TAs.Infrastructure.Seeder.Lessons.Request
{
    public class LessonSeedRequest
    {
        // Guid thường không nullable trong trường hợp này vì LessonId là định danh
        // Tuy nhiên, nếu bạn muốn nó có thể null từ JSON, bạn có thể thêm "?"
        [JsonPropertyName("lessonId")]
        public Guid LessonId { get; set; }

        [JsonPropertyName("lessonName")]
        public string? LessonName { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("audioSrc")]
        public string? AudioSrc { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("vocabLevel")]
        public string? VocabLevel { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("speechToTextLangCode")]
        public string? SpeechToTextLangCode { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("challenges")]
        // List<T> thường không cần nullable ở đây vì nó sẽ là một list rỗng nếu không có dữ liệu
        public List<ChallengeDto>? Challenges { get; set; } = new List<ChallengeDto>(); // List có thể null nếu JSON không có, nhưng khởi tạo rỗng an toàn hơn

        [JsonPropertyName("youtubeUrl")]
        public string? YoutubeUrl { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("youtubeEmbedUrl")]
        public string? YoutubeEmbedUrl { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("videoId")]
        public string? VideoId { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("videoTitle")]
        public string? VideoTitle { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("description")]
        public string? Description { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("accent")]
        public string? Accent { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("topics")]
        public string? Topics { get; set; } = string.Empty; // Đã là nullable và mặc định
    }

    public class ChallengeDto
    {
        // Guid thường không nullable trong trường hợp này, nhưng có thể thêm "?" nếu cần
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("defaultInput")]
        public string? DefaultInput { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("jsonContent")]
        public List<JsonElement>? JsonContent { get; set; } = new List<JsonElement>(); // List có thể null nếu JSON không có, nhưng khởi tạo rỗng an toàn hơn

        [JsonPropertyName("solution")]
        public List<List<string>>? Solution { get; set; } = new List<List<string>>(); // Đã là nullable và mặc định

        [JsonPropertyName("audioSrc")]
        public string? AudioSrc { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("timeStart")]
        public float? TimeStart { get; set; } // Đã là nullable

        [JsonPropertyName("timeEnd")]
        public float? TimeEnd { get; set; } // Đã là nullable

        [JsonPropertyName("hint")]
        public string? Hint { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("hints")]
        public List<string>? Hints { get; set; } = new List<string>(); // Đã là nullable và mặc định

        [JsonPropertyName("explanation")]
        public string? Explanation { get; set; } = string.Empty; // Đã là nullable và mặc định

        [JsonPropertyName("alwaysShowExplanation")]
   
        public bool AlwaysShowExplanation { get; set; }

        [JsonPropertyName("nbComments")]
        public int NbComments { get; set; }
    }
}