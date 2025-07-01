using TAs.Domain.Errors;

namespace TAs.Application.ProgressApplication.HandleProgressErrors
{
    public class ProgressError
    {
        public static Error ProgressNotFound(Guid progressId, int sentenceIndex)
        => new("ProgressNotFound", $"Progress with Id: {progressId} or SentenceIndex with index: {sentenceIndex} does not exist.");
        public static Error UserNotFound(Guid userId)
        => new("UserNotFound", $"Progress with User's Id: {userId} does not exist.");
        public static Error ProgressIdNotFound(Guid progressId)
        => new("ProgressIdNotFound", $"Progress with Id: {progressId} does not exist.");
    }
}