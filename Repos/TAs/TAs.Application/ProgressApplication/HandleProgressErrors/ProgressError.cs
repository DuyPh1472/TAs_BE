using TAs.Domain.Errors;

namespace TAs.Application.ProgressApplication.HandleProgressErrors
{
    public class ProgressError
    {
        public static Error ProgressNotFound(Guid progressId, int sentenceIndex)
        => new("ProgressNotFound", $"Progress with Id: {progressId} or SentenceIndex with index: {sentenceIndex} does not exist.");
    }
}