using TAs.Domain.Result;
namespace TAs.Domain.Errors

{
    public sealed record Error(string Code, string? Description = null)
    {
        public static readonly Error None = new(string.Empty);
        public static implicit operator Result<object>(Error error)
         => Result<object>.Failure(error);
    }

}