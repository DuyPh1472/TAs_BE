using TAs.Domain.Errors;

namespace Namespace
{
    public static class ProgressDetailError
    {
        public static Error ProgressDetailNotFound(Guid progressId)
        => new("ProgressDetailNotFound", $"Progress detail with progress Id: {progressId} does not exist.");
    }
}