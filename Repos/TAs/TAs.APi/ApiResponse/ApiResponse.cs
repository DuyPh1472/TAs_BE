namespace TAs.APi.ApiResponse
{
    public class ApiResponse<T>
    {
        public bool Success { get; }
        public int Status { get; }
        public T? Data { get; }
        public string? Message { get; }
        public ApiResponse
                (bool success,
                T? data = default,
                int status = 0,
                string? message = null)
        {
            Success = success;
            Status = status;
            Data = data;
            Message = message;
        }
    }
}