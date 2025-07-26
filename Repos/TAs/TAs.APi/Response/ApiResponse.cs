namespace TAs.APi.Response
{
    public class ApiResponse<T>(bool success,
            T? data = default,
            int status = 0,
            string? message = null)
    {
        public bool Success { get; } = success;
        public int Status { get; } = status;
        public T? Data { get; } = data;
        public string? Message { get; } = message;
    }
}