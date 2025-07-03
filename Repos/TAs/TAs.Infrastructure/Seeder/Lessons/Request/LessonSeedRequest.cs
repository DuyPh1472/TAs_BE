using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TAs.Infrastructure.Seeder.Lessons.Request
{
    public class LessonSeedRequest
    {
        [JsonPropertyName("lessonId")]
        public Guid LessonId { get; set; }

        [JsonPropertyName("lessonName")]
        public string LessonName { get; set; } = string.Empty;

        [JsonPropertyName("audioSrc")]
        public string AudioSrc { get; set; } = string.Empty;

        [JsonPropertyName("vocabLevel")]
        public string VocabLevel { get; set; } = string.Empty;

        [JsonPropertyName("speechToTextLangCode")]
        public string SpeechToTextLangCode { get; set; } = string.Empty;

        [JsonPropertyName("challenges")]
        public List<ChallengeDto> Challenges { get; set; } = new List<ChallengeDto>();

        [JsonPropertyName("youtubeUrl")]
        public string YoutubeUrl { get; set; } = string.Empty;

        [JsonPropertyName("youtubeEmbedUrl")]
        public string YoutubeEmbedUrl { get; set; } = string.Empty;

        [JsonPropertyName("videoId")]
        public string VideoId { get; set; } = string.Empty;

        [JsonPropertyName("videoTitle")]
        public string VideoTitle { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("accent")]
        public string Accent { get; set; } = string.Empty;

        [JsonPropertyName("topics")]
        public string Topics { get; set; } = string.Empty;

        [JsonPropertyName("categoryId")]
        public string CategoryId { get; set; } = string.Empty;
    }

    public class ChallengeDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("defaultInput")]
        public string DefaultInput { get; set; } = string.Empty;

        [JsonPropertyName("jsonContent")]
        public List<JsonElement> JsonContent { get; set; } = new List<JsonElement>();

        [JsonPropertyName("solution")]
        public List<List<string>> Solution { get; set; } = new List<List<string>>();

        [JsonPropertyName("audioSrc")]
        public string AudioSrc { get; set; } = string.Empty;

        [JsonPropertyName("timeStart")]
        public float TimeStart { get; set; }

        [JsonPropertyName("timeEnd")]
        public float TimeEnd { get; set; }

        [JsonPropertyName("hint")]
        public string Hint { get; set; } = string.Empty;

        [JsonPropertyName("hints")]
        public List<string> Hints { get; set; } = new List<string>();

        [JsonPropertyName("explanation")]
        public string Explanation { get; set; } = string.Empty;

        [JsonPropertyName("alwaysShowExplanation")]
        public bool AlwaysShowExplanation { get; set; }

        [JsonPropertyName("nbComments")]
        public int NbComments { get; set; }
    }
}
