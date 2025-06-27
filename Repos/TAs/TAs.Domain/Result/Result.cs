using TAs.Domain.Errors; 
namespace TAs.Domain.Result
{
    public class Result
    {
        private Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None
            || !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }
            IsSuccess = isSuccess;
            Error = error;    
        }
        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);
    }
    public class Result<T>
    {
        private Result(bool isSuccess, T? data, Error error)
        {
            if (isSuccess && error != Error.None
            || !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }
            Data = data;
            IsSuccess = isSuccess;
            Error = error;
        }

        public T? Data { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        public static Result<T> Success(T data) => new(true, data, Error.None);
        public static Result<T> Failure(Error error) => new(false, default, error);
    }
}